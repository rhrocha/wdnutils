@ECHO OFF
PROMPT $

REM ========================================================================
ECHO ### Clean up WDNUtils.Common

IF EXIST ".\WDNUtils.Common\WDNUtils.Common.csproj.user" DEL /A /F /Q ".\WDNUtils.Common\WDNUtils.Common.csproj.user"
IF EXIST ".\WDNUtils.Common\bin" DEL /A /F /S /Q ".\WDNUtils.Common\bin"
IF EXIST ".\WDNUtils.Common\bin" RMDIR /S /Q ".\WDNUtils.Common\bin"
IF EXIST ".\WDNUtils.Common\obj" DEL /A /F /S /Q ".\WDNUtils.Common\obj"
IF EXIST ".\WDNUtils.Common\obj" RMDIR /S /Q ".\WDNUtils.Common\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Oracle.Core

IF EXIST ".\WDNUtils.Oracle.Core\WDNUtils.Oracle.Core.csproj.user" DEL /A /F /Q ".\WDNUtils.Oracle.Core\WDNUtils.Oracle.Core.csproj.user"
IF EXIST ".\WDNUtils.Oracle.Core\bin" DEL /A /F /S /Q ".\WDNUtils.Oracle.Core\bin"
IF EXIST ".\WDNUtils.Oracle.Core\bin" RMDIR /S /Q ".\WDNUtils.Oracle.Core\bin"
IF EXIST ".\WDNUtils.Oracle.Core\obj" DEL /A /F /S /Q ".\WDNUtils.Oracle.Core\obj"
IF EXIST ".\WDNUtils.Oracle.Core\obj" RMDIR /S /Q ".\WDNUtils.Oracle.Core\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Oracle.Win32

IF EXIST ".\WDNUtils.Oracle.Win32\WDNUtils.Oracle.Win32.csproj.user" DEL /A /F /Q ".\WDNUtils.Oracle.Win32\WDNUtils.Oracle.Win32.csproj.user"
IF EXIST ".\WDNUtils.Oracle.Win32\bin" DEL /A /F /S /Q ".\WDNUtils.Oracle.Win32\bin"
IF EXIST ".\WDNUtils.Oracle.Win32\bin" RMDIR /S /Q ".\WDNUtils.Oracle.Win32\bin"
IF EXIST ".\WDNUtils.Oracle.Win32\obj" DEL /A /F /S /Q ".\WDNUtils.Oracle.Win32\obj"
IF EXIST ".\WDNUtils.Oracle.Win32\obj" RMDIR /S /Q ".\WDNUtils.Oracle.Win32\obj"

REM ========================================================================
ECHO ### Clean up vistual studio generated files

IF EXIST ".\.vs\" DEL /A /F /S /Q ".\.vs"
IF EXIST ".\.vs\" RMDIR /S /Q ".\.vs"
IF EXIST ".\TestResults\" DEL /A /F /S /Q ".\TestResults"
IF EXIST ".\TestResults\" RMDIR /S /Q ".\TestResults"

ECHO ### Finished!
PAUSE
EXIT /B
