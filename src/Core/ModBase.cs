using System;
using System.Runtime.InteropServices;
using TF2Mod.UI;
using TF2Mod.Hooks;

namespace TF2Mod.Core
{
    /// <summary>
    /// Main mod initialization and lifecycle management.
    /// Serves as the entry point for the TF2 mod.
    /// </summary>
    public class ModBase
    {
        private static ModBase _instance;
        private ImGuiManager _imGuiManager;
        private ServerConnectionHook _connectionHook;
        private ConnectionMonitor _connectionMonitor;
        private UIPanel _mainPanel;
        private bool _initialized = false;

        public static ModBase Instance => _instance ??= new ModBase();

        public ConnectionMonitor ConnectionMonitor => _connectionMonitor;
        public UIPanel MainPanel => _mainPanel;

        /// <summary>
        /// Initialize the mod. Call this once at startup.
        /// </summary>
        public void Initialize()
        {
            if (_initialized) return;

            try
            {
                // Initialize UI system
                _imGuiManager = new ImGuiManager();
                _imGuiManager.Initialize();

                // Initialize connection monitoring
                _connectionMonitor = new ConnectionMonitor();

                // Initialize server connection hook
                _connectionHook = new ServerConnectionHook(_connectionMonitor);
                _connectionHook.Install();

                // Create main UI panel
                _mainPanel = new UIPanel();
                _mainPanel.Initialize();

                _initialized = true;
                Log("TF2 Mod initialized successfully");
            }
            catch (Exception ex)
            {
                LogError($"Failed to initialize mod: {ex}");
            }
        }

        /// <summary>
        /// Update mod state. Call this every frame or at regular intervals.
        /// </summary>
        public void Update()
        {
            if (!_initialized) return;

            try
            {
                _imGuiManager?.Update();
                _connectionMonitor?.Update();
                _mainPanel?.Update();
            }
            catch (Exception ex)
            {
                LogError($"Error during update: {ex}");
            }
        }

        /// <summary>
        /// Render UI. Call after Update.
        /// </summary>
        public void Render()
        {
            if (!_initialized) return;

            try
            {
                _imGuiManager?.BeginFrame();
                _mainPanel?.Render();
                _imGuiManager?.EndFrame();
            }
            catch (Exception ex)
            {
                LogError($"Error during render: {ex}");
            }
        }

        /// <summary>
        /// Shutdown the mod. Call on unload.
        /// </summary>
        public void Shutdown()
        {
            if (!_initialized) return;

            try
            {
                _connectionHook?.Uninstall();
                _mainPanel?.Shutdown();
                _imGuiManager?.Shutdown();
                _initialized = false;
                Log("TF2 Mod shutdown successfully");
            }
            catch (Exception ex)
            {
                LogError($"Error during shutdown: {ex}");
            }
        }

        public static void Log(string message)
        {
            Console.WriteLine($"[TF2Mod] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[TF2Mod ERROR] {DateTime.Now:HH:mm:ss.fff} - {message}");
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[TF2Mod WARNING] {DateTime.Now:HH:mm:ss.fff} - {message}");
            Console.ResetColor();
        }
    }
}