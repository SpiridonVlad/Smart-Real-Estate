@echo off

:: Check if Java is installed
java -version >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Java is not installed. Starting the provided Java17 executable...
    start "" "C:\Facultate\.NET\Smart-Real-Estate\SonarQube\Java17\Java17.exe"
    timeout /t 10 >nul  :: Wait for 10 seconds to ensure Java starts properly (adjust if needed)
) else (
    echo Java is already installed and available in PATH.
)

:: Start the SonarQube server
echo Starting SonarQube...
start "" "C:\Facultate\.NET\Smart-Real-Estate\SonarQube\sonarqube-10.7.0.96327\bin\windows-x86-64\StartSonar.bat"

:: Pause to keep the console open if needed (optional)
pause
