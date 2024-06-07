# Stage 1: Build the Angular application
FROM node:latest AS build-stage
WORKDIR /app
COPY Erfpacht058-Web/package*.json /app/
RUN npm install

# Copy the source application to the container
COPY Erfpacht058-Web/ /app/

# Copy environment file from the config dir
COPY config/environment.ts ./src/environments/environment.ts

# Build the Angular application
RUN npm run build

# Stage 2: Converteer certificaat naar een los cert en private key bestanden
FROM alpine:latest AS cert-stage
RUN apk add --no-cache openssl
WORKDIR /certs
ARG CERTFILE
ARG CERTPASSWORD
COPY ${CERTFILE} /certs/certificate.pfx
RUN openssl pkcs12 -in /certs/certificate.pfx -passin pass:${CERTPASSWORD} -clcerts -nokeys -out /certs/certificate.crt && \
    openssl pkcs12 -in /certs/certificate.pfx -passin pass:${CERTPASSWORD} -nocerts -out /certs/private_key.key -nodes

# Stage 3: Kopieer Angular files en certicaten naar Apache container, activeer SSL
FROM httpd:latest
COPY --from=build-stage /app/dist/erfpacht058-web/browser /usr/local/apache2/htdocs/
COPY --from=cert-stage /certs/certificate.crt /usr/local/apache2/conf/ssl/certificate.crt
COPY --from=cert-stage /certs/private_key.key /usr/local/apache2/conf/ssl/private_key.key

RUN sed -i '/LoadModule ssl_module/s/^#//g' /usr/local/apache2/conf/httpd.conf && \
    sed -i '/Include conf\/extra\/httpd-ssl.conf/s/^#//g' /usr/local/apache2/conf/httpd.conf

EXPOSE 80
EXPOSE 443