@echo off
set CONTAINER_NAME=real_estate_management_system

REM Check if the container already exists
docker ps -a --filter "name=%CONTAINER_NAME%" --format "{{.Names}}" | findstr /i "%CONTAINER_NAME%" >nul

IF %ERRORLEVEL% EQU 0 (
    echo Container %CONTAINER_NAME% already exists. Starting container...
    docker start -i %CONTAINER_NAME%
) ELSE (
    echo Container %CONTAINER_NAME% does not exist. Building and running a new container...
    docker build -t %CONTAINER_NAME% .
    docker run -d --name %CONTAINER_NAME% -p 80:80 %CONTAINER_NAME%
    echo Container %CONTAINER_NAME% is now running.
)
