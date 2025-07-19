# Open Window Console Application

A C# console application that runs silently in the background and opens a specified folder in Windows Explorer, bringing the window to the front and setting it to focus.

## Features

- Opens a folder in Windows Explorer
- Brings the Explorer window to the front and gives it focus
- Configurable folder path via `appsettings.json`
- Runs silently without displaying a console window
- Error handling for invalid paths

## Configuration

Edit the `appsettings.json` file to specify the folder you want to open:

```json
{
  "FolderSettings": {
    "FolderPath": "C:\\Users\\Public\\Documents"
  }
}
```

## Building and Running

### Prerequisites
- .NET 8.0 SDK or later
- Windows operating system

### Build the application
```cmd
dotnet build
```

### Run the application
```cmd
dotnet run --project OpenWindow.csproj
```

Or use the provided batch files:
- `run.bat` - Runs the application with dotnet
- `run-silent.bat` - Runs the compiled executable silently in the background

### Create a self-contained executable
```cmd
dotnet publish OpenWindow.csproj -c Release -o publish
```

This will create a self-contained single executable in the `publish\` directory:
- `OpenWindow.exe` - The main executable (doesn't require .NET to be installed)
- `appsettings.json` - The configuration file
- `OpenWindow.pdb` - Debug symbols (can be deleted for distribution)

## Usage

1. Configure the folder path in `appsettings.json`
2. Run the executable
3. The specified folder will open in Windows Explorer and be brought to the front

## Technical Details

The application uses:
- Windows API calls (`user32.dll`) to manipulate window focus and visibility
- Microsoft.Extensions.Configuration for reading settings
- Process.Start to launch Windows Explorer
- Window enumeration to find and focus the correct Explorer window

## Error Handling

The application handles the following error scenarios:
- Missing or invalid `appsettings.json` file
- Invalid folder path in configuration
- Folder doesn't exist
- General exceptions during execution
