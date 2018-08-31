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
IF EXIST ".\TestResults\" RMDIR /S /Q ".\TestResults"

ECHO ### Finished!
PAUSE
EXIT /B
