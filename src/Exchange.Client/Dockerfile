FROM node:14.7.0-alpine3.10 as node-angular-cli
 
# Building Angular app
WORKDIR /app
COPY src/package.json /app
COPY src/package-lock.json /app
COPY src/.npmrc /app

RUN npm install @aas/core

RUN npm config set registry https://registry.npmjs.org/

RUN npm install 

COPY ./src /app
RUN npm run build:ssr:dev
 
FROM node:14.7.0-alpine3.10 
WORKDIR /app
COPY --from=node-angular-cli /app/dist ./dist
EXPOSE 8210
ENV PORT 80
CMD [ "node", "dist/exchange/server/main.js" ]