@echo off
cls
:: заголовок окна
Title "MY CUSTOM WCFWindowsMessageCollectorServiceSample"

Echo Stopping WCFWindowsMessageCollectorServiceSample...
net stop WCFWindowsMessageCollectorServiceSample
echo errorlevel %errorlevel%
pause


Echo Starting delete WCFWindowsMessageCollectorServiceSample with InstllUtill.exe

"%SystemDrive%\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "/u" "%~d0%~p0\WcfEchoServiceHost\bin\Debug\WcfEchoServiceHost.exe"
:: если errorlevel = 0 то ОК, любой другой нет ок =))
echo errorlevel %errorlevel%
pause

