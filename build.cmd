@echo off
set zip="tools\7za.exe"
set nuget="tools\NuGet.exe"
rem set msbuildenv="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsMSBuildCmd.bat"
set msbuildenv="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"

if not exist %msbuildenv% goto error
call %msbuildenv%

:cleanup
if not exist build mkdir build
del /s /f /q build\*

if not exist build\bin mkdir build\bin
if not exist build\dist mkdir build\dist

:build
%nuget% restore
msbuild Kyrodan.HiDriveSdk.sln /p:Configuration=Release /t:Clean,Build /fl /flp:logfile=build\build.log
if %errorlevel% NEQ 0 goto error

:test
vstest.console /logger:Console src\Kyrodan.HiDrive.Tests\bin\Release\Kyrodan.HiDrive.Tests.dll 
rem > build\testresults.txt
if %errorlevel% NEQ 0 goto error


:package
xcopy src\Kyrodan.HiDrive\bin\Release\*.* build\bin
rem del build\bin\*.pdb build\bin\*.xml 
%zip% a -tzip build\dist\Kyrodan.HiDriveSDK.zip .\build\bin\*
%nuget% pack src\Kyrodan.HiDrive\Kyrodan.HiDrive.csproj -Properties "Configuration=Release;Platform=AnyCPU" -OutputDirectory build\dist



:final
goto end

:error
echo !
echo !
echo !
echo Fehler im Buildvorgang
echo !
echo !
echo !


:end
pause
