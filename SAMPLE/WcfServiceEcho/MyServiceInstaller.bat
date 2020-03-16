@echo off
cls
:: заголовок окна
Title "MY CUSTOM WCFWindowsMessageCollectorServiceSample"

Echo Starting install MathWinService with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "%~d0%~p0\WcfEchoServiceHost\bin\Debug\WcfEchoServiceHost.exe"

:: если errorlevel = 0 то ОК, любой другой нет ок =))
echo errorlevel %errorlevel%
pause

Echo Starting WCFWindowsMessageCollectorServiceSample...
net start WCFWindowsMessageCollectorServiceSample
echo errorlevel %errorlevel%
pause