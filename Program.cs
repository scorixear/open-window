using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OrdnerOeffner;

public class Program
{
    // Windows API declarations for bringing window to front
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll")]
    private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    private const int SW_RESTORE = 9;

    public static void Main()
    {
        try
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            // Get folder path from configuration
            var folderPath = configuration["FolderSettings:FolderPath"];

            if (string.IsNullOrEmpty(folderPath))
            {
                _ = MessageBox(IntPtr.Zero, "Kein Ordner konfiguriert in appsettings.json", "Error", 0x10);
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                _ = MessageBox(IntPtr.Zero, $"Ordner '{folderPath}' existiert nicht", "Error", 0x10);
                return;
            }

            // Get list of existing Explorer windows before opening new one
            var existingWindows = GetExplorerWindows();

            // Open folder in Windows Explorer
            var processInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{folderPath}\"",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
            };

            var process = Process.Start(processInfo);
            
            if (process != null)
            {
                // Wait a moment for the window to appear
                Thread.Sleep(500);

                // Find and bring the new Explorer window to front
                BringNewExplorerWindowToFront(existingWindows);
            }
        }
        catch (Exception ex)
        {
            _ = MessageBox(IntPtr.Zero, $"Unbekannter Fehler: {ex.Message}", "Error", 0x10);
        }
    }

    private static List<IntPtr> GetExplorerWindows()
    {
        var windows = new List<IntPtr>();
        
        EnumWindows((hWnd, lParam) =>
        {
            var className = new StringBuilder(256);
            _ = GetClassName(hWnd, className, className.Capacity);

            if (className.ToString() == "CabinetWClass")
            {
                windows.Add(hWnd);
            }
            return true;
        }, IntPtr.Zero);

        return windows;
    }

    private static void BringNewExplorerWindowToFront(List<IntPtr> existingWindows)
    {
        var currentWindows = GetExplorerWindows();
        var newWindows = currentWindows.Except(existingWindows).ToList();
        
        if (newWindows.Count > 0)
        {
            var targetWindow = newWindows.First();
            ShowWindow(targetWindow, SW_RESTORE);
            SetForegroundWindow(targetWindow);
        }
    }
}
