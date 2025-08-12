:: this will download the main script to always be up-to-date.
:: Windows
:: dir [root]/
:: Simple-YTDLP.cmd


@echo off

set "versionLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/version.guust"
set "scriptLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTd/main/NeoCircuit-Studios/mainscripty.tmp.cmd"

cd /d "%~dp0"

color 1

if exist "NeoCircuit-Studios" (
    echo.
    echo "Loading.."
    echo.
) else (
    mkdir "NeoCircuit-Studios"

    echo. 
    echo @@@@@@@@@@@@@@@@@@@@
    echo       Made By:
    echo  NeoCircuit-Studios
    echo         2025
    echo @@@@@@@@@@@@@@@@@@@@
    echo.
)

timeout /t 3 >nul


echo.
echo "Updating Simple-YTDLP..."
echo.

cd "NeoCircuit-Studios"

if exist "version.guust" (
    type "version.guust" 
) else (
    curl -L -o "version.guust" "%versionLink%"
    type "version.guust"
)

:: "" is for if the path has spaces at the start
if exist "mainscripty.tmp.cmd" (
    start "" "mainscripty.tmp.cmd" 
) else (
    curl -L -o "mainscripty.tmp.cmd" "%scriptLink%"
    timeout /t 3 >nul
    if exist "mainscripty.tmp.cmd" (
        start "" "mainscripty.tmp.cmd"
    ) else (
        echo.
        echo "Huh could not download the files..."
        echo "ERROR 1"
        exit /b 1
    )
)