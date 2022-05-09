@echo off
setlocal

cd "%~dp0"

xcopy dist\ bld\dist\ /E

For %%a in (
"XwaAlliance\Alliance\bin\Release\net48\*.exe"
"XwaAlliance\Alliance\bin\Release\net48\*.exe.config"
"XwaAlliance\Alliance\bin\Release\net48\*.jpg"
) do (
xcopy /s /d "%%~a" bld\dist\
)
