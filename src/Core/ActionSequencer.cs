using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TF2Mod.Core
{
    /// <summary>
    /// Manages the timed sequence of actions during server connection.
    /// Executes the pattern:
    /// - 4500ms initial sequence
    /// - 300ms pause
    /// - 800ms sequence
    /// - Loop
    /// </summary>
    public class ActionSequencer
    {
        private SequenceState _state = SequenceState.Idle;
        private Stopwatch _timer = new Stopwatch();
        private List<SequenceAction> _currentActions = new List<SequenceAction>();

        private const long INITIAL_SEQUENCE_MS = 4500;
        private const long SHORT_SEQUENCE_MS = 800;
        private const long PAUSE_MS = 300;

        public SequenceState CurrentState => _state;

        public ActionSequencer()
        {
        }

        public void StartSequence()
        {
            _state = SequenceState.InitialSequence;
            _timer.Restart();
            _currentActions.Clear();
            ModBase.Log("Action sequence started: Initial sequence (4500ms)");
        }

        public void StopSequence()
        {
            if (_state == SequenceState.Idle) return;

            _timer.Stop();
            _state = SequenceState.Idle;
            _currentActions.Clear();
            ModBase.Log("Action sequence stopped");
        }

        public void Update()
        {
            if (_state == SequenceState.Idle) return;

            long elapsed = _timer.ElapsedMilliseconds;

            switch (_state)
            {
                case SequenceState.InitialSequence:
                    ExecuteInitialSequence(elapsed);
                    break;
                case SequenceState.Pause:
                    ExecutePause(elapsed);
                    break;
                case SequenceState.ShortSequence:
                    ExecuteShortSequence(elapsed);
                    break;
            }
        }

        private void ExecuteInitialSequence(long elapsed)
        {
            if (elapsed < INITIAL_SEQUENCE_MS)
            {
                // Execute actions during the 4.5 second window
                ExecuteAction("InitialSequence", elapsed, INITIAL_SEQUENCE_MS);
            }
            else
            {
                // Transition to pause
                _state = SequenceState.Pause;
                _timer.Restart();
                ModBase.Log("Initial sequence complete, entering pause (300ms)");
            }
        }

        private void ExecutePause(long elapsed)
        {
            if (elapsed >= PAUSE_MS)
            {
                // Transition to short sequence
                _state = SequenceState.ShortSequence;
                _timer.Restart();
                ModBase.Log("Pause complete, starting short sequence (800ms)");
            }
        }

        private void ExecuteShortSequence(long elapsed)
        {
            if (elapsed < SHORT_SEQUENCE_MS)
            {
                ExecuteAction("ShortSequence", elapsed, SHORT_SEQUENCE_MS);
            }
            else
            {
                // Loop back to initial sequence
                _state = SequenceState.InitialSequence;
                _timer.Restart();
                ModBase.Log("Short sequence complete, restarting loop");
            }
        }

        private void ExecuteAction(string sequenceName, long currentMs, long totalMs)
        {
            // This is where actual game actions would be executed
            // Examples: mouse movements, key presses, network packet injection, etc.
            
            // For demonstration, we calculate progress and could emit action events
            float progress = (float)currentMs / totalMs;
            
            // Placeholder for actual action execution
            // In a real implementation, this would interact with the game
        }

        public enum SequenceState
        {
            Idle,
            InitialSequence,
            Pause,
            ShortSequence
        }
    }

    /// <summary>
    /// Represents a single action to execute during a sequence.
    /// </summary>
    public class SequenceAction
    {
        public string Name { get; set; }
        public long DelayMs { get; set; }
        public Action Callback { get; set; }

        public SequenceAction(string name, long delayMs, Action callback)
        {
            Name = name;
            DelayMs = delayMs;
            Callback = callback;
        }
    }
}