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
ECHO ### Clean up WDNUtils.DBOracle

IF EXIST "..\src\WDNUtils.DBOracle\WDNUtils.DBOracle.csproj.user" DEL /A /F /Q "..\src\WDNUtils.DBOracle\WDNUtils.DBOracle.csproj.user"
IF EXIST "..\src\WDNUtils.DBOracle\bin" DEL /A /F /S /Q "..\src\WDNUtils.DBOracle\bin"
IF EXIST "..\src\WDNUtils.DBOracle\bin" RMDIR /S /Q "..\src\WDNUtils.DBOracle\bin"
IF EXIST "..\src\WDNUtils.DBOracle\obj" DEL /A /F /S /Q "..\src\WDNUtils.DBOracle\obj"
IF EXIST "..\src\WDNUtils.DBOracle\obj" RMDIR /S /Q "..\src\WDNUtils.DBOracle\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.DBSqlServer

IF EXIST "..\src\WDNUtils.DBSqlServer\WDNUtils.DBSqlServer.csproj.user" DEL /A /F /Q "..\src\WDNUtils.DBSqlServer\WDNUtils.DBSqlServer.csproj.user"
IF EXIST "..\src\WDNUtils.DBSqlServer\bin" DEL /A /F /S /Q "..\src\WDNUtils.DBSqlServer\bin"
IF EXIST "..\src\WDNUtils.DBSqlServer\bin" RMDIR /S /Q "..\src\WDNUtils.DBSqlServer\bin"
IF EXIST "..\src\WDNUtils.DBSqlServer\obj" DEL /A /F /S /Q "..\src\WDNUtils.DBSqlServer\obj"
IF EXIST "..\src\WDNUtils.DBSqlServer\obj" RMDIR /S /Q "..\src\WDNUtils.DBSqlServer\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Win32

IF EXIST "..\src\WDNUtils.Win32\WDNUtils.Win32.csproj.user" DEL /A /F /Q "..\src\WDNUtils.Win32\WDNUtils.Win32.csproj.user"
IF EXIST "..\src\WDNUtils.Win32\bin" DEL /A /F /S /Q "..\src\WDNUtils.Win32\bin"
IF EXIST "..\src\WDNUtils.Win32\bin" RMDIR /S /Q "..\src\WDNUtils.Win32\bin"
IF EXIST "..\src\WDNUtils.Win32\obj" DEL /A /F /S /Q "..\src\WDNUtils.Win32\obj"
IF EXIST "..\src\WDNUtils.Win32\obj" RMDIR /S /Q "..\src\WDNUtils.Win32\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Common.Test

IF EXIST ".\WDNUtils.Common.Test\WDNUtils.Common.Test.csproj.user" DEL /A /F /Q ".\WDNUtils.Common.Test\WDNUtils.Common.Test.csproj.user"
IF EXIST ".\WDNUtils.Common.Test\bin" DEL /A /F /S /Q ".\WDNUtils.Common.Test\bin"
IF EXIST ".\WDNUtils.Common.Test\bin" RMDIR /S /Q ".\WDNUtils.Common.Test\bin"
IF EXIST ".\WDNUtils.Common.Test\obj" DEL /A /F /S /Q ".\WDNUtils.Common.Test\obj"
IF EXIST ".\WDNUtils.Common.Test\obj" RMDIR /S /Q ".\WDNUtils.Common.Test\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.DBOracle.Test

IF EXIST ".\WDNUtils.DBOracle.Test\WDNUtils.DBOracle.Test.csproj.user" DEL /A /F /Q ".\WDNUtils.DBOracle.Test\WDNUtils.DBOracle.Test.csproj.user"
IF EXIST ".\WDNUtils.DBOracle.Test\bin" DEL /A /F /S /Q ".\WDNUtils.DBOracle.Test\bin"
IF EXIST ".\WDNUtils.DBOracle.Test\bin" RMDIR /S /Q ".\WDNUtils.DBOracle.Test\bin"
IF EXIST ".\WDNUtils.DBOracle.Test\obj" DEL /A /F /S /Q ".\WDNUtils.DBOracle.Test\obj"
IF EXIST ".\WDNUtils.DBOracle.Test\obj" RMDIR /S /Q ".\WDNUtils.DBOracle.Test\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.DBSqlServer.Test

IF EXIST ".\WDNUtils.DBSqlServer.Test\WDNUtils.DBSqlServer.Test.csproj.user" DEL /A /F /Q ".\WDNUtils.DBSqlServer.Test\WDNUtils.DBSqlServer.Test.csproj.user"
IF EXIST ".\WDNUtils.DBSqlServer.Test\bin" DEL /A /F /S /Q ".\WDNUtils.DBSqlServer.Test\bin"
IF EXIST ".\WDNUtils.DBSqlServer.Test\bin" RMDIR /S /Q ".\WDNUtils.DBSqlServer.Test\bin"
IF EXIST ".\WDNUtils.DBSqlServer.Test\obj" DEL /A /F /S /Q ".\WDNUtils.DBSqlServer.Test\obj"
IF EXIST ".\WDNUtils.DBSqlServer.Test\obj" RMDIR /S /Q ".\WDNUtils.DBSqlServer.Test\obj"

REM ========================================================================
ECHO ### Clean up WDNUtils.Win32.Test

IF EXIST ".\WDNUtils.Win32.Test\WDNUtils.Win32.Test.csproj.user" DEL /A /F /Q ".\WDNUtils.Win32.Test\WDNUtils.Win32.Test.csproj.user"
IF EXIST ".\WDNUtils.Win32.Test\bin" DEL /A /F /S /Q ".\WDNUtils.Win32.Test\bin"
IF EXIST ".\WDNUtils.Win32.Test\bin" RMDIR /S /Q ".\WDNUtils.Win32.Test\bin"
IF EXIST ".\WDNUtils.Win32.Test\obj" DEL /A /F /S /Q ".\WDNUtils.Win32.Test\obj"
IF EXIST ".\WDNUtils.Win32.Test\obj" RMDIR /S /Q ".\WDNUtils.Win32.Test\obj"

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
