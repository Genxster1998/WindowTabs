namespace Bemo
open System

/// DPI Helper module for Per-Monitor DPI awareness support
module DpiHelper =
    /// Base DPI (100% scaling)
    let baseDpi = 96

    /// Get DPI for a specific window handle (safe, returns 96 on failure)
    let getDpiForWindow (hwnd: IntPtr) : int =
        try
            if hwnd = IntPtr.Zero then
                WinUserApi.GetSystemDpiSafe()
            else
                WinUserApi.GetDpiForWindowSafe(hwnd)
        with _ -> baseDpi

    /// Get system DPI (primary monitor)
    let getSystemDpi () : int =
        try
            WinUserApi.GetSystemDpiSafe()
        with _ -> baseDpi

    /// Get scale factor for a window (1.0 = 100%, 1.25 = 125%, 1.5 = 150%, 2.0 = 200%)
    let getScaleFactor (hwnd: IntPtr) : float =
        try
            let dpi = getDpiForWindow hwnd
            if dpi > 0 then float dpi / float baseDpi else 1.0
        with _ -> 1.0

    /// Get system scale factor
    let getSystemScaleFactor () : float =
        try
            let dpi = getSystemDpi()
            if dpi > 0 then float dpi / float baseDpi else 1.0
        with _ -> 1.0

    /// Scale a logical size to physical pixels for a specific window
    let scaleSize (hwnd: IntPtr) (size: int) : int =
        let factor = getScaleFactor hwnd
        int (float size * factor + 0.5)

    /// Scale a logical size using system DPI
    let scaleSystemSize (size: int) : int =
        let factor = getSystemScaleFactor()
        int (float size * factor + 0.5)

    /// Scale a Sz (size struct) for a specific window
    let scaleSz (hwnd: IntPtr) (sz: Sz) : Sz =
        let factor = getScaleFactor hwnd
        Sz(int (float sz.width * factor + 0.5), int (float sz.height * factor + 0.5))

    /// Scale a Sz using system DPI
    let scaleSystemSz (sz: Sz) : Sz =
        let factor = getSystemScaleFactor()
        Sz(int (float sz.width * factor + 0.5), int (float sz.height * factor + 0.5))

    /// Scale a float value for a specific window
    let scaleFloat (hwnd: IntPtr) (value: float32) : float32 =
        let factor = getScaleFactor hwnd
        float32 (float value * factor)

    /// Scale a float value using system DPI  
    let scaleSystemFloat (value: float32) : float32 =
        let factor = getSystemScaleFactor()
        float32 (float value * factor)
