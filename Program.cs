using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Power.SPMEMS.Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using CronNET;
using System.Collections;
using Power.Business;
using XCode.DataAccessLayer;

namespace Power.SPMEMS.Services
{
    class Program
    {
        private static readonly CronDaemon cron_daemon = new CronDaemon();
        static void Main(string[] args)
        {
            //cron_daemon.AddJob("58 15 * * *", ImportQG);
            //cron_daemon.Start();

            //// Wait and sleep forever. Let the cron daemon run.
            //while (true) Thread.Sleep(6000);
            //spm
            ImportQG();
            ImprotCLCRK();
            //ImportQG();
            ImportSbDh();
            ImportFbcldh();
            ImportSckcpd();
            ImportGcclhx();
            ImportFbclhx();
            ImportCmzt();
            ImportFF();

            //ems
            GetSj();

            Power.Controls.StdPlan.PlanControl.GetPlanStatusControlDataForApp()

        public DataTable ReslutData(string KeyWord, string KeyValue)
        {

            // 上传的数据
            DataTable tempFile = new DataTable();

            //找到目标文件对象
            //System.Web.HttpPostedFile uploadFile = this.Context.Request.Files["PCPath"];

            // 如果有文件, 则读取文件信息
            // if (uploadFile.ContentLength > 0)
            //{
            //System.IO.Stream fileDataStream = uploadFile.InputStream;
            //int fileLength = uploadFile.ContentLength;
            // byte[] fileData = new byte[fileLength];
            //通过KeyWord、Keyword找到PB_DocFiles对应的数据
            string[] keys = { "BOKeyWord", "FolderId" };
            string[] values = { KeyWord, KeyValue };
            Power.Systems.Systems.DocFileBO docfile = Power.Business.BusinessFactory.CreateBusinessOperate("DocFile").FindByKey(keys, values) as Power.Systems.Systems.DocFileBO;

            // 查询基础数据中SFTP信息
            String UserId = "", UserPwd = "", Ip = "", Port = "";
            Power.Business.IBusinessOperate dataAct = Power.Business.BusinessFactory.CreateBusinessOperate("BaseData");
            Power.Business.IBusinessList dataList = dataAct.FindAll(" DataType='SFTP' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (dataList.Count > 0)
            {
                Power.Business.IBusinessOperate listAct = Power.Business.BusinessFactory.CreateBusinessOperate("BaseDataList");
                Power.Business.IBusinessList exactList = listAct.FindAll(" BaseDataId='" + dataList[0]["Id"] + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (exactList.Count > 0)
                {
                    foreach (Power.Business.IBaseBusiness item in exactList)
                    {
                        if (item["Code"].ToString() == "UserId") UserId = item["Name"].ToString();
                        if (item["Code"].ToString() == "UserPwd") UserPwd = item["Name"].ToString();
                        if (item["Code"].ToString() == "Ip") Ip = item["Name"].ToString();
                        if (item["Code"].ToString() == "Port") Port = item["Name"].ToString();
                    }
                }
            }

            string filePath = "ftp://127.0.0.1:21" + docfile.ServerUrl;
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            byte[] bt = Power.Global.Files.FtpHelper.FtpfileDownLoad(filePath, "ZX", "Power4005").GetBuffer();
            //20200409补丁之后，请将Power.Global.FtpHelper改为Power.Global.Files.FtpHelper
            foreach (byte item in bt)
            {
                memory.WriteByte(item);
            }

            //System.IO.StreamWriter writer = new System.IO.StreamWriter(memory);
            string fileurl = AppDomain.CurrentDomain.BaseDirectory + "\\" + docfile.Name + docfile.FileExt;
            System.IO.FileStream file = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + docfile.Name + docfile.FileExt, System.IO.FileMode.CreateNew);
            memory.WriteTo(file);

            file.Dispose();
            //writer.Dispose();
            memory.Dispose();
            //把文件流填充到数组   
            //fileDataStream.Read(fileData, 0, fileLength);
            //解码二进制数组
            DataSet ds = Power.Global.PowerGlobal.Office.ExcelToDataSet(AppDomain.CurrentDomain.BaseDirectory + "\\" + docfile.Name + docfile.FileExt);
            //tempFile = GetExcelDatatable(AppDomain.CurrentDomain.BaseDirectory + "\\" + docfile.Name + docfile.FileExt, "dt1");
            tempFile = ds.Tables[0];

            if (System.IO.File.Exists(fileurl))
                System.IO.File.Delete(fileurl);
            docfile.Delete();
            // uploadFile.SaveAs(string.Format("{0}{1}{2}", tempFile, "Images/", uploadFile.FileName));
            // }
            //tempFile = System.IO.File.ReadAllText(@"C:\Data.txt");

            //从ftp下载文件到本地服务器

            //读取服务器上的文件
            return tempFile;
        }

        public string GetPDBH()
        {
            string sYEAR = DateTime.Now.Year.ToString();
            string PDBBH;
            try
            {
                string sSQL = "select max(ContratorCode) as code from NPS_PROJLIST_MainInfo_CBS where  ContratorCode like '" + "%" + sYEAR.PadLeft(4, '0') + "%" + "' ";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (CwData.Rows.Count > 0 && !string.IsNullOrEmpty(CwData.Rows[0]["code"].ToString().Trim()))
                {
                    string sPDBBH = CwData.Rows[0]["code"].ToString().Trim();
                    sPDBBH = sPDBBH.Substring(4, sPDBBH.Length - 4);
                    if (int.Parse(sPDBBH) > 0)
                    {
                        PDBBH = sYEAR + Convert.ToString(int.Parse(sPDBBH) + 1).PadLeft(4, '0');
                        return PDBBH;
                    }
                    else
                    {
                        PDBBH = sYEAR + "0001";
                        return PDBBH;
                    }
                }
                else
                {
                    PDBBH = sYEAR + "0001";
                    return PDBBH;
                }
            }
            catch (Exception ex)
            {
                PDBBH = sYEAR + "0001";
                return PDBBH;
            }
        }

            public string GetFJS(string ID)
            {
                string sResult = string.Empty;
                ArrayList arrayList = new ArrayList();
                string sSQL = "Select 1 From  PB_DocFiles A Where   (0=0)  and (A.FolderId='"+ ID + "' and (A.DeletFlag=0)) Order By  A.RegDate ";
                DataTable dataTable = DAL.QuerySQL(sSQL);

                string SL = dataTable.Rows.Count.ToString();
                return SL;
            }

            ///
            public void UpdateMX(string ID, string FID)
            {

                SqlDataBase dbSQL = new SqlDataBase();
                StringBuilder sSQL = new StringBuilder();
                StringBuilder sPS_MDM = new StringBuilder();
                StringBuilder sBOQ = new StringBuilder();
                StringBuilder UpadateSQL = new StringBuilder();

                sPS_MDM.Clear();
                sPS_MDM.AppendLine("select parent_wbs_guid from pln_projwbs where wbs_short_name='001001001003002002' and proj_id=6 ");
                DataSet dsSQL = dbSQL.getDataSet(sPS_MDM.ToString());

                sPS_MDM.Clear();


                XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
                dal.Execute("delete from PS_DOC_FolderConfig where FID = '" + ID + "' ");

                string sDOC = "";
                sDOC = "insert into PS_DOC_FolderConfig (ID,Code,ConfigField,ConfigFieldName,ParentID,TableName,BizAreaId,Sequ," +
                    "Status,RegHumId,RegHumName,RegDate,RegPosiId,RegDeptId,EpsProjId,RecycleHumId,UpdHumId,UpdHumName,UpdDate," +
                    "ApprHumId,ApprHumName,ApprDate,Remark,OwnProjId,OwnProjName,EpsProjCode,EpsProjName,FID)" +
                    "select NEWID(),Code,ConfigField,ConfigFieldName,ParentID,TableName,BizAreaId,Sequ," +
                    "Status,RegHumId,RegHumName,RegDate,RegPosiId,RegDeptId,EpsProjId,RecycleHumId,UpdHumId,UpdHumName,UpdDate," +
                    "ApprHumId,ApprHumName,ApprDate,Remark,OwnProjId,OwnProjName,EpsProjCode,EpsProjName,'" + ID + "'" +
                    " from PS_DOC_FolderConfig where FID='" + FID + "' ";
                dal.Execute(sDOC);

                string sSQL1 = "select ID from PS_DOC_Folders where ParentId='" + ID + "'";
                DataTable DOC = XCode.DataAccessLayer.DAL.QuerySQL(sSQL1);
                for (int i = 0; i < DOC.Rows.Count; i++)
                {
                    UpdateMX(DOC.Rows[i]["ID"].ToString(), FID);
                }
            }

            public string GetXMList()
            {
                string sResult = string.Empty;
                ArrayList arrayList = new ArrayList();
                string sSQL = " select code as id, name as text from PB_BaseDataList where BaseDataId=(select x.Id from PB_BaseData x where x.DataType='PS_DOC_JYData_Tree') and parentid='861B51DC-DED8-3E7D-EF80-7C0638F21A2F' order by sequ ";
                DataTable dataTable = DAL.QuerySQL(sSQL);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    JObject obj = new JObject();
                    obj.Add("id", dataTable.Rows[i]["id"].ToString().Trim());
                    obj.Add("text", dataTable.Rows[i]["text"].ToString().Trim());
                    arrayList.Add(obj);
                }

                JsonSerializerSettings s_setting = new JsonSerializerSettings();
                sResult = JsonConvert.SerializeObject(arrayList, s_setting);
                return sResult;
            }

            public string GetTYPEList()
            {
                string sResult = string.Empty;
                ArrayList arrayList = new ArrayList();
                string sSQL = " select code as id, name as text from PB_BaseDataList where BaseDataId=(select x.Id from PB_BaseData x where x.DataType='PS_DOC_JYData_Tree') and parentid='456D08AB-8B64-0982-5333-828B95667787' order by sequ ";
                DataTable dataTable = DAL.QuerySQL(sSQL);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    JObject obj = new JObject();
                    obj.Add("id", dataTable.Rows[i]["id"].ToString().Trim());
                    obj.Add("text", dataTable.Rows[i]["text"].ToString().Trim());
                    arrayList.Add(obj);
                }

                JsonSerializerSettings s_setting = new JsonSerializerSettings();
                sResult = JsonConvert.SerializeObject(arrayList, s_setting);
                return sResult;
            }


            public string ImportExcel(string KeyWord, string KeyValue, string FID)
        {
            DataTable data = ReslutData(KeyWord, KeyValue);
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();

            Power.Business.IBusinessList ZxIns = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PROJLIST_MainInfo_CBS").FindAll("Sequ", "-2");

            List<string> CodeList = new List<string>();
            string sSQL = "select ContratorName from NPS_PROJLIST_MainInfo_CBS ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            for (int i = 0; i < CwData.Rows.Count; i++)
            {
                CodeList.Add(CwData.Rows[i]["ContratorName"].ToString());
            }

            float Sequ = 0;
            sSQL = "";
            sSQL = "select Max(Sequ) as Sequ from NPS_PROJLIST_MainInfo_CBS ";
            CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(CwData.Rows[0]["Sequ"].ToString()))
                {
                    Sequ = float.Parse(CwData.Rows[0]["Sequ"].ToString()) + 1;
                }
                else
                {
                    Sequ = 1;
                }
            }

            foreach (DataRow item in data.Rows)
            {
                if (!CodeList.Contains(item["承包商名称"] == null ? "" : item["承包商名称"].ToString()))
                {
                    Power.Business.IBaseBusiness LbItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PROJLIST_MainInfo_CBS");
                    string LbZj = Guid.NewGuid().ToString();
                    LbItem.SetItem("ID", LbZj);
                    LbItem.SetItem("Code", GetPDBH());
                    LbItem.SetItem("ContratorCode", GetPDBH());
                    LbItem.SetItem("ContratorName", item["承包商名称"]==null ? "" : item["承包商名称"].ToString());
                    LbItem.SetItem("CBS_TYPE", item["承包商类别"].ToString() == null ? "" : item["承包商类别"].ToString());
                    LbItem.SetItem("CBS_LXR", item["联系人"].ToString() == null ? "" : item["联系人"].ToString());
                    LbItem.SetItem("CBS_LXDH", item["联系电话"].ToString() == null ? "" : item["联系电话"].ToString());
                    LbItem.SetItem("CBS_EMILE", item["邮箱"].ToString() == null ? "" : item["邮箱"].ToString());
                    LbItem.SetItem("CBS_XCFZR", item["现场负责人"].ToString() == null ? "" : item["现场负责人"].ToString());
                    LbItem.SetItem("CBS_FZRDH", item["现场负责人联系电话"].ToString() == null ? "" : item["现场负责人联系电话"].ToString());
                    LbItem.SetItem("CBS_SFQDHT", item["是否签订合同"].ToString() == null ? "" : item["是否签订合同"].ToString());
                    LbItem.SetItem("CBS_BZ", item["备注"].ToString() == null ? "" : item["备注"].ToString());
                    LbItem.SetItem("ParentID", "00000000-0000-0000-0000-000000000000");

                    LbItem.SetItem("TableName", "NPS_PROJLIST_MainInfo_CBS");
                    LbItem.SetItem("BizAreaId", "00000000-0000-0000-0000-00000000000A");
                    LbItem.SetItem("Sequ", Sequ);
                    LbItem.SetItem("Status", "0");
                    LbItem.SetItem("RegHumId", "AD000000-0000-0000-0000-000000000000");
                    LbItem.SetItem("RegHumName", "系统管理员");
                    LbItem.SetItem("RegDate", DateTime.Now);
                    LbItem.SetItem("RegPosiId", "00000000-0000-0000-0000-000000000000");
                    LbItem.SetItem("RegDeptId", "00000000-0000-0000-0000-000000000000");
                    LbItem.SetItem("EpsProjId", "59B04048-CB4B-4B13-A808-5C30F288FD4C");
                    LbItem.SetItem("RecycleHumId", "AD000000-0000-0000-0000-000000000000");

                    LbItem.SetItem("UpdHumId", "AD000000-0000-0000-0000-000000000000");
                    LbItem.SetItem("UpdHumName", "系统管理员");
                    LbItem.SetItem("UpdDate", DateTime.Now);
                    LbItem.SetItem("ApprHumId", "00000000-0000-0000-0000-000000000000");
                    LbItem.SetItem("OwnProjId", "59B04048-CB4B-4B13-A808-5C30F288FD4C");
                    //LbItem.SetItem("OwnProjName", LbZj);
                    //LbItem.SetItem("EpsProjCode", LbZj);
                    //LbItem.SetItem("EpsProjName", LbZj);
                    LbItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    Sequ++;
                }
                else
                {
                    string UpdateBOQZQ = "update NPS_PROJLIST_MainInfo_CBS set CBS_TYPE='" + item["承包商类别"].ToString() + "',CBS_LXR='" + item["联系人"].ToString() + "',";
                    UpdateBOQZQ += "CBS_LXDH='" + item["联系电话"].ToString() + "',CBS_EMILE='" + item["邮箱"].ToString() + "',CBS_XCFZR='" + item["现场负责人"].ToString() + "'" +
                        " ,CBS_FZRDH='" + item["现场负责人联系电话"].ToString() + "',CBS_SFQDHT='" + item["是否签订合同"].ToString() + "',CBS_BZ='" + item["备注"].ToString() + "'";
                    UpdateBOQZQ += "where ContratorName='" + item["承包商名称"].ToString() + "' ";
                    dal.Execute(UpdateBOQZQ);
                }
            }
            return "";
        }

        public void DeleteData(string Masterid)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.BeginTransaction();
            try
            {
                string sDelData = "delete from NPS_BOQZQ where Masterid='" + Masterid + "'";
                dal.Execute(sDelData);

                sDelData = "";
                sDelData = "delete from PLN_TASKPROCZQ where Masterid='" + Masterid + "'";
                dal.Execute(sDelData);

                sDelData = "";
                sDelData = "delete from PS_PLN_TaskProc_SubZQ where Masterid='" + Masterid + "'";
                dal.Execute(sDelData);
                dal.Commit();
            }
            catch (Exception ex)
            {
                dal.Rollback();
            }

               // Power.Controls.StdPlan.PlanControl.
        }

        public void UpdateProc(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date=null where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanSatrt=null where FID = '" + sGuid + "' ");
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanSatrt='" + JHKSSJ + "' where FID = '" + sGuid + "' ");
            }
        }
        public void UpdateProcend(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date=null where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanEnd=null where FID = '" + sGuid + "' ");
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanEnd='" + JHKSSJ + "' where FID = '" + sGuid + "' ");
            }
        }

        public void UpdateProactend(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
                dal.Execute("update PLN_TaskProc  set PLN_act_end_date=null where proc_guid = '" + sGuid + "' ");
            else
                dal.Execute("update PLN_TaskProc  set PLN_act_end_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
        }

        public void UpdateProactstart(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
                dal.Execute("update PLN_TaskProc  set PLN_act_start_date=null where proc_guid = '" + sGuid + "' ");
            else
                dal.Execute("update PLN_TaskProc  set PLN_act_start_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
        }

        public void UpdateProZQactend(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            string MasterID = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1900-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set completedate=null,complete_pct=0 where proc_guid = '" + sGuid + "' and masterid='" + MasterID + "' ");
                dal.Execute("update PLN_TaskProcZQ  set PLN_act_end_date=null where proc_guid = '" + sGuid + "' and masterid='" + MasterID + "' ");
            }
            else
            {
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set completedate='" + JHKSSJ + "',complete_pct=100 where proc_guid = '" + sGuid + "' and masterid='" + MasterID + "' ");
                dal.Execute("update PLN_TaskProcZQ  set PLN_act_end_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "'and masterid='" + MasterID + "' ");
            }
        }

        public void UpdateProZQactstart(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            string MasterID = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1900-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set checkdate=null where proc_guid = '" + sGuid + "' and masterid='" + MasterID + "' ");
                dal.Execute("update PLN_TaskProcZQ  set PLN_act_start_date=null where proc_guid = '" + sGuid + "' and masterid='" + MasterID + "' ");
            }
            else
            {
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set checkdate='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' and masterid='" + MasterID + "' ");
                dal.Execute("update PLN_TaskProcZQ  set PLN_act_start_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "'and masterid='" + MasterID + "' ");
            }
        }

        public IList GetTaskDetailsByJp(string task_guid, string detailKeyword, string taskGuidField, string detailType)
        {
            string text = taskGuidField + "='" + task_guid + "'";
            List<Hashtable> list2 = new List<Hashtable>();


            string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
            sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
            sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + task_guid + "'  order by seq_num";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            for (int i = 0; i < CwData.Rows.Count; i++)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("proc_id", CwData.Rows[i]["proc_id"].ToString());
                hashtable.Add("proj_id", CwData.Rows[i]["proj_id"].ToString());
                hashtable.Add("wbs_id", CwData.Rows[i]["wbs_id"].ToString());
                hashtable.Add("task_id", CwData.Rows[i]["task_id"].ToString());
                hashtable.Add("task_guid", CwData.Rows[i]["task_guid"].ToString());
                hashtable.Add("seq_num", CwData.Rows[i]["seq_num"].ToString());
                hashtable.Add("CompleteOrNot", CwData.Rows[i]["CompleteOrNot"].ToString());
                hashtable.Add("proc_name", CwData.Rows[i]["proc_name"].ToString());
                hashtable.Add("proc_descri", CwData.Rows[i]["proc_descri"].ToString());
                hashtable.Add("est_wt", CwData.Rows[i]["est_wt"].ToString());
                hashtable.Add("complete_pct", CwData.Rows[i]["complete_pct"].ToString());
                hashtable.Add("act_end_date", CwData.Rows[i]["act_end_date"].ToString());
                hashtable.Add("SysOrNot", CwData.Rows[i]["SysOrNot"].ToString());
                hashtable.Add("target_end_date_lag", CwData.Rows[i]["target_end_date_lag"].ToString());
                hashtable.Add("expect_end_date_lag", CwData.Rows[i]["expect_end_date_lag"].ToString());
                hashtable.Add("temp_id", CwData.Rows[i]["temp_id"].ToString());
                hashtable.Add("update_date", CwData.Rows[i]["update_date"].ToString());
                hashtable.Add("p3ec_proc_id", CwData.Rows[i]["p3ec_proc_id"].ToString());
                hashtable.Add("p3ec_flag", CwData.Rows[i]["p3ec_flag"].ToString());
                hashtable.Add("proc_guid", CwData.Rows[i]["proc_guid"].ToString());
                hashtable.Add("proj_guid", CwData.Rows[i]["proj_guid"].ToString());
                hashtable.Add("plan_guid", CwData.Rows[i]["plan_guid"].ToString());
                hashtable.Add("wbs_guid", CwData.Rows[i]["wbs_guid"].ToString());
                hashtable.Add("rsrc_guid", CwData.Rows[i]["rsrc_guid"].ToString());
                hashtable.Add("temp_guid", CwData.Rows[i]["temp_guid"].ToString());
                hashtable.Add("est_wt_pct", CwData.Rows[i]["est_wt_pct"].ToString());
                hashtable.Add("keyword", CwData.Rows[i]["keyword"].ToString());
                hashtable.Add("formid", CwData.Rows[i]["formid"].ToString());
                hashtable.Add("update_user", CwData.Rows[i]["update_user"].ToString());
                hashtable.Add("create_date", CwData.Rows[i]["create_date"].ToString());
                hashtable.Add("create_user", CwData.Rows[i]["create_user"].ToString());
                hashtable.Add("delete_session_id", CwData.Rows[i]["delete_session_id"].ToString());
                hashtable.Add("delete_date", CwData.Rows[i]["delete_date"].ToString());
                hashtable.Add("target_end_date", CwData.Rows[i]["target_end_date"].ToString());
                hashtable.Add("proc_code", CwData.Rows[i]["proc_code"].ToString());
                hashtable.Add("PLN_target_start_date", CwData.Rows[i]["PLN_target_start_date"].ToString());
                hashtable.Add("PLN_target_end_date", CwData.Rows[i]["PLN_target_end_date"].ToString());
                hashtable.Add("PLN_act_end_date", CwData.Rows[i]["PLN_act_end_date"].ToString());
                hashtable.Add("PLN_act_start_date", CwData.Rows[i]["PLN_act_start_date"].ToString());
                list2.Add(hashtable);
            }
            return list2.ToList();
        }

        public void InsertTaskprocZQ(string currentfeedback, string currentplan, string currentperiod, string currentrsrc)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sFeedBackSQL = "";
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' ";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                period_begindate = Convert.ToDateTime(FeedBackData.Rows[0]["period_begindate"].ToString());
                period_enddate = Convert.ToDateTime(FeedBackData.Rows[0]["period_enddate"].ToString());
            }

            dal.BeginTransaction();
            try
            {
                string sSQL = "Select masterid,task_guid From V_PS_PLN_FeedBackTask VPPFB Where  ";
                sSQL += "1=1  and ( VPPFB.MasterId='" + currentfeedback + "')  ";
                sSQL += "and (1=1 ) and rsrc_guid is not null Order By  type,VPPFB.task_code,VPPFB.Sequ ASC ";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    string masterid = CwData.Rows[i]["masterid"].ToString();
                    string task_guid = CwData.Rows[i]["task_guid"].ToString();

                    string sSQL1 = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                    sSQL1 += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                    sSQL1 += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + task_guid + "'  order by seq_num";
                    DataTable TaskProcData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL1);
                    for (int j = 0; j < TaskProcData.Rows.Count; j++)
                    {
                        string complete_pct = "0";
                        string sSQL3 = "select sum(complete_pct*est_wt_pct)/100 as complete_pct from PS_PLN_TaskProc_Sub where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "' group by proc_guid";
                        DataTable PS_PLN_TaskProc_Sub = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        if (PS_PLN_TaskProc_Sub.Rows.Count > 0)
                        {
                            complete_pct = PS_PLN_TaskProc_Sub.Rows[j]["complete_pct"].ToString();
                        }
                        else if (PS_PLN_TaskProc_Sub.Rows.Count == 0)
                        {
                            if (TaskProcData.Rows[j]["PLN_act_end_date"].ToString().Trim().Equals("") || string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString()))
                            {
                                complete_pct = "0";
                            }
                            else
                            {
                                complete_pct = "100";
                            }
                        }

                        string CheckDate = "";
                        string CompleteDate = "";
                        sSQL3 = "";
                        sSQL3 = "select min(isnull(CheckDate,'')) as CheckDate,max(isnull(CompleteDate,'')) as CompleteDate  from PS_PLN_TaskProc_Sub where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "' group by proc_guid";
                        DataTable PS_PLN_TaskProc_Sub1 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        if (PS_PLN_TaskProc_Sub1.Rows.Count > 0)
                        {
                            CheckDate = PS_PLN_TaskProc_Sub1.Rows[0]["CheckDate"].ToString();
                            CompleteDate = PS_PLN_TaskProc_Sub1.Rows[0]["CompleteDate"].ToString();
                        }

                        string sSQL2 = "insert into PLN_TaskprocZQ ";
                        sSQL2 += "(proj_id,wbs_id,task_id,";
                        sSQL2 += "task_guid,seq_num,CompleteOrNot,proc_name,";
                        sSQL2 += "proc_descri,est_wt,complete_pct,act_end_date,";
                        sSQL2 += "SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                        sSQL2 += "update_date,p3ec_proc_id,p3ec_flag,";
                        sSQL2 += "proc_guid,proj_guid,plan_guid,plan_id,";
                        sSQL2 += "wbs_guid,rsrc_guid,temp_guid,est_wt_pct,";
                        sSQL2 += "keyword,formid,update_user,create_date,";
                        sSQL2 += "create_user,delete_session_id,delete_date,";
                        sSQL2 += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,";
                        sSQL2 += " PLN_act_end_date,PLN_act_start_date,MasterId)";
                        sSQL2 += "  values ('" + TaskProcData.Rows[j]["proj_id"].ToString() + "','" + TaskProcData.Rows[j]["wbs_id"].ToString() + "','" + TaskProcData.Rows[j]["task_id"].ToString() + "',";
                        sSQL2 += " '" + TaskProcData.Rows[j]["task_guid"].ToString() + "','" + TaskProcData.Rows[j]["seq_num"].ToString() + "','" + TaskProcData.Rows[j]["CompleteOrNot"].ToString() + "','" + TaskProcData.Rows[j]["proc_name"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["proc_descri"].ToString() + "','" + TaskProcData.Rows[j]["est_wt"].ToString() + "','" + complete_pct + "','" + TaskProcData.Rows[j]["act_end_date"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["SysOrNot"].ToString() + "','" + TaskProcData.Rows[j]["target_end_date_lag"].ToString() + "','" + TaskProcData.Rows[j]["expect_end_date_lag"].ToString() + "','" + TaskProcData.Rows[j]["rsrc_id"].ToString() + "','" + TaskProcData.Rows[j]["temp_id"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["update_date"].ToString() + "','" + TaskProcData.Rows[j]["p3ec_proc_id"].ToString() + "','" + TaskProcData.Rows[j]["p3ec_flag"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["proc_guid"].ToString() + "','" + TaskProcData.Rows[j]["proj_guid"].ToString() + "','" + TaskProcData.Rows[j]["plan_guid"].ToString() + "','" + TaskProcData.Rows[j]["plan_id"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["wbs_guid"].ToString() + "','" + TaskProcData.Rows[j]["rsrc_guid"].ToString() + "','" + TaskProcData.Rows[j]["temp_guid"].ToString() + "','" + TaskProcData.Rows[j]["est_wt_pct"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["keyword"].ToString() + "','" + TaskProcData.Rows[j]["formid"].ToString() + "','" + TaskProcData.Rows[j]["update_user"].ToString() + "','" + TaskProcData.Rows[j]["create_date"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["create_user"].ToString() + "','" + TaskProcData.Rows[j]["delete_session_id"].ToString() + "','" + TaskProcData.Rows[j]["delete_date"].ToString() + "',";
                        sSQL2 += "'" + TaskProcData.Rows[j]["target_end_date"].ToString() + "','" + TaskProcData.Rows[j]["proc_code"].ToString() + "','" + TaskProcData.Rows[j]["PLN_target_start_date"].ToString() + "','" + TaskProcData.Rows[j]["PLN_target_end_date"].ToString() + "',";
                        //sSQL2 += "'" + CompleteDate + "','" + CheckDate + "','"+ masterid + "')"; 接口暂时没通，所以开始时间完成时间暂时先用产品表，等到接口通后再改回来
                        sSQL2 += "'" + TaskProcData.Rows[j]["PLN_act_end_date"].ToString() + "','" + TaskProcData.Rows[j]["PLN_act_start_date"].ToString() + "','" + masterid + "')";
                        dal.Execute(sSQL2);
                        //暂时没能找到有效的数据库对应方法，只能通过直接传参数值的方式，所以处理日期字段需要另外刷一遍，待日后找到对应方法后，修改过来。
                        string sUpdateSQL = "update PLN_TaskprocZQ set PLN_target_end_date=null where PLN_target_end_date='1900-01-01 00:00:00.000'";
                        dal.Execute(sUpdateSQL);
                        sUpdateSQL = "";
                        sUpdateSQL = "update PLN_TaskprocZQ set PLN_target_start_date=null where PLN_target_start_date='1900-01-01 00:00:00.000'";
                        dal.Execute(sUpdateSQL);
                        sUpdateSQL = "";
                        sUpdateSQL = "update PLN_TaskprocZQ set PLN_act_end_date=null where PLN_act_end_date='1900-01-01 00:00:00.000'";
                        dal.Execute(sUpdateSQL);
                        sUpdateSQL = "";
                        sUpdateSQL = "update PLN_TaskprocZQ set PLN_act_start_date=null where PLN_act_start_date='1900-01-01 00:00:00.000'";
                        dal.Execute(sUpdateSQL);

                        sSQL3 = "";
                        sSQL3 = "select ProcSub_guid,ProcSub_Name,proj_guid,plan_guid,wbs_guid,task_guid,proc_guid,seq_num,est_wt,est_wt_pct,complete_pct,";
                        sSQL3 += "      temp_guid,remark,RegDate,RegHumName,RegHumId,UpdHumId,UpdHuman,UpdDate,CheckDate,SubState,CompleteDate,ProcSub_Code,target_end_date,act_end_date";
                        sSQL3 += " from PS_PLN_TaskProc_Sub ";
                        sSQL3 += "where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "'";
                        PS_PLN_TaskProc_Sub = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        for (int k = 0; k < PS_PLN_TaskProc_Sub.Rows.Count; k++)
                        {
                            string WCBFB = "0";
                            if (PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString().Equals("1900-01-01") || string.IsNullOrEmpty(PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString()))
                            {
                                WCBFB = "0";
                            }
                            else
                            {
                                WCBFB = "100";
                            }
                            string SubState = "";
                            if ((PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString().Equals("1900-01-01") || string.IsNullOrEmpty(PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString()))
                                && (PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString().Equals("1900-01-01") || string.IsNullOrEmpty(PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString())))
                            {
                                SubState = "未开工";
                            }
                            else if ((!PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString().Equals("1900-01-01") && !string.IsNullOrEmpty(PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString()))
                                && (PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString().Equals("1900-01-01") || string.IsNullOrEmpty(PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString())))
                            {
                                SubState = "已开工";
                            }
                            else
                            {
                                SubState = "已完工";
                            }


                            sSQL2 = "";
                            sSQL2 += "insert into PS_PLN_TaskProc_SubZQ( ";
                            sSQL2 += " ProcSub_guid,ProcSub_Name,proj_guid,plan_guid,";
                            sSQL2 += " wbs_guid,task_guid,proc_guid,MasterId,";
                            sSQL2 += "seq_num,est_wt,";
                            sSQL2 += "est_wt_pct,complete_pct,target_end_date,";
                            sSQL2 += "act_end_date,temp_guid,remark,RegDate,";
                            sSQL2 += "RegHumName,RegHumId,UpdHumId,";
                            sSQL2 += "UpdHuman,UpdDate,CheckDate,SubState,";
                            sSQL2 += "CompleteDate,ProcSub_Code) ";
                            sSQL2 += " values ( ";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["ProcSub_guid"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["ProcSub_Name"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["proj_guid"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["plan_guid"].ToString() + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["wbs_guid"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["task_guid"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["proc_guid"].ToString() + "','" + masterid + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["seq_num"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["est_wt"].ToString() + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["est_wt_pct"].ToString() + "','" + WCBFB + "','" + PS_PLN_TaskProc_Sub.Rows[k]["target_end_date"].ToString() + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["act_end_date"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["temp_guid"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["remark"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["RegDate"].ToString() + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["RegHumName"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["RegHumId"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["UpdHumId"].ToString() + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["UpdHuman"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["UpdDate"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString() + "','" + SubState + "',";
                            sSQL2 += "'" + PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString() + "','" + PS_PLN_TaskProc_Sub.Rows[k]["ProcSub_Code"].ToString() + "')";
                            dal.Execute(sSQL2);

                            sUpdateSQL = "";
                            sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set target_end_date=null where target_end_date='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);

                            sUpdateSQL = "";
                            sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set act_end_date=null where act_end_date='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);

                            sUpdateSQL = "";
                            sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set CompleteDate=null where CompleteDate='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);

                            sUpdateSQL = "";
                            sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set CheckDate=null where CheckDate='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);


                        }

                        string sBOQ = "";
                        sBOQ = " select ID,Code,Title,TableName,BizAreaId,Sequ,Status,";
                        sBOQ += " RegHumId,RegHumName,RegDate,RegPosiId,RegDeptId,EpsProjId,";
                        sBOQ += " RecycleHumId,UpdHumId,UpdHumName,UpdDate,ApprHumId,ApprHumName,";
                        sBOQ += " ApprDate,Remark,OwnProjId,OwnProjName,EpsProjCode,EpsProjName,";
                        sBOQ += " FID,Chapter,ListingCode,ListingName,ListingPrice,ListingNum,";
                        sBOQ += " Amount,PlanNum,PlanPrice,PlanSatrt,PlanEnd,S_ISBN,Ndljtz from NPS_BOQ";
                        sBOQ += " where FID='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "' ";
                        DataTable NPS_BOQZQ = XCode.DataAccessLayer.DAL.QuerySQL(sBOQ);
                        for (int h = 0; h < NPS_BOQZQ.Rows.Count; h++)
                        {
                            double KGLJGCL = 0;
                            double KGLJJE = 0;
                            if ((!TaskProcData.Rows[j]["PLN_act_start_date"].ToString().Equals("1900-01-01") && !string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_start_date"].ToString()))
                                && (TaskProcData.Rows[j]["PLN_act_end_date"].ToString().Equals("1900-01-01") || string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString())))
                            {
                                KGLJGCL = float.Parse(NPS_BOQZQ.Rows[h]["ListingNum"].ToString()) * float.Parse(TaskProcData.Rows[j]["complete_pct"].ToString());
                                KGLJJE = float.Parse(NPS_BOQZQ.Rows[h]["ListingPrice"].ToString()) * float.Parse(TaskProcData.Rows[j]["complete_pct"].ToString());
                            }
                            else if ((!TaskProcData.Rows[j]["PLN_act_start_date"].ToString().Equals("1900-01-01") && !string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_start_date"].ToString()))
                                && (!TaskProcData.Rows[j]["PLN_act_end_date"].ToString().Equals("1900-01-01") && !string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString())))
                            {
                                KGLJGCL = float.Parse(NPS_BOQZQ.Rows[h]["ListingNum"].ToString());
                                KGLJJE = float.Parse(NPS_BOQZQ.Rows[h]["ListingPrice"].ToString());
                            }
                            double BYJHGCL = 0; //本月计划实物工程量
                            double BYJHGCJE = 0;//本月计划实物工程量（元）
                            double BNJHGCJE = 0; //本年计划实物工程量（元）
                            if (!string.IsNullOrEmpty(NPS_BOQZQ.Rows[h]["PlanSatrt"].ToString()))
                            {
                                if (DateTime.Parse(NPS_BOQZQ.Rows[h]["PlanSatrt"].ToString()).Month.Equals(period_enddate.Month))
                                {
                                    BYJHGCL = double.Parse(NPS_BOQZQ.Rows[h]["ListingNum"].ToString());
                                    BYJHGCJE = double.Parse(NPS_BOQZQ.Rows[h]["ListingPrice"].ToString());

                                }
                            }
                            if (!string.IsNullOrEmpty(NPS_BOQZQ.Rows[h]["PlanEnd"].ToString()))
                            {
                                if (DateTime.Parse(NPS_BOQZQ.Rows[h]["PlanEnd"].ToString()).Year.Equals(period_enddate.Year))
                                {
                                    BNJHGCJE = double.Parse(NPS_BOQZQ.Rows[h]["ListingPrice"].ToString());
                                }
                            }
                            double SQKGLJGCL = 0;//上期开工累计工程量   
                            double SQKGLJJE = 0;//上期开工累计金额
                                                //计算本月实际实物工程量
                                                //根据PLAN_GUID和周期后一个日期字段查询PS_PLN_FeedBackRecord表的ID字段，确定哪个周期，然后根据ID对应MasterId,找PS_PLN_FeedBackRecord_Task表，
                                                //查询出task_guid，根据task_guid和proc_guid字段，查询出NPS_BOQZQ的开工累计实物工程量。
                            if (string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString()))
                                CompleteDate = "1900-01-01";
                            if (string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_start_date"].ToString()))
                                CheckDate = "1900-01-01";

                            string sSQL4 = "select ID,plan_guid from PS_PLN_FeedBackRecord where plan_guid='" + Guid.Parse(currentplan) + "' and period_enddate=(select max(period_enddate) from ";
                            sSQL4 += "PS_PLN_FeedBackRecord where month(period_enddate)=month('" + period_enddate + "')-1) ";
                            DataTable FeedBackRecord = XCode.DataAccessLayer.DAL.QuerySQL(sSQL4);
                            if (FeedBackRecord.Rows.Count > 0)
                            {
                                string master = FeedBackRecord.Rows[0]["ID"].ToString();
                                string sSQL6 = "select KGLJGCL,KGLJJE from NPS_BOQZQ where masterid='" + master + "' and id='" + NPS_BOQZQ.Rows[h]["ID"].ToString() + "'";
                                DataTable SQKGLJGCLData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL6);
                                if (SQKGLJGCLData.Rows.Count > 0)
                                {
                                    SQKGLJGCL = float.Parse(SQKGLJGCLData.Rows[0]["KGLJGCL"].ToString());
                                    SQKGLJJE = float.Parse(SQKGLJGCLData.Rows[0]["KGLJJE"].ToString());
                                }
                            }


                            double BYSJGCL = 0;
                            double BYSJJE = 0;
                            BYSJGCL = KGLJGCL - SQKGLJGCL;//本月实际实物工程量
                            BYSJJE = KGLJJE - SQKGLJJE;//本月实际实物工程量（元）

                            if (TaskProcData.Rows[j]["PLN_act_start_date"].ToString().Trim().Equals("1900-01-01") && TaskProcData.Rows[j]["PLN_act_end_date"].ToString().Trim().Equals("1900-01-01"))//无实际开始时间，无实际完成时间
                            {
                                BYSJGCL = 0;
                                BYSJJE = 0;
                            }
                            if (!string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString()))
                            {
                                if (!DateTime.Parse(TaskProcData.Rows[j]["PLN_act_end_date"].ToString()).Month.Equals(period_enddate.Month))//实际完成时间与周期后一个判断是否再同一个月
                                {
                                    BYSJGCL = 0;
                                    BYSJJE = 0;
                                }
                            }

                            //计算本年计划工程量
                            double BNJHGCL = 0;
                            if (!string.IsNullOrEmpty(NPS_BOQZQ.Rows[h]["Planend"].ToString()))
                            {
                                if (DateTime.Parse(NPS_BOQZQ.Rows[h]["Planend"].ToString()).Year.Equals(period_enddate.Year))
                                {
                                    BNJHGCL = float.Parse(NPS_BOQZQ.Rows[h]["ListingNum"].ToString());
                                }
                            }

                            string sInsertBOQ = "";
                            sInsertBOQ = "insert into NPS_BOQZQ(ID,MasterId,Code,Title,TableName,BizAreaId,Sequ,Status,";
                            sInsertBOQ += " RegHumId,RegHumName,RegDate,RegPosiId,RegDeptId,";
                            sInsertBOQ += " RecycleHumId,UpdHumId,UpdHumName,UpdDate,ApprHumId,ApprHumName,";
                            sInsertBOQ += " ApprDate,Remark,OwnProjName,EpsProjCode,EpsProjName,";
                            sInsertBOQ += " FID,Chapter,ListingCode,ListingName,ListingPrice,ListingNum,";
                            sInsertBOQ += " Amount,PlanNum,PlanPrice,PlanSatrt,PlanEnd,S_ISBN,Ndljtz,act_startdate,act_enddate,KGLJGCL,KGLJJE,BYJHGCL,BYJHGCJE,BNJHGCJE,BYSJGCL,BYSJJE,BNJHGCL ) ";
                            sInsertBOQ += " values (";
                            sInsertBOQ += "'" + NPS_BOQZQ.Rows[h]["ID"].ToString() + "','" + masterid + "','" + NPS_BOQZQ.Rows[h]["Code"].ToString() + "','" + NPS_BOQZQ.Rows[h]["Title"].ToString() + "','" + NPS_BOQZQ.Rows[h]["TableName"].ToString() + "','" + NPS_BOQZQ.Rows[h]["BizAreaId"].ToString() + "','" + NPS_BOQZQ.Rows[h]["Sequ"].ToString() + "','" + NPS_BOQZQ.Rows[h]["Status"].ToString() + "',";
                            sInsertBOQ += "'" + NPS_BOQZQ.Rows[h]["RegHumId"].ToString() + "','" + NPS_BOQZQ.Rows[h]["RegHumName"].ToString() + "','" + NPS_BOQZQ.Rows[h]["RegDate"].ToString() + "','" + NPS_BOQZQ.Rows[h]["RegPosiId"].ToString() + "','" + NPS_BOQZQ.Rows[h]["RegDeptId"].ToString() + "',";
                            sInsertBOQ += "'" + NPS_BOQZQ.Rows[h]["RecycleHumId"].ToString() + "','" + NPS_BOQZQ.Rows[h]["UpdHumId"].ToString() + "','" + NPS_BOQZQ.Rows[h]["UpdHumName"].ToString() + "','" + NPS_BOQZQ.Rows[h]["UpdDate"].ToString() + "','" + NPS_BOQZQ.Rows[h]["ApprHumId"].ToString() + "','" + NPS_BOQZQ.Rows[h]["ApprHumName"].ToString() + "',";
                            sInsertBOQ += "'" + NPS_BOQZQ.Rows[h]["ApprDate"].ToString() + "','" + NPS_BOQZQ.Rows[h]["Remark"].ToString() + "','" + NPS_BOQZQ.Rows[h]["OwnProjName"].ToString() + "','" + NPS_BOQZQ.Rows[h]["EpsProjCode"].ToString() + "','" + NPS_BOQZQ.Rows[h]["EpsProjName"].ToString() + "',";
                            sInsertBOQ += "'" + NPS_BOQZQ.Rows[h]["FID"].ToString() + "','" + NPS_BOQZQ.Rows[h]["Chapter"].ToString() + "','" + NPS_BOQZQ.Rows[h]["ListingCode"].ToString() + "','" + NPS_BOQZQ.Rows[h]["ListingName"].ToString() + "','" + NPS_BOQZQ.Rows[h]["ListingPrice"].ToString() + "','" + NPS_BOQZQ.Rows[h]["ListingNum"].ToString() + "',";
                            sInsertBOQ += "'" + NPS_BOQZQ.Rows[h]["Amount"].ToString() + "','" + NPS_BOQZQ.Rows[h]["PlanNum"].ToString() + "','" + NPS_BOQZQ.Rows[h]["PlanPrice"].ToString() + "','" + TaskProcData.Rows[j]["PLN_target_start_date"].ToString() + "','" + TaskProcData.Rows[j]["PLN_target_end_date"].ToString() + "','" + NPS_BOQZQ.Rows[h]["S_ISBN"].ToString() + "','" + NPS_BOQZQ.Rows[h]["Ndljtz"].ToString() + "',";
                            sInsertBOQ += "'" + TaskProcData.Rows[j]["PLN_act_start_date"].ToString() + "','" + TaskProcData.Rows[j]["PLN_act_end_date"].ToString() + "','" + KGLJGCL + "','" + KGLJJE + "',";
                            sInsertBOQ += "'" + BYJHGCL + "','" + BYJHGCJE + "','" + BNJHGCJE + "','" + BYSJGCL + "','" + BYSJJE + "','" + BNJHGCL + "')";
                            dal.Execute(sInsertBOQ);

                            sUpdateSQL = "";
                            sUpdateSQL = "update NPS_BOQZQ set act_enddate=null where act_enddate='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);
                            sUpdateSQL = "";
                            sUpdateSQL = "update NPS_BOQZQ set act_startdate=null where act_startdate='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);
                            sUpdateSQL = "";
                            sUpdateSQL = "update NPS_BOQZQ set PlanSatrt=null where PlanSatrt='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);
                            sUpdateSQL = "";
                            sUpdateSQL = "update NPS_BOQZQ set PlanEnd=null where PlanEnd='1900-01-01 00:00:00.000'";
                            dal.Execute(sUpdateSQL);
                        }
                    }
                }
                dal.Commit();
            }
            catch (Exception ex)
            {
                dal.Rollback();
            }
        }
        public IList GetPLN_TaskProcZQByJp(string task_guid, string Masterid)
        {
            List<Hashtable> list2 = new List<Hashtable>();


            string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
            sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
            sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProcZQ  where task_guid = '" + task_guid + "' and Masterid='" + Masterid + "'  order by seq_num";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            for (int i = 0; i < CwData.Rows.Count; i++)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("proc_id", CwData.Rows[i]["proc_id"].ToString());
                hashtable.Add("proj_id", CwData.Rows[i]["proj_id"].ToString());
                hashtable.Add("wbs_id", CwData.Rows[i]["wbs_id"].ToString());
                hashtable.Add("task_id", CwData.Rows[i]["task_id"].ToString());
                hashtable.Add("task_guid", CwData.Rows[i]["task_guid"].ToString());
                hashtable.Add("seq_num", CwData.Rows[i]["seq_num"].ToString());
                hashtable.Add("CompleteOrNot", CwData.Rows[i]["CompleteOrNot"].ToString());
                hashtable.Add("proc_name", CwData.Rows[i]["proc_name"].ToString());
                hashtable.Add("proc_descri", CwData.Rows[i]["proc_descri"].ToString());
                hashtable.Add("est_wt", CwData.Rows[i]["est_wt"].ToString());
                hashtable.Add("complete_pct", CwData.Rows[i]["complete_pct"].ToString());
                hashtable.Add("act_end_date", CwData.Rows[i]["act_end_date"].ToString());
                hashtable.Add("SysOrNot", CwData.Rows[i]["SysOrNot"].ToString());
                hashtable.Add("target_end_date_lag", CwData.Rows[i]["target_end_date_lag"].ToString());
                hashtable.Add("expect_end_date_lag", CwData.Rows[i]["expect_end_date_lag"].ToString());
                hashtable.Add("temp_id", CwData.Rows[i]["temp_id"].ToString());
                hashtable.Add("update_date", CwData.Rows[i]["update_date"].ToString());
                hashtable.Add("p3ec_proc_id", CwData.Rows[i]["p3ec_proc_id"].ToString());
                hashtable.Add("p3ec_flag", CwData.Rows[i]["p3ec_flag"].ToString());
                hashtable.Add("proc_guid", CwData.Rows[i]["proc_guid"].ToString());
                hashtable.Add("proj_guid", CwData.Rows[i]["proj_guid"].ToString());
                hashtable.Add("plan_guid", CwData.Rows[i]["plan_guid"].ToString());
                hashtable.Add("wbs_guid", CwData.Rows[i]["wbs_guid"].ToString());
                hashtable.Add("rsrc_guid", CwData.Rows[i]["rsrc_guid"].ToString());
                hashtable.Add("temp_guid", CwData.Rows[i]["temp_guid"].ToString());
                hashtable.Add("est_wt_pct", CwData.Rows[i]["est_wt_pct"].ToString());
                hashtable.Add("keyword", CwData.Rows[i]["keyword"].ToString());
                hashtable.Add("formid", CwData.Rows[i]["formid"].ToString());
                hashtable.Add("update_user", CwData.Rows[i]["update_user"].ToString());
                hashtable.Add("create_date", CwData.Rows[i]["create_date"].ToString());
                hashtable.Add("create_user", CwData.Rows[i]["create_user"].ToString());
                hashtable.Add("delete_session_id", CwData.Rows[i]["delete_session_id"].ToString());
                hashtable.Add("delete_date", CwData.Rows[i]["delete_date"].ToString());
                hashtable.Add("target_end_date", CwData.Rows[i]["target_end_date"].ToString());
                hashtable.Add("proc_code", CwData.Rows[i]["proc_code"].ToString());
                hashtable.Add("PLN_target_start_date", CwData.Rows[i]["PLN_target_start_date"].ToString());
                hashtable.Add("PLN_target_end_date", CwData.Rows[i]["PLN_target_end_date"].ToString());
                hashtable.Add("PLN_act_end_date", CwData.Rows[i]["PLN_act_end_date"].ToString());
                hashtable.Add("PLN_act_start_date", CwData.Rows[i]["PLN_act_start_date"].ToString());
                list2.Add(hashtable);
            }
            return list2.ToList();
        }
        public void UpdateZQ(string masterid, string proc_guid, string currentplan, string currentperiod, string currentrsrc)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            double SQKGLJGCL = 0;//上期开工累计工程量   
            double SQKGLJJE = 0;//上期开工累计金额
            double KGLJGCL = 0;//开工累计实物工程量
            double KGLJGCJE = 0; //开工累计实物工程量（元）
            double BYSJGCL = 0;//本月实际实物工程量
            double BYSJGCJE = 0;//本月实际实物工程量（元）
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' ";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                period_begindate = Convert.ToDateTime(FeedBackData.Rows[0]["period_begindate"].ToString());
                period_enddate = Convert.ToDateTime(FeedBackData.Rows[0]["period_enddate"].ToString());
            }

            string sSQL = "select PLN_act_end_date,PLN_act_start_date,complete_pct from PLN_TaskprocZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (PLN_TaskprocZQ.Rows.Count > 0)
            {
                #region 以下是计算上期开工累计工程量、上期开工累计金额
                string sSQL1 = "select ListingPrice,ListingNum,ID from NPS_BOQZQ where FID='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable NPS_BOQZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL1);
                for (int i = 0; i < NPS_BOQZQ.Rows.Count; i++)
                {
                    string sSQL4 = "select ID,plan_guid from PS_PLN_FeedBackRecord where plan_guid='" + Guid.Parse(currentplan) + "' and period_enddate=(select max(period_enddate) from ";
                    sSQL4 += "PS_PLN_FeedBackRecord where month(period_enddate)=month('" + period_enddate + "')-1) ";
                    DataTable FeedBackRecord = XCode.DataAccessLayer.DAL.QuerySQL(sSQL4);
                    if (FeedBackRecord.Rows.Count > 0)
                    {
                        string master = FeedBackRecord.Rows[0]["ID"].ToString();
                        string sSQL6 = "select KGLJGCL,KGLJJE from NPS_BOQZQ where masterid='" + master + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "'";
                        DataTable SQKGLJGCLData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL6);
                        if (SQKGLJGCLData.Rows.Count > 0)
                        {
                            SQKGLJGCL = float.Parse(SQKGLJGCLData.Rows[0]["KGLJGCL"].ToString());
                            SQKGLJJE = float.Parse(SQKGLJGCLData.Rows[0]["KGLJJE"].ToString());
                        }
                    }
                    #endregion
                    #region 以下计算各个需要更新的字段
                    //有实际开始时间，无实际完成时间
                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }

                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }
                    #endregion
                    #region 根据以上计算字段，更新NPS_BOQZQ表记录。
                    string UpdateBOQZQ = "update NPS_BOQZQ set KGLJGCL='" + KGLJGCL + "',KGLJJE='" + KGLJGCJE + "',";
                    UpdateBOQZQ += "BYSJGCL='" + BYSJGCL + "',BYSJJE='" + BYSJGCJE + "'";
                    UpdateBOQZQ += "where masterid='" + masterid + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "' ";
                    dal.Execute(UpdateBOQZQ);
                    #endregion
                }

            }

        }
        public void UpdatestartdateByZY(string sCS)
        {
            string sTask_Guid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date=null where task_guid = '" + sTask_Guid + "' ");
                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set PlanSatrt=null where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date='" + JHKSSJ + "' where task_guid = '" + sTask_Guid + "' ");

                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set PlanSatrt='" + JHKSSJ + "' where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
        }

        public void UpdateEnddateByZY(string sCS)
        {
            string sTask_Guid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date=null where task_guid = '" + sTask_Guid + "' ");
                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set Planend=null where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date='" + JHKSSJ + "' where task_guid = '" + sTask_Guid + "' ");

                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set Planend='" + JHKSSJ + "' where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
        }


        public void UpCheckDate(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            string Masterid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set checkdate=null where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set checkdate=null where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                string sSQL = "select completedate from PS_PLN_TaskProc_SubZQ where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "'";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (CwData.Rows.Count > 0)
                {
                    if (CwData.Rows[0]["completedate"].ToString().Equals(""))
                    {
                        dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='未开始' where ProcSub_guid = '" + sGuid + "' ");
                        dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='未开始' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                    }
                    else
                    {
                        dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='已完成' where ProcSub_guid = '" + sGuid + "' ");
                        dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='已完成' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                    }
                }
            }
            else
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set checkdate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set checkdate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                string sSQL = "select completedate from PS_PLN_TaskProc_SubZQ where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "'";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (CwData.Rows.Count > 0)
                {
                    if (CwData.Rows[0]["completedate"].ToString().Equals(""))
                    {
                        dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='已开始' where ProcSub_guid = '" + sGuid + "' ");
                        dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='已开始' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                    }
                    else
                    {
                        dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='已完成' where ProcSub_guid = '" + sGuid + "' ");
                        dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='已完成' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                    }
                }
            }
        }
        public void Upest_wt_pct(string sCS)//工序的权重点击事件，更新权重百分比
        {
            string sGuid = sCS.Split('|')[0];
            float est_wt = float.Parse(sCS.Split('|')[1]);
            string Masterid = sCS.Split('|')[2];
            string ProcSub_guid= sCS.Split('|')[3];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.Execute("update PS_PLN_TaskProc_SubZQ set est_wt='"+ est_wt + "' where masterid='" + Masterid + "' and ProcSub_guid='"+ ProcSub_guid + "'");
            dal.Execute("update PS_PLN_TaskProc_SubZQ set est_wt_pct=(round(CONVERT(float,est_wt)/(select sum(A.est_wt) from PS_PLN_TaskProc_SubZQ A where A.proc_guid=B.proc_guid),4))*100 from PS_PLN_TaskProc_SubZQ B" +
                " where B.proc_guid in (select proc_guid from PS_PLN_TaskProc_SubZQ group by proc_guid,masterid)");

            dal.Execute("update  PS_PLN_TaskProc_SubZQ set  est_wt_pct = round((select est_wt_pct from PS_PLN_TaskProc_SubZQ C where C.proc_guid=A.proc_guid and proc_id=(select min(proc_id) " +
                "from PS_PLN_TaskProc_SubZQ D where A.proc_guid=D.proc_guid ))" +
                "+(select 100-sum(B.est_wt_pct) from PS_PLN_TaskProc_SubZQ B where A.proc_guid=B.proc_guid),2)" +
                "from PS_PLN_TaskProc_SubZQ A " +
                " where A.proc_guid in (select proc_guid from PS_PLN_TaskProc_SubZQ group by proc_guid,masterid) " +
                "and proc_id=(select min(proc_id) from PS_PLN_TaskProc_SubZQ D where A.proc_guid=D.proc_guid )" +
                "and masterid='"+ Masterid + "' and proc_guid='"+ sGuid + "'");
        }

        public void UpCompletepct(string sCS)//工序的完成百分比点击事件，更新完成百分比
        {
            string sGuid = sCS.Split('|')[0];
            float complete_pct = float.Parse(sCS.Split('|')[1]);
            string Masterid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.Execute("update PS_PLN_TaskProc_Sub  set complete_pct='"+ complete_pct + "' where ProcSub_guid = '" + sGuid + "' ");
            dal.Execute("update PS_PLN_TaskProc_SubZQ  set complete_pct='" + complete_pct + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
        }

        public void UpCompleteDate(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            string Masterid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set completedate=null,complete_pct='0.00' where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set completedate=null,complete_pct='0.00' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                string sSQL = "select checkdate from PS_PLN_TaskProc_SubZQ where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "'";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (CwData.Rows.Count > 0)
                {
                    if (CwData.Rows[0]["checkdate"].ToString().Equals(""))
                    {
                        dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='未开始' where ProcSub_guid = '" + sGuid + "' ");
                        dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='未开始' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                    }
                    else
                    {
                        dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='已开始' where ProcSub_guid = '" + sGuid + "' ");
                        dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='已开始' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
                    }
                }
            }
            else
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set SubState='已完成', complete_pct='100.00',completedate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='已完成' ,complete_pct='100.00', completedate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
            }
            
        }

        public void UpdateSubZQ(string masterid, string proc_guid, string currentplan, string currentperiod, string currentrsrc,Boolean flag=false)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            double SQKGLJGCL = 0;//上期开工累计工程量   
            double SQKGLJJE = 0;//上期开工累计金额
            double KGLJGCL = 0;//开工累计实物工程量
            double KGLJGCJE = 0; //开工累计实物工程量（元）
            double BYSJGCL = 0;//本月实际实物工程量
            double BYSJGCJE = 0;//本月实际实物工程量（元）
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' ";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                period_begindate = Convert.ToDateTime(FeedBackData.Rows[0]["period_begindate"].ToString());
                period_enddate = Convert.ToDateTime(FeedBackData.Rows[0]["period_enddate"].ToString());
            }

            //以下这段是为了取最小开始时间，最大完成时间
            string sSQL11 = "select min(CheckDate) as PLN_act_start_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ11 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL11);
            string PLN_act_start_date = PLN_TaskprocZQ11.Rows[0]["PLN_act_start_date"].ToString();

            string PLN_act_end_date = "1900-01-01 00:00:00.000";
            Boolean flag = true;
            string sSQL12 = "select est_wt_pct,est_wt,complete_pct,CompleteDate as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ12 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL12);
            for (int u = 0; u < PLN_TaskprocZQ12.Rows.Count; u++)
            {
                if (string.IsNullOrEmpty(PLN_TaskprocZQ12.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ12.Rows[u]["PLN_act_end_date"].ToString().Trim().Equals(""))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
            {
                PLN_act_end_date = "1900-01-01 00:00:00.000";
            }
            else
            {
                string sSQL13 = "select max(CompleteDate) as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable PLN_TaskprocZQ13 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL13);
                if (PLN_TaskprocZQ13.Rows.Count > 0)
                {
                    PLN_act_end_date = PLN_TaskprocZQ13.Rows[0]["PLN_act_end_date"].ToString();
                }
            }
            //重新根据工序的完成百分比*权重%累加得出对应构件的完成百分比
            float WCBFB = 0;
            for (int y = 0; y < PLN_TaskprocZQ12.Rows.Count; y++)
            {
                WCBFB += (float.Parse(PLN_TaskprocZQ12.Rows[y]["est_wt_pct"].ToString()) * float.Parse(PLN_TaskprocZQ12.Rows[y]["complete_pct"].ToString()))/100;
            }
            //根据新的开始时间、完成时间更新对应构件的开始、完成时间
            string updPLN_TaskprocZQ = "update PLN_TaskprocZQ set ";
            if (PLN_act_end_date != "1900-01-01 00:00:00.000" && PLN_act_start_date != "")
            {
                updPLN_TaskprocZQ += "PLN_act_end_date='" + PLN_act_end_date + "'";
                updPLN_TaskprocZQ += ",PLN_act_start_date='" + PLN_act_start_date + "' ";
                updPLN_TaskprocZQ += ",complete_pct='" + WCBFB + "'";
            }
            else if (PLN_act_end_date != "1900-01-01 00:00:00.000" && PLN_act_start_date == "")
            {
                updPLN_TaskprocZQ += "PLN_act_end_date='" + PLN_act_end_date + "'";
                updPLN_TaskprocZQ += ",PLN_act_start_date=null ";
                updPLN_TaskprocZQ += ",complete_pct='" + WCBFB + "'";
            }
            else if (PLN_act_end_date == "1900-01-01 00:00:00.000" && PLN_act_start_date != "")
            {
                updPLN_TaskprocZQ += "PLN_act_end_date=null";
                updPLN_TaskprocZQ += ",PLN_act_start_date='" + PLN_act_start_date + "' ";
                updPLN_TaskprocZQ += ",complete_pct='" + WCBFB + "'";
            }
            else
            {
                updPLN_TaskprocZQ += "PLN_act_end_date=null";
                updPLN_TaskprocZQ += ",PLN_act_start_date=null ";
                updPLN_TaskprocZQ += ",complete_pct='" + WCBFB + "'";
            }
            updPLN_TaskprocZQ += "where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            dal.Execute(updPLN_TaskprocZQ);

            string sSQL = "select PLN_act_end_date,PLN_act_start_date,complete_pct from PLN_TaskprocZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (PLN_TaskprocZQ.Rows.Count > 0)
            {
                #region 以下是计算上期开工累计工程量、上期开工累计金额
                string sSQL1 = "select ListingPrice,ListingNum,ID from NPS_BOQZQ where FID='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable NPS_BOQZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL1);
                for (int i = 0; i < NPS_BOQZQ.Rows.Count; i++)
                {
                    string sSQL4 = "select ID,plan_guid from PS_PLN_FeedBackRecord where plan_guid='" + Guid.Parse(currentplan) + "' and period_enddate=(select max(period_enddate) from ";
                    sSQL4 += "PS_PLN_FeedBackRecord where month(period_enddate)=month('" + period_enddate + "')-1) ";
                    DataTable FeedBackRecord = XCode.DataAccessLayer.DAL.QuerySQL(sSQL4);
                    if (FeedBackRecord.Rows.Count > 0)
                    {
                        string master = FeedBackRecord.Rows[0]["ID"].ToString();
                        string sSQL6 = "select KGLJGCL,KGLJJE from NPS_BOQZQ where masterid='" + master + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "'";
                        DataTable SQKGLJGCLData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL6);
                        if (SQKGLJGCLData.Rows.Count > 0)
                        {
                            SQKGLJGCL = float.Parse(SQKGLJGCLData.Rows[0]["KGLJGCL"].ToString());
                            SQKGLJJE = float.Parse(SQKGLJGCLData.Rows[0]["KGLJJE"].ToString());
                        }
                    }
                    #endregion
                    #region 以下计算各个需要更新的字段
                    //有实际开始时间，无实际完成时间
                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }

                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }
                    #endregion
                    #region 根据以上计算字段，更新NPS_BOQZQ表记录。
                    string UpdateBOQZQ = "update NPS_BOQZQ set KGLJGCL='" + KGLJGCL + "',KGLJJE='" + KGLJGCJE + "',";
                    UpdateBOQZQ += "BYSJGCL='" + BYSJGCL + "',BYSJJE='" + BYSJGCJE + "'";
                    UpdateBOQZQ += "where masterid='" + masterid + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "' ";
                    dal.Execute(UpdateBOQZQ);
                    #endregion
                }

            }

        }





        public static void task()
        {
            Console.WriteLine("Hello, world.");
        }
         
        private static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        static CookieContainer cookie = new CookieContainer();
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception)
            {

                return "false";
            }

        }

        public void UpdatestartdateByZY(string sCS)
        {
            string sTask_Guid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date=null where task_guid = '" + sTask_Guid + "' ");
                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set PlanSatrt=null where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date='" + JHKSSJ + "' where task_guid = '" + sTask_Guid + "' ");

                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set PlanSatrt='" + JHKSSJ + "' where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
        }

        public void UpdateEnddateByZY(string sCS)
        {
            string sTask_Guid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date=null where task_guid = '" + sTask_Guid + "' ");
                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set Planend=null where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date='" + JHKSSJ + "' where task_guid = '" + sTask_Guid + "' ");

                string sSQL = "select proc_id,proj_id,wbs_id,task_id,task_guid,seq_num,CompleteOrNot,proc_name,proc_descri,est_wt,complete_pct,act_end_date,SysOrNot,target_end_date_lag,expect_end_date_lag,rsrc_id,temp_id,";
                sSQL += "update_date,p3ec_proc_id,p3ec_flag,proc_guid,proj_guid,plan_guid,plan_id,wbs_guid,rsrc_guid,temp_guid,est_wt_pct,keyword,formid,update_user,create_date,create_user,delete_session_id,delete_date,";
                sSQL += "target_end_date,proc_code,PLN_target_start_date,PLN_target_end_date,PLN_act_end_date,PLN_act_start_date from  PLN_TaskProc  where task_guid = '" + sTask_Guid + "'  order by seq_num";
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)
                {
                    dal.Execute("update NPS_BOQ  set Planend='" + JHKSSJ + "' where FID = '" + CwData.Rows[i]["proc_guid"].ToString() + "' ");
                }
            }
        }

        public void UpdateProc(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date=null where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanSatrt=null where FID = '" + sGuid + "' ");
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanSatrt='" + JHKSSJ + "' where FID = '" + sGuid + "' ");
            }
        }
        public void UpdateProcend(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date=null where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanEnd=null where FID = '" + sGuid + "' ");
            }
            else
            {
                dal.Execute("update PLN_TaskProc  set PLN_target_end_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
                dal.Execute("update NPS_BOQ  set PlanEnd='" + JHKSSJ + "' where FID = '" + sGuid + "' ");
            }
        }

        public void UpCheckDate(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            string Masterid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set checkdate=null where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set checkdate=null where ProcSub_guid = '" + sGuid + "'and Masterid='"+ Masterid + "' ");
            }
            else
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set checkdate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set checkdate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
            }
        }

        public void UpCompleteDate(string sCS)
        {
            string sGuid = sCS.Split('|')[0];
            string JHKSSJ = sCS.Split('|')[1];
            string Masterid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            if (JHKSSJ.Trim().ToString().Equals("1970-01-01") || JHKSSJ.Trim().ToString().Equals("null"))
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set completedate=null where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set completedate=null where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
            }
            else
            {
                dal.Execute("update PS_PLN_TaskProc_Sub  set completedate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "' ");
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set completedate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
            }
        }

        public void UpdateZQ(string masterid, string proc_guid, string currentplan, string currentperiod, string currentrsrc)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            double SQKGLJGCL = 0;//上期开工累计工程量   
            double SQKGLJJE = 0;//上期开工累计金额
            double KGLJGCL = 0;//开工累计实物工程量
            double KGLJGCJE = 0; //开工累计实物工程量（元）
            double BYSJGCL = 0;//本月实际实物工程量
            double BYSJGCJE = 0;//本月实际实物工程量（元）
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' ";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                period_begindate = Convert.ToDateTime(FeedBackData.Rows[0]["period_begindate"].ToString());
                period_enddate = Convert.ToDateTime(FeedBackData.Rows[0]["period_enddate"].ToString());
            }

            string sSQL = "select PLN_act_end_date,PLN_act_start_date,complete_pct from PLN_TaskprocZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (PLN_TaskprocZQ.Rows.Count > 0)
            {
                #region 以下是计算上期开工累计工程量、上期开工累计金额
                string sSQL1 = "select ListingPrice,ListingNum,ID from NPS_BOQZQ where FID='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable NPS_BOQZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL1);
                for (int i = 0; i < NPS_BOQZQ.Rows.Count; i++)
                {
                    string sSQL4 = "select ID,plan_guid from PS_PLN_FeedBackRecord where plan_guid='" + Guid.Parse(currentplan) + "' and period_enddate=(select max(period_enddate) from ";
                    sSQL4 += "PS_PLN_FeedBackRecord where month(period_enddate)=month('" + period_enddate + "')-1) ";
                    DataTable FeedBackRecord = XCode.DataAccessLayer.DAL.QuerySQL(sSQL4);
                    if (FeedBackRecord.Rows.Count > 0)
                    {
                        string master = FeedBackRecord.Rows[0]["ID"].ToString();
                        string sSQL6 = "select KGLJGCL,KGLJJE from NPS_BOQZQ where masterid='" + master + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "'";
                        DataTable SQKGLJGCLData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL6);
                        if (SQKGLJGCLData.Rows.Count > 0)
                        {
                            SQKGLJGCL = float.Parse(SQKGLJGCLData.Rows[0]["KGLJGCL"].ToString());
                            SQKGLJJE = float.Parse(SQKGLJGCLData.Rows[0]["KGLJJE"].ToString());
                        }
                    }
                    #endregion
                    #region 以下计算各个需要更新的字段
                    //有实际开始时间，无实际完成时间
                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }

                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }
                    #endregion
                    #region 根据以上计算字段，更新NPS_BOQZQ表记录。
                    string UpdateBOQZQ = "update NPS_BOQZQ set KGLJGCL='" + KGLJGCL + "',KGLJJE='" + KGLJGCJE + "',";
                    UpdateBOQZQ += "BYSJGCL='" + BYSJGCL + "',BYSJJE='" + BYSJGCJE + "'";
                    UpdateBOQZQ += "where masterid='" + masterid + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "' ";
                    dal.Execute(UpdateBOQZQ);
                    #endregion
                }

            }

        }


        public void UpdateSubZQ(string masterid, string proc_guid, string currentplan, string currentperiod, string currentrsrc)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            double SQKGLJGCL = 0;//上期开工累计工程量   
            double SQKGLJJE = 0;//上期开工累计金额
            double KGLJGCL = 0;//开工累计实物工程量
            double KGLJGCJE = 0; //开工累计实物工程量（元）
            double BYSJGCL = 0;//本月实际实物工程量
            double BYSJGCJE = 0;//本月实际实物工程量（元）
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' ";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                period_begindate = Convert.ToDateTime(FeedBackData.Rows[0]["period_begindate"].ToString());
                period_enddate = Convert.ToDateTime(FeedBackData.Rows[0]["period_enddate"].ToString());
            }

            //以下这段是为了取最小开始时间，最大完成时间
            string sSQL11 = "select min(CheckDate) as PLN_act_start_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ11 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL11);
            string PLN_act_start_date = PLN_TaskprocZQ11.Rows[0]["PLN_act_start_date"].ToString();

            string PLN_act_end_date = "1900-01-01 00:00:00.000";
            Boolean flag = true;
            string sSQL12 = "select CompleteDate as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ12= XCode.DataAccessLayer.DAL.QuerySQL(sSQL12);
            for (int u = 0; u < PLN_TaskprocZQ12.Rows.Count; u++)
            {
                if (string.IsNullOrEmpty(PLN_TaskprocZQ12.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ12.Rows[u]["PLN_act_end_date"].ToString().Trim().Equals(""))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
            {
                PLN_act_end_date = "1900-01-01 00:00:00.000";
            }
            else
            {
                string sSQL13 = "select max(CompleteDate) as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable PLN_TaskprocZQ13 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL13);
                if (PLN_TaskprocZQ13.Rows.Count > 0)
                {
                    PLN_act_end_date = PLN_TaskprocZQ13.Rows[0]["PLN_act_end_date"].ToString();
                }
            }
            //根据新的开始时间、完成时间更新对应构件的开始、完成时间
            string updPLN_TaskprocZQ = "update PLN_TaskprocZQ set PLN_act_end_date='"+ PLN_act_end_date + "',PLN_act_start_date='"+ PLN_act_start_date + "' where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            dal.Execute(updPLN_TaskprocZQ);

            string sSQL = "select PLN_act_end_date,PLN_act_start_date,complete_pct from PLN_TaskprocZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
            DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (PLN_TaskprocZQ.Rows.Count > 0)
            {
                #region 以下是计算上期开工累计工程量、上期开工累计金额
                string sSQL1 = "select ListingPrice,ListingNum,ID from NPS_BOQZQ where FID='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable NPS_BOQZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL1);
                for (int i = 0; i < NPS_BOQZQ.Rows.Count; i++)
                {
                    string sSQL4 = "select ID,plan_guid from PS_PLN_FeedBackRecord where plan_guid='" + Guid.Parse(currentplan) + "' and period_enddate=(select max(period_enddate) from ";
                    sSQL4 += "PS_PLN_FeedBackRecord where month(period_enddate)=month('" + period_enddate + "')-1) ";
                    DataTable FeedBackRecord = XCode.DataAccessLayer.DAL.QuerySQL(sSQL4);
                    if (FeedBackRecord.Rows.Count > 0)
                    {
                        string master = FeedBackRecord.Rows[0]["ID"].ToString();
                        string sSQL6 = "select KGLJGCL,KGLJJE from NPS_BOQZQ where masterid='" + master + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "'";
                        DataTable SQKGLJGCLData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL6);
                        if (SQKGLJGCLData.Rows.Count > 0)
                        {
                            SQKGLJGCL = float.Parse(SQKGLJGCLData.Rows[0]["KGLJGCL"].ToString());
                            SQKGLJJE = float.Parse(SQKGLJGCLData.Rows[0]["KGLJJE"].ToString());
                        }
                    }
                    #endregion
                    #region 以下计算各个需要更新的字段
                    //有实际开始时间，无实际完成时间
                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }

                    if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000"))
                    && (!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000")))
                    {
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString());//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString());//开工累计实物工程量

                        BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                        BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                    }
                    #endregion
                    #region 根据以上计算字段，更新NPS_BOQZQ表记录。
                    string UpdateBOQZQ = "update NPS_BOQZQ set KGLJGCL='" + KGLJGCL + "',KGLJJE='" + KGLJGCJE + "',";
                    UpdateBOQZQ += "BYSJGCL='" + BYSJGCL + "',BYSJJE='" + BYSJGCJE + "'";
                    UpdateBOQZQ += "where masterid='" + masterid + "' and id='" + NPS_BOQZQ.Rows[i]["ID"].ToString() + "' ";
                    dal.Execute(UpdateBOQZQ);
                    #endregion
                }

            }

        }


        //请购  
        public static void ImportQG()
        {
            //获取相关辅助表
            List<NPS_BID_StatusTable> dataTable = DBService.Context.FromSql("select * from NPS_BID_StatusTable where 1=1").ToList<NPS_BID_StatusTable>();
            for (int i = 0; i < dataTable.Count; i++)
            {
                DBService.Context.Delete<NPS_BID_StatusTable>(d => d.ID == dataTable[i].ID);
            }
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_BID_StatusTable").ToDataTable();
            List<NPS_BID_StatusTable> InsertDt = DBService.Context.FromSql("select * from NPS_BID_StatusTable where 1=0").ToList<NPS_BID_StatusTable>();
            string count=HttpPost("http://10.150.2.36:7080/home/spmGetQGCount","");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetQG?Num=5000&Page="+ IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    if (QgDt.Select(" RequestCode='" + item["prNo"] + "' and EpsProjName='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"] + "' and RequestVersion='" + item["prVersion"] + "'").Length == 0)
                    {
                        NPS_BID_StatusTable d = new NPS_BID_StatusTable();
                        d.ProjectDep = Project.Select(" project_guid='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["parent_guid"] + "'")[0]["project_name"].ToString();
                        d.EpsProjName = item["proName"].ToString();
                        d.AdditionalQua = item["proUnit"].ToString();
                        d.RequestCode = item["prNo"].ToString();
                        d.SPMCode = item["proNo"].ToString();
                        d.SPMDes = item["des"].ToString();
                        d.RequestVersion = item["prVersion"].ToString();
                        d.RequestAmount = item["prQuantity"].ToString();
                        d.RequestDate = Convert.ToDateTime(item["prDate"].ToString());
                        d.MaterialDate = Convert.ToDateTime(item["planArriveDate"].ToString());
                        d.ConAmount = item["poQuantity"].ToString();
                        if (item["poDate"] != null && item["poDate"].ToString() != "0" && item["poDate"].ToString() != "")
                        {
                            d.ConSignDate = Convert.ToDateTime(Convert.ToDateTime(item["poDate"].ToString()).ToString("yyyy-MM-dd"));
                        }

                        d.ReleaseAmount = item["issueQuantity"].ToString();
                        if (item["issueDate"] != null && item["issueDate"].ToString() != "")
                        {
                            d.RequestDate = Convert.ToDateTime(Convert.ToDateTime(item["issueDate"].ToString()).ToString("yyyy-MM-dd"));
                        }
                        if (item["receiveDate"] != null && item["receiveDate"].ToString() != "")
                        {
                            d.ActualDate = Convert.ToDateTime(item["receiveDate"].ToString()).ToString("yyyy-MM-dd");
                        }

                        d.InventoryAmount = item["receiveQuantity"].ToString();

                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_BID_StatusTable";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num+5000>=int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }

            //批量加入
            DBService.Context.Insert(InsertDt);
        }

        //设备材料到货一览表（改）
        public static string ImportSbDh()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_EquipArrive").ToDataTable();
            List<NPS_PUR_EquipArrive> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_EquipArrive where 1=0").ToList<NPS_PUR_EquipArrive>();

            string count = HttpPost("http://10.150.2.36:7080/home/spmGetSbcldhCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetSbcldh?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["项目ID"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
    
                    {
                        NPS_PUR_EquipArrive d = new NPS_PUR_EquipArrive();
                        //d.CKProjectDep = Project.Select(" project_guid='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["parent_guid"] + "'")[0]["project_name"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.ProfName = item["zhuanye"].ToString();
                        d.EquipName = item["材料描述"].ToString();
                        d.Unit = item["UNIT_CODE"].ToString();
                        d.PlanNum = float.Parse(item["设计总量"].ToString());
                        d.ArriveNum = float.Parse(item["入库总量"].ToString());
                        d.GapNum = float.Parse(item["总缺口"].ToString());
                        d.ArriveRate = float.Parse(item["入库总量"].ToString())/ float.Parse(item["设计总量"].ToString())*100;
                        d.SendNum = float.Parse(item["发放总量"].ToString());
                        d.SendRate = float.Parse(item["发放百分百"].ToString());

                        d.StoreNum = float.Parse(item["库存总量"].ToString());

                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_BID_StatusTable";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }






            //批量加入
            DBService.Context.Insert(InsertDt);
            return "完成";
        }


        //分承包商设备材料发放一览表
        public static string ImportFbcldh()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_SubcontEquipInOut").ToDataTable();
            List<NPS_PUR_SubcontEquipInOut> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_SubcoNPS_PUR_SubcontEquipInOutntEquipInOutTwo where 1=0").ToList<NPS_PUR_SubcontEquipInOut>();

            string count = HttpPost("http://10.150.2.36:7080/home/spmGetFbcldhCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetFbcldh?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        NPS_PUR_SubcontEquipInOut d = new NPS_PUR_SubcontEquipInOut();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.SubcontName = item["公司名称"].ToString();
                        d.ProfName = item["专业"].ToString();
                        d.Name = item["材料描述"].ToString();
                        d.Unit = item["unit"].ToString();
                        d.PlanNum = float.Parse(item["bom量"].ToString());
                        d.SendNum = float.Parse(item["发放量"].ToString());
                        d.SendNum = float.Parse(item["发放量"].ToString());
                        d.GapNum = float.Parse(item["bom量"].ToString())-float.Parse(item["发放量"].ToString());
                        d.SendRate = float.Parse(item["发放量"].ToString()) / float.Parse(item["bom量"].ToString())*100;
                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_PUR_SubcontEquipInOut";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }






            //批量加入
            DBService.Context.Insert(InsertDt);
            return "完成";
        }

        //实际库存盘点表
        public static string ImportSckcpd()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_MatInventory").ToDataTable();
            List<NPS_PUR_MatInventory> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_MatInventory where 1=0").ToList<NPS_PUR_MatInventory>();

            string count = HttpPost("http://10.150.2.36:7080/home/spmGetPkdCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetPkd?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        NPS_PUR_MatInventory d = new NPS_PUR_MatInventory();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.WareName = item["ioc_code"].ToString();
                        d.MatCode = item["cc"].ToString();
                        d.UMatCode = item["ident_code"].ToString();
                        d.MatName = item["ident_code"].ToString();
                        d.Unit = item["单位"].ToString();
                        d.IDENT = item["Ident"].ToString();
                        d.MatDescribe = item["材料描述"].ToString();
                        d.Stock = float.Parse(item["存量"].ToString());
                      
                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_PUR_MatInventory";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }






            //批量加入
            DBService.Context.Insert(InsertDt);
            return "完成";
        }

        //工程材料核销台账（改）
        public static string ImportGcclhx()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_MatVerificationBook").ToDataTable();
            List<NPS_PUR_MatVerificationBook> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_MatVerificationBook where 1=0").ToList<NPS_PUR_MatVerificationBook>();

            string count = HttpPost("http://10.150.2.36:7080/home/spmGetClhxCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetClhx?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        NPS_PUR_MatVerificationBook d = new NPS_PUR_MatVerificationBook();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.UMatCode = item["ident_code"].ToString();
                        d.Ident = item["Ident"].ToString();
                        d.MatCode = item["cc"].ToString();
                        d.Unit = item["unit"].ToString();
                        d.MatName = item["ident_code"].ToString();
                        d.MatSpec = item["材料描述"].ToString();
                        d.Plan_Plan = float.Parse(item["bom量"].ToString());
                        d.PurNum = float.Parse(item["到货量"].ToString());
                        d.Use_Self = float.Parse(item["发放量"].ToString());
                        d.StoreNum = float.Parse(item["到货量"].ToString())-float.Parse(item["发放量"].ToString());
                        d.OverNum = float.Parse(item["bom量"].ToString()) - float.Parse(item["到货量"].ToString());
                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_PUR_MatVerificationBook";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }






            //批量加入
            DBService.Context.Insert(InsertDt);
            return "完成";
        }

        //分承包商材料核销台账（改）
        public static string ImportFbclhx()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_SubcontVerificationBook").ToDataTable();
            List<NPS_PUR_SubcontVerificationBook> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_SubcontVerificationBook where 1=0").ToList<NPS_PUR_SubcontVerificationBook>();

            string count = HttpPost("http://10.150.2.36:7080/home/spmGetFbclhxCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetFbclhx?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        NPS_PUR_SubcontVerificationBook d = new NPS_PUR_SubcontVerificationBook();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.UMatCode = item["ident_code"].ToString();
                        d.Ident = item["Ident"].ToString();
                        d.MatCode = item["cc"].ToString();
                        d.Unit = item["unit"].ToString();
                        d.MatName = item["ident_code"].ToString();
                        d.MatSpec = item["材料描述"].ToString();
                        d.Demand_Demand = float.Parse(item["bom量"].ToString());
                        d.CollectNum = float.Parse(item["发放量"].ToString());
                        d.OverNum = float.Parse(item["bom量"].ToString()) - float.Parse(item["发放量"].ToString());
                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_PUR_SubcontVerificationBook";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }






            //批量加入
            DBService.Context.Insert(InsertDt);
            return "完成";
        }

        //采买状态
        public static string ImportCmzt()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            List<NPS_PUR_PurBook> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_PurBook ").ToList<NPS_PUR_PurBook>();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_PurBook").ToDataTable();
            string count = HttpPost("http://10.150.2.36:7080/home/spmGetCmztCount", "");
            int IndexPage = 1;
            
            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetCmzt?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["项目ID"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    if (QgDt.Select(" PurchaseNo='" + item["r_code"] + "' and EpsProjName='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["项目ID"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"] + "' and Version='" + item["r_supp"] + "'").Length > 0)
                    {
                        NPS_PUR_PurBook d = InsertDt.Find(x=>x.ID== Guid.Parse(QgDt.Select(" PurchaseNo='" + item["r_code"] + "' and EpsProjName='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["项目ID"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"] + "' and Version='" + item["r_supp"] + "'")[0]["ID"].ToString()));
                        if (d!=null)
                        {
                            if(!d.SPMCode.Trim().Equals(item["PO_NUMBER"].ToString()))
                            d.SPMCode = d.SPMCode +","+ item["PO_NUMBER"].ToString();
                            d.Num= float.Parse(item["po qty"].ToString());
                            d.SubcontCode= item["sup_code"].ToString();
                            d.SubcontName = item["company_name"].ToString();
                            d.PurchaseNo = item["r_code"].ToString();
                            d.Version= item["R_SUPP"].ToString();
                        }
                    }
                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }
            //批量加入
            DBService.Context.Update(InsertDt);
            return "完成";
        }

        //设备材料出入库一览表（采购）
        public static string ImprotCLCRK()
        {
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            List<NPS_PUR_EPCEquipInOutCount> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_EPCEquipInOutCount ").ToList<NPS_PUR_EPCEquipInOutCount>();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_EPCEquipInOutCount").ToDataTable();
            string count = HttpPost("http://10.150.2.36:7080/home/spmGetCLCRKCount", "");

            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetCLCRK?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["项目ID"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    if (QgDt.Select(" PurchaseNo='" + item["请购单号"] + "' and EpsProjName='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["项目名称"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"] + "' and Version='" + item["请购版次"] + "'").Length > 0)
                    {
                        NPS_PUR_EPCEquipInOutCount d = InsertDt.Find(x => x.ID == Guid.Parse(QgDt.Select(" PurchaseNo='" + item["请购单号"] + "' and EpsProjName='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["项目名称"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"] + "' and Version='" + item["请购版次"] + "'")[0]["ID"].ToString()));
                        if (d != null)
                        {
                            d.EpsProjName = item["项目名称"].ToString();
                            d.PurchaseNo = item["请购单号"].ToString();
                            d.Version = item["请购版次"].ToString();
                            d.ConCode = item["合同号"].ToString();
                            d.Unit = item["单位"].ToString();
                            d.Num = float.Parse(item["请购数量"].ToString());
                            d.WNum = float.Parse(item["放行量"].ToString());
                            d.InNum = float.Parse(item["收货数量"].ToString());
                            d.SubcontName = item["供应商"].ToString();
                        }
                    }
                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }
            //批量加入
            DBService.Context.Update(InsertDt);

            public void UpdateProc(string sCS)
            {
                string sGuid = sCS.Split('|')[0];
                string JHKSSJ = sCS.Split('|')[1];
                XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
                if(JHKSSJ.Trim().ToString().Equals("null"))
                    dal.Execute("update PLN_TaskProc  set PLN_target_start_date=null where proc_guid = '" + sGuid + "' ");
                else
                dal.Execute("update PLN_TaskProc  set PLN_target_start_date='" + JHKSSJ + "' where proc_guid = '" + sGuid + "' ");
            }



            return "完成";
        }

            public string GetXMList()
            {
                string sResult = string.Empty;
                ArrayList arrayList = new ArrayList();
                string sSQL = " select project_shortname as code, project_name as name from pln_project where ( parent_guid<>'00000000-0000-0000-0000-0000000000a4' and project_guid<>'00000000-0000-0000-0000-0000000000a4') ";
                DataTable dataTable = DAL.QuerySQL(sSQL);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    JObject obj = new JObject();
                    obj.Add("code", dataTable.Rows[i]["code"].ToString().Trim());
                    obj.Add("name", dataTable.Rows[i]["name"].ToString().Trim());
                    arrayList.Add(obj);
                }

                JsonSerializerSettings s_setting = new JsonSerializerSettings();
                sResult = JsonConvert.SerializeObject(arrayList, s_setting);
                return sResult;
            }

            //发放状态
            public static string ImportFF()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_BID_Ratio").ToDataTable();
            List<NPS_BID_Ratio> InsertDt = DBService.Context.FromSql("select * from NPS_BID_Ratio where 1=0").ToList<NPS_BID_Ratio>();

            string count = HttpPost("http://10.150.2.36:7080/home/spmGetFFCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://10.150.2.36:7080/home/spmGetFF?Num=5000&Page=" + IndexPage + "", "");
                //数据处理成json字符串
                JArray SpmDataList = (JArray)JsonConvert.DeserializeObject(result);
                foreach (JObject item in SpmDataList)
                {
                    if (SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'").Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        NPS_BID_Ratio d = new NPS_BID_Ratio();
                        d.ProjectDep = Project.Select(" project_guid='" + Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["parent_guid"] + "'")[0]["project_name"].ToString();
                        d.EpsProjName = item["proName"].ToString();
                        d.Zy = item["dis"].ToString();
                        d.LongDes = item["ident_code"].ToString();
                        d.CodeType = item["cc"].ToString();
                        if (d.CodeType.StartsWith("PP"))
                        {
                            d.GroupContent = "PP";
                            if (d.LongDes.Contains("N04400") || d.LongDes.Contains("N05500"))
                            {
                                d.Remark = "蒙乃尔钢管";
                            }
                            else if (d.LongDes.EndsWith("GZ") || d.LongDes.EndsWith("GY"))
                            {
                                d.Remark = "氧气用钢管";
                            }
                            else if (d.LongDes.EndsWith("FZ"))
                            {
                                d.Remark = "临氢钢管";
                            }
                            else if (d.LongDes.Contains("ANTI-HIC") || d.LongDes.Contains("ANTI-H2S"))
                            {
                                d.Remark = "抗硫化氢钢管";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("4"))
                            {
                                d.Remark = "低温钢管";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("3"))
                            {
                                d.Remark = "不锈钢焊管";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("1"))
                            {
                                d.Remark = "碳钢管";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("2"))
                            {
                                d.Remark = "合金钢管";
                            }
                        }
                        else if (d.CodeType.StartsWith("PJ"))
                        {
                            d.GroupContent = "PJ";
                            if (d.LongDes.Contains("N04400") || d.LongDes.Contains("N05500"))
                            {
                                d.Remark = "蒙乃尔管件";
                            }
                            else if (d.LongDes.EndsWith("GZ") || d.LongDes.EndsWith("GY"))
                            {
                                d.Remark = "氧气用管件";
                            }
                            else if (d.LongDes.EndsWith("FZ"))
                            {
                                d.Remark = "临氢管件";
                            }
                            else if (d.LongDes.Contains("ANTI-HIC") || d.LongDes.Contains("ANTI-H2S"))
                            {
                                d.Remark = "抗硫化氢管件";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("4"))
                            {
                                d.Remark = "低温管件";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("3"))
                            {
                                d.Remark = "不锈钢管件";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("1"))
                            {
                                d.Remark = "碳钢管件";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("2"))
                            {
                                d.Remark = "合金钢管件";
                            }
                        }
                        else if (d.CodeType.StartsWith("PV"))
                        {
                            d.GroupContent = "PV";
                            d.Remark = "阀门";
                        }
                        else if (d.CodeType.StartsWith("PFB"))
                        {
                            d.GroupContent = "PFB";
                            if (d.LongDes.Contains("N04400") || d.LongDes.Contains("N05500"))
                            {
                                d.Remark = "蒙乃尔法兰";
                            }
                            else if (d.LongDes.EndsWith("GZ") || d.LongDes.EndsWith("GY"))
                            {
                                d.Remark = "氧气用法兰";
                            }
                            else if (d.LongDes.EndsWith("FZ"))
                            {
                                d.Remark = "临氢法兰";
                            }
                            else if (d.LongDes.Contains("ANTI-HIC") || d.LongDes.Contains("ANTI-H2S"))
                            {
                                d.Remark = "抗硫化氢法兰";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("4"))
                            {
                                d.Remark = "低温法兰";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("3"))
                            {
                                d.Remark = "不锈钢法兰";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("1"))
                            {
                                d.Remark = "碳钢法兰";
                            }
                            else if (d.LongDes.Substring(5, 6).Trim().ToString().Equals("2"))
                            {
                                d.Remark = "合金钢法兰";
                            }
                        }
                        else if (d.CodeType.StartsWith("PL"))
                        {
                            d.GroupContent = "PL";
                            d.Remark = "螺栓";
                        }
                        else if (d.CodeType.StartsWith("PG"))
                        {
                            d.GroupContent = "PG";
                            d.Remark = "垫片";
                        }
                        else if (d.CodeType.StartsWith("CIL"))
                        {
                            d.GroupContent = "CIL";
                            d.Remark = "型材(米)";
                        }
                        else if (d.CodeType.StartsWith("CS"))
                        {
                            d.GroupContent = "CS";
                            d.Remark = "板材(m²)";
                        }
                        else if (d.CodeType.StartsWith("PSSV"))
                        {
                            d.GroupContent = "PSSV";
                            d.Remark = "安全阀";
                        }
                        else if (d.CodeType.StartsWith("PMMT"))
                        {
                            d.GroupContent = "PMMT";
                            d.Remark = "疏水器";
                        }
                        else if (d.CodeType.StartsWith("PMMS"))
                        {
                            d.GroupContent = "PMMS";
                            d.Remark = "过滤器";
                        }
                        d.Type = item["size1"].ToString();
                        d.DesignAmount = item["designQty"].ToString();
                        d.RequestAmount = item["prqty"].ToString();
                        d.ArrivalAmount = item["receiveQty"].ToString();
                        d.ArrivalWeight = item["totalWeight"].ToString();
                        d.OpenWeight = item["sendQty"].ToString();
                        d.OpenCo = float.Parse(item["prqty"].ToString()) - float.Parse(item["receiveQty"].ToString())+"";
                        d.ArrivalRate = float.Parse(item["receiveQty"].ToString()) / float.Parse(item["prqty"].ToString()) + "";
                        d.DistributionRate = float.Parse(item["sendQty"].ToString()) / float.Parse(item["receiveQty"].ToString()) + "";
                        //补充缺省列
                        //处理缺省列
                        d.ID = Guid.NewGuid();
                        d.TableName = "NPS_BID_Ratio";
                        d.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        d.Status = 0;
                        d.RegHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegHumName = "系统管理员";
                        d.RegDate = DateTime.Now;
                        d.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.EpsProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.RecycleHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        d.UpdHumName = "系统管理员";
                        d.UpdDate = DateTime.Now;
                        d.OwnProjId = Guid.Parse(Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_guid"].ToString());
                        d.OwnProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        d.EpsProjCode = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_shortname"].ToString();
                        d.EpsProjName = Project.Select(" project_guid='" + SPMList.Select(" SpmCode='" + item["proNo"].ToString() + "'")[0]["EpsProjId"] + "'")[0]["project_name"].ToString();
                        InsertDt.Add(d);
                    }


                }
                if (Num + 5000 >= int.Parse(count))
                {
                    xh = false;
                }
                else
                {
                    Num += 5000;
                }
            }





            //批量加入
            DBService.Context.Insert(InsertDt);
            return "完成";
        }



        //设计·人工时对比
        public static string GetSj()
        {
            //获取各个表信息
            //工时主表
            List<DCWTRECMST> gszb = DBService.EMS.FromSql("select * from DCWTRECMST").ToList<DCWTRECMST>();
            //工时子表
            List<DCWTRECLIN> gszblist = DBService.EMS.FromSql("select * from DCWTRECLIN").ToList<DCWTRECLIN>();
            //日历子表
            List<DCWTDURLIN> rizblist = DBService.EMS.FromSql("select * from DCWTDURLIN").ToList<DCWTDURLIN>();
            //工时类型
            List<DCWTTYPMST> gslx = DBService.EMS.FromSql("select * from DCWTTYPMST").ToList<DCWTTYPMST>();
            //部门
            List<OGCSTMST> bm = DBService.EMS.FromSql("select * from OGCSTMST").ToList<OGCSTMST>();
            //专业
            List<DCSKLMST> zy = DBService.EMS.FromSql("select * from DCSKLMST").ToList<DCSKLMST>();
            //人员
            List<CMEMPMST> ry = DBService.EMS.FromSql("select * from CMEMPMST").ToList<CMEMPMST>();

            //计算
            List<NPS_DES_ArtificialTime> Sjrgs = new List<NPS_DES_ArtificialTime>();
            foreach (DCWTRECMST item in gszb)
            {
                NPS_DES_ArtificialTime i = new NPS_DES_ArtificialTime();
                i.Year = item.YEAR;
                i.Week = rizblist.Find(z=>z.DURL_NO==item.CURWEEK)!=null? rizblist.Find(z => z.DURL_NO == item.CURWEEK).WEEKSEQ.ToString(): "";
                i.Rec = bm.Find(z=>z.CST_NO==item.REC_CST)!=null? bm.Find(z => z.CST_NO == item.REC_CST).CST_NAM:"";
                DCWTRECLIN d = gszblist.Find(z => z.REC_NO == item.REC_NO);
                if (d != null)
                {
                    i.SKL = zy.Find(z => z.SKL_NO == d.SKL_NO) != null ? zy.Find(z => z.SKL_NO == d.SKL_NO).SKL_NAM : "";
                }
               i.User= ry.Find(z => z.EMP_ID == item.REC_USR) != null ? ry.Find(z => z.EMP_ID == item.REC_USR).EMP_NAM : "";

                i.SumFdwt = item.SUMFDWT.ToString();
                int sum = 0;
                sum += item.SUMJ1!=null?(int)item.SUMJ1:0;
                sum += item.SUMJ2 != null ? (int)item.SUMJ2 : 0;
                sum += item.SUMJ3 != null ? (int)item.SUMJ3 : 0;
                sum += item.SUMJ4 != null ? (int)item.SUMJ4 : 0;
                sum += item.SUMJ5 != null ? (int)item.SUMJ5 : 0;
                sum += item.SUMJ6 != null ? (int)item.SUMJ6 : 0;
                sum += item.SUMJ7 != null ? (int)item.SUMJ7 : 0;
                i.SumJ += sum;
                i.SumactWt = item.SUMACTWT!=null?item.SUMACTWT.ToString():"0";
                if (gszblist.Find(z=>z.REC_NO==item.REC_NO)!=null&& gszblist.Find(z => z.REC_NO == item.REC_NO).WTLX_NO== "XMYG")
                {
                    i.Wtlx=item.SUMACTWT != null ? item.SUMJ6.ToString() : "0";
                }
               i.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
               i.Status = 0;
               i.RegHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
               i.RegHumName = "系统管理员";
               i.RegDate = DateTime.Now;
               i.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
               i.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
               i.EpsProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
               i.RecycleHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
               i.UpdHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
               i.UpdHumName = "系统管理员";
               i.UpdDate = DateTime.Now;
               i.OwnProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
                i.OwnProjName = "中石化南京工程公司";
               i.ID = Guid.NewGuid();
                Sjrgs.Add(i);
            }


            //任务单表
            List<DCTASKMENUMST> rwdb = DBService.EMS.FromSql("select * from DCTASKMENUMST").ToList<DCTASKMENUMST>();
            //项目表
            List<DCPROJECTMST> xmb = DBService.EMS.FromSql("select * from DCPROJECTMST").ToList<DCPROJECTMST>();
            //

            //设计人工时一览表
            List<NPS_DES_HumanTime> sj = new List<NPS_DES_HumanTime>();
            foreach (DCWTRECMST item in gszb)
            {
               
                List<DCWTRECLIN> list = gszblist.FindAll(z=>z.REC_NO==item.REC_NO);
                string week= rizblist.Find(z => z.DURL_NO == item.CURWEEK) != null ? rizblist.Find(z => z.DURL_NO == item.CURWEEK).WEEKSEQ.ToString() : "";
                foreach (DCWTRECLIN child in list)
                {
                    DCSKLMST dCSKLMST = zy.Find(z=>z.SKL_NO==child.SKL_NO);
                    if (sj.Find(z=>z.ReportYear==item.YEAR&&z.ReportWeek== week&& z.ProjName== child.PROJECT_NAM)!=null)
                    {
                        NPS_DES_HumanTime a = sj.Find(z => z.ReportYear == item.YEAR && z.ReportWeek == week && z.ProjName == child.PROJECT_NAM);
                        switch (dCSKLMST.SKL_NAM)
                        {
                            case "工艺":
                                a.Process= (child.SUMWT != null ? (int)child.SUMWT : 0)+ (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "安全":
                                a.Security = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "技术经济":
                                a.Technical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "环保":
                                a.Environmental = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "管道":
                                a.Piping = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "管机":
                                a.Mill = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "管材":
                                a.Tube = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "电气":
                                a.Electrical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "仪表":
                                a.Instrument = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "电信":
                                a.Telecom = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "静设备":
                                a.StaticEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "动设备":
                                a.MovingEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "建筑":
                                a.Building = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "结构":
                                a.Structure = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "消防":
                                a.Fire = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "暖通":
                                a.Hvac = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "热工":
                                a.Thermal = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "给排水":
                                a.Drainage = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "总图运输":
                                a.Transport = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "粉体":
                                a.Powder = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "机修":
                                a.MachinePart = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "概算":
                                a.Estimate = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            default:
                                break;
                        }

                    }
                    else
                    {
                        NPS_DES_HumanTime a = new NPS_DES_HumanTime();
                        a.ReportYear = item.YEAR;
                        a.ReportWeek = week;
                        a.ProjName = child.PROJECT_NAM;
                        a.ProjId = rwdb.Find(z=>(int)z.PROJECT_NO==Convert.ToInt32( child.PROJECT_NO)).PRJ_ID;
                        a.ManageName = ry.Find(d => d.EMP_ID == rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO)).PRJMGR_ID).EMP_NAM;
                        switch (dCSKLMST.SKL_NAM)
                        {
                            case "工艺":
                                a.Process = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "安全":
                                a.Security = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "技术经济":
                                a.Technical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "环保":
                                a.Environmental = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "管道":
                                a.Piping = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "管机":
                                a.Mill = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "管材":
                                a.Tube = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "电气":
                                a.Electrical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "仪表":
                                a.Instrument = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "电信":
                                a.Telecom = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "静设备":
                                a.StaticEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "动设备":
                                a.MovingEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "建筑":
                                a.Building = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "结构":
                                a.Structure = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "消防":
                                a.Fire = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "暖通":
                                a.Hvac = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "热工":
                                a.Thermal = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "给排水":
                                a.Drainage = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "总图运输":
                                a.Transport = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "粉体":
                                a.Powder = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "机修":
                                a.MachinePart = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            case "概算":
                                a.Estimate = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
                                break;
                            default:
                                break;
                        }
                        a.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                        a.Status = 0;
                        a.RegHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
                        a.RegHumName = "系统管理员";
                        a.RegDate = DateTime.Now;
                        a.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        a.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        a.EpsProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
                        a.RecycleHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
                        a.UpdHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
                        a.UpdHumName = "系统管理员";
                        a.UpdDate = DateTime.Now;
                        a.OwnProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
                        a.OwnProjName = "中石化南京工程公司";
                        a.ID = Guid.NewGuid();
                        sj.Add(a);
                    }
                }
            }

            List<DCAREATYPMST> sh = DBService.EMS.FromSql("select * from DCAREATYPMST").ToList<DCAREATYPMST>();
            List<DCAREAMST> sq = DBService.EMS.FromSql("select * from DCAREAMST").ToList<DCAREAMST>(); 
            List<DCPRJJDMST> sjjd = DBService.EMS.FromSql("select * from DCPRJJDMST").ToList<DCPRJJDMST>();
            //设计项目一览表
            List<NPS_DES_ProjInfo> sjxm = new List<NPS_DES_ProjInfo>();
            List<NPS_DES_ProjInfo> XtList = DBService.Context.FromSql("select * from NPS_DES_ProjInfo").ToList<NPS_DES_ProjInfo>();
            foreach (DCPROJECTMST item in xmb)
            {
                //查找原来的数据，找到就更新，找不到就不更新
          
                NPS_DES_ProjInfo Xt = XtList.Find(z=>z.ProjCode== rwdb.Find(zz => zz.PROJECT_NO == item.PROJECT_NO).PRJ_ID);
                if (Xt!=null)
                {
                    //更新某些字段
                    Xt.Address = sh.Find(d => sq.Find(z => z.AREA_NO == item.AREA_NO).AREATYP_NO == d.AREATYP_NO).AREATYP_NAM;
                    Xt.ConCode = item.BARTZ_ID;
                    Xt.ProjName = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PROJECT_NAM;
                    Xt.ProjCode = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJ_ID;
                    Xt.ConStartDate = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PLABEG_DTM;
                    Xt.ConEndDate = DateTime.Parse(xmb.Find(z => z.PROJECT_NO == item.PROJECT_NO).BAR_JD);
                    Xt.ManageName = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                    Xt.JhName = ry.Find(d => d.EMP_ID == item.PLAPRJ_USR).EMP_NAM;
                    Xt.Engineer = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).WDSUR_ID).EMP_NAM;
                    Xt.Design = sjjd.Find(d => d.PRJJD_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJJD_ID).PRJJD_NAM;
                    Xt.LeadingRoom = bm.Find(d => d.CST_NO == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).CST_NO).CST_NAM;
                    Xt.DesignMoney = item.SJTZ_AMT;
                    Xt.ConMoney = item.YGBAR_AMT;
                    Xt.CollectionManage = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                    switch (item.PRO_STA)
                    {
                        case "6":
                            Xt.XmStatus = "关闭";
                            break;
                        case "1":
                            Xt.XmStatus = "启动";
                            break;
                        case "2":
                            Xt.XmStatus = "运行";
                            break;
                        case "3":
                            Xt.XmStatus = "暂停";
                            break;
                        case "4":
                            Xt.XmStatus = "中止";
                            break;
                        case "5":
                            Xt.XmStatus = "完工";
                            break;
                        default:
                            break;
                    }
                }

               NPS_DES_ProjInfo a = new NPS_DES_ProjInfo();
                a.Address = sh.Find(d => sq.Find(z => z.AREA_NO == item.AREA_NO).AREATYP_NO == d.AREATYP_NO).AREATYP_NAM;
                a.ConCode = item.BARTZ_ID;
                a.ProjName = rwdb.Find(z=>z.PROJECT_NO==item.PROJECT_NO).PROJECT_NAM;
                a.ProjCode= rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJ_ID;
                a.ManageName= ry.Find(d=>d.EMP_ID== rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                a.JhName= ry.Find(d => d.EMP_ID == item.PLAPRJ_USR).EMP_NAM;
                a.Engineer= ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).WDSUR_ID).EMP_NAM;
                a.Design = sjjd.Find(d=>d.PRJJD_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJJD_ID).PRJJD_NAM;
                a.LeadingRoom = bm.Find(d=>d.CST_NO == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).CST_NO).CST_NAM;
                a.DesignMoney = item.SJTZ_AMT;
                a.ConMoney = item.YGBAR_AMT;
                a.CollectionManage= ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                switch (item.PRO_STA)
                {
                    case "6":
                        a.XmStatus = "关闭";
                        break;
                    case "1":
                        a.XmStatus = "启动";
                        break;
                    case "2":
                        a.XmStatus = "运行";
                        break;
                    case "3":
                        a.XmStatus = "暂停";
                        break;
                    case "4":
                        a.XmStatus = "中止";
                        break;
                    case "5":
                        a.XmStatus = "完工";
                        break;
                    default:
                        break;
                }
                a.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                a.Status = 0;
                a.RegHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
                a.RegHumName = "系统管理员";
                a.RegDate = DateTime.Now;
                a.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                a.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                a.EpsProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
                a.RecycleHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
                a.UpdHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
                a.UpdHumName = "系统管理员";
                a.UpdDate = DateTime.Now;
                a.OwnProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
                a.OwnProjName = "中石化南京工程公司";
                a.ID = Guid.NewGuid();
                sjxm.Add(a);
            }


            //加载数据
            DBService.Context.Insert(sj);
            DBService.Context.Insert(Sjrgs);
            DBService.Context.Insert(sjxm);
            return "";
        }


       
    }
}
