using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power.SPMEMS.Services
{
    class RecursionHelper//29号五点半的备份
    {
        /*
        计算项目产值支取对应产值
        胡宁绘
        */
        public string GetBonus(string ID)
        {
            // 找到对应表单数据
            Power.Business.IBusinessOperate ysBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目产值申请支取主表
            Power.Business.IBusinessList ysBoList = ysBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (ysBoList.Count > 0)
            {
                // 定义各产值类型的汇总字段
                string field = "";
                // 判断是否加上老项目产值
                int ifOld = 0;
                // 定义项目申请总产值
                double ApplyBonus = 0;
                // 定义老项目要加的各个产值
                // 专业设计产值  已支取专业产值  剩余专业产值
                double totalProfBonus = 0, grantProfBonus = 0, remainProfBonus = 0;
                // 专业追加产值  已支取追加产值  剩余追加产值
                double totalAddBonus = 0, grantAddBonus = 0, remainAddBonus = 0;
                // 定义已支取产值
                double grantBonus = 0;
                // 总产值、已支取、剩余 最终结果
                double total = 0, grant = 0, remain = 0;

                //定义不同类型追加查询条件
                string swhere = ""; // 查询项目产值申请
                string where = ""; // 查询项目产值支取
                string Type = ysBoList[0]["OutputType"].ToString();
                if (Type == "专业产值")
                {
                    field = "ProfDesBonus";
                    ifOld = 1;
                    Type = "施工图产值";
                }
                else if (Type == "所管产值")
                {
                    field = "DeptBonus";
                    Type = "施工图产值";
                }
                else if (Type == "追加产值")
                {
                    field = "AddBonus";
                    ifOld = 1;
                }
                else if (Type == "临时产值")
                {
                    field = "ApplyTempBonus";
                }

                if (Type == "临时产值")
                {
                    swhere = " AND B.DeptID = '" + ysBoList[0]["DeptID"] + @"'";
                    where = " AND DeptID = '" + ysBoList[0]["DeptID"] + @"'";
                }
                else
                {
                    if (Type == "施工图产值" || Type == "追加产值")
                    {
                        swhere = " AND B.DeptID = '" + ysBoList[0]["DeptID"] + @"'
                               AND B.ProfID = '" + ysBoList[0]["ProfID"] + @"'";
                        where = " AND DeptID = '" + ysBoList[0]["DeptID"] + @"'
                               AND ProfID = '" + ysBoList[0]["ProfID"] + @"'";
                    }
                    else
                    {
                        swhere = " AND DeptID = '" + ysBoList[0]["DeptID"] + @"'
                               AND ProfID = '" + ysBoList[0]["ProfID"] + @"'";
                        where = " AND DeptID = '" + ysBoList[0]["DeptID"] + @"'
                               AND ProfID = '" + ysBoList[0]["ProfID"] + @"'";
                    }
                }

                // 计算项目产值申请总产值
                XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                string sql = "";
                if (Type == "优化产值" || Type == "三维产值")
                {
                    // 优化产值和三维产值只有主表数据
                    field = "PeriodApplyBonus";
                    sql = @"--查询对应类型项目产值申请总产值:类型为  " + Type + @"
                     SELECT
                     ISNULL(SUM(A." + field + @"),0) " + field + @" FROM NPS_DES_ProjectOutputApply A
                     WHERE A.ProjectID = '" + ysBoList[0]["ProjectID"] + @"' --项目
                     AND A.OutputType = '" + Type + @"' --类型
                     AND A.Year = '" + ysBoList[0]["Year"] + @"' --年度
                     AND A.Month = '" + ysBoList[0]["Month"] + @"' --月度
                     AND A.Status IN(35,50) --表单状态
                     " + swhere + @" --部门、专业
                      ";
                }
                else
                {
                    sql = @"--查询对应类型项目产值申请总产值:类型为  " + Type + @"
                     SELECT
                     ISNULL(SUM(B." + field + @"),0) " + field + @" FROM NPS_DES_ProjectOutputApply A
                     JOIN NPS_DES_ProjectOutputApply_List B
                     ON A.ID = B.FID
                     WHERE A.ProjectID = '" + ysBoList[0]["ProjectID"] + @"' --项目
                     AND A.OutputType = '" + Type + @"' --类型
                     AND A.Year = '" + ysBoList[0]["Year"] + @"' --年度
                     AND A.Month = '" + ysBoList[0]["Month"] + @"' --月度
                     AND A.Status IN(35,50) --表单状态
                     " + swhere + @" --对应部门、专业，临时产值为部门
                      ";
                }
                System.Data.DataTable dt1 = dal1.Session.Query(sql).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    // 赋值项目申请产值
                    ApplyBonus = double.Parse(dt1.Rows[0][field].ToString());
                    // 判断是否需要加上项目产值信息处产值
                    if (ifOld == 1)
                    {
                        XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
                        string sql2 = "";
                        sql2 = @"--查询对应类型项目产值申请总产值:类型为  " + Type + @"
                                SELECT
                                --专业设计产值  已支取专业产值  剩余专业产值
                                ISNULL(SUM(Y.TotalProfBonus),0) TotalProfBonus, ISNULL(SUM(Y.GrantProfBonus),0) GrantProfBonus,ISNULL(SUM(Y.RemainProfBonus),0) RemainProfBonus,
                                --专业追加产值  已支取追加产值  剩余追加产值
                                ISNULL(SUM(Y.ProfAddBonus),0) ProfAddBonus, ISNULL(SUM(Y.GrantAddBonus),0) GrantAddBonus, ISNULL(SUM(Y.RemainAddBonus),0) RemainAddBonus
                                FROM NPS_DES_ProjectOutputCut X
                                JOIN NPS_DES_ProjectOutputCut_List Y
                                ON X.ID = Y.FID
                                WHERE X.ProjectID = '" + ysBoList[0]["ProjectID"] + @"'
                                AND X.Status IN (35,50)
                                " + where + @"
                        ";
                        System.Data.DataTable dt2 = dal2.Session.Query(sql2).Tables[0];
                        // 有对应产值
                        if (dt2.Rows.Count > 0)
                        {
                            // 赋值各个对应产值
                            totalProfBonus = double.Parse(dt2.Rows[0]["TotalProfBonus"].ToString());
                            grantProfBonus = double.Parse(dt2.Rows[0]["GrantProfBonus"].ToString());
                            remainProfBonus = double.Parse(dt2.Rows[0]["RemainProfBonus"].ToString());
                            totalAddBonus = double.Parse(dt2.Rows[0]["ProfAddBonus"].ToString());
                            grantAddBonus = double.Parse(dt2.Rows[0]["GrantAddBonus"].ToString());
                            remainAddBonus = double.Parse(dt2.Rows[0]["RemainAddBonus"].ToString());
                        }
                    }
                }

                // 计算项目支取
                XCode.DataAccessLayer.DAL dal3 = XCode.DataAccessLayer.DAL.Create();
                string sql3 = "";
                sql3 = @"--查询对应类型项目产值已支取产值:类型为  " + Type + @"
                               SELECT
                               -- 本次申请产值汇总
                               ISNULL(SUM(A.PeriodGrantBonus),0) PeriodGrantBonus
                               FROM NPS_DES_ProjectOutputGrant A
                               WHERE A.ProjectID = '" + ysBoList[0]["ProjectID"] + @"'
                               AND A.OutputType = '" + Type + @"'
                               AND A.Year = '" + ysBoList[0]["Year"] + @"'
                               AND A.Month = '" + ysBoList[0]["Month"] + @"'
                               AND A.Status IN (35,50)
                               " + where + @"
                        ";
                System.Data.DataTable dt3 = dal3.Session.Query(sql3).Tables[0];
                if (dt3.Rows.Count > 0)
                {
                    // 赋值已支取产值
                    grantBonus = double.Parse(dt3.Rows[0]["PeriodGrantBonus"].ToString());
                }


                if (ifOld == 1)
                {

                    if (ysBoList[0]["OutputType"].ToString() == "专业产值")
                    {
                        // 计算总产值
                        // 项目产值申请 + 老项目总产值
                        total = ApplyBonus + totalProfBonus;
                        // 计算已支取
                        // 项目产值支取 + 老项目已支取
                        grant = grantBonus + grantProfBonus;
                        // 计算剩余产值
                        // 总产值 - 已支取产值 + 老项目剩余产值
                        remain = (total - grant) + remainProfBonus;
                    }
                    else if (Type == "追加产值")
                    {
                        // 项目产值申请 + 老项目总产值
                        total = ApplyBonus + totalAddBonus;
                        // 计算已支取
                        // 项目产值支取 + 老项目已支取
                        grant = grantBonus + grantAddBonus;
                        // 计算剩余产值
                        // 总产值 - 已支取产值 + 老项目剩余产值
                        remain = (total - grant) + remainAddBonus;
                    }
                }
                else
                {
                    // 非老项目
                    // 计算剩余产值
                    total = ApplyBonus;
                    grant = grantBonus;
                    remain = ApplyBonus - grantBonus;
                }

                // 总产值、已支取产值、剩余产值已计算好
                ysBoList[0].SetItem("TotalBonus", total); // 总产值
                ysBoList[0].SetItem("GrantBonus", grant); // 已支取产值
                ysBoList[0].SetItem("RemainBonus", remain); // 剩余产值
                ysBoList[0].Save(System.ComponentModel.DataObjectMethodType.Update);

            }

            return "";
        }




        /*
        计算支取比例
        胡宁绘
        */
        public string GetBonusRate(string ID)
        {
            // 定义总产值、剩余产值、已支取产值、本次支取产值变量
            double total = 0, remain = 0, grant = 0, preBonus = 0;
            // 定义产值类型变量
            string type = "";
            // 返回的判断值
            string result = "";
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql = "";
            sql = @"--查询当前总产值、剩余产值、本次支取产值
                      SELECT TotalBonus,RemainBonus,GrantBonus,PeriodGrantBonus,OutputType FROM NPS_DES_ProjectOutputGrant A
                      WHERE A.ID = '" + ID + @"'
                  ";
            System.Data.DataTable dt1 = dal1.Session.Query(sql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                total = double.Parse(dt1.Rows[0]["TotalBonus"].ToString());
                remain = double.Parse(dt1.Rows[0]["RemainBonus"].ToString());
                grant = double.Parse(dt1.Rows[0]["GrantBonus"].ToString());
                preBonus = double.Parse(dt1.Rows[0]["PeriodGrantBonus"].ToString());
                type = dt1.Rows[0]["OutputType"].ToString();
            }
            if (type == "专业产值")
            {
                // 计算支取比例
                double grantRate = (grant / total) * 100;
                // 计算已支取比例
                double preGrantRate = (preBonus / total) * 100;
                // 修改比例
                XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
                string sql2 = "Update NPS_DES_ProjectOutputGrant set GrantRate=" + grantRate + " , PeriodGrantRate=" + preGrantRate + @"
                               WHERE ID = '" + ID + @"'
                               ";
                dal2.Session.Execute(sql2);
            }
            return result;
        }


        /*
        项目产值支取获取临时总产值
        胡宁绘
        */
        public string TempSum(string swhere, string deptName)
        {
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql = "";
            sql = @"--根据产值类型和部门和年月找到对应申请单临时产值本月汇总
                      SELECT isnull(SUM(ProcessSum),0) 工艺所,isnull(SUM(ElectricitySum),0) 自电所,isnull(SUM(CivilSum),0) 土建所,isnull(SUM(MatDivisionSum),0) 材料工程事业部 FROM NPS_DES_ProjectTempOutputSum
                      WHERE " + swhere + @"
                      ";
            System.Data.DataTable dt1 = dal1.Session.Query(sql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                return dt1.Rows[0][deptName].ToString();
            }
            return "0";
        }

        /*
        切换向导时清空子表数据
        胡宁绘
        */
        public string DeleteProf(string ID)
        {
            try
            {
                Power.Business.IBusinessOperate bo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");
                Power.Business.IBusinessList boList = bo.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (boList.Count > 0)
                {
                    boList.Delete();
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";
        }

        public string DeleteProfBonus(string ID)
        {
            try
            {
                Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目专业产值支取
                Power.Business.IBusinessList GrantBo = grantBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (GrantBo.Count > 0)
                {
                    Power.Business.IBusinessOperate grantList = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");//项目专业产值支取子表1
                    Power.Business.IBusinessList GrantListBo = grantList.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    Power.Business.IBusinessOperate grantListTwo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_EpsList");//项目专业产值支取子表2
                    Power.Business.IBusinessList GrantListBoTwo = grantListTwo.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (GrantListBoTwo.Count > 0)
                    {
                        for (int i = 0; i < GrantListBoTwo.Count; i++)
                        {
                            string htsql = "";
                            XCode.DataAccessLayer.DAL dalsql = XCode.DataAccessLayer.DAL.Create();
                            string sSQL = "";
                            sSQL = "select isnull(StatusCode,'') as StatusCode from  NPS_DES_ProfOutputCut_ListS WHERE ID = '" + GrantListBoTwo[i]["ListSID"] + "' ";
                            System.Data.DataTable dt2 = dalsql.Session.Query(sSQL).Tables[0];
                            if (dt2.Rows.Count > 0)
                            {
                                if (dt2.Rows[0]["StatusCode"].ToString().Equals("正在支取") || dt2.Rows[0]["StatusCode"].ToString().Equals(""))
                                {
                                    htsql = "";
                                    htsql = @"UPDATE NPS_DES_ProfOutputCut_ListS SET StatusCode = '' WHERE ID = '" + GrantListBoTwo[i]["ListSID"] + "'";
                                    dalsql.Session.Execute(htsql);
                                }
                                else if (dt2.Rows[0]["StatusCode"].ToString().Equals("正在增补"))
                                {
                                    htsql = "";
                                    htsql = @"UPDATE NPS_DES_ProfOutputCut_ListS SET StatusCode = '待增补' WHERE ID = '" + GrantListBoTwo[i]["ListSID"] + "'";
                                    dalsql.Session.Execute(htsql);
                                }

                            }

                            htsql = "";
                            htsql = @"
                                UPDATE NPS_DES_ProfOutputCut_ListS SET IfGrant = NULL WHERE ID = '" + GrantListBoTwo[i]["ListSID"] + "'";
                            dalsql.Session.Execute(htsql);


                        }
                        GrantListBo.Delete();
                        GrantListBoTwo.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";
        }





        public string CheckEpsProf(string EpsProjId, string ProfID, string ID, string OutputType, string Month, string Year)
        {
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            sql1 = @"SELECT * FROM NPS_DES_ProjectOutputGrant WHERE ProjectID = '" + EpsProjId.ToString() + "' AND ProfID = '" + ProfID.ToString() + @"'  AND ID != '" + ID.ToString() + @"' AND OutPutType='" + OutputType.ToString() + @"' AND Year = '" + Year.ToString() + @"' AND Month = '" + Month.ToString() + @"'";
            if (OutputType.ToString() == "专业产值")
            {
                sql1 += " AND (Status != 35 AND Status != 50 AND Status != 40)";
            }
            else
            {
                sql1 += " AND (Status !=40)";
            }
            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                return "已存在";
            }
            return "";
        }
        /*
        根据专业施工图产值获取对应人员的专业产值
        胡宁绘
        */
        public string GetProfBonus(string ID)
        {
            DeleteProfBonus(ID);
            DataTable tblDatas = new DataTable("Datas");
            tblDatas.Columns.Add("HumanID", Type.GetType("System.String"));//设计人ID
            tblDatas.Columns.Add("HumanCode", Type.GetType("System.String"));//设计人编号
            tblDatas.Columns.Add("HumanName", Type.GetType("System.String"));//设计人名称
            tblDatas.Columns.Add("Bonus", Type.GetType("System.String"));//分配总产值
            XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
            Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目专业产值支取
            Power.Business.IBusinessList GrantBo = grantBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (GrantBo.Count > 0)
            {
                string sql2 = "";
                sql2 = @"select * from view_ProfOutputCut WHERE HeadHumID = '" + GrantBo[0]["RegHumId"] + @"' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND IfIssue = '待支取'";
                System.Data.DataTable dt2 = dal2.Session.Query(sql2).Tables[0];
                if (dt2.Rows.Count > 0)
                {
                    for (int x = 0; x < dt2.Rows.Count; x++)
                    {
                        string sSQL = "select* from NPS_ENGPLAN_Project where IsEnableProfOutput = '是'  and ID='" + dt2.Rows[x]["EpsProjId"] + "'";
                        System.Data.DataTable DsData = dal2.Session.Query(sSQL).Tables[0];
                        if (DsData.Rows.Count > 0)//新项目
                        {

                            if (dt2.Rows[x]["IfGrant"].ToString().Equals("") && (dt2.Rows[x]["PayBonus"].ToString().Equals("0") && dt2.Rows[x]["PayBonusRate"].ToString().Equals("0")))
                            {
                                Power.Business.IBaseBusiness sItem = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_EpsList");
                                sItem.SetItem("ID", Guid.NewGuid().ToString());
                                sItem.SetItem("FID", ID);
                                sItem.SetItem("AllBonus", dt2.Rows[x]["Bonus"]);
                                sItem.SetItem("ThisApplyScale", dt2.Rows[x]["PeriodCompleteScale"]);
                                sItem.SetItem("SubmitDocuOrderID", dt2.Rows[x]["SubmitDocuOrderID"].ToString());//关联互提资料单ID
                                sItem.SetItem("ProjectID", dt2.Rows[x]["EpsProjId"].ToString());//项目ID
                                sItem.SetItem("ProjectCode", dt2.Rows[x]["EpsProjCode"].ToString());//项目编号
                                sItem.SetItem("ProjectName", dt2.Rows[x]["EpsProjName"].ToString());//项目名称
                                sItem.SetItem("HumanID", dt2.Rows[x]["DesHumanID"].ToString());//人员ID
                                sItem.SetItem("HumanCode", dt2.Rows[x]["DesHumanCode"].ToString());//人员编号
                                sItem.SetItem("HumanName", dt2.Rows[x]["DesHumanName"].ToString());//人员名称
                                sItem.SetItem("Bonus", dt2.Rows[x]["AllocationBonus"].ToString());//产值
                                sItem.SetItem("SubID", dt2.Rows[x]["SubID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["SubID"].ToString());//子项ID
                                sItem.SetItem("SubCode", dt2.Rows[x]["SubCode"].ToString());//子项编号
                                sItem.SetItem("SubName", dt2.Rows[x]["SubName"].ToString());//子项名称
                                sItem.SetItem("TaskID", dt2.Rows[x]["TaskID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["TaskID"].ToString());//任务ID
                                sItem.SetItem("TaskCode", dt2.Rows[x]["TaskCode"].ToString());//任务编号
                                sItem.SetItem("TaskName", dt2.Rows[x]["TaskName"].ToString());//任务名称
                                sItem.SetItem("TaskType", dt2.Rows[x]["TaskType"].ToString());//任务类型
                                sItem.SetItem("Rate", dt2.Rows[x]["Rate"].ToString());//产值比例
                                sItem.SetItem("DesignRate", dt2.Rows[x]["DesignRate"].ToString());//任务比例
                                sItem.SetItem("WBSID", dt2.Rows[x]["WBSID"].ToString());//WBSID
                                sItem.SetItem("IfGrant", dt2.Rows[x]["IfGrant"].ToString());//是否支取
                                sItem.SetItem("Type", dt2.Rows[x]["Type"].ToString());//设校审类型
                                sItem.SetItem("HeadHumID", dt2.Rows[x]["HeadHumID"].ToString());//专业负责人ID
                                sItem.SetItem("HeadHumCode", dt2.Rows[x]["HeadHumCode"].ToString());//专业负责人编号
                                sItem.SetItem("HeadHumName", dt2.Rows[x]["HeadHumName"].ToString());//专业负责人名称
                                sItem.SetItem("ListSID", dt2.Rows[x]["ProfOutputCutLSID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["ProfOutputCutLSID"].ToString());//专业施工图产值分解孙表ID
                                sItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                XCode.DataAccessLayer.DAL dalsql1 = XCode.DataAccessLayer.DAL.Create();
                                string cpsql = "";
                                cpsql = "UPDATE NPS_DES_ProfOutputCut_ListS SET StatusCode  = '正在支取' WHERE ID = '" + dt2.Rows[x]["ProfOutputCutLSID"] + "'";
                                dalsql1.Session.Execute(cpsql);
                            }
                            else
                            {
                                Power.Business.IBaseBusiness sItem = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_EpsList");
                                sItem.SetItem("ID", Guid.NewGuid().ToString());
                                sItem.SetItem("FID", ID);
                                sItem.SetItem("AllBonus", dt2.Rows[x]["Bonus"]);
                                sItem.SetItem("ThisApplyScale", dt2.Rows[x]["PeriodCompleteScale"]);
                                sItem.SetItem("SubmitDocuOrderID", dt2.Rows[x]["SubmitDocuOrderID"].ToString());//关联互提资料单ID
                                sItem.SetItem("ProjectID", dt2.Rows[x]["EpsProjId"].ToString());//项目ID
                                sItem.SetItem("ProjectCode", dt2.Rows[x]["EpsProjCode"].ToString());//项目编号
                                sItem.SetItem("ProjectName", dt2.Rows[x]["EpsProjName"].ToString());//项目名称
                                sItem.SetItem("HumanID", dt2.Rows[x]["DesHumanID"].ToString());//人员ID
                                sItem.SetItem("HumanCode", dt2.Rows[x]["DesHumanCode"].ToString());//人员编号
                                sItem.SetItem("HumanName", dt2.Rows[x]["DesHumanName"].ToString());//人员名称
                                sItem.SetItem("Bonus", dt2.Rows[x]["AllocationBonus"].ToString());//产值
                                sItem.SetItem("SubID", dt2.Rows[x]["SubID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["SubID"].ToString());//子项ID
                                sItem.SetItem("SubCode", dt2.Rows[x]["SubCode"].ToString());//子项编号
                                sItem.SetItem("SubName", dt2.Rows[x]["SubName"].ToString());//子项名称
                                sItem.SetItem("TaskID", dt2.Rows[x]["TaskID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["TaskID"].ToString());//任务ID
                                sItem.SetItem("TaskCode", dt2.Rows[x]["TaskCode"].ToString());//任务编号
                                sItem.SetItem("TaskName", dt2.Rows[x]["TaskName"].ToString());//任务名称
                                sItem.SetItem("TaskType", dt2.Rows[x]["TaskType"].ToString());//任务类型
                                sItem.SetItem("Rate", dt2.Rows[x]["Rate"].ToString());//产值比例
                                sItem.SetItem("DesignRate", dt2.Rows[x]["DesignRate"].ToString());//任务比例
                                sItem.SetItem("WBSID", dt2.Rows[x]["WBSID"].ToString());//WBSID
                                sItem.SetItem("IfGrant", dt2.Rows[x]["IfGrant"].ToString());//是否支取
                                sItem.SetItem("Type", dt2.Rows[x]["Type"].ToString());//设校审类型
                                sItem.SetItem("HeadHumID", dt2.Rows[x]["HeadHumID"].ToString());//专业负责人ID
                                sItem.SetItem("HeadHumCode", dt2.Rows[x]["HeadHumCode"].ToString());//专业负责人编号
                                sItem.SetItem("HeadHumName", dt2.Rows[x]["HeadHumName"].ToString());//专业负责人名称
                                sItem.SetItem("ListSID", dt2.Rows[x]["ProfOutputCutLSID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["ProfOutputCutLSID"].ToString());//专业施工图产值分解孙表ID
                                sItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                XCode.DataAccessLayer.DAL dalsql1 = XCode.DataAccessLayer.DAL.Create();
                                string cpsql = "";
                                cpsql = "UPDATE NPS_DES_ProfOutputCut_ListS SET StatusCode  = '正在支取' WHERE ID = '" + dt2.Rows[x]["ProfOutputCutLSID"] + "'";
                                dalsql1.Session.Execute(cpsql);
                            }
                        }
                        else//老项目
                        {
                            Power.Business.IBaseBusiness sItem = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_EpsList");
                            sItem.SetItem("ID", Guid.NewGuid().ToString());
                            sItem.SetItem("FID", ID);
                            sItem.SetItem("AllBonus", dt2.Rows[x]["Bonus"]);
                            sItem.SetItem("ThisApplyScale", dt2.Rows[x]["PeriodCompleteScale"]);
                            sItem.SetItem("SubmitDocuOrderID", dt2.Rows[x]["SubmitDocuOrderID"].ToString());//关联互提资料单ID
                            sItem.SetItem("ProjectID", dt2.Rows[x]["EpsProjId"].ToString());//项目ID
                            sItem.SetItem("ProjectCode", dt2.Rows[x]["EpsProjCode"].ToString());//项目编号
                            sItem.SetItem("ProjectName", dt2.Rows[x]["EpsProjName"].ToString());//项目名称
                            sItem.SetItem("HumanID", dt2.Rows[x]["DesHumanID"].ToString());//人员ID
                            sItem.SetItem("HumanCode", dt2.Rows[x]["DesHumanCode"].ToString());//人员编号
                            sItem.SetItem("HumanName", dt2.Rows[x]["DesHumanName"].ToString());//人员名称
                            sItem.SetItem("Bonus", dt2.Rows[x]["AllocationBonus"].ToString());//产值
                            sItem.SetItem("SubID", dt2.Rows[x]["SubID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["SubID"].ToString());//子项ID
                            sItem.SetItem("SubCode", dt2.Rows[x]["SubCode"].ToString());//子项编号
                            sItem.SetItem("SubName", dt2.Rows[x]["SubName"].ToString());//子项名称
                            sItem.SetItem("TaskID", dt2.Rows[x]["TaskID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["TaskID"].ToString());//任务ID
                            sItem.SetItem("TaskCode", dt2.Rows[x]["TaskCode"].ToString());//任务编号
                            sItem.SetItem("TaskName", dt2.Rows[x]["TaskName"].ToString());//任务名称
                            sItem.SetItem("TaskType", dt2.Rows[x]["TaskType"].ToString());//任务类型
                            sItem.SetItem("Rate", dt2.Rows[x]["Rate"].ToString());//产值比例
                            sItem.SetItem("DesignRate", dt2.Rows[x]["DesignRate"].ToString());//任务比例
                            sItem.SetItem("WBSID", dt2.Rows[x]["WBSID"].ToString());//WBSID
                            sItem.SetItem("IfGrant", dt2.Rows[x]["IfGrant"].ToString());//是否支取
                            sItem.SetItem("Type", dt2.Rows[x]["Type"].ToString());//设校审类型
                            sItem.SetItem("HeadHumID", dt2.Rows[x]["HeadHumID"].ToString());//专业负责人ID
                            sItem.SetItem("HeadHumCode", dt2.Rows[x]["HeadHumCode"].ToString());//专业负责人编号
                            sItem.SetItem("HeadHumName", dt2.Rows[x]["HeadHumName"].ToString());//专业负责人名称
                            sItem.SetItem("ListSID", dt2.Rows[x]["ProfOutputCutLSID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["ProfOutputCutLSID"].ToString());//专业施工图产值分解孙表ID
                            sItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                            XCode.DataAccessLayer.DAL dalsql1 = XCode.DataAccessLayer.DAL.Create();
                            string cpsql = "";
                            cpsql = "UPDATE NPS_DES_ProfOutputCut_ListS SET IfGrant  = '占用' WHERE ID = '" + dt2.Rows[x]["ProfOutputCutLSID"] + "'";
                            dalsql1.Session.Execute(cpsql);


                        }
                    }
                }
            }


            // 数据已插入 进行根据人员进行分组
            XCode.DataAccessLayer.DAL dal3 = XCode.DataAccessLayer.DAL.Create();
            string sql3 = "";
            sql3 = @"
                         SELECT HumanID,HumanCode,HumanName,SUM(Bonus) Bonus FROM NPS_DES_ProjectOutputGrant_EpsList
                         WHERE FID = '" + ID + @"'
                         GROUP BY HumanID,HumanCode,HumanName
                         ";
            System.Data.DataTable dt3 = dal3.Session.Query(sql3).Tables[0];
            if (dt3.Rows.Count > 0)
            {
                for (int y = 0; y < dt3.Rows.Count; y++)
                {
                    tblDatas.Rows.Add(new object[] {
                        dt3.Rows[y]["HumanID"], dt3.Rows[y]["HumanCode"], dt3.Rows[y]["HumanName"],
                        double.Parse(dt3.Rows[y]["Bonus"].ToString())});
                }
            }
            foreach (DataRow dataRows in tblDatas.Rows)
            {
                Power.Business.IBaseBusiness sItemList = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_HumList");
                sItemList.SetItem("ID", Guid.NewGuid().ToString());
                sItemList.SetItem("FID", ID);
                sItemList.SetItem("HumanID", dataRows["HumanID"].ToString());//人员ID
                sItemList.SetItem("HumanCode", dataRows["HumanCode"].ToString());//人员编号
                sItemList.SetItem("HumanName", dataRows["HumanName"].ToString());//人员名称
                sItemList.SetItem("Bonus", dataRows["Bonus"].ToString());//产值
                sItemList.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            OutputControl outputControl = new OutputControl();
            outputControl.GetZB(ID);
            return "";
        }




        public string GetDelData(string ID)
        {
            Power.Business.IBusinessOperate HumList = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_EpsList");
            Power.Business.IBusinessList HumListBO = HumList.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (HumListBO.Count > 0)
            {
                XCode.DataAccessLayer.DAL dalsql = XCode.DataAccessLayer.DAL.Create();
                string htsql = "";
                htsql = @"
                           SELECT
                           stuff((select ','+''''+cast(ListSID as varchar(max))+''''   from NPS_DES_ProjectOutputGrant_EpsList A
                           WHERE  FID =  '" + ID + @"'
                           FOR xml PATH('')), 1, 1, '') ID
                         ";
                System.Data.DataTable dt = dalsql.Session.Query(htsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["ID"].ToString();
                }
            }
            return "";
        }

        public void UpdateIfGrant(string ID)
        {
            XCode.DataAccessLayer.DAL dalsql = XCode.DataAccessLayer.DAL.Create();
            string htsql = "";
            htsql = @"UPDATE NPS_DES_ProfOutputCut_ListS SET IfGrant = NULL WHERE ID IN (
                                " + ID + @" )";
            dalsql.Session.Execute(htsql);
        }

        public string DeleteIfGrant(string ID)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sql = "";
            sql = @"UPDATE NPS_DES_ProfOutputCut_ListS SET IfGrant = NULL
                    WHERE ID IN (
                    SELECT ListSID FROM NPS_DES_ProjectOutputGrant_EpsList WHERE FID = '" + ID + @"'
                    )";
            dal.Session.Execute(sql);
            return "";
        }

        public string getHumanId(string KeyValue, string DeptName)
        {
            Power.Business.IBusinessOperate HumList = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");
            Power.Business.IBusinessList HumListBO = HumList.FindAll("FID='" + KeyValue + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (HumListBO.Count > 0)
            {
                Power.Business.IBusinessOperate List = Power.Business.BusinessFactory.CreateBusinessOperate("Human");
                foreach (Business.IBaseBusiness item in HumListBO)
                {
                    Power.Business.IBusinessList ListBO = List.FindAll("Name='" + item["HumanName"] + "' AND DeptName = '" + DeptName + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    item.SetItem("HumanID", ListBO[0]["Id"]);
                    item.SetItem("HumanCode", ListBO[0]["Code"]);
                    item.Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
            return "OK";
        }

        public string SumProfBonus(string ID)
        {
            //更新孙表比例
            XCode.DataAccessLayer.DAL dal0 = XCode.DataAccessLayer.DAL.Create();
            string sql0 = @"update NPS_DES_ProjectOutputGrant_HumList_SubDetail set BonusScale= (Bonus / TotalBonus) * 100 where fid in (

    select id from NPS_DES_ProjectOutputGrant_HumList where fid='" + ID + @"') ";
            dal0.Session.Execute(sql0);

            //更新子表
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql = @"update t1 set  t1.Bonus=t2.Bonus from NPS_DES_ProjectOutputGrant_HumList t1,

                                (select SUM(Bonus)Bonus,FID from NPS_DES_ProjectOutputGrant_HumList_SubDetail

                                where FID IN (select ID from NPS_DES_ProjectOutputGrant_HumList where FID='" + ID + @"')

                                GROUP BY FID) t2

                                where t1.ID=t2.FID";
            dal1.Session.Execute(sql);


            //更新主表
            XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
            string sql2 = @"    update NPS_DES_ProjectOutputGrant set PeriodGrantBonus=(select sum(Bonus) from NPS_DES_ProjectOutputGrant_HumList where FID='" + ID + @"')
    
                        where ID='" + ID + @"'";

            dal2.Session.Execute(sql2);

            return "";
        }


        /*
         获取临时产值分解人员
        */
        public string GetTempHuman(string ID, string Year, string Quarter, string Month, string DeptID)
        {

            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sql = @"select m.HumanCode,m.HumanName,m.HumanID,sum(TempSum)TempSum from NPS_DES_ProfTempOutputCut_ListS m where m.FID in (

            select ID from NPS_DES_ProfTempOutputCut_List x

            where x.FID in (select ID from NPS_DES_ProfTempOutputCut where Status in (35, 50) and Year = '" + Year + @"' and Quarter = '" + Quarter + @"' and Month = '" + Month + @"'

            and DeptID = '" + DeptID + @"')

        ) group by m.HumanCode,m.HumanName,m.HumanID";

            DataTable dt = dal.Session.Query(sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                Power.Business.IBusinessOperate HumList = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");
                Power.Business.IBusinessList HumListBO = HumList.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                HumListBO.Delete();

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    Power.Business.IBaseBusiness sItem = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_HumList");
                    sItem.SetItem("ID", Guid.NewGuid().ToString());
                    sItem.SetItem("FID", ID);
                    sItem.SetItem("HumanName", dt.Rows[x]["HumanName"].ToString());
                    sItem.SetItem("HumanCode", dt.Rows[x]["HumanCode"].ToString());
                    sItem.SetItem("HumanID", dt.Rows[x]["HumanID"].ToString());
                    sItem.SetItem("Bonus", dt.Rows[x]["TempSum"].ToString());
                    sItem.SetItem("ResolveBonus", dt.Rows[x]["TempSum"].ToString());
                    sItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                }
            }
            return "";
        }

        public string CheckProfRate(string KeyValue)
        {
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            //查询孙表，子项比例有没有超100的数据
            sql1 = @"select count(*) from (

                select SubID,sum(BonusScale)BonusScale from NPS_DES_ProjectOutputGrant_HumList_SubDetail where fid in (

                    select id from NPS_DES_ProjectOutputGrant_HumList where fid='" + KeyValue + @"'

                ) group by SubID

            ) M where BonusScale > 100 ";

            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                return "已超";
            }
            return "未超";
        }


        //lx 新增三维产值获取

        public string GetProfBonusV3(string ID)
        {
            DeleteProfBonusV3(ID);
            Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目专业产值支取
            Power.Business.IBusinessList GrantBo = grantBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (GrantBo.Count > 0)
            {

                //sql3 -- 获取人员总产值信息
                XCode.DataAccessLayer.DAL dal3 = XCode.DataAccessLayer.DAL.Create();
                string sql3 = "";

                sql3 = "select Sum(AllocationBonus) as AllocationBonus,DesHumanID,DesHumanName,DesHumanCode from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND IfIssue = '待支取' group by DesHumanID,DesHumanName,DesHumanCode";

                /*三维产值获取子表 人员可支产值汇总--由两个部分拼接而成
                    1.非设计类，完成比例达100%
                    2.设计类，
                */
                /*
                 sql3 = "SELECT Sum(AllocationBonus) as AllocationBonus,DesHumanID,DesHumanName,DesHumanCode FROM (select Sum(AllocationBonus) as AllocationBonus,DesHumanID,DesHumanName,DesHumanCode from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND IfIssue = '待支取' and 1 = (case when Type <> '设计' and PeriodCompleteScale=100 then 1 else 0 end) group by DesHumanID,DesHumanName,DesHumanCode UNION All select Sum(AllocationBonus) as AllocationBonus,DesHumanID,DesHumanName,DesHumanCode from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND IfIssue = '待支取' and 1 = (case when Type = '设计' then 1 else 0 end) group by DesHumanID,DesHumanName,DesHumanCode)AA group by DesHumanID,DesHumanName,DesHumanCode";
                */

                System.Data.DataTable dt3 = dal3.Session.Query(sql3).Tables[0];
                if (dt3.Rows.Count > 0)
                {
                    for (int y = 0; y < dt3.Rows.Count; y++)
                    {
                        string NewID = Guid.NewGuid().ToString();//子表ID
                        Power.Business.IBaseBusiness sItem = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_HumList");
                        sItem.SetItem("ID", NewID); //新ID
                        sItem.SetItem("FID", ID);
                        sItem.SetItem("HumanID", dt3.Rows[y]["DesHumanID"].ToString());//人员ID
                        sItem.SetItem("HumanCode", dt3.Rows[y]["DesHumanCode"].ToString());//人员编号
                        sItem.SetItem("HumanName", dt3.Rows[y]["DesHumanName"].ToString());//人员名称
                        sItem.SetItem("Bonus", dt3.Rows[y]["AllocationBonus"].ToString());//产值
                        sItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
                        string sql2 = "";
                        /*sql2 = "select * from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND DesHumanID='" + dt3.Rows[y]["DesHumanID"] + "' AND IfIssue = '待支取' and 1 = (case when Type <> '设计' and PeriodCompleteScale=100 then 1 else 0 end) UNION ALL select top 1 * from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND DesHumanID='" + dt3.Rows[y]["DesHumanID"] + "' AND IfIssue = '待支取' and 1 = (case when Type = '设计' then 1 else 0 end) order by PeriodCompleteScale desc";*/
                        sql2 = "select * from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND DesHumanID='" + dt3.Rows[y]["DesHumanID"] + "' AND IfIssue = '待支取'";

                        System.Data.DataTable dt2 = dal2.Session.Query(sql2).Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                            for (int x = 0; x < dt2.Rows.Count; x++)
                            {
                                /*
                                Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList_SubDetail");
                                Power.Business.IBusinessList SbList2 = Sb.FindAll(" WBSID = '" + dt2.Rows[x]["WBSID"].ToString() + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                                */


                                Power.Business.IBaseBusiness sItems = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputGrant_HumList_SubDetail");
                                sItems.SetItem("ID", Guid.NewGuid().ToString());
                                sItems.SetItem("FID", NewID);
                                sItems.SetItem("TotalBonus", dt2.Rows[x]["Bonus"]);
                                //  sItems.SetItem("TotalBonus", dt2.Rows[x]["Bonus"]);
                                //  sItems.SetItem("TotalBonus", dt2.Rows[x]["Bonus"]);
                                sItems.SetItem("BonusScale", dt2.Rows[x]["PeriodCompleteScale"]);//本次完成比例
                                sItems.SetItem("SubmitDocuOrderID", dt2.Rows[x]["SubmitDocuOrderID"].ToString());//关联互提资料单ID
                                sItems.SetItem("ProjectID", dt2.Rows[x]["EpsProjId"].ToString());//项目ID
                                sItems.SetItem("ProjectCode", dt2.Rows[x]["EpsProjCode"].ToString());//项目编号
                                sItems.SetItem("ProjectName", dt2.Rows[x]["EpsProjName"].ToString());//项目名称
                                sItems.SetItem("HumanID", dt2.Rows[x]["DesHumanID"].ToString());//人员ID
                                sItems.SetItem("HumanCode", dt2.Rows[x]["DesHumanCode"].ToString());//人员编号
                                sItems.SetItem("HumanName", dt2.Rows[x]["DesHumanName"].ToString());//人员名称
                                sItems.SetItem("Bonus", dt2.Rows[x]["AllocationBonus"].ToString());//产值
                                sItems.SetItem("SubID", dt2.Rows[x]["SubID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["SubID"].ToString());//子项ID
                                sItems.SetItem("SubCode", dt2.Rows[x]["SubCode"].ToString());//子项编号
                                sItems.SetItem("SubName", dt2.Rows[x]["SubName"].ToString());//子项名称
                                sItems.SetItem("TaskID", dt2.Rows[x]["TaskID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["TaskID"].ToString());//任务ID
                                sItems.SetItem("TaskCode", dt2.Rows[x]["TaskCode"].ToString());//任务编号
                                sItems.SetItem("TaskName", dt2.Rows[x]["TaskName"].ToString());//任务名称
                                sItems.SetItem("TaskType", dt2.Rows[x]["TaskType"].ToString());//任务类型
                                sItems.SetItem("Rate", dt2.Rows[x]["Rate"].ToString());//产值比例
                                sItems.SetItem("DesignRate", dt2.Rows[x]["DesignRate"].ToString());//任务比例
                                sItems.SetItem("WBSID", dt2.Rows[x]["WBSID"].ToString());//WBSID
                                sItems.SetItem("IfGrant", dt2.Rows[x]["IfGrant"].ToString());//是否支取
                                sItems.SetItem("Type", dt2.Rows[x]["Type"].ToString());//设校审类型
                                sItems.SetItem("ThreeDHeadHumID", dt2.Rows[x]["ThreeDHeadHumID"].ToString());//专业负责人ID
                                sItems.SetItem("ThreeDHeadHumCode", dt2.Rows[x]["ThreeDHeadHumCode"].ToString());//专业负责人编号
                                sItems.SetItem("ThreeDHeadHumName", dt2.Rows[x]["ThreeDHeadHumName"].ToString());//专业负责人名称
                                sItems.SetItem("ListSID", dt2.Rows[x]["ProfOutputCutLSID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["ProfOutputCutLSID"].ToString());//专业施工图产值分解孙表ID
                                sItems.Save(System.ComponentModel.DataObjectMethodType.Insert);
                            }



                        }

                    }

                }
                CutStatus(ID);


            }
            return "";
        }
        public string CutStatus(string ID)
        {
            try
            {
                Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目专业产值支取
                Power.Business.IBusinessList GrantBo = grantBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (GrantBo.Count > 0)
                {
                    Power.Business.IBusinessOperate grantOneBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");//项目专业产值支取子表1
                    Power.Business.IBusinessList GrantOneBoList = grantOneBo.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (GrantOneBoList.Count > 0)
                    {
                        for (int i = 0; i < GrantOneBoList.Count; i++)
                        {
                            Power.Business.IBusinessOperate grantTwoBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList_SubDetail");//项目专业产值支取子表2
                            Power.Business.IBusinessList GrantTwoBoList = grantTwoBo.FindAll("FID='" + GrantOneBoList[i]["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            if (GrantTwoBoList.Count > 0)
                            {
                                for (int j = 0; j < GrantTwoBoList.Count; j++)
                                {

                                    Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
                                    Power.Business.IBusinessList SbList2 = Sb.FindAll(" ID = '" + GrantTwoBoList[j]["ListSID"] + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                                    if (SbList2.Count > 0)
                                    {
                                        /* 根据三维产值分解表的WBSID，获取完成比例最高的完成审批后互提资料单
                                        XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                                        string sql1 = "";
                                        sql1 = "SELECT top 1 *  FROM NPS_DES_SubmitDocuOrder WHERE WBSID = '" + SbList2[0]["WBSID"] + "' and (Status =35 or Status =50)  order by ThisComScaleN desc";
                                        System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
                                        if (dt1.Rows.Count > 0)
                                        {
                                            if (Convert.ToDouble(dt1.Rows[0]["ThisComScaleN"]) < 100)
                                            {
                                                SbList2[0].SetItem("IfGrant", "部分占用");//项目名称
                                                SbList2[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                                            }
                                            else
                                            {

                                            }
                                        }
                                        */
                                        SbList2[0].SetItem("IfGrant", "占用");//项目名称
                                        SbList2[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                                        //



                                    }

                                }


                            }
                        }

                    }

                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";

        }
        //删除时回退产值状态 new
        public string DeleteProfBonusV3(string ID)
        {
            try
            {
                Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目专业产值支取
                Power.Business.IBusinessList GrantBo = grantBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (GrantBo.Count > 0)
                {
                    Power.Business.IBusinessOperate grantOneBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");//项目专业产值支取子表1
                    Power.Business.IBusinessList GrantOneBoList = grantOneBo.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (GrantOneBoList.Count > 0)
                    {
                        for (int i = 0; i < GrantOneBoList.Count; i++)
                        {
                            Power.Business.IBusinessOperate grantTwoBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList_SubDetail");//项目专业产值支取子表2
                            Power.Business.IBusinessList GrantTwoBoList = grantTwoBo.FindAll("FID='" + GrantOneBoList[i]["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            if (GrantTwoBoList.Count > 0)
                            {
                                for (int j = 0; j < GrantTwoBoList.Count; j++)
                                {
                                    Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
                                    Power.Business.IBusinessList SbList2 = Sb.FindAll(" ID = '" + GrantTwoBoList[j]["ListSID"] + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                                    SbList2[0].SetItem("IfGrant", null);//项目名称
                                    SbList2[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                                    /*
                                    if (SbList2.Count > 0)
                            {
                                当前任务的完成比例大于0，已支取产值大于0
                                if (Convert.ToDouble(SbList2[0]["PayBonusRate"]) > 0 && Convert.ToDouble(SbList2[0]["PayBonus"]) > 0)
                                {
                                    SbList2[0].SetItem("IfGrant", "部分占用");//项目名称
                                    SbList2[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                                }
                                else
                                {
                                    SbList2[0].SetItem("IfGrant", null);//项目名称
                                    SbList2[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                                }
                            }*/


                                }
                                GrantTwoBoList.Delete();

                            }
                        }
                        GrantOneBoList.Delete();
                    }

                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "";
        }

        public string GetDistributeHuman(string ID, string HtmlPath, string OutputType)
        {
            NewLife.Log.XTrace.WriteLine("继承task执行钱。。。");
            if (OutputType == "其他进度、配合产值支取")
            {
                //新建产值支取对象
                Power.Business.IBusinessOperate Grant = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");
                //根据ID找到当前支取主表
                Power.Business.IBusinessList GrantBO = Grant.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (GrantBO.Count > 0)
                {
                    //获取子表对象
                    Power.Business.IBusinessOperate GrantGrid1 = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant_HumList");
                    //找对应子表数据
                    Power.Business.IBusinessList GrantGrid1BO = GrantGrid1.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (GrantGrid1BO.Count > 0)
                    {
                        string ProfCodeArr = "";
                        for (int i = 0; i < GrantGrid1BO.Count; i++)
                        {
                            if (i == GrantGrid1BO.Count - 1)
                            {
                                ProfCodeArr += "'" + GrantGrid1BO[i]["ProfCode"] + "'";
                            }
                            else
                            {
                                ProfCodeArr += "'" + GrantGrid1BO[i]["ProfCode"] + "',";
                            }
                        }
                        //1.根据主表DeptID找到对应部门部长
                        XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                        string sql = "";
                        sql = @"--根据主表DeptID找到对应部门部长
                            select * from pb_human where id in(
                            SELECT HumanId FROM PB_HumanRelation MM WHERE  RelationType = '3'  and ISNULL(MM.RelationId,'00000000-0000-0000-0000-000000000000') in (select ID from PB_Position   where (name = '部长' or name = '所长') and DeptID = '" + GrantBO[0]["DeptID"] + @"' ))
                            ";
                        System.Data.DataTable dt1 = dal1.Session.Query(sql).Tables[0];
                        if (dt1.Rows.Count > 0)
                        {
                            Power.Business.IBaseBusiness PBDistributeList2 = Power.Business.BusinessFactory.CreateBusiness("Distribute");
                            PBDistributeList2.SetItem("ID", Guid.NewGuid().ToString());
                            PBDistributeList2.SetItem("KeyWord", "NPS_DES_ProjectOutputGrant");
                            PBDistributeList2.SetItem("KeyValue", ID);
                            PBDistributeList2.SetItem("HtmlPath", HtmlPath);
                            PBDistributeList2.SetItem("HumanId", dt1.Rows[0]["Id"]);
                            PBDistributeList2.SetItem("HumanCode", dt1.Rows[0]["Code"]);
                            PBDistributeList2.SetItem("HumanName", dt1.Rows[0]["Name"]);
                            PBDistributeList2.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }
                        //2.根据子表专业插入专业负责人
                        XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
                        string sql2 = "";
                        sql2 = @"--根据子表专业插入专业负责人
                            select * from NPS_DES_ProfHead_List A left join  NPS_DES_ProfHead B on A.fid = b.id
                            where B.EpsProjCode='" + GrantBO[0]["ProjectCode"] + @"' and A.ProfCode in (" + ProfCodeArr + @")
                            ";
                        System.Data.DataTable dt2 = dal2.Session.Query(sql2).Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                            for (int x = 0; x < dt2.Rows.Count; x++)
                            {
                                Power.Business.IBaseBusiness PBDistributeList3 = Power.Business.BusinessFactory.CreateBusiness("Distribute");
                                PBDistributeList3.SetItem("ID", Guid.NewGuid().ToString());
                                PBDistributeList3.SetItem("KeyWord", "NPS_DES_ProjectOutputGrant");
                                PBDistributeList3.SetItem("KeyValue", ID);
                                PBDistributeList3.SetItem("HtmlPath", HtmlPath);
                                PBDistributeList3.SetItem("HumanId", dt2.Rows[x]["HeadHumID"]);
                                PBDistributeList3.SetItem("HumanCode", dt2.Rows[x]["HeadHumCode"]);
                                PBDistributeList3.SetItem("HumanName", dt2.Rows[x]["HeadHumName"]);
                                PBDistributeList3.Save(System.ComponentModel.DataObjectMethodType.Insert);
                            }
                        }
                        //3.分发子表设计人员
                        for (int i = 0; i < GrantGrid1BO.Count; i++)
                        {
                            Power.Business.IBaseBusiness PBDistributeList3 = Power.Business.BusinessFactory.CreateBusiness("Distribute");
                            PBDistributeList3.SetItem("ID", Guid.NewGuid().ToString());
                            PBDistributeList3.SetItem("KeyWord", "NPS_DES_ProjectOutputGrant");
                            PBDistributeList3.SetItem("KeyValue", ID);
                            PBDistributeList3.SetItem("HtmlPath", HtmlPath);
                            PBDistributeList3.SetItem("HumanId", GrantGrid1BO[i]["HumanId"]);
                            PBDistributeList3.SetItem("HumanCode", GrantGrid1BO[i]["HumanCode"]);
                            PBDistributeList3.SetItem("HumanName", GrantGrid1BO[i]["HumanName"]);
                            PBDistributeList3.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }
                    }
                }
            }
            return "";
        }

        //三维产值审批后将对应子项已支取产值回写到对应任务分解表上
        public string ReWriteBIMFinishBouns(string ID)
        {
            //根据三维产值分解表的WBSID，获取完成比例最高的完成审批后互提资料单
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            sql1 = "select max(Bonus) Bonus,max(PeriodCompleteScale) PeriodCompleteScale,WBSID from NPS_DES_ProjectOutputGrant_HumList_SubDetail where fid in ( select ID from NPS_DES_ProjectOutputGrant_HumList where FID ='" + ID + "') group by WBSID";
            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int x = 0; x < dt1.Rows.Count; x++)
                {
                    Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
                    Power.Business.IBusinessList SbList2 = Sb.FindAll(" WBSID = '" + dt1.Rows[x]["WBSID"].ToString() + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (SbList2.Count > 0)
                    {
                        for (int j = 0; j < SbList2.Count; j++)
                        {
                            SbList2[j]["PayBouns"] = dt1.Rows[x]["Bonus"];//回写支取时产值
                            SbList2[j]["PayBounsRate"] = dt1.Rows[x]["PeriodCompleteScale"];//回写支取时比例
                            SbList2[j].Save(System.ComponentModel.DataObjectMethodType.Update);
                        }
                    }
                }

            }
            return "";
        }

        //将项目专业产值支取模块中的本次支取产值和本次支取比例根据wbsid更新到对应专业施工图产值分解
        public string UpCZFJBouns(string ID)
        {
            //根据项目专业产值支取的id找到项目人员分解明细的明细记录，根据明细记录中的wbsid找到对应专业施工图产值分解
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            sql1 = "select sum(Bonus) as Bonus,WBSID from NPS_DES_ProjectOutputGrant_EpsList where fid='" + ID + "'  group by WBSID ";
            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
            for (int x = 0; x < dt1.Rows.Count; x++)//这是每一个wbsid的本次支取产值的和
            {
                NewLife.Log.XTrace.WriteLine("是否执行此方法1");
                Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS");
                Power.Business.IBusinessList SbList2 = Sb.FindAll(" WBSID = '" + dt1.Rows[x]["WBSID"].ToString() + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);//根据wbsid找到专业施工图产值分解累加到已支取产值中
                if (SbList2.Count > 0)
                {
                    NewLife.Log.XTrace.WriteLine("是否执行此方法2");
                    for (int j = 0; j < SbList2.Count; j++)
                    {
                        string PayBonus = (double.Parse(SbList2[j]["PayBonus"].ToString()) + double.Parse(dt1.Rows[x]["Bonus"].ToString())).ToString();
                        SbList2[j]["PayBonus"] = double.Parse(SbList2[j]["PayBonus"].ToString()) + double.Parse(dt1.Rows[x]["Bonus"].ToString());//回写支取时产值
                        if (double.Parse(SbList2[j]["Bonus"].ToString()) < double.Parse(SbList2[j]["PayBonus"].ToString()))
                        {
                            SbList2[j]["StatusCode"] = "超额支取";
                            sql1 = "";
                            sql1 = "update NPS_DES_ProfOutputCut_ListS set StatusCode = '超额支取' where WBSID='" + dt1.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }
                        sql1 = "";
                        sql1 = "update NPS_DES_ProfOutputCut_ListS set PayBonus = '" + PayBonus + "' where WBSID='" + dt1.Rows[x]["WBSID"].ToString() + "' ";
                        dal1.Session.Execute(sql1);
                        NewLife.Log.XTrace.WriteLine("是否执行此方法3");
                    }
                }
            }

            //更新专业施工图产值分解中的支取时完成比例字段
            sql1 = "";
            sql1 = "select sum(convert(float,ThisApplyScale)) as ThisApplyScale,WBSID from NPS_DES_ProjectOutputGrant_EpsList where fid='" + ID + "' group by WBSID,Type having Type='设计' ";
            System.Data.DataTable dt2 = dal1.Session.Query(sql1).Tables[0];
            for (int x = 0; x < dt2.Rows.Count; x++)//这是每一个wbsid的本次支取产值的和
            {
                NewLife.Log.XTrace.WriteLine("是否执行此方法4");
                Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS");
                Power.Business.IBusinessList SbList2 = Sb.FindAll(" WBSID = '" + dt2.Rows[x]["WBSID"].ToString() + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);//根据wbsid找到专业施工图产值分解累加到已支取产值中
                if (SbList2.Count > 0)
                {
                    for (int j = 0; j < SbList2.Count; j++)
                    {
                        string PayBonusRate = (double.Parse(SbList2[j]["PayBonusRate"].ToString()) + double.Parse(dt2.Rows[x]["ThisApplyScale"].ToString())).ToString();
                        SbList2[j]["PayBonusRate"] = double.Parse(SbList2[j]["PayBonusRate"].ToString()) + double.Parse(dt2.Rows[x]["ThisApplyScale"].ToString());//回写支取时产值
                        if (double.Parse(SbList2[j]["PayBonusRate"].ToString()) > 100)
                        {
                            SbList2[j]["StatusCode"] = "超额支取";
                            sql1 = "";
                            sql1 = "update NPS_DES_ProfOutputCut_ListS set StatusCode = '超额支取' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }
                        else if (double.Parse(SbList2[j]["PayBonusRate"].ToString()) == 100)
                        {
                            SbList2[j]["IfGrant"] = "占用";
                            sql1 = "";
                            sql1 = "update NPS_DES_ProfOutputCut_ListS set IfGrant = '占用' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }

                        sql1 = "";
                        sql1 = "update NPS_DES_ProfOutputCut_ListS set PayBonusRate = '" + PayBonusRate + "' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                        dal1.Session.Execute(sql1);
                        //SbList2[j].Save(System.ComponentModel.DataObjectMethodType.Update);
                        NewLife.Log.XTrace.WriteLine("是否执行此方法5");
                    }
                }
            }
            return "";
        }
    }
}
