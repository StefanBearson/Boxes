FROM node:18 AS build

WORKDIR /app

COPY magic-frontend/*.json ./

run ls -ls /app
RUN npm install

COPY magic-frontend/ .
RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]