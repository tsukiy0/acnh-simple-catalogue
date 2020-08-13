FROM node:12-alpine

RUN apk add --update --no-cache -q bash py-pip docker
RUN pip install --no-cache-dir --upgrade -q awscli
