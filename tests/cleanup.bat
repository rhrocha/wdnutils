@ECHO OFF
PROMPT $

REM ========================================================================
ECHO ### Clean up WDNUtils.Common

IF EXIST "..\src\WDNUtils.Common\WDNUtils.Common.csproj.user" DEL /A /F /Q "..\src\WDNUtils.Common\WDNUtils.Common.csproj.user"
IF EXIST "..\src\WDNUtils.Common\bin" DEL /A /F /S /Q "..\src\WDNUtils.Common\bin"
IF EXIST "..\src\WDNUtils.Common\bin" RMDIR /S /Q "..\src\WDNUtils.Common\bin"
IF EXIST "..\src\WDNUtils.Common\obj" DEL /A /F /S /Q "..\src\WDNUtils.Common\obj"
IF EXIST "..\src\WDNUtils.Common\obj" RMDIR /S /Q "..\src\WDNUtils.Common\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Oracle.Core

IF EXIST "..\src\WDNUtils.Oracle.Core\WDNUtils.Oracle.Core.csproj.user" DEL /A /F /Q "..\src\WDNUtils.Oracle.Core\WDNUtils.Oracle.Core.csproj.user"
IF EXIST "..\src\WDNUtils.Oracle.Core\bin" DEL /A /F /S /Q "..\src\WDNUtils.Oracle.Core\bin"
IF EXIST "..\src\WDNUtils.Oracle.Core\bin" RMDIR /S /Q "..\src\WDNUtils.Oracle.Core\bin"
IF EXIST "..\src\WDNUtils.Oracle.Core\obj" DEL /A /F /S /Q "..\src\WDNUtils.Oracle.Core\obj"
IF EXIST "..\src\WDNUtils.Oracle.Core\obj" RMDIR /S /Q "..\src\WDNUtils.Oracle.Core\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Oracle.Win32

IF EXIST "..\src\WDNUtils.Oracle.Win32\WDNUtils.Oracle.Win32.csproj.user" DEL /A /F /Q "..\src\WDNUtils.Oracle.Win32\WDNUtils.Oracle.Win32.csproj.user"
IF EXIST "..\src\WDNUtils.Oracle.Win32\bin" DEL /A /F /S /Q "..\src\WDNUtils.Oracle.Win32\bin"
IF EXIST "..\src\WDNUtils.Oracle.Win32\bin" RMDIR /S /Q "..\src\WDNUtils.Oracle.Win32\bin"
IF EXIST "..\src\WDNUtils.Oracle.Win32\obj" DEL /A /F /S /Q "..\src\WDNUtils.Oracle.Win32\obj"
IF EXIST "..\src\WDNUtils.Oracle.Win32\obj" RMDIR /S /Q "..\src\WDNUtils.Oracle.Win32\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Common.Test

IF EXIST ".\WDNUtils.Common.Test\WDNUtils.Common.Test.csproj.user" DEL /A /F /Q ".\WDNUtils.Common.Test\WDNUtils.Common.Test.csproj.user"
IF EXIST ".\WDNUtils.Common.Test\bin" DEL /A /F /S /Q ".\WDNUtils.Common.Test\bin"
IF EXIST ".\WDNUtils.Common.Test\bin" RMDIR /S /Q ".\WDNUtils.Common.Test\bin"
IF EXIST ".\WDNUtils.Common.Test\obj" DEL /A /F /S /Q ".\WDNUtils.Common.Test\obj"
IF EXIST ".\WDNUtils.Common.Test\obj" RMDIR /S /Q ".\WDNUtils.Common.Test\obj"

REM ========================================================================
ECHO ### Clean up vistual studio generated files

IF EXIST ".\.vs\" DEL /A /F /S /Q ".\.vs"
IF EXIST ".\.vs\" RMDIR /S /Q ".\.vs"
IF EXIST ".\TestResults\" DEL /A /F /S /Q ".\TestResults"
IF EXIST ".\TestResults\" RMDIR /S /Q ".\TestResults"

IF EXIST "..\src\.vs\" DEL /A /F /S /Q "..\src\.vs"
IF EXIST "..\src\.vs\" RMDIR /S /Q "..\src\.vs"
IF EXIST "..\src\TestResults\" DEL /A /F /S /Q "..\src\TestResults"
IF EXIST "..\src\TestResults\" RMDIR /S /Q "..\src\TestResults"

ECHO ### Finished!
PAUSE
EXIT /B
