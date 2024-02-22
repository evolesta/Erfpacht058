# Dockerfile om een statische docker container voor de API op te zetten

# Nieuwe test image 'builden' met de Dockerfile van Microsoft
FROM erfpacht058api AS builder
COPY Erfpacht058-API Erfpacht058-API/
RUN docker build -t Erfpacht058-API:test .

# Nieuwe container opzetten
FROM erfpacht058api
COPY --from=builder /Erfpacht058-API /app
RUN docker run --name Erfpacht058-APIst -d -p 8080:8080 -p 8081:8081 -d erfpacht058api:dev