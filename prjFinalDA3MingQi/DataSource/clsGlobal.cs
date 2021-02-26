using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace prjFinalDA3MingQi.DataSource
{
    public static class clsGlobal
    {
        //Declaring global variables accessible around the whole project
        public static DataSet mySet;
        public static OleDbConnection myCon;
        public static OleDbDataAdapter adpTeacher, adpCourse, adpStudent;
    }
}
