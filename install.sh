#!/bin/bash

xbuild src/CrackerJac.csproj
cp src/bin/Debug/CrackerJac.exe /usr/bin/CrackerJac.exe
cp CrackerJac.sh /usr/bin/CrackerJac
chmod +x /usr/bin/CrackerJac
