# Releasenotes
Dit document bevat de releasenotes betreffende de Erfpacht058 applicatie.

### Versie 1.3
Geautomatiseerd unittesten toegevoegd aan de frontend en backend. Code kan nu getest worden vooraf deze gepubliceerd wordt. 

### Versie 1.2
Kadaster BAG API geimplementeerd. Er kan nu informatie vanuit het Kadaster gesynchroniseerd worden voor een eigendom.

### Versie 1.1
Bevat bugfixes n.a.v. problemen die in de acceptatieomgeving zijn ontstaan en ontdekt in de acceptatietest
* Bug opgelost die na het vernieuwen van de bestaande verlopen token niet goed uitgelezen werd, hierdoor werd de gebruiker na een succesvolle inlog herleid naar de loginpagina
* Bug opgelost omtrent het up- en downloaden van bestanden. Zie issuetracker voor meer details. 

### Versie 1.0
Betreft de allereerste productie release. 
* Implementatie van Eigendom module, betreffende alle gerelateerde informatie rondom ontwerp
* Implementatie Storage i.c.m. Docker en externe Windows fileshares
* Implementatie import- en exportfunctionaliteit
* Implementatie overeenkomsten en eigenaren met relaties tot eigendommen
* Implementatie tot genereren van facturen
* Implementatie van gebruikersbeheer
* Implementatie van authenticatie en autorisatie