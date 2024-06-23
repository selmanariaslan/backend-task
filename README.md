# backend-task

## Über das Projekt

Dieses Projekt ist ein OData RESTful Service, der mit .NET 6 oder .NET 8 entwickelt wurde. Es verwendet Entity Framework Core (EF Core) für die Verbindung mit einer PostgreSQL-Datenbank. Das Projekt ist für grundlegende CRUD-Operationen (Erstellen, Lesen, Aktualisieren, Löschen) ausgelegt.

## Voraussetzungen
 - .NET 6 oder .NET 8 SDK
 - PostgreSQL
 - Entity Framework Core
 - OData
 - xUnit


## Einrichtung

### 1. Klonen des Quellcodes
```bash
git clone https://github.com/selmanariaslan/backend-task.git		
cd Task.Api
```

### 2. Abhängigkeiten installieren
```bash
dotnet restore
```
	
### 3. Datenbankkonfiguration

Zunächst müssen Sie das Projekt ausführen und den ConnectionString über die folgende API verschlüsseln 

```http
	 https://localhost:5000/Crypto/Encrypt/{YourConnectionString}
```
	 
Bearbeiten Sie die appsettings.json Datei, um Ihre PostgreSQL-Verbindungsinformationen einzugeben:
	

```json
"ConnectionStrings": {
"LogManagement": "127204136057027103049196120217247097199089083214053088235022131184058045171000124233209082089253",
"TaskDb": "127204136057027103049196120217247097199089083214053088235022131184058045171000124233209082089253"}
```



### 4. Anwenden der Datenbankmigrationen
	
Dies sollte sowohl für DatabaseContext(LogManagementContext, TaskContext)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```


### 5. Starten der Anwendung
```bash
dotnet run
```

Öffnen Sie Ihren Browser und gehen Sie zu https://localhost:5000/odata/Products, um die API zu testen.

## Ausführen der Unit Tests
Gehen Sie zum Testprojekt und führen Sie die Tests aus:
```bash
cd ../Task.Tests
dotnet test
```

### Testszenarien
- **GetProductsReturnsOkResponse:** Gibt erfolgreich alle Produkte zurück.
- **GetProductByIdReturnsOkResponse:** Gibt ein bestimmtes Produkt nach ID erfolgreich zurück.
- **PostProductReturnsCommonUpsertModel:** Erstellt erfolgreich ein neues Produkt.
- **PutProductReturnsCommonUpsertModel:** Aktualisiert erfolgreich ein bestehendes Produkt.
- **DeleteProductReturnsCommonUpsertModel:** Löscht(soft) erfolgreich ein bestimmtes Produkt nach ID.


## Weitere Informationen
### Swagger: 
Das Swagger-Interface ist unter **http://localhost:5000/swagger** verfügbar.

### OData Endpoints:
- GET /odata/Products
- GET /odata/Products({id})
- POST /odata/Products
- PUT /odata/Products({id})
- DELETE /odata/Products({id})