@echo off
cls
:: заголовок окна
Title "MY CUSTOM FOLDER MONOTORING SERVICE"

Echo Sopping Services1...
net stop Service1
echo errorlevel %errorlevel%
pause


Echo Starting delete services with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "/u" "%~d0%~p0\MonitoringFolderSevice\bin\Debug\MonitoringFolderSevice.exe"
:: если errorlevel = 0 то ОК, любой другой нет ок =))
echo errorlevel %errorlevel%
pause

