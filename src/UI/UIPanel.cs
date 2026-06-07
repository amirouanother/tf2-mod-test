using System;
using System.Numerics;
using ImGuiNET;
using TF2Mod.UI.Tabs;

namespace TF2Mod.UI
{
    /// <summary>
    /// Main UI panel with customizable position, size, and appearance.
    /// Manages tab system and settings persistence.
    /// </summary>
    public class UIPanel
    {
        private UISettings _settings;
        private TF2CBSTab _tf2CBSTab;
        private int _selectedTabIndex = 0;
        private bool _isVisible = true;
        private Vector2 _panelPosition = new Vector2(100, 100);
        private Vector2 _panelSize = new Vector2(500, 400);

        public UISettings Settings => _settings;
        public bool IsVisible => _isVisible;

        public void Initialize()
        {
            _settings = new UISettings();
            _settings.Load();

            _tf2CBSTab = new TF2CBSTab();
            _tf2CBSTab.Initialize();

            ModBase.Log("UI Panel initialized");
        }

        public void Update()
        {
            if (!_isVisible) return;

            _tf2CBSTab?.Update();
        }

        public void Render()
        {
            if (!_isVisible) return;

            // Main panel
            ImGui.SetNextWindowPos(_panelPosition, ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(_panelSize, ImGuiCond.FirstUseEver);

            if (ImGui.Begin("TF2 Mod##main", ref _isVisible, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize))
            {
                // Store new position and size
                _panelPosition = ImGui.GetWindowPos();
                _panelSize = ImGui.GetWindowSize();

                RenderTabs();

                ImGui.End();
            }
        }

        private void RenderTabs()
        {
            if (ImGui.BeginTabBar("##TabBar"))
            {
                // TF2CBS Tab
                if (ImGui.BeginTabItem("TF2CBS##tab"))
                {
                    _selectedTabIndex = 0;
                    _tf2CBSTab?.Render();
                    ImGui.EndTabItem();
                }

                // Settings Tab
                if (ImGui.BeginTabItem("Settings##tab"))
                {
                    _selectedTabIndex = 1;
                    RenderSettingsTab();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        private void RenderSettingsTab()
        {
            ImGui.SeparatorText("Appearance");

            // Color settings
            Vector4 bgColor = _settings.BackgroundColor;
            if (ImGui.ColorEdit4("Background Color##bg", ref bgColor))
            {
                _settings.BackgroundColor = bgColor;
            }

            Vector4 textColor = _settings.TextColor;
            if (ImGui.ColorEdit4("Text Color##text", ref textColor))
            {
                _settings.TextColor = textColor;
            }

            // Transparency
            float opacity = _settings.Opacity;
            if (ImGui.SliderFloat("Opacity", ref opacity, 0.0f, 1.0f))
            {
                _settings.Opacity = opacity;
            }

            ImGui.Separator();
            ImGui.SeparatorText("Keybinds");

            ImGui.Text("Toggle UI: Not implemented");
            ImGui.Text("Monitor Connection: Bound to TF2CBS button");

            ImGui.Separator();

            if (ImGui.Button("Save Settings", new Vector2(150, 0)))
            {
                _settings.Save();
                ModBase.Log("Settings saved");
            }

            ImGui.SameLine();

            if (ImGui.Button("Reset to Defaults", new Vector2(150, 0)))
            {
                _settings.ResetToDefaults();
                ModBase.Log("Settings reset to defaults");
            }
        }

        public void Shutdown()
        {
            _settings?.Save();
            _tf2CBSTab?.Shutdown();
            ModBase.Log("UI Panel shutdown");
        }

        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
        }
    }
}