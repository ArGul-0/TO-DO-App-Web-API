FROM nginx:1.27-alpine

RUN apk add --no-cache openssl && \
    mkdir -p /etc/nginx/ssl && \
    openssl req -x509 -newkey rsa:4096 \
        -keyout /etc/nginx/ssl/key.pem \
        -out /etc/nginx/ssl/cert.pem \
        -days 3650 -nodes -subj "/CN=localhost"

RUN mkdir -p /etc/nginx/conf.d

COPY default.conf /etc/nginx/conf.d/default.conf