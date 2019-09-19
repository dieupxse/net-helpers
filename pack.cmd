dotnet pack
key: oy2f36oe67owm2e6xnuqal72kgd6cqleffle3ut2hocdm4
dotnet build --configuration Release
dotnet nuget push Net.Helpers/bin/Release/Net.Helpers.1.0.1.nupkg -k oy2f36oe67owm2e6xnuqal72kgd6cqleffle3ut2hocdm4 -s https://api.nuget.org/v3/index.json