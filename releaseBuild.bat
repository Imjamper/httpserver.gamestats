@echo off
"Tools\nuget.exe" restore "Kontur.GameStats.sln" 
"Tools\msbuild.exe" "Kontur.GameStats.sln" /t:Build /p:Configuration=Release
"Tools\nUnit3\nunit3-console.exe" /work:TestResults "GL.HttpServer.UnitTests\GL.HttpServer.UnitTests.csproj"
"Tools\nUnit3\nunit3-console.exe" /work:TestResults "Kontur.GameStats.Server.UnitTests\Kontur.GameStats.Server.UnitTests.csproj"
