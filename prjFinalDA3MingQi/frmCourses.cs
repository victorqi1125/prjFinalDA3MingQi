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
    public partial class frmCourses : Form
    {
        public frmCourses()
        {
            InitializeComponent();
        }
        DataTable tabCourse, tabStudent, tabTeacher;
        int currentIndex;
        string mode;
        private void frmCourses_Load(object sender, EventArgs e)
        {
            tabCourse = clsGlobal.mySet.Tables["Courses"];

            tabStudent = clsGlobal.mySet.Tables["Students"];

            tabTeacher = clsGlobal.mySet.Tables["Teacher"];

            var TeachN = from tn in tabTeacher.AsEnumerable()
                         select new
                         {
                             TeacherName = tn.Field<string>("FullName"),
                             RefTeacher = tn.Field<int>("RefTeacher")
                         };
            cboTeacher.DataSource = TeachN.ToList();
            cboTeacher.DisplayMember = "TeacherName";
            cboTeacher.ValueMember = "RefTeacher";
            currentIndex = 0;
            Display();
            ActivateButtons(true, true, true, false, false, true, true);
            var st = from slst in tabStudent.AsEnumerable()
                     select new
                     {
                         Names = slst.Field<string>("FullName"),
                         Birthdate = slst.Field<DateTime>("Birthdate"),
                         Genders = slst.Field<string>("Gender"),
                         Average = slst.Field<int>("Average")
                     };
            dataGridView1.DataSource = st.ToList();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            currentIndex = 0;
            Display();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                Display();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex < tabCourse.Rows.Count - 1)
            {
                currentIndex++;
                Display();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            currentIndex = tabCourse.Rows.Count - 1;
            Display();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmTeachers myTeacher = new frmTeachers();
            myTeacher.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mode = "add";
            txtNumber.Text = txtTitle.Text = txtDuration.Text  = cboTeacher.Text= "";
            
            txtNumber.Focus();
            lblInfo.Text = "-----Adding Mode-----";
            ActivateButtons(false, false, false, true, true, false, false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            mode = "edit";


            txtNumber.Focus();
            lblInfo.Text = "-----Editing Mode-----";
            ActivateButtons(false, false, false, true, true, false, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Course Info?", "Course Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Delete the record(or datarow) positioned at the currentindex
                tabCourse.Rows[currentIndex].Delete();
                //Save (or synchronize) the contents of the mySet.tables["Course"] to the database
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpCourse);
                clsGlobal.adpCourse.Update(clsGlobal.mySet, "Courses");
                //Refrresh the datatable with the content of the database in the case of changes
                clsGlobal.mySet.Tables.Remove("Courses");
                clsGlobal.adpCourse.Fill(clsGlobal.mySet, "Courses");

                tabCourse = clsGlobal.mySet.Tables["Courses"];

                currentIndex = 0;
                Display();

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataRow currentRow;
            if (mode == "add")
            {
                if (clsGlobal.mySet.Tables["Courses"].Columns["Number"]
                    .ColumnName.Contains(txtNumber.Text)!=true)
                {
                    currentRow = tabCourse.NewRow();
                    currentRow["Number"] = txtNumber.Text;
                    currentRow["Title"] = txtTitle.Text;
                    currentRow["Duration"] = txtDuration.Text;
                    currentRow["ReferTeacher"] = cboTeacher.SelectedValue;

                    tabCourse.Rows.Add(currentRow);

                    //Save (or synchronize) the contents of the mySet.tables["Student"] to the database
                    OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpCourse);
                    clsGlobal.adpCourse.Update(clsGlobal.mySet, "Courses");
                    //Refrresh the datatable with the content of the database in the case of changes
                    clsGlobal.mySet.Tables.Remove("Courses");
                    clsGlobal.adpCourse.Fill(clsGlobal.mySet, "Courses");

                    tabCourse = clsGlobal.mySet.Tables["Courses"];
                    currentIndex = tabCourse.Rows.Count - 1;
                }
                else
                {
                    MessageBox.Show("This Course already exists", "Adding Failed");
                    txtNumber.Focus();
                    return;
                }
            }
            else if (mode == "edit")
            {
                currentRow = tabCourse.Rows[currentIndex];
                currentRow["Number"] = txtNumber.Text;
                currentRow["Title"] = txtTitle.Text;
                currentRow["Duration"] = txtDuration.Text;
                currentRow["ReferTeacher"] = cboTeacher.SelectedValue;

                tabCourse.Rows.Add(currentRow);
                //Save (or synchronize) the contents of the mySet.tables["Course"] to the database
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpCourse);
                clsGlobal.adpCourse.Update(clsGlobal.mySet, "Courses");
                //Refrresh the datatable with the content of the database in the case of changes
                clsGlobal.mySet.Tables.Remove("Courses");
                clsGlobal.adpCourse.Fill(clsGlobal.mySet, "Courses");

                tabCourse = clsGlobal.mySet.Tables["Courses"];
            }
            Display();
            ActivateButtons(true, true, true, false, false, true, true);
        }

        private void ActivateButtons(bool Add, bool Edit, bool Del, bool Save, bool Cancel, bool Navigates, bool Close)
        {
            btnAdd.Enabled = Add;
            btnEdit.Enabled = Edit;
            btnDelete.Enabled = Del;
            btnSave.Enabled = Save;
            btnCancel.Enabled = Cancel;
            btnFirst.Enabled = btnPrevious.Enabled = btnNext.Enabled
                = btnLast.Enabled = Navigates;
            btnExit.Enabled = Close;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Display();
            ActivateButtons(true, true, true, false, false, true, true);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Display()
        {
            txtNumber.Text = tabCourse.Rows[currentIndex]["Number"].ToString();
            txtTitle.Text = tabCourse.Rows[currentIndex]["Title"].ToString();
            txtDuration.Text= tabCourse.Rows[currentIndex]["Duration"].ToString();


            cboTeacher.SelectedValue = Convert.ToInt32(tabCourse.Rows[currentIndex]["ReferTeacher"].ToString());

            dataGridView1.DataSource = tabStudent.AsEnumerable().
                Where(x => x.Field<int>("ReferCourse") ==
                Convert.ToInt32(tabCourse.Rows[currentIndex]["RefCourse"].ToString()))
                .CopyToDataTable();

            lblInfo.Text = "Courses No. " + (currentIndex + 1) + " on a Total of " + tabCourse.Rows.Count;
        }
    }
}
