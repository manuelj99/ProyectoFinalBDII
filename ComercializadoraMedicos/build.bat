@echo off
echo Building ComercializadoraMedicos...
cd /d "%~dp0"

REM Try to find MSBuild
set MSBUILD_PATH=""

REM Check VS 2022
if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
)

REM Check VS 2019
if %MSBUILD_PATH%=="" (
    if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
        set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    )
)

if %MSBUILD_PATH%=="" (
    echo MSBuild not found. Please install Visual Studio.
    exit /b 1
)

echo Using MSBuild: %MSBUILD_PATH%
%MSBUILD_PATH% ComercializadoraMedicos.sln /t:Rebuild /p:Configuration=Debug /v:minimal

if %ERRORLEVEL% EQU 0 (
    echo Build successful!
) else (
    echo Build failed with errors.
)

pause
