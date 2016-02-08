using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;

using System.Windows.Input;

using ImageComparison;
using System.Drawing;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {

        int i = 0;
        int initialFileCount = 0;
        DateTime initialLatestDatetime = DateTime.MinValue;
      

        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=NJ-123\\SCOTT;Initial Catalog=studentdatabase;Integrated Security=True");


        byte[] convertToByte(string sourcepath)
        {
           
            FileInfo finfo = new FileInfo(sourcepath);
            long size = finfo.Length;
            FileStream fs = new FileStream(sourcepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] data = br.ReadBytes((int)size);
            return data;
        }

        public static Image bytetoimage(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image returnval = Image.FromStream(ms);
            return returnval;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;

            var files = Directory.GetFiles(textBox1.Text);

            if (i == 15)
            {
                i = 0;
                timer1.Enabled = false;
               MessageBox.Show("No Finger print entered, Please try again");
            }
            else if (files.Count() > initialFileCount)
            {
                timer1.Enabled = false;

                foreach (var file in files)
                {
                    DateTime dt = File.GetCreationTime(file);

                    if (dt > initialLatestDatetime)
                    {
                        l_putfinger.Visible = false;
                        pictureBox1.ImageLocation = file;


                        Bitmap im1 = new Bitmap(file);
                        
 


               
                                        int count = 0;int temp=0;
                                        con.Close();
                                        con.Open();

                                        SqlCommand selcmd=new SqlCommand("select * from studentdatabase.dbo.student3",con);

                                        Image im = null; int identity = 0;
                                                    SqlDataReader rdr= selcmd.ExecuteReader();
                                                    while(rdr.Read())
                                                    {

                                                        Image btm = bytetoimage((byte[])rdr["finger_image"]);
                                                        Bitmap im2 = (Bitmap)btm;
                                                        count = ImageTool.call2(im1, im2);

                                                       
                                                        if (count > temp)
                                                        {
                                                            temp = count;
                                                            im = btm;
                                                            identity = (int)rdr["Id"];
                                   
                                                        }
                                                    }
                                                    selcmd.Dispose();
                                                    rdr.Dispose();
                                                    con.Close();



                                                    if (temp > 170)
                                                    {

                                                        con.Close();
                                                        con.Open();
                                                        SqlCommand selcmd1 = new SqlCommand("select * from studentdatabase.dbo.student3 where Id=" + identity + "", con);
                                                        SqlDataReader rdr1 = selcmd1.ExecuteReader();
                                                        while (rdr1.Read())
                                                        {
                                                            l_dataenrolled.Visible = true;

                                                            l_dataenrolled.Text = "Name :- " + rdr1["name"].ToString() + " \nRoll.No :- " + rdr1["roll_no"].ToString() + "  already exist";


                                                        }

                                                        selcmd1.Dispose();
                                                        rdr1.Dispose();

                                                        con.Close();




                                                    }
                                                    else
                                                    {
                                                        con.Close();
                                                        con.Open();

                                                        DataTable dt1 = new DataTable();
                                                        SqlDataAdapter sda = new SqlDataAdapter("select * from studentdatabase.dbo.student3", con);
                                                        sda.Fill(dt1);
                                                        DataRow r = dt1.NewRow();
                                                        r[1] = t_roll.Text;
                                                        r[2] = t_name.Text;
                                                        r[3] = convertToByte(file);
                                                        dt1.Rows.Add(r);
                                                        SqlCommandBuilder cb = new SqlCommandBuilder(sda);
                                                        sda.Update(dt1);

                                                        l_dataenrolled.Visible = true;
                                                        l_dataenrolled.Text = "New Data enrolled";

                                                        con.Close();


                                                        break;
                                                    }
                    }
                }
            }

        }


        

      

       
    
        private void timer2_Tick(object sender, EventArgs e)
        {
            i++;

            var files = Directory.GetFiles(textBox1.Text);

            if (i == 15)
            {
                i = 0;
                timer2.Enabled = false;
                MessageBox.Show("No Finger print entered, Please try again");
            }
            else if (files.Count() > initialFileCount)
            {
                timer2.Enabled = false;
                var file1="";
                foreach (var file in files)
                {
                    DateTime dt = File.GetCreationTime(file);

                    if (dt > initialLatestDatetime)
                    {
                        l_putfinger.Visible = false;
                        pictureBox2.ImageLocation = file;

                        file1 = file;
                        break;
                    }
                }

                Bitmap im1 = new Bitmap(file1);
               
                int count = 0;int temp=0;
                con.Close();
                con.Open();

                SqlCommand selcmd=new SqlCommand("select * from studentdatabase.dbo.student3",con);

                Image im = null; int identity = 0;
                            SqlDataReader rdr= selcmd.ExecuteReader();
                            while(rdr.Read())
                            {

                                Image btm = bytetoimage((byte[])rdr["finger_image"]);
                                Bitmap im2 = (Bitmap)btm;
                                count = ImageTool.call2(im1, im2);

                                Bitmap b = ImageTool.call1(im1, im2);
                                pictureBox4.Visible = true;
                                pictureBox4.Image = b;

                                if (count > temp)
                                {
                                    temp = count;
                                    im = btm;
                                    identity = (int)rdr["Id"];
                                   
                                }
                            }
                            selcmd.Dispose();
                            rdr.Dispose();
                            con.Close();


                            DateTime datetime = DateTime.Now;
                            int time = datetime.Hour; int abc = 0; string mon = ""; string days = "";
                            if (datetime.Month < 10)
                               mon = "0" + datetime.Month.ToString();
                            else
                                mon = datetime.Month.ToString();

                            if (datetime.Day < 10)
                                days = "0" + datetime.Day.ToString();
                            else
                                days = datetime.Day.ToString();

                if (temp > 170)
                {
                    pictureBox3.Image = im;
                  
                    l_matched.ForeColor=(Color.Green);
                    con.Open();
                    SqlCommand selcmd1=new SqlCommand("select * from studentdatabase.dbo.student3 where Id="+identity+"",con);
                    SqlDataReader rdr1= selcmd1.ExecuteReader();
                            while (rdr1.Read())
                            {
                                l_matched.Visible = true;
                                l_matched.Text ="Name :- "+ rdr1["name"].ToString() + " \nRoll.No :- " + rdr1["roll_no"].ToString();

                              abc=(int)rdr1["Id"];
                            
                            }

                            selcmd1.Dispose();
                            rdr1.Dispose();

                            con.Close();

                            if (time >= 9 && time <= 12)
                            {
                                string sub1 = "";


                                con.Open();
                                SqlCommand selcmd3 = new SqlCommand("select * from studentdatabase.dbo.lecture1 where student_id=" + abc + " AND date1='"+datetime.Year+"-"+mon+"-"+days+"'", con);
                                SqlDataReader rdr2 = selcmd3.ExecuteReader();

                                if (rdr2.Read())
                                {
                                    MessageBox.Show("Your Attendance for First Half is already Marked");
                                    selcmd3.Dispose();
                                    rdr2.Dispose();
                                    con.Close();

                                }
                                else
                                {
                                    MessageBox.Show("Attendance of First Half of College hours will be marked ");
                                    selcmd3.Dispose();
                                    rdr2.Dispose();

                                    con.Close();
                                    con.Open();
                                    SqlCommand selcmd4 = new SqlCommand("select subject_"+datetime.DayOfWeek.ToString()+" from studentdatabase.dbo.lec_sub1 where lec_id=1", con);
                                    SqlDataReader rdr4 = selcmd4.ExecuteReader();
                                    if (rdr4.Read())
                                    {
                                        sub1 = rdr4["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                       
                                    }

                                    selcmd4.Dispose();
                                    rdr4.Dispose();
                                    con.Close();
                                    con.Open();



                                    DataTable dt1 = new DataTable();
                                    SqlDataAdapter sda1 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture1", con);
                                    sda1.Fill(dt1);
                                    DataRow r1 = dt1.NewRow();
                                    r1[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                    r1[2] = abc;
                                    r1[3] = sub1;
                                    dt1.Rows.Add(r1);
                                    SqlCommandBuilder cb1 = new SqlCommandBuilder(sda1);
                                    sda1.Update(dt1);

                                    con.Close();
                                    con.Open();

                                    SqlCommand selcmd5 = new SqlCommand("select subject_" + datetime.DayOfWeek.ToString() + " from studentdatabase.dbo.lec_sub1 where lec_id=2", con);
                                    SqlDataReader rdr5 = selcmd5.ExecuteReader();
                                    if (rdr5.Read())
                                    {

                                        sub1 = rdr5["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                      
                                    }

                                    selcmd5.Dispose();
                                    rdr5.Dispose();

                                    con.Close();
                                    con.Open();

                                    DataTable dt2 = new DataTable();
                                    SqlDataAdapter sda2 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture2", con);
                                    sda2.Fill(dt2);
                                    DataRow r2 = dt2.NewRow();
                                    r2[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                    r2[2] = abc;
                                    r2[3] = sub1;
                                    dt2.Rows.Add(r2);
                                    SqlCommandBuilder cb2 = new SqlCommandBuilder(sda2);
                                    sda2.Update(dt2);

                                    con.Close();
                                    con.Open();

                                    SqlCommand selcmd6 = new SqlCommand("select subject_" + datetime.DayOfWeek.ToString() + " from studentdatabase.dbo.lec_sub1 where lec_id=3", con);
                                    SqlDataReader rdr6 = selcmd6.ExecuteReader();
                                    if (rdr6.Read())
                                    {
                                        sub1 = rdr6["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                       
                                    }

                                    selcmd6.Dispose();
                                    rdr6.Dispose();
                                    con.Close();
                                    con.Open();


                                    DataTable dt3 = new DataTable();
                                    SqlDataAdapter sda3 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture3", con);
                                    sda3.Fill(dt3);
                                    DataRow r3 = dt3.NewRow();
                                    r3[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                    r3[2] = abc;
                                    r3[3] = sub1;
                                    dt3.Rows.Add(r3);
                                    SqlCommandBuilder cb3 = new SqlCommandBuilder(sda3);
                                    sda3.Update(dt3);


                                    con.Close();
                                    con.Open();

                                    
                                    SqlCommand selcmd7 = new SqlCommand("select subject_" + datetime.DayOfWeek.ToString() + " from studentdatabase.dbo.lec_sub1 where lec_id=4", con);
                                    SqlDataReader rdr7= selcmd7.ExecuteReader();
                                    if (rdr7.Read())
                                    {
                                        sub1 = rdr7["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                       
                                    }

                                    selcmd7.Dispose();
                                    rdr7.Dispose();
                                    con.Close();
                                    con.Open();


                                    DataTable dt4 = new DataTable();
                                    SqlDataAdapter sda4 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture4", con);
                                    sda3.Fill(dt4);
                                    DataRow r4 = dt4.NewRow();
                                    r4[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                    r4[2] = abc;
                                    r4[3] = sub1;
                                    dt4.Rows.Add(r4);
                                    SqlCommandBuilder cb4 = new SqlCommandBuilder(sda4);
                                    sda4.Update(dt4);


                                    con.Close();
                                    con.Open();

                                    SqlCommand selcmd9 = new SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + abc + " AND date1='" + datetime.Year + "-" + mon + "-" + days + "'", con);
                                    SqlDataReader rdr9 = selcmd9.ExecuteReader();
                                    if (rdr9.Read())
                                    {
                                        selcmd9.Dispose();
                                        rdr9.Dispose();
                                        con.Close();
                                        con.Open();
                                    }
                                    else
                                    {
                                        selcmd9.Dispose();
                                        rdr9.Dispose();
                                        con.Close();
                                        con.Open();

                                        DataTable dt5= new DataTable();
                                        SqlDataAdapter sda5= new SqlDataAdapter("select * from studentdatabase.dbo.dayattendance", con);
                                        sda5.Fill(dt5);
                                        DataRow r5 = dt5.NewRow();
                                        r5[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                        r5[2] = abc;
                                        dt5.Rows.Add(r5);
                                        SqlCommandBuilder cb5 = new SqlCommandBuilder(sda5);
                                        sda5.Update(dt5);


                                        con.Close();

                                    }
                                    

                                }
                            }
                            else if (time >= 13 && time <= 24)
                            {
                                string sub1 = "";

                                con.Open();

                                SqlCommand selcmd3 = new SqlCommand("select * from studentdatabase.dbo.lecture5 where student_id=" + abc + " AND date1 ='" + datetime.Year + "-" + mon + "-" + days + "'", con);
                                SqlDataReader rdr2 = selcmd3.ExecuteReader();

                                if (rdr2.Read())
                                {
                                    MessageBox.Show("Your Attendance for Second Half is already Marked");
                                    selcmd3.Dispose();
                                    rdr2.Dispose();
                                    con.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Attendance of Second Half of College hours will be marked ");
                                    selcmd3.Dispose();
                                    rdr2.Dispose();
                                    con.Close();
                                    con.Open();
                                    SqlCommand selcmd4 = new SqlCommand("select subject_" + datetime.DayOfWeek.ToString() + " from studentdatabase.dbo.lec_sub1 where lec_id=5", con);
                                    SqlDataReader rdr4 = selcmd4.ExecuteReader();
                                   if (rdr4.Read())
                                    {
                                        sub1 = rdr4["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                       
                                   }

                                    selcmd4.Dispose();
                                    rdr4.Dispose();
                                    con.Close();
                                    con.Open();



                                    DataTable dt1 = new DataTable();
                                    SqlDataAdapter sda1 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture5", con);
                                    sda1.Fill(dt1);
                                    DataRow r1 = dt1.NewRow();
                                    r1[1] = ""+mon+"-"+days+"-"+datetime.Year+"";
                                    r1[2] = abc;
                                    r1[3] = sub1;
                                    dt1.Rows.Add(r1);
                                    SqlCommandBuilder cb1 = new SqlCommandBuilder(sda1);
                                    sda1.Update(dt1);

                                    con.Close();
                                    con.Open();

                                    SqlCommand selcmd5 = new SqlCommand("select subject_"+datetime.DayOfWeek.ToString()+" from studentdatabase.dbo.lec_sub1 where lec_id=6", con);
                                    SqlDataReader rdr5 = selcmd5.ExecuteReader();
                                    if (rdr5.Read())
                                    {

                                        sub1 = rdr5["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                       
                                    }

                                    selcmd5.Dispose();
                                    rdr5.Dispose();

                                    con.Close();
                                    con.Open();

                               DataTable dt2 = new DataTable();
                                    SqlDataAdapter sda2 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture6", con);
                                    sda2.Fill(dt2);
                                    DataRow r2 = dt2.NewRow();
                                    r2[1] = ""+mon+"-"+days+"-"+datetime.Year+"";
                                    r2[2] = abc;
                                    r2[3] = sub1;
                                    dt2.Rows.Add(r2);
                                    SqlCommandBuilder cb2 = new SqlCommandBuilder(sda2);
                                    sda2.Update(dt2);

                                    con.Close();
                                    con.Open();

                                    SqlCommand selcmd6 = new SqlCommand("select subject_" + datetime.DayOfWeek.ToString() + " from studentdatabase.dbo.lec_sub1 where lec_id=7", con);
                                    SqlDataReader rdr6 = selcmd6.ExecuteReader();
                                    if (rdr6.Read())
                                    {
                                        sub1 = rdr6["subject_" + datetime.DayOfWeek.ToString() + ""].ToString();
                                      
                                    }

                                    selcmd6.Dispose();
                                    rdr6.Dispose();
                                    con.Close();
                                    con.Open();


                                    DataTable dt3 = new DataTable();
                                    SqlDataAdapter sda3 = new SqlDataAdapter("select * from studentdatabase.dbo.lecture7", con);
                                    sda3.Fill(dt3);
                                    DataRow r3 = dt3.NewRow();
                                    r3[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                    r3[2] = abc;
                                    r3[3] = sub1;
                                    dt3.Rows.Add(r3);
                                    SqlCommandBuilder cb3 = new SqlCommandBuilder(sda3);
                                    sda3.Update(dt3);


                                    con.Close();
                                    con.Open();

                                    SqlCommand selcmd9 = new SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + abc + " AND date1='" + datetime.Year + "-" + mon + "-" + days + "'", con);
                                    SqlDataReader rdr9 = selcmd9.ExecuteReader();
                                    if (rdr9.Read())
                                    {
                                        selcmd9.Dispose();
                                        rdr9.Dispose();
                                        con.Close();
                                        con.Open();
                                    }
                                    else
                                    {
                                        selcmd9.Dispose();
                                        rdr9.Dispose();
                                        con.Close();
                                        con.Open();

                                        DataTable dt4 = new DataTable();
                                        SqlDataAdapter sda4 = new SqlDataAdapter("select * from studentdatabase.dbo.dayattendance", con);
                                        sda4.Fill(dt4);
                                        DataRow r4 = dt4.NewRow();
                                        r4[1] = "" + mon + "-" + days + "-" + datetime.Year + "";
                                        r4[2] = abc;
                                        dt4.Rows.Add(r4);
                                        SqlCommandBuilder cb4 = new SqlCommandBuilder(sda4);
                                        sda4.Update(dt4);


                                        con.Close();
                                      
                                    }
                                    

                                }
                            
                            
                            }

                 
                }
                else
                {
                    l_matched.Visible = true;
                    l_matched.ForeColor = (Color.Red);
                    l_matched.Text = "Sorry no match Found. New student click on Enroll ";
                }



            }
        }

      
      

        private void b_login_Click(object sender, EventArgs e)
        {
            if(t_password.Text=="oistit09")
           {b_enroll.Visible = true;
            b_enrolledstudent.Visible = true;
            b_switch.Visible = true;
            b_monthattendance.Visible = true;
            b_lectures.Visible = true;
            b_subjects.Visible = true;
            b_nonworking.Visible = true;

            l_password.Visible = false;
            t_password.Visible = false;
            b_login.Visible = false;
            l_welcome.Visible = false;
           }
           else
           {
               MessageBox.Show("You entered Wrong Password \n!! Please Try again !!");
               t_password.Text="";
        }

        }

       

      
       

        private void b_student_Click(object sender, EventArgs e)
        {

            b_mark.Visible = true;
            b_switch.Visible = true;
            b_faculty.Visible = false;
            b_student.Visible = false;
            l_welcome.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rollname.Text = "";
            m1.Text = "";
            m2.Text = "";

            m3.Text = "";
            m4.Text = "";
            m5.Text = "";
            m6.Text = "";
            m7.Text = "";
            m8.Text = "";
            m9.Text = "";
            m10.Text = "";
            m11.Text = "";
            m12.Text = "";
            m13.Text = "";
            m14.Text = "";
            m15.Text = "";
            m16.Text = "";
            m17.Text = "";
            m18.Text = "";
            m19.Text = "";
            m20.Text = "";
            m21.Text = "";
            m22.Text = "";
            m23.Text = "";
            m24.Text = "";
            m25.Text = "";
            m26.Text = "";
            m27.Text = "";
            m28.Text = "";

            m29.Text = "";
            m30.Text = "";

            m31.Text = ""; 
            this.AutoSize = false;

            selectday.Visible = false;
            dropdown.Visible = false;
            lec1.Visible = false;
            lec2.Visible = false;
            lec3.Visible = false;
            lec4.Visible = false;
            lec5.Visible = false;
            lec6.Visible = false;
            lec7.Visible = false;
            dd1.Visible = false;
            dd2.Visible = false;
            dd3.Visible = false;
            dd4.Visible = false;
            dd5.Visible = false;
            dd6.Visible = false;
            dd7.Visible = false;
            saveme.Visible = false;
            save.Visible = false;
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = false;
            s5.Visible = false;
            daywise.Visible = false;
            subwise.Visible = false;
            rollname.Visible = false;
            l_days.Visible = false;
            m1.Visible = false;
            b_faculty.Visible = true;
            b_student.Visible = true;
            l_welcome.Visible = true;

           
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
            l_dataenrolled.Visible = false;
            l_putfinger.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            l_matched.Visible = false;
            t_name.Visible = false;
            t_roll.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            timer1.Enabled = false;
            timer2.Enabled = false;

            b_enroll.Visible = false;
            b_mark.Visible = false;
            b_enrolledstudent.Visible = false;
            b_monthattendance.Visible = false;
            b_submit.Visible = false;
            b_switch.Visible = false;
            b_lectures.Visible = false;
            b_subjects.Visible = false;
            b_nonworking.Visible = false;
            studentlist.Visible = false;
        }

        private void b_switch_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        private void b_faculty_Click(object sender, EventArgs e)
        {
            l_password.Visible = true;
            t_password.Visible = true;
            t_password.Text = "";
            b_login.Visible = true;

            b_faculty.Visible = false;
            b_student.Visible = false;
        }

        private void b_mark_Click(object sender, EventArgs e)
        {

            DateTime datetime = DateTime.Now;

            int time = datetime.Hour;
            if (time >= 9 && time <= 24)
            {
                l_welcome.Visible = false;
                l_dataenrolled.Visible = false;
                l_putfinger.Visible = true;
                l_enterroll.Visible = false;
                l_entername.Visible = false;

                b_submit.Visible = false;
                t_roll.Visible = false;
                t_name.Visible = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                pictureBox3.Visible = true;

                initialLatestDatetime = DateTime.MinValue;

                if (!string.IsNullOrEmpty(textBox1.Text) && Directory.Exists(textBox1.Text))
                {
                    var files = Directory.GetFiles(textBox1.Text);

                    initialFileCount = files.Count();

                    foreach (var file in files)
                    {
                        DateTime dt = File.GetCreationTime(file);

                        if (dt > initialLatestDatetime)
                        {
                            initialLatestDatetime = dt;
                        }
                    }


                }

                i = 0;

                timer2.Enabled = true;

               
            }
            else
            {

                MessageBox.Show("Sorry Invalid College Hours.. \nTry again in college Hours");

            }
            
          
        
        
        }

        private void b_submit_Click(object sender, EventArgs e)
        {

            if ((t_roll.Text).Length == 12)
            {
                con.Open();
                SqlCommand selcmd = new SqlCommand("select Id from studentdatabase.dbo.student3 where roll_no='" + t_roll.Text + "'", con);

                SqlDataReader rdr = selcmd.ExecuteReader();

                if (rdr.Read())
                {

                    MessageBox.Show("This roll.no already exist!!\nTry again");
                    t_roll.Text = "";
                }
                else
                {

                    l_enterroll.Visible = false;
                    l_entername.Visible = false;
                    t_roll.Visible = false;
                    t_name.Visible = false;
                    b_submit.Visible = false;


                    l_putfinger.Visible = true;
                    pictureBox1.Visible = true;


                    i = 0;
                    timer1.Enabled = true;
                }

                con.Close();
            }
            else
            {
                t_roll.Text = "";
                MessageBox.Show("Enter 12 digit roll.no.. \nExample :- 0105IT091069");
            }
        }

        private void b_enroll_Click(object sender, EventArgs e)
        {
            selectday.Visible = false;
            dropdown.Visible = false;
            lec1.Visible = false;
            lec2.Visible = false;
            lec3.Visible = false;
            lec4.Visible = false;
            lec5.Visible = false;
            lec6.Visible = false;
            lec7.Visible = false;
            dd1.Visible = false;
            dd2.Visible = false;
            dd3.Visible = false;
            dd4.Visible = false;
            dd5.Visible = false;
            dd6.Visible = false;
            dd7.Visible = false;
            saveme.Visible = false;


            save.Visible = false;
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = false;
            s5.Visible = false;
            daywise.Visible = false;
            subwise.Visible = false; 
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
            rollname.Visible = false;
            l_days.Visible = false;
            m1.Visible = false;
            l_welcome.Visible = false;
            l_dataenrolled.Visible = false;
            l_putfinger.Visible = false;
            l_matched.Visible = false;
            l_enterroll.Visible = true;
            l_entername.Visible = true;
            t_roll.Visible = true;
            t_name.Visible = true;
            b_submit.Visible = true;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            studentlist.Visible = false;
         
            
            initialLatestDatetime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(textBox1.Text) && Directory.Exists(textBox1.Text))
            {
                var files = Directory.GetFiles(textBox1.Text);

                initialFileCount = files.Count();

                foreach (var file in files)
                {
                    DateTime dt = File.GetCreationTime(file);

                    if (dt > initialLatestDatetime)
                    {
                        initialLatestDatetime = dt;
                    }
                }


            }
            //timer1.Start();
        }

        private void b_enrolledstudent_Click(object sender, EventArgs e)
        {
            selectday.Visible = false;
            dropdown.Visible = false;
            lec1.Visible = false;
            lec2.Visible = false;
            lec3.Visible = false;
            lec4.Visible = false;
            lec5.Visible = false;
            lec6.Visible = false;
            lec7.Visible = false;
            dd1.Visible = false;
            dd2.Visible = false;
            dd3.Visible = false;
            dd4.Visible = false;
            dd5.Visible = false;
            dd6.Visible = false;
            dd7.Visible = false;
            saveme.Visible = false;
            
            save.Visible = false;
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = false;
            s5.Visible = false;
            daywise.Visible = false;
            subwise.Visible = false; 
            rollname.Visible = false;
            l_days.Visible = false;
            m1.Visible = false;
            t_name.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            t_roll.Visible = false;
            t_name.Visible = false;
            b_submit.Visible = false;
            l_putfinger.Visible = false;
            pictureBox1.Visible = false;
            l_dataenrolled.Visible = false;
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
            studentlist.Visible = true;

            con.Open();


            SqlCommand selcmd = new System.Data.SqlClient.SqlCommand("select roll_no,name from studentdatabase.dbo.student3 ", con);

            SqlDataReader rdr = selcmd.ExecuteReader();


           

            studentlist.Text = "Student Roll_no        Student Name \n --------------------------------------";


           while (rdr.Read())
            {

                studentlist.Text += "\n\n"+rdr["roll_no"].ToString()+"             "+rdr["name"];
            }

            con.Close();

              

        }

        private void b_monthattendance_Click(object sender, EventArgs e)
        {
            daywise.Visible = true;
            subwise.Visible = true;

            selectday.Visible = false;
            dropdown.Visible = false;
            lec1.Visible = false;
            lec2.Visible = false;
            lec3.Visible = false;
            lec4.Visible = false;
            lec5.Visible = false;
            lec6.Visible = false;
            lec7.Visible = false;
            dd1.Visible = false;
            dd2.Visible = false;
            dd3.Visible = false;
            dd4.Visible = false;
            dd5.Visible = false;
            dd6.Visible = false;
            dd7.Visible = false;
            saveme.Visible = false;
            save.Visible = false;
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = false;
            s5.Visible = false;
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
            rollname.Visible = false;
            l_days.Visible = false;
            t_name.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            t_roll.Visible = false;
            t_name.Visible = false;
            b_submit.Visible = false;
            l_putfinger.Visible = false;
            pictureBox1.Visible = false;
            l_dataenrolled.Visible = false;

            studentlist.Visible = false;

   

        }

        private void b_subjects_Click(object sender, EventArgs e)
        {
            save.Visible = true;
            s1.Visible = true;
            s2.Visible = true;
            s3.Visible = true;
            s4.Visible = true;
            s5.Visible = true;


            con.Open();


            SqlCommand selcmd = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.subject1 ", con);

            SqlDataReader rdr = selcmd.ExecuteReader();

         
           for(int d=1;d<=5;d++)
           {
              

               if(d==1 &&  rdr.Read())
                s1.Text = rdr["subname"].ToString();
               else if (d == 2 &&  rdr.Read())
                   s2.Text = rdr["subname"].ToString();
               else if (d == 3 &&  rdr.Read())

                   s3.Text = rdr["subname"].ToString();
               else if (d == 4 &&  rdr.Read())
                   s4.Text = rdr["subname"].ToString();
               else if (d == 5 &&  rdr.Read())
                   s5.Text = rdr["subname"].ToString();
               
            }

            con.Close();

            selectday.Visible = false;
            dropdown.Visible = false;
            lec1.Visible = false;
            lec2.Visible = false;
            lec3.Visible = false;
            lec4.Visible = false;
            lec5.Visible = false;
            lec6.Visible = false;
            lec7.Visible = false;
            dd1.Visible = false;
            dd2.Visible = false;
            dd3.Visible = false;
            dd4.Visible = false;
            dd5.Visible = false;
            dd6.Visible = false;
            dd7.Visible = false;
            saveme.Visible = false;
            daywise.Visible = false;
            subwise.Visible = false; 
            rollname.Visible = false;
            l_days.Visible = false;
            m1.Visible = false;
            t_name.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            t_roll.Visible = false;
            t_name.Visible = false;
            b_submit.Visible = false;
            l_putfinger.Visible = false;
            pictureBox1.Visible = false;
            l_dataenrolled.Visible = false;
            studentlist.Visible = false;
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
        }

        private void b_lectures_Click(object sender, EventArgs e)
        {


            save.Visible = false;
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = false;
            s5.Visible = false;
            daywise.Visible = false;
            subwise.Visible = false;
            rollname.Visible = false;
            l_days.Visible = false;
            m1.Visible = false;
            t_name.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            t_roll.Visible = false;
            t_name.Visible = false;
            b_submit.Visible = false;
            l_putfinger.Visible = false;
            pictureBox1.Visible = false;
            l_dataenrolled.Visible = false;
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
            studentlist.Visible = false;



            selectday.Visible = true;
            dropdown.Visible = true;
            lec1.Visible = true;
            lec2.Visible = true;
            lec3.Visible = true;
            lec4.Visible = true;
            lec5.Visible = true;
            lec6.Visible = true;
            lec7.Visible = true;
            dd1.Visible = true;
            dd2.Visible = true;
            dd3.Visible = true;
            dd4.Visible = true;
            dd5.Visible = true;
            dd6.Visible = true;
            dd7.Visible = true;
            saveme.Visible = true;


        }    


        private void b_nonworking_Click(object sender, EventArgs e)
        {
            selectday.Visible = false;
            dropdown.Visible = false;
            lec1.Visible = false;
            lec2.Visible = false;
            lec3.Visible = false;
            lec4.Visible = false;
            lec5.Visible = false;
            lec6.Visible = false;
            lec7.Visible = false;
            dd1.Visible = false;
            dd2.Visible = false;
            dd3.Visible = false;
            dd4.Visible = false;
            dd5.Visible = false;
            dd6.Visible = false;
            dd7.Visible = false;
            saveme.Visible = false;
            save.Visible = false;
            s1.Visible = false;
            s2.Visible = false;
            s3.Visible = false;
            s4.Visible = false;
            s5.Visible = false;
            daywise.Visible = false;
            subwise.Visible = false; 
            rollname.Visible = false;
            l_days.Visible = false;
            m1.Visible = false;
            t_name.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            t_roll.Visible = false;
            t_name.Visible = false;
            b_submit.Visible = false;
            l_putfinger.Visible = false;
            pictureBox1.Visible = false;
            l_dataenrolled.Visible = false;
            m1.Visible = false;
            m2.Visible = false;
            m3.Visible = false;
            m4.Visible = false;
            m5.Visible = false;
            m6.Visible = false;
            m7.Visible = false;
            m8.Visible = false;
            m9.Visible = false;
            m10.Visible = false;
            m11.Visible = false;
            m12.Visible = false;
            m13.Visible = false;
            m14.Visible = false;
            m15.Visible = false;
            m16.Visible = false;
            m17.Visible = false;
            m18.Visible = false;
            m19.Visible = false;
            m20.Visible = false;
            m21.Visible = false;
            m22.Visible = false;
            m23.Visible = false;
            m24.Visible = false;
            m25.Visible = false;
            m26.Visible = false;
            m27.Visible = false;
            m28.Visible = false;
            m29.Visible = false;
            m30.Visible = false;
            m31.Visible = false;
            
          


            studentlist.Visible = false;


            con.Open();


            SqlCommand selcmd1 = new System.Data.SqlClient.SqlCommand(" INSERT INTO OPENROWSET ('Microsoft.Jet.OLEDB.4.0', 'Excel 8.0;Database=c:\\Test.xls;','select roll_no,name from studentdatabase.dbo.student3')", con);

            selcmd1.BeginExecuteNonQuery();
            con.Close();


        }

        private void daywise_Click(object sender, EventArgs e)
        {


            DateTime datetime = DateTime.Now;


            int mon = datetime.Month;
            int year = datetime.Year;
            int d2 = (int)datetime.Day;
            daywise.Visible = false;
            subwise.Visible = false;

            rollname.Visible = true;

            l_days.Text = "1     2     3     4     5      6     7     8     9     10   11   12    13   14    15   16   17    18   19    20   21    22   23   24    25   26    27   28   29    30   31";
            l_days.Visible = true;

            m1.Text = "";

            m2.Text = "";

            m3.Text = "";

            m4.Text = "";

            m5.Text = "";

            m6.Text = "";

            m7.Text = "";

            m8.Text = "";

            m9.Text = "";



            m10.Text = "";

            m11.Text = "";

            m12.Text = "";

            m13.Text = "";

            m14.Text = "";

            m15.Text = "";

            m16.Text = "";

            m17.Text = "";

            m18.Text = "";

            m19.Text = "";

            m20.Text = "";

            m21.Text = "";

            m22.Text = "";

            m23.Text = "";

            m24.Text = "";

            m25.Text = "";

            m26.Text = "";

            m27.Text = "";



            m28.Text = "";
            m29.Text = "";

            m30.Text = "";

            m31.Text = "";
            rollname.Text = "";
          
            m1.Visible = true;
            m2.Visible = true;
            m3.Visible = true;
            m4.Visible = true;
            m5.Visible = true;
            m6.Visible = true;
            m7.Visible = true;
            m8.Visible = true;
            m9.Visible = true;
            m10.Visible = true;
            m11.Visible = true;
            m12.Visible = true;
            m13.Visible = true;
            m14.Visible = true;
            m15.Visible = true;
            m16.Visible = true;
            m17.Visible = true;
            m18.Visible = true;
            m19.Visible = true;
            m20.Visible = true;
            m21.Visible = true;
            m22.Visible = true;
            m23.Visible = true;
            m24.Visible = true;
            m25.Visible = true;
            m26.Visible = true;
            m27.Visible = true;
            m28.Visible = true;
            m29.Visible = true;
            m30.Visible = true;
            m31.Visible = true;





            con.Open();


            SqlCommand selcmd = new System.Data.SqlClient.SqlCommand("select Id,roll_no,name from studentdatabase.dbo.student3 ", con);

            SqlDataReader rdr = selcmd.ExecuteReader();
            int i = 0;


            while (rdr.Read())
            {

                i++;

            }
            int[] temp = new int[i];
            con.Close();
            con.Open();

            SqlCommand selcmd10 = new System.Data.SqlClient.SqlCommand("select Id,roll_no,name from studentdatabase.dbo.student3 ", con);

            SqlDataReader rdr10 = selcmd10.ExecuteReader();


            i = 0;
            while (rdr10.Read())
            {


                temp[i] = (int)rdr10["Id"];
                i++;


                rollname.Text += rdr10["roll_no"].ToString() + " " + rdr10["name"] + "\n\n";


            }

            con.Close();

            i = i - 1;

            string mon1 = "";
            if (mon < 10)
                mon1 = "0" + mon.ToString();
            else
                mon1 = mon.ToString();
            for (int p = 0; p <= i; p++)
            {
                if (mon == 2)
                {
                    if (year % 4 == 0)
                    {
                        for (int k = 1; k <= 29; k++)
                        {

                            string day1 = "";
                            if (k < 10)
                                day1 = "0" + k.ToString();
                            else
                                day1 = k.ToString();
                            con.Close();

                            con.Open();
                            SqlCommand selcmd1 = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + temp[p] + " AND date1 ='" + year + "-" + mon1 + "-" + day1 + "'", con);

                            SqlDataReader rdr1 = selcmd1.ExecuteReader();


                            if (rdr1.Read())
                            {
                                if (k == 1)
                                    m1.Text += "1\n\n";
                                else if (k == 2)
                                    m2.Text += "1\n\n";
                                else if (k == 3)
                                    m3.Text += "1\n\n";
                                else if (k == 4)
                                    m4.Text += "1\n\n";
                                else if (k == 5)
                                    m5.Text += "1\n\n";
                                else if (k == 6)
                                    m6.Text += "1\n\n";
                                else if (k == 7)
                                    m7.Text += "1\n\n";
                                else if (k == 8)
                                    m8.Text += "1\n\n";
                                else if (k == 9)
                                    m9.Text += "1\n\n";
                                else if (k == 10)
                                    m10.Text += "1\n\n";
                                else if (k == 11)
                                    m11.Text += "1\n\n";
                                else if (k == 12)
                                    m12.Text += "1\n\n";
                                else if (k == 13)
                                    m13.Text += "1\n\n";
                                else if (k == 14)
                                    m14.Text += "1\n\n";
                                else if (k == 15)
                                    m15.Text += "1\n\n";
                                else if (k == 16)
                                    m16.Text += "1\n\n";
                                else if (k == 17)
                                    m17.Text += "1\n\n";
                                else if (k == 18)
                                    m18.Text += "1\n\n";
                                else if (k == 19)
                                    m19.Text += "1\n\n";
                                else if (k == 20)
                                    m20.Text += "1\n\n";
                                else if (k == 21)
                                    m21.Text += "1\n\n";
                                else if (k == 22)
                                    m22.Text += "1\n\n";
                                else if (k == 23)
                                    m23.Text += "1\n\n";
                                else if (k == 24)
                                    m24.Text += "1\n\n";
                                else if (k == 25)
                                    m25.Text += "1\n\n";
                                else if (k == 26)
                                    m26.Text += "1\n\n";
                                else if (k == 27)
                                    m27.Text += "1\n\n";
                                else if (k == 28)
                                    m28.Text += "1\n\n";
                                else if (k == 29)
                                    m29.Text += "1\n\n";
                                else if (k == 30)
                                    m30.Text += "1\n\n";
                                else if (k == 31)
                                    m31.Text += "1\n\n";
                            }
                            else
                            {
                                if (k == 1)
                                    m1.Text += "A\n\n";
                                else if (k == 2)
                                    m2.Text += "A\n\n";
                                else if (k == 3)
                                    m3.Text += "A\n\n";
                                else if (k == 4)
                                    m4.Text += "A\n\n";
                                else if (k == 5)
                                    m5.Text += "A\n\n";
                                else if (k == 6)
                                    m6.Text += "A\n\n";
                                else if (k == 7)
                                    m7.Text += "A\n\n";
                                else if (k == 8)
                                    m8.Text += "A\n\n";
                                else if (k == 9)
                                    m9.Text += "A\n\n";
                                else if (k == 10)
                                    m10.Text += "A\n\n";
                                else if (k == 11)
                                    m11.Text += "A\n\n";
                                else if (k == 12)
                                    m12.Text += "A\n\n";
                                else if (k == 13)
                                    m13.Text += "A\n\n";
                                else if (k == 14)
                                    m14.Text += "A\n\n";
                                else if (k == 15)
                                    m15.Text += "A\n\n";
                                else if (k == 16)
                                    m16.Text += "A\n\n";
                                else if (k == 17)
                                    m17.Text += "A\n\n";
                                else if (k == 18)
                                    m18.Text += "A\n\n";
                                else if (k == 19)
                                    m19.Text += "A\n\n";
                                else if (k == 20)
                                    m20.Text += "A\n\n";
                                else if (k == 21)
                                    m21.Text += "A\n\n";
                                else if (k == 22)
                                    m22.Text += "A\n\n";
                                else if (k == 23)
                                    m23.Text += "A\n\n";
                                else if (k == 24)
                                    m24.Text += "A\n\n";
                                else if (k == 25)
                                    m25.Text += "A\n\n";
                                else if (k == 26)
                                    m26.Text += "A\n\n";
                                else if (k == 27)
                                    m27.Text += "A\n\n";
                                else if (k == 28)
                                    m28.Text += "A\n\n";
                                else if (k == 29)
                                    m29.Text += "A\n\n";
                                else if (k == 30)
                                    m30.Text += "A\n\n";
                                else if (k == 31)
                                    m31.Text += "A\n\n";
                            }
                            con.Close();

                        }
                    }
                    else
                    {
                        for (int k = 1; k <= 28; k++)
                        {
                            string day1 = "";
                            if (k < 10)
                                day1 = "0" + k.ToString();
                            else
                                day1 = k.ToString();
                            con.Close();
                            con.Open();
                            SqlCommand selcmd2 = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + temp[p] + " AND date1 ='" + year + "-" + mon1 + "-" + day1 + "'", con);

                            SqlDataReader rdr2 = selcmd2.ExecuteReader();


                            if (rdr2.Read())
                            {
                                if (k == 1)
                                    m1.Text += "1\n\n";
                                else if (k == 2)
                                    m2.Text += "1\n\n";
                                else if (k == 3)
                                    m3.Text += "1\n\n";
                                else if (k == 4)
                                    m4.Text += "1\n\n";
                                else if (k == 5)
                                    m5.Text += "1\n\n";
                                else if (k == 6)
                                    m6.Text += "1\n\n";
                                else if (k == 7)
                                    m7.Text += "1\n\n";
                                else if (k == 8)
                                    m8.Text += "1\n\n";
                                else if (k == 9)
                                    m9.Text += "1\n\n";
                                else if (k == 10)
                                    m10.Text += "1\n\n";
                                else if (k == 11)
                                    m11.Text += "1\n\n";
                                else if (k == 12)
                                    m12.Text += "1\n\n";
                                else if (k == 13)
                                    m13.Text += "1\n\n";
                                else if (k == 14)
                                    m14.Text += "1\n\n";
                                else if (k == 15)
                                    m15.Text += "1\n\n";
                                else if (k == 16)
                                    m16.Text += "1\n\n";
                                else if (k == 17)
                                    m17.Text += "1\n\n";
                                else if (k == 18)
                                    m18.Text += "1\n\n";
                                else if (k == 19)
                                    m19.Text += "1\n\n";
                                else if (k == 20)
                                    m20.Text += "1\n\n";
                                else if (k == 21)
                                    m21.Text += "1\n\n";
                                else if (k == 22)
                                    m22.Text += "1\n\n";
                                else if (k == 23)
                                    m23.Text += "1\n\n";
                                else if (k == 24)
                                    m24.Text += "1\n\n";
                                else if (k == 25)
                                    m25.Text += "1\n\n";
                                else if (k == 26)
                                    m26.Text += "1\n\n";
                                else if (k == 27)
                                    m27.Text += "1\n\n";
                                else if (k == 28)
                                    m28.Text += "1\n\n";
                                else if (k == 29)
                                    m29.Text += "1\n\n";
                                else if (k == 30)
                                    m30.Text += "1\n\n";
                                else if (k == 31)
                                    m31.Text += "1\n\n";
                            }
                            else
                            {
                                if (k == 1)
                                    m1.Text += "A\n\n";
                                else if (k == 2)
                                    m2.Text += "A\n\n";
                                else if (k == 3)
                                    m3.Text += "A\n\n";
                                else if (k == 4)
                                    m4.Text += "A\n\n";
                                else if (k == 5)
                                    m5.Text += "A\n\n";
                                else if (k == 6)
                                    m6.Text += "A\n\n";
                                else if (k == 7)
                                    m7.Text += "A\n\n";
                                else if (k == 8)
                                    m8.Text += "A\n\n";
                                else if (k == 9)
                                    m9.Text += "A\n\n";
                                else if (k == 10)
                                    m10.Text += "A\n\n";
                                else if (k == 11)
                                    m11.Text += "A\n\n";
                                else if (k == 12)
                                    m12.Text += "A\n\n";
                                else if (k == 13)
                                    m13.Text += "A\n\n";
                                else if (k == 14)
                                    m14.Text += "A\n\n";
                                else if (k == 15)
                                    m15.Text += "A\n\n";
                                else if (k == 16)
                                    m16.Text += "A\n\n";
                                else if (k == 17)
                                    m17.Text += "A\n\n";
                                else if (k == 18)
                                    m18.Text += "A\n\n";
                                else if (k == 19)
                                    m19.Text += "A\n\n";
                                else if (k == 20)
                                    m20.Text += "A\n\n";
                                else if (k == 21)
                                    m21.Text += "A\n\n";
                                else if (k == 22)
                                    m22.Text += "A\n\n";
                                else if (k == 23)
                                    m23.Text += "A\n\n";
                                else if (k == 24)
                                    m24.Text += "A\n\n";
                                else if (k == 25)
                                    m25.Text += "A\n\n";
                                else if (k == 26)
                                    m26.Text += "A\n\n";
                                else if (k == 27)
                                    m27.Text += "A\n\n";
                                else if (k == 28)
                                    m28.Text += "A\n\n";
                                else if (k == 29)
                                    m29.Text += "A\n\n";
                                else if (k == 30)
                                    m30.Text += "A\n\n";
                                else if (k == 31)
                                    m31.Text += "A\n\n";
                            }
                            con.Close();

                        }
                    }
                }
                else
                    if (mon == 8)
                    {
                        for (int k = 1; k <= 31; k++)
                        {
                            string day1 = "";
                            if (k < 10)
                                day1 = "0" + k.ToString();
                            else
                                day1 = k.ToString();

                            con.Close();
                            con.Open();
                            SqlCommand selcmd3 = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + temp[p] + " AND date1 ='" + year + "-" + mon1 + "-" + day1 + "'", con);

                            SqlDataReader rdr3 = selcmd3.ExecuteReader();


                            if (rdr3.Read())
                            {
                                if (k == 1)
                                    m1.Text += "1\n\n";
                                else if (k == 2)
                                    m2.Text += "1\n\n";
                                else if (k == 3)
                                    m3.Text += "1\n\n";
                                else if (k == 4)
                                    m4.Text += "1\n\n";
                                else if (k == 5)
                                    m5.Text += "1\n\n";
                                else if (k == 6)
                                    m6.Text += "1\n\n";
                                else if (k == 7)
                                    m7.Text += "1\n\n";
                                else if (k == 8)
                                    m8.Text += "1\n\n";
                                else if (k == 9)
                                    m9.Text += "1\n\n";
                                else if (k == 10)
                                    m10.Text += "1\n\n";
                                else if (k == 11)
                                    m11.Text += "1\n\n";
                                else if (k == 12)
                                    m12.Text += "1\n\n";
                                else if (k == 13)
                                    m13.Text += "1\n\n";
                                else if (k == 14)
                                    m14.Text += "1\n\n";
                                else if (k == 15)
                                    m15.Text += "1\n\n";
                                else if (k == 16)
                                    m16.Text += "1\n\n";
                                else if (k == 17)
                                    m17.Text += "1\n\n";
                                else if (k == 18)
                                    m18.Text += "1\n\n";
                                else if (k == 19)
                                    m19.Text += "1\n\n";
                                else if (k == 20)
                                    m20.Text += "1\n\n";
                                else if (k == 21)
                                    m21.Text += "1\n\n";
                                else if (k == 22)
                                    m22.Text += "1\n\n";
                                else if (k == 23)
                                    m23.Text += "1\n\n";
                                else if (k == 24)
                                    m24.Text += "1\n\n";
                                else if (k == 25)
                                    m25.Text += "1\n\n";
                                else if (k == 26)
                                    m26.Text += "1\n\n";
                                else if (k == 27)
                                    m27.Text += "1\n\n";
                                else if (k == 28)
                                    m28.Text += "1\n\n";
                                else if (k == 29)
                                    m29.Text += "1\n\n";
                                else if (k == 30)
                                    m30.Text += "1\n\n";
                                else if (k == 31)
                                    m31.Text += "1\n\n";
                            }
                            else
                            {
                                if (k == 1)
                                    m1.Text += "A\n\n";
                                else if (k == 2)
                                    m2.Text += "A\n\n";
                                else if (k == 3)
                                    m3.Text += "A\n\n";
                                else if (k == 4)
                                    m4.Text += "A\n\n";
                                else if (k == 5)
                                    m5.Text += "A\n\n";
                                else if (k == 6)
                                    m6.Text += "A\n\n";
                                else if (k == 7)
                                    m7.Text += "A\n\n";
                                else if (k == 8)
                                    m8.Text += "A\n\n";
                                else if (k == 9)
                                    m9.Text += "A\n\n";
                                else if (k == 10)
                                    m10.Text += "A\n\n";
                                else if (k == 11)
                                    m11.Text += "A\n\n";
                                else if (k == 12)
                                    m12.Text += "A\n\n";
                                else if (k == 13)
                                    m13.Text += "A\n\n";
                                else if (k == 14)
                                    m14.Text += "A\n\n";
                                else if (k == 15)
                                    m15.Text += "A\n\n";
                                else if (k == 16)
                                    m16.Text += "A\n\n";
                                else if (k == 17)
                                    m17.Text += "A\n\n";
                                else if (k == 18)
                                    m18.Text += "A\n\n";
                                else if (k == 19)
                                    m19.Text += "A\n\n";
                                else if (k == 20)
                                    m20.Text += "A\n\n";
                                else if (k == 21)
                                    m21.Text += "A\n\n";
                                else if (k == 22)
                                    m22.Text += "A\n\n";
                                else if (k == 23)
                                    m23.Text += "A\n\n";
                                else if (k == 24)
                                    m24.Text += "A\n\n";
                                else if (k == 25)
                                    m25.Text += "A\n\n";
                                else if (k == 26)
                                    m26.Text += "A\n\n";
                                else if (k == 27)
                                    m27.Text += "A\n\n";
                                else if (k == 28)
                                    m28.Text += "A\n\n";
                                else if (k == 29)
                                    m29.Text += "A\n\n";
                                else if (k == 30)
                                    m30.Text += "A\n\n";
                                else if (k == 31)
                                    m31.Text += "A\n\n";
                            }
                            con.Close();

                        }
                    }
                    else
                        if (mon % 2 == 1)
                        {
                            for (int k = 1; k <= 31; k++)
                            {

                                string day1 = "";
                                if (k < 10)
                                    day1 = "0" + k.ToString();
                                else
                                    day1 = k.ToString();
                                con.Close();
                                con.Open();
                                SqlCommand selcmd4 = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + temp[p] + " AND date1 ='" + year + "-" + mon1 + "-" + day1 + "'", con);

                                SqlDataReader rdr4 = selcmd4.ExecuteReader();


                                if (rdr4.Read())
                                {
                                    if (k == 1)
                                        m1.Text += "1\n\n";
                                    else if (k == 2)
                                        m2.Text += "1\n\n";
                                    else if (k == 3)
                                        m3.Text += "1\n\n";
                                    else if (k == 4)
                                        m4.Text += "1\n\n";
                                    else if (k == 5)
                                        m5.Text += "1\n\n";
                                    else if (k == 6)
                                        m6.Text += "1\n\n";
                                    else if (k == 7)
                                        m7.Text += "1\n\n";
                                    else if (k == 8)
                                        m8.Text += "1\n\n";
                                    else if (k == 9)
                                        m9.Text += "1\n\n";
                                    else if (k == 10)
                                        m10.Text += "1\n\n";
                                    else if (k == 11)
                                        m11.Text += "1\n\n";
                                    else if (k == 12)
                                        m12.Text += "1\n\n";
                                    else if (k == 13)
                                        m13.Text += "1\n\n";
                                    else if (k == 14)
                                        m14.Text += "1\n\n";
                                    else if (k == 15)
                                        m15.Text += "1\n\n";
                                    else if (k == 16)
                                        m16.Text += "1\n\n";
                                    else if (k == 17)
                                        m17.Text += "1\n\n";
                                    else if (k == 18)
                                        m18.Text += "1\n\n";
                                    else if (k == 19)
                                        m19.Text += "1\n\n";
                                    else if (k == 20)
                                        m20.Text += "1\n\n";
                                    else if (k == 21)
                                        m21.Text += "1\n\n";
                                    else if (k == 22)
                                        m22.Text += "1\n\n";
                                    else if (k == 23)
                                        m23.Text += "1\n\n";
                                    else if (k == 24)
                                        m24.Text += "1\n\n";
                                    else if (k == 25)
                                        m25.Text += "1\n\n";
                                    else if (k == 26)
                                        m26.Text += "1\n\n";
                                    else if (k == 27)
                                        m27.Text += "1\n\n";
                                    else if (k == 28)
                                        m28.Text += "1\n\n";
                                    else if (k == 29)
                                        m29.Text += "1\n\n";
                                    else if (k == 30)
                                        m30.Text += "1\n\n";
                                    else if (k == 31)
                                        m31.Text += "1\n\n";
                                }
                                else
                                {
                                    if (k == 1)
                                        m1.Text += "A\n\n";
                                    else if (k == 2)
                                        m2.Text += "A\n\n";
                                    else if (k == 3)
                                        m3.Text += "A\n\n";
                                    else if (k == 4)
                                        m4.Text += "A\n\n";
                                    else if (k == 5)
                                        m5.Text += "A\n\n";
                                    else if (k == 6)
                                        m6.Text += "A\n\n";
                                    else if (k == 7)
                                        m7.Text += "A\n\n";
                                    else if (k == 8)
                                        m8.Text += "A\n\n";
                                    else if (k == 9)
                                        m9.Text += "A\n\n";
                                    else if (k == 10)
                                        m10.Text += "A\n\n";
                                    else if (k == 11)
                                        m11.Text += "A\n\n";
                                    else if (k == 12)
                                        m12.Text += "A\n\n";
                                    else if (k == 13)
                                        m13.Text += "A\n\n";
                                    else if (k == 14)
                                        m14.Text += "A\n\n";
                                    else if (k == 15)
                                        m15.Text += "A\n\n";
                                    else if (k == 16)
                                        m16.Text += "A\n\n";
                                    else if (k == 17)
                                        m17.Text += "A\n\n";
                                    else if (k == 18)
                                        m18.Text += "A\n\n";
                                    else if (k == 19)
                                        m19.Text += "A\n\n";
                                    else if (k == 20)
                                        m20.Text += "A\n\n";
                                    else if (k == 21)
                                        m21.Text += "A\n\n";
                                    else if (k == 22)
                                        m22.Text += "A\n\n";
                                    else if (k == 23)
                                        m23.Text += "A\n\n";
                                    else if (k == 24)
                                        m24.Text += "A\n\n";
                                    else if (k == 25)
                                        m25.Text += "A\n\n";
                                    else if (k == 26)
                                        m26.Text += "A\n\n";
                                    else if (k == 27)
                                        m27.Text += "A\n\n";
                                    else if (k == 28)
                                        m28.Text += "A\n\n";
                                    else if (k == 29)
                                        m29.Text += "A\n\n";
                                    else if (k == 30)
                                        m30.Text += "A\n\n";
                                    else if (k == 31)
                                        m31.Text += "A\n\n";
                                } con.Close();


                            }
                        }
                        else
                            if (mon % 2 == 0)
                            {
                                for (int k = 1; k <= 30; k++)
                                {


                                    string day1 = "";
                                    if (k < 10)
                                        day1 = "0" + k.ToString();
                                    else
                                        day1 = k.ToString();
                                    con.Close();
                                    con.Open();
                                    SqlCommand selcmd5 = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.dayattendance where student_id=" + temp[p] + " AND date1 ='" + year.ToString() + "-" + mon1 + "-" + day1 + "'", con);

                                    SqlDataReader rdr5 = selcmd5.ExecuteReader();


                                    if (rdr5.Read())
                                    {
                                        if (k == 1)
                                            m1.Text += "1\n\n";
                                        else if (k == 2)
                                            m2.Text += "1\n\n";
                                        else if (k == 3)
                                            m3.Text += "1\n\n";
                                        else if (k == 4)
                                            m4.Text += "1\n\n";
                                        else if (k == 5)
                                            m5.Text += "1\n\n";
                                        else if (k == 6)
                                            m6.Text += "1\n\n";
                                        else if (k == 7)
                                            m7.Text += "1\n\n";
                                        else if (k == 8)
                                            m8.Text += "1\n\n";
                                        else if (k == 9)
                                            m9.Text += "1\n\n";
                                        else if (k == 10)
                                            m10.Text += "1\n\n";
                                        else if (k == 11)
                                            m11.Text += "1\n\n";
                                        else if (k == 12)
                                            m12.Text += "1\n\n";
                                        else if (k == 13)
                                            m13.Text += "1\n\n";
                                        else if (k == 14)
                                            m14.Text += "1\n\n";
                                        else if (k == 15)
                                            m15.Text += "1\n\n";
                                        else if (k == 16)
                                            m16.Text += "1\n\n";
                                        else if (k == 17)
                                            m17.Text += "1\n\n";
                                        else if (k == 18)
                                            m18.Text += "1\n\n";
                                        else if (k == 19)
                                            m19.Text += "1\n\n";
                                        else if (k == 20)
                                            m20.Text += "1\n\n";
                                        else if (k == 21)
                                            m21.Text += "1\n\n";
                                        else if (k == 22)
                                            m22.Text += "1\n\n";
                                        else if (k == 23)
                                            m23.Text += "1\n\n";
                                        else if (k == 24)
                                            m24.Text += "1\n\n";
                                        else if (k == 25)
                                            m25.Text += "1\n\n";
                                        else if (k == 26)
                                            m26.Text += "1\n\n";
                                        else if (k == 27)
                                            m27.Text += "1\n\n";
                                        else if (k == 28)
                                            m28.Text += "1\n\n";
                                        else if (k == 29)
                                            m29.Text += "1\n\n";
                                        else if (k == 30)
                                            m30.Text += "1\n\n";
                                        else if (k == 31)
                                            m31.Text += "1\n\n";
                                    }
                                    else
                                    {
                                        if (k == 1)
                                            if (k < d2)
                                                m1.Text += "A\n\n";
                                            else
                                                m1.Text += "-\n\n";
                                        else if (k == 2)
                                            if (k < d2)
                                                m2.Text += "A\n\n";
                                            else
                                                m2.Text += "-\n\n";
                                        else if (k == 3)
                                            if (k < d2)
                                                m3.Text += "A\n\n";
                                            else
                                                m3.Text += "-\n\n";
                                        else if (k == 4)
                                            if (k < d2)
                                                m4.Text += "A\n\n";
                                            else
                                                m4.Text += "-\n\n";
                                        else if (k == 5)
                                            if (k < d2)
                                                m5.Text += "A\n\n";
                                            else
                                                m5.Text += "-\n\n";
                                        else if (k == 6)
                                            if (k < d2)
                                                m6.Text += "A\n\n";
                                            else
                                                m6.Text += "-\n\n";
                                        else if (k == 7)
                                            if (k < d2)
                                                m7.Text += "A\n\n";
                                            else
                                                m7.Text += "-\n\n";
                                        else if (k == 8)
                                            if (k < d2)
                                                m8.Text += "A\n\n";
                                            else
                                                m8.Text += "-\n\n";
                                        else if (k == 9)
                                            if (k < d2)
                                                m9.Text += "A\n\n";
                                            else
                                                m9.Text += "-\n\n";
                                        else if (k == 10)
                                            if (k < d2)
                                                m10.Text += "A\n\n";
                                            else
                                                m10.Text += "-\n\n";
                                        else if (k == 11)
                                            if (k < d2)
                                                m11.Text += "A\n\n";
                                            else
                                                m11.Text += "-\n\n";
                                        else if (k == 12)
                                            if (k < d2)
                                                m12.Text += "A\n\n";
                                            else
                                                m12.Text += "-\n\n";
                                        else if (k == 13)
                                            if (k < d2)
                                                m13.Text += "A\n\n";
                                            else
                                                m13.Text += "-\n\n";
                                        else if (k == 14)
                                            if (k < d2)
                                                m14.Text += "A\n\n";
                                            else
                                                m14.Text += "-\n\n";
                                        else if (k == 15)
                                            if (k < d2)
                                                m15.Text += "A\n\n";
                                            else
                                                m15.Text += "-\n\n";
                                        else if (k == 16)
                                            if (k < d2)
                                                m16.Text += "A\n\n";
                                            else
                                                m16.Text += "-\n\n";
                                        else if (k == 17)
                                            if (k < d2)
                                                m17.Text += "A\n\n";
                                            else
                                                m17.Text += "-\n\n";
                                        else if (k == 18)
                                            if (k < d2)
                                                m18.Text += "A\n\n";
                                            else
                                                m18.Text += "-\n\n";
                                        else if (k == 19)
                                            if (k < d2)
                                                m19.Text += "A\n\n";
                                            else
                                                m19.Text += "-\n\n";
                                        else if (k == 20)
                                            if (k < d2)
                                                m20.Text += "A\n\n";
                                            else
                                                m20.Text += "-\n\n";
                                        else if (k == 21)
                                            if (k < d2)
                                                m21.Text += "A\n\n";
                                            else
                                                m21.Text += "-\n\n";
                                        else if (k == 22)
                                            if (k < d2)
                                                m22.Text += "A\n\n";
                                            else
                                                m22.Text += "-\n\n";
                                        else if (k == 23)
                                            if (k < d2)
                                                m23.Text += "A\n\n";
                                            else
                                                m23.Text += "-\n\n";
                                        else if (k == 24)
                                            if (k < d2)
                                                m24.Text += "A\n\n";
                                            else
                                                m24.Text += "-\n\n";
                                        else if (k == 25)
                                            if (k < d2)
                                                m25.Text += "A\n\n";
                                            else
                                                m25.Text += "-\n\n";
                                        else if (k == 26)
                                            if (k < d2)
                                                m26.Text += "A\n\n";
                                            else
                                                m26.Text += "-\n\n";
                                        else if (k == 27)
                                            if (k < d2)
                                                m27.Text += "A\n\n";
                                            else
                                                m27.Text += "-\n\n";
                                        else if (k == 28)
                                            if (k < d2)
                                                m28.Text += "A\n\n";
                                            else
                                                m28.Text += "-\n\n";
                                        else if (k == 29)
                                            if (k < d2)
                                                m29.Text += "A\n\n";
                                            else
                                                m29.Text += "-\n\n";
                                        else if (k == 30)
                                            if (k < d2)
                                                m30.Text += "A\n\n";
                                            else
                                                m30.Text += "-\n\n";
                                        else if (k == 31)
                                            if (k < d2)
                                                m31.Text += "A\n\n";
                                            else
                                                m31.Text += "-\n\n";
                                    }
                                } con.Close();

                            }
            }









            t_name.Visible = false;
            l_enterroll.Visible = false;
            l_entername.Visible = false;
            t_roll.Visible = false;
            t_name.Visible = false;
            b_submit.Visible = false;
            l_putfinger.Visible = false;
            pictureBox1.Visible = false;
            l_dataenrolled.Visible = false;

            studentlist.Visible = false;

        }

        private void subwise_Click(object sender, EventArgs e)
        {
           
            daywise.Visible = false;
            subwise.Visible = false;

            m1.Text = "";
            m5.Text = "";
            m8.Text = "";
            m13.Text = "";
            m18.Text = "";
          


            con.Open();
                                    SqlCommand selcmd6= new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.subject1", con);

                                    SqlDataReader rdr6 = selcmd6.ExecuteReader();

                                    for(int d=1;d<=5;d++)
                                   {
                                       rdr6.Read();
                                        
                                        if(d==1)
                                      m1.Text += ""+rdr6["subname"].ToString()+"\n\n";
                                        else if (d == 2)
                                      m5.Text += "" + rdr6["subname"].ToString() + "\n\n";
                                        else if (d == 3)
                                            m8.Text += "" + rdr6["subname"].ToString() + "\n\n";
                                        else if (d == 4)
                                            m13.Text += "" + rdr6["subname"].ToString() + "\n\n";
                                        else if (d == 5)
                                            m18.Text += "" + rdr6["subname"].ToString() + "\n\n";
                                    }

                                  selcmd6.Dispose();
                                  rdr6.Dispose();
                                  con.Close();

      

           
            rollname.Text = "";

            rollname.Visible = true;
            m1.Visible = true;
            m5.Visible = true;
            m8.Visible = true;
            m13.Visible = true;
            m18.Visible = true;
           

            DateTime datetime = DateTime.Now;


            int mon = datetime.Month;
            int year = datetime.Year;

            con.Open();


            SqlCommand selcmd = new System.Data.SqlClient.SqlCommand("select Id,roll_no,name from studentdatabase.dbo.student3 ", con);

            SqlDataReader rdr = selcmd.ExecuteReader();
            int i = 0;


            while (rdr.Read())
            {

                i++;

            }
            int[] temp = new int[i];
            con.Close();
            con.Open();

            SqlCommand selcmd10 = new System.Data.SqlClient.SqlCommand("select Id,roll_no,name from studentdatabase.dbo.student3 ", con);

            SqlDataReader rdr10 = selcmd10.ExecuteReader();


            i = 0;

            rollname.Text = "\n\n";
            while (rdr10.Read())
            {


                temp[i] = (int)rdr10["Id"];
                i++;


                rollname.Text += rdr10["roll_no"].ToString() + " " + rdr10["name"] + "\n\n";


            }

            con.Close();

            i = i - 1;

            string mon1 = ""; int daysof =0;
            if (mon < 10)
            
                mon1 = "0" + mon.ToString();
               
           
            else
            
                mon1 = mon.ToString();

            if (mon == 2)
            {
                if (year % 4 == 0)
                    daysof = 29;
                else
                    daysof = 28;
            }
            else if (mon < 8 && mon % 2 == 0)
                daysof = 30;
            else if (mon < 8 && mon % 2 == 1)
                daysof = 31;
            else if (mon > 7 && mon % 2 == 0)
                daysof = 31;
            else if (mon > 7 && mon % 2 == 1)
                daysof = 30;


            for (int p = 0; p <= i; p++)
            {
                        for (int k = 1; k <= 5; k++)
                        {
                            int sum = 0;
                            con.Open();
                            SqlCommand selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture1 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + "  AND date1<='" + year + "-" + mon1 + "-"+daysof+"'", con);

                            SqlDataReader rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();

                            selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture2 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + " AND date1<='" + year + "-" + mon1 + "-" + daysof + "'", con);

                            rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();

                            selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture3 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + " AND date1<='" + year + "-" + mon1 + "-" + daysof + "'", con);

                         rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();

                            selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture4 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + " AND date1<='" + year + "-" + mon1 + "-" + daysof + "'", con);

                          rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();


                            selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture5 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + " AND date1<='" + year + "-" + mon1 + "-" + daysof + "'", con);

                            rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();

                            selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture6 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + " AND date1<='" + year + "-" + mon1 + "-" + daysof + "'", con);

                            rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();


                            selcmd1 = new System.Data.SqlClient.SqlCommand("select COUNT(*) as num from studentdatabase.dbo.lecture7 where student_id=" + temp[p] + " AND date1>='" + year + "-" + mon1 + "-01'  AND subject=" + k + "  AND date1<='" + year + "-" + mon1 + "-" + daysof + "'", con);

                            rdr1 = selcmd1.ExecuteReader();

                            rdr1.Read();
                            sum += (int)rdr1["num"];
                            selcmd1.Dispose();
                            rdr1.Dispose();


                            if (k == 1)
                                m1.Text += ""+sum+"\n\n";
                            else if (k == 2)
                                m5.Text += "" +sum+ "\n\n";
                            else if (k == 3)
                                m8.Text += "" +sum+ "\n\n";
                            else if (k == 4)
                                m13.Text += "" +sum+ "\n\n";
                            else if (k == 5)
                                m18.Text += "" +sum+ "\n\n";
                             con.Close();
                                    
                        }
                               
            } 
                                 
        
            
        }

        private void save_Click(object sender, EventArgs e)
        {


            con.Open();
            SqlCommand selcmd1 = new System.Data.SqlClient.SqlCommand("delete from studentdatabase.dbo.subject1 ", con);
            selcmd1.BeginExecuteNonQuery();
            con.Close();


            con.Open();
            DataTable dt1 = new DataTable();
            SqlDataAdapter sda1 = new SqlDataAdapter("select * from studentdatabase.dbo.subject1", con);
            sda1.Fill(dt1);
            DataRow r1 = dt1.NewRow();
            r1[0] = 1;
            r1[1] = "" +s1.Text+"";

            dt1.Rows.Add(r1);
            SqlCommandBuilder cb1= new SqlCommandBuilder(sda1);
            sda1.Update(dt1);

            DataRow r2 = dt1.NewRow();
            r2[0] = 2;
            r2[1] = "" + s2.Text + "";

            dt1.Rows.Add(r2);
            SqlCommandBuilder cb2 = new SqlCommandBuilder(sda1);
            sda1.Update(dt1);



            DataRow r3 = dt1.NewRow();
            r3[0] = 3;
            r3[1] = "" + s3.Text + "";

            dt1.Rows.Add(r3);
            SqlCommandBuilder cb3 = new SqlCommandBuilder(sda1);
            sda1.Update(dt1);

            DataRow r4 = dt1.NewRow();
            r4[0] = 4;
            r4[1] = "" + s4.Text + "";

            dt1.Rows.Add(r4);
            SqlCommandBuilder cb4 = new SqlCommandBuilder(sda1);
            sda1.Update(dt1);


            DataRow r5 = dt1.NewRow();
            r5[0] = 5;
            r5[1] = "" + s5.Text + "";

            dt1.Rows.Add(r5);
            SqlCommandBuilder cb5 = new SqlCommandBuilder(sda1);
            sda1.Update(dt1);

            con.Close();

          
            MessageBox.Show("Subject names are saved for 8th semester");

        }

        private void dropdown_SelectedItemChanged(object sender, EventArgs e)
        {

            con.Open();


            SqlCommand selcmd = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.subject1 ", con);

            SqlDataReader rdr = selcmd.ExecuteReader();


            while (rdr.Read())
            {
                dd1.Items.Add(rdr["subname"].ToString());
                dd2.Items.Add(rdr["subname"].ToString());
                dd3.Items.Add(rdr["subname"].ToString());
                dd4.Items.Add(rdr["subname"].ToString());
                dd5.Items.Add(rdr["subname"].ToString());
                dd6.Items.Add(rdr["subname"].ToString());
                dd7.Items.Add(rdr["subname"].ToString());
            }
            selcmd.Dispose();
            rdr.Dispose();
            con.Close();


            string day = "";
            day = dropdown.SelectedItem.ToString();

            con.Open();


            SqlCommand selcmd1 = new System.Data.SqlClient.SqlCommand("select lec_id,subject_" + day + " from studentdatabase.dbo.lec_sub1", con);

            SqlDataReader rdr1 = selcmd1.ExecuteReader();

            int[] arr = new int[7];
            int i = 0;

            while (rdr1.Read())
            {
                arr[i] = (int)rdr1["subject_" + day + ""];
                i++;
            }
            selcmd1.Dispose();
            rdr1.Dispose();
            con.Close();


            for (i = 0; i < 7; i++)
            {

                con.Open();

                SqlCommand selcmd2 = new System.Data.SqlClient.SqlCommand("select * from studentdatabase.dbo.subject1 where sub_id=" + arr[i] + "", con);

                SqlDataReader rdr2 = selcmd2.ExecuteReader();



                if (i == 0 && rdr2.Read())
                    dd1.Text = rdr2["subname"].ToString();
                else if (i == 1 && rdr2.Read())
                    dd2.Text = rdr2["subname"].ToString();
                else if (i == 2 && rdr2.Read())
                    dd3.Text = rdr2["subname"].ToString();
                else if (i == 3 && rdr2.Read())
                    dd4.Text = rdr2["subname"].ToString();
                else if (i == 4 && rdr2.Read())
                    dd5.Text = rdr2["subname"].ToString();
                else if (i == 5 && rdr2.Read())
                    dd6.Text = rdr2["subname"].ToString();
                else if (i == 6 && rdr2.Read())
                    dd7.Text = rdr2["subname"].ToString();

                selcmd2.Dispose();
                rdr2.Dispose();
                con.Close();
            }



        }

        private void saveme_Click(object sender, EventArgs e)
        {
            string str = "";
            if (dd1.SelectedItem == null)
                str = dd1.Text;
            else
                str = dd1.SelectedItem.ToString();

            con.Open();

            SqlCommand selcmd1 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" +str+ "'", con);

            SqlDataReader rdr1 = selcmd1.ExecuteReader();
            int read = 0;
            if (rdr1.Read())
            {
                read=(int)rdr1["sub_id"];
            }
            selcmd1.Dispose();
            rdr1.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd2 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_"+dropdown.SelectedItem.ToString()+"=" +read + " where lec_id=1", con);

           SqlDataReader rdr2 = selcmd2.ExecuteReader();
         
           if (rdr2.Read())
           {
              
           }
           selcmd2.Dispose();
           rdr2.Dispose();
           con.Close();

           if (dd2.SelectedItem == null)
               str = dd2.Text;
           else
               str = dd2.SelectedItem.ToString();
           con.Open();

           SqlCommand selcmd3 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" + str + "'", con);

           SqlDataReader rdr3 = selcmd3.ExecuteReader();
          
           if (rdr3.Read())
           {
               read = (int)rdr3["sub_id"];
           }
           selcmd3.Dispose();
           rdr3.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd4 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_" + dropdown.SelectedItem.ToString() + "=" + read + " where lec_id=2", con);

           SqlDataReader rdr4 = selcmd4.ExecuteReader();

           if (rdr4.Read())
           {

           }
           selcmd4.Dispose();
           rdr4.Dispose();
           con.Close();


           if (dd3.SelectedItem == null)
               str = dd3.Text;
           else
               str = dd3.SelectedItem.ToString();
           con.Open();

           SqlCommand selcmd5 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" + str + "'", con);

           SqlDataReader rdr5 = selcmd5.ExecuteReader();

           if (rdr5.Read())
           {
               read = (int)rdr5["sub_id"];
           }
           selcmd5.Dispose();
           rdr5.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd6 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_" + dropdown.SelectedItem.ToString() + "=" + read + " where lec_id=3", con);

           SqlDataReader rdr6 = selcmd6.ExecuteReader();

           if (rdr6.Read())
           {

           }
           selcmd6.Dispose();
           rdr6.Dispose();
           con.Close();



           if (dd4.SelectedItem == null)
               str = dd4.Text;
           else
               str = dd4.SelectedItem.ToString();
           con.Open();

           SqlCommand selcmd7 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" + str + "'", con);

           SqlDataReader rdr7= selcmd7.ExecuteReader();

           if (rdr7.Read())
           {
               read = (int)rdr7["sub_id"];
           }
           selcmd7.Dispose();
           rdr7.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd8 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_" + dropdown.SelectedItem.ToString() + "=" + read + " where lec_id=4", con);

           SqlDataReader rdr8 = selcmd8.ExecuteReader();

           if (rdr8.Read())
           {

           }
           selcmd8.Dispose();
           rdr8.Dispose();
           con.Close();



           if (dd5.SelectedItem == null)
               str = dd5.Text;
           else
               str = dd5.SelectedItem.ToString();
           con.Open();

           SqlCommand selcmd9 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" + str + "'", con);

           SqlDataReader rdr9 = selcmd9.ExecuteReader();

           if (rdr9.Read())
           {
               read = (int)rdr9["sub_id"];
           }
           selcmd9.Dispose();
           rdr9.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd10 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_" + dropdown.SelectedItem.ToString() + "=" + read + " where lec_id=5", con);

           SqlDataReader rdr10 = selcmd10.ExecuteReader();

           if (rdr10.Read())
           {

           }
           selcmd10.Dispose();
           rdr10.Dispose();
           con.Close();


           if (dd6.SelectedItem == null)
               str = dd6.Text;
           else
               str = dd6.SelectedItem.ToString();
           con.Open();

           SqlCommand selcmd11 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" + str + "'", con);

           SqlDataReader rdr11 = selcmd11.ExecuteReader();

           if (rdr11.Read())
           {
               read = (int)rdr11["sub_id"];
           }
           selcmd11.Dispose();
           rdr11.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd12 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_" + dropdown.SelectedItem.ToString() + "=" + read + " where lec_id=6", con);

           SqlDataReader rdr12 = selcmd12.ExecuteReader();

           if (rdr12.Read())
           {

           }
           selcmd12.Dispose();
           rdr12.Dispose();
           con.Close();


           if (dd7.SelectedItem == null)
               str = dd7.Text;
           else
               str = dd7.SelectedItem.ToString();
           con.Open();

           SqlCommand selcmd13 = new System.Data.SqlClient.SqlCommand("select sub_id from studentdatabase.dbo.subject1 where subname='" + str + "'", con);

           SqlDataReader rdr13 = selcmd13.ExecuteReader();

           if (rdr13.Read())
           {
               read = (int)rdr13["sub_id"];
           }
           selcmd13.Dispose();
           rdr13.Dispose();
           con.Close();

           con.Open();

           SqlCommand selcmd14 = new System.Data.SqlClient.SqlCommand("update studentdatabase.dbo.lec_sub1 set subject_" + dropdown.SelectedItem.ToString() + "=" + read + " where lec_id=7", con);

           SqlDataReader rdr14 = selcmd14.ExecuteReader();

           if (rdr14.Read())
           {

           }
           selcmd14.Dispose();
           rdr14.Dispose();
           con.Close();
        } 


      
      
       

       

       

    }
}
