:: main script for Simple-YTDLP, downloads YT-DLP and ffmpeg and runs it.
:: Windows
:: dir [root]/NeoCircuit-Studios && dir [root]/NeoCircuit-Studios/tools
:: mainscripty.tmp.cmd

@echo off
setlocal enabledelayedexpansion


set "YTDLPappLINK=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/bin.6.tmp.zip"
set "ffmpegappLINK=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/bin.1.tmp.zip"
set "ffplayappLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/bin.2.tmp.zip"
set "ffprobeappLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/bin.3.tmp.zip"

set "LICENSE_yt_dlplink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/LICENSE-yt-dlp.txt"
set "LICENSE_ffmpeglink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/LICENSE-ffmpeg.txt"

set "updatelink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/Simple-YTDLP.cmd"

set "outputdir=20251000"

echo. 
echo @@@@@@@@@@@@@@@@@@@@
echo       Made By:
echo  NeoCircuit-Studios
echo         2025
echo @@@@@@@@@@@@@@@@@@@@
echo.

echo This script auto-updates.
echo.

echo Loading..
echo.

echo Checking for updates...

cd ..

:: dir [ROOT]

curl -L "%updatelink%" -o "Simple-YTDLP.cmd"

if exist "Simple-YTDLP.cmd" (
    echo.
    echo Update complete...
    echo.
) else (
    echo.
    echo Failed to update Simple-YTDLP.cmd
    echo ERROR 2
    pause
)

cd "NeoCircuit-Studios"

::[root]/NeoCircuit-Studios/

echo Checking for tools Packs...

if not exist "tools" (
    mkdir "tools"
)

cd "tools"

:: [root]/NeoCircuit-Studios/tools/

if not exist "yt-dlp.exe" (
    echo.
    echo Downloading yt-dlp...
    echo.   
    curl -L "%YTDLPappLINK%" -o "bin.6.tmp.zip"
) 

if not exist "ffmpeg.exe" (
    echo.
    echo Downloading ffmpeg...
    echo.
    curl -L "%ffmpegappLINK%" -o "bin.1.tmp.zip"
)

if not exist "ffplay.exe" (
    echo.
    echo Downloading ffplay...
    echo.
    curl -L "%ffplayappLink%" -o "bin.2.tmp.zip"
) 

if not exist "ffprobe.exe" (
    echo.
    echo Downloading ffprobe...
    echo.
    curl -L "%ffprobeappLink%" -o "bin.3.tmp.zip"
)

if not exist "LICENSE-yt-dlp.txt" (
    echo.
    echo Downloading LICENSE_1
    echo.
    curl -L "%LICENSE_yt_dlplink%" -o "LICENSE-yt-dlp.txt"
)

if not exist "LICENSE-ffmpeg.txt" (
    echo.
    echo Downloading LICENSE_2
    echo.
    curl -L "%LICENSE_ffmpeglink%" -o "LICENSE-ffmpeg.txt" 
)

echo.

echo Validating All Tools...
echo.
timeout /t 2 >nul

if exist "bin.1.tmp.zip" (
    echo 5%
    tar -xf "bin.1.tmp.zip"
    del /f "bin.1.tmp.zip"
)

if exist "bin.2.tmp.zip" (
    echo 6%
    tar -xf "bin.2.tmp.zip"
    del /f "bin.2.tmp.zip"
)

if exist "bin.3.tmp.zip" (
    echo 7%
    tar -xf "bin.3.tmp.zip"
    del /f "bin.3.tmp.zip"
)

if exist "bin.6.tmp.zip" (
    echo 8%
    tar -xf "bin.6.tmp.zip"
    del /f "bin.6.tmp.zip"
)

if exist "yt-dlp.exe" (
    echo 10%
) else (
    echo yt-dlp.exe not found!
    echo ERROR 3
    pause
)

if exist "ffmpeg.exe" (
    echo 40%
) else (
    echo ffmpeg.exe not found!
    echo ERROR 4
    pause
)

if exist "ffplay.exe" (
    echo 80%
) else (
    echo ffplay.exe not found!
    echo ERROR 5
    pause
)

if exist "ffprobe.exe" (
    echo 90%
) else (
    echo ffprobe.exe not found!
    echo ERROR 6
    pause
)

echo 100%

echo.

:CheckDir
if exist "%outputdir%" (
    set /a outputdir+=1
    goto CheckDir
)
mkdir "%outputdir%"


echo All tools are downloaded and ready to use...
echo.
timeout /t 2 >nul
:: clear the very nice echo's (not really)
cls

color 2

:retry_input
set /p usrlink="Enter Your Public YouTube Playlist Link: "
set "usrlink=%usrlink:"=%"
::If someone pastes a YouTube link in quotes, yt-dlp might misinterpret it. that would suck

cd ..

echo.

if "%usrlink%"=="" (
    echo oy.. No playlist link provided!
    goto retry_input
)

set /p mp3="Do you want to download the playlist as MP3? (y/n): (if no then it will be MP4)  " 

if /i "%mp3%"=="y" (
    echo.
    echo Downloading playlist as MP3...
    echo.
    yt-dlp.exe -x --audio-format mp3 "%usrlink%" -o "%outputdir%/%(title)s.%(ext)s"
) else (
    echo.
    echo Downloading playlist as MP4...
    echo.
    yt-dlp.exe "%usrlink%" -o "%outputdir%/%(title)s.%(ext)s"
)

exit /b 0
