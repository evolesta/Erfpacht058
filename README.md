# Erfpacht 058
Deze repository bevat de oplossing voor het vastleggen van de erfpacht administratie specifiek voor gemeenten. 
Deze oplossing is exclusief gerealiseerd voor gemeente Leeuwarden, maar kan ook door andere gemeenten toegepast worden. 
Het product is gerealiseerd als een full-stack webapplicatie, en draait middels een losse front-end, back-end en database. 

## Vereisten
Om het product te hosten dient de architectuur over de volgende vereisten:
- Een container platform (Docker, Kubernetes, of cloud)
- MSSQL database

## Configuratie
Pas .sample config bestanden voor de front- en back-end aan met de correcte gegevens. Deze config bestanden worden in de containers gekopieerd. 
De volgende config bestanden zijn vereist om de applicatie te installeren / updaten:

./config/appsettings.json => Back-end config
./config/environment.ts => front-end config
./config/certificaat.pfx => Certificaat voor SSL

Docker zorgt bij het bouwen van de images ervoor dat de config bestanden worden gekopieerd naar de betreffende containers. 

Denk bij de config van appsettings.json (config backend) om:
- Configureer de SQL server connection string met de credentials van de database en user (https://www.connectionstrings.com/sql-server/).
- Vervang de Jwt Secret key met een nieuwe random string (https://www.random.org/strings/).
- Configureer de bestandspaden. De paden voor de bestandsopslaglocatie zijn standaard geconfigureerd om deze in de container op te slaan. Voor productie doeleinden wordt geadviseerd deze aan te passen naar een externe locatie. 
- Configureer evt. de CORS domeinnamen in AllowedCors als de API en frontend op hetzelfde IP / DNS gedraaid gaan worden. 

Denk bij de config van de frontend (environment.ts) om:
- Configureer in environment.ts (config van de frontend) de correcte URL naar de API (backend)

Globale config:
- Configureer in .env (in de root van de repo) het pad naar het certicaat en het wachtwoord (zie hoofdstuk over certificaten)

# Installatie / update
Het product kan met Docker compose worden geinstalleerd in het Docker (gerelateerde) platform. 
Voer het Docker compose commando uit:
```
docker compose up --build --detach
```

Docker zal vanaf de broncode de back-end en front-end compileren en installeren in een container. 
Na de compose worden de containers automatisch gestart. De applicatie is benaderbaar via de volgende URLs:

https://ipofurl:8080 => Voor de frontend
https://ipofurl:8001/api => API endpoints (ga naar /swagger voor de API documentatie)

Voor updates kan dezelfde procedure gevolgd worden. Er wordt aangeraden geen data in de containers op te slaan, waardoor deze stateless zijn. Hierdoor kunnen deze ten alle tijden gereset, verwijderd en opnieuw opgetuigd worden zonder verlies van data.

# Database installeren
In de Erfpacht058-API map staat een SQL script (InitialCreate.sql) die uitgevoerd dient te worden op de nieuwe (productie) database. 
Gebruik een database management tool (Bijv. SQL Server Management Studio) om het script uit te voeren. 
Het script creeert de benodigde tabellen en kolommen en zal een eerste gebruiker toevoegen aan de gebruikerstabel. 

## Demo / testomgevingen
Wanneer er geen acceptatie of test databaseomgeving beschikbaar is, is er ook een optie om een SQL Server container op te tuigen in Docker (https://hub.docker.com/_/microsoft-mssql-server).

# Certificaten
De frontend en backend draaien beiden op een SSL verbinding, ook onderling. Daarom is het van belang dat de backend en frontend over een geldig certificaat beschikken. 
Voor ontwikkel- en testdoeleinden kan op de lokale machine een self-signed certificaat aangemaakt worden, zonder aanvullende kosten.
Voor productiedoeleinden wordt geadviseerd om een certificaat aan te vragen. Als het product intern gebruikt gaat worden, volstaat een intern certificaat. Ook kan een extern certificaat aangeschaft worden. 

Het product ondersteunt PFX certificaten met een wachtwoord. Plaats het certificaat in de `config` map en wijzig het pad en wachtwoord in het .env bestand.
Dit certificaat wordt geimporteerd in de containers van de front- en backend. 

## Self-signed certificaat aanmaken
Voor het genereren van een self-signed certificaat op de lokale machine, volg onderstaande stappen (Windows).

1. Open een Powershell terminal als Administrator.
2. Genereer een self-signed certificaat:
(Optioneel): Maak een domeinnaam aan in C:\Windows\System32\drivers\etc\hosts (bijv. erfpacht058.test die verwijst naar 127.0.0.1)
```
New-SelfSignedCertificate -CertStoreLocation Cert:\LocalMachine\My -DnsName erfpacht058.test
```
3. Exporteer het certificaat PFX bestand. Plaats in config directory van de repository:
```
Export-PfxCertificate -FilePath /pad/naar/certifcaat.pfx -Password TEST123
```
4. Installeer het certificaat op de lokale machine in de root:
```
Import-PfxCertificate -FilePath /pad/naar/certifcaat.pfx -CertStoreLocation Cert:\LocalMachine\Root -Password (ConvertTo-SecureString 'TEST123' -AsPlainText -Force)
```

Bij het aanroepen van de front- en backend URLs zouden er geen certificaat foutmeldingen meer moeten ontstaan op de lokale machine.