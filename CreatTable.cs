using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LinkedIn_for_sqlserver
{
    class CreatTable
    {
        public void creatdt(DataTable dt, int flag)
        {
            DataColumn workCol;
            if (flag == 1)
            {
                workCol = dt.Columns.Add("id", typeof(Int32));
                workCol.AllowDBNull = false;
                workCol.Unique = true;
                workCol.AutoIncrement = true;
                workCol.AutoIncrementSeed = 0;
                workCol.AutoIncrementStep = 1;
                dt.Columns.Add("username", typeof(string));
                workCol = dt.Columns.Add("userlink", typeof(string));
                workCol.Unique = true;
                dt.Columns.Add("time", typeof(System.DateTime));
                dt.Columns.Add("flag", typeof(int));
            }
            else if(flag == 2)
            {
                workCol = dt.Columns.Add("id", typeof(Int32));
                workCol.AllowDBNull = false;
                workCol.Unique = true;
                workCol.AutoIncrement = true;
                workCol.AutoIncrementSeed = 0;
                workCol.AutoIncrementStep = 1;
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Headline", typeof(string));
                dt.Columns.Add("Locality", typeof(string));
                dt.Columns.Add("Industry", typeof(string));
                dt.Columns.Add("Current", typeof(string));
                dt.Columns.Add("Previous", typeof(string));
                dt.Columns.Add("Education", typeof(string));
                dt.Columns.Add("Recommend", typeof(string));
                dt.Columns.Add("Connection", typeof(string));
                dt.Columns.Add("Summary", typeof(string));
                dt.Columns.Add("Experience", typeof(string));
                dt.Columns.Add("Project", typeof(string));
                dt.Columns.Add("Skill", typeof(string));
                dt.Columns.Add("Edufull", typeof(string));
                workCol = dt.Columns.Add("Link", typeof(string));
                workCol.Unique = true;
                dt.Columns.Add("Time", typeof(System.DateTime));
            }
            else if(flag == 3)
            {
                workCol = dt.Columns.Add("SOFID", typeof(Int32));
                workCol.AllowDBNull = false;
                workCol.Unique = true;
                dt.Columns.Add("Name", typeof(string));
                workCol = dt.Columns.Add("Avatar", typeof(string));
                workCol.Unique = false;
                workCol = dt.Columns.Add("Link", typeof(string));
                workCol.Unique = true;
                dt.Columns.Add("Position", typeof(string));
                dt.Columns.Add("Company", typeof(string));
                dt.Columns.Add("Location", typeof(string));
                dt.Columns.Add("Website", typeof(string));
                dt.Columns.Add("Technologies", typeof(string));
                dt.Columns.Add("Experience", typeof(string));
                dt.Columns.Add("Education", typeof(string));
                dt.Columns.Add("Time", typeof(System.DateTime));
            }
        }
    }
}
