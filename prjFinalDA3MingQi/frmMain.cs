using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using prjFinalDA3MingQi.DataSource;
using System.Data.OleDb;

namespace prjFinalDA3MingQi
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Create a global dataset
            clsGlobal.mySet = new DataSet();

            //open connection
            clsGlobal.myCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\victo\source\repos\mutliTier\prjFinalDA3MingQi\prjFinalDA3MingQi\DataSource\DB_College.mdb;Persist Security Info=True");
            clsGlobal.myCon.Open();

            //recuperate and insert the table Houses from database to dataset
            OleDbCommand myCmd = new OleDbCommand("SELECT * from Teacher", clsGlobal.myCon);
            clsGlobal.adpTeacher = new OleDbDataAdapter(myCmd);
            clsGlobal.adpTeacher.Fill(clsGlobal.mySet, "Teacher");

            //recuperate and insert the table Client from database to dataset
            myCmd = new OleDbCommand("SELECT * from Courses", clsGlobal.myCon);
            clsGlobal.adpCourse = new OleDbDataAdapter(myCmd);
            clsGlobal.adpCourse.Fill(clsGlobal.mySet, "Courses");

            //recuperate and insert the table Employee from database to dataset
            myCmd = new OleDbCommand("SELECT * from Students", clsGlobal.myCon);
            clsGlobal.adpStudent = new OleDbDataAdapter(myCmd);
            clsGlobal.adpStudent.Fill(clsGlobal.mySet, "Students");
        }

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string info = "Are you sure you want to quit?";
            string title = "Application Notice";

            if (MessageBox.Show(info, title, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch mySearch = new frmSearch();
            mySearch.MdiParent = this;
            mySearch.Show();
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCourses myCourse = new frmCourses();
            myCourse.MdiParent = this;
            myCourse.Show();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudents myStudent = new frmStudents();
            myStudent.MdiParent = this;
            myStudent.Show();
        }
    }
}
