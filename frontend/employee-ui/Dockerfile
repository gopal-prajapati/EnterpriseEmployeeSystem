# ---------- Build Stage ----------
FROM node:22-alpine AS build

WORKDIR /app

# Copy package files first for better caching
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY . .

# Build Angular app
RUN npm run build -- --configuration production

# ---------- Runtime Stage ----------
FROM nginx:alpine

# Copy Angular build output to Nginx html folder
COPY nginx.conf /etc/nginx/nginx.conf

COPY --from=build /app/dist/employee-ui/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
