@echo off
cls
:: заголовок окна
Title "MY CUSTOM WIN SERVICE FolderWatcherService"

Echo Stopping FolderWatcherService...
net stop FolderWatcherService
echo errorlevel %errorlevel%
pause


Echo Starting delete FolderWatcherService with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "/u" "%~d0%~p0\FolderWatcherService\bin\Debug\FolderWatcherService.exe"
:: если errorlevel = 0 то ОК, любой другой ошибка
echo errorlevel %errorlevel%
pause

