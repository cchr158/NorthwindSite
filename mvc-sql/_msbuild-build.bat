REM MSBuild.exe _.sln /t:clean /m

REM pause

SET EnableNuGetPackageRestore=true

MSBuild.exe Callans_site.sln /t:build /m

pause
