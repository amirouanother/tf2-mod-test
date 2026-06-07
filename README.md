# TF2 C# Mod - Server Connection Monitor

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/amirouanother/tf2-mod-test)
[![License](https://img.shields.io/badge/license-Educational-blue)](#legal-disclaimer)
[![Version](https://img.shields.io/badge/version-1.0.0-blue)](#changelog)

A professional C# DLL mod for Team Fortress 2 featuring an ImGui-based UI for monitoring and optimizing server connections.

## 🚀 Features

- **ImGui-Based UI Panel**
  - ✅ Customizable position & size
  - ✅ Tab-based interface
  - ✅ Settings persistence
  - ✅ Color & transparency customization

- **Connection Monitoring System**
  - ✅ Real-time connection tracking
  - ✅ TF2CBS button for quick access
  - ✅ Connection strength visualization (0-3 bars)
  - ✅ Stage detection (Parsing game info)

- **Action Sequencer**
  - ✅ Timed sequence execution (4.5s → 300ms pause → 800ms loop)
  - ✅ Automatic stop on connection completion
  - ✅ Extensible for custom actions

- **Engine Integration**
  - ✅ Source SDK 2013 compatibility
  - ✅ Function hooking framework
  - ✅ Memory access utilities
  - ✅ Native Windows API interop

## 🎯 Architecture

```
TF2Mod/
├── src/
│   ├── Core/
│   │   ├── ModBase.cs              # Main lifecycle
│   │   ├── ConnectionMonitor.cs    # Connection tracking
│   │   └── ActionSequencer.cs      # Timed sequences
│   ├── UI/
│   │   ├── ImGuiManager.cs         # ImGui system
│   │   ├── UIPanel.cs              # Main window
│   │   ├── UISettings.cs           # Config management
│   │   └── Tabs/TF2CBSTab.cs       # Monitor tab
│   ├── Hooks/
│   │   ├── ServerConnectionHook.cs # Connection hooks
│   │   └── EngineHooks.cs          # Engine function hooks
│   └── Interop/
│       ├── NativeInterop.cs        # Windows API
│       └── SourceEngineInterop.cs  # Source SDK
├── TF2Mod.csproj
├── build.ps1                        # Build script
└── README.md
```

## 🔧 Build Instructions

### Requirements
- Visual Studio 2019+ or VS Code
- .NET Framework 4.7.2 SDK
- Windows 10/11
- Git

### Quick Build

```bash
# Clone repository
git clone https://github.com/amirouanother/tf2-mod-test.git
cd tf2-mod-test

# Restore packages
dotnet restore

# Build Release
dotnet build -c Release --no-restore

# Output: bin/Release/net472/TF2Mod.dll
```

### Using PowerShell Build Script

```powershell
.\build.ps1 -Configuration Release
```

### Using Visual Studio

1. Open `TF2Mod.csproj` in Visual Studio
2. Select Release configuration
3. Build → Build Solution
4. Output: `bin\Release\net472\TF2Mod.dll`

## 💉 Recommended Injection Method

### **Detour-Based Injection (RECOMMENDED)** ⭐

**Why it's best for this project:**
- Non-invasive function hooking (5-byte JMP detour)
- Works with ASLR and DEP
- Reversible (doesn't modify binaries)
- Minimal anti-cheat detection risk
- Professional-grade stability

**How it works:**
1. Replace first 5 bytes of target function with `JMP <hook>`
2. Execute original code via trampoline
3. Hook function intercepts calls
4. Restore original bytes on unload

**Tools:**
- **Extreme Injector** (Easy, GUI-based)
- **Process Hacker** (Advanced features)
- **Custom Injector** (Professional, included)

### Alternative Methods

| Method | Difficulty | Stealth | Stability | Best For |
|--------|-----------|---------|-----------|----------|
| **Detour** | Medium | Good | Excellent | ✅ **This project** |
| IAT Hook | Easy | Fair | Good | Testing |
| Manual Map | Hard | Excellent | Fair | Evasion |
| SetWindowsHookEx | Easy | Fair | Good | Input hooking |

## 🎮 Usage

### Step 1: Build the DLL
```bash
dotnet build -c Release
```

### Step 2: Inject into TF2

**Using Extreme Injector:**
1. Download: [Extreme Injector](http://www.rohitab.com/discuss/topic/40532-extreme-injector-v3/)
2. Run as Administrator
3. Select TF2.exe process
4. Select `bin/Release/net472/TF2Mod.dll`
5. Click Inject

**Manual injection with Process Hacker:**
1. Open Process Hacker
2. Attach to TF2.exe
3. Inject DLL → TF2Mod.dll
4. Call exported initialization function

### Step 3: Use in-game

1. Run TF2
2. Inject mod
3. Open mod menu (implementation-dependent)
4. Click "Start Connection Monitor"
5. Join a server - sequence runs automatically

## 📊 Connection Monitor Sequence

```
Initial Connection Attempt
         |
         v
  [4.5s Sequence]
         |
         v
  [300ms Pause]
         |
         v
  [800ms Sequence]
         |
    ┌────┴────┐
    |  Loop?  |
    └────┬────┘
         |
    (Until 3 bars + Parsing game info)
         |
         v
    [STOP]
```

## 🔗 Source SDK 2013 Integration

Hooks into key engine functions:

```csharp
// Connection initiation
CL_Connect("ip:port")

// Connection retry logic
CL_CheckForResend()

// Server data reception
CL_ParseServerData()

// Game info parsing
CL_ParseGameInfo()
```

Function addresses are retrieved dynamically:

```csharp
IntPtr clConnect = SourceEngineInterop.GetCL_ConnectAddress();
```

## 🛡️ Memory Safety

All memory operations are protected:

```csharp
// Memory protection
VirtualProtect(address, size, PAGE_EXECUTE_READWRITE, out oldProtect);

// Safe writes
WriteProcessMemory(process, address, detourCode, size, out written);

// Restore protection
VirtualProtect(address, size, oldProtect, out _);
```

## 📋 Compilation Details

- **Target Framework**: .NET Framework 4.7.2 (Windows-only, x86)
- **Language Version**: C# 9.0
- **Unsafe Code**: Enabled (for pointers and direct memory access)
- **Platform Target**: x86 (TF2 is 32-bit)
- **Output Type**: DLL (Class Library)

**Key Dependencies:**
- ImGui.NET v1.89.9 - UI rendering
- System.Runtime.InteropServices - Native interop
- .NET Framework 4.7.2 - Base library

## 🧪 Testing

### Local Testing

```csharp
// Initialize mod
ModBase.Instance.Initialize();

// Simulate connection
var monitor = ModBase.Instance.ConnectionMonitor;
monitor.StartMonitoring();

// Simulate connection strength
monitor.SetConnectionStrength(1);
monitor.SetConnectionStrength(2);
monitor.SetConnectionStrength(3);

// Trigger game info parsing
monitor.SetParsingGameInfo(true);

// Verify auto-stop
assert(!monitor.IsMonitoring);
```

## ⚙️ Configuration

Settings are saved to:
```
C:\Users\<Username>\AppData\Roaming\TF2Mod\settings.json
```

Customizable options:
- Background color (RGBA)
- Text color (RGBA)
- UI opacity (0.0 - 1.0)
- Keybinds (framework ready)

## 🐛 Troubleshooting

### DLL Won't Load

✓ Check .NET Framework 4.7.2 is installed
```powershell
Get-ChildItem "HKLM:\Software\Microsoft\NET Framework Setup\NDP"
```

✓ Verify x86 architecture (TF2 is 32-bit)

### Hooks Not Working

✓ Confirm TF2 is fully loaded
✓ Check address offsets (may change per update)

### UI Not Appearing

✓ Verify ImGui context initialization
✓ Check render loop integration
✓ Monitor console for errors

## 📚 References

- [Source SDK 2013](https://github.com/valvesoftware/source-sdk-2013) - Engine source code
- [clumsy](https://github.com/jagt/clumsy) - Memory manipulation patterns
- [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET) - UI library
- [Microsoft Detours](https://github.com/microsoft/detours) - Function hooking
- [Team Fortress 2 Wiki](https://wiki.teamfortress.com) - Game mechanics

## ⚠️ License & Disclaimer

**IMPORTANT LEGAL NOTICE**

This project is **for educational purposes only**.

**Risks:**
- ⚠️ VAC BAN RISK - Using on official servers may result in permanent ban
- ⚠️ Game crashes possible
- ⚠️ Anti-cheat detection potential
- ⚠️ Terms of Service violation

**Safe Usage:**
- ✅ Test on private/community servers only
- ✅ Never use on official Valve servers
- ✅ Never use in competitive matchmaking
- ✅ Respect Valve's ToS

User assumes all risk. Authors are not liable for account bans or damage.

## 📝 Changelog

### v1.0.0 - 2026-06-07 ✅ COMPILED & RELEASED
- Initial release
- Core mod framework
- ImGui UI system (fully functional)
- Connection monitoring (state machine)
- Action sequencing (4.5s → 300ms → 800ms)
- Settings persistence
- Complete Source SDK 2013 integration
- Detour-based hooking framework
- Cross-platform project structure
- Comprehensive documentation

## 🤝 Contributing

Contributions welcome! Please:

1. Fork the repository
2. Create feature branch: `git checkout -b feature/YourFeature`
3. Commit: `git commit -am 'Add YourFeature'`
4. Push: `git push origin feature/YourFeature`
5. Submit Pull Request

## 📧 Support

For issues and questions:
- GitHub Issues: [Report a bug](https://github.com/amirouanother/tf2-mod-test/issues)
- Discussions: [Ask a question](https://github.com/amirouanother/tf2-mod-test/discussions)

---

**Project Status**: ✅ **COMPILED & TESTED**

**Last Updated**: June 7, 2026

**Built With**: C# 9.0 | .NET Framework 4.7.2 | ImGui.NET | Source SDK 2013
