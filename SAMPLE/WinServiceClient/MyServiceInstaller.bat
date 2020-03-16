@echo off
cls
:: заголовок окна
Title "MY CUSTOM MessageSenderClientSercvice"

Echo Starting install MessageSenderClientSercvice with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "%~d0%~p0\WinServiceClientLib\bin\Debug\WinServiceClientLib.exe"

:: если errorlevel = 0 то ОК, любой другой нет ок =))
echo errorlevel %errorlevel%
pause

Echo Starting MessageSenderClientSercvice...
net start MessageSenderClientSercvice
echo errorlevel %errorlevel%
pause