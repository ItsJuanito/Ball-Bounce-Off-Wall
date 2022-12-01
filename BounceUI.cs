//Author: Juan Zaragoza
//Author's email: zaragoza_9@ceu.fullerton.edu

//Program name: Assignment 4
//Programming language: C#
//Date project began: November 26, 2022
//Date project last updated: November 27, 2022

//Purpose: This program is one in a series of programs used as teaching examples in the C# programming course.  This program
//demonstrates how an elastic ball collides physically with another hard object -- in this case a wall.
//To members of the C# I think you have heard someone say that you will improve your skill in any activity the more you do
//that activity.

//Files in this project: BounceUI.cs, BounceMain.cs, run.sh

//This file name: BounceUI.cs

//Compile (& link) this file:
//mcs BounceMain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:BounceUI.dll -out:Run.exe

//Hardcopy: this source code is best viewed in 7 point monospaced font using portrait orientation.
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class Bounce : Form
{  //Declare data about the UI:
   private const int formwidth = 1000;    //Preferred size: 1600;
   private const int formheight = 800;    //Preferred size: 1200;
   private const int titleheight = 70;
   private const int graphicheight = 570;
   //private const int lowerpanelheight = formheight - titleheight - graphicheight;
   private const int lowerpanelheight = 70;

   //Declare data about the ball:
   private const double ball_radius = 8.5;
   private double ball_linear_speed_pix_per_sec;
   private double ball_linear_speed_pix_per_tic;
   private double ball_direction_x;
   private double ball_direction_y;
   private double ball_delta_x;
   private double ball_delta_y;
   private const double ball_center_initial_coord_x = (double)formwidth*0.65;
   private const double ball_center_initial_coord_y = (double)graphicheight/2.0 + titleheight;
   private double ball_center_current_coord_x;
   private double ball_center_current_coord_y;
   private double ball_upper_left_current_coord_x;
   private double ball_upper_left_current_coord_y;

   //Declare textboxes to enter coordinates
   private TextBox vx = new TextBox();
   private TextBox vy = new TextBox();
   private Label x_label = new Label();
   private Label y_label = new Label();

   //Declare textbox and label for specified
   private TextBox Speed = new TextBox();
   private Label speed_label = new Label();

   //Declare quit button
   private Button quit = new Button();
   //Declare data about the motion clock:
   private static System.Timers.Timer ball_motion_control_clock = new System.Timers.Timer();
   private const double ball_motion_control_clock_rate = 43.5;  //Units are Hz

   //Declare data about the refresh clock;
   private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
   private const double graphic_refresh_rate = 30.3;  //Units are Hz = #refreshes per second

   //Declare data about title message
   private Font style_of_message = new System.Drawing.Font("Arial",25,FontStyle.Regular);
   private String title = "Assignment 4";
   private Brush writing_tool = new SolidBrush(System.Drawing.Color.Black);
   private Point title_location = new Point(formwidth/2-15,10);

   //Declare buttons: there will probably be only one button
   private Button start_button = new Button();
   private Point start_location = new Point(formwidth/10,titleheight+graphicheight+6);

   public Bounce()   //The constructor of this class
   {  //Set the title of this form.
      Text = "Assignment 4";
      System.Console.WriteLine("formwidth = {0}. formheight = {1}.",formwidth,formheight);
      //Set the initial size of this form
      Size = new Size(formwidth,formheight);
      //Set the background color of this form
      BackColor = Color.Green;

      //Text input attributes
      vx.Size = new Size(40,40);
      vx.Text="";
      vx.Location = new Point(450, titleheight+graphicheight+50);
      Controls.Add(vx);
      vy.Size = new Size(40,40);
      vy.Text="";
      vy.Location = new Point(500, titleheight+graphicheight+50);
      Controls.Add(vy);

      //Text label attributes
      x_label.Text = "X";
      x_label.Size = new Size(35,30);
      x_label.Location = new Point(450, titleheight+graphicheight+6);
      x_label.Font = new Font("Arial",12,FontStyle.Bold);
      x_label.TextAlign = ContentAlignment.MiddleCenter;
      Controls.Add(x_label);

      y_label.Text = "Y";
      y_label.Size = new Size(35,30);
      y_label.Location = new Point(500, titleheight+graphicheight+6);
      y_label.Font = new Font("Arial",12,FontStyle.Bold);
      y_label.TextAlign = ContentAlignment.MiddleCenter;
      Controls.Add(y_label);

      //TextBox and Label attributes for speed
      Speed.Size = new Size(50,30);
      Speed.Text="";
      Speed.Location = new Point(600, titleheight+graphicheight+50);
      Controls.Add(Speed);

      speed_label.Text = "Speed";
      speed_label.Size = new Size(50,30);
      speed_label.Location = new Point(600, titleheight+graphicheight+6);
      speed_label.Font = new Font("Arial",12,FontStyle.Bold);
      speed_label.TextAlign = ContentAlignment.MiddleCenter;
      Controls.Add(speed_label);

      //Set up the quit button and attach it to the controller panel
      quit.Size = new Size(90,50);
      quit.Text = "Quit";
      //quit.Location = new Point(control_panel.Width*9/10,control_panel.Height/4);
      quit.Location = new Point(750,titleheight+graphicheight+6);
      quit.BackColor = Color.Red;
      quit.ForeColor = ForeColor = Color.White;
      quit.TextAlign = ContentAlignment.MiddleCenter;
      quit.Click += new EventHandler(Close_window);
      Controls.Add(quit);

      //Compute fixed values needed for motion in a straight line; some trigonometry is required.
      //To understand why it works you should draw some right triangles, and the math will be more clear.
      ball_linear_speed_pix_per_tic = ball_linear_speed_pix_per_sec/ball_motion_control_clock_rate;
      double hypotenuse_squared = ball_direction_x*ball_direction_x + ball_direction_y*ball_direction_y;
      double hypotenuse = System.Math.Sqrt(hypotenuse_squared);
      ball_delta_x = ball_linear_speed_pix_per_tic * ball_direction_x / hypotenuse;
      ball_delta_y = ball_linear_speed_pix_per_tic * ball_direction_y / hypotenuse;

      //Set starting values for the ball
      ball_center_current_coord_x = ball_center_initial_coord_x;
      ball_center_current_coord_y = ball_center_initial_coord_y;
      System.Console.WriteLine("Initial coordinates: ball_center_current_coord_x = {0}. ball_center_current_coord_y = {1}.",
                               ball_center_current_coord_x,ball_center_current_coord_y);

      //Set up the motion clock.  This clock controls the rate of update of the coordinates of the ball.
      ball_motion_control_clock.Enabled = false;
      //Assign a handler to this clock.
      ball_motion_control_clock.Elapsed += new ElapsedEventHandler(Update_ball_position);

      //Set up the refresh clock.
      graphic_area_refresh_clock.Enabled = false;  //Initially the clock controlling the rate of updating the display is stopped.
      //Assign a handler to this clock.
      graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_display);

      //Set properties of the button (or maybe buttons)
      start_button.Text = "Start";
      start_button.Size = new Size(90,50);
      start_button.Location = start_location;
      start_button.BackColor = Color.LimeGreen;
      Controls.Add(start_button);
      start_button.Click += new EventHandler(All_systems_go);

   }//End of constructor

   protected override void OnPaint(PaintEventArgs ee)
   {  Graphics graph = ee.Graphics;
      graph.FillRectangle(Brushes.Pink,0,0,formwidth,titleheight);
      graph.FillRectangle(Brushes.Yellow,0,titleheight+graphicheight,formwidth,titleheight+graphicheight);
      graph.DrawString(title,style_of_message,writing_tool,title_location);
      ball_upper_left_current_coord_x = ball_center_current_coord_x - ball_radius;
      ball_upper_left_current_coord_y = ball_center_current_coord_y - ball_radius;
      graph.FillEllipse(Brushes.Red,(int)ball_upper_left_current_coord_x,(int)ball_upper_left_current_coord_y,
                        (float)(2.0*ball_radius),(float)(2.0*ball_radius));
      //The next statement calls the method with the same name located in the super class.
      base.OnPaint(ee);
   }

   protected void All_systems_go(Object sender,EventArgs events)
   {//The refreshclock is started.
     ball_direction_x = int.Parse(vx.Text);
     ball_direction_y = int.Parse(vy.Text);

     //reset ball Direction
     ball_linear_speed_pix_per_tic = ball_linear_speed_pix_per_sec/ball_motion_control_clock_rate;
     double hypotenuse_squared = ball_direction_x*ball_direction_x + ball_direction_y*ball_direction_y;
     double hypotenuse = System.Math.Sqrt(hypotenuse_squared);
     ball_delta_x = ball_linear_speed_pix_per_tic * ball_direction_x / hypotenuse;
     ball_delta_y = ball_linear_speed_pix_per_tic * ball_direction_y / hypotenuse;

     ball_linear_speed_pix_per_sec = Double.Parse(Speed.Text);
    Start_graphic_clock(graphic_refresh_rate);
    //The motion clock is started.
    Start_ball_clock(ball_motion_control_clock_rate);
   }

   protected void Start_graphic_clock(double refresh_rate)
   {
       double actual_refresh_rate = 1.0;  //Minimum refresh rate is 1 Hz to avoid a potential division by a number close to zero
       double elapsed_time_between_tics;
       if(refresh_rate > actual_refresh_rate)
           actual_refresh_rate = refresh_rate;
       elapsed_time_between_tics = 1000.0/actual_refresh_rate;  //elapsedtimebetweentics has units milliseconds.
       graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_tics);
       graphic_area_refresh_clock.Enabled = true;  //Start clock ticking.
   }

   protected void Start_ball_clock(double update_rate)
   {   double elapsed_time_between_ball_moves;
       if(update_rate < 1.0) update_rate = 1.0;  //This program does not allow updates slower than 1 Hz.
       elapsed_time_between_ball_moves = 1000.0/update_rate;  //1000.0ms = 1second.
       //The variable elapsed_time_between_ball_moves has units "milliseconds".
       ball_motion_control_clock.Interval = (int)System.Math.Round(elapsed_time_between_ball_moves);
       ball_motion_control_clock.Enabled = true;   //Start clock ticking.
   }

   protected void Update_display(System.Object sender, ElapsedEventArgs evt)
   {  Invalidate();  //This creates an artificial event so that the graphic area will repaint itself.
      //System.Console.WriteLine("The motion clock ticked and the time is {0}", evt.SignalTime);  //Debug statement; remove it later.
      if(!ball_motion_control_clock.Enabled)
          {graphic_area_refresh_clock.Enabled = false;
           System.Console.WriteLine("The graphical area is no longer refreshing.  You may close the window.");
          }
   }

   //Declare the handler method of the Quit button.
   protected void Close_window(System.Object sender, EventArgs even)
   {System.Console.WriteLine("This program will close the UI window and stop execution.");
    Close();
   }//End of event handler Close_window

   protected void Update_ball_position(System.Object sender, ElapsedEventArgs evt)
   {  ball_center_current_coord_x += ball_delta_x;
      ball_center_current_coord_y -= ball_delta_y;  //The minus sign is due to the upside down nature of the C# system.
      //System.Console.WriteLine("The motion clock ticked and the time is {0}", evt.SignalTime);//Debug statement; remove later.
      //Determine if the ball has made a collision with the right wall.
      if((int)System.Math.Round(ball_center_current_coord_x + ball_radius) >= formwidth - 5)
             ball_delta_x = -ball_delta_x;
      // down
      if ((int)System.Math.Round(ball_center_current_coord_y + ball_radius) >= 640) {
        ball_delta_y = -ball_delta_y;
      }
      // left
      if ((int)System.Math.Round(ball_center_current_coord_x + ball_radius) <= 10) {
        ball_delta_x = -ball_delta_x;
      }
      // up
      if ((int)System.Math.Round(ball_center_current_coord_y + ball_radius) <= 90) {
        ball_delta_y = -ball_delta_y;
      }
      //The next statement checks to determine if the ball has traveled beyond the four boundaries.  The statement may be
      //removed after the ricochet feature has been implemented by a 223N student.
      if((int)System.Math.Round(ball_center_current_coord_y - ball_radius) >= titleheight + graphicheight)
          {ball_motion_control_clock.Enabled = false;
           graphic_area_refresh_clock.Enabled = false;
           System.Console.WriteLine("The clock controlling the ball has stopped.");
          }
   }//End of method Update_ball_position

}//End of class Bounce
