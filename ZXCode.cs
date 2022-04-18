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

namespace Power.SPMEMS.Services
{
    class ZXCode
    {
        /// <summary>
        /// by gzz  人员设置获取项目团队组建
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="EpsProjId"></param>
        /// <param name="Department"></param>
        /// <returns></returns>
        public string GenerateGroupTemplate(string FID, string EpsProjId)
        {

            //清理NPS_ENGPLAN_Group_List
            Power.Business.IBusinessOperate bNPS_BUILD_ProjTeam_List = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_TEAM_Generate_PosSet");
            Power.Business.IBusinessList boList = bNPS_BUILD_ProjTeam_List.FindAll("FID", FID);
            boList.Delete();

            //获取NPS_ENGPLAN_Group
            // Power.Business.IBusinessOperate BONPS_ENGPLAN_Group = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_ENGPLAN_Group");
            // Power.Business.IBusinessList boGroupList = BONPS_ENGPLAN_Group.FindAll(" EpsProjId='" + EpsProjId + "' and Status=35", "RegDate DESC", "", 0, 0, Business.SearchFlag.IgnoreRight);
            // if (boGroupList.Count == 0)
            // {
            //     throw new Exception("不存在已生效的团队组建信息");
            // }
            List<string> stringNodes = Power.Global.PowerGlobal.SessionUtil.getSession().EpsProjLongCode.ToString().Split(".").ToList();
            stringNodes.RemoveAt(stringNodes.Count - 1);
            string parentNodeCode = string.Join(".", stringNodes);

            string sql = "select * from NPS_TEAM_OBSTemplate where EpsProjId='00000000-0000-0000-0000-0000000000A4'";
            DataTable dt = XCode.DataAccessLayer.DAL.QuerySQL(sql);
            NewTableTree(dt);
            //postparentid被改为parentid
            ////更改列名PostParentID以重建树
            //dt.Columns["PostParentID"].ColumnName = "ParentID";
            //if (!Power.NanJin.NPSControls.Main.NewTableTree(dt))
            //    throw new Exception("重建树失败，请检查计划模板的树结构");
            foreach (DataRow dataRow in dt.Rows)
            {
                Power.Business.IBaseBusiness baseBusiness = Power.Business.BusinessFactory.CreateBusiness("NPS_TEAM_Generate_PosSet");
                baseBusiness.SetItem("OBSName", dataRow["OBSName"]);
                baseBusiness.SetItem("OBSCode", dataRow["OBSCode"]);
                baseBusiness.SetItem("OBSID", dataRow["PermissionID"]);
                baseBusiness.SetItem("ID", dataRow["ID"]);
                baseBusiness.SetItem("FilterID", dataRow["FilterID"]);
                baseBusiness.SetItem("ParentID", dataRow["ParentID"]);
                baseBusiness.SetItem("DeptName", dataRow["DeptName"]);
                baseBusiness.SetItem("Sequ", dataRow["Sequ"]);
                baseBusiness.SetItem("PermissionName", dataRow["PermissionName"]);
                baseBusiness.SetItem("FID", FID);
                baseBusiness.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            return "success";
        }

        /// <summary>
        /// 要重建树的表，必须具有ID和ParentID字段，并且拥有严格的树结构
        /// by gzz
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="IsKeepId">是否保留原有ID，以列名OldID追加在表的最后一列</param>
        /// <returns></returns>
        public bool NewTableTree(DataTable dt, bool IsKeepId = true)
        {
            //复制出来，变换ID
            DataColumn dc = new DataColumn("OldID", typeof(Guid));
            dt.Columns.Add(dc);
            //复制出ID
            foreach (DataRow item in dt.Rows)
            {
                item["OldID"] = item["ID"];
                //赋予新的ID
                item["ID"] = Guid.NewGuid();
            }
            //整理父子关系
            foreach (DataRow RightRow in dt.Rows)
            {
                if (RightRow["ParentID"].ToString() == "00000000-0000-0000-0000-000000000000")
                    continue;
                //设置ID
                DataRow LeftRow;
                DataRow[] dtTempRows = dt.Select("OldID='" + RightRow["ParentID"] + "'");
                if (dtTempRows.Length > 0)
                {
                    LeftRow = dtTempRows[0];
                    RightRow["ParentID"] = LeftRow["ID"].ToString();
                }
                else
                {
                    RightRow["ParentID"] = "00000000-0000-0000-0000-000000000000";
                }
            }
            if (IsKeepId == false)
                dt.Columns.Remove("OldID");
            return true;
        }


        public string IssueHuman(string ID, string EpsProjId, string EpsProjCode, string EpsProjName)
        {
            //每次先删除已有权限绑定
            deleteData(EpsProjId);

            //查询子表数据
            Power.Business.IBusinessOperate SubOBSBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_TEAM_Generate_PosSet");
            Power.Business.IBusinessList SubAllOBS = SubOBSBO.FindAll(" FID = '" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

            if (SubAllOBS.Count > 0)
            {
                foreach (Power.Business.IBaseBusiness OBSitem in SubAllOBS)
                {
                    // 岗位有则更新 无则插入  
                    Power.Business.IBusinessOperate posi = Power.Business.BusinessFactory.CreateBusinessOperate("Position");
                    Power.Business.IBusinessList posiList = posi.FindAll("Id='" + OBSitem["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (posiList.Count > 0)
                    {
                        posiList[0].SetItem("Id", OBSitem["ID"]);
                        posiList[0].SetItem("ParentId", OBSitem["ParentID"]);
                        posiList[0].SetItem("Code", OBSitem["OBSCode"]);
                        posiList[0].SetItem("Name", OBSitem["OBSName"]);
                        posiList[0].SetItem("Sequ", OBSitem["Sequ"]);
                        posiList[0].SetItem("PosiType", "PROJ");
                        posiList[0].SetItem("Actived", "1");
                        posiList[0].SetItem("OwnProjId", EpsProjId);
                        posiList[0].SetItem("OwnProjName", EpsProjName);
                        posiList[0].SetItem("EpsProjId", EpsProjId);
                        posiList[0].SetItem("EpsProjCode", EpsProjCode);
                        posiList[0].SetItem("EpsProjName", EpsProjName);
                        if (OBSitem["OBSName"].ToString() == "项目经理")
                        {
                            Power.Business.IBusinessOperate BaseDataList = Power.Business.BusinessFactory.CreateBusinessOperate("BaseDataList");
                            Power.Business.IBusinessList BaseDataListBO = BaseDataList.FindAll(" Name = '" + OBSitem["OBSName"] + "' and BaseDataId in (select x1.Id from PB_BaseData x1 where x1.DataType='BD_Position' and x1.UseRange='Global')", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            if (BaseDataListBO.Count > 0)
                            {
                                posiList[0].SetItem("BaseDataName", BaseDataListBO[0]["Name"]);
                                posiList[0].SetItem("BaseDataId", BaseDataListBO[0]["Id"]);
                            }
                        }
                        posiList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                    }
                    else
                    {
                        //插入岗位
                        Power.Business.IBaseBusiness PositionBase = Power.Business.BusinessFactory.CreateBusiness("Position");
                        PositionBase.SetItem("Id", OBSitem["ID"]);
                        PositionBase.SetItem("ParentId", OBSitem["ParentID"]);
                        PositionBase.SetItem("Code", OBSitem["OBSCode"]);
                        PositionBase.SetItem("Name", OBSitem["OBSName"]);
                        PositionBase.SetItem("Sequ", OBSitem["Sequ"]);
                        PositionBase.SetItem("PosiType", "PROJ");
                        PositionBase.SetItem("Actived", "1");
                        PositionBase.SetItem("OwnProjId", EpsProjId);
                        PositionBase.SetItem("OwnProjName", EpsProjName);
                        PositionBase.SetItem("EpsProjId", EpsProjId);
                        PositionBase.SetItem("EpsProjCode", EpsProjCode);
                        PositionBase.SetItem("EpsProjName", EpsProjName);
                        if (OBSitem["OBSName"].ToString() == "项目经理")
                        {
                            Power.Business.IBusinessOperate BaseDataList = Power.Business.BusinessFactory.CreateBusinessOperate("BaseDataList");
                            Power.Business.IBusinessList BaseDataListBO = BaseDataList.FindAll(" Name = '" + OBSitem["OBSName"] + "' and BaseDataId in (select x1.Id from PB_BaseData x1 where x1.DataType='BD_Position' and x1.UseRange='Global')", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            if (BaseDataListBO.Count > 0)
                            {
                                PositionBase.SetItem("BaseDataName", BaseDataListBO[0]["Name"]);
                                PositionBase.SetItem("BaseDataId", BaseDataListBO[0]["Id"]);
                            }
                        }
                        PositionBase.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    }

                    //查询孙表
                    Power.Business.IBusinessOperate OBSListBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_TEAM_Generate_PosSetS");
                    Power.Business.IBusinessList OBSList = OBSListBo.FindAll(" FID = '" + OBSitem["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

                    //孙表存在数据
                    if (OBSList.Count > 0)
                    {
                        foreach (Power.Business.IBaseBusiness Sublist in OBSList)
                        {
                            //插入关系表 人员 与 岗位的关系
                            Power.Business.IBaseBusiness HumanRelate = Power.Business.BusinessFactory.CreateBusiness("HumanRelation");
                            HumanRelate.SetItem("Id", Guid.NewGuid());
                            HumanRelate.SetItem("HumanId", Sublist["HeadHumID"]);
                            HumanRelate.SetItem("RelationId", OBSitem["ID"]);
                            HumanRelate.SetItem("RelationType", "3");
                            HumanRelate.SetItem("Actived", "1");
                            HumanRelate.SetItem("IsMain", "0");
                            HumanRelate.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }
                    }

                    //插入权限组 与 岗位的 关系
                    Power.Business.IBaseBusiness GroupMap = Power.Business.BusinessFactory.CreateBusiness("RightGroupPosiMap");
                    GroupMap.SetItem("Id", Guid.NewGuid());
                    GroupMap.SetItem("PositionId", OBSitem["ID"]);
                    GroupMap.SetItem("RightGroupId", OBSitem["OBSID"]);
                    GroupMap.Save(System.ComponentModel.DataObjectMethodType.Insert);


                    //插入授权范围
                    //Power.Business.IBaseBusiness Range = Power.Business.BusinessFactory.CreateBusiness("Right");
                    //Range.SetItem("GroupType", 0);
                    //Range.SetItem("GroupId", OBSitem["ID"]);
                    //Range.SetItem("ObjectType", 8);
                    //Range.SetItem("ObjectId", EpsProjId);
                    //查询项目长代码
                    //Power.Business.IBusinessOperate Project = Power.Business.BusinessFactory.CreateBusinessOperate("Project");
                    //Power.Business.IBusinessList ProjectBO = Project.FindAll(" project_guid = '" + EpsProjId + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    //if (ProjectBO.Count > 0)
                    //{
                    //   Range.SetItem("ObjectCode", ProjectBO[0]["LongCode"]);
                    //}
                    //Range.SetItem("ObjectName", EpsProjName);
                    //Range.SetItem("bEdit", 1);
                    //Range.SetItem("bView", 1);
                    //Range.SetItem("bToChild", 1);
                    //Range.SetItem("bRightToChild", 1);
                    //Range.Save(System.ComponentModel.DataObjectMethodType.Insert);

                }
            }
            return "success";
        }



        // YYK
        public string InsertGridData(string ID, string PassEpsProjId, string PassEpsProjName)
        {

            //查询子表数据
            Power.Business.IBusinessOperate InsertGridBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_BUILD_ProjTeam_List");
            Power.Business.IBusinessList InsertGridList = InsertGridBO.FindAll(" FID = '" + ID + "' and '" + PassEpsProjId + "' not in (select mm.EpsProjId from NPS_BUILD_ProjTeam mm where mm.ID != '" + ID + "')", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

            //当前的是不同项目,尽情的插入数据
            if (InsertGridList.Count > 0)
            {
                foreach (Power.Business.IBaseBusiness InsertGridItem in InsertGridList)
                {
                    //查询孙表
                    Power.Business.IBusinessOperate InsertGridTwoBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_BUILD_ProjTeam_ListS");
                    Power.Business.IBusinessList InsertGridTwoList = InsertGridTwoBO.FindAll(" FID = '" + InsertGridItem["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

                    //孙表存在数据
                    if (InsertGridTwoList.Count > 0)
                    {
                        foreach (Power.Business.IBaseBusiness InsertGridTwoItem in InsertGridTwoList)
                        {
                            Power.Business.IBusinessOperate WorkerBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PROJPER_WorkExperience");//查看员工信息库是否有跟他  编号姓名相同的
                            Power.Business.IBusinessList WorkerList = WorkerBO.FindAll("'0' + WorkerCode = '" + InsertGridTwoItem["HeadHumCode"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            //Power.Business.IBaseBusiness HumanRelate = Power.Business.BusinessFactory.CreateBusiness("NPS_PROJPER_WorkExperience");
                            if (WorkerList.Count > 0)
                            {
                                foreach (Power.Business.IBaseBusiness WorkerItem in WorkerList)
                                {
                                    Power.Business.IBusinessOperate WorkerBO2 = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PROJPER_WorkExperience_List");//查看员工信息库子表有无数据且是否 时间一样
                                    Power.Business.IBusinessList WorkerList2 = WorkerBO2.FindAll("FID = '" + WorkerItem["ID"] + "' and ComeInDate = '" + InsertGridTwoItem["CallInDate"] + "' and ComeOutDate = '" + InsertGridTwoItem["CallOutDate"] + "' and BelongDeptName = '" + PassEpsProjName + "' and Position = '" + InsertGridItem["OBSName"] + "' and AppointDate = '" + InsertGridTwoItem["AppointedDate"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                                    if (WorkerList2.Count == 0)
                                    {
                                        Power.Business.IBaseBusiness WorkExperienceList = Power.Business.BusinessFactory.CreateBusiness("NPS_PROJPER_WorkExperience_List");
                                        WorkExperienceList.SetItem("ID", Guid.NewGuid());
                                        WorkExperienceList.SetItem("FID", WorkerItem["ID"]);
                                        WorkExperienceList.SetItem("ComeInDate", InsertGridTwoItem["CallInDate"]);//调入时间
                                        WorkExperienceList.SetItem("ComeOutDate", InsertGridTwoItem["CallOutDate"]);//调出时间
                                        WorkExperienceList.SetItem("AppointDate", InsertGridTwoItem["AppointedDate"]);//任命日期
                                        WorkExperienceList.SetItem("BelongDeptName", PassEpsProjName);//项目部名称
                                        WorkExperienceList.SetItem("Position", InsertGridItem["OBSName"]);//项目部名称
                                        WorkExperienceList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //当前的是相同项目
                foreach (Power.Business.IBaseBusiness InsertGridItem in InsertGridList)
                {
                    //查询孙表
                    Power.Business.IBusinessOperate InsertGridTwoBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_BUILD_ProjTeam_ListS");
                    Power.Business.IBusinessList InsertGridTwoList = InsertGridTwoBO.FindAll(" FID = '" + InsertGridItem["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

                    //孙表存在数据
                    if (InsertGridTwoList.Count > 0)
                    {
                        foreach (Power.Business.IBaseBusiness InsertGridTwoItem in InsertGridTwoList)
                        {
                            Power.Business.IBusinessOperate WorkerBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PROJPER_WorkExperience");//查看员工信息库是否有跟他  编号姓名相同的
                            Power.Business.IBusinessList WorkerList = WorkerBO.FindAll("'0' + WorkerCode = '" + InsertGridTwoItem["HeadHumCode"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            //Power.Business.IBaseBusiness HumanRelate = Power.Business.BusinessFactory.CreateBusiness("NPS_PROJPER_WorkExperience");
                            if (WorkerList.Count > 0)
                            {
                                foreach (Power.Business.IBaseBusiness WorkerItem in WorkerList)
                                {
                                    Power.Business.IBusinessOperate WorkerBO2 = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PROJPER_WorkExperience_List");//查看员工信息库子表有无数据且是否 时间一样
                                    Power.Business.IBusinessList WorkerList2 = WorkerBO2.FindAll("FID = '" + WorkerItem["ID"] + "' and ComeInDate = '" + InsertGridTwoItem["CallInDate"] + "' and ComeOutDate = '" + InsertGridTwoItem["CallOutDate"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                                    if (WorkerList2.Count == 0)
                                    {
                                        Power.Business.IBaseBusiness WorkExperienceList = Power.Business.BusinessFactory.CreateBusiness("NPS_PROJPER_WorkExperience_List");
                                        WorkExperienceList.SetItem("ID", Guid.NewGuid());
                                        WorkExperienceList.SetItem("FID", WorkerItem["ID"]);
                                        WorkExperienceList.SetItem("ComeInDate", InsertGridTwoItem["CallInDate"]);//调入时间
                                        WorkExperienceList.SetItem("ComeOutDate", InsertGridTwoItem["CallOutDate"]);//调出时间
                                        WorkExperienceList.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return "1";

        }


        public string deleteData(string EpsProjId)
        {
            int flag = 0;

            if (EpsProjId != "00000000-0000-0000-0000-0000000000A4")
            {
                flag = 1;

                //查询当前项目的岗位
                Power.Business.IBusinessOperate List = Power.Business.BusinessFactory.CreateBusinessOperate("Position");
                Power.Business.IBusinessList SubResult = List.FindAll(" EpsProjId = '" + EpsProjId + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

                if (SubResult.Count > 0)
                {
                    foreach (Power.Business.IBaseBusiness Subitem in SubResult)
                    {

                        //删除人员关系表
                        Power.Business.IBusinessOperate RelationBo = Power.Business.BusinessFactory.CreateBusinessOperate("HumanRelation");
                        Power.Business.IBusinessList RelationList = RelationBo.FindAll(" RelationId = '" + Subitem["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        RelationList.Delete();

                        //删除权限组和岗位关系表
                        Power.Business.IBusinessOperate RelationBo2 = Power.Business.BusinessFactory.CreateBusinessOperate("RightGroupPosiMap");
                        Power.Business.IBusinessList RelationList2 = RelationBo2.FindAll(" PositionId = '" + Subitem["ID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        RelationList2.Delete();

                    }
                    //删除当前项目岗位
                    //SubResult.Delete();
                }
            }
            else
            {
                flag = 2;
            }

            return flag.ToString();
        }

        /// <summary>
        /// 获取岗位模板
        /// </summary>
        /// <param name="FID">表单FID</param>
        /// <param name="LYID">选择的向导ID</param>
        /// <returns></returns>
        public string InsertPosSet(string FID, string LYID)
        {
            //清理NPS_ENGPLAN_Group_List
            Power.Business.IBusinessOperate bNPS_BUILD_ProjTeam_List = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_TEAM_Generate_PosSet");
            Power.Business.IBusinessList boList = bNPS_BUILD_ProjTeam_List.FindAll("FID", FID);
            boList.Delete();

            string sql = "select * from NPS_TEAM_OBSTemplate where FID='" + LYID + "'";
            DataTable dt = XCode.DataAccessLayer.DAL.QuerySQL(sql);
            NewTableTree(dt);
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["OBSCode"] != null)
                {
                    Power.Business.IBaseBusiness baseBusiness = Power.Business.BusinessFactory.CreateBusiness("NPS_TEAM_Generate_PosSet");
                    baseBusiness.SetItem("OBSName", dataRow["OBSName"]);
                    baseBusiness.SetItem("OBSCode", dataRow["OBSCode"]);
                    baseBusiness.SetItem("OBSID", dataRow["PermissionID"]);
                    baseBusiness.SetItem("ID", dataRow["ID"]);
                    baseBusiness.SetItem("FilterID", dataRow["FilterID"]);
                    baseBusiness.SetItem("ParentID", dataRow["ParentID"]);
                    baseBusiness.SetItem("DeptName", dataRow["DeptName"]);
                    baseBusiness.SetItem("Sequ", dataRow["Sequ"]);
                    baseBusiness.SetItem("PermissionName", dataRow["PermissionName"]);
                    baseBusiness.SetItem("FID", FID);
                    baseBusiness.Save(System.ComponentModel.DataObjectMethodType.Insert);
                }
            }
            return "ok";
        }


        public string ImportPosSet(string FID)
        {
            //Power.Business.IBusinessOperate bNPS_BUILD_ProjTeam_List = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_TEAM_OBSTemplate");
            //Power.Business.IBusinessList WorkerList2 = bNPS_BUILD_ProjTeam_List.FindAll("Epsprojid = '00000000-0000-0000-0000-0000000000A4' and (obscode = '010' or parentid = '5AD515A0-803D-3AEF-E913-AF78525BE6D4')", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

            string sql1 = "select max(obscode)+1 as obscode from NPS_TEAM_Generate_PosSet where FID='" + FID + "' and parentid='00000000-0000-0000-0000-000000000000'";
            DataTable dt1 = XCode.DataAccessLayer.DAL.QuerySQL(sql1);
            double obscode = 0;
            if (dt1.Rows.Count > 0)
            {
                obscode = int.Parse(dt1.Rows[0]["obscode"].ToString());
            }


            string sql2 = "select max(sequ) as maxsequ from NPS_TEAM_Generate_PosSet where FID='" + FID + "' ";
            DataTable dt2 = XCode.DataAccessLayer.DAL.QuerySQL(sql2);
            double maxsequ = 0;
            if (dt2.Rows.Count > 0)
            {
                maxsequ = int.Parse(dt2.Rows[0]["maxsequ"].ToString())+1;
            }

            string sql = "select * from NPS_TEAM_OBSTemplate where Epsprojid='00000000-0000-0000-0000-0000000000A4' and (obscode='010' or parentid='5AD515A0-803D-3AEF-E913-AF78525BE6D4') order by OBSCode ";
            DataTable dt = XCode.DataAccessLayer.DAL.QuerySQL(sql);
            NewTableTree(dt);
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["OBSCode"] != null)
                {
                    Power.Business.IBaseBusiness baseBusiness = Power.Business.BusinessFactory.CreateBusiness("NPS_TEAM_Generate_PosSet");
                    baseBusiness.SetItem("OBSName", dataRow["OBSName"]+"(导入的数据)");
                    baseBusiness.SetItem("OBSCode", obscode.ToString().PadLeft(obscode.ToString().Length+1, '0'));
                    baseBusiness.SetItem("OBSID", dataRow["PermissionID"]);
                    baseBusiness.SetItem("ID", dataRow["ID"]);
                    baseBusiness.SetItem("FilterID", dataRow["FilterID"]);
                    baseBusiness.SetItem("ParentID", dataRow["ParentID"]);
                    baseBusiness.SetItem("DeptName", dataRow["DeptName"]);
                    baseBusiness.SetItem("Sequ", maxsequ);
                    baseBusiness.SetItem("PermissionName", dataRow["PermissionName"]);
                    baseBusiness.SetItem("FID", FID);
                    baseBusiness.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    obscode += 0.01;
                    maxsequ++;
                }
            }
            return "ok";
        }

    }
}
