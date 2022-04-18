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
using System.Data.SqlClient;
using Power.Business.Tree;
using Power.Business;
using Power.Business.Attributes;
using Power.Business.Cache;
using Power.Business.Common;
using Power.Business.Exceptions;
using Power.Business.ViewEntity;
using Power.Controls;

namespace Power.SPMEMS.Services
{
    class taskproc
    {

        public string SelProjDZ(string EpsProjCode)
        {
            string POSID = "";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = "select * from NPS_INV_ProjectContrast where YPC_POSID='" + EpsProjCode + "' ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                POSID = CwData.Rows[0]["POSID"].ToString()+"|"+ CwData.Rows[0]["POST1"].ToString();
            }
            return POSID;
        }


        public string SelSAPProjDZ(string EpsProjCode)
        {
            string POSID = "";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = "select * from NPS_INV_ProjectContrast where POSID='" + EpsProjCode + "' ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                POSID = CwData.Rows[0]["YPC_POSID"].ToString() + "|" + CwData.Rows[0]["YPC_POST1"].ToString();
            }
            return POSID;
        }

        public string SelApproval(string EpsProjCode)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string ID = "00000000-0000-0000-0000-000000000000";
            string sSQL = "select * from NPS_DES_DesignApproval where EpsProjCode='" + EpsProjCode + "' order by UpdDate desc ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                ID = CwData.Rows[0]["ID"].ToString();
            }
            return ID;
        }

        public void InsContrast_XZZY(string ID, string FID, string parentID, string POSID)
        {
            string ssSQL = "";
            string GSLX = "", ListCode = "", ListName = "";
            ssSQL = "select * from  NPS_INV_ProjectContrast where ID='" + ID + "'";
            DataTable Data = XCode.DataAccessLayer.DAL.QuerySQL(ssSQL);
            if (Data.Rows.Count > 0)
            {
                GSLX = Data.Rows[0]["GSLX"].ToString();
                ListCode = Data.Rows[0]["POSID"].ToString();
                ListName = Data.Rows[0]["POST1"].ToString();
            }

            ID = ID.TrimEnd(',');
            string[] IDList = ID.Split(',');
            string sSQL = "";
            for (int i = 0; i < IDList.Length; i++)
            {
                sSQL = "";
                sSQL = "SELECT * FROM SAP_ET_ZIMPSTLH00590 where LEVEL=3";
                DataTable SAP_ET = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);

                sSQL = "";
                sSQL = "SELECT * FROM NPS_INV_ProjectContrast_List where ID='" + parentID + "'";
                DataTable NameData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);

                sSQL = "";
                sSQL = "select * from NPS_DES_DesignApproval_FirstList where  ID='" + IDList[i] + "'";//将概算明细查询出来
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                string sDM = "";
                for (int j = 0; j < CwData.Rows.Count; j++)
                {
                    for (int k = 0; k < SAP_ET.Rows.Count; k++)
                    {
                        if (SAP_ET.Rows[k]["POST1"].ToString() == "设备购置费")
                            SAP_ET.Rows[k]["POST1"] = "设备费";
                        if (SAP_ET.Rows[k]["POST1"].ToString() == NameData.Rows[0]["ListName"].ToString())
                        {
                            sDM = SAP_ET.Rows[k]["POSID"].ToString().Substring(14, 2);
                        }
                    }
                    Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                    Guid guid = Guid.NewGuid();
                    MXItem_L4.SetItem("ID", guid);
                    MXItem_L4.SetItem("FID", FID);
                    MXItem_L4.SetItem("ListCode", POSID + "0000" + sDM);
                    MXItem_L4.SetItem("ListName", CwData.Rows[j]["ListName"].ToString());
                    MXItem_L4.SetItem("ParentID", parentID);
                    if (NameData.Rows[0]["ListName"].ToString() == "设备费")
                    {
                        MXItem_L4.SetItem("EquipmentCost", CwData.Rows[j]["EquipmentCost"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[j]["EquipmentCost"].ToString());
                    }
                    else if (NameData.Rows[0]["ListName"].ToString() == "主要材料费")
                    {
                        MXItem_L4.SetItem("MaterialCost", CwData.Rows[j]["MaterialCost"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[j]["MaterialCost"].ToString());
                    }
                    else if (NameData.Rows[0]["ListName"].ToString() == "安装费")
                    {
                        MXItem_L4.SetItem("InstallCost", CwData.Rows[j]["InstallCost"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[j]["InstallCost"].ToString());
                    }
                    else if (NameData.Rows[0]["ListName"].ToString() == "建筑工程费")
                    {
                        MXItem_L4.SetItem("CivilConstruction", CwData.Rows[j]["CivilConstruction"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[j]["CivilConstruction"].ToString());
                    }
                    else if (NameData.Rows[0]["ListName"].ToString() == "其他费")
                    {
                        MXItem_L4.SetItem("OtherCost", CwData.Rows[j]["OtherCost"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[j]["OtherCost"].ToString());
                    }
                    MXItem_L4.SetItem("GSLX", GSLX);
                    MXItem_L4.SetItem("EpsProjLongCode", "1");
                    MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
                }
            }
        }

        public void InsContrast_List(string ID, string FID, string parentID, string POSID)
        {
            string ssSQL = "";
            string GSLX = "", ListCode = "", ListName = "";
            ssSQL = "select * from  NPS_INV_ProjectContrast where ID='" + ID + "'";
            DataTable Data = XCode.DataAccessLayer.DAL.QuerySQL(ssSQL);
            if (Data.Rows.Count > 0)
            {
                GSLX = Data.Rows[0]["GSLX"].ToString();
                ListCode = Data.Rows[0]["POSID"].ToString();
                ListName = Data.Rows[0]["POST1"].ToString();
            }

            ID = ID.TrimEnd(',');
            string[] IDList = ID.Split(',');
            string sSQL = "";
            for (int i = 0; i < IDList.Length; i++)
            {
                string EquipmentCost = "", MaterialCost = "", InstallCost = "", CivilConstruction = "", OtherCost = "", Totaled = "";
                sSQL = "";
                sSQL = "select * from NPS_DES_DesignApproval_FirstList where  ID='" + IDList[i] + "'";//将概算明细查询出来
                DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int j = 0; j < CwData.Rows.Count; j++)
                {
                    sSQL = "";
                    sSQL = @"select sum(EquipmentCost) as EquipmentCost,sum(MaterialCost) as MaterialCost,sum(InstallCost) as InstallCost,sum(CivilConstruction) as CivilConstruction,
                                sum(OtherCost) as OtherCost,sum(EquipmentCost+MaterialCost+InstallCost+CivilConstruction+OtherCost) as Totaled
                                 from NPS_DES_DesignApproval_FirstList where  ParentID='" + IDList[i] + "'";
                    DataTable SumData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                    if (SumData.Rows.Count > 0)
                    {
                        EquipmentCost = SumData.Rows[0]["EquipmentCost"].ToString();
                        MaterialCost = SumData.Rows[0]["MaterialCost"].ToString();
                        InstallCost = SumData.Rows[0]["InstallCost"].ToString();
                        CivilConstruction = SumData.Rows[0]["CivilConstruction"].ToString();
                        OtherCost = SumData.Rows[0]["OtherCost"].ToString();
                        Totaled = SumData.Rows[0]["Totaled"].ToString();
                    }

                    Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                    Guid guid = Guid.NewGuid();
                    MXItem_L4.SetItem("ID", guid);
                    MXItem_L4.SetItem("FID", FID);
                    MXItem_L4.SetItem("ListCode", POSID + "0000");
                    MXItem_L4.SetItem("ListName", CwData.Rows[j]["ListName"].ToString());
                    MXItem_L4.SetItem("ParentID", parentID);
                    MXItem_L4.SetItem("EquipmentCost", EquipmentCost);
                    MXItem_L4.SetItem("MaterialCost", MaterialCost);
                    MXItem_L4.SetItem("InstallCost", InstallCost);
                    MXItem_L4.SetItem("CivilConstruction", CivilConstruction);
                    MXItem_L4.SetItem("OtherCost", OtherCost);
                    MXItem_L4.SetItem("Totaled", Totaled);
                    MXItem_L4.SetItem("GSLX", "基础设计概算");
                    MXItem_L4.SetItem("EpsProjLongCode", "1");
                    MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    string[] Name = { "设备费", "主要材料费", "安装费", "建筑工程费", "其他费" };
                    for (int k = 0; k < Name.Length; k++)//插入5条记录
                    {
                        Power.Business.IBaseBusiness MX = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                        Guid guid1 = Guid.NewGuid();
                        MX.SetItem("ID", guid1);
                        MX.SetItem("FID", FID);
                        MX.SetItem("ListCode", POSID + "0000");
                        MX.SetItem("ListName", Name[k]);
                        MX.SetItem("ParentID", guid);
                        if (Name[k] == "设备费")
                            MX.SetItem("EquipmentCost", EquipmentCost);
                        else if (Name[k] == "主要材料费")
                            MX.SetItem("MaterialCost", MaterialCost);
                        else if (Name[k] == "安装费")
                            MX.SetItem("InstallCost", InstallCost);
                        else if (Name[k] == "建筑工程费")
                            MX.SetItem("CivilConstruction", CivilConstruction);
                        else if (Name[k] == "其他费")
                            MX.SetItem("OtherCost", OtherCost);
                        MX.SetItem("Totaled", Totaled);
                        MX.SetItem("GSLX", GSLX);
                        MX.SetItem("EpsProjLongCode", "1");
                        MX.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    }

                }
            }
        }


        public string SelGS(string ID)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string JG = "";
            string sSQL = "select * from NPS_INV_ProjectContrast where ID='" + ID + "' ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                JG = CwData.Rows[0]["GSLX"].ToString();
            }
            return JG;
        }

        public string SelGSLX(string YPC_POSID,string ID)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string JG = "";
            string sSQL = "select * from NPS_ENGPLAN_AdviseBook where projectcode='" + YPC_POSID + "' ";//是否存在可研批复
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)//如果存在记录，则继续判断是否存在基础设计概算
            {
                JG = "可研概算";
                sSQL = "";
                sSQL = "select* from NPS_DES_DesignApproval where epsprojcode = '"+YPC_POSID+"' ";
                CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (CwData.Rows.Count > 0)//如果存在基础设计概算。则继续判断是否存在概算调整
                {
                    JG = "基础设计概算";
                    sSQL = "";
                    sSQL = "select * from NPS_COST_OverBudgetApply where epsprojcode='"+ YPC_POSID + "' ";
                    CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                    if (CwData.Rows.Count > 0)
                        JG = "概算调整";
                }                    
            }
            else
                JG = "";//没有结果，即为空

            sSQL = "";
            sSQL = "update NPS_INV_ProjectContrast set GSLX='"+JG+ "' where ID='"+ID+"'";
            dal.Execute(sSQL);
            return JG;
        }

        public string SelZT(string YPC_POSID, string ID)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string JG = "";
            string sSQL = "";
            string GSLX = "";//主表的概算类型
            sSQL = "select * from NPS_INV_ProjectContrast_List where  FID='" + ID + "'";//是否存在记录
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);

            sSQL = "select * from  NPS_INV_ProjectContrast where ID='" + ID + "'";
            DataTable Data = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (Data.Rows.Count > 0)
            {
                GSLX = Data.Rows[0]["GSLX"].ToString();
            }

            if (CwData.Rows.Count > 0)//如果存在记录，则先对比主表与子表的概算类型是否一样
            {
                if (GSLX == CwData.Rows[0]["GSLX"].ToString())
                {
                    JG = "已同步";
                }
                else
                {
                    JG = "待更新";
                }
            }
            else//不存在就是待同步
            {
                JG = "待同步";
            }

            return JG;
        }

        public void UpGS(string ZDMC, string parentID,string ID,string Totaled)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = "";
            sSQL = "";
            sSQL = "update NPS_INV_ProjectContrast_list set " + ZDMC.Trim() + "='" + Totaled.ToString() + "' where id='" + ID + "'";
            dal.Execute(sSQL);

            sSQL = "";
            sSQL = "update NPS_INV_ProjectContrast_list set " + ZDMC.Trim() + "='" + Totaled.ToString() + "',Totaled='"+ Totaled.ToString() + "' where id='" + parentID + "'";
            dal.Execute(sSQL);
        }

        public string InsertGSMX(string YPC_POSID, string ID)
        {
            string JG = "";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = "";
            string GSLX = "", ListCode="",ListName="";
            sSQL = "select * from  NPS_INV_ProjectContrast where ID='" + ID + "'";
            DataTable Data = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (Data.Rows.Count > 0)
            {
                GSLX = Data.Rows[0]["GSLX"].ToString();
                ListCode = Data.Rows[0]["POSID"].ToString();
                ListName = Data.Rows[0]["POST1"].ToString();
            }
            sSQL = "";
            sSQL = "select * from NPS_DES_DesignApproval where EpsProjCode='" + YPC_POSID + "'";//是否存在基础设计概算
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                Guid guid = Guid.NewGuid();
                MXItem_L4.SetItem("ID", guid);
                MXItem_L4.SetItem("FID", ID);
                MXItem_L4.SetItem("ListCode", ListCode);
                MXItem_L4.SetItem("ListName", ListName);
                MXItem_L4.SetItem("ParentID", "00000000-0000-0000-0000-000000000000");
                MXItem_L4.SetItem("GSLX", GSLX);
                MXItem_L4.SetItem("EpsProjLongCode", "1");
                MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            return JG;

        }

       

        public string InsGSMX(string YPC_POSID, string ID)
        {
            Dictionary<string, string> Zid = new Dictionary<string, string>();
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = "";
            string GSLX = "";
            sSQL = "select * from  NPS_INV_ProjectContrast where ID='" + ID + "'";
            DataTable Data = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (Data.Rows.Count > 0)
            {
                GSLX = Data.Rows[0]["GSLX"].ToString();
            }
            string JG = "";
            sSQL = "";
            sSQL = "select * from NPS_DES_DesignApproval where EpsProjCode='"+ YPC_POSID + "'";//是否存在基础设计概算
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                string FID = CwData.Rows[0]["ID"].ToString();
                sSQL = "";
                sSQL = "delete from  NPS_INV_ProjectContrast_List where FID='"+ ID + "'";
                dal.Execute(sSQL);
                sSQL = "";
                sSQL = "select * from NPS_DES_DesignApproval_FirstList where  FID='"+ FID + "'";//将概算明细查询出来
                CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                foreach (DataRow item in CwData.Rows)
                {
                    Guid guid = Guid.NewGuid();
                    Zid.Add(item["ID"].ToString(), guid.ToString());
                }
                for (int i=0;i< CwData.Rows.Count;i++)
                {
                    Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                   
                    MXItem_L4.SetItem("ID", Zid[CwData.Rows[i]["ID"].ToString()].ToString());
                    MXItem_L4.SetItem("FID", ID);
                    MXItem_L4.SetItem("ListCode", CwData.Rows[i]["ListCode"].ToString());
                    MXItem_L4.SetItem("ListName", CwData.Rows[i]["ListName"].ToString());
                    if(Zid.ContainsKey(CwData.Rows[i]["ParentID"].ToString()))
                        MXItem_L4.SetItem("ParentID", Zid[CwData.Rows[i]["ParentID"].ToString()].ToString());
                    else
                        MXItem_L4.SetItem("ParentID", "00000000-0000-0000-0000-000000000000");
                    MXItem_L4.SetItem("EquipmentCost", CwData.Rows[i]["EquipmentCost"].ToString());
                    MXItem_L4.SetItem("MaterialCost", CwData.Rows[i]["MaterialCost"].ToString());
                    MXItem_L4.SetItem("InstallCost", CwData.Rows[i]["InstallCost"].ToString());
                    MXItem_L4.SetItem("CivilConstruction", CwData.Rows[i]["CivilConstruction"].ToString());
                    MXItem_L4.SetItem("OtherCost", CwData.Rows[i]["OtherCost"].ToString());
                    MXItem_L4.SetItem("Totaled", CwData.Rows[i]["Totaled"].ToString());
                    MXItem_L4.SetItem("GSLX", GSLX);
                    MXItem_L4.SetItem("EpsProjLongCode", "1");
                    MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
                }

                string[] Name = { "设备费", "主要材料费", "安装费", "建筑工程费", "其他费" };
                sSQL = "";
                sSQL = "select * from NPS_INV_ProjectContrast_list A where not exists(select 1 from NPS_INV_ProjectContrast_list B where B.parentid=A.ID) and fid='"+ID+"'";//查询明细记录的最末级
                CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                for (int i = 0; i < CwData.Rows.Count; i++)//在每个最末级后增加5条记录
                {
                    for (int j = 0; j < Name.Length; j++)//插入5条记录
                    {
                        Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                        Guid guid = Guid.NewGuid();
                        MXItem_L4.SetItem("ID", guid);
                        MXItem_L4.SetItem("FID", ID);
                        MXItem_L4.SetItem("ListCode", CwData.Rows[i]["ListCode"].ToString());
                        MXItem_L4.SetItem("ListName", Name[j]);
                        MXItem_L4.SetItem("ParentID", CwData.Rows[i]["ID"].ToString());
                        if (Name[j] == "设备费")
                            MXItem_L4.SetItem("EquipmentCost", CwData.Rows[i]["EquipmentCost"].ToString());
                        else if (Name[j] == "主要材料费")
                            MXItem_L4.SetItem("MaterialCost", CwData.Rows[i]["MaterialCost"].ToString());
                        else if (Name[j] == "安装费")
                            MXItem_L4.SetItem("InstallCost", CwData.Rows[i]["InstallCost"].ToString());
                        else if (Name[j] == "建筑工程费")
                            MXItem_L4.SetItem("CivilConstruction", CwData.Rows[i]["CivilConstruction"].ToString());
                        else if (Name[j] == "其他费")
                            MXItem_L4.SetItem("OtherCost", CwData.Rows[i]["OtherCost"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[i]["Totaled"].ToString());
                        MXItem_L4.SetItem("GSLX", GSLX);
                        MXItem_L4.SetItem("EpsProjLongCode", "1");
                        MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    }                    
                }
            }
            else//如果不存在，则查询可研概算
            {
                sSQL = "";
                sSQL = " select * from NPS_ENGPLAN_AdviseBook where EpsProjCode='" + YPC_POSID + "'";
                CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (CwData.Rows.Count > 0)
                {
                    sSQL = "";
                    sSQL = "delete from  NPS_INV_ProjectContrast_List where FID='" + ID + "'";
                    dal.Execute(sSQL);
                    string FID = CwData.Rows[0]["ID"].ToString();
                    sSQL = "";
                    sSQL = "select * from NPS_ENGPLAN_AdviseBook_List where  FID='" + FID + "'";//将概算明细查询出来
                    CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                    foreach (DataRow item in CwData.Rows)
                    {
                        Guid guid = Guid.NewGuid();
                        Zid.Add(item["ID"].ToString(), guid.ToString());
                    }
                    for (int i = 0; i < CwData.Rows.Count; i++)
                    {
                        Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                        Guid guid = Guid.NewGuid();
                        MXItem_L4.SetItem("ID", Zid[CwData.Rows[i]["ID"].ToString()].ToString());
                        MXItem_L4.SetItem("FID", ID);
                        MXItem_L4.SetItem("ListCode", CwData.Rows[i]["ListCode"].ToString());
                        MXItem_L4.SetItem("ListName", CwData.Rows[i]["ListName"].ToString());
                        if (Zid.ContainsKey(CwData.Rows[i]["ParentID"].ToString()))
                            MXItem_L4.SetItem("ParentID", Zid[CwData.Rows[i]["ParentID"].ToString()].ToString());
                        else
                            MXItem_L4.SetItem("ParentID", "00000000-0000-0000-0000-000000000000");
                        MXItem_L4.SetItem("EquipmentCost", CwData.Rows[i]["EquipmentCost"].ToString());
                        MXItem_L4.SetItem("MaterialCost", CwData.Rows[i]["MaterialCost"].ToString());
                        MXItem_L4.SetItem("InstallCost", CwData.Rows[i]["InstallCost"].ToString());
                        MXItem_L4.SetItem("CivilConstruction", CwData.Rows[i]["CivilConstruction"].ToString());
                        MXItem_L4.SetItem("OtherCost", CwData.Rows[i]["OtherCost"].ToString());
                        MXItem_L4.SetItem("Totaled", CwData.Rows[i]["Totaled"].ToString());
                        MXItem_L4.SetItem("GSLX", GSLX);
                        MXItem_L4.SetItem("EpsProjLongCode", "1");
                        MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    }

                    string[] Name = { "设备费", "主要材料费", "安装费", "建筑工程费", "其他费" };
                    sSQL = "";
                    sSQL = "select * from NPS_INV_ProjectContrast_list A where not exists(select 1 from NPS_INV_ProjectContrast_list B where B.parentid=A.ID) and fid='" + ID + "'";//查询明细记录的最末级
                    CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                    for (int i = 0; i < CwData.Rows.Count; i++)//在每个最末级后增加5条记录
                    {
                        for (int j = 0; j < Name.Length; j++)//插入5条记录
                        {
                            Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast_List");
                            Guid guid = Guid.NewGuid();
                            MXItem_L4.SetItem("ID", guid);
                            MXItem_L4.SetItem("FID", ID);
                            MXItem_L4.SetItem("ListCode", CwData.Rows[i]["ListCode"].ToString());
                            MXItem_L4.SetItem("ListName", Name[j]);
                            MXItem_L4.SetItem("ParentID", CwData.Rows[i]["ID"].ToString());
                            if (Name[j] == "设备费")
                                MXItem_L4.SetItem("EquipmentCost", CwData.Rows[i]["EquipmentCost"].ToString());
                            else if (Name[j] == "主要材料费")
                                MXItem_L4.SetItem("MaterialCost", CwData.Rows[i]["MaterialCost"].ToString());
                            else if (Name[j] == "安装费")
                                MXItem_L4.SetItem("InstallCost", CwData.Rows[i]["InstallCost"].ToString());
                            else if (Name[j] == "建筑工程费")
                                MXItem_L4.SetItem("CivilConstruction", CwData.Rows[i]["CivilConstruction"].ToString());
                            else if (Name[j] == "其他费")
                                MXItem_L4.SetItem("OtherCost", CwData.Rows[i]["OtherCost"].ToString());
                            MXItem_L4.SetItem("Totaled", CwData.Rows[i]["Totaled"].ToString());
                            MXItem_L4.SetItem("GSLX", GSLX);
                            MXItem_L4.SetItem("EpsProjLongCode", "1");
                            MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }
                    }

                }
            }
            return JG;
        }


        public string InsContrast()
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = "";
            string JG = "";
            sSQL = "Select A.* From  SAP_ET_XMINFO A Where   (0=0)  and (1=1 and    A.POSID NOT IN(select isnull(ET.POSID,'') FROM NPS_INV_ProjectContrast ET )) ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            for(int i=0;i< CwData.Rows.Count;i++)
            {
                Power.Business.IBaseBusiness MXItem_L4 = Power.Business.BusinessFactory.CreateBusiness("NPS_INV_ProjectContrast");
                Guid guid = Guid.NewGuid();
                MXItem_L4.SetItem("ID", guid);
                MXItem_L4.SetItem("POSID", CwData.Rows[i]["POSID"].ToString());
                MXItem_L4.SetItem("POST1", CwData.Rows[i]["POST1"].ToString());
                MXItem_L4.SetItem("BizAreaId", "00000000-0000-0000-0000-00000000000A");
                MXItem_L4.SetItem("EpsProjLongCode", "1");
                MXItem_L4.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }

                return JG;
        }



        public void AddTaskprocZQ(string currentfeedback, string currentplan, string currentperiod, string currentrsrc)
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
                sSQL += "and (1=1 ) and rsrc_guid is not null and not exists(select 1 from PLN_TaskProcZQ A where A.task_guid=VPPFB.task_guid and A.masterid=VPPFB.masterid) Order By  type,VPPFB.task_code,VPPFB.Sequ ASC ";
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
                        float complete_pct = 0;
                        string sSQL3 = "select sum(complete_pct*est_wt_pct) as complete_pct from PS_PLN_TaskProc_Sub where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "' group by proc_guid";
                        DataTable PS_PLN_TaskProc_Sub = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        if (PS_PLN_TaskProc_Sub.Rows.Count > 0)
                        {
                            if (PS_PLN_TaskProc_Sub.Rows[0]["complete_pct"].ToString().Trim().Equals(""))
                            {
                                complete_pct = 0;
                            }
                            else
                                complete_pct = float.Parse(PS_PLN_TaskProc_Sub.Rows[0]["complete_pct"].ToString());
                        }
                        else if (PS_PLN_TaskProc_Sub.Rows.Count == 0)
                        {
                            if (TaskProcData.Rows[j]["PLN_act_end_date"].ToString().Trim().Equals("") || string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString()))
                            {
                                complete_pct = 0;
                            }
                            else
                            {
                                complete_pct = 100;
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

                        string sSQL2 = "";
                        sSQL2 = "insert into PLN_TaskprocZQ ";
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


                        //Power.Business.IBaseBusiness NewMain = Power.Business.BusinessFactory.CreateBusiness("PLN_TaskprocZQ");
                        //NewMain.SetItem("proj_id", TaskProcData.Rows[j]["proj_id"].ToString());
                        //NewMain.SetItem("wbs_id", TaskProcData.Rows[j]["wbs_id"].ToString());
                        //NewMain.SetItem("task_id", TaskProcData.Rows[j]["task_id"].ToString());
                        //NewMain.SetItem("task_guid", TaskProcData.Rows[j]["task_guid"].ToString());
                        //NewMain.SetItem("seq_num", TaskProcData.Rows[j]["seq_num"].ToString());
                        //NewMain.SetItem("CompleteOrNot", TaskProcData.Rows[j]["CompleteOrNot"].ToString());
                        //NewMain.SetItem("proc_name", TaskProcData.Rows[j]["proc_name"].ToString());
                        //NewMain.SetItem("proc_descri", TaskProcData.Rows[j]["proc_descri"].ToString());
                        //NewMain.SetItem("est_wt", TaskProcData.Rows[j]["est_wt"].ToString());
                        //NewMain.SetItem("complete_pct", complete_pct);
                        //NewMain.SetItem("act_end_date", TaskProcData.Rows[j]["act_end_date"].ToString());
                        //NewMain.SetItem("SysOrNot", TaskProcData.Rows[j]["SysOrNot"].ToString());
                        //NewMain.SetItem("target_end_date_lag", TaskProcData.Rows[j]["target_end_date_lag"].ToString());
                        //NewMain.SetItem("expect_end_date_lag", TaskProcData.Rows[j]["expect_end_date_lag"].ToString());
                        //NewMain.SetItem("rsrc_id", TaskProcData.Rows[j]["rsrc_id"].ToString());
                        //NewMain.SetItem("temp_id", TaskProcData.Rows[j]["temp_id"].ToString());
                        //NewMain.SetItem("update_date", TaskProcData.Rows[j]["update_date"].ToString());
                        //NewMain.SetItem("p3ec_proc_id", TaskProcData.Rows[j]["p3ec_proc_id"].ToString());
                        //NewMain.SetItem("p3ec_flag", TaskProcData.Rows[j]["p3ec_flag"].ToString());
                        //NewMain.SetItem("proc_guid", TaskProcData.Rows[j]["proc_guid"].ToString());
                        //NewMain.SetItem("proj_guid", TaskProcData.Rows[j]["proj_guid"].ToString());
                        //NewMain.SetItem("plan_guid", TaskProcData.Rows[j]["plan_guid"].ToString());
                        //NewMain.SetItem("plan_id", TaskProcData.Rows[j]["plan_id"].ToString());
                        //NewMain.SetItem("wbs_guid", TaskProcData.Rows[j]["wbs_guid"].ToString());
                        //NewMain.SetItem("rsrc_guid", TaskProcData.Rows[j]["rsrc_guid"].ToString());
                        //NewMain.SetItem("temp_guid", TaskProcData.Rows[j]["temp_guid"].ToString());
                        //NewMain.SetItem("est_wt_pct", TaskProcData.Rows[j]["est_wt_pct"].ToString());
                        //NewMain.SetItem("keyword", TaskProcData.Rows[j]["keyword"].ToString());
                        //NewMain.SetItem("formid", TaskProcData.Rows[j]["formid"].ToString());
                        //NewMain.SetItem("update_user", TaskProcData.Rows[j]["update_user"].ToString());
                        //NewMain.SetItem("create_date", TaskProcData.Rows[j]["create_date"].ToString());
                        //NewMain.SetItem("create_user", TaskProcData.Rows[j]["create_user"].ToString());
                        //NewMain.SetItem("delete_session_id", TaskProcData.Rows[j]["delete_session_id"].ToString());
                        //NewMain.SetItem("delete_date", TaskProcData.Rows[j]["delete_date"].ToString());
                        //NewMain.SetItem("target_end_date", TaskProcData.Rows[j]["target_end_date"].ToString());
                        //NewMain.SetItem("proc_code", TaskProcData.Rows[j]["proc_code"].ToString());
                        //NewMain.SetItem("PLN_target_start_date", TaskProcData.Rows[j]["PLN_target_start_date"].ToString());
                        //NewMain.SetItem("PLN_target_end_date", TaskProcData.Rows[j]["PLN_target_end_date"].ToString());
                        //NewMain.SetItem("PLN_act_end_date", TaskProcData.Rows[j]["PLN_act_end_date"].ToString());
                        //NewMain.SetItem("PLN_act_start_date", TaskProcData.Rows[j]["PLN_act_start_date"].ToString());
                        //NewMain.SetItem("masterid", masterid);
                        //NewMain.Save(System.ComponentModel.DataObjectMethodType.Insert);


                        sSQL3 = "";
                        sSQL3 = "select ProcSub_guid,ProcSub_Name,proj_guid,plan_guid,wbs_guid,task_guid,proc_guid,seq_num,est_wt,est_wt_pct,complete_pct,";
                        sSQL3 += "      temp_guid,remark,RegDate,RegHumName,RegHumId,UpdHumId,UpdHuman,UpdDate,CheckDate,SubState,CompleteDate,ProcSub_Code,target_end_date,act_end_date";
                        sSQL3 += " from PS_PLN_TaskProc_Sub ";
                        sSQL3 += "where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "'";
                        PS_PLN_TaskProc_Sub = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        for (int k = 0; k < PS_PLN_TaskProc_Sub.Rows.Count; k++)
                        {
                            string WCBFB = "0";
                            string SubState = "";
                            string begindate = "";
                            string enddate = "";
                            if (PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString().Trim() == "")
                            {
                                begindate = "1900-01-01";
                            }
                            else
                                begindate = PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString();
                            if (PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString().Trim().Equals(""))
                            {
                                enddate = "1900-01-01";
                            }
                            else
                                enddate = PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString();
                            if (PS_PLN_TaskProc_Sub.Rows[k]["complete_pct"].ToString() == "" || float.Parse(PS_PLN_TaskProc_Sub.Rows[k]["complete_pct"].ToString()) == 0)
                            {
                                //第一种情况，开始时间在周期时间之前且在周期完成时间之内
                                if (DateTime.Parse(begindate) <= period_begindate && DateTime.Parse(enddate) >= period_begindate && DateTime.Parse(enddate) <= period_enddate)
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if ((begindate != "1900-01-01") && (enddate == "1900-01-01"))//有开始时间没有完成时间
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if ((begindate == "1900-01-01") && (enddate == "1900-01-01"))
                                {
                                    SubState = "未开始";
                                    WCBFB = "0";
                                }
                                else if (DateTime.Parse(begindate) <= period_begindate && DateTime.Parse(enddate) >= period_enddate)//第二种情况，开始时间在周期开始时间之前且完成时间在周期完成时间之后
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if (DateTime.Parse(begindate) <= period_begindate && DateTime.Parse(enddate) <= period_enddate)//第三种情况，开始时间和结束时间都在周期开始时间之前
                                {
                                    SubState = "已完成";
                                    WCBFB = "100";
                                }
                                else if ((DateTime.Parse(begindate) >= period_begindate && DateTime.Parse(begindate) <= period_enddate) &&
                                          DateTime.Parse(enddate) >= period_begindate && DateTime.Parse(enddate) <= period_enddate)//第四种情况，开始时间和结束时间都在周期开始与完成时间之内
                                {
                                    SubState = "已完成";
                                    WCBFB = "100";
                                }
                                else if ((DateTime.Parse(begindate) >= period_begindate && DateTime.Parse(begindate) <= period_enddate) && DateTime.Parse(enddate) >= period_enddate)//第五种情况，开始时间在周期开始、完成时间之内，结束时间在周期结束时间之后
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if (DateTime.Parse(begindate) >= period_enddate && DateTime.Parse(enddate) >= period_enddate)//第六种情况，开始时间和完成时间都在周期结束时间之后
                                {
                                    SubState = "未开始";
                                    WCBFB = "0";
                                }

                            }
                            else
                            {
                                WCBFB = PS_PLN_TaskProc_Sub.Rows[k]["complete_pct"].ToString();
                                //第一种情况，开始时间在周期时间之前且在周期完成时间之内
                                if (DateTime.Parse(begindate) <= period_begindate && DateTime.Parse(enddate) >= period_begindate && DateTime.Parse(enddate) <= period_enddate)
                                {
                                    SubState = "已开始";
                                }
                                else if ((begindate != "1900-01-01") && (enddate == "1900-01-01"))//有开始时间没有完成时间
                                {
                                    SubState = "已开始";
                                }
                                else if ((begindate == "1900-01-01") && (enddate == "1900-01-01"))
                                {
                                    SubState = "未开始";
                                }
                                else if (DateTime.Parse(begindate) <= period_begindate && DateTime.Parse(enddate) >= period_enddate)//第二种情况，开始时间在周期开始时间之前且完成时间在周期完成时间之后
                                {
                                    SubState = "已开始";
                                }
                                else if (DateTime.Parse(begindate) <= period_begindate && DateTime.Parse(enddate) <= period_enddate)//第三种情况，开始时间和结束时间都在周期开始时间之前
                                {
                                    SubState = "已完成";
                                }
                                else if ((DateTime.Parse(begindate) >= period_begindate && DateTime.Parse(begindate) <= period_enddate) &&
                                          DateTime.Parse(enddate) >= period_begindate && DateTime.Parse(enddate) <= period_enddate)//第四种情况，开始时间和结束时间都在周期开始与完成时间之内
                                {
                                    SubState = "已完成";
                                }
                                else if ((DateTime.Parse(begindate) >= period_begindate && DateTime.Parse(begindate) <= period_enddate) && DateTime.Parse(enddate) >= period_enddate)//第五种情况，开始时间在周期开始、完成时间之内，结束时间在周期结束时间之后
                                {
                                    SubState = "已开始";
                                }
                                else if (DateTime.Parse(begindate) >= period_enddate && DateTime.Parse(enddate) >= period_enddate)//第六种情况，开始时间和完成时间都在周期结束时间之后
                                {
                                    SubState = "未开始";
                                }


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
                                KGLJGCL = float.Parse(NPS_BOQZQ.Rows[h]["ListingNum"].ToString()) * (float.Parse(TaskProcData.Rows[j]["complete_pct"].ToString()) / 100);
                                KGLJJE = float.Parse(NPS_BOQZQ.Rows[h]["ListingPrice"].ToString()) * (float.Parse(TaskProcData.Rows[j]["complete_pct"].ToString()) / 100);
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
                                if (DateTime.Parse(NPS_BOQZQ.Rows[h]["PlanSatrt"].ToString()).Month.Equals(period_begindate.Month))
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


                        }
                        UpdateSubZQ(currentfeedback, TaskProcData.Rows[j]["proc_guid"].ToString(), currentplan, currentperiod, currentrsrc);
                    }
                    UpFeedBackRecord(task_guid, currentfeedback, currentplan, currentperiod, currentrsrc);
                }
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
                sUpdateSQL = "";
                sUpdateSQL = "update PS_PLN_FeedBackRecord_Task set act_start_date=null where act_start_date <convert(datetime,'1950-01-01 23:00:00.000') ";
                dal.Execute(sUpdateSQL);
                dal.Commit();
            }
            catch (Exception ex)
            {
                dal.Rollback();
                throw (ex);
            }
        }



        public string GetConEpsCode(string ConCode, string ProjectNature, string ConType, string SignYear)
        {
            string Code = "";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string Sql = "select * from NPS_CON_RevenueContract A where A.ConType='" + ConType + "' and A.ConEpsCode like '%" + SignYear + "%' and A.ConEpsCode like '%" + ProjectNature + "%' Order by RIGHT(A.ConEpsCode,6)  desc";
            System.Data.DataTable Dt = dal.Session.Query(Sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                string LastCode = Dt.Rows[0]["ConEpsCode"].ToString().Split("-")[4];
                Code = ConCode + "-" + ProjectNature + (Convert.ToInt32(LastCode.Split(ProjectNature)[0].Split("】")[0]) + 1).ToString().PadLeft(3, '0') + "】";
            }
            else
            {
                Code = ConCode + "-" + ProjectNature + "001" + "】";
            }
            return Code;
        }

        public DataTable ReslutData1(string KeyWord, string KeyValue)
        {

            DataTable tempFile = new DataTable();
            //解码二进制数组
            DataSet ds = Power.Global.PowerGlobal.Office.ExcelToDataSet("C:\\导入文件2.xlsx");
            //tempFile = GetExcelDatatable(AppDomain.CurrentDomain.BaseDirectory + "\\" + docfile.Name + docfile.FileExt, "dt1");
            tempFile = ds.Tables[0];
            return tempFile;
        }

        public string  IsBetween(string currentplan, string currentperiod, string currentrsrc,string SJ)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";//当前周期的开始-结束时间
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' and  '"+SJ+ "'>period_begindate and '"+SJ+ "'<period_enddate";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                return "success";
            }
            else
                return "false";
        }


        public void UpdateSubZQDBJL(string masterid, string proc_guid, string currentplan, string currentperiod, string currentrsrc, Boolean SFGXSJ = false)
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
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";//当前周期的开始-结束时间
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
                string sUpdateSQL = "";
                sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set CompleteDate=null where CompleteDate<convert(datetime,'1950-01-01 23:00:00.000') ";
                dal.Execute(sUpdateSQL);

                sUpdateSQL = "";
                sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set CheckDate=null where CheckDate<convert(datetime,'1950-01-01 23:00:00.000') ";
                dal.Execute(sUpdateSQL);

                //以下这段是为了取最小开始时间，最大完成时间
                string sSQL11 = "select min(CheckDate) as PLN_act_start_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "' and est_wt<>0 and (CompleteDate<='" + period_enddate + "') ";
                DataTable PLN_TaskprocZQ11 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL11);
                string PLN_act_start_date = PLN_TaskprocZQ11.Rows[0]["PLN_act_start_date"].ToString();

                string PLN_act_end_date = "1900-01-01 00:00:00.000";
                Boolean flag = true;
                string sSQL12 = "select est_wt_pct,est_wt,complete_pct,CompleteDate as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "' and est_wt<>0 and (CompleteDate<='" + period_enddate + "') ";
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
                    string sSQL13 = "select max(CompleteDate) as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "' and est_wt<>0 and (CompleteDate<='" + period_enddate + "') ";
                    DataTable PLN_TaskprocZQ13 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL13);
                    if (PLN_TaskprocZQ13.Rows.Count > 0)
                    {
                        PLN_act_end_date = PLN_TaskprocZQ13.Rows[0]["PLN_act_end_date"].ToString();
                    }
                }
                //重新根据工序的完成百分比*权重累加得出对应构件的完成百分比
                float WCBFB = 0;
                for (int y = 0; y < PLN_TaskprocZQ12.Rows.Count; y++)
                {
                    WCBFB += (float.Parse(PLN_TaskprocZQ12.Rows[y]["est_wt_pct"].ToString()) * float.Parse(PLN_TaskprocZQ12.Rows[y]["complete_pct"].ToString())) / 100;
                }

                if (SFGXSJ)//如果是直接修改构件的实际开始、完成时间，则不需要再根据工序的实际开始，实际完成时间计算
                {
                    string updPLN_TaskprocZQ = "update PLN_TaskprocZQ set  complete_pct = '" + WCBFB + "'";
                    updPLN_TaskprocZQ += "where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
                    dal.Execute(updPLN_TaskprocZQ);

                    string updPLN_TaskprocJH = "update PLN_Taskproc set complete_pct = '" + WCBFB + "'";
                    updPLN_TaskprocJH += "where proc_guid='" + proc_guid + "'";
                    dal.Execute(updPLN_TaskprocJH);
                }
                else
                {
                    //根据新的开始时间、完成时间更新对应构件的开始、完成时间和完成百分比
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



                    //更新对应计划的开始时间、完成时间更新对应构件的开始、完成时间和完成百分比
                    string updPLN_TaskprocJH = "update PLN_Taskproc set ";
                    if (PLN_act_end_date != "1900-01-01 00:00:00.000" && PLN_act_start_date != "")
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date='" + PLN_act_end_date + "'";
                        updPLN_TaskprocJH += ",PLN_act_start_date='" + PLN_act_start_date + "' ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    else if (PLN_act_end_date != "1900-01-01 00:00:00.000" && PLN_act_start_date == "")
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date='" + PLN_act_end_date + "'";
                        updPLN_TaskprocJH += ",PLN_act_start_date=null ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    else if (PLN_act_end_date == "1900-01-01 00:00:00.000" && PLN_act_start_date != "")
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date=null";
                        updPLN_TaskprocJH += ",PLN_act_start_date='" + PLN_act_start_date + "' ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    else
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date=null";
                        updPLN_TaskprocJH += ",PLN_act_start_date=null ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    updPLN_TaskprocJH += "where proc_guid='" + proc_guid + "'";
                    dal.Execute(updPLN_TaskprocJH);
                }




                string sSQL = "select task_guid,PLN_act_end_date,PLN_act_start_date,complete_pct from PLN_TaskprocZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (PLN_TaskprocZQ.Rows.Count > 0)
                {
                    #region 根据以上的更新构件的完成百分比，再根据构件的完成百分比更新构件对应作业的累计完成
                    float LJWC = 0;
                    string staks_guid = PLN_TaskprocZQ.Rows[0]["task_guid"].ToString();
                    string sLJWCSQL = "select est_wt_pct,complete_pct from PLN_TaskprocZQ where task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString()
                        + "' and masterid='" + masterid + "'";
                    DataTable LJWCData = XCode.DataAccessLayer.DAL.QuerySQL(sLJWCSQL);
                    for (int r = 0; r < LJWCData.Rows.Count; r++)
                    {
                        LJWC += float.Parse(LJWCData.Rows[r]["est_wt_pct"].ToString()) * (float.Parse(LJWCData.Rows[r]["complete_pct"].ToString()) / 100);
                    }

                    #endregion

                    #region 根据构件的最小实际开始时间，和最大实际开始时间回填作业的实际开始和实际完成时间。

                    sUpdateSQL = "update PLN_TaskprocZQ set PLN_target_end_date=null where PLN_target_end_date='1900-01-01 00:00:00.000'";
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

                    //取最小构件实际开始时间
                    string sGJMin = "select min(PLN_act_start_date) as PLN_act_start_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "'";
                    DataTable GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                    string Minstart_date = GJMin.Rows[0]["PLN_act_start_date"].ToString();
                    if (Minstart_date.Trim().Equals(""))
                    {
                        Minstart_date = "1900-01-01 00:00:00.000";
                    }
                    //取最大构件实际完成时间
                    Boolean flag1 = false;
                    string Maxact_end_date = "1900-01-01 00:00:00.000";
                    sGJMin = "";
                    sGJMin = "select PLN_act_end_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "' ";
                    GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                    for (int t = 0; t < GJMin.Rows.Count; t++)
                    {
                        if (GJMin.Rows[t]["PLN_act_end_date"].ToString().Trim().Equals(""))
                        {
                            flag1 = true;
                            break;
                        }
                    }
                    if (flag1)
                    {
                        Maxact_end_date = "1900-01-01 00:00:00.000";
                    }
                    else
                    {
                        sGJMin = "";
                        sGJMin = "select max(PLN_act_end_date) as PLN_act_end_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "' ";
                        GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                        if (GJMin.Rows.Count > 0)
                            Maxact_end_date = GJMin.Rows[0]["PLN_act_end_date"].ToString();
                    }
                    //计算完最大完成时间和最小开始时间后，根据本周期的开始结束时间判断更新其他字段
                    if (period_enddate < DateTime.Parse(Minstart_date))
                    {
                        Minstart_date = period_enddate.ToString();
                    }
                    if (period_enddate < DateTime.Parse(Maxact_end_date))
                    {
                        Maxact_end_date = "1900-01-01 00:00:00.000";
                    }
                    string updFeedBackRecord_Task = "";
                    if (DateTime.Parse(Maxact_end_date) > DateTime.Parse("1900-01-01 00:00:00.000"))
                    {
                        updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                            "act_end_date='" + Maxact_end_date + "',complete_pct='100',act_start_date='" + Minstart_date + "' " +
                            "where masterid='" + masterid +
                            "' and task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString() + "'";
                        dal.Execute(updFeedBackRecord_Task);
                    }
                    else
                    {
                        if (DateTime.Parse(Minstart_date) > DateTime.Parse("1900-01-01 00:00:00.000"))
                        {
                            updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                                "complete_pct='" + LJWC + "',act_start_date='" + Minstart_date + "' " +
                                "where masterid='" + masterid +
                                "' and task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString() + "'";
                            dal.Execute(updFeedBackRecord_Task);
                        }
                        else
                        {
                            updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                                "complete_pct='" + LJWC + "'" +
                                "where masterid='" + masterid +
                                "' and task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString() + "'";
                            dal.Execute(updFeedBackRecord_Task);
                        }
                    }

                    #endregion

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
                        if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000") && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals(""))
                        && (string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000") || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("")))
                        {
                            KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * (float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString()) / 100);//开工累计实物工程量（元）
                            KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * (float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString()) / 100);//开工累计实物工程量

                            BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                            BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                        }

                        if ((!PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Trim().Equals("1900/1/1 0:00:00"))
                        && (!PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Trim().Equals("1900/1/1 0:00:00")))
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
                dal.Commit();
            }
            catch (Exception ex)
            {
                dal.Rollback();
                throw (ex);
            }
        }



        //导入收入合同
        public string ImportExcelContract(string KeyWord, string KeyValue, string Id)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            System.Data.DataTable ImportData = ReslutData1(KeyWord, KeyValue);
            if (ImportData.Rows.Count > 0)
            {
                foreach (DataRow row in ImportData.Rows)
                {
                    Power.Business.IBusinessOperate BO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_CON_RevenueContract");
                    Power.Business.IBusinessList BOList = BO.FindAll("ConCode='" + row["合同编号"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    //主表
                    Power.Business.IBaseBusiness mainList = Power.Business.BusinessFactory.CreateBusiness("NPS_CON_RevenueContract");
                    mainList.SetItem("ID", Guid.NewGuid());
                    mainList.SetItem("ConCode", row["合同编号"]);
                    mainList.SetItem("ConName", row["合同名称"]);
                    mainList.SetItem("ProjCode", row["项目编号"]);
                    mainList.SetItem("ProjName", row["项目名称"]);
                    mainList.SetItem("ConType", row["合同类型"]);
                    mainList.SetItem("ProjectNature", row["项目性质"]);
                    mainList.SetItem("ConEpsCode", row["立项项目/性质合同编号"]);
                    mainList.SetItem("ConEpsName", row["立项项目/性质合同名称"]);
                    mainList.SetItem("ConMoney", row["合同金额"]);
                    mainList.SetItem("SupConMoney", row["补充合同金额"]);
                    mainList.SetItem("ConTotMoney", row["合同总金额"]);
                    mainList.SetItem("SignDate", row["签订日期"]);
                    mainList.SetItem("PartyA", row["甲方单位"]);
                    mainList.SetItem("PartyAAddress", row["甲方地址"]);
                    mainList.SetItem("PartyAAgent", row["甲方经办人"]);
                    mainList.SetItem("PartyATelephone", row["甲方电话"]);
                    mainList.SetItem("PartyB", row["乙方单位"]);
                    Power.Business.IBusinessOperate Unit = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_CON_UnitRegistration");
                    Power.Business.IBusinessList UnitList = Unit.FindAll("CompanyName='" + row["乙方单位"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (UnitList.Count > 0)
                    {
                        mainList.SetItem("PartyBID", UnitList[0]["ID"]);
                    }
                    mainList.SetItem("PartyBAddress", row["乙方地址"]);
                    mainList.SetItem("PartyBAgent", row["乙方经办人"]);
                    Power.Business.IBusinessOperate Human = Power.Business.BusinessFactory.CreateBusinessOperate("Human");
                    Power.Business.IBusinessList HumanList = Human.FindAll("Name='" + row["乙方经办人"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (HumanList.Count > 0)
                    {
                        mainList.SetItem("PartyBAgentID", HumanList[0]["Id"]);
                        mainList.SetItem("PartyBAgentDept", HumanList[0]["DeptName"]);
                        mainList.SetItem("PartyBAgentDeptID", HumanList[0]["DeptId"]);
                    }
                    mainList.SetItem("PartyBAgentDept", row["部门"]);
                    mainList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                }
            }
            return "导入成功";
        }





        public DataTable ContractDetail(string ConCode)
        {
            string SQL = @"select * from (                            
                            select                             
                            A.ID,A.ConCode,A.ConReviewID,G.ID as BuildID,       
                            --合同状态完结
                            case when G.ID is null then '未立项'
                                when (
                                        (B.NotInvoice != '1' AND isnull(C.InvMoney,0) = isnull(A.ConTotMoney,0)- isnull(E.DeductionAmount,0) )
                                        AND (isnull(D.ActRecMoney,0) = isnull(A.ConTotMoney,0)- isnull(E.DeductionAmount,0))
                                        AND (isnull(B.InfoFee,0) + isnull(B.PrivRefund,0) = isnull(E.CurrentInfoFee,0) + isnull(E.CurrentPrivRefund,0))
                                ) then '已完结'
                                else '执行中'
                                END ConStatus,
                            A.ConEpsCode,A.ConEpsName,A.PartyA,A.PartyAAgent,A.PartyATelephone,A.PartyBAgent,A.PartyB,A.SignDate,A.ConMoney,A.SupConMoney,A.ConTotMoney,                            
                            isnull(B.InfoFee,0) as InfoFee,isnull(B.PrivRefund,0) as PrivRefund,                            
                            --实际收入=合同总金额-应付信息费-应付私退款                            
                            isnull(A.ConTotMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0) as ActSR,case when B.NotInvoice='1' then '否' else '是' end NotInvoice,                            
                            isnull(C.InvMoney,0) as InvMoney,isnull(C.Tax,0) as Tax,isnull(D.ActRecMoney,0) as ActRecMoney,                            
                            isnull(E.CurrentInfoFee,0) as CurrentInfoFee,isnull(E.CurrentPrivRefund,0) as CurrentPrivRefund,isnull(H.Money,0) as PSF,isnull(F.SubConTotMoney,0) as SubConTotMoney,isnull(E.DeductionAmount,0) as DeductionAmount,                            
                            --合同毛利=实际收入-累计税额-累计支付评审费-分包合同总金额-合同扣减金额                            
                            isnull(A.ConTotMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0)-isnull(C.Tax,0)- isnull(H.Money,0) - isnull(F.SubConTotMoney,0)-isnull(E.DeductionAmount,0) as ConML                            
                            from NPS_CON_RevenueContract A                            
                            left join NPS_CON_CloseApplicatio B on A.ID=B.ConID                            
                            left join (select ConID,sum(isnull(InvMoney,0)) as InvMoney,sum(isnull(Tax,0)) as Tax from NPS_CON_ContractInvoiceApplication group by ConID) C on A.ID=C.ConID                            
                            left join (select ConID,sum(isnull(ActRecMoney,0)) as ActRecMoney from NPS_CON_ContractCollection group by ConID) D on A.ID=D.ConID                            
                            left join (select ConID,sum(isnull(CurrentInfoFee,0)) as CurrentInfoFee,sum(isnull(CurrentPrivRefund,0)) as CurrentPrivRefund,                                            
                                sum(isnull(DeductionAmount,0)) as DeductionAmount                                        
                                from NPS_CON_RevenueContract_OtherExpend group by ConID) E on A.ID=E.ConID                             
                            left join (select ConID,sum(isnull(SubConTotMoney,0)) as SubConTotMoney from NPS_CON_Subcontract group by ConID) F on A.ID=F.ConID                            
                            left join NPS_ENVI_Build G on A.ID=G.ConID                            
                            left join (select ConID,sum(isnull(Money,0)) as Money from NPS_HFS_Other_List K
                            left join NPS_HFS_Other L on K.FID=L.ID where L.ConID is not null and K.FeeProject='专家评审费' group by ConID ) H on A.ID=H.ConID                            
                            ) MM                         
                            where ConCode='" + ConCode + "' order by ConEpsCode"
                        ;
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }



        public DataTable ContractBook(string Year, string swhere, string content)
        {
            string SQL = @"select * from (                            
                                    select
                                    --合同状态完结
                                    case when H.ID is null then '未立项'
                                        when (
                                                (J.NotInvoice != '1' AND isnull(D.InvMoney,0) = isnull(B.ConTotMoney,0)- isnull(F.DeductionAmount,0) )
                                                AND (isnull(E.ActRecMoney,0) = isnull(B.ConTotMoney,0)- isnull(F.DeductionAmount,0))
                                                AND (isnull(C.InfoFee,0) + isnull(C.PrivRefund,0) = isnull(F.CurrentInfoFee,0) + isnull(F.CurrentPrivRefund,0))
                                        ) then '已完结'
                                        else '执行中'
                                        END ConStatus,
                                A.ID,A.ConCode,A.ConName,B.PartyA,B.PartyAAgent,B.PartyATelephone,B.PartyBAgent,B.PartyB,B.SignDate,B.ConMoney,B.SupConMoney,B.ConTotMoney,
                                --信息费，私退款
                                isnull(C.InfoFee,0) as InfoFee,isnull(C.PrivRefund,0) as PrivRefund,                    
                                --实际收入=合同总金额-应付信息费-应付私退款                            
                                isnull(B.ConTotMoney,0)-isnull(C.InfoFee,0)-isnull(C.PrivRefund,0) as ActSR,
                                --是否开票    
                                case when J.NotInvoice='1' then '否' else '是' end NotInvoice,
                                --累计已开票金额，累计税额，累计已收款
                                isnull(D.InvMoney,0) as InvMoney,isnull(D.Tax,0) as Tax,isnull(E.ActRecMoney,0) as ActRecMoney,
                                --累计支付信息费，累计支付私退款，累计支付评审费
                                isnull(F.CurrentInfoFee,0) as CurrentInfoFee,isnull(F.CurrentPrivRefund,0) as CurrentPrivRefund,
                                isnull(I.Money,0) as PSF,isnull(G.SubConTotMoney,0) as SubConTotMoney,isnull(F.DeductionAmount,0) as DeductionAmount,
                                --合同毛利=实际收入-累计税额-累计支付评审费-分包合同总金额-合同扣减金额
                                isnull(B.ConTotMoney,0)-isnull(C.InfoFee,0)-isnull(C.PrivRefund,0)-isnull(D.Tax,0)-isnull(I.Money,0)-isnull(G.SubConTotMoney,0)-isnull(F.DeductionAmount,0) ConML
                                from NPS_CON_ContractReview A
                                left join (select * from (
                                              select ROW_NUMBER() over(partition by ConReviewID  order by RegDate desc) RowNum
                                               ,* from NPS_CON_RevenueContract ) as t1  where RowNum = 1
                                          ) B on B.ConReviewID=A.ID       
                                --收入合同辅助管理
                                left join (select ConReviewID,sum(isnull(M.InfoFee,0)) as InfoFee,sum(isnull(M.PrivRefund,0)) as PrivRefund from NPS_CON_CloseApplicatio M
                                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by N.ConReviewID) C on A.ID=C.ConReviewID        
                                left join (select ConReviewID,sum(isnull(InvMoney,0)) as InvMoney,sum(isnull(Tax,0)) as Tax from NPS_CON_ContractInvoiceApplication M
                                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by N.ConReviewID) D on A.ID=D.ConReviewID                             
                                left join (select ConReviewID,sum(isnull(ActRecMoney,0)) as ActRecMoney from NPS_CON_ContractCollection M
                                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by N.ConReviewID) E on A.ID=E.ConReviewID                             
                                left join (select ConReviewID,sum(isnull(CurrentInfoFee,0)) as CurrentInfoFee,sum(isnull(CurrentPrivRefund,0)) as CurrentPrivRefund,sum(isnull(DeductionAmount,0)) as DeductionAmount from NPS_CON_RevenueContract_OtherExpend M
                                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by N.ConReviewID) F on A.ID=F.ConReviewID      
                                left join (select ConReviewID,sum(isnull(SubConTotMoney,0)) as SubConTotMoney from NPS_CON_Subcontract M
                                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by N.ConReviewID) G on A.ID=G.ConReviewID
                                left join (select * from (
                                              select ROW_NUMBER() over(partition by ConReviewID  order by RegDate desc) RowNum
                                               ,* from (
                                                        select M.ID,N.ConReviewID,M.RegDate from NPS_ENVI_Build M left join NPS_CON_RevenueContract N on M.ConID=N.ID
                                                        )  MM
                                              ) as t1  where RowNum = 1
                                          ) H on H.ConReviewID=A.ID      
                                left join (select ConReviewID,sum(isnull(Money,0)) as Money from NPS_HFS_Other_List K
                                                left join NPS_HFS_Other L on K.FID=L.ID
                                                left join NPS_CON_RevenueContract P on L.ConID=P.ID where L.ConID is not null and K.FeeProject='专家评审费' group by P.ConReviewID ) I on A.ID=I.ConReviewID   
                                left join (select * from (
                                              select ROW_NUMBER() over(partition by ConReviewID  order by NotInvoice desc) RowNum
                                               ,* from (
                                                        select M.NotInvoice,N.ConReviewID,M.RegDate from NPS_CON_CloseApplicatio M left join NPS_CON_RevenueContract N on M.ConID=N.ID
                                                        )  MM
                                              ) as t1  where RowNum = 1
                                          ) J on J.ConReviewID=A.ID            
                            ) NN                        
                        where Year(SignDate) = '" + Year + "' and " + swhere + " order by ConCode"
                        ;
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }



        public DataTable SubContractBook(string ConCode)
        {
            string SQL = @"select   A.ID,
                                    case when isnull(B.RecInvMoney,0)=SubConTotMoney and isnull(D.PayMoney,0)=SubConTotMoney then '已完结' else '执行中' END ConStatus,
                                    SubConCode,SubConName,SubConMoney,SupSubConMoney,SubConTotMoney,isnull(B.RecInvMoney,0) as RecInvMoney,isnull(C.TaxAmount,0) as TaxAmount,
                                    isnull(D.PayMoney,0) as PayMoney
                            from NPS_CON_Subcontract A
                            left join (select ConID,sum(isnull(RecInvMoney,0)) as RecInvMoney from NPS_CON_ContractReceipt group by ConID) B on A.ID=B.ConID
                            left join (select ConID,sum(isnull(M.TaxAmount,0)) as TaxAmount from NPS_CON_ContractReceipt_List M
                                                left join NPS_CON_ContractReceipt N on M.FID=N.ID group by N.ConID) C on A.ID=C.ConID
                            left join (select ConID,sum(isnull(PayMoney,0)) as PayMoney from NPS_CON_PayApplication group by ConID) D on A.ID=D.ConID
                            where A.ConCode='" + ConCode + "'"
                        ;
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }


        public DataTable StatisticLeftUp(string Year)
        {
            string SQL = @"select * from (
                                select '累计收入合同额' as Name,sum(isnull(ConMoney,0)) as Money,'-'  percentage,Year(SignDate) as year from NPS_CON_RevenueContract where (Status='35' or Status='50') group by Year(SignDate)
                                union all
                                select '累计应付信息费' as Name,sum(isnull(B.InfoFee,0)) as Money,
                                case when sum(isnull(A.ConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.InfoFee,0)),0)/sum(isnull(A.ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (select sum(isnull(ConMoney,0)) as ConMoney,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by Year(SignDate),ConEpsCode ) A
                                left join (select sum(isnull(InfoFee,0)) as InfoFee,Year(RegDate) as year,ConCode from NPS_CON_CloseApplicatio where (Status='35' or Status='50') group by Year(RegDate),ConCode) B on A.ConEpsCode=B.ConCode
                                group by A.year
                                union all
                                select '累计应付私退款' as Name,sum(isnull(B.PrivRefund,0)) as Money,
                                case when sum(isnull(A.ConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.PrivRefund,0)),0)/sum(isnull(A.ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (select sum(isnull(ConMoney,0)) as ConMoney,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by Year(SignDate),ConEpsCode) A
                                left join (select sum(isnull(PrivRefund,0)) as PrivRefund,Year(RegDate) as year,ConCode from NPS_CON_CloseApplicatio where NotInvoice='0' and (Status='35' or Status='50')  group by Year(RegDate),ConCode) B on A.ConEpsCode=B.ConCode
                                group by A.year
                                union all
                                select '累计实际收入' as Name,sum(isnull(A.ConMoney,0)) - sum(isnull(B.PrivRefund,0)) - sum(isnull(B.InfoFee,0)) as Money,
                                case when sum(isnull(A.ConMoney,0))=0 then '0' else
                                    cast(ROUND(isnull(sum(isnull(A.ConMoney,0)) - sum(isnull(B.PrivRefund,0)) - sum(isnull(B.InfoFee,0)),0)/sum(isnull(A.ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (select sum(isnull(ConMoney,0)) as ConMoney,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by Year(SignDate),ConEpsCode) A
                                left join (select sum(isnull(PrivRefund,0)) as PrivRefund,sum(isnull(InfoFee,0)) as InfoFee,Year(RegDate) as year,ConCode from NPS_CON_CloseApplicatio where  (Status='35' or Status='50') group by Year(RegDate),ConCode) B on A.ConEpsCode=B.ConCode
                                group by A.year
                            ) M
                            where year='" + Year + "'";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }



        public DataTable StatisticLeftDown(string Year)
        {
            string SQL = @"select * from (
                                select '累计收入合同额' as Name,sum(isnull(ConMoney,0)) as Money,'―'  percentage,Year(SignDate) as year from NPS_CON_RevenueContract where (Status=50 or Status=35) group by Year(SignDate)
                                union all
                                select '累计已收款' as Name,sum(isnull(B.ActRecMoney,0)) as Money,
                                case when sum(isnull(A.ConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.ActRecMoney,0)),0)/sum(isnull(A.ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (select sum(isnull(ConMoney,0)) as ConMoney,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract where (Status=50 or Status=35) group by Year(SignDate),ConEpsCode ) A
                                left join (select sum(isnull(ActRecMoney,0)) as ActRecMoney,Year(RegDate) as year,ConCode from NPS_CON_ContractCollection where (Status=50 or Status=35) group by Year(RegDate),ConCode) B on A.ConEpsCode=B.ConCode
                                group by A.year
                                union all
                                select '累计应开票' as Name,sum(isnull(Money,0)) as Money,'―'  percentage,BB.year from
								(
								select sum(isnull(A.ConMoney,0))-sum(isnull(C.ConMoney,0)) as Money,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract A
								left  join ( select sum(isnull(B.ConMoney,0)) as ConMoney,Year(B.RegDate) As RegDate,ConCode from NPS_CON_CloseApplicatio B
								where B.NotInvoice='1' and  (B.Status=50 or B.Status=35) and (B.Status=50 or B.Status=35)
								group by Year(B.RegDate),ConCode
								) C on C.ConCode=A.ConEpsCode
								where (Status=50 or Status=35) group by Year(SignDate),ConEpsCode
								)BB
								group by  BB.year
                                union all
                                select '累计已开票' as Name,sum(isnull(totalmoney,0)) as Money, case when sum(isnull(ConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(totalmoney,0)),0)/sum(isnull(ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,year
								from (
																	select A.ConEpsCode,a.ConEpsName,a.ConTotMoney,a.ConType,a.SignDate,b.totalmoney,b.totalTax,C.ConMoney,Year(A.SignDate) as year from NPS_CON_RevenueContract as A
									left join (select m.ConCode as ConCode ,sum(m.InvMoney) as totalmoney,sum(n.TaxAmount) as totalTax from NPS_CON_ContractInvoiceApplication m
											   left join NPS_CON_ContractInvoiceApplication_List n
											   on m.id=n.fid
											   where (m.Status='50' or m.Status=35) 
											   group by m.ConCode) as B 
									on a.ConEpsCode=b.ConCode
									left join (select sum(isnull(N.ConMoney,0)) as ConMoney,YEAR(M.RegDate) as year,ConEpsCode from NPS_CON_CloseApplicatio M
																			left join NPS_CON_RevenueContract N on M.ConID=N.ID
																		 where   (M.Status=50 )
																		 group by Year(M.RegDate),ConEpsCode) C on C.ConEpsCode=A.ConEpsCode
									where  (a.Status='50' or a.Status=35) 
									)TT
									group by year
                                union all
                                select '累计应付信息费/私退款' as Name,sum(isnull(InfoFee,0))+ sum(isnull(PrivRefund,0)) as Money,'―' percentage,A.year from
                                (
								SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,ConType,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								   left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								   where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								   group by ConType,Year(A.SignDate) 
								) A
                                group by A.year
                                union all
                                select '累计已付信息费/私退款' as Name,sum(isnull(B.InfoFee,0)) as Money,
                                  case when sum(isnull(B.InfoFee,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.InfoFee,0)),0)/sum(isnull(A.InfoFee,0)),2)*100 as nvarchar) + '%' end percentage,A.year from
								  (
									  select sum(isnull(InfoFee,0)) as InfoFee,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract 
									  left join (select sum(isnull(InfoFee+PrivRefund,0)) as InfoFee,YEAR(RegDate) as year,ConCode from NPS_CON_CloseApplicatio where  (Status=50 or Status=35) group by Year(RegDate),ConCode)E on E.ConCode=ConEpsCode
									group by Year(SignDate),ConEpsCode  									  
								  ) A
								  left join ( select sum(isnull(CurrentInfoFee+CurrentPrivRefund,0)) as InfoFee,YEAR(RegDate) as year,ConCode from NPS_CON_RevenueContract_OtherExpend C where  (Status=50 or Status=35)  group by Year(RegDate),ConCode)B on B.ConCode=A.ConEpsCode
								  group by A.year
                                union all
                                select '累计分包合同额' as Name,sum(isnull(SubConMoney,0)) as Money,'―'  percentage,Year(A.SignDate) as year from NPS_CON_RevenueContract A							
							left join NPS_CON_Subcontract B on A.ConEpsCode=B.ConCode
							where  (A.Status=50 or A.Status=35) and (B.Status=50 or B.Status=35)
							group by Year(A.SignDate)
                                union all
                                select '累计分包支付额' as Name,sum(isnull(B.PayMoney,0)) as Money,
                                case when sum(isnull(A.SubConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.PayMoney,0)),0)/sum(isnull(A.SubConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (
								select sum(isnull(SubConMoney,0)) as SubConMoney,Year(SignDate) as year,ConEpsCode from NPS_CON_RevenueContract 
								left join (select sum(isnull(SubConMoney,0)) as SubConMoney,ConCode,Year(SignDate) as year from NPS_CON_Subcontract group by Year(SignDate),ConCode)E on E.ConCode=ConEpsCode
								group by Year(SignDate),ConEpsCode 
								) A								
                                left join (select sum(isnull(PayMoney,0)) as PayMoney,Year(RegDate) as year from NPS_CON_PayApplication group by Year(RegDate)) B on A.year=B.year
                                group by A.year
                            ) P
                            where year='" + Year + "'";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }

        public DataTable StatisticUp(string Year)
        {
            string SQL = @"select * from (
                                select case when A.ConType='HJ' then '环境'
                                            when A.ConType='QT' then '其他'
                                            when A.ConType='ZW' then '职卫非煤'
                                            when A.ConType='ZM' then '职卫煤矿' end ConType,isnull(A.ConMoney,0) as ConMoney,
                                        case when isnull(B.ConMoney,0)=0 then '0' else cast(ROUND(isnull(isnull(A.ConMoney,0),0)/isnull(B.ConMoney,0),2)*100 as nvarchar) + '%' end percentage,
                                        isnull(C.InfoFee,0) as InfoFee,isnull(C.PrivRefund,0) as PrivRefund,
                                        isnull(A.ConMoney,0)-isnull(C.InfoFee,0)-isnull(C.PrivRefund,0) as ActSR,
                                        case when isnull(isnull(B.ConMoney,0)-isnull(D.InfoFee,0)-isnull(D.PrivRefund,0),0)=0 then '0' else cast(ROUND(isnull(isnull(isnull(A.ConMoney,0)-isnull(C.InfoFee,0)-isnull(C.PrivRefund,0),0),0)/isnull(isnull(B.ConMoney,0)-isnull(D.InfoFee,0)-isnull(D.PrivRefund,0),0),2)*100 as nvarchar) + '%' end ActSRpercentage,
                                        A.year
                                 from
                                (select Year(SignDate) as year,ConType,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by Year(SignDate),ConType) A
                                --每年的收入合同总额
                                left join (select Year(SignDate) as year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract where (Status='35' or Status='50') group by Year(SignDate)) B on A.year=B.year
                                --每年的累计应付信息费、私退款总额
                                left join (
								SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,ConType,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								   left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								   where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								   group by ConType,Year(A.SignDate) 
								) D on A.year=D.year and A.ConType=D.ConType
                                --累计每年累计应付信息费、私退款
                                left join (
                                    SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,ConType,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								   left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								   where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								   group by ConType,Year(A.SignDate) 
                                ) C on A.year=C.year and A.ConType=C.ConType
                                union all
                                select '总计' ConType,isnull(A.ConMoney,0) as ConMoney,
                                        case when isnull(B.ConMoney,0)=0 then '-' else '-' end percentage,
                                        C.InfoFee,C.PrivRefund,
                                        A.ConMoney-C.InfoFee-C.PrivRefund as ActSR,
                                        case when isnull(B.ConMoney-D.InfoFee-D.PrivRefund,0)=0 then '-' else '-' end ActSRpercentage,
                                        A.year
                                from
                                (select Year(SignDate) as year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by Year(SignDate)) A
                                --每年的收入合同总额
                                left join (select Year(SignDate) as year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by Year(SignDate)) B on A.year=B.year
                                --每年的累计应付信息费、私退款总额
                                left join (
                                    SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								   left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								   where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								   group by Year(A.SignDate) 
                                ) D on A.year=D.year 
                                --累计每年累计应付信息费、私退款
                                left join (
                                    SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								   left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								   where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								   group by Year(A.SignDate) 
                                ) C on A.year=C.year 
                            ) LL
                            where year='" + Year + "'";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }



        public DataTable ContractML(string Year)
        {
            string SQL = @"select *,concat(round((ML/ReML) * 100,2),'' , '%') as MLpercentage from (
                                select case when A.ConType='HJ' then '环境'
                                                when A.ConType='QT' then '其他'
                                                when A.ConType='ZW' then '职卫非煤'
                                                when A.ConType='ZM' then '职卫煤矿' end ConType,A.year,A.ConMoney,isnull(B.InfoFee,0) as InfoFee,isnull(B.PrivRefund,0) as PrivRefund,isnull(A.ConMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0) as ActSR,isnull(C.Tax,0) as Tax,isnull(D.Money,0) as PSF,
                                isnull(E.SubConMoney,0) SubConMoney,F.DeductionAmount,isnull(A.ConMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0)-isnull(C.Tax,0)-isnull(D.Money,0)-isnull(E.SubConMoney,0)-isnull(F.DeductionAmount,0) as ML from
                                (select ConType,Year(SignDate) year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract where (Status='35' or Status='50')  group by ConType,Year(SignDate)) A
                                left join
                                (
                                   SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,ConType,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								   left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								   where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								   group by ConType,Year(A.SignDate) 
                                ) B on A.year=B.year and A.ConType=B.ConType
                                left join
                                (
                                    SELECT Year(A.SignDate) as year,ConType,sum(Tax) as Tax FROM NPS_CON_RevenueContract A
									left join NPS_CON_ContractInvoiceApplication B on A.ConEpsCode = b.ConCode
									where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50')
									group by ConType,Year(A.SignDate) 
                                ) C on A.year=C.year and A.ConType=C.ConType
                                left join
                                (
                                  select Year(A.SignDate) as year,a.ConType,sum(isnull(Money,0)) as Money from NPS_CON_RevenueContract A
									left join NPS_HFS_Other B on  a.ConEpsCode=b.ConCode
									left join NPS_HFS_Other_List C on C.FID=B.ID
									where (B.Status='50' or B.Status='35') and (A.Status='50' or A.Status='35')and C.FeeProject='专家评审费' 
									group by Year(A.SignDate),a.ConType
                                ) D on A.year=D.year and A.ConType=D.ConType
                                left join
                                (
								  select sum(isnull(SubConMoney,0)) as SubConMoney,Year(A.SignDate) as year,A.ConType from NPS_CON_RevenueContract A							
									left join NPS_CON_Subcontract B on A.ConEpsCode=B.ConCode
									where  (A.Status=50 or A.Status=35) and (B.Status=50 or B.Status=35)
									group by Year(A.SignDate),ConType
                                ) E on A.year=E.year and A.ConType=E.ConType
                                left join
                                (
                                    select sum(isnull(DeductionAmount,0)) as DeductionAmount,Year(A.RegDate) as year,A.ConType from NPS_CON_RevenueContract A
										left join NPS_CON_RevenueContract_OtherExpend B on A.ConEpsCode=B.ConCode
										where (A.Status='35' or A.Status='50') and ((B.Status='35' or B.Status='50') )
										group by Year(A.RegDate),A.ConType 
                                ) F on A.year=F.year and A.ConType=F.ConType
                            ) HH
                            left join(
                                (select 
                                    A.year Reyear,isnull(A.ConMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0)-isnull(C.Tax,0)-isnull(D.Money,0)-isnull(E.SubConMoney,0)-isnull(F.DeductionAmount,0) as ReML from 
                                (select  Year(SignDate) year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract  where (Status='35' or Status='50')  group by Year(SignDate)) A
                                left join(SELECT sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(A.SignDate) as year FROM NPS_CON_RevenueContract A
								left join NPS_CON_CloseApplicatio B on A.ConEpsCode = b.ConCode
								where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50') 
								group by year(A.SignDate)) B on A.year=B.year 
                                left join(SELECT Year(A.SignDate) as year,sum(Tax) as Tax FROM NPS_CON_RevenueContract A
									left join NPS_CON_ContractInvoiceApplication B on A.ConEpsCode = b.ConCode
									where (A.Status='35' or A.Status='50') and (B.Status='35' or B.Status='50')
									group by Year(A.SignDate)) C on A.year=C.year 
                                left join(
								select Year(A.SignDate) as year,sum(isnull(Money,0)) as Money from NPS_CON_RevenueContract A
									left join NPS_HFS_Other B on  a.ConEpsCode=b.ConCode
									left join NPS_HFS_Other_List C on C.FID=B.ID
									where (B.Status='50' or B.Status='35')and (A.Status='50' or A.Status='35') and C.FeeProject='专家评审费' 
									group by Year(A.SignDate)
								) D on A.year=D.year  
                                left join(select sum(isnull(SubConMoney,0)) as SubConMoney,Year(A.SignDate) as year from NPS_CON_RevenueContract A							
									left join NPS_CON_Subcontract B on A.ConEpsCode=B.ConCode
									where  (A.Status=50 or A.Status=35) and (B.Status=50 or B.Status=35)
									group by Year(A.SignDate)) E on A.year=E.year
                                left join(
								 select sum(isnull(DeductionAmount,0)) as DeductionAmount,Year(A.RegDate) as year from NPS_CON_RevenueContract A
										left join NPS_CON_RevenueContract_OtherExpend B on A.ConEpsCode=B.ConCode
										where (A.Status='35' or A.Status='50') and ((B.Status='35' or B.Status='50') )
										group by Year(A.RegDate)
								) F on A.year=F.year) 
                                ) X on X.Reyear = HH.year
                            where year='" + Year + "'";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
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

        public string GetMaxXH(string ID)
        {
            string XH = "";
            string sSQL = "select max(XH) as XH from NPS_SUBCON_FWPOMX where id='"+ID+"'";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (CwData.Rows.Count > 0)
            {
                XH = (int.Parse(CwData.Rows[0]["XH"].ToString()) + 1).ToString();
            }
            else
            {
                XH = "1";
            }
            return XH;
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
                        float complete_pct = 0;
                        string sSQL3 = "select sum(complete_pct*est_wt_pct) as complete_pct from PS_PLN_TaskProc_Sub where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "' group by proc_guid";
                        DataTable PS_PLN_TaskProc_Sub = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        if (PS_PLN_TaskProc_Sub.Rows.Count > 0)
                        {
                            if (PS_PLN_TaskProc_Sub.Rows[0]["complete_pct"].ToString().Trim().Equals(""))
                            {
                                complete_pct = 0;
                            }
                            else
                                complete_pct = float.Parse(PS_PLN_TaskProc_Sub.Rows[0]["complete_pct"].ToString());
                        }
                        else if (PS_PLN_TaskProc_Sub.Rows.Count == 0)
                        {
                            if (TaskProcData.Rows[j]["PLN_act_end_date"].ToString().Trim().Equals("") || string.IsNullOrEmpty(TaskProcData.Rows[j]["PLN_act_end_date"].ToString()))
                            {
                                complete_pct = 0;
                            }
                            else
                            {
                                complete_pct = 100;
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

                        string sSQL2 = "";
                        sSQL2 = "insert into PLN_TaskprocZQ ";
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


                        //Power.Business.IBaseBusiness NewMain = Power.Business.BusinessFactory.CreateBusiness("PLN_TaskprocZQ");
                        //NewMain.SetItem("proj_id", TaskProcData.Rows[j]["proj_id"].ToString());
                        //NewMain.SetItem("wbs_id", TaskProcData.Rows[j]["wbs_id"].ToString());
                        //NewMain.SetItem("task_id", TaskProcData.Rows[j]["task_id"].ToString());
                        //NewMain.SetItem("task_guid", TaskProcData.Rows[j]["task_guid"].ToString());
                        //NewMain.SetItem("seq_num", TaskProcData.Rows[j]["seq_num"].ToString());
                        //NewMain.SetItem("CompleteOrNot", TaskProcData.Rows[j]["CompleteOrNot"].ToString());
                        //NewMain.SetItem("proc_name", TaskProcData.Rows[j]["proc_name"].ToString());
                        //NewMain.SetItem("proc_descri", TaskProcData.Rows[j]["proc_descri"].ToString());
                        //NewMain.SetItem("est_wt", TaskProcData.Rows[j]["est_wt"].ToString());
                        //NewMain.SetItem("complete_pct", complete_pct);
                        //NewMain.SetItem("act_end_date", TaskProcData.Rows[j]["act_end_date"].ToString());
                        //NewMain.SetItem("SysOrNot", TaskProcData.Rows[j]["SysOrNot"].ToString());
                        //NewMain.SetItem("target_end_date_lag", TaskProcData.Rows[j]["target_end_date_lag"].ToString());
                        //NewMain.SetItem("expect_end_date_lag", TaskProcData.Rows[j]["expect_end_date_lag"].ToString());
                        //NewMain.SetItem("rsrc_id", TaskProcData.Rows[j]["rsrc_id"].ToString());
                        //NewMain.SetItem("temp_id", TaskProcData.Rows[j]["temp_id"].ToString());
                        //NewMain.SetItem("update_date", TaskProcData.Rows[j]["update_date"].ToString());
                        //NewMain.SetItem("p3ec_proc_id", TaskProcData.Rows[j]["p3ec_proc_id"].ToString());
                        //NewMain.SetItem("p3ec_flag", TaskProcData.Rows[j]["p3ec_flag"].ToString());
                        //NewMain.SetItem("proc_guid", TaskProcData.Rows[j]["proc_guid"].ToString());
                        //NewMain.SetItem("proj_guid", TaskProcData.Rows[j]["proj_guid"].ToString());
                        //NewMain.SetItem("plan_guid", TaskProcData.Rows[j]["plan_guid"].ToString());
                        //NewMain.SetItem("plan_id", TaskProcData.Rows[j]["plan_id"].ToString());
                        //NewMain.SetItem("wbs_guid", TaskProcData.Rows[j]["wbs_guid"].ToString());
                        //NewMain.SetItem("rsrc_guid", TaskProcData.Rows[j]["rsrc_guid"].ToString());
                        //NewMain.SetItem("temp_guid", TaskProcData.Rows[j]["temp_guid"].ToString());
                        //NewMain.SetItem("est_wt_pct", TaskProcData.Rows[j]["est_wt_pct"].ToString());
                        //NewMain.SetItem("keyword", TaskProcData.Rows[j]["keyword"].ToString());
                        //NewMain.SetItem("formid", TaskProcData.Rows[j]["formid"].ToString());
                        //NewMain.SetItem("update_user", TaskProcData.Rows[j]["update_user"].ToString());
                        //NewMain.SetItem("create_date", TaskProcData.Rows[j]["create_date"].ToString());
                        //NewMain.SetItem("create_user", TaskProcData.Rows[j]["create_user"].ToString());
                        //NewMain.SetItem("delete_session_id", TaskProcData.Rows[j]["delete_session_id"].ToString());
                        //NewMain.SetItem("delete_date", TaskProcData.Rows[j]["delete_date"].ToString());
                        //NewMain.SetItem("target_end_date", TaskProcData.Rows[j]["target_end_date"].ToString());
                        //NewMain.SetItem("proc_code", TaskProcData.Rows[j]["proc_code"].ToString());
                        //NewMain.SetItem("PLN_target_start_date", TaskProcData.Rows[j]["PLN_target_start_date"].ToString());
                        //NewMain.SetItem("PLN_target_end_date", TaskProcData.Rows[j]["PLN_target_end_date"].ToString());
                        //NewMain.SetItem("PLN_act_end_date", TaskProcData.Rows[j]["PLN_act_end_date"].ToString());
                        //NewMain.SetItem("PLN_act_start_date", TaskProcData.Rows[j]["PLN_act_start_date"].ToString());
                        //NewMain.SetItem("masterid", masterid);
                        //NewMain.Save(System.ComponentModel.DataObjectMethodType.Insert);


                        sSQL3 = "";
                        sSQL3 = "select ProcSub_guid,ProcSub_Name,proj_guid,plan_guid,wbs_guid,task_guid,proc_guid,seq_num,est_wt,est_wt_pct,complete_pct,";
                        sSQL3 += "      temp_guid,remark,RegDate,RegHumName,RegHumId,UpdHumId,UpdHuman,UpdDate,CheckDate,SubState,CompleteDate,ProcSub_Code,target_end_date,act_end_date";
                        sSQL3 += " from PS_PLN_TaskProc_Sub ";
                        sSQL3 += "where proc_guid='" + TaskProcData.Rows[j]["proc_guid"].ToString() + "'";
                        PS_PLN_TaskProc_Sub = XCode.DataAccessLayer.DAL.QuerySQL(sSQL3);
                        for (int k = 0; k < PS_PLN_TaskProc_Sub.Rows.Count; k++)
                        {
                            string WCBFB = "0";
                            string SubState = "";
                            string begindate = "";
                            string enddate = "";
                            if (PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString().Trim() == "")
                            {
                                begindate = "1900-01-01";
                            }
                            else
                                begindate = PS_PLN_TaskProc_Sub.Rows[k]["CheckDate"].ToString();
                            if (PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString().Trim().Equals(""))
                            {
                                enddate = "1900-01-01";
                            }
                            else
                                enddate = PS_PLN_TaskProc_Sub.Rows[k]["CompleteDate"].ToString();
                            if (PS_PLN_TaskProc_Sub.Rows[k]["complete_pct"].ToString() == "" || float.Parse(PS_PLN_TaskProc_Sub.Rows[k]["complete_pct"].ToString()) == 0)
                            {
                                //第一种情况，开始时间在周期时间之前且在周期完成时间之内
                                if (DateTime.Parse(begindate) < period_begindate && DateTime.Parse(enddate) > period_begindate && DateTime.Parse(enddate) < period_enddate)
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if (DateTime.Parse(begindate) < period_begindate && DateTime.Parse(enddate) > period_enddate)//第二种情况，开始时间在周期开始时间之前且完成时间在周期完成时间之后
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if (DateTime.Parse(begindate) < period_begindate && DateTime.Parse(enddate) < period_enddate)//第三种情况，开始时间和结束时间都在周期开始时间之前
                                {
                                    SubState = "已完成";
                                    WCBFB = "100";
                                }
                                else if ((DateTime.Parse(begindate) > period_begindate && DateTime.Parse(begindate) < period_enddate) &&
                                          DateTime.Parse(enddate) > period_begindate && DateTime.Parse(enddate) < period_enddate)//第四种情况，开始时间和结束时间都在周期开始与完成时间之内
                                {
                                    SubState = "已完成";
                                    WCBFB = "100";
                                }
                                else if ((DateTime.Parse(begindate) > period_begindate && DateTime.Parse(begindate) < period_enddate) && DateTime.Parse(enddate) > period_enddate)//第五种情况，开始时间在周期开始、完成时间之内，结束时间在周期结束时间之后
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if (DateTime.Parse(begindate) > period_enddate && DateTime.Parse(enddate) > period_enddate)//第六种情况，开始时间和完成时间都在周期结束时间之后
                                {
                                    SubState = "未开始";
                                    WCBFB = "0";
                                }
                                else if ((begindate != "1900-01-01") && (enddate == "1900-01-01"))//有开始时间没有完成时间
                                {
                                    SubState = "已开始";
                                    WCBFB = "0";
                                }
                                else if ((begindate == "1900-01-01") && (enddate == "1900-01-01"))
                                {
                                    SubState = "未开始";
                                    WCBFB = "0";
                                }
                            }
                            else
                            {
                                WCBFB = PS_PLN_TaskProc_Sub.Rows[k]["complete_pct"].ToString();
                                //第一种情况，开始时间在周期时间之前且在周期完成时间之内
                                if (DateTime.Parse(begindate) < period_begindate && DateTime.Parse(enddate) > period_begindate && DateTime.Parse(enddate) < period_enddate)
                                {
                                    SubState = "已开始";
                                }
                                else if (DateTime.Parse(begindate) < period_begindate && DateTime.Parse(enddate) > period_enddate)//第二种情况，开始时间在周期开始时间之前且完成时间在周期完成时间之后
                                {
                                    SubState = "已开始";
                                }
                                else if (DateTime.Parse(begindate) < period_begindate && DateTime.Parse(enddate) < period_enddate)//第三种情况，开始时间和结束时间都在周期开始时间之前
                                {
                                    SubState = "已完成";
                                }
                                else if ((DateTime.Parse(begindate) > period_begindate && DateTime.Parse(begindate) < period_enddate) &&
                                          DateTime.Parse(enddate) > period_begindate && DateTime.Parse(enddate) < period_enddate)//第四种情况，开始时间和结束时间都在周期开始与完成时间之内
                                {
                                    SubState = "已完成";
                                }
                                else if ((DateTime.Parse(begindate) > period_begindate && DateTime.Parse(begindate) < period_enddate) && DateTime.Parse(enddate) > period_enddate)//第五种情况，开始时间在周期开始、完成时间之内，结束时间在周期结束时间之后
                                {
                                    SubState = "已开始";
                                }
                                else if (DateTime.Parse(begindate) > period_enddate && DateTime.Parse(enddate) > period_enddate)//第六种情况，开始时间和完成时间都在周期结束时间之后
                                {
                                    SubState = "未开始";
                                }
                                else if ((begindate != "1900-01-01") && (enddate == "1900-01-01"))//有开始时间没有完成时间
                                {
                                    SubState = "已开始";
                                }

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
                                KGLJGCL = float.Parse(NPS_BOQZQ.Rows[h]["ListingNum"].ToString()) * (float.Parse(TaskProcData.Rows[j]["complete_pct"].ToString()) / 100);
                                KGLJJE = float.Parse(NPS_BOQZQ.Rows[h]["ListingPrice"].ToString()) * (float.Parse(TaskProcData.Rows[j]["complete_pct"].ToString()) / 100);
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
                                if (DateTime.Parse(NPS_BOQZQ.Rows[h]["PlanSatrt"].ToString()).Month.Equals(period_begindate.Month))
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


                        }
                        UpdateSubZQ(currentfeedback, TaskProcData.Rows[j]["proc_guid"].ToString(), currentplan, currentperiod, currentrsrc);
                    }
                    UpFeedBackRecord(task_guid, currentfeedback, currentplan, currentperiod, currentrsrc);
                }
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
                sUpdateSQL = "";
                sUpdateSQL = "update PS_PLN_FeedBackRecord_Task set act_start_date=null where act_start_date <convert(datetime,'1950-01-01 23:00:00.000') ";
                dal.Execute(sUpdateSQL);
                dal.Commit();
            }
            catch (Exception ex)
            {
                dal.Rollback();
                throw (ex);
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
                        KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * (float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString()) / 100);//开工累计实物工程量（元）
                        KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * (float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString()) / 100);//开工累计实物工程量

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
                dal.Execute("update PS_PLN_TaskProc_SubZQ  set SubState='已完成', complete_pct='100.00', completedate='" + JHKSSJ + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
            }

        }

        public void UpFeedBackRecord(string staks_guid, string masterid, string currentplan, string currentperiod, string currentrsrc, Boolean SFGXSJ = false)
        {
            #region 根据构件的最小实际开始时间，和最大实际开始时间回填作业的实际开始和实际完成时间。
            
            DateTime period_begindate = new DateTime();
            DateTime period_enddate = new DateTime();
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";//当前周期的开始-结束时间
            sFeedBackSQL += "plan_guid='" + Guid.Parse(currentplan) + "' and period_guid='" + Guid.Parse(currentperiod) + "' and RegHumId='" + Guid.Parse(currentrsrc) + "' ";
            DataTable FeedBackData = XCode.DataAccessLayer.DAL.QuerySQL(sFeedBackSQL);
            if (FeedBackData.Rows.Count > 0)
            {
                period_begindate = Convert.ToDateTime(FeedBackData.Rows[0]["period_begindate"].ToString());
                period_enddate = Convert.ToDateTime(FeedBackData.Rows[0]["period_enddate"].ToString());
            }
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
            #endregion
            //取最小构件实际开始时间
            string sGJMin = "select min(PLN_act_start_date) as PLN_act_start_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "'";
            DataTable GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
            string Minstart_date = GJMin.Rows[0]["PLN_act_start_date"].ToString();
            if (Minstart_date.Trim().Equals(""))
            {
                Minstart_date = "1900-01-01 00:00:00.000";
            }
            //取最大构件实际完成时间
            Boolean flag1 = false;
            string Maxact_end_date = "1900-01-01 00:00:00.000";
            sGJMin = "";
            sGJMin = "select PLN_act_end_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "' ";
            GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
            for (int t = 0; t < GJMin.Rows.Count; t++)
            {
                if (GJMin.Rows[t]["PLN_act_end_date"].ToString().Trim().Equals(""))
                {
                    flag1 = true;
                    break;
                }
            }
            if (flag1)
            {
                Maxact_end_date = "1900-01-01 00:00:00.000";
            }
            else
            {
                sGJMin = "";
                sGJMin = "select max(PLN_act_end_date) as PLN_act_end_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "' ";
                GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                if (GJMin.Rows.Count > 0)
                    Maxact_end_date = GJMin.Rows[0]["PLN_act_end_date"].ToString();
            }
            //计算完最大完成时间和最小开始时间后，根据本周期的开始结束时间判断更新其他字段
            if (period_enddate < DateTime.Parse(Minstart_date))
            {
                Minstart_date = period_enddate.ToString();
            }
            if (period_enddate < DateTime.Parse(Maxact_end_date))
            {
                Maxact_end_date = "1900-01-01 00:00:00.000";
            }
            string updFeedBackRecord_Task = "";
            float LJWC = 0;
            string sLJWCSQL = "select est_wt_pct,complete_pct from PLN_TaskprocZQ where task_guid='" + staks_guid
                + "' and masterid='" + masterid + "'";
            DataTable LJWCData = XCode.DataAccessLayer.DAL.QuerySQL(sLJWCSQL);
            for (int r = 0; r < LJWCData.Rows.Count; r++)
            {
                LJWC += float.Parse(LJWCData.Rows[r]["est_wt_pct"].ToString()) * (float.Parse(LJWCData.Rows[r]["complete_pct"].ToString()) / 100);
            }
            if (DateTime.Parse(Maxact_end_date) > DateTime.Parse("1900-01-01 00:00:00.000"))
            {
                updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                    "act_end_date='" + Maxact_end_date + "',complete_pct='100',act_start_date='" + Minstart_date + "' " +
                    "where masterid='" + masterid +
                    "' and task_guid='" + staks_guid + "'";
                dal.Execute(updFeedBackRecord_Task);
            }
            else
            {
                if (DateTime.Parse(Minstart_date) > DateTime.Parse("1900-01-01 00:00:00.000"))
                {
                    updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                        "complete_pct='" + LJWC + "',act_start_date='" + Minstart_date + "' " +
                        "where masterid='" + masterid +
                        "' and task_guid='" + staks_guid + "'";
                    dal.Execute(updFeedBackRecord_Task);
                }
                else
                {
                    updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                        "complete_pct='" + LJWC + "'" +
                        "where masterid='" + masterid +
                        "' and task_guid='" + staks_guid + "'";
                    dal.Execute(updFeedBackRecord_Task);
                }
            }

            #endregion
        }

        public void UpdateSubZQ(string masterid, string proc_guid, string currentplan, string currentperiod, string currentrsrc, Boolean SFGXSJ = false)
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
            string sFeedBackSQL = "Select period_begindate,period_enddate From  PS_PLN_FeedBackRecord where ";//当前周期的开始-结束时间
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
                string sUpdateSQL = "";
                sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set CompleteDate=null where CompleteDate<convert(datetime,'1950-01-01 23:00:00.000') ";
                dal.Execute(sUpdateSQL);

                sUpdateSQL = "";
                sUpdateSQL = "update PS_PLN_TaskProc_SubZQ set CheckDate=null where CheckDate<convert(datetime,'1950-01-01 23:00:00.000') ";
                dal.Execute(sUpdateSQL);

                //以下这段是为了取最小开始时间，最大完成时间
                string sSQL11 = "select min(CheckDate) as PLN_act_start_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "' and est_wt<>0 and (CompleteDate>'"+ period_begindate + "' and CompleteDate<'"+ period_enddate + "')";
                DataTable PLN_TaskprocZQ11 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL11);
                string PLN_act_start_date = PLN_TaskprocZQ11.Rows[0]["PLN_act_start_date"].ToString();

                string PLN_act_end_date = "1900-01-01 00:00:00.000";
                Boolean flag = true;
                string sSQL12 = "select est_wt_pct,est_wt,complete_pct,CompleteDate as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "' and est_wt<>0 and (CompleteDate>'" + period_begindate + "' and CompleteDate<'" + period_enddate + "')";
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
                    string sSQL13 = "select max(CompleteDate) as PLN_act_end_date from PS_PLN_TaskProc_SubZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "' and est_wt<>0 and (CompleteDate>'" + period_begindate + "' and CompleteDate<'" + period_enddate + "') ";
                    DataTable PLN_TaskprocZQ13 = XCode.DataAccessLayer.DAL.QuerySQL(sSQL13);
                    if (PLN_TaskprocZQ13.Rows.Count > 0)
                    {
                        PLN_act_end_date = PLN_TaskprocZQ13.Rows[0]["PLN_act_end_date"].ToString();
                    }
                }
                //重新根据工序的完成百分比*权重累加得出对应构件的完成百分比
                float WCBFB = 0;
                for (int y = 0; y < PLN_TaskprocZQ12.Rows.Count; y++)
                {
                    WCBFB += (float.Parse(PLN_TaskprocZQ12.Rows[y]["est_wt_pct"].ToString()) * float.Parse(PLN_TaskprocZQ12.Rows[y]["complete_pct"].ToString())) / 100;
                }

                if (SFGXSJ)//如果是直接修改构件的实际开始、完成时间，则不需要再根据工序的实际开始，实际完成时间计算
                {
                    string updPLN_TaskprocZQ = "update PLN_TaskprocZQ set  complete_pct = '" + WCBFB + "'";
                    updPLN_TaskprocZQ += "where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
                    dal.Execute(updPLN_TaskprocZQ);

                    string updPLN_TaskprocJH = "update PLN_Taskproc set complete_pct = '" + WCBFB + "'";
                    updPLN_TaskprocJH += "where proc_guid='" + proc_guid + "'";
                    dal.Execute(updPLN_TaskprocJH);
                }
                else
                {
                    //根据新的开始时间、完成时间更新对应构件的开始、完成时间和完成百分比
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



                    //更新对应计划的开始时间、完成时间更新对应构件的开始、完成时间和完成百分比
                    string updPLN_TaskprocJH = "update PLN_Taskproc set ";
                    if (PLN_act_end_date != "1900-01-01 00:00:00.000" && PLN_act_start_date != "")
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date='" + PLN_act_end_date + "'";
                        updPLN_TaskprocJH += ",PLN_act_start_date='" + PLN_act_start_date + "' ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    else if (PLN_act_end_date != "1900-01-01 00:00:00.000" && PLN_act_start_date == "")
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date='" + PLN_act_end_date + "'";
                        updPLN_TaskprocJH += ",PLN_act_start_date=null ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    else if (PLN_act_end_date == "1900-01-01 00:00:00.000" && PLN_act_start_date != "")
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date=null";
                        updPLN_TaskprocJH += ",PLN_act_start_date='" + PLN_act_start_date + "' ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    else
                    {
                        updPLN_TaskprocJH += "PLN_act_end_date=null";
                        updPLN_TaskprocJH += ",PLN_act_start_date=null ";
                        updPLN_TaskprocJH += ",complete_pct='" + WCBFB + "'";
                    }
                    updPLN_TaskprocJH += "where proc_guid='" + proc_guid + "'";
                    dal.Execute(updPLN_TaskprocJH);
                }




                string sSQL = "select task_guid,PLN_act_end_date,PLN_act_start_date,complete_pct from PLN_TaskprocZQ where proc_guid='" + proc_guid + "' and masterid='" + masterid + "'";
                DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
                if (PLN_TaskprocZQ.Rows.Count > 0)
                {
                    #region 根据以上的更新构件的完成百分比，再根据构件的完成百分比更新构件对应作业的累计完成
                    float LJWC = 0;
                    string staks_guid = PLN_TaskprocZQ.Rows[0]["task_guid"].ToString();
                    string sLJWCSQL = "select est_wt_pct,complete_pct from PLN_TaskprocZQ where task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString()
                        + "' and masterid='" + masterid + "'";
                    DataTable LJWCData = XCode.DataAccessLayer.DAL.QuerySQL(sLJWCSQL);
                    for (int r = 0; r < LJWCData.Rows.Count; r++)
                    {
                        LJWC += float.Parse(LJWCData.Rows[r]["est_wt_pct"].ToString()) * (float.Parse(LJWCData.Rows[r]["complete_pct"].ToString()) / 100);
                    }

                    #endregion

                    #region 根据构件的最小实际开始时间，和最大实际开始时间回填作业的实际开始和实际完成时间。

                    //string sUpdateSQL = "update PLN_TaskprocZQ set PLN_target_end_date=null where PLN_target_end_date='1900-01-01 00:00:00.000'";
                    //dal.Execute(sUpdateSQL);
                    //sUpdateSQL = "";
                    //sUpdateSQL = "update PLN_TaskprocZQ set PLN_target_start_date=null where PLN_target_start_date='1900-01-01 00:00:00.000'";
                    //dal.Execute(sUpdateSQL);
                    //sUpdateSQL = "";
                    //sUpdateSQL = "update PLN_TaskprocZQ set PLN_act_end_date=null where PLN_act_end_date='1900-01-01 00:00:00.000'";
                    //dal.Execute(sUpdateSQL);
                    //sUpdateSQL = "";
                    //sUpdateSQL = "update PLN_TaskprocZQ set PLN_act_start_date=null where PLN_act_start_date='1900-01-01 00:00:00.000'";
                    //dal.Execute(sUpdateSQL);

                    ////取最小构件实际开始时间
                    //string sGJMin = "select min(PLN_act_start_date) as PLN_act_start_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "'";
                    //DataTable GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                    //string Minstart_date = GJMin.Rows[0]["PLN_act_start_date"].ToString();
                    //if (Minstart_date.Trim().Equals(""))
                    //{
                    //    Minstart_date = "1900-01-01 00:00:00.000";
                    //}
                    ////取最大构件实际完成时间
                    //Boolean flag1 = false;
                    //string Maxact_end_date = "1900-01-01 00:00:00.000";
                    //sGJMin = "";
                    //sGJMin = "select PLN_act_end_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "' ";
                    //GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                    //for (int t = 0; t < GJMin.Rows.Count; t++)
                    //{
                    //    if (GJMin.Rows[t]["PLN_act_end_date"].ToString().Trim().Equals(""))
                    //    {
                    //        flag1 = true;
                    //        break;
                    //    }
                    //}
                    //if (flag1)
                    //{
                    //    Maxact_end_date = "1900-01-01 00:00:00.000";
                    //}
                    //else
                    //{
                    //    sGJMin = "";
                    //    sGJMin = "select max(PLN_act_end_date) as PLN_act_end_date from PLN_taskProcZQ where task_guid='" + staks_guid + "' and masterid='" + masterid + "' ";
                    //    GJMin = XCode.DataAccessLayer.DAL.QuerySQL(sGJMin);
                    //    if (GJMin.Rows.Count > 0)
                    //        Maxact_end_date = GJMin.Rows[0]["PLN_act_end_date"].ToString();
                    //}
                    ////计算完最大完成时间和最小开始时间后，根据本周期的开始结束时间判断更新其他字段
                    //if (period_enddate < DateTime.Parse(Minstart_date))
                    //{
                    //    Minstart_date = period_enddate.ToString();
                    //}
                    //if (period_enddate < DateTime.Parse(Maxact_end_date))
                    //{
                    //    Maxact_end_date = "1900-01-01 00:00:00.000";
                    //}
                    //string updFeedBackRecord_Task = "";
                    //if (DateTime.Parse(Maxact_end_date) > DateTime.Parse("1900-01-01 00:00:00.000"))
                    //{
                    //    updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                    //        "act_end_date='" + Maxact_end_date + "',complete_pct='100',act_start_date='" + Minstart_date + "' " +
                    //        "where masterid='" + masterid +
                    //        "' and task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString() + "'";
                    //    dal.Execute(updFeedBackRecord_Task);
                    //}
                    //else
                    //{
                    //    if (DateTime.Parse(Minstart_date) > DateTime.Parse("1900-01-01 00:00:00.000"))
                    //    {
                    //        updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                    //            "complete_pct='" + LJWC + "',act_start_date='" + Minstart_date + "' " +
                    //            "where masterid='" + masterid +
                    //            "' and task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString() + "'";
                    //        dal.Execute(updFeedBackRecord_Task);
                    //    }
                    //    else
                    //    {
                    //        updFeedBackRecord_Task = "update PS_PLN_FeedBackRecord_Task set " +
                    //            "complete_pct='" + LJWC + "'" +
                    //            "where masterid='" + masterid +
                    //            "' and task_guid='" + PLN_TaskprocZQ.Rows[0]["task_guid"].ToString() + "'";
                    //        dal.Execute(updFeedBackRecord_Task);
                    //    }
                    //}

                    #endregion

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
                        if ((!string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString()) && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals("1900-01-01 00:00:00.000") && !PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Equals(""))
                        && (string.IsNullOrEmpty(PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString()) || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("1900-01-01 00:00:00.000") || PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Equals("")))
                        {
                            KGLJGCJE = float.Parse(NPS_BOQZQ.Rows[i]["ListingPrice"].ToString()) * (float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString()) / 100);//开工累计实物工程量（元）
                            KGLJGCL = float.Parse(NPS_BOQZQ.Rows[i]["ListingNum"].ToString()) * (float.Parse(PLN_TaskprocZQ.Rows[0]["complete_pct"].ToString()) / 100);//开工累计实物工程量

                            BYSJGCJE = KGLJGCJE - SQKGLJJE;//挂接金额*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。      
                            BYSJGCL = KGLJGCL - SQKGLJGCL;//挂接数量*构件完成百分比-周期反馈区间后一个日期上月最后一期的开工累计实物量。
                        }

                        if ((!PLN_TaskprocZQ.Rows[0]["PLN_act_start_date"].ToString().Trim().Equals("1900/1/1 0:00:00"))
                        && (!PLN_TaskprocZQ.Rows[0]["PLN_act_end_date"].ToString().Trim().Equals("1900/1/1 0:00:00")))
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
                dal.Commit();
            }
            catch (Exception ex)
            {
                dal.Rollback();
                throw (ex);
            }
        }
        public void UpCompletepct(string sCS)//工序的完成百分比点击事件，更新完成百分比
        {
            string sGuid = sCS.Split('|')[0];
            float complete_pct = float.Parse(sCS.Split('|')[1]);
            string Masterid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.Execute("update PS_PLN_TaskProc_Sub  set complete_pct='" + complete_pct + "' where ProcSub_guid = '" + sGuid + "' ");
            dal.Execute("update PS_PLN_TaskProc_SubZQ  set complete_pct='" + complete_pct + "' where ProcSub_guid = '" + sGuid + "'and Masterid='" + Masterid + "' ");
        }
        public void Upest_wt_pct(string sCS)//工序的权重点击事件，更新权重百分比
        {
            string sGuid = sCS.Split('|')[0];
            float est_wt = float.Parse(sCS.Split('|')[1]);
            string Masterid = sCS.Split('|')[2];
            string ProcSub_guid = sCS.Split('|')[3];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.Execute("update PS_PLN_TaskProc_SubZQ set est_wt='" + est_wt + "' where masterid='" + Masterid + "' and ProcSub_guid='" + ProcSub_guid + "'");
            dal.Execute("update PS_PLN_TaskProc_SubZQ set est_wt_pct=(round(CONVERT(float,est_wt)/(select sum(A.est_wt) from PS_PLN_TaskProc_SubZQ A where A.proc_guid=B.proc_guid),4))*100 from PS_PLN_TaskProc_SubZQ B" +
                " where B.proc_guid in (select proc_guid from PS_PLN_TaskProc_SubZQ group by proc_guid,masterid)" +
                "and masterid='" + Masterid + "' and proc_guid='" + sGuid + "'");

            dal.Execute("update  PS_PLN_TaskProc_SubZQ set  est_wt_pct = round((select est_wt_pct from PS_PLN_TaskProc_SubZQ C where C.proc_guid=A.proc_guid and proc_id=(select min(proc_id) " +
                "from PS_PLN_TaskProc_SubZQ D where A.proc_guid=D.proc_guid ))" +
                "+(select 100-sum(B.est_wt_pct) from PS_PLN_TaskProc_SubZQ B where A.proc_guid=B.proc_guid),2)" +
                "from PS_PLN_TaskProc_SubZQ A " +
                " where A.proc_guid in (select proc_guid from PS_PLN_TaskProc_SubZQ group by proc_guid,masterid) " +
                "and proc_id=(select min(proc_id) from PS_PLN_TaskProc_SubZQ D where A.proc_guid=D.proc_guid )" +
                "and masterid='" + Masterid + "' and proc_guid='" + sGuid + "'");
        }
        public void UpGJest_wt_pct(string sCS)//构件的权重点击事件，更新权重百分比
        {
            string sGuid = sCS.Split('|')[0];
            float est_wt = float.Parse(sCS.Split('|')[1]);
            string Masterid = sCS.Split('|')[2];
            string task_guid = "";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.Execute("update PLN_TaskProcZQ set est_wt='" + est_wt + "' where masterid='" + Masterid + "' and proc_guid='" + sGuid + "'");
            dal.Execute("update PLN_TaskProc set est_wt='" + est_wt + "' where  proc_guid='" + sGuid + "'");
            string sSQL = "select task_guid from PLN_TaskprocZQ where proc_guid='" + sGuid + "' and masterid='" + Masterid + "'";
            DataTable PLN_TaskprocZQ = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            if (PLN_TaskprocZQ.Rows.Count > 0)
            {
                task_guid = PLN_TaskprocZQ.Rows[0]["task_guid"].ToString();

            }
            dal.Execute("update PLN_TaskProcZQ set est_wt_pct=(round(CONVERT(float,est_wt)/(select sum(A.est_wt) from PLN_TaskProcZQ A " +
            "where A.task_guid=B.task_guid),4))*100 from PLN_TaskProcZQ B " +
            "where B.task_guid in (select task_guid from PLN_TaskProcZQ group by task_guid) " +
            " and masterid='" + Masterid + "' and task_guid='" + task_guid + "'");

            dal.Execute("update  PLN_TaskProcZQ set " +
                "est_wt_pct = round((select est_wt_pct from PLN_TaskProcZQ C where C.task_guid=A.task_guid and seq_num=(select min(seq_num) from PLN_TaskProcZQ D where A.task_guid=D.task_guid )) " +
                " +(select 100-sum(B.est_wt_pct) from PLN_TaskProcZQ B where A.task_guid=B.task_guid),2) " +
                "from PLN_TaskProcZQ A " +
                " where A.task_guid in (select task_guid from PLN_TaskProcZQ group by task_guid) and seq_num=(select min(seq_num) from PLN_TaskProcZQ D where A.task_guid=D.task_guid ) " +
                "and masterid='" + Masterid + "' and task_guid='" + task_guid + "' ");

            dal.Execute("update PLN_TaskProc set est_wt_pct=(round(CONVERT(float,est_wt)/(select sum(A.est_wt) from PLN_TaskProc A " +
                "where A.task_guid=B.task_guid),4))*100 from PLN_TaskProc B " +
                "where B.task_guid in (select task_guid from PLN_TaskProc group by task_guid) " +
                " and task_guid='" + task_guid + "'");

            dal.Execute("update  PLN_TaskProc set " +
                " est_wt_pct = round((select est_wt_pct from PLN_TaskProc C where C.task_guid=A.task_guid and seq_num=(select min(seq_num) from PLN_TaskProc D where A.task_guid=D.task_guid )) " +
                " +(select 100-sum(B.est_wt_pct) from PLN_TaskProc B where A.task_guid=B.task_guid),2) " +
                " from PLN_TaskProc A " +
                " where A.task_guid in (select task_guid from PLN_TaskProc group by task_guid) and seq_num=(select min(seq_num) from PLN_TaskProc D where A.task_guid=D.task_guid ) " +
                " and task_guid='" + task_guid + "' ");
        }
        public void UpJHest_wt_pct(string sCS)//计划工序的权重点击事件，更新权重百分比
        {
            string sGuid = sCS.Split('|')[0];
            float est_wt = float.Parse(sCS.Split('|')[1]);
            string ProcSub_guid = sCS.Split('|')[2];
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            dal.Execute("update PS_PLN_TaskProc_Sub set est_wt='" + est_wt + "' where  ProcSub_guid='" + ProcSub_guid + "'");
            dal.Execute("update PS_PLN_TaskProc_Sub set est_wt_pct=(round(CONVERT(float,est_wt)/(select sum(A.est_wt) from PS_PLN_TaskProc_Sub A where A.proc_guid=B.proc_guid),4))*100 from PS_PLN_TaskProc_Sub B" +
                " where B.proc_guid in (select proc_guid from PS_PLN_TaskProc_Sub group by proc_guid)  " +
                "  and proc_guid='" + sGuid + "'");

            dal.Execute("update  PS_PLN_TaskProc_Sub set est_wt_pct = round((select est_wt_pct from PS_PLN_TaskProc_Sub C where C.proc_guid=A.proc_guid   " +
                "and seq_num=(select min(seq_num) from PS_PLN_TaskProc_Sub D where A.proc_guid=D.proc_guid ))+(select 100-sum(B.est_wt_pct) from PS_PLN_TaskProc_Sub B " +
                " where A.proc_guid=B.proc_guid),2) from PS_PLN_TaskProc_Sub A where A.proc_guid in (select proc_guid from PS_PLN_TaskProc_Sub group by proc_guid)   " +
                "and seq_num=(select min(seq_num) from PS_PLN_TaskProc_Sub D where A.proc_guid=D.proc_guid )  " +
                " and  proc_guid='" + sGuid + "'");
        }


        public string GetDocID(string ID)
        {
            string Code = "";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string Sql = "select ID from PB_DocFiles where FolderId='"+ ID + "'";
            System.Data.DataTable Dt = dal.Session.Query(Sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                Code = Dt.Rows[0]["ID"].ToString();
            }
            else
            {
                Code = "00000000-0000-0000-0000-00000000000A";
            }
            return Code;
        }


        public string GetData(string ID)
        {
            Power.IBaseCore.ISession session = null;
            if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();
            //判断是否已存在之前的数据
            string HkLsit = "select * from NPS_MAD_Jdyb where ID<>'" + ID + "' and EpsProjId='" + session.EpsProjId + "' ORDER BY  RegDate ";
            DataTable Project = XCode.DataAccessLayer.DAL.QuerySQL(HkLsit);

            string[] Dq = new string[] { "电气接地线/接地极（m/根）", "室内桥架/室外桥架安装（m）" ,
            "动力/照明/报警/对讲配管（m）","盘柜安装/变压器（台）","就地设备安装（台）","室内电缆（km）","室外电缆敷设（km）","动力电缆接线（头）","控制电缆接线（头）",
            "室内灯具/室外灯具安装（套）","单体试验","电机单转(台)"};

            string[] Yb = new string[] { "仪表室内盘柜安装", "主槽盒安装（m）", "分支槽盒安装（m）", "保护管安装（m）", "主电缆敷设（km）",
                "分支电缆敷设（点）","仪表安装（台）","接线箱、就地仪表盘安装（台）","主电缆接线（头）","分支电缆接线（头）","导压管、伴热管安装（m）","风管、铜管安装（m）"
            ,"导压管、伴热管、风管打压（m）","仪表单校","回路测试/联校"};

            if (Project.Rows.Count > 0)
            {
                //构造新的数据
                string NSQL = "select * from NPS_MAD_Jdyb_List where FID='" + Project.Rows[0]["ID"] + "' ";
                DataTable OldDt = XCode.DataAccessLayer.DAL.QuerySQL(NSQL);
                DataTable NewDt = OldDt.Clone();
                foreach (DataRow item in OldDt.Rows)
                {
                    DataRow dataRow = NewDt.NewRow();
                    dataRow["Zy"] = item["Zy"];
                    dataRow["Zx"] = item["Zx"];
                    dataRow["Sequ"] = item["Sequ"];
                    dataRow["ID"] = Guid.NewGuid();
                    dataRow["FID"] = ID;
                    dataRow["Gcl"] = item["Gcl"];
                    //dataRow["Dwde"] = item["Dwde"];
                    dataRow["BizAreaId"] = item["BizAreaId"];
                    NewDt.Rows.Add(dataRow);
                }
                SqlBulkCopyByDataTable("NPS_MAD_Jdyb_List", NewDt);
            }
        }


        public string GetData(string ID)
        {
            Power.IBaseCore.ISession session = null;
            if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();
            //判断是否已存在之前的数据
            string HkLsit = "select * from NPS_MAD_CZGL where ID<>'" + ID + "' and EpsProjId='" + session.EpsProjId + "' ORDER BY  RegDate ";
            DataTable Project = XCode.DataAccessLayer.DAL.QuerySQL(HkLsit);
            if (Project.Rows.Count == 0)
            {

                string[] Dq = new string[] { "合计", "国外小计", "自营", "分包", "国内小计", "自营", "分包", "电仪自营", "电仪分包" };

                //构造新的数据
                string NSQL = "select * from NPS_MAD_CZGL_List where 1=0 ";
                DataTable NewDt = XCode.DataAccessLayer.DAL.QuerySQL(NSQL);

                int sequ = 0;
                foreach (string item in Dq)
                {
                    DataRow dataRow = NewDt.NewRow();
                    dataRow["Zx"] = item;
                    dataRow["Sequ"] = sequ;
                    dataRow["ID"] = Guid.NewGuid();
                    dataRow["FID"] = ID;
                    dataRow["Year"] = DateTime.Now.Year;
                    dataRow["Month"] = DateTime.Now.Month;
                    dataRow["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                    sequ++;
                    NewDt.Rows.Add(dataRow);
                }
                sequ++;

                //加入
                SqlBulkCopyByDataTable("NPS_MAD_CZGL_List", NewDt);
            }
            else
            {
                //构造新的数据
                string NSQL = "select * from NPS_MAD_CZGL_List where FID='" + Project.Rows[0]["ID"] + "' ";
                DataTable OldDt = XCode.DataAccessLayer.DAL.QuerySQL(NSQL);
                DataTable NewDt = OldDt.Clone();
                int sequ = 0;
                foreach (DataRow item in OldDt.Rows)
                {
                    DataRow dataRow = NewDt.NewRow();
                    dataRow["Zx"] = item;
                    dataRow["Sequ"] = sequ;
                    dataRow["ID"] = Guid.NewGuid();
                    dataRow["FID"] = ID;
                    dataRow["Year"] = DateTime.Now.Year;
                    dataRow["Month"] = DateTime.Now.Month;
                    dataRow["QNJHCZ"]= item["QNJHCZ"];
                    dataRow["QNJHJCZ"] = item["QNJHJCZ"];
                    dataRow["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                    sequ++;
                    NewDt.Rows.Add(dataRow);
                }
                SqlBulkCopyByDataTable("NPS_MAD_CZGL_List", NewDt);
            }

            return "true";
        }


        public string GetData(string ID)
        {
            Power.IBaseCore.ISession session = null;
            if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();
            //判断是否已存在之前的数据
            string HkLsit = "select * from NPS_MAD_Jdyb where ID<>'" + ID + "' and EpsProjId='" + session.EpsProjId + "' ORDER BY  RegDate ";
            DataTable Project = XCode.DataAccessLayer.DAL.QuerySQL(HkLsit);

            string[] Dq = new string[] { "电气接地线/接地极（m/根）", "室内桥架/室外桥架安装（m）" ,
            "动力/照明/报警/对讲配管（m）","盘柜安装/变压器（台）","就地设备安装（台）","室内电缆（km）","室外电缆敷设（km）","动力电缆接线（头）","控制电缆接线（头）",
            "室内灯具/室外灯具安装（套）","单体试验","电机单转(台)"};

            string[] Yb = new string[] { "仪表室内盘柜安装", "主槽盒安装（m）", "分支槽盒安装（m）", "保护管安装（m）", "主电缆敷设（km）",
                "分支电缆敷设（点）","仪表安装（台）","接线箱、就地仪表盘安装（台）","主电缆接线（头）","分支电缆接线（头）","导压管、伴热管安装（m）","风管、铜管安装（m）"
            ,"导压管、伴热管、风管打压（m）","仪表单校","回路测试/联校"};

            string[] DQSL = new string[] { "0.0251", "0.4031", "0.1213", "3.758", "1.155", "6.7", "31.9", "0.25", "0.25", "0.2566", "4.5", "1" };

            string[] YbSL = new string[] { "1.6","0.42", "0.092", "0.1088", "16", "0.3", "0.667", "1.52", "0.49", "0.12", "0.10656", "0.06656", 
                "0.02164", "0.315", "0.389"};

            if (Project.Rows.Count > 0)
            {
                //构造新的数据
                string NSQL = "select * from NPS_MAD_Jdyb_List where FID='" + Project.Rows[0]["ID"] + "' ";
                DataTable OldDt = XCode.DataAccessLayer.DAL.QuerySQL(NSQL);
                DataTable NewDt = OldDt.Clone();
                foreach (DataRow item in OldDt.Rows)
                {
                    DataRow dataRow = NewDt.NewRow();
                    dataRow["Zy"] = item["Zy"];
                    dataRow["Zx"] = item["Zx"];
                    dataRow["Sequ"] = item["Sequ"];
                    dataRow["ID"] = Guid.NewGuid();
                    dataRow["FID"] = ID;
                    dataRow["Gcl"] = item["Gcl"];
                    //dataRow["Dwde"] = item["Dwde"];
                    dataRow["BizAreaId"] = item["BizAreaId"];
                    NewDt.Rows.Add(dataRow);
                }
                SqlBulkCopyByDataTable("NPS_MAD_Jdyb_List", NewDt);
            }
            else
            {
                //构造新的数据
                string NSQL = "select * from NPS_MAD_Jdyb_List where 1=0 ";
                DataTable NewDt = XCode.DataAccessLayer.DAL.QuerySQL(NSQL);
                int sequ = 0;
                foreach (string item in Dq)
                {
                    DataRow dataRow = NewDt.NewRow();
                    dataRow["Zy"] = "电气";
                    dataRow["Zx"] = item;
                    dataRow["Sequ"] = sequ;
                    dataRow["ID"] = Guid.NewGuid();
                    dataRow["FID"] = ID;
                    dataRow["Dwde"] = DQSL[sequ];
                    dataRow["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                    sequ++;
                    NewDt.Rows.Add(dataRow);
                }
                DataRow Sum = NewDt.NewRow();
                Sum["Zy"] = "电气";
                Sum["Sequ"] = sequ;
                Sum["ID"] = Guid.NewGuid();
                Sum["FID"] = ID;
                Sum["Zx"] = "电气安装小计";
                Sum["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(Sum);
                sequ++;
                DataRow SY = NewDt.NewRow();
                SY["Zy"] = "电气";
                SY["Sequ"] = sequ;
                SY["ID"] = Guid.NewGuid();
                SY["FID"] = ID;
                SY["Zx"] = "电气试验小计";
                SY["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(SY);
                sequ++;
                DataRow ZYLJ = NewDt.NewRow();
                ZYLJ["Zy"] = "电气";
                ZYLJ["Sequ"] = sequ;
                ZYLJ["ID"] = Guid.NewGuid();
                ZYLJ["FID"] = ID;
                ZYLJ["Zx"] = "电气专业累计";
                ZYLJ["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(ZYLJ);
                sequ++;

                int XH = 0;
                foreach (string item in Yb)
                {
                    DataRow dataRow = NewDt.NewRow();
                    dataRow["Zy"] = "仪表";
                    dataRow["Zx"] = item;
                    dataRow["Sequ"] = sequ;
                    dataRow["ID"] = Guid.NewGuid();
                    dataRow["FID"] = ID;
                    dataRow["Dwde"] = YbSL[XH];
                    dataRow["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                    sequ++;
                    NewDt.Rows.Add(dataRow);
                    XH++;
                }
                DataRow Ybxj = NewDt.NewRow();
                Ybxj["Zy"] = "仪表";
                Ybxj["Sequ"] = sequ;
                Ybxj["ID"] = Guid.NewGuid();
                Ybxj["FID"] = ID;
                Ybxj["Zx"] = "仪表安装小计";
                Ybxj["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(Ybxj);
                sequ++;
                DataRow Hj = NewDt.NewRow();
                Hj["Zy"] = "仪表";
                Hj["Sequ"] = sequ;
                Hj["ID"] = Guid.NewGuid();
                Hj["FID"] = ID;
                Hj["Zx"] = "仪表调试小计";
                Hj["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                sequ++;
                NewDt.Rows.Add(Hj);
                DataRow Dy = NewDt.NewRow();
                Dy["Zy"] = "仪表";
                Dy["Sequ"] = sequ;
                Dy["ID"] = Guid.NewGuid();
                Dy["FID"] = ID;
                Dy["Zx"] = "仪表专业累计";
                Dy["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(Dy);

                DataRow DYAZLJ = NewDt.NewRow();
                DYAZLJ["Zy"] = "综合";
                DYAZLJ["Sequ"] = sequ;
                DYAZLJ["ID"] = Guid.NewGuid();
                DYAZLJ["FID"] = ID;
                DYAZLJ["Zx"] = "电仪安装累计";
                DYAZLJ["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(DYAZLJ);

                DYAZLJ = NewDt.NewRow();
                DYAZLJ["Zy"] = "综合";
                DYAZLJ["Sequ"] = sequ;
                DYAZLJ["ID"] = Guid.NewGuid();
                DYAZLJ["FID"] = ID;
                DYAZLJ["Zx"] = "电仪调试累计";
                DYAZLJ["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(DYAZLJ);

                DYAZLJ = NewDt.NewRow();
                DYAZLJ["Zy"] = "综合";
                DYAZLJ["Sequ"] = sequ;
                DYAZLJ["ID"] = Guid.NewGuid();
                DYAZLJ["FID"] = ID;
                DYAZLJ["Zx"] = "电仪累计";
                DYAZLJ["BizAreaId"] = "00000000-0000-0000-0000-00000000000a";
                NewDt.Rows.Add(DYAZLJ);

                //加入
                SqlBulkCopyByDataTable("NPS_MAD_Jdyb_List", NewDt);
            }
            return "true";
        }

    }
}
