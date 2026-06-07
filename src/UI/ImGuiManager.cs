using System;
using ImGuiNET;

namespace TF2Mod.UI
{
    /// <summary>
    /// Manages ImGui initialization, frame lifecycle, and input handling.
    /// </summary>
    public class ImGuiManager : IDisposable
    {
        private ImGuiIOPtr _io;
        private bool _initialized = false;

        public bool IsInitialized => _initialized;

        public void Initialize()
        {
            try
            {
                IntPtr context = ImGui.CreateContext();
                ImGui.SetCurrentContext(context);

                _io = ImGui.GetIO();
                _io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
                _io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

                // Load default font
                _io.Fonts.AddFontDefault();
                _io.Fonts.Build();

                // Set ImGui style
                ImGui.StyleColorsDark();

                _initialized = true;
                ModBase.Log("ImGui initialized successfully");
            }
            catch (Exception ex)
            {
                ModBase.LogError($"Failed to initialize ImGui: {ex}");
            }
        }

        public void BeginFrame()
        {
            if (!_initialized) return;

            ImGui.NewFrame();
        }

        public void EndFrame()
        {
            if (!_initialized) return;

            ImGui.Render();
            ImGui.EndFrame();
        }

        public void Update()
        {
            if (!_initialized) return;

            // Update ImGui state
            // This would normally be called from your render loop
        }

        public void Shutdown()
        {
            if (!_initialized) return;

            try
            {
                ImGui.DestroyContext();
                _initialized = false;
                ModBase.Log("ImGui shutdown successfully");
            }
            catch (Exception ex)
            {
                ModBase.LogError($"Error shutting down ImGui: {ex}");
            }
        }

        public void Dispose()
        {
            Shutdown();
        }
    }
}