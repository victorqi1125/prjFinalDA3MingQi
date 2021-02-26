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
    public partial class frmStudents : Form
    {
        public frmStudents()
        {
            InitializeComponent();
        }
        DataTable tabCourse, tabStudent, tabTeacher;
        int currentIndex;
        string mode;
        private void frmStudents_Load(object sender, EventArgs e)
        {
            tabCourse = clsGlobal.mySet.Tables["Courses"];

            tabStudent = clsGlobal.mySet.Tables["Students"];

            tabTeacher = clsGlobal.mySet.Tables["Teacher"];

            var CourN = from cn in tabCourse.AsEnumerable()
                        select new
                        {
                            CourseTitle = cn.Field<string>("Title"),
                            RefCourse = cn.Field<int>("RefCourse")
                        };
            cbCourse.DataSource = CourN.ToList();
            cbCourse.DisplayMember = "CourseTitle";
            cbCourse.ValueMember = "RefCourse";

            currentIndex = 0;
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
            if (currentIndex < tabStudent.Rows.Count - 1)
            {
                currentIndex++;
                Display();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            currentIndex = tabStudent.Rows.Count - 1;
            Display();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mode = "add";

            txtName.Text = txtGender.Text =  cbCourse.Text = "";
            dtpBirth.ResetText();
            txtName.Focus();
            lblInfo.Text = "-----Adding Mode-----";
            ActivateButtons(false, false, false, true, true, false, false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            mode = "edit";

            txtName.Focus();
            lblInfo.Text = "-----Editing Mode-----";
            ActivateButtons(false, false, false, true, true, false, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Student Info?", "Info Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Delete the record(or datarow) positioned at the currentindex
                tabStudent.Rows[currentIndex].Delete();
                //Save (or synchronize) the contents of the mySet.tables["Student"] to the database
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpStudent);
                clsGlobal.adpStudent.Update(clsGlobal.mySet, "Students");
                //Refrresh the datatable with the content of the database in the case of changes
                clsGlobal.mySet.Tables.Remove("Students");
                clsGlobal.adpStudent.Fill(clsGlobal.mySet, "Students");

                tabStudent = clsGlobal.mySet.Tables["Students"];

                currentIndex = 0;
                Display();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataRow currentRow;
            if (mode == "add")
            {
                if (clsGlobal.mySet.Tables["Students"].Columns["FullName"]
                    .ColumnName.Contains(txtName.Text) != true)
                {
                    currentRow = tabStudent.NewRow();
                    currentRow["FullName"] = txtName.Text;
                    currentRow["Gender"] = txtGender.Text;
                    currentRow["Birthdate"] =dtpBirth.Value;
                    currentRow["ReferCourse"] = cbCourse.SelectedValue;

                    tabStudent.Rows.Add(currentRow);

                    //Save (or synchronize) the contents of the mySet.tables["Student"] to the database
                    OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpStudent);
                    clsGlobal.adpStudent.Update(clsGlobal.mySet, "Students");
                    //Refrresh the datatable with the content of the database in the case of changes
                    clsGlobal.mySet.Tables.Remove("Students");
                    clsGlobal.adpStudent.Fill(clsGlobal.mySet, "Students");

                    tabStudent = clsGlobal.mySet.Tables["Students"];
                    currentIndex = tabStudent.Rows.Count - 1;
                }
                else
                {
                    MessageBox.Show("This Student already exists", "Adding Failed");
                    txtName.Focus();
                    return;
                }
            }
            else if (mode == "edit")
            {
                currentRow = tabStudent.Rows[currentIndex];
                currentRow["FullName"] = txtName.Text;
                currentRow["Gender"] = txtGender.Text;
                currentRow["Birthdate"] = dtpBirth.Value;
                currentRow["ReferCourse"] = cbCourse.SelectedValue;

                tabStudent.Rows.Add(currentRow);
                //Save (or synchronize) the contents of the mySet.tables["Student"] to the database
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpStudent);
                clsGlobal.adpStudent.Update(clsGlobal.mySet, "Students");
                //Refrresh the datatable with the content of the database in the case of changes
                clsGlobal.mySet.Tables.Remove("Students");
                clsGlobal.adpStudent.Fill(clsGlobal.mySet, "Students");

                tabStudent = clsGlobal.mySet.Tables["Students"];
            }
            Display();
            ActivateButtons(true, true, true, false, false, true, true);
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
            txtName.Text = tabStudent.Rows[currentIndex]["FullName"].ToString();
            dtpBirth.Value = Convert.ToDateTime(tabStudent.Rows[currentIndex]["Birthdate"].ToString());
            txtGender.Text = tabStudent.Rows[currentIndex]["Gender"].ToString();


            cbCourse.SelectedValue = Convert.ToInt32(tabStudent.Rows[currentIndex]["ReferCourse"].ToString());


            lblInfo.Text = "Student No. " + (currentIndex + 1) + " on a Total of " + tabStudent.Rows.Count;
        }


    }
}
