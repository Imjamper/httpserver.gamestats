@echo off
"Tools\nuget.exe" restore "Kontur.GameStats.sln" 
"Tools\msbuild.exe" "Kontur.GameStats.sln" /t:Build /p:Configuration=Release
