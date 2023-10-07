.DEFAULT_GOAL := build
.PHONY: build publish clean

build:
	dotnet build

publish:
	dotnet publish ./PrimeNumber.Client/ --configuration Release --use-current-runtime --self-contained true --output ./publish /p:PublishSingleFile=true /p:DebugType=None /p:DebugSymbols=false
	dotnet publish ./PrimeNumber.Service/ --configuration Release --use-current-runtime --self-contained true --output ./publish /p:PublishSingleFile=true /p:DebugType=None /p:DebugSymbols=false

clean:
	dotnet clean