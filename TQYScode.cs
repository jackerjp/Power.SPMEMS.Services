using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power.SPMEMS.Services
{
    class TQYScode
    {

        //将项目专业产值支取模块中的本次支取产值和本次支取比例根据wbsid更新到对应专业施工图产值分解
        public string UpThreeCZFJBouns(string ID)
        {
            //根据项目专业产值支取的id找到项目人员分解明细的明细记录，根据明细记录中的wbsid找到对应专业施工图产值分解
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            sql1 = "select sum(Bonus) as Bonus,WBSID from NPS_DES_ProjectOutputGrant_HumList_SubDetail where fid='" + ID + "'  group by WBSID ";
            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
            for (int x = 0; x < dt1.Rows.Count; x++)//这是每一个wbsid的本次支取产值的和
            {
                NewLife.Log.XTrace.WriteLine("是否执行此方法1");
                Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
                Power.Business.IBusinessList SbList2 = Sb.FindAll(" WBSID = '" + dt1.Rows[x]["WBSID"].ToString() + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);//根据wbsid找到专业施工图产值分解累加到已支取产值中
                if (SbList2.Count > 0)
                {
                    NewLife.Log.XTrace.WriteLine("是否执行此方法2");
                    for (int j = 0; j < SbList2.Count; j++)
                    {
                        double dPayBonus = 0, dBonus = 0;
                        string StatusCode = "";
                        if (SbList2[j]["StatusCode"] == null)
                            StatusCode = "";
                        else
                            StatusCode = SbList2[j]["StatusCode"].ToString();
                        if (SbList2[j]["PayBonus"] == null)
                            dPayBonus = 0;
                        else
                            dPayBonus = double.Parse(SbList2[j]["PayBonus"].ToString());
                        if (dt1.Rows[x]["Bonus"] == null)
                            dBonus = 0;
                        else
                            dBonus = double.Parse(dt1.Rows[x]["Bonus"].ToString());

                        string PayBonus = (dPayBonus + dBonus).ToString();
                        //SbList2[j]["PayBonus"] = double.Parse(SbList2[j]["PayBonus"].ToString()) + double.Parse(dt1.Rows[x]["Bonus"].ToString());//回写支取时产值
                        if (dBonus < dPayBonus)
                        {
                            SbList2[j]["StatusCode"] = "超额支取";
                            sql1 = "";
                            sql1 = "update NPS_DES_ThreeDimensional_ListS set StatusCode = '超额支取' where WBSID='" + dt1.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }
                        sql1 = "";
                        sql1 = "update NPS_DES_ThreeDimensional_ListS set PayBonus = '" + PayBonus + "' where WBSID='" + dt1.Rows[x]["WBSID"].ToString() + "' ";
                        dal1.Session.Execute(sql1);
                        NewLife.Log.XTrace.WriteLine("是否执行此方法3");
                    }
                }
            }

            //更新专业施工图产值分解中的支取时完成比例字段
            sql1 = "";
            sql1 = "select sum(convert(float,BonusScale)) as ThisApplyScale,WBSID from NPS_DES_ProjectOutputGrant_HumList_SubDetail where fid='" + ID + "' and isnull(remark,'') ='' group by WBSID,Type having Type='设计' ";
            System.Data.DataTable dt2 = dal1.Session.Query(sql1).Tables[0];
            for (int x = 0; x < dt2.Rows.Count; x++)//这是每一个wbsid的本次支取产值的和
            {
                NewLife.Log.XTrace.WriteLine("是否执行此方法4");
                Power.Business.IBusinessOperate Sb = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
                Power.Business.IBusinessList SbList2 = Sb.FindAll(" WBSID = '" + dt2.Rows[x]["WBSID"].ToString() + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);//根据wbsid找到专业施工图产值分解累加到已支取产值中
                if (SbList2.Count > 0)
                {
                    for (int j = 0; j < SbList2.Count; j++)
                    {
                        double dPayBonusRate = 0, dThisApplyScale = 0;
                        if (SbList2[j]["PayBonusRate"] == null)
                            dPayBonusRate = 0;
                        else
                            dPayBonusRate = double.Parse(SbList2[j]["PayBonusRate"].ToString());
                        if (dt2.Rows[x]["ThisApplyScale"] == null)
                            dThisApplyScale = 0;
                        else
                            dThisApplyScale = double.Parse(dt2.Rows[x]["ThisApplyScale"].ToString());

                        string PayBonusRate = (dPayBonusRate + dThisApplyScale).ToString();
                        //SbList2[j]["PayBonusRate"] = double.Parse(SbList2[j]["PayBonusRate"].ToString()) + double.Parse(dt2.Rows[x]["ThisApplyScale"].ToString());//回写支取时产值
                        if (dPayBonusRate > 100)
                        {
                            SbList2[j]["StatusCode"] = "超额支取";
                            sql1 = "";
                            sql1 = "update NPS_DES_ThreeDimensional_ListS set StatusCode = '超额支取' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }
                        else if (dPayBonusRate == 100)
                        {
                            SbList2[j]["IfGrant"] = "占用";
                            sql1 = "";
                            sql1 = "update NPS_DES_ThreeDimensional_ListS set IfGrant = '占用' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }

                        sql1 = "";
                        sql1 = "update NPS_DES_ThreeDimensional_ListS set PayBonusRate = '" + PayBonusRate + "' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                        dal1.Session.Execute(sql1);
                        //SbList2[j].Save(System.ComponentModel.DataObjectMethodType.Update);
                        NewLife.Log.XTrace.WriteLine("是否执行此方法5");
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
            sql1 = "select max(Bonus) Bonus,max(PeriodCompleteScale) PeriodCompleteScale,WBSID from NPS_DES_ProjectOutputGrant_HumList_SubDetail where " +
                   "fid in ( select ID from NPS_DES_ProjectOutputGrant_HumList where FID ='" + ID + "') group by WBSID";
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
                        double dPayBonus = 0, dBonus = 0;
                        string StatusCode = "";
                        if (SbList2[j]["StatusCode"] == null)
                            StatusCode = "";
                        else
                            StatusCode = SbList2[j]["StatusCode"].ToString();
                        if (SbList2[j]["PayBonus"] == null)
                            dPayBonus = 0;
                        else
                            dPayBonus = double.Parse(SbList2[j]["PayBonus"].ToString());
                        if (dt1.Rows[x]["Bonus"] == null)
                            dBonus = 0;
                        else
                            dBonus = double.Parse(dt1.Rows[x]["Bonus"].ToString());
                        string PayBonus = (dPayBonus+ dBonus).ToString();
                        //SbList2[j]["PayBonus"] = double.Parse(SbList2[j]["PayBonus"].ToString()) + double.Parse(dt1.Rows[x]["Bonus"].ToString());//回写支取时产值
                        NewLife.Log.XTrace.WriteLine("是否执行此方法6"+ StatusCode);
                        if (StatusCode.Trim().Equals("正在支取") || StatusCode.Trim().Equals("正在增补"))
                        {
                            sql1 = "";
                            sql1 = "update NPS_DES_ProfOutputCut_ListS set StatusCode = '' where WBSID='" + dt1.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }
                        else if (dBonus < dPayBonus)
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
            sql1 = "select sum(convert(float,ThisApplyScale)) as ThisApplyScale,WBSID from NPS_DES_ProjectOutputGrant_EpsList where fid='" + ID + "'  and isnull(remark,'') ='' group by WBSID,Type having Type='设计' ";
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
                        double dPayBonusRate = 0, dThisApplyScale = 0;
                        if (SbList2[j]["PayBonusRate"] == null)
                            dPayBonusRate = 0;
                        else
                            dPayBonusRate = double.Parse(SbList2[j]["PayBonusRate"].ToString());
                        if (dt2.Rows[x]["ThisApplyScale"] == null)
                            dThisApplyScale = 0;
                        else
                            dThisApplyScale = double.Parse(dt2.Rows[x]["ThisApplyScale"].ToString());

                        string PayBonusRate = (dPayBonusRate + dThisApplyScale).ToString();
                        //SbList2[j]["PayBonusRate"] = double.Parse(SbList2[j]["PayBonusRate"].ToString()) + double.Parse(dt2.Rows[x]["ThisApplyScale"].ToString());//回写支取时产值
                        if (dPayBonusRate > 100)
                        {
                            SbList2[j]["StatusCode"] = "超额支取";
                            sql1 = "";
                            sql1 = "update NPS_DES_ProfOutputCut_ListS set StatusCode = '超额支取' where WBSID='" + dt2.Rows[x]["WBSID"].ToString() + "' ";
                            dal1.Session.Execute(sql1);
                        }
                        else if (dPayBonusRate == 100)
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

        //lx V3
        public void backOverwriteProfOutCutV3(string FID, string originFID)
        {
            Power.Business.IBusinessOperate adjustBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust");
            Power.Business.IBusinessOperate adjustListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust_List");
            Power.Business.IBusinessOperate adjustListSBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust_ListS");
            Power.Business.IBusinessOperate adjustListDesListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust_ListS_DesList");
            Power.Business.IBusinessOperate originBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputApply");
            Power.Business.IBusinessOperate originListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputApply_ThreeDimensional");
            Power.Business.IBusinessOperate originListSBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
            Power.Business.IBusinessOperate originListDesListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS_DesList");

            Power.Business.IBusinessList submitListList = originListBO.FindAll(" 1=0 ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            Power.Business.IBusinessList submitListSList = originListSBO.FindAll(" 1=0 ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            Power.Business.IBusinessList submitListDesListList = originListDesListBO.FindAll(" 1=0 ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);



            //根据 调整表 重写 产值分解表
            //子表
            Power.Business.IBusinessList adjustListList = adjustListBO.FindAll("FID", FID, Power.Business.SearchFlag.IgnoreRight);


            double BonusSum = 0;
            foreach (var listItem in adjustListList)
            {
                BonusSum += Convert.ToDouble(listItem["Bonus"]);
                Power.Business.IBaseBusiness originList = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProjectOutputApply_ThreeDimensional");
                foreach (KeyValuePair<string, Power.Business.BusinessProperty> businessProperty in originListBO.EntityPropertyList)
                {
                    originList.SetItem(businessProperty.Value.PropertyName, listItem[businessProperty.Value.PropertyName]);
                }
                if (!(Convert.ToString(listItem["ProfListID"]) == "00000000-0000-0000-0000-000000000000"))
                {
                    originList.SetItem("ID", listItem["ProfListID"]);
                }
                originList.SetItem("FID", originFID);
                string listSFID = Convert.ToString(originList["ID"]);
                //孙表
                Power.Business.IBusinessList adjustListSList = adjustListSBO.FindAll("FID", Convert.ToString(listItem["ID"]));
                //支取检测
                //Power.Business.IBusinessList grantChildrenChangeCount = originListSBO.FindCount("ID='" + Convert.ToString(listSItem["TaskListID"]) + "' and IfGrant='" + listSItem["IfGrant"] + "'")
                XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                string sql = "";
                sql = @"--查询调整孙表的IfGrant和原子表是否有出入
                     select COUNT(*)
                     from NPS_DES_ThreeDimensional_ListS C
                     inner JOIN NPS_DES_ProfOutputCutAdjust_ListS P
                     ON P.TaskListID = C.ID and ISNULL(P.IfGrant,' ')!=ISNULL(C.IfGrant,' ')
                     where P.FID='" + Convert.ToString(listItem["ID"]) + "'";
                System.Data.DataTable dt1 = dal1.Session.Query(sql).Tables[0];
                if (Convert.ToInt32(dt1.Rows[0][0]) > 0)
                {
                    var objMain = adjustBO.FindByKey(FID);
                    objMain.SetItem("IfAdjust", "否");
                    objMain.Save(System.ComponentModel.DataObjectMethodType.Update);
                    return;
                    //throw new Exception("当前产值分解在调整期间发生支取，调整失败！");
                }

                foreach (var listSItem in adjustListSList)
                {
                    Power.Business.IBaseBusiness originListS = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ThreeDimensional_ListS");
                    foreach (KeyValuePair<string, Power.Business.BusinessProperty> businessProperty in originListSBO.EntityPropertyList)
                    {
                        originListS.SetItem(businessProperty.Value.PropertyName, listSItem[businessProperty.Value.PropertyName]);
                    }
                    if (!(Convert.ToString(listSItem["TaskListID"]) == "00000000-0000-0000-0000-000000000000"))
                    {
                        originListS.SetItem("ID", listSItem["TaskListID"]);
                    }
                    NewLife.Log.XTrace.WriteLine("二维产值调整修改分配产值");
                    string adjustTaskBonus = Convert.ToString(Convert.ToDouble(listItem["Bonus"]) * Convert.ToDouble(listSItem["Rate"]) / 100);//修改后分配产值=修改后车间产值*修改后车间比例
                    originListS.SetItem("Bonus", adjustTaskBonus);
                    originListS.SetItem("FID", listSFID);
                    string desListFID = Convert.ToString(originListS["ID"]);
                    Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ThreeDimensional_ListS");
                    Power.Business.IBusinessList GrantBo = grantBo.FindAll("WBSID='" + listSItem["WBSID"].ToString() + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (GrantBo.Count > 0)
                    {
                        NewLife.Log.XTrace.WriteLine("值等于=" + adjustTaskBonus);
                        NewLife.Log.XTrace.WriteLine("值等于=" + GrantBo[0]["Bonus"].ToString());
                        double PayBonus = 0, PayBonusRate = 0;
                        if (GrantBo[0]["PayBonus"] == null)
                        {
                            PayBonus = 0;
                        }
                        else
                            PayBonus = double.Parse(GrantBo[0]["PayBonus"].ToString());
                        if (GrantBo[0]["PayBonusRate"] == null)
                        {
                            PayBonusRate = 0;
                        }
                        else
                            PayBonusRate = double.Parse(GrantBo[0]["PayBonusRate"].ToString());

                        if ((!adjustTaskBonus.Equals(GrantBo[0]["Bonus"].ToString())) && (PayBonus != 0 && PayBonusRate != 0))//判断值不一样
                        {
                            originListS.SetItem("StatusCode", "待增补");
                        }
                    }
                    //孙孙表
                    Power.Business.IBusinessList adjustListDesListList = adjustListDesListBO.FindAll("FID", Convert.ToString(listSItem["ID"]));
                    foreach (var listDesListItem in adjustListDesListList)
                    {
                        Power.Business.IBaseBusiness originListDesList = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ThreeDimensional_ListS_DesList");
                        foreach (KeyValuePair<string, Power.Business.BusinessProperty> businessProperty in originListDesListBO.EntityPropertyList)
                        {
                            originListDesList.SetItem(businessProperty.Value.PropertyName, listDesListItem[businessProperty.Value.PropertyName]);
                        }
                        if (!(Convert.ToString(listDesListItem["DesListID"]) == "00000000-0000-0000-0000-000000000000"))
                        {
                            originListDesList.SetItem("ID", listDesListItem["DesListID"]);
                        }
                        originListDesList.SetItem("FID", desListFID);
                        //originListDesList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        submitListDesListList.Add(originListDesList);
                    }
                    //originListS.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    submitListSList.Add(originListS);
                }
                //originList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                submitListList.Add(originList);
            }


            //删除原有的子、孙、孙孙表数据
            //delete(originFID);
            //子表
            Power.Business.IBusinessList originListList = originListBO.FindAll("FID", originFID, Power.Business.SearchFlag.IgnoreRight);

            foreach (var listItem in originListList)
            {
                //孙表
                Power.Business.IBusinessList originListSList = originListSBO.FindAll("FID", Convert.ToString(listItem["ID"]));
                foreach (var listSItem in originListSList)
                {
                    NewLife.Log.XTrace.WriteLine("继承task执行钱。。。");
                    //孙孙表
                    Power.Business.IBusinessList originListDesListList = originListDesListBO.FindAll("FID", Convert.ToString(listSItem["ID"]));
                    originListDesListList.Delete();
                }
                originListSList.Delete();
            }
            originListList.Delete();



            //保存新数据
            submitListList.Save(true);
            submitListSList.Save(true);
            submitListDesListList.Save(true);
            //更新原有的分配产值（主表）
            Power.Business.IBaseBusiness baseBusiness = originBO.FindByKey(originFID, Power.Business.SearchFlag.IgnoreRight);

            baseBusiness.SetItem("PeriodApplyBonus", BonusSum);
            baseBusiness.SetItem("PeriodReplyBonus", BonusSum);


            baseBusiness.Save(System.ComponentModel.DataObjectMethodType.Update);

        }

        /*
第一次保存时自动插入专业施工图产值分解明细
胡宁绘
*/

        public string GetDataOutPut(string ID, string EpsProjId, string ProfID)
        {
            // 根据项目和专业 找到专业施工图产值分解
            Power.Business.IBusinessOperate outputBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut");
            Power.Business.IBusinessList OutputBo = outputBo.FindAll("ProjectID='" + EpsProjId + "' AND ProfID = '" + ProfID + "' AND Status = 50", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (OutputBo.Count > 0)
            {
                // 根据专业施工图产值分解 找到专业施工图产值分解子表
                Power.Business.IBusinessOperate outputListBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_List");
                Power.Business.IBusinessList OutputListBo = outputListBo.FindAll("FID='" + OutputBo[0]["ID"] + "'", "Sequ", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (OutputListBo.Count > 0)
                {
                    for (int i = 0; i < OutputListBo.Count; i++)
                    {
                        string ListID = Guid.NewGuid().ToString();
                        // 找到专业施工图产值分解子表之后 对数据处理进入循环 插入到专业施工图产值分解调整子表中
                        Power.Business.IBaseBusiness Listbus = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProfOutputCutScaleAdjust_List");//专业产值分解调整子表
                        Listbus.SetItem("ID", ListID);
                        Listbus.SetItem("FID", ID);
                        Listbus.SetItem("SubID", OutputListBo[i]["SubID"]);//子项ID
                        Listbus.SetItem("SubCode", OutputListBo[i]["SubCode"]);//子项编号
                        Listbus.SetItem("SubName", OutputListBo[i]["SubName"]);//子项名称
                        Listbus.SetItem("Bonus", OutputListBo[i]["Bonus"]);//分配产值
                        Listbus.SetItem("ProfListID", OutputListBo[i]["ID"]);//分解子项ID（子表ID）
                        Listbus.SetItem("IfAdjustSub", "是");
                        Listbus.Save(System.ComponentModel.DataObjectMethodType.Insert);

                        // 插入专业施工图产值分解子表之后 查找对应专业施工图产值分解孙表数据
                        Power.Business.IBusinessOperate outputListSBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS");
                        Power.Business.IBusinessList OutputListSBo = outputListSBo.FindAll("FID='" + OutputListBo[i]["ID"] + "'", "Sequ", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        if (OutputListSBo.Count > 0)
                        {
                            for (int x = 0; x < OutputListSBo.Count; x++)
                            {
                                string ListSID = Guid.NewGuid().ToString();
                                // 找到专业施工图产值分解孙表数据 进行循环处理 插入到专业施工图产值分解调整孙表中
                                Power.Business.IBaseBusiness ListSbus = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProfOutputCutScaleAdjust_ListS");
                                ListSbus.SetItem("ID", ListSID);
                                ListSbus.SetItem("FID", ListID);
                                ListSbus.SetItem("PayBonus", OutputListSBo[x]["PayBonus"]);
                                ListSbus.SetItem("PayBonusRate", OutputListSBo[x]["PayBonusRate"]);
                                ListSbus.SetItem("TaskID", OutputListSBo[x]["TaskID"]); // 任务ID
                                ListSbus.SetItem("TaskCode", OutputListSBo[x]["TaskCode"]); // 任务编号
                                ListSbus.SetItem("TaskName", OutputListSBo[x]["TaskName"]); // 任务名称
                                ListSbus.SetItem("Rate", OutputListSBo[x]["Rate"]); // 分配比例
                                ListSbus.SetItem("DesType", OutputListSBo[x]["DesType"]); // 类型(设计人)
                                ListSbus.SetItem("DesignRate", OutputListSBo[x]["DesignRate"]); // 设计
                                ListSbus.SetItem("CheckRate", OutputListSBo[x]["CheckRate"]); // 校对
                                ListSbus.SetItem("ReviewRate", OutputListSBo[x]["ReviewRate"]); // 审核
                                ListSbus.SetItem("ApproveRate", OutputListSBo[x]["ApproveRate"]); // 审定
                                ListSbus.SetItem("Bonus", OutputListSBo[x]["Bonus"]); // 分配产值
                                ListSbus.SetItem("TaskListID", OutputListSBo[x]["ID"]); // 分解任务ID（孙表ID）
                                ListSbus.SetItem("IfGrant", OutputListSBo[x]["IfGrant"]); // 是否支取
                                ListSbus.SetItem("WBSID", OutputListSBo[x]["WBSID"]); // WBSID
                                ListSbus.SetItem("OriginRate", OutputListSBo[x]["OriginRate"]); // 是否支取
                                ListSbus.SetItem("BaseTaskID", OutputListSBo[x]["BaseTaskID"]); // 基础数据ID
                                ListSbus.Save(System.ComponentModel.DataObjectMethodType.Insert);

                                // 插入专业施工图产值分解孙表之后 查找对应专业施工图产值分解孙孙表数据
                                Power.Business.IBusinessOperate outputListSDesBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS_DesList");
                                Power.Business.IBusinessList OutputListSDesBo = outputListSDesBo.FindAll("FID='" + OutputListSBo[x]["ID"] + "'", "Sequ", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                                if (OutputListSDesBo.Count > 0)
                                {
                                    for (int y = 0; y < OutputListSDesBo.Count; y++)
                                    {
                                        // 找到专业施工图产值分解孙孙表数据 进行循环处理 插入到专业施工图产值分解调整孙孙表中
                                        Power.Business.IBaseBusiness ySubList = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProfOutputCutScaleAdjust_ListS_DesList");
                                        ySubList.SetItem("ID", Guid.NewGuid().ToString());
                                        ySubList.SetItem("FID", ListSID);
                                        ySubList.SetItem("DesHumanID", OutputListSDesBo[y]["DesHumanID"]); // 设计人员ID
                                        ySubList.SetItem("DesHumanCode", OutputListSDesBo[y]["DesHumanCode"]); // 设计人员编号
                                        ySubList.SetItem("DesHumanName", OutputListSDesBo[y]["DesHumanName"]); // 设计人员名称
                                        ySubList.SetItem("AllocationBonus", OutputListSDesBo[y]["AllocationBonus"]); // 分配产值
                                        ySubList.SetItem("IfMore", OutputListSDesBo[y]["IfMore"]); // 是否多设计人
                                        ySubList.SetItem("DesBonus", OutputListSDesBo[y]["DesBonus"]); // 设计人员分配总产值
                                        ySubList.SetItem("DesListID", OutputListSDesBo[y]["ID"]); // 分解设计产值ID（孙孙表ID）
                                        ySubList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}
            return "";
        }

        /// <summary>
        /// by gzz 20191213
        /// </summary>
        /// <param name="FID">调整表主表ID</param>
        /// <param name="originFID">原来的表的主表ID</param>
        public void backOverwriteProfOutCut(string FID, string originFID)
        {
            Power.Business.IBusinessOperate adjustBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust");
            Power.Business.IBusinessOperate adjustListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust_List");
            Power.Business.IBusinessOperate adjustListSBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust_ListS");
            Power.Business.IBusinessOperate adjustListDesListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCutAdjust_ListS_DesList");
            Power.Business.IBusinessOperate originBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut");
            Power.Business.IBusinessOperate originListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_List");
            Power.Business.IBusinessOperate originListSBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS");
            Power.Business.IBusinessOperate originListDesListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS_DesList");

            Power.Business.IBusinessList submitListList = originListBO.FindAll(" 1=0 ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            Power.Business.IBusinessList submitListSList = originListSBO.FindAll(" 1=0 ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            Power.Business.IBusinessList submitListDesListList = originListDesListBO.FindAll(" 1=0 ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);



            //根据 调整表 重写 产值分解表
            //子表
            Power.Business.IBusinessList adjustListList = adjustListBO.FindAll("FID", FID, Power.Business.SearchFlag.IgnoreRight);


            double BonusSum = 0;
            if (adjustListList.Count == 0)
            {
                NewLife.Log.XTrace.WriteLine("调整审批后事件--根据调整表主表ID未找到对应子项，主表ID为：" + FID);
            }
            foreach (var listItem in adjustListList)
            {
                NewLife.Log.XTrace.WriteLine("调整审批后事件--调整子表循环");
                BonusSum += Convert.ToDouble(listItem["Bonus"]);
                Power.Business.IBaseBusiness originList = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProfOutputCut_List");
                foreach (KeyValuePair<string, Power.Business.BusinessProperty> businessProperty in originListBO.EntityPropertyList)
                {
                    NewLife.Log.XTrace.WriteLine("调整审批后事件--写入子表循环");
                    originList.SetItem(businessProperty.Value.PropertyName, listItem[businessProperty.Value.PropertyName]);
                }
                if (!(Convert.ToString(listItem["ProfListID"]) == "00000000-0000-0000-0000-000000000000"))
                {
                    originList.SetItem("ID", listItem["ProfListID"]);
                }
                originList.SetItem("FID", originFID);
                string listSFID = Convert.ToString(originList["ID"]);
                //孙表
                Power.Business.IBusinessList adjustListSList = adjustListSBO.FindAll("FID", Convert.ToString(listItem["ID"]));
                //支取检测
                //Power.Business.IBusinessList grantChildrenChangeCount = originListSBO.FindCount("ID='" + Convert.ToString(listSItem["TaskListID"]) + "' and IfGrant='" + listSItem["IfGrant"] + "'")
                XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                string sql = "";
                sql = @"--查询调整孙表的IfGrant和原子表是否有出入
                     select COUNT(*)
                     from NPS_DES_ProfOutputCut_ListS C
                     inner JOIN NPS_DES_ProfOutputCutAdjust_ListS P
                     ON P.TaskListID = C.ID and ISNULL(P.IfGrant,' ')!=ISNULL(C.IfGrant,' ')
                     where P.FID='" + Convert.ToString(listItem["ID"]) + "'";
                System.Data.DataTable dt1 = dal1.Session.Query(sql).Tables[0];

                if (Convert.ToInt32(dt1.Rows[0][0]) > 0)
                {
                    NewLife.Log.XTrace.WriteLine("调整审批后事件--查询调整孙表的IfGrant和原子表是否有出入 -- 跳出循环");
                    var objMain = adjustBO.FindByKey(FID);
                    objMain.SetItem("IfAdjust", "否");
                    objMain.Save(System.ComponentModel.DataObjectMethodType.Update);
                    return;
                    //throw new Exception("当前产值分解在调整期间发生支取，调整失败！");
                }
                else
                {
                    NewLife.Log.XTrace.WriteLine("调整审批后事件--查询调整孙表的IfGrant和原子表是否有出入 -- 继续执行循环");
                }

                if (adjustListSList.Count == 0)
                {
                    NewLife.Log.XTrace.WriteLine("调整审批后事件--根据调整表子项ID未找到对应任务，子项ID为：" + Convert.ToString(listItem["ID"]));
                }
                foreach (var listSItem in adjustListSList)
                {
                    NewLife.Log.XTrace.WriteLine("调整审批后事件--调整孙表循环");
                    Power.Business.IBaseBusiness originListS = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProfOutputCut_ListS");

                    

                    foreach (KeyValuePair<string, Power.Business.BusinessProperty> businessProperty in originListSBO.EntityPropertyList)
                    {
                        NewLife.Log.XTrace.WriteLine("调整审批后事件--写入孙表循环");
                        originListS.SetItem(businessProperty.Value.PropertyName, listSItem[businessProperty.Value.PropertyName]);
                    }
                    if (!(Convert.ToString(listSItem["TaskListID"]) == "00000000-0000-0000-0000-000000000000"))
                    {
                        originListS.SetItem("ID", listSItem["TaskListID"]);
                    }
                    NewLife.Log.XTrace.WriteLine("二维产值调整修改分配产值");                   


                    string adjustTaskBonus = Convert.ToString(Convert.ToDouble(listItem["Bonus"]) * Convert.ToDouble(listSItem["Rate"]) / 100);//修改后分配产值=修改后车间产值*修改后车间比例
                    originListS.SetItem("Bonus", adjustTaskBonus);
                    originListS.SetItem("FID", listSFID);
                    string desListFID = Convert.ToString(originListS["ID"]);

                    Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfOutputCut_ListS");
                    Power.Business.IBusinessList GrantBo = grantBo.FindAll("WBSID='" + listSItem["WBSID"].ToString() + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if(GrantBo.Count>0)
                    {
                        NewLife.Log.XTrace.WriteLine("值等于=" + adjustTaskBonus);
                        NewLife.Log.XTrace.WriteLine("值等于=" + GrantBo[0]["Bonus"].ToString());
                        double PayBonus = 0, PayBonusRate = 0;
                        if (GrantBo[0]["PayBonus"] == null)
                        {
                            PayBonus = 0;
                        }
                        else
                            PayBonus = double.Parse(GrantBo[0]["PayBonus"].ToString());
                        if (GrantBo[0]["PayBonusRate"] == null)
                        {
                            PayBonusRate = 0;
                        }
                        else
                            PayBonusRate = double.Parse(GrantBo[0]["PayBonusRate"].ToString());

                        if ((!adjustTaskBonus.Equals(GrantBo[0]["Bonus"].ToString())) && (PayBonus != 0 && PayBonusRate != 0))//判断值不一样
                        {
                            originListS.SetItem("StatusCode", "待增补");
                        }
                    }
                    
                        //孙孙表
                        Power.Business.IBusinessList adjustListDesListList = adjustListDesListBO.FindAll("FID", Convert.ToString(listSItem["ID"]));
                    if (adjustListDesListList.Count == 0)
                    {
                        NewLife.Log.XTrace.WriteLine("调整审批后事件--根据调整表任务ID未找到对应人员，任务ID为：" + Convert.ToString(listSItem["ID"]));
                    }
                    foreach (var listDesListItem in adjustListDesListList)
                    {
                        NewLife.Log.XTrace.WriteLine("调整审批后事件--调整孙孙表循环");
                        Power.Business.IBaseBusiness originListDesList = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_ProfOutputCut_ListS_DesList");
                        foreach (KeyValuePair<string, Power.Business.BusinessProperty> businessProperty in originListDesListBO.EntityPropertyList)
                        {
                            NewLife.Log.XTrace.WriteLine("调整审批后事件--写入孙孙表循环");
                            originListDesList.SetItem(businessProperty.Value.PropertyName, listDesListItem[businessProperty.Value.PropertyName]);
                        }
                        if (!(Convert.ToString(listDesListItem["DesListID"]) == "00000000-0000-0000-0000-000000000000"))
                        {
                            originListDesList.SetItem("ID", listDesListItem["DesListID"]);
                        }
                        originListDesList.SetItem("FID", desListFID);
                        //originListDesList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        submitListDesListList.Add(originListDesList);
                    }
                    //originListS.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    submitListSList.Add(originListS);


                }
                //originList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                submitListList.Add(originList);
            }
            //删除原有的子、孙、孙孙表数据
            //delete(originFID);
            //子表
            Power.Business.IBusinessList originListList = originListBO.FindAll("FID", originFID, Power.Business.SearchFlag.IgnoreRight);
            foreach (var listItem in originListList)
            {
                //孙表
                Power.Business.IBusinessList originListSList = originListSBO.FindAll("FID", Convert.ToString(listItem["ID"]));
                foreach (var listSItem in originListSList)
                {
                    //孙孙表
                    Power.Business.IBusinessList originListDesListList = originListDesListBO.FindAll("FID", Convert.ToString(listSItem["ID"]));
                    originListDesListList.Delete();
                }
                originListSList.Delete();
            }
            originListList.Delete();

            //保存新数据
            submitListList.Save(true);
            submitListSList.Save(true);
            submitListDesListList.Save(true);
            //更新原有的分配产值（主表）
            Power.Business.IBaseBusiness baseBusiness = originBO.FindByKey(originFID, Power.Business.SearchFlag.IgnoreRight);

            baseBusiness.SetItem("AllocationBonus", BonusSum);
            baseBusiness.Save(System.ComponentModel.DataObjectMethodType.Update);

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
                                    //SbList2[0].SetItem("IfGrant", null);//项目名称
                                    //SbList2[0].Save(System.ComponentModel.DataObjectMethodType.Update);


                                    string htsql = "";
                                    XCode.DataAccessLayer.DAL dalsql = XCode.DataAccessLayer.DAL.Create();
                                    string sSQL = "";
                                    sSQL = "select isnull(StatusCode,'') as StatusCode from  NPS_DES_ThreeDimensional_ListS WHERE ID = '" + GrantTwoBoList[j]["ListSID"] + "' ";
                                    System.Data.DataTable dt2 = dalsql.Session.Query(sSQL).Tables[0];
                                    if (dt2.Rows.Count > 0)
                                    {
                                        if (dt2.Rows[0]["StatusCode"].ToString().Equals("正在支取") || dt2.Rows[0]["StatusCode"].ToString().Equals(""))
                                        {
                                            htsql = "";
                                            htsql = @"UPDATE NPS_DES_ThreeDimensional_ListS SET StatusCode = '' WHERE ID = '" + GrantTwoBoList[j]["ListSID"] + "'";
                                            dalsql.Session.Execute(htsql);
                                        }
                                        else if (dt2.Rows[0]["StatusCode"].ToString().Equals("正在增补"))
                                        {
                                            htsql = "";
                                            htsql = @"UPDATE NPS_DES_ThreeDimensional_ListS SET StatusCode = '待增补' WHERE ID = '" + GrantTwoBoList[j]["ListSID"] + "'";
                                            dalsql.Session.Execute(htsql);
                                        }

                                    }


                                }
                                //GrantTwoBoList.Delete();

                            }
                        }
                        //GrantOneBoList.Delete();
                    }
                    XCode.DataAccessLayer.DAL dsSQL = XCode.DataAccessLayer.DAL.Create();
                    string SQL = "";
                    SQL = @" delete from NPS_DES_ProjectOutputGrant_HumList_SubDetail  where fid in (select ID from NPS_DES_ProjectOutputGrant_HumList where fid='"+ID+"')";
                    dsSQL.Session.Execute(SQL);

                    SQL = "";
                    SQL = @"  delete from  NPS_DES_ProjectOutputGrant_HumList where fid='"+ID+"' ";
                    dsSQL.Session.Execute(SQL);

                }
            }
            catch (Exception e)
            {
                return e.ToString();
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
                                        if (SbList2[0]["StatusCode"].ToString().Equals(""))
                                        {
                                            SbList2[0].SetItem("StatusCode", "正在支取");
                                        }
                                        else if(SbList2[0]["StatusCode"].ToString().Equals("待增补"))
                                        {
                                            SbList2[0].SetItem("StatusCode", "正在增补");
                                        }
                                        
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
                            string IfGrant = "";
                            if (dt2.Rows[x]["IfGrant"] == null)
                                IfGrant = "";
                            else
                                IfGrant = dt2.Rows[x]["IfGrant"].ToString().Trim();

                            if (IfGrant.Equals("") && (dt2.Rows[x]["PayBonus"].ToString().Equals("0") && dt2.Rows[x]["PayBonusRate"].ToString().Equals("0")))
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
                                sItem.SetItem("IfGrant", IfGrant);//是否支取
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
                                sItem.SetItem("IfGrant", IfGrant);//是否支取
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
                            sItem.SetItem("IfGrant", dt2.Rows[x]["IfGrant"]==null?"": dt2.Rows[x]["IfGrant"].ToString());//是否支取
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

        //lx 新增三维产值获取

        public string GetProfBonusV3(string ID)
        {
            DeleteProfBonusV3(ID);
            Power.Business.IBusinessOperate grantBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProjectOutputGrant");//项目专业产值支取
            Power.Business.IBusinessList GrantBo = grantBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (GrantBo.Count > 0)
            {

                XCode.DataAccessLayer.DAL dal2 = XCode.DataAccessLayer.DAL.Create();
                string sql2 = "";
                /*sql2 = "select * from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND DesHumanID='" + dt3.Rows[y]["DesHumanID"] + "' AND IfIssue = '待支取' and 1 = (case when Type <> '设计' and PeriodCompleteScale=100 then 1 else 0 end) UNION ALL select top 1 * from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' AND DesHumanID='" + dt3.Rows[y]["DesHumanID"] + "' AND IfIssue = '待支取' and 1 = (case when Type = '设计' then 1 else 0 end) order by PeriodCompleteScale desc";*/
                sql2 = "select * from view_ProfOutputCutV3 WHERE ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成'  AND IfIssue = '待支取'";
                System.Data.DataTable dt2 = dal2.Session.Query(sql2).Tables[0];

                //sql3 -- 获取人员总产值信息
                XCode.DataAccessLayer.DAL dal3 = XCode.DataAccessLayer.DAL.Create();
                string sql3 = "";

                sql3 = "select Sum(AllocationBonus) as AllocationBonus,DesHumanID,DesHumanName,DesHumanCode from view_ProfOutputCutV3 WHERE " +
                    "ThreeDHeadHumID = '" + GrantBo[0]["RegHumId"] + "' AND ProfID = '" + GrantBo[0]["ProfID"] + "' AND IfComplete = '已完成' " +
                    "AND IfIssue = '待支取' group by DesHumanID,DesHumanName,DesHumanCode";

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
                        
                        if (dt2.Rows.Count > 0)
                        {
                            for (int x = 0; x < dt2.Rows.Count; x++)
                            {
                                if (dt2.Rows[x]["DesHumanID"].ToString().ToUpper().Trim().Equals(dt3.Rows[y]["DesHumanID"].ToString().ToUpper().Trim()))
                                {
                                    string IfGrant = "";
                                    if (dt2.Rows[x]["IfGrant"] == null)
                                        IfGrant = "";
                                    else
                                        IfGrant = dt2.Rows[x]["IfGrant"].ToString().Trim();

                                    if (IfGrant.Equals("") && (dt2.Rows[x]["PayBonus"].ToString().Equals("0") && dt2.Rows[x]["PayBonusRate"].ToString().Equals("0")))
                                    {
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
                                        sItems.SetItem("IfGrant", dt2.Rows[x]["IfGrant"] == null ? "" : dt2.Rows[x]["IfGrant"].ToString());//是否支取
                                        sItems.SetItem("Type", dt2.Rows[x]["Type"].ToString());//设校审类型
                                        sItems.SetItem("ThreeDHeadHumID", dt2.Rows[x]["ThreeDHeadHumID"].ToString());//专业负责人ID
                                        sItems.SetItem("ThreeDHeadHumCode", dt2.Rows[x]["ThreeDHeadHumCode"].ToString());//专业负责人编号
                                        sItems.SetItem("ThreeDHeadHumName", dt2.Rows[x]["ThreeDHeadHumName"].ToString());//专业负责人名称
                                        sItems.SetItem("ListSID", dt2.Rows[x]["ProfOutputCutLSID"] == null ? "00000000-0000-0000-0000-000000000000" : dt2.Rows[x]["ProfOutputCutLSID"].ToString());//专业施工图产值分解孙表ID
                                        sItems.Save(System.ComponentModel.DataObjectMethodType.Insert);

                                    }
                                    else
                                    {
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
                                        sItems.SetItem("IfGrant", dt2.Rows[x]["IfGrant"] == null ? "" : dt2.Rows[x]["IfGrant"].ToString());//是否支取
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
                    }
                }
                CutStatus(ID);
            }
            OutputControl outputControl = new OutputControl();
            outputControl.GetZB3V(ID);
            return "";
        }

        //调整明细审批后事件
        public string UpTZMXStatusCode(string ID, string originFID)
        {
            //查询调整明细中的子项明细信息
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            sql1 = "select * from NPS_DES_ProfOutputCutAdjust_List where fid='" + ID + "' ";
            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];//调整明细每一个子项信息

            sql1 = "";
            sql1 = "select * from NPS_DES_ProfOutputCut_List where fid='" + originFID + "' ";
            System.Data.DataTable dt2 = dal1.Session.Query(sql1).Tables[0];//产值明细的每一个子项信息
            for (int x = 0; x < dt2.Rows.Count; x++)//循环产值明细中的每一个子项，每一个子项的每一个专业与调整明细中的产值明细的每一个子项，每一个子项中的每一个专业，是否产值比例相同
            {
                for (int i = 0; i < dt1.Rows.Count; i++)//调整明细每一个子项信息
                {
                    if (dt2.Rows[x]["SubCode"].ToString().Equals(dt1.Rows[i]["SubCode"].ToString()))//通过子项编号对应
                    {
                        //根据此时的id找到所有任务，再根据任务对应
                        sql1 = "";
                        sql1 = "select * from NPS_DES_ProfOutputCutAdjust_ListS where fid='" + dt1.Rows[0]["ID"].ToString() + "' ";
                        System.Data.DataTable dt3 = dal1.Session.Query(sql1).Tables[0];//调整明细中的任务


                        sql1 = "";
                        sql1 = "select * from NPS_DES_ProfOutputCut_ListS where fid='" + dt2.Rows[0]["ID"].ToString() + "' ";
                        System.Data.DataTable dt4 = dal1.Session.Query(sql1).Tables[0];//产值明细中的任务

                        for (int j = 0; j < dt3.Rows.Count; j++)//循环调整明细中的任务
                        {
                            for (int k = 0; k < dt4.Rows.Count; k++)//循环产值明细中的任务
                            {
                                if (dt3.Rows[j]["WBSID"].ToString().Equals(dt4.Rows[k]["WBSID"].ToString()))//如果存在wbsid相同的记录，则判断产值比例是否相同
                                {
                                    if (!dt3.Rows[j]["Rate"].ToString().Equals(dt4.Rows[k]["Rate"].ToString()))//如果不等于原有值，则将statuscode=待增补
                                    {
                                        sql1 = "";
                                        sql1 = "update NPS_DES_ProfOutputCut_ListS set StatusCode = '待增补' where ID='" + dt4.Rows[k]["ID"].ToString() + "' ";
                                        dal1.Session.Execute(sql1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }



        //比例调整明细审批后事件
        public string UpBLTZMXStatusCode(string ID, string originFID)
        {
            //查询调整明细中的子项明细信息
            XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
            string sql1 = "";
            sql1 = "select * from NPS_DES_ProfOutputCutScaleAdjust_List where fid='" + ID + "' ";
            System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];//比例调整明细每一个子项信息

            sql1 = "";
            sql1 = "select * from NPS_DES_ProfOutputCut_List where fid='" + originFID + "' ";
            System.Data.DataTable dt2 = dal1.Session.Query(sql1).Tables[0];//产值明细的每一个子项信息
            for (int x = 0; x < dt2.Rows.Count; x++)//循环产值明细中的每一个子项，每一个子项的每一个专业与调整明细中的产值明细的每一个子项，每一个子项中的每一个专业，是否产值比例相同
            {
                for (int i = 0; i < dt1.Rows.Count; i++)//调整明细每一个子项信息
                {
                    if (dt2.Rows[x]["SubCode"].ToString().Equals(dt1.Rows[i]["SubCode"].ToString()))//通过子项编号对应
                    {
                        //根据此时的id找到所有任务，再根据任务对应
                        sql1 = "";
                        sql1 = "select * from NPS_DES_ProfOutputCutScaleAdjust_ListS where fid='" + dt1.Rows[0]["ID"].ToString() + "' ";
                        System.Data.DataTable dt3 = dal1.Session.Query(sql1).Tables[0];//比例调整明细中的任务


                        sql1 = "";
                        sql1 = "select * from NPS_DES_ProfOutputCut_ListS where fid='" + dt2.Rows[0]["ID"].ToString() + "' ";
                        System.Data.DataTable dt4 = dal1.Session.Query(sql1).Tables[0];//产值明细中的任务

                        for (int j = 0; j < dt3.Rows.Count; j++)//循环调整明细中的任务
                        {
                            for (int k = 0; k < dt4.Rows.Count; k++)//循环产值明细中的任务
                            {
                                if (dt3.Rows[j]["WBSID"].ToString().Equals(dt4.Rows[k]["WBSID"].ToString()))//如果存在wbsid相同的记录，则判断产值比例是否相同
                                {
                                    if (!dt3.Rows[j]["Rate"].ToString().Equals(dt4.Rows[k]["Rate"].ToString()))//如果不等于原有值，则将statuscode=待增补
                                    {
                                        sql1 = "";
                                        sql1 = "update NPS_DES_ProfOutputCut_ListS set StatusCode = '待增补' where ID='" + dt4.Rows[k]["ID"].ToString() + "' ";
                                        dal1.Session.Execute(sql1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }












            
        
    }
}
