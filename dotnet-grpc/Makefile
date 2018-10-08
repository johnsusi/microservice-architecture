
all:
	@echo
	@echo "make [help]                  - This help"
	@echo
	@echo "-- Tasks --"
	@echo "make build                   - Build microservices
	@echo "make clean                   - Clean"
	@echo "make certs                   - Create certificates"
	@echo "make run                     - Run web app"
	@echo
	@echo "See README.md for example of how to start."
	@echo

help: all
.PHONE: build
build:
	@docker-compose build

.PHONY: certs
certs:
	@openssl req \
		-x509 \
		-newkey rsa:4096 \
		-keyout server/key.pem \
		-out server/cert.pem \
		-days 365 \
		-nodes \
		-subj '/CN=server'
	@cp server/cert.pem client/roots.pem

.PHONY: clean
clean:
	@dotnet clean
	@rm -Rf dist/*

.PHONY: run
run:
	@docker-compose up -d
	@docker-compose logs client
	@docker-compose down
