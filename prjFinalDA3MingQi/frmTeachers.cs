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
    public partial class frmTeachers : Form
    {
        public frmTeachers()
        {
            InitializeComponent();
        }
        DataTable tabTeacher;
        int currentIndex;
        string mode;
        private void frmTeachers_Load(object sender, EventArgs e)
        {

            tabTeacher = clsGlobal.mySet.Tables["Teacher"];

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
        private void Display()
        {
            txtName.Text = tabTeacher.Rows[currentIndex]["FullName"].ToString();
            txtEmail.Text = tabTeacher.Rows[currentIndex]["Email"].ToString();
            txtSalary.Text = tabTeacher.Rows[currentIndex]["Salary"].ToString();


            


            lblInfo.Text = "Teacher No. " + (currentIndex + 1) + " on a Total of " + tabTeacher.Rows.Count;
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
            if (currentIndex < tabTeacher.Rows.Count - 1)
            {
                currentIndex++;
                Display();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            currentIndex = tabTeacher.Rows.Count - 1;
            Display();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            mode = "add";

            txtName.Text = txtEmail.Text = txtSalary.Text = "";
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
            if (MessageBox.Show("Are you sure you want to delete this Teacher Info?", "Info Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                //Delete the record(or datarow) positioned at the currentindex
                tabTeacher.Rows[currentIndex].Delete();
                //Save (or synchronize) the contents of the mySet.tables["Teacher"] to the database
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpTeacher);
                clsGlobal.adpTeacher.Update(clsGlobal.mySet, "Teacher");
                //Refrresh the datatable with the content of the database in the case of changes
                clsGlobal.mySet.Tables.Remove("Teacher");
                clsGlobal.adpTeacher.Fill(clsGlobal.mySet, "Teacher");

                tabTeacher = clsGlobal.mySet.Tables["Teacher"];

                currentIndex = 0;
                Display();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            DataRow currentRow;
            if (mode == "add")
            {
                if (clsGlobal.mySet.Tables["Teacher"].Columns["FullName"]
                    .ColumnName.Contains(txtName.Text) != true)
                {
                    currentRow = tabTeacher.NewRow();
                    currentRow["FullName"] = txtName.Text;
                    currentRow["Email"] = txtEmail.Text;
                    currentRow["Salary"] = Convert.ToDecimal(txtSalary.Text);

                    tabTeacher.Rows.Add(currentRow);

                    //Save (or synchronize) the contents of the mySet.tables["Student"] to the database
                    OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpTeacher);
                    clsGlobal.adpTeacher.Update(clsGlobal.mySet, "Teacher");
                    //Refrresh the datatable with the content of the database in the case of changes
                    clsGlobal.mySet.Tables.Remove("Teacher");
                    clsGlobal.adpTeacher.Fill(clsGlobal.mySet, "Teacher");

                    tabTeacher = clsGlobal.mySet.Tables["Teacher"];
                    currentIndex = tabTeacher.Rows.Count - 1;
                }
                else
                {
                    MessageBox.Show("This Teacher already exists", "Adding Failed");
                    txtName.Focus();
                    return;
                }
            }
            else if (mode == "edit")
            {
                currentRow = tabTeacher.Rows[currentIndex];
                currentRow["FullName"] = txtName.Text;
                currentRow["Email"] = txtEmail.Text;
                currentRow["Salary"] = Convert.ToDecimal(txtSalary.Text);

                tabTeacher.Rows.Add(currentRow);
                //Save (or synchronize) the contents of the mySet.tables["Teacher"] to the database
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(clsGlobal.adpTeacher);
                clsGlobal.adpTeacher.Update(clsGlobal.mySet, "Teacher");
                //Refrresh the datatable with the content of the database in the case of changes
                clsGlobal.mySet.Tables.Remove("Teacher");
                clsGlobal.adpTeacher.Fill(clsGlobal.mySet, "Teacher");

                tabTeacher = clsGlobal.mySet.Tables["Teacher"];
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
    }
}
