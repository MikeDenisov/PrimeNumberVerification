.DEFAULT_GOAL := build
.PHONY: build publish clean

build:
	dotnet build

publish:
	dotnet publish -c Release -p:PublishSingleFile=true --self-contained true -o ./publish

clean:
	dotnet clean