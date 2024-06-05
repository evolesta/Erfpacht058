# Compileer Angular vanaf broncode naar een functionele container
FROM node:latest AS build-stage
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY ./ /app/
RUN npm run build

# Configureer de Apache Http container
FROM httpd:latest
COPY --from=build-stage /app/dist/erfpacht058-web/browser /usr/local/apache2/htdocs/