# Erfpacht 058
Deze repository bevat de oplossing voor het vastleggen van de erfpacht administratie specifiek voor gemeenten. 
Deze oplossing is exclusief gerealiseerd voor gemeente Leeuwarden, maar kan ook door andere gemeenten toegepast worden. 
Het product is gerealiseerd als een full-stack webapplicatie, en draait middels een losse front-end, back-end en database. 

## Vereisten
Om het product te hosten dient de architectuur over de volgende vereisten:
- Een container platform (Docker, Kubernetes, of cloud)
- MSSQL database

## Server - REST API
De applicatie gebruikt ASP.NET core framework als back-end met MSSQL. 
Bij het installeren middels Docker compose zal de back-end in de keten automatisch worden opgezet.
De back-end kan ook als losse image en container worden opgetuigd. De image van de broncode kan als volgt gebouwd en gestart worden als container:
```
cd .\Erfpacht058-API\Erfpacht058-API
docker build -f Dockerfile -t erfpacht058api:test ..
docker run -d -p 8080:8080 --name Erfpacht058-APItst erfpacht058api:test
```