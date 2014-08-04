@echo off

nuget pack %~dp0\..\src\Cob.Umb.NavBuilder\Cob.Umb.NavBuilder.csproj -Build -OutputDirectory %~dp0

pause