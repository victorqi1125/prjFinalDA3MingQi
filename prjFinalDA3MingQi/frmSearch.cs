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
    public partial class frmSearch : Form
    {
        public frmSearch()
        {
            InitializeComponent();
        }
        DataTable tabStudent,tabCourse;

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ckGender.Checked == true && ckCourse.Checked == false)
            {
                var selectg = from sl in tabStudent.AsEnumerable()
                             where (sl.Field<string>("Gender") == cbGender.Text.ToString())
                             select new
                     {
                         Names = sl.Field<string>("FullName"),
                         Birthdate = sl.Field<DateTime>("Birthdate"),
                         Genders = sl.Field<string>("Gender"),
                         Average = sl.Field<int>("Average")
                     };

                dataGridView1.DataSource = (selectg.Count() > 0) ?
                    selectg.ToList() : null;
            }
            else if (ckGender.Checked == false && ckCourse.Checked == false)
            {
                dataGridView1.DataSource = tabStudent.AsEnumerable().CopyToDataTable();
            }
            else if (ckGender.Checked == false && ckCourse.Checked == true)
            {
                var selectc = from sl in tabStudent.AsEnumerable()
                             where (sl.Field<int>("ReferCourse") == Convert.ToInt32(cbCourse.SelectedValue))
                              select new
                              {
                                  Names = sl.Field<string>("FullName"),
                                  Birthdate = sl.Field<DateTime>("Birthdate"),
                                  Genders = sl.Field<string>("Gender"),
                                  Average = sl.Field<int>("Average")
                              };

                dataGridView1.DataSource = (selectc.Count() > 0) ?
                    selectc.ToList() : null;
            }
            else if (ckGender.Checked == true && ckCourse.Checked == true)
            {
                var selectcg = from sl in tabStudent.AsEnumerable()
                             where (sl.Field<string>("Gender") == cbGender.Text.ToString()
                             && sl.Field<int>("ReferCourse") == Convert.ToInt32(cbCourse.SelectedValue))
                               select new
                               {
                                   Names = sl.Field<string>("FullName"),
                                   Birthdate = sl.Field<DateTime>("Birthdate"),
                                   Genders = sl.Field<string>("Gender"),
                                   Average = sl.Field<int>("Average")
                               };

                dataGridView1.DataSource = (selectcg.Count()>0)?
                    selectcg.ToList() : null;
            }
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            tabStudent = clsGlobal.mySet.Tables["Students"];

            tabCourse = clsGlobal.mySet.Tables["Courses"];
            var allGender = from gender in tabStudent.AsEnumerable().Distinct()
                            select new { Gender = gender.Field<string>("Gender") };
            cbGender.DataSource = allGender.ToList();
            cbGender.DisplayMember = "Gender";

            var CourN = from cn in tabCourse.AsEnumerable()
                        select new
                        {
                            CourseTitle = cn.Field<string>("Title"),
                            RefCourse = cn.Field<int>("RefCourse")
                        };
            cbCourse.DataSource = CourN.ToList();
            cbCourse.DisplayMember = "CourseTitle";
            cbCourse.ValueMember = "RefCourse";


            dataGridView1.DataSource = tabStudent.AsEnumerable().CopyToDataTable();
        }
    }
}
