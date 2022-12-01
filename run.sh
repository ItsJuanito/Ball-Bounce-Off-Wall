#!/bin/bash

#Author: Juan Zaragoza

#Program name: Assignment 4



mcs -target:library BounceUI.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:BounceUI.dll


mcs BounceMain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:BounceUI.dll -out:Run.exe


./Run.exe
