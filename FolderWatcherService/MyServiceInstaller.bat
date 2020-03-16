@echo off
cls
:: заголовок окна
Title "MY CUSTOM Win SERVICE FolderWatcherService"

Echo Starting install FolderWatcherService with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "%~d0%~p0\FolderWatcherService\bin\Debug\FolderWatcherService.exe"
:: если errorlevel = 0 то ОК, любой другой ошибка
echo errorlevel %errorlevel%
pause

Echo Starting FolderWatcherService...
net start FolderWatcherService
echo errorlevel %errorlevel%
pause