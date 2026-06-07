using System;
using System.Runtime.InteropServices;

namespace TF2Mod.Interop
{
    /// <summary>
    /// Native interop declarations for Windows API calls.
    /// Used for memory access, hooking, and window management.
    /// </summary>
    public static class NativeInterop
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool IsProcessorFeaturePresent(uint ProcessorFeature);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, uint count);

        // Window/input related
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Memory protection constants
        public const uint PAGE_NOACCESS = 0x01;
        public const uint PAGE_READONLY = 0x02;
        public const uint PAGE_READWRITE = 0x04;
        public const uint PAGE_WRITECOPY = 0x08;
        public const uint PAGE_EXECUTE = 0x10;
        public const uint PAGE_EXECUTE_READ = 0x20;
        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        public const uint PAGE_EXECUTE_WRITECOPY = 0x80;

        public static IntPtr GetModuleExport(string moduleName, string exportName)
        {
            IntPtr hModule = GetModuleHandle(moduleName);
            if (hModule == IntPtr.Zero)
            {
                ModBase.LogWarning($"Module not found: {moduleName}");
                return IntPtr.Zero;
            }

            IntPtr procAddr = GetProcAddress(hModule, exportName);
            if (procAddr == IntPtr.Zero)
            {
                ModBase.LogWarning($"Export not found: {exportName} in {moduleName}");
                return IntPtr.Zero;
            }

            return procAddr;
        }
    }
}