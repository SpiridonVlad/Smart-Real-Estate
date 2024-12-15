@echo off
echo Running git add ...
git add .

echo Running git rm -r --cached .\Smart-Real-Estate\Src\WebAPI\.sonarqube\ ...
git rm -r --cached .\Smart-Real-Estate\Src\WebAPI\.sonarqube\

echo Running git rm -r --cached .\Smart-Real-Estate\SonarQube\ ...
git rm -r --cached .\Smart-Real-Estate\SonarQube\
git rm -r --cached .\Smart-Real-Estate\Src\WebAPI\.idea\
echo Running git status ...
git status

pause
