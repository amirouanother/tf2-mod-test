using System;
using System.Diagnostics;

namespace TF2Mod.Core
{
    /// <summary>
    /// Monitors server connection state and manages the action sequence.
    /// Detects connection attempts and tracks connection strength.
    /// </summary>
    public class ConnectionMonitor
    {
        private ActionSequencer _sequencer;
        private bool _isMonitoring = false;
        private bool _hasReachedParsingGameInfo = false;
        private ConnectionState _currentState = ConnectionState.Disconnected;
        private int _connectionStrengthBars = 0;
        private DateTime _monitoringStartTime;

        public event EventHandler MonitoringStarted;
        public event EventHandler MonitoringStopped;
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public bool IsMonitoring => _isMonitoring;
        public ConnectionState CurrentState => _currentState;
        public int ConnectionStrengthBars => _connectionStrengthBars;

        public ConnectionMonitor()
        {
            _sequencer = new ActionSequencer();
        }

        /// <summary>
        /// Start monitoring server connection attempts.
        /// Called when user clicks the TF2CBS button.
        /// </summary>
        public void StartMonitoring()
        {
            if (_isMonitoring) return;

            _isMonitoring = true;
            _hasReachedParsingGameInfo = false;
            _connectionStrengthBars = 0;
            _monitoringStartTime = DateTime.Now;

            ModBase.Log("Connection monitoring started");
            MonitoringStarted?.Invoke(this, EventArgs.Empty);

            // Begin the action sequence
            _sequencer.StartSequence();
        }

        /// <summary>
        /// Stop monitoring. Called when connection reaches full strength and "Parsing game info" is reached.
        /// </summary>
        public void StopMonitoring()
        {
            if (!_isMonitoring) return;

            _isMonitoring = false;
            _sequencer.StopSequence();

            var duration = DateTime.Now - _monitoringStartTime;
            ModBase.Log($"Connection monitoring stopped after {duration.TotalSeconds:F2}s");
            MonitoringStopped?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update connection state. This should be called from the mod's Update loop
        /// or from game event hooks.
        /// </summary>
        public void Update()
        {
            if (!_isMonitoring) return;

            // Check connection strength (this would be integrated with game events)
            CheckConnectionStrength();

            // Check if we've reached "Parsing game info" stage
            CheckParsingGameInfoStage();

            // Stop if conditions are met
            if (_connectionStrengthBars >= 3 && _hasReachedParsingGameInfo)
            {
                StopMonitoring();
            }

            _sequencer.Update();
        }

        private void CheckConnectionStrength()
        {
            // This would be integrated with TF2's connection strength indicators
            // In a real implementation, this would read from the game's memory or event system
            // For now, this is a placeholder
        }

        private void CheckParsingGameInfoStage()
        {
            // This would be integrated with TF2's connection stage detection
            // In a real implementation, this would hook into the engine's connection messages
            // For now, this is a placeholder
        }

        public void SetConnectionStrength(int bars)
        {
            if (bars >= 0 && bars <= 3 && bars != _connectionStrengthBars)
            {
                int previousBars = _connectionStrengthBars;
                _connectionStrengthBars = bars;
                ModBase.Log($"Connection strength changed: {previousBars} -> {bars} bars");
            }
        }

        public void SetParsingGameInfo(bool parsing)
        {
            if (parsing && !_hasReachedParsingGameInfo)
            {
                _hasReachedParsingGameInfo = true;
                ModBase.Log("Reached 'Parsing game info' stage");
            }
        }

        public void SetConnectionState(ConnectionState newState)
        {
            if (newState != _currentState)
            {
                ConnectionState oldState = _currentState;
                _currentState = newState;
                ModBase.Log($"Connection state changed: {oldState} -> {newState}");
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(oldState, newState));
            }
        }
    }

    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
        Loading,
        InGame
    }

    public class ConnectionStateChangedEventArgs : EventArgs
    {
        public ConnectionState OldState { get; }
        public ConnectionState NewState { get; }

        public ConnectionStateChangedEventArgs(ConnectionState oldState, ConnectionState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
    }
}