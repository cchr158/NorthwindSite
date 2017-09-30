REM MSBuild.exe _.sln /t:clean /m

REM pause

SET EnableNuGetPackageRestore=true

MSBuild.exe Callans_site.sln /t:clean

rmdir /s /q Callans_site.sln\bin
rmdir /s /q Callans_site.sln\obj

pause
