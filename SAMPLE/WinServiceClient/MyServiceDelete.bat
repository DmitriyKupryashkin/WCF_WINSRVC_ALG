@echo off
cls
:: заголовок окна
Title "MY CUSTOM MessageSenderClientSercvice"

Echo Stopping MessageSenderClientSercvice...
net stop MessageSenderClientSercvice
echo errorlevel %errorlevel%
pause


Echo Starting delete MessageSenderClientSercvice with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "/u" "%~d0%~p0\WinServiceClientLib\bin\Debug\WinServiceClientLib.exe"
:: если errorlevel = 0 то ОК, любой другой нет ок =))
echo errorlevel %errorlevel%
pause

