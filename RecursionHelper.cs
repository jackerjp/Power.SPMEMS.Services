using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power.SPMEMS.Services
{
    class RecursionHelper
    {
        string Title = "";
        string parent_wbs_guid = "";
        public static void SelParent(string parent_wbs_guid, ref string Title)
        {
            SqlDataBase dbSQL = new SqlDataBase();
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendLine("select  wbs_name,parent_wbs_guid  from pln_projwbs where wbs_guid=@wbs_guid and proj_id=8");
            dbSQL.addParam("wbs_guid", parent_wbs_guid);
            DataSet dsSQL = dbSQL.getDataSet(sSQL.ToString());
            if (dsSQL.Tables[0].Rows.Count > 0)
            {
                Title = dsSQL.Tables[0].Rows[0]["wbs_name"].ToString() + "," + Title;
                SelParent(dsSQL.Tables[0].Rows[0]["parent_wbs_guid"].ToString(), ref Title);
            }
        }
    }
}
