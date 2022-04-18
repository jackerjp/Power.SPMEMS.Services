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
using System.Threading.Tasks;

namespace Power.SPMEMS.Services
{
    class Program
    {
        static void Main(string[] args)
        {

            //spm
            //ImportQG();
            //ImportSbDh();
            //ImportFbcldh();
            //ImportSckcpd();
            //ImportGcclhx();
            //ImportFbclhx();
            //ImportCmzt();
            //ImportFF();
            //ems
            GetSj();
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
        //请购
        public static string ImportQG()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_BID_StatusTable").ToDataTable();
            List<NPS_BID_StatusTable> InsertDt = DBService.Context.FromSql("select * from NPS_BID_StatusTable where 1=0").ToList<NPS_BID_StatusTable>();

            string count=HttpPost("http://127.0.0.1:7080/home/spmGetQGCount","");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetQG?Num=5000&Page="+ IndexPage + "", "");
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
            return "完成";
        }

        //设备材料到货一览表（改）
        public static string ImportSbDh()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_EquipArrive").ToDataTable();
            List<NPS_PUR_EquipArrive> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_EquipArrive where 1=0").ToList<NPS_PUR_EquipArrive>();

            string count = HttpPost("http://127.0.0.1:7080/home/spmGetSbcldhCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetSbcldh?Num=5000&Page=" + IndexPage + "", "");
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
                        d.Unit = item["UNITCODE"].ToString();
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
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_PUR_SubcontEquipInOutTwo").ToDataTable();
            List<NPS_PUR_SubcontEquipInOutTwo> InsertDt = DBService.Context.FromSql("select * from NPS_PUR_SubcontEquipInOutTwo where 1=0").ToList<NPS_PUR_SubcontEquipInOutTwo>();

            string count = HttpPost("http://127.0.0.1:7080/home/spmGetFbcldhCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetFbcldh?Num=5000&Page=" + IndexPage + "", "");
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
                        NPS_PUR_SubcontEquipInOutTwo d = new NPS_PUR_SubcontEquipInOutTwo();
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
                        d.TableName = "NPS_PUR_SubcontEquipInOutTwo";
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

            string count = HttpPost("http://127.0.0.1:7080/home/spmGetPkdCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetPkd?Num=5000&Page=" + IndexPage + "", "");
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

            string count = HttpPost("http://127.0.0.1:7080/home/spmGetClhxCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetClhx?Num=5000&Page=" + IndexPage + "", "");
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

            string count = HttpPost("http://127.0.0.1:7080/home/spmGetFbclhxCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetFbclhx?Num=5000&Page=" + IndexPage + "", "");
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
            string count = HttpPost("http://127.0.0.1:7080/home/spmGetCmztCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetCmzt?Num=5000&Page=" + IndexPage + "", "");
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
                            d.SPMCode = item["po number"].ToString();
                            d.Num= float.Parse(item["po qty"].ToString());
                            d.SubcontCode= item["sup_code"].ToString();
                            d.SubcontName = item["company_name"].ToString();
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

        //发放状态
        public static string ImportFF()
        {
            //获取相关辅助表
            DataTable Project = DBService.Context.FromSql("select * from pln_project").ToDataTable();
            DataTable SPMList = DBService.Context.FromSql("select distinct  SpmCode,EpsProjId from NPS_BID_Integrated where  isnull(spmcode,'')<>'' ").ToDataTable();
            DataTable QgDt = DBService.Context.FromSql("select * from NPS_BID_Ratio").ToDataTable();
            List<NPS_BID_Ratio> InsertDt = DBService.Context.FromSql("select * from NPS_BID_Ratio where 1=0").ToList<NPS_BID_Ratio>();

            string count = HttpPost("http://127.0.0.1:7080/home/spmGetFFCount", "");
            int IndexPage = 1;

            int Num = 0;
            bool xh = true;
            while (xh)
            {
                string result = HttpPost("http://127.0.0.1:7080/home/spmGetFF?Num=5000&Page=" + IndexPage + "", "");
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
            List<DCWTRECMST> gszb = DBService.EMS.FromSql("select * from sneiems.DCWTRECMST").ToList<DCWTRECMST>();
            //工时子表
            List<DCWTRECLIN> gszblist = DBService.EMS.FromSql("select * from sneiems.DCWTRECLIN").ToList<DCWTRECLIN>();
            //日历子表
            List<DCWTDURLIN> rizblist = DBService.EMS.FromSql("select * from sneiems.DCWTDURLIN").ToList<DCWTDURLIN>();
            //工时类型
            List<DCWTTYPMST> gslx = DBService.EMS.FromSql("select * from sneiems.DCWTTYPMST").ToList<DCWTTYPMST>();
            //部门
            List<OGCSTMST> bm = DBService.EMS.FromSql("select * from sneiems.OGCSTMST").ToList<OGCSTMST>();
            //专业
            List<DCSKLMST> zy = DBService.EMS.FromSql("select * from sneiems.DCSKLMST").ToList<DCSKLMST>();
            //人员
            List<CMEMPMST> ry = DBService.EMS.FromSql("select * from sneiems.CMEMPMST").ToList<CMEMPMST>();

            //计算
            List<NPS_DES_ArtificialTime> Sjrgs = new List<NPS_DES_ArtificialTime>();

            //任务单表
            List<DCTASKMENUMST> rwdb = DBService.EMS.FromSql("select * from sneiems.DCTASKMENUMST").ToList<DCTASKMENUMST>();
            //项目表
            List<DCPROJECTMST> xmb = DBService.EMS.FromSql("select * from sneiems.DCPROJECTMST").ToList<DCPROJECTMST>();
            //设计人工时一览表
            List<NPS_DES_HumanTime> sj = new List<NPS_DES_HumanTime>();
            //foreach (DCWTRECMST item in gszb)
            //{
            //    NPS_DES_ArtificialTime i = new NPS_DES_ArtificialTime();
            //    i.Year = item.YEAR;
            //    i.Week = rizblist.Find(z => z.DURL_NO == item.CURWEEK) != null ? rizblist.Find(z => z.DURL_NO == item.CURWEEK).WEEKSEQ.ToString() : "";
            //    i.Rec = bm.Find(z => z.CST_NO == item.REC_CST) != null ? bm.Find(z => z.CST_NO == item.REC_CST).CST_NAM : "";
            //    DCWTRECLIN d = gszblist.Find(z => z.REC_NO == item.REC_NO);
            //    if (d != null)
            //    {
            //        i.SKL = zy.Find(z => z.SKL_NO == d.SKL_NO) != null ? zy.Find(z => z.SKL_NO == d.SKL_NO).SKL_NAM : "";
            //    }
            //    i.User = ry.Find(z => z.EMP_ID == item.REC_USR) != null ? ry.Find(z => z.EMP_ID == item.REC_USR).EMP_NAM : "";

            //    i.SumFdwt = item.SUMFDWT.ToString();
            //    int sum = 0;
            //    sum += item.SUMJ1 != null ? (int)item.SUMJ1 : 0;
            //    sum += item.SUMJ2 != null ? (int)item.SUMJ2 : 0;
            //    sum += item.SUMJ3 != null ? (int)item.SUMJ3 : 0;
            //    sum += item.SUMJ4 != null ? (int)item.SUMJ4 : 0;
            //    sum += item.SUMJ5 != null ? (int)item.SUMJ5 : 0;
            //    sum += item.SUMJ6 != null ? (int)item.SUMJ6 : 0;
            //    sum += item.SUMJ7 != null ? (int)item.SUMJ7 : 0;
            //    i.SumJ += sum;
            //    i.SumactWt = item.SUMACTWT != null ? item.SUMACTWT.ToString() : "0";
            //    if (gszblist.Find(z => z.REC_NO == item.REC_NO) != null && gszblist.Find(z => z.REC_NO == item.REC_NO).WTLX_NO == "XMYG")
            //    {
            //        i.Wtlx = item.SUMACTWT != null ? item.SUMJ6.ToString() : "0";
            //    }
            //    i.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
            //    i.Status = 0;
            //    i.RegHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
            //    i.RegHumName = "系统管理员";
            //    i.RegDate = DateTime.Now;
            //    i.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //    i.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //    i.EpsProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
            //    i.RecycleHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
            //    i.UpdHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
            //    i.UpdHumName = "系统管理员";
            //    i.UpdDate = DateTime.Now;
            //    i.OwnProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
            //    i.OwnProjName = "中石化南京工程公司";
            //    i.ID = Guid.NewGuid();
            //    Sjrgs.Add(i);


            //    //设计人工时一览表

            //    List<DCWTRECLIN> list = gszblist.FindAll(z => z.REC_NO == item.REC_NO);
            //    string week = rizblist.Find(z => z.DURL_NO == item.CURWEEK) != null ? rizblist.Find(z => z.DURL_NO == item.CURWEEK).WEEKSEQ.ToString() : "";
            //    foreach (DCWTRECLIN child in list)
            //    {
            //        DCSKLMST dCSKLMST = zy.Find(z => z.SKL_NO == child.SKL_NO);
            //        if (sj.Find(z => z.ReportYear == item.YEAR && z.ReportWeek == week && z.ProjName == child.PROJECT_NAM) != null)
            //        {
            //            NPS_DES_HumanTime a = sj.Find(z => z.ReportYear == item.YEAR && z.ReportWeek == week && z.ProjName == child.PROJECT_NAM);
            //            if (dCSKLMST == null)
            //            {

            //            }
            //            else
            //            {
            //                switch (dCSKLMST.SKL_NAM)
            //                {
            //                    case "工艺":
            //                        a.Process = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "安全":
            //                        a.Security = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "技术经济":
            //                        a.Technical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "环保":
            //                        a.Environmental = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "管道":
            //                        a.Piping = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "管机":
            //                        a.Mill = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "管材":
            //                        a.Tube = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "电气":
            //                        a.Electrical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "仪表":
            //                        a.Instrument = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "电信":
            //                        a.Telecom = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "静设备":
            //                        a.StaticEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "动设备":
            //                        a.MovingEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "建筑":
            //                        a.Building = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "结构":
            //                        a.Structure = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "消防":
            //                        a.Fire = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "暖通":
            //                        a.Hvac = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "热工":
            //                        a.Thermal = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "给排水":
            //                        a.Drainage = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "总图运输":
            //                        a.Transport = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "粉体":
            //                        a.Powder = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "机修":
            //                        a.MachinePart = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "概算":
            //                        a.Estimate = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    default:
            //                        break;
            //                }
            //            }

            //        }
            //        else
            //        {
            //            NPS_DES_HumanTime a = new NPS_DES_HumanTime();
            //            a.ReportYear = item.YEAR;
            //            a.ReportWeek = week;
            //            a.ProjName = child.PROJECT_NAM;
            //            //if (rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO))==null)
            //            //{
            //            //    continue;
            //            //}
            //            a.ProjId = rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO)) == null ? "" : rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO)).PRJ_ID;
            //            if (rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO)) == null)
            //            {
            //                a.ManageName = "";
            //            }
            //            else
            //            {
            //                a.ManageName = ry.Find(f => f.EMP_ID == rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO)).PRJMGR_ID) == null ? "" : ry.Find(f => f.EMP_ID == rwdb.Find(z => (int)z.PROJECT_NO == Convert.ToInt32(child.PROJECT_NO)).PRJMGR_ID).EMP_NAM;
            //            }

            //            if (dCSKLMST == null)
            //            {

            //            }
            //            else
            //            {
            //                switch (dCSKLMST.SKL_NAM)
            //                {
            //                    case "工艺":
            //                        a.Process = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "安全":
            //                        a.Security = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "技术经济":
            //                        a.Technical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "环保":
            //                        a.Environmental = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "管道":
            //                        a.Piping = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "管机":
            //                        a.Mill = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "管材":
            //                        a.Tube = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "电气":
            //                        a.Electrical = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "仪表":
            //                        a.Instrument = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "电信":
            //                        a.Telecom = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "静设备":
            //                        a.StaticEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "动设备":
            //                        a.MovingEquip = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "建筑":
            //                        a.Building = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "结构":
            //                        a.Structure = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "消防":
            //                        a.Fire = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "暖通":
            //                        a.Hvac = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "热工":
            //                        a.Thermal = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "给排水":
            //                        a.Drainage = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "总图运输":
            //                        a.Transport = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "粉体":
            //                        a.Powder = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "机修":
            //                        a.MachinePart = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    case "概算":
            //                        a.Estimate = (child.SUMWT != null ? (int)child.SUMWT : 0) + (child.JBWT != null ? (int)child.JBWT : 0);
            //                        break;
            //                    default:
            //                        break;
            //                }
            //            }

            //            a.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
            //            a.Status = 0;
            //            a.RegHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
            //            a.RegHumName = "系统管理员";
            //            a.RegDate = DateTime.Now;
            //            a.RegPosiId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //            a.RegDeptId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //            a.EpsProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
            //            a.RecycleHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
            //            a.UpdHumId = Guid.Parse("AD000000-0000-0000-0000-000000000000");
            //            a.UpdHumName = "系统管理员";
            //            a.UpdDate = DateTime.Now;
            //            a.OwnProjId = Guid.Parse("00000000-0000-0000-0000-0000000000A4");//Guid.Parse("00000000-0000-0000-0000-0000000000A4");
            //            a.OwnProjName = "中石化南京工程公司";
            //            a.ID = Guid.NewGuid();
            //            sj.Add(a);
            //        }
            //    }

            //}







            List<DCAREATYPMST> sh = DBService.EMS.FromSql("select * from sneiems.DCAREATYPMST").ToList<DCAREATYPMST>();
            List<DCAREAMST> sq = DBService.EMS.FromSql("select * from sneiems.DCAREAMST").ToList<DCAREAMST>(); 
            List<DCPRJJDMST> sjjd = DBService.EMS.FromSql("select * from sneiems.DCPRJJDMST").ToList<DCPRJJDMST>();
            //设计项目一览表
            List<NPS_DES_ProjInfo> sjxm = new List<NPS_DES_ProjInfo>();
            List<NPS_DES_ProjInfo> Insertsjxm = new List<NPS_DES_ProjInfo>();
            List<NPS_DES_ProjInfo> XtList = DBService.Context.FromSql("select * from NPS_DES_ProjInfo").ToList<NPS_DES_ProjInfo>();
            foreach (DCPROJECTMST item in xmb)
            {
                //查找原来的数据，找到就更新，找不到就不更新

                if (rwdb.Find(zz => zz.PROJECT_NO == item.PROJECT_NO)==null)
                {
                    continue;
                }
                NPS_DES_ProjInfo Xt = XtList.Find(z=>z.ProjCode== rwdb.Find(zz => zz.PROJECT_NO == item.PROJECT_NO).PRJ_ID);
                if (Xt!=null)
                {
                    //更新某些字段
                    Xt.Address = sh.Find(d => sq.Find(z => z.AREA_NO == item.AREA_NO).AREATYP_NO == d.AREATYP_NO).AREATYP_NAM;
                    Xt.ConCode = item.BARTZ_ID;
                    Xt.ProjName = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PROJECT_NAM;
                    Xt.ProjCode = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJ_ID;
                    Xt.ConStartDate = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PLABEG_DTM;
                    DateTime date;
                    if (DateTime.TryParse(xmb.Find(z => z.PROJECT_NO == item.PROJECT_NO).BAR_JD, out date))
                    {
                        Xt.ConEndDate = date;
                    }
                    if ( rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO)!=null)
                    {
                        Xt.ManageName = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID)==null?"": ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                    }
                    else
                    {
                        Xt.ManageName = "";
                    }

                    if (ry.Find(d => d.EMP_ID == item.PLAPRJ_USR) != null)
                    {
                        Xt.JhName = ry.Find(d => d.EMP_ID == item.PLAPRJ_USR).EMP_NAM;
                    }
                    else
                    {
                        Xt.JhName = "";
                    }
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {

                        Xt.Engineer = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).WDSUR_ID) == null ? "" : ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).WDSUR_ID).EMP_NAM;
                    }
                    else
                    {
                        Xt.Engineer = "";
                    }
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {

                        Xt.Design = sjjd.Find(d => d.PRJJD_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJJD_ID) == null ? "" : sjjd.Find(d => d.PRJJD_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJJD_ID).PRJJD_NAM;
                        Xt.LeadingRoom = bm.Find(d => d.CST_NO == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).CST_NO)==null?"": bm.Find(d => d.CST_NO == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).CST_NO).CST_NAM;
                    }
                    else
                    {
                        Xt.Design = "";
                        Xt.LeadingRoom = "";
                    }
                 
                    Xt.DesignMoney = item.SJTZ_AMT;
                    Xt.ConMoney = item.YGBAR_AMT;
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {
                       
                        Xt.CollectionManage = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID) == null ? "" : ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                    }
                    else
                    {
                        Xt.CollectionManage = "";
                    }
                    if (item.PROJECT_NO==1291)
                    {
                        string a = "";
                    }

                    if (item.PRO_STA==null)
                    {
                        Xt.XmStatus = "";
                    }
                    else
                    {
                        switch (Convert.ToInt32(item.PRO_STA))
                        {
                            case 6:
                                Xt.XmStatus = "关闭";
                                break;
                            case 1:
                                Xt.XmStatus = "启动";
                                break;
                            case 2:
                                Xt.XmStatus = "运行";
                                break;
                            case 3:
                                Xt.XmStatus = "暂停";
                                break;
                            case 4:
                                Xt.XmStatus = "中止";
                                break;
                            case 5:
                                Xt.XmStatus = "完工";
                                break;
                            default:
                                break;
                        }
                    }
                   

                    sjxm.Add(Xt);
                }
                else
                {
                    NPS_DES_ProjInfo a = new NPS_DES_ProjInfo();
                    a.Address = sh.Find(d => sq.Find(z => z.AREA_NO == item.AREA_NO).AREATYP_NO == d.AREATYP_NO).AREATYP_NAM;
                    a.ConCode = item.BARTZ_ID;
                    a.ProjName = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PROJECT_NAM;
                    a.ProjCode = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJ_ID;
                    a.ConStartDate = rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PLABEG_DTM;
                    DateTime date;
                    if (DateTime.TryParse(xmb.Find(z => z.PROJECT_NO == item.PROJECT_NO).BAR_JD, out date))
                    {
                        a.ConEndDate = date;
                    }
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {
                        a.ManageName = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID) == null ? "" : ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                    }
                    else
                    {
                        a.ManageName = "";
                    }

                    if (ry.Find(d => d.EMP_ID == item.PLAPRJ_USR) != null)
                    {
                        a.JhName = ry.Find(d => d.EMP_ID == item.PLAPRJ_USR).EMP_NAM;
                    }
                    else
                    {
                        a.JhName = "";
                    }
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {

                        a.Engineer = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).WDSUR_ID) == null ? "" : ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).WDSUR_ID).EMP_NAM;
                    }
                    else
                    {
                        a.Engineer = "";
                    }
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {

                        a.Design = sjjd.Find(d => d.PRJJD_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJJD_ID) == null ? "" : sjjd.Find(d => d.PRJJD_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJJD_ID).PRJJD_NAM;
                        a.LeadingRoom = bm.Find(d => d.CST_NO == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).CST_NO) == null ? "" : bm.Find(d => d.CST_NO == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).CST_NO).CST_NAM;
                    }
                    else
                    {
                        a.Design = "";
                        a.LeadingRoom = "";
                    }

                    a.DesignMoney = item.SJTZ_AMT;
                    a.ConMoney = item.YGBAR_AMT;
                    if (rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO) != null)
                    {

                        a.CollectionManage = ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID) == null ? "" : ry.Find(d => d.EMP_ID == rwdb.Find(z => z.PROJECT_NO == item.PROJECT_NO).PRJMGR_ID).EMP_NAM;
                    }
                    else
                    {
                        a.CollectionManage = "";
                    }

                    if (item.PRO_STA == null)
                    {
                        a.XmStatus = "";
                    }
                    else
                    {
                        switch (Convert.ToInt32(item.PRO_STA))
                        {
                            case 6:
                                a.XmStatus = "关闭";
                                break;
                            case 1:
                                a.XmStatus = "启动";
                                break;
                            case 2:
                                a.XmStatus = "运行";
                                break;
                            case 3:
                                a.XmStatus = "暂停";
                                break;
                            case 4:
                                a.XmStatus = "中止";
                                break;
                            case 5:
                                a.XmStatus = "完工";
                                break;
                            default:
                                break;
                        }
                    }
                    a.BizAreaId = Guid.Parse("00000000-0000-0000-0000-00000000000A");
                    a.TableName = "NPS_DES_ProjInfo";
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
                    Insertsjxm.Add(a);
                }

              
            }


            //加载数据
            //DBService.Context.Insert(sj);
            //DBService.Context.Insert(Sjrgs);
            DBService.Context.Insert(Insertsjxm);
            DBService.Context.Update(sjxm);
            return "";
        }


       
    }
}
