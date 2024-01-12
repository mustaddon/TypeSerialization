dotnet build -c Release 
dotnet pack .\TypeSerialization\ -c Release -o ..\_publish
dotnet pack .\TypeSerialization.Json\ -c Release -o ..\_publish
