
@echo off

:: Run Docker Compose with --build, --detach, and --remove-orphans
docker-compose up --build --detach --remove-orphans

:: Check if Docker Compose command succeeded
if %ERRORLEVEL% neq 0 (
    echo Docker Compose failed to start. Check your configuration and try again.
    exit /b 1
)

echo Docker Compose started successfully.
pause
