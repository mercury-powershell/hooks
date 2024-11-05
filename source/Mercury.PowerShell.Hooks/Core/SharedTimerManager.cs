// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.ComplexTypes.Options;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Core;

/// <summary>
///   Manages the timer shared between all <see cref="PowerShell" /> with the <see cref="Hooks" /> module instances.
/// </summary>
internal static class SharedTimerManager {
  private const string MUTEX_NAME = "Global\\Mercury.PowerShell.Hooks.TimerManagerMutex";

  private static Timer? _mainTimer;
  private static Timer? _retryTimer;
  private static Mutex? _mutex;
  private static bool _isLeader;

  /// <summary>
  ///   Initializes the shared timer manager.
  /// </summary>
  public static void Initialize() {
    _mutex = new Mutex(false, MUTEX_NAME);
    TryBecomeLeader();
  }

  private static void TryBecomeLeader() {
    try {
      if (_mutex?.WaitOne(0) ?? false) {
        _isLeader = true;
        StartMainTimer();
        StopRetryTimer();
      }
      else {
        StartRetryTimer();
      }
    }
    catch (AbandonedMutexException) {
      _isLeader = true;
      StartMainTimer();
      StopRetryTimer();
    }
  }

  private static void StartMainTimer() {
    if (!StateManager.TryGetValue<HookOptions>(HookOptions.KEY, out var options)) {
      options = HookOptions.InitialValue;
    }

    _mainTimer = new Timer(MainTimerCallback, null, options.PeriodicTimeSpan, options.PeriodicTimeSpan);
  }

  private static void MainTimerCallback(object? state) {
    if (!StateManager.TryGetValue<HookStore>(HookType.Periodic.GetVariableKey(), out var hookStore)) {
      throw new InvalidOperationException("The periodic hook store was not found.");
    }

    HookStore.InvokeAll(hookStore.Items);
  }

  private static void StartRetryTimer()
    => _retryTimer ??= new Timer(RetryTimerCallback, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));

  private static void RetryTimerCallback(object? state)
    => TryBecomeLeader();

  private static void StopRetryTimer() {
    _retryTimer?.Dispose();
    _retryTimer = null;
  }

  /// <summary>
  ///   Cleans up the shared timer manager.
  /// </summary>
  public static void Cleanup() {
    try {
      if (_isLeader) {
        _mainTimer?.Dispose();
        _mainTimer = null;
      }

      StopRetryTimer();
    }
    finally {
      if (_mutex != null) {
        if (_isLeader) {
          try {
            _mutex.ReleaseMutex();
          }
          catch (ApplicationException) {
            // The mutex was not owned by the calling thread
          }
        }

        _mutex.Dispose();
        _mutex = null;
      }

      _isLeader = false;
    }
  }
}
