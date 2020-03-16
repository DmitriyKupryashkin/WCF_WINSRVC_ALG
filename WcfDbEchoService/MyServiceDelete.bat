@echo off
cls
:: заголовок окна
Title "MY CUSTOM WCF SERVICE WcfDbEchoService"

Echo Stopping WcfDbEchoService...
net stop WcfDbEchoService
echo errorlevel %errorlevel%
pause


Echo Starting delete WcfDbEchoService with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "/u" "%~d0%~p0\WcfDbEchoServiceHost\bin\Debug\WcfDbEchoServiceHost.exe"
:: если errorlevel = 0 то ОК, любой другой ошибка
echo errorlevel %errorlevel%
pause

