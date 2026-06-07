using System;
using System.Numerics;
using ImGuiNET;
using TF2Mod.Core;

namespace TF2Mod.UI.Tabs
{
    /// <summary>
    /// Tab containing the connection monitoring button.
    /// Status display for connection state and action sequence.
    /// </summary>
    public class TF2CBSTab
    {
        private ConnectionMonitor _monitor;
        private bool _isMonitoring = false;
        private string _statusText = "Ready";
        private Vector4 _statusColor = new Vector4(0, 1, 0, 1); // Green

        public void Initialize()
        {
            _monitor = ModBase.Instance.ConnectionMonitor;
            if (_monitor != null)
            {
                _monitor.MonitoringStarted += (s, e) => OnMonitoringStarted();
                _monitor.MonitoringStopped += (s, e) => OnMonitoringStopped();
                _monitor.ConnectionStateChanged += (s, e) => OnConnectionStateChanged(e);
            }
            ModBase.Log("TF2CBS Tab initialized");
        }

        public void Update()
        {
            if (_monitor != null)
            {
                _isMonitoring = _monitor.IsMonitoring;

                if (_isMonitoring)
                {
                    int bars = _monitor.ConnectionStrengthBars;
                    _statusText = $"Monitoring... ({bars}/3 bars)";
                    _statusColor = new Vector4(1, 1, 0, 1); // Yellow
                }
            }
        }

        public void Render()
        {
            ImGui.Text("Server Connection Monitor");
            ImGui.Separator();

            // Status display
            ImGui.TextColored(_statusColor, $"Status: {_statusText}");

            ImGui.Spacing();
            ImGui.Spacing();

            // Main action button
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.6f, 0.9f, 1));
            if (ImGui.Button("Start Connection Monitor", new Vector2(-1, 50)))
            {
                if (!_isMonitoring)
                {
                    _monitor?.StartMonitoring();
                }
                else
                {
                    _monitor?.StopMonitoring();
                }
            }
            ImGui.PopStyleColor();

            ImGui.Spacing();

            // Info box
            ImGui.BeginChild("##InfoBox", new Vector2(0, 200), true);
            ImGui.TextWrapped(
                "This tool monitors your Team Fortress 2 server connection and executes a timed " +
                "sequence of actions to optimize the connection process.\n\n" +
                "How it works:\n" +
                "1. Click 'Start Connection Monitor'\n" +
                "2. The sequence will run automatically\n" +
                "3. It will stop when connection reaches full strength\n\n" +
                "Note: This requires integration with TF2's engine hooks."
            );
            ImGui.EndChild();

            ImGui.Spacing();

            // Debug info
            if (ImGui.CollapsingHeader("Debug Information"))
            {
                if (_monitor != null)
                {
                    ImGui.Text($"Monitoring: {(_isMonitoring ? "Yes" : "No")}");
                    ImGui.Text($"Connection State: {_monitor.CurrentState}");
                    ImGui.Text($"Connection Strength: {_monitor.ConnectionStrengthBars}/3");
                }
            }
        }

        public void Shutdown()
        {
            if (_monitor != null)
            {
                _monitor.StopMonitoring();
            }
            ModBase.Log("TF2CBS Tab shutdown");
        }

        private void OnMonitoringStarted()
        {
            _isMonitoring = true;
            _statusText = "Monitoring (0/3 bars)";
            _statusColor = new Vector4(1, 1, 0, 1); // Yellow
        }

        private void OnMonitoringStopped()
        {
            _isMonitoring = false;
            _statusText = "Complete";
            _statusColor = new Vector4(0, 1, 0, 1); // Green
        }

        private void OnConnectionStateChanged(Core.ConnectionStateChangedEventArgs args)
        {
            ModBase.Log($"Connection state changed: {args.OldState} -> {args.NewState}");
        }
    }
}