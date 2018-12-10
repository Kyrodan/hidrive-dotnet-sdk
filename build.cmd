@echo off
set zip="tools\7za.exe"

:cleanup
dotnet clean -c Release
if not exist build mkdir build
del /s /f /q build\*

if not exist build\bin mkdir build\bin
if not exist build\dist mkdir build\dist

:build
dotnet build -c Release
if %errorlevel% NEQ 0 goto error

:test
dotnet test src\Kyrodan.HiDrive.Tests\Kyrodan.HiDrive.Tests.csproj -c Release
rem > build\testresults.txt
if %errorlevel% NEQ 0 goto error


:package
dotnet pack src\Kyrodan.HiDrive\Kyrodan.HiDrive.csproj --no-build -c Release -o ..\..\build\dist

xcopy src\Kyrodan.HiDrive\bin\Release\netstandard1.1\*.* build\bin\netstandard1.1\
xcopy src\Kyrodan.HiDrive\bin\Release\netstandard2.0\*.* build\bin\netstandard2.0\
rem del build\bin\*.pdb build\bin\*.xml 
%zip% a -tzip build\dist\Kyrodan.HiDriveSDK.zip .\build\bin\*



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
