FROM node:22-slim
WORKDIR /app
COPY /servers/darena-solutions/fhir-crud ./
RUN npm install

ENV NODE_ENV=production
CMD ["npm", "run", "start"]
