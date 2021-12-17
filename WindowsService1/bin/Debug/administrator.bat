@echo off
::ENTER YOUR CODE BELOW::   
REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Installing Win Service...
echo ---------------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil /i "%~dp0WindowsService1.exe"
net start MyWindowsService
echo ---------------------------------------------------
::END OF YOUR CODE::
echo.
echo...Script Complete....