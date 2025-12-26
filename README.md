# 🚀 Windows Verbose Status Tool

A lightweight utility designed to enable **Verbose Status Messages** on Windows. When enabled, Windows will display detailed technical information during Startup and Shutdown (e.g., *Loading user profile...*, *Stopping services...*) instead of just "Shutting down".

## ✨ Features
- **One-Click Toggle:** Easily enable or disable the feature with a single button.
- **Lightweight:** Built on .NET 2.0/3.5 for maximum compatibility and minimal file size.
- **"Fake it 'til you make it":** Uses `App.config` trickery to run on Windows 10/11 without requiring the .NET 3.5 feature to be installed.
- **Safe & Professional:** Includes automatic Administrator privilege checks and OS compatibility verification.

## 🚀 How to Use (For Users)
1. Download the `VerboseTool.zip` from the [Releases](../../releases) section.
2. Extract and run `VerboseTool.exe`.
3. Click **Enable Verbose** (Administrator rights are required).
4. Restart your computer to see the detailed status messages in action!

## 💻 For Developers
This project is an open-source example of system-level registry manipulation using C#.

- **Framework:** .NET 2.0 / 3.5.
- **Key Techniques:**
  - `Microsoft.Win32` for registry access.
  - `App.config` runtime redirection for cross-version .NET support.
  - `Application Manifest` for UAC (User Account Control) integration.
  - Real-time UI synchronization with system state.

## ⚠️ Important Note
This tool modifies the System Registry (`HKEY_LOCAL_MACHINE`). It must be run as an **Administrator** to function correctly.
