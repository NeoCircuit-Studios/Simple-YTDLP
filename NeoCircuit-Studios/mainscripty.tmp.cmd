:: main script for Simple-YTDLP, downloads YT-DLP and ffmpeg and runs it.
:: Windows
:: dir [root]/NeoCircuit-Studios && dir [root]/NeoCircuit-Studios/tools
:: mainscripty.tmp.cmd

@echo off
setlocal enabledelayedexpansion


set "YTDLPappLINK=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/yt-dlp.exe"
set "ffmpegappLINK=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/ffmpeg.exe"
set "ffplayappLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/ffplay.exe"
set "ffprobeappLink=https://raw.githubusercontent.com/NeoCircuit-Studios/Simple-YTDLP/main/NeoCircuit-Studios/tools/ffprobe.exe"

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

echo "Loading.."
echo.

echo "Checking for updates..."

cd ..

::~/..

del /f /q "Simple-YTDLP.cmd" 

if exist "Simple-YTDLP.cmd" (
)
else (
    echo.
    echo "ERROR 20"
    echo.
    exit /b 1
)

curl -s -o "Simple-YTDLP.cmd" "%updatelink%"
if exist "Simple-YTDLP.cmd" (
    echo.
    echo "Update complete..."
    echo.
) else (
    echo.
    echo "Failed to update Simple-YTDLP.cmd"
    echo "ERROR 2"
    exit /b 1
)

cd "NeoCircuit-Studios"

::dir [root]/NeoCircuit-Studios/

echo "Checking for tools..."

if not exist "tools" (
    mkdir "tools"
)

cd "tools"

::dir [root]/NeoCircuit-Studios/tools/

if not exist "yt-dlp.exe" (
    echo.
    echo "Downloading yt-dlp..."
    echo.
    curl -L -o "yt-dlp.exe" "%YTDLPappLINK%"
) 

if not exist "ffmpeg.exe" (
    echo.
    echo "Downloading ffmpeg..."
    echo.
    curl -L -o "ffmpeg.exe" "%ffmpegappLINK%"
)

if not exist "ffplay.exe" (
    echo.
    echo "Downloading ffplay..."
    echo.
    curl -L -o "ffplay.exe" "%ffplayappLink%"
) 

if not exist "ffprobe.exe" (
    echo.
    echo "Downloading ffprobe..."
    echo.
    curl -L -o "ffprobe.exe" "%ffprobeappLink%"
)

if not exist "LICENSE-yt-dlp.txt" (
    echo.
    echo "Downloading LICENSE-yt-dlp.txt..."
    echo.
    curl -L -o "LICENSE-yt-dlp.txt" "%LICENSE_yt_dlplink%"
)

if not exist "LICENSE-ffmpeg.txt" (
    echo.
    echo "Downloading LICENSE-ffmpeg.txt..."
    echo.
    curl -L -o "LICENSE-ffmpeg.txt" "%LICENSE_ffmpeglink%"
)

echo.

echo "Validating All Tools..."
echo.
timeout /t 2 >nul


if exist "yt-dlp.exe" (
    echo "10%"
)
else (
    echo "yt-dlp.exe not found!"
    echo "ERROR 3"
    exit /b 1
)

if exist "ffmpeg.exe" (
    echo "40%"
)
else (
    echo "ffmpeg.exe not found!"
    echo "ERROR 4"
    exit /b 1
)

if exist "ffplay.exe" (
    echo "80%"
)
else (
    echo "ffplay.exe not found!"
    echo "ERROR 5"
    exit /b 1
)

if exist "ffprobe.exe" (
    echo "90%"
)
else (
    echo "ffprobe.exe not found!"
    echo "ERROR 6"
    exit /b 1
)

echo "100%"

echo.

:CheckDir
if exist "%outputdir%" (
    set /a outputdir+=1
    goto CheckDir
)
mkdir "%outputdir%"


echo "All tools are downloaded and ready to use..."
echo.
timeout /t 2 >nul
:: clear the shitty screen
cls

color 2

set /p usrlink="Enter Your Public YouTube Playlist Link: "

pause

echo.

set /p mp3="Do you want to download the playlist as MP3? (y/n): (if no then it will be MP4)"

if /i "%mp3%"=="y" (
    echo.
    echo "Downloading playlist as MP3..."
    echo.
    start "" "yt-dlp.exe" -x --audio-format mp3 "%usrlink%" -o "%outputdir%/%(title)s.%(ext)s"
) else (
    echo.
    echo "Downloading playlist as MP4..."
    echo.
    start "" "yt-dlp.exe" "%usrlink%" -o "%outputdir%/%(title)s.%(ext)s"
)

exit /b 0