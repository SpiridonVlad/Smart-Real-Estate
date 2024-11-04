@echo off
echo Building Docker image with name: realestate

REM Build the Docker image
docker build -t realestate .

REM Check if the build was successful
if %errorlevel% neq 0 (
    echo Docker build failed.
    exit /b %errorlevel%
)

echo Running Docker container on port 8080
docker run -d -p 8080:80 --name realestate_container realestate

REM Check if the container started successfully
if %errorlevel% neq 0 (
    echo Docker run failed. The container might already be running.
    exit /b %errorlevel%
)

echo Deployment complete. The application is available at http://localhost:8080
