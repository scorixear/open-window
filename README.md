# Ordner Öffner

Eine C# Konsolenanwendung, die im Hintergrund läuft und einen angegebenen Ordner im Windows Explorer öffnet, das Fenster in den Vordergrund bringt und fokussiert.

## Funktionen

- Öffnet einen Ordner im Windows Explorer
- Bringt das Explorer-Fenster in den Vordergrund und fokussiert es
- Konfigurierbarer Ordnerpfad über `appsettings.json`
- Läuft im Hintergrund ohne Anzeige eines Konsolenfensters
- Fehlerbehandlung für ungültige Pfade mit deutschen Meldungen

## Konfiguration

Bearbeiten Sie die `appsettings.json` Datei, um den zu öffnenden Ordner anzugeben:

```json
{
  "Ordner": "C:\\Users\\Public\\Documents"
}
```

## Erstellen und Ausführen

### Voraussetzungen
- .NET 8.0 SDK oder neuer
- Windows-Betriebssystem

### Anwendung erstellen
```cmd
dotnet build OrdnerOeffner.csproj
```

### Anwendung ausführen
```cmd
dotnet run --project OrdnerOeffner.csproj
```

### Eigenständige ausführbare Datei erstellen
```cmd
dotnet publish OrdnerOeffner.csproj -c Release -o publish
```

Dies erstellt eine eigenständige ausführbare Datei im `publish\` Verzeichnis:
- `OrdnerOeffner.exe` - Die Hauptanwendung (benötigt keine .NET-Installation)
- `appsettings.json` - Die Konfigurationsdatei

## Verwendung

1. Konfigurieren Sie den Ordnerpfad in `appsettings.json`
2. Führen Sie die ausführbare Datei aus
3. Der angegebene Ordner wird im Windows Explorer geöffnet und in den Vordergrund gebracht

## Technische Details

Die Anwendung verwendet:
- Windows-API-Aufrufe (`user32.dll`) zur Fenstermanipulation
- Microsoft.Extensions.Configuration zum Lesen der Einstellungen
- Process.Start zum Starten des Windows Explorers
- Fenstererkennung um das korrekte Explorer-Fenster zu fokussieren

## Fehlerbehandlung

Die Anwendung behandelt die folgenden Fehlerszenarios:
- Fehlende oder ungültige `appsettings.json` Datei
- Ungültiger Ordnerpfad in der Konfiguration
- Ordner existiert nicht
- Allgemeine Ausnahmen während der Ausführung

Alle Fehlermeldungen werden als Windows-Dialogfenster angezeigt.
