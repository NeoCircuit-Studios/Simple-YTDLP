:: this will download the main script to always be up-to-date.
:: Windows
:: dir [root]/
:: Simple-YTDLP.cmd


@echo off

set "versionLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/version.guust"
set "scriptLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/mainscripty.tmp.cmd"

cd /d "%~dp0"

echo This script auto-updates.
echo.

color 2

if exist "NeoCircuit-Studios" (
    echo.
    echo Loading..
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

cd "NeoCircuit-Studios"

type "version.guust" 

echo.
echo Updating Simple-YTDLP...
echo.

curl -L "%versionLink%" -o "version.guust"
type "version.guust"

:: "" is for if the path has spaces at the start
curl -L "%scriptLink%" -o "mainscripty.tmp.cmd"
if exist "mainscripty.tmp.cmd" (
    start "" "mainscripty.tmp.cmd" 
) else (
    curl -L "%scriptLink%" -o "mainscripty.tmp.cmd"
    timeout /t 3 >nul
    if exist "mainscripty.tmp.cmd" (
        start "" "mainscripty.tmp.cmd"
    ) else (
        echo.
        echo Huh.. could not download the needed files...
        echo ERROR 1
        pause
    )
)
