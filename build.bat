@echo off
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe /nologo /target:exe /out:WslKeepAlive.exe /r:System.ServiceProcess.dll WslKeepAlive.cs
