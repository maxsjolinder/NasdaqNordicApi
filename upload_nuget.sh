#!/usr/bin/env bash

mkdir -p ./nuget
dotnet pack ./NorthernLight.NasdaqNordic/NorthernLight.NasdaqNordic.csproj -c Release -o ./nuget
dotnet nuget push ./NorthernLight.NasdaqNordic/nuget/NorthernLight.NasdaqNordic.*.nupkg -s https://api.nuget.org/v3/index.json -k $NUGET_API_KEY