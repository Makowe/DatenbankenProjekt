# Datenbanken Projekt

## Ordnerstruktur

### api
Sourcecode des .NET Core Backends in C#

### web-app
Sourcecode der Angular App

### db
project_9275184.sql Datei als Backup für die Datenbank

### publish
vollständig generierte Anwendung

### Tests
Unittests für das Backend

## Vorraussetzungen

### MySql-Server und MySql-Client installieren
```
sudo apt update
sudo apt install mysql-server mysql-client
systemctl start mysql
```

### MySql User erstellen
```
mysql
CREATE USER '9275184'@'localhost' IDENTIFIED BY 'nico';
GRANT ALL PRIVILEGES ON * . * TO '9275184'@'localhost';
quit;
```

### dotnet Framwork insatllieren
```
sudo snap install dotnet-sdk --classic
```

## Starten der Anwendung

### 1. Repository clonen

Das Repository in einen beliebigen Ordner auf den Computer clonen.
Ein Terminal öffnen und mit cd in den Hauptordner navigieren

### 2. Datenbank importieren
```
mysql -u 9275184 -p 
Passwort: nico
CREATE DATABASE project_9275184;
quit;
mysql -u 9275184 -p project_9275184 < db/project_9275184.sql
```

### 3. Anwendung starten
mit cd in den Hauptordner navigieren
```
cd publish
dotnet api.dll
```

### 4. Im Browser öffnen

Die Seite https://localhost:5000 aufrufen

### Tests ausführen
mit cd in den Hauptordner navigieren
```
cd api
dotnet test
```
