@echo off
"Tools\nuget.exe" restore "Kontur.GameStats.sln" 
"Tools\msbuild.exe" "Kontur.GameStats.sln" /t:Build /p:Configuration=Release
"Tools\msbuild.exe" "HttpClient\HttpClient.sln" /t:Build /p:Configuration=Release
"Tools\nUnit3\nunit3-console.exe" --noresult "GL.HttpServer.UnitTests\GL.HttpServer.UnitTests.csproj" /config:Release
"Tools\nUnit3\nunit3-console.exe" --noresult "Kontur.GameStats.Server.UnitTests\Kontur.GameStats.Server.UnitTests.csproj" /config:Release
