//****************************************************************************************************************************
//Program name: "Ricochet Ball".  This program shows a ball moving in a straight line.  When it reaches a wall it ricochets *
//off of that wall and continues its linear motion.                                                                          *
//Copyright (C) 2017  Floyd Holliday                                                                                         *
//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License  *
//version 3 as published by the Free Software Foundation.                                                                    *
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied         *
//warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.     *
//A copy of the GNU General Public License v3 is available here:  <https://www.gnu.org/licenses/>.                           *
//****************************************************************************************************************************








﻿//Ruler:=1=========2=========3=========4=========5=========6=========7=========8=========9=========0=========1=========2=========3**

//Author: Juan Zaragoza
//Author's email: zaragoza_9@csu.fullerton.edu

//Program name: Assignment 4
//Programming language: C#
//Date project began: November 26, 2022
//Date project last updated: November 27, 2022

//Purpose: This program is one in a series of programs used as teaching examples in the C# programming course.  This
//program demonstrates how an elastic ball collides physically with another hard object -- in this case a wall.

//Improvements needed:
//1.  An exit button
//2.  Real-time instantaneous display of coordinates
//3.  Ricochet capability on the north, west, and south sides.
//4.  Hyper jumping: in place of ricochet cause the ball to re-appear on the left as it exits from the right.

//Files in this project: BOunceMain.cs, BounceUI.cs, run.sh

//This file name: BounceMain.cs
//
//Compile (& link) this file:
//mcs mcs -target:library BounceUI.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:BounceUI.dll


using System;
using System.Windows.Forms;            //Needed for "Application" near the end of Main function.
public class BounceMain
{  public static void Main()
   {  System.Console.WriteLine("The assignment 4 program will begin now.");
      Bounce bounce = new Bounce();
      Application.Run(bounce);
      System.Console.WriteLine("This assignment 4 program has ended.  Bye.");
   }//End of Main function
}//End of Movingballs class
