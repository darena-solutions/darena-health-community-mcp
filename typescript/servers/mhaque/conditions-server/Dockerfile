FROM node:22-slim
WORKDIR /app
COPY /servers/mhaque/conditions-server ./
RUN npm install

ENV NODE_ENV=production
CMD ["npm", "run", "start"]
