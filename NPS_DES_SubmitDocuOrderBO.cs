


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

using Power.NanJin.NPSControls;
using Power.WorkFlows.Core.Trans;
using Power.WorkFlows.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NewLife.Log;
using XCode.DataAccessLayer;
using Power.Global;
using XCode;
using Power.Systems.StdMasterData.StdMasterData;
using Power.Systems.StdPlan.StdPlan;
using Power.Systems.StdSystem;
using Power.Systems.Systems;
using Power.Business;
using Power.Service.PlanService.Plan;

namespace Power.NPMS.NPS.NPS_DES
{
    /// <summary>
    /// 互提资料单 消息申明
    /// </summary>
    public partial class NPS_DES_SubmitDocuOrderBO
    {
        #region 用户自定义代码，相应外部消息域


        #endregion
    }

    /// <summary>
    /// 互提资料单
    /// </summary>
    public partial class NPS_DES_SubmitDocuOrderBO<TBusiness, TNPS_DES_SubmitDocuOrder_ListBO>
    {
        #region 响应内部事件
        public override void EndFlowChange(Power.IWorkFlow.WorkFlow.EFlowOperate flowOperate, Power.IWorkFlow.WorkFlow.ERecordStatus recordStatus, System.Collections.Hashtable hasFlowInfo)
        {
            //此处可撰写流程状态改变后代码

            if (recordStatus == Power.IWorkFlow.WorkFlow.ERecordStatus.Finish || recordStatus == Power.IWorkFlow.WorkFlow.ERecordStatus.IsSign)
            {
                Power.Business.IBusinessOperate BO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_DesignSchedule_List");
                Power.Business.IBusinessList Result = BO.FindAll("WBSID = '" + this.WBSID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (Result.Count > 0)
                {
                    foreach (Power.Business.IBaseBusiness Item in Result)
                    {
                        if (Item["ActEndDate"] == null || Item["ActEndDate"].ToString() == "0001/1/1 0:00:00")
                        {
                            Item.SetItem("ActEndDate", DateTime.Now.ToString());
                            Item.Save(System.ComponentModel.DataObjectMethodType.Update);
                        }
                    }
                }
                //分发分发表的人员
                GetDistributeHuman(this.ID.ToString(), "45bec70a-498f-4abf-a22b-6445c051d286", this.SubID.ToString(), this.EpsProjId.ToString());
                Power.Controls.PMS.FormControl fc = new Power.Controls.PMS.FormControl();
                fc.DistributeAction("NPS_DES_SubmitDocuOrder", this.ID.ToString());

                if (this.ID.ToString() != "5c09179b-6427-42e2-a496-1219b77571d6"
                                && this.ID.ToString() != "afc92508-676a-4e5a-a954-b0b672a7b301"
                                && this.ID.ToString() != "ec74521c-b96e-44eb-9b1c-d37241579ed7"
                                && this.ID.ToString() != "a030e3db-272f-4af1-83e7-7ee9f709ac0d"
                                && this.ID.ToString() != "5b89e72c-285e-4782-b182-3c1ccd51de41"
                                && this.ID.ToString() != "1593166d-2711-48f4-9b24-b6c364894e7f"
                                && this.ID.ToString() != "53e21c45-afdf-45dc-b8e4-b266ba4e3722"
                                && this.ID.ToString() != "8cce6b96-17c9-4e2e-bd42-f963d3431f94"
                                && this.ID.ToString() != "d46eade6-a102-4d38-8017-55b050bf1027"
                                && this.ID.ToString() != "bc58964a-b454-47dd-b923-6ec042ba78d9"
                                && this.ID.ToString() != "2494c891-6274-421f-a3f2-78b10c090bae"
                                && this.ID.ToString() != "3b995f4c-5b8e-4f31-b226-6fc7cab1fb06"
                                && this.ID.ToString() != "ceb51f01-798c-4174-a84c-d08d22e86720"
                                && this.ID.ToString() != "1ebde289-aa53-44a5-bf78-d327b4b1d478"
                                && this.ID.ToString() != "56dadcea-6e77-41d3-9008-3348df07a497"
                                && this.ID.ToString() != "575ae34b-cbed-4ae6-84a1-d0a38244a5d6"
                                )
                {
                    InsertData(this.ID.ToString());
                }

            }




            return;

        }
        protected override void EndSave(Hashtable formData, System.ComponentModel.DataObjectMethodType methodType)
        {


            base.EndSave(formData, methodType);

            //此处可撰写保存后代码 

            //1.分发分发表的人员
            Power.Controls.PMS.FormControl fc = new Power.Controls.PMS.FormControl();
            fc.DistributeAction("NPS_DES_SubmitDocuOrder", this.ID.ToString());

            //4.录入分发表人员编号
            //查找到二级计划表,此处主要用于判断是否表单状态，只有批准、生效状态需要执行下面的操作，其他状态点击保存按钮不触发
            Power.Business.IBusinessOperate TwoLevelPlanBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder");
            Power.Business.IBusinessList TwoLevelPlanResult = TwoLevelPlanBO.FindAll("ID='" + this.ID + "' and (Status='35' or Status='50')", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

            if (TwoLevelPlanResult.Count > 0)
            {
                //找到对应的二级计划分发表
                Power.Business.IBusinessOperate DistributeBO = Power.Business.BusinessFactory.CreateBusinessOperate("Distribute");
                Power.Business.IBusinessList DistributeResult = DistributeBO.FindAll("KeyValue='" + this.ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                string HumanCodeString = "";
                string HumanIDString = "";
                if (DistributeResult.Count > 0)
                {
                    for (int i = 0; i < DistributeResult.Count; i++)
                    {
                        if (HumanCodeString != "")
                        {
                            HumanCodeString = HumanCodeString + ";" + DistributeResult[i]["HumanName"].ToString();
                            HumanIDString = HumanIDString + ";" + DistributeResult[i]["HumanId"].ToString();
                        }
                        if (HumanCodeString == "")
                        {
                            HumanCodeString = DistributeResult[i]["HumanName"].ToString();
                            HumanIDString = HumanIDString + ";" + DistributeResult[i]["HumanId"].ToString();
                        }
                    }
                }
                //将分发表中的人员编号添加到字段中
                TwoLevelPlanResult[0].SetItem("DistributeHumName", HumanCodeString);
                TwoLevelPlanResult[0].SetItem("DistributeHumID", HumanIDString);
                TwoLevelPlanResult[0].Save(System.ComponentModel.DataObjectMethodType.Update);
            }


        }


        #endregion

        #region 用户自定义代码
        /*
   发起查对意见单
   胡宁绘
   */
        public string GetCheck(string ID, string Code, string Title, string FID)
        {
            // 根据FID查找到当前互提资料单的主表
            Power.Business.IBusinessOperate ReportBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder");
            Power.Business.IBaseBusiness ReportResult = ReportBO.FindByKey(FID);
            // 根据ID查找到当前互提资料子表
            Power.Business.IBusinessOperate ReportListBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder_List");
            Power.Business.IBaseBusiness ReportListResult = ReportListBO.FindByKey(ID);
            // 先设立要生成的查对意见单的主表ID    



            string NewID = Guid.NewGuid().ToString();
            if (ReportListResult != null)
            {
                // 根据当前所级设计输入报告评审主表判断，是否存在对应评审表ID，如果有代表已经生成过，这里将NewID重新赋值为已经存在的ID

                // 如果不存在对应的生成记录，这边开始生成工作
                Power.Business.IBaseBusiness NewMain = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_CheckOpinion"); // 查对意见单
                NewMain.SetItem("ID", NewID); // ID
                NewMain.SetItem("SubmitID", ID); // 互提资料单ID
                NewMain.SetItem("Code", Code); // 互提资料单编号
                NewMain.SetItem("Title", ReportResult["SubCode"] + "-" + ReportListResult["ProfCode"] + "-" + ReportResult["TaskName"] + "-查对意见单"); // 标题
                NewMain.SetItem("EpsProjCode", ReportResult["EpsProjCode"]); // 工程编号
                NewMain.SetItem("EpsProjId", ReportResult["EpsProjId"]); // 工程ID
                NewMain.SetItem("EpsProjName", ReportResult["EpsProjName"]); // 工程名称
                NewMain.SetItem("ProjectCode", ReportResult["ProjectCode"]); // 项目编号
                NewMain.SetItem("ProjectID", ReportResult["ProjectID"]); // 项目ID
                NewMain.SetItem("ProjectName", ReportResult["ProjectName"]); // 项目名称
                NewMain.SetItem("DataName", ReportResult["DataName"]); // 资料名称
                NewMain.SetItem("DataType", ReportResult["DataType"]); // 资料类型
                NewMain.SetItem("FormType", Title); // 表单类型
                                                    // 提出资料专业
                NewMain.SetItem("ProfName", ReportResult["ProfName"]); // 专业名称
                NewMain.SetItem("ProfCode", ReportResult["ProfCode"]); // 专业编号
                NewMain.SetItem("ProfID", ReportResult["ProfID"]); // 专业ID
                                                                   // 提资部门
                NewMain.SetItem("DeptName", ReportResult["DeptName"]); // 部门名称
                NewMain.SetItem("DeptCode", ReportResult["DeptCode"]); // 部门编号
                NewMain.SetItem("DeptID", ReportResult["DeptID"]); // 部门ID
                                                                   // 提资人
                NewMain.SetItem("ProvideHumName", ReportResult["ProvideHumName"]); // 提资人
                NewMain.SetItem("ProvideHumCode", ReportResult["ProvideHumCode"]); // 提资人编号
                NewMain.SetItem("ProvideHumID", ReportResult["ProvideHumID"]); // 提资人ID
                                                                               // 提资专业负责人
                NewMain.SetItem("HeadHumName", ReportResult["HeadHumName"]); // 提资专业负责人
                NewMain.SetItem("HeadHumCode", ReportResult["HeadHumCode"]); // 提资专业负责人编号
                NewMain.SetItem("HeadHumID", ReportResult["HeadHumID"]); // 提资专业负责人ID
                NewMain.SetItem("ActSubmitDate", ReportResult["PlanStartDate"]); // 提出资料日期
                NewMain.SetItem("Phase", ReportResult["Phase"]); // 设计阶段
                                                                 // 设计经理
                NewMain.SetItem("Manager", ReportResult["ManagerName"]); // 设计经理
                NewMain.SetItem("ManagerCode", ReportResult["ManagerCode"]); // 设计经理编号
                NewMain.SetItem("ManagerID", ReportResult["ManagerID"]); // 设计经理ID


                // 查对专业
                NewMain.SetItem("CheckProfName", ReportListResult["ProfName"]); // 查对专业
                NewMain.SetItem("CheckProfCode", ReportListResult["ProfCode"]); // 查对专业编号
                NewMain.SetItem("CheckProfID", ReportListResult["ProfID"]); // 查对专业ID
                                                                            // 查对部门
                NewMain.SetItem("CheckDeptName", ReportListResult["DeptName"]); // 查对部门
                NewMain.SetItem("CheckDeptCode", ReportListResult["DeptCode"]); // 查对部门编号
                NewMain.SetItem("CheckDeptID", ReportListResult["DeptID"]); // 查对部门ID
                                                                            // 查对专业负责人
                NewMain.SetItem("CheckHeadHumName", ReportListResult["HeadHumName"]); // 查对专业负责人
                NewMain.SetItem("CheckHeadHumCode", ReportListResult["HeadHumCode"]); // 查对专业负责人编号
                NewMain.SetItem("CheckHeadHumID", ReportListResult["HeadHumID"]); // 查对专业负责人ID

                // 子项
                NewMain.SetItem("SubID", ReportResult["SubID"]); // 子项ID
                NewMain.SetItem("SubCode", ReportResult["SubCode"]); // 子项编号
                NewMain.SetItem("SubName", ReportResult["SubName"]); // 子项名称

                NewMain.SetItem("TaskID", ReportResult["TaskID"]); // 任务ID
                NewMain.SetItem("TaskName", ReportResult["TaskName"]); // 任务名称
                                                                       // 查对部门
                Power.IBaseCore.ISession session = null;
                if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();
                // 查对人
                //NewMain.SetItem("CheckHumName", ReportListResult["RegHumName"]); // 查对人
                //NewMain.SetItem("CheckHumID", ReportListResult["RegHumId"]); // 查对人ID
                NewMain.SetItem("CheckHumName", session.HumanName.ToString()); // 查对人
                NewMain.SetItem("CheckHumID", session.HumanId.ToString()); // 查对人ID
                NewMain.Save(System.ComponentModel.DataObjectMethodType.Insert);

                // 修改所级设计输入报告评审
                string CheckID = "";
                if (ReportListResult["CheckID"] != null && ReportListResult["CheckID"].ToString() != "" && ReportListResult["CheckID"].ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    CheckID = ReportListResult["CheckID"].ToString() + "," + NewID;
                }
                else
                {
                    CheckID = NewID;
                }
                ReportListResult.SetItem("CheckID", CheckID); // 修改值为新增的所级设计评审结论实施记录单ID
                ReportListResult.Save(System.ComponentModel.DataObjectMethodType.Update);

                XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                string sql = @"update NPS_DES_SubmitDocuOrderReceive set CheckID='" + CheckID + "' where PreFID='" + ID + "'";
                dal1.Session.Execute(sql);

                return NewID;
            }
            return "";
        }





        /*
        设计人节点时根据设计人接收日期自动修改接收时间
        胡宁绘
        */
        public string updateTime(string ID, string HumanId)
        {
            //1.找到互提资料单对应子表数据
            Power.Business.IBusinessOperate docuBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder_List");//互提资料单子表
            Power.Business.IBusinessList DocuBo = docuBo.FindAll("FID='" + ID + "' AND DesignHumID = '" + HumanId + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (DocuBo.Count > 0)
            {
                for (int i = 0; i < DocuBo.Count; i++)
                {
                    DocuBo[i].SetItem("ReceiveDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    DocuBo[i].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
            return "";
        }
        public string GetDistributeHuman(string ID, string HtmlPath, string SubID, string EpsProjId)
        {
            //查找到互提资料单子表
            Power.Business.IBusinessOperate PlanningBO = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder_List");
            Power.Business.IBusinessList PlanningResult = PlanningBO.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

            Power.Business.IBusinessOperate OrderBo = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder");
            Power.Business.IBusinessList DocuOrderBo = OrderBo.FindAll("ID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            string HumanCodeString = "";
            string HumanIDString = "";
            if (PlanningResult.Count > 0)
            {
                for (int i = 0; i < PlanningResult.Count; i++)
                {
                    // 查找L5
                    XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                    string sql1 = "";
                    sql1 = @"SELECT  
                                     stuff((select ',' +DesignHumID  from
                                     NPS_DES_DesignSchedule_List
                                     WHERE ParentID IN
                                     (
                                     SELECT ID FROM NPS_DES_DesignSchedule_List
                                     WHERE ParentID IN (SELECT ID FROM NPS_DES_DesignSchedule_List WHERE WBSID =  '" + SubID + @"')
                                     AND OriginWBSID = '" + PlanningResult[i]["ProfID"] + @"'
                                     )FOR xml PATH('')), 1, 1, '') DesignHumID";
                    System.Data.DataTable dt1 = dal1.Session.Query(sql1).Tables[0];
                    if (dt1.Rows.Count > 0 && dt1.Rows[0]["DesignHumID"].ToString() != "NULL" && dt1.Rows[0]["DesignHumID"].ToString() != "")
                    {
                        for (int x = 0; x < dt1.Rows.Count; x++)
                        {
                            Power.Business.IBusinessOperate PBUserBO1 = Power.Business.BusinessFactory.CreateBusinessOperate("Human");
                            Power.Business.IBusinessList PBUserResult1 = PBUserBO1.FindAll("Id IN (" + dt1.Rows[x]["DesignHumID"] + ")", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            if (PBUserResult1.Count > 0)
                            {
                                for (int y = 0; y < PBUserResult1.Count; y++)
                                {
                                    // 插入当前互提资料单子表设计人
                                    Power.Business.IBaseBusiness PBDistributeList1 = Power.Business.BusinessFactory.CreateBusiness("Distribute");
                                    PBDistributeList1.SetItem("ID", Guid.NewGuid().ToString());
                                    PBDistributeList1.SetItem("KeyWord", "NPS_DES_SubmitDocuOrder");
                                    PBDistributeList1.SetItem("KeyValue", ID);
                                    PBDistributeList1.SetItem("HtmlPath", HtmlPath);
                                    PBDistributeList1.SetItem("HumanId", PBUserResult1[y]["Id"]);
                                    PBDistributeList1.SetItem("HumanCode", PBUserResult1[y]["Code"]);
                                    PBDistributeList1.SetItem("HumanName", PBUserResult1[y]["Name"]);
                                    PBDistributeList1.Save(System.ComponentModel.DataObjectMethodType.Insert);

                                    if (HumanCodeString != "")
                                    {
                                        HumanCodeString = HumanCodeString + ";" + PBUserResult1[y]["Name"].ToString();
                                    }
                                    if (HumanCodeString == "")
                                    {
                                        HumanCodeString = PBUserResult1[y]["Name"].ToString();
                                    }

                                    if (HumanIDString != "")
                                    {
                                        HumanIDString = HumanIDString + ";" + PBUserResult1[y]["Id"].ToString();
                                    }
                                    if (HumanIDString == "")
                                    {
                                        HumanIDString = PBUserResult1[y]["Id"].ToString();
                                    }
                                }
                            }
                        }
                    }

                    Power.Business.IBusinessOperate PBUserBO = Power.Business.BusinessFactory.CreateBusinessOperate("Human");
                    Power.Business.IBusinessList PBUserResult = PBUserBO.FindAll("Id= '" + PlanningResult[i]["HeadHumID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (PBUserResult.Count > 0)
                    {
                        for (int j = 0; j < PBUserResult.Count; j++)
                        {
                            // 插入当前互提资料单子表专业负责人
                            Power.Business.IBaseBusiness PBDistributeList = Power.Business.BusinessFactory.CreateBusiness("Distribute");
                            string NewID = Guid.NewGuid().ToString();
                            PBDistributeList.SetItem("ID", NewID);
                            PBDistributeList.SetItem("KeyWord", "NPS_DES_SubmitDocuOrder");
                            PBDistributeList.SetItem("KeyValue", ID);
                            PBDistributeList.SetItem("HtmlPath", HtmlPath);
                            PBDistributeList.SetItem("HumanId", PBUserResult[j]["Id"]);
                            PBDistributeList.SetItem("HumanCode", PBUserResult[j]["Code"]);
                            PBDistributeList.SetItem("HumanName", PBUserResult[j]["Name"]);
                            PBDistributeList.Save(System.ComponentModel.DataObjectMethodType.Insert);

                            if (HumanCodeString != "")
                            {
                                HumanCodeString = HumanCodeString + ";" + PBUserResult[j]["Name"].ToString();
                            }
                            if (HumanCodeString == "")
                            {
                                HumanCodeString = PBUserResult[j]["Name"].ToString();
                            }

                            if (HumanIDString != "")
                            {
                                HumanIDString = HumanIDString + ";" + PBUserResult[j]["Id"].ToString();
                            }
                            if (HumanIDString == "")
                            {
                                HumanIDString = PBUserResult[j]["Id"].ToString();
                            }
                        }
                    }
                }
            }


            // 增加设计经理分发
            Power.Business.IBusinessOperate profHead = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_ProfHead");
            Power.Business.IBusinessList profHeadBO = profHead.FindAll("EpsProjId='" + EpsProjId + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (profHeadBO.Count > 0)
            {
                Power.Business.IBusinessOperate human = Power.Business.BusinessFactory.CreateBusinessOperate("Human");
                Power.Business.IBusinessList HumanBo = human.FindAll("Id= '" + profHeadBO[0]["ManagerID"] + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (HumanBo.Count > 0)
                {
                    Power.Business.IBaseBusiness HumanList = Power.Business.BusinessFactory.CreateBusiness("Distribute");
                    string NewID = Guid.NewGuid().ToString();
                    HumanList.SetItem("ID", NewID);
                    HumanList.SetItem("KeyWord", "NPS_DES_SubmitDocuOrder");
                    HumanList.SetItem("KeyValue", ID);
                    HumanList.SetItem("HtmlPath", HtmlPath);
                    HumanList.SetItem("HumanId", HumanBo[0]["Id"]);
                    HumanList.SetItem("HumanCode", HumanBo[0]["Code"]);
                    HumanList.SetItem("HumanName", HumanBo[0]["Name"]);
                    HumanList.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    if (HumanCodeString != "")
                    {
                        HumanCodeString = HumanCodeString + ";" + HumanBo[0]["Name"].ToString();
                    }
                    if (HumanCodeString == "")
                    {
                        HumanCodeString = HumanBo[0]["Name"].ToString();
                    }

                    if (HumanIDString != "")
                    {
                        HumanIDString = HumanIDString + ";" + HumanBo[0]["Id"].ToString();
                    }
                    if (HumanIDString == "")
                    {
                        HumanIDString = HumanBo[0]["Id"].ToString();
                    }

                }
            }


            DocuOrderBo[0].SetItem("DistributeHumName", HumanCodeString);
            DocuOrderBo[0].SetItem("DistributeHumID", HumanIDString);
            DocuOrderBo[0].Save(System.ComponentModel.DataObjectMethodType.Update);

            return "";
        }



        public string InsertData(string ID)
        {
            Power.IBaseCore.ISession session = null;
            if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();

            //获取主表数据
            Power.Business.IBusinessOperate bop = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder");
            Power.Business.IBaseBusiness list = bop.FindByKey(ID);

            //获取子表数据
            Power.Business.IBusinessOperate bop2 = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_DES_SubmitDocuOrder_List");
            Power.Business.IBusinessList list2 = bop2.FindAll("FID='" + ID + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);

            int flag = 0;
            if (list != null && list2.Count > 0)
            {
                flag++;
                foreach (Power.Business.IBaseBusiness item in list2)
                {
                    string NewID = Guid.NewGuid().ToString();
                    Power.NanJin.NPSControls.Main main = new Power.NanJin.NPSControls.Main();
                    string Code = main.GetCodeOnly("NPS_DES_SubmitDocuOrderReceive");

                    Power.Business.IBaseBusiness NewMain = Power.Business.BusinessFactory.CreateBusiness("NPS_DES_SubmitDocuOrderReceive"); //互提资料单（接收专业）
                    NewMain.SetItem("ID", NewID);
                    NewMain.SetItem("Code", Code);
                    NewMain.SetItem("PreID", list["ID"]); //存储主表ID
                    NewMain.SetItem("PreFID", item["ID"]);//存储子表FID

                    NewMain.SetItem("Status", "0");//状态
                    NewMain.SetItem("Title", list["Title"]); // 标题
                    NewMain.SetItem("TaskType", list["TaskType"]);
                    NewMain.SetItem("IfProfData", list["IfProfData"]);
                    NewMain.SetItem("PlanID", list["PlanID"]);
                    NewMain.SetItem("WBSCode", list["WBSCode"]);

                    NewMain.SetItem("EpsProjCode", list["EpsProjCode"]);
                    NewMain.SetItem("EpsProjName", list["EpsProjName"]);
                    NewMain.SetItem("EpsProjId", list["EpsProjId"]);
                    NewMain.SetItem("ProjectCode", list["ProjectCode"]);
                    NewMain.SetItem("ProjectName", list["ProjectName"]);
                    NewMain.SetItem("ProjectID", list["ProjectID"]);

                    NewMain.SetItem("Phase", list["Phase"]);
                    NewMain.SetItem("ManagerName", list["ManagerName"]);

                    NewMain.SetItem("SubCode", list["SubCode"]);
                    NewMain.SetItem("SubName", list["SubName"]);

                    NewMain.SetItem("WBSName", list["WBSName"]);
                    NewMain.SetItem("ProfName", list["ProfName"]);
                    NewMain.SetItem("ProfCode", list["ProfCode"]);
                    NewMain.SetItem("ProfID", list["ProfID"]);
                    NewMain.SetItem("ProfID", list["ProfID"]);

                    NewMain.SetItem("HeadHumName", list["HeadHumName"]);
                    NewMain.SetItem("HeadHumCode", list["HeadHumCode"]);
                    NewMain.SetItem("HeadHumID", list["HeadHumID"]);

                    NewMain.SetItem("ReviewHumName", list["ReviewHumName"]);
                    NewMain.SetItem("ReviewHumID", list["ReviewHumID"]);
                    NewMain.SetItem("ReviewHumCode", list["ReviewHumCode"]);

                    NewMain.SetItem("DataName", list["DataName"]);
                    NewMain.SetItem("DataType", list["DataType"]);
                    NewMain.SetItem("WBSID", list["WBSID"]);

                    NewMain.SetItem("EquivalentA1", list["EquivalentA1"]);
                    NewMain.SetItem("PlanStartDate", list["PlanStartDate"]);

                    NewMain.SetItem("ProvideHumName", list["ProvideHumName"]);
                    NewMain.SetItem("ProvideHumCode", list["ProvideHumCode"]);
                    NewMain.SetItem("ProvideHumID", list["ProvideHumID"]);

                    NewMain.SetItem("DeptName", list["DeptName"]);
                    NewMain.SetItem("DeptCode", list["DeptCode"]);

                    NewMain.SetItem("DesCheck", list["DesCheck"]);
                    NewMain.SetItem("DesCheckID", list["DesCheckID"]);

                    NewMain.SetItem("DesTargetTime", list["DesTargetTime"]);
                    NewMain.SetItem("DataContentExplain", list["DataContentExplain"]);

                    NewMain.SetItem("RegHumName", item["HeadHumName"]);
                    NewMain.SetItem("RegDate", DateTime.Now.ToString());

                    //专业
                    NewMain.SetItem("SubProfName", item["ProfName"]);
                    NewMain.SetItem("SubProfCode", item["ProfCode"]);
                    NewMain.SetItem("SubProfID", item["ProfID"]);

                    //专业负责人
                    NewMain.SetItem("SubHeadHumName", item["HeadHumName"]);
                    NewMain.SetItem("SubHeadHumCode", item["HeadHumCode"]);
                    NewMain.SetItem("SubHeadHumID", item["HeadHumID"]);
                    NewMain.SetItem("CheckID", item["CheckID"]);

                    //本专业接收负责人
                    NewMain.SetItem("MajorReciveName", item["MajorReciveName"]);
                    NewMain.SetItem("MajorReciveCode", item["MajorReciveCode"]);
                    NewMain.SetItem("MajorReciveID", item["MajorReciveID"]);

                    //要求查返日期
                    NewMain.SetItem("CheckDate", item["CheckDate"]);
                    NewMain.SetItem("Remark", item["Remark"]);
                    NewMain.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    if (item["HeadHumID"] == null)
                    {
                        continue;
                    }

                    XCode.DataAccessLayer.DAL dal1 = XCode.DataAccessLayer.DAL.Create();
                    string sql = @"SELECT * from PB_Human where ID = '" + item["HeadHumID"].ToString() + "'";
                    System.Data.DataTable SessionObj = dal1.Session.Query(sql).Tables[0];

                    if (SessionObj.Rows.Count <= 0)
                    {
                        continue;
                    }

                    TSession tSession = new TSession();
                    tSession.BizAreaId = SessionObj.Rows[0]["BizAreaId"] == null ? null : SessionObj.Rows[0]["BizAreaId"].ToString();
                    tSession.EpsProjectId = session.EpsProjId;
                    tSession.EpsProjectCode = session.EpsProjCode;
                    tSession.EpsProjectName = session.EpsProjName;
                    tSession.HumanId = SessionObj.Rows[0]["Id"].ToString();
                    tSession.HumanName = SessionObj.Rows[0]["Name"].ToString();
                    tSession.HumanCode = SessionObj.Rows[0]["Code"].ToString();
                    tSession.SourceMode = ESourceMode.PositionAndUser;
                    tSession.DeptPositionID = SessionObj.Rows[0]["PosiId"] == null ? null : SessionObj.Rows[0]["PosiId"].ToString();
                    tSession.DeptPositionName = SessionObj.Rows[0]["PosiName"] == null ? null : SessionObj.Rows[0]["PosiName"].ToString();

                    if (string.IsNullOrEmpty(tSession.DeptPositionID))
                    {
                        tSession.SourceMode = ESourceMode.DeptAndUser;
                        tSession.DeptPositionID = SessionObj.Rows[0]["DeptId"] == null ? null : SessionObj.Rows[0]["DeptId"].ToString();
                        tSession.DeptPositionName = SessionObj.Rows[0]["DeptName"] == null ? null : SessionObj.Rows[0]["DeptName"].ToString();
                    }

                    if (string.IsNullOrEmpty(tSession.DeptPositionID) && SessionObj.Rows[0]["Name"].ToString() != "admin")
                    {
                        NewLife.Log.XTrace.WriteLine("对不起，当前用户既没有岗位信息，也没有部门信息,也不是管理员");
                        throw new Exception("对不起，当前用户既没有岗位信息，也没有部门信息,也不是管理员");
                    }

                    Main entity = new Main();
                    entity.AutoPushWorkFlow(NewID, "a54856da-3d40-4961-8644-97ccc1496538", item["HeadHumID"].ToString(), tSession);
                }
            }
            return flag.ToString();
        }

        //2021-4-27 王顺清 处理菜单对应的路径，将结果存放于 NPS_OA_MenuUrl 表
        public string DownUrlFiles(string Ip, string EpsProjId, string EpsProjCode, string HumId, string HumName)
        {
            string sLongCode_A = GetLongCode_A();
            string sLongCode_B = GetLongCode_B();

            //返回数据结果集
            System.Data.DataTable dt = new System.Data.DataTable();
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            //排除作为父节点的数据，即只查最后一层级数据and LongCode not like '125.243%'
            string sql = @"delete from NPS_OA_MenuUrl
                    insert into NPS_OA_MenuUrl
                    select x3.KeyWord,x1.Id,x1.ParentId,x1.Code,x1.Name,x1.LongCode,''
                    from (
                        Select A.* From  PB_Menu A  
                        Where LongCode like '" + sLongCode_A + @".%' and LongCode not like '" + sLongCode_B + @"%'
                        ) x1
                    left join PB_MenuWidget x2 on x1.id = x2.MenuId
                    left join PB_Widget x3 on x2.WidgetId = x3.Id ";
            dal.Session.Execute(sql);
            sql = "select * from NPS_OA_MenuUrl";
            dt = dal.Session.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string LCode = dt.Rows[i]["LongCode"].ToString();
                string[] allCode = LCode.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                string sUrl = "''";
                //系统目前最多4层菜单，判断是最后一层菜单（即第4层）
                if (allCode.Length == 4)
                {
                    sUrl = "select (select Name from PB_Menu where LongCode='" + allCode[0] + "." + allCode[1] + "') + '/' + (select Name from PB_Menu where LongCode='" + allCode[0] + "." + allCode[1] + "." + allCode[2] + "') +'/'+'" + dt.Rows[i]["Name"].ToString().Replace("/", "&") + "'+'/'";
                }
                else if (allCode.Length == 3)
                {
                    sUrl = "select (select Name from PB_Menu where LongCode='" + allCode[0] + "." + allCode[1] + "') + '/' +'" + dt.Rows[i]["Name"].ToString().Replace("/", "&") + "'+'/'";
                }
                sql = "update NPS_OA_MenuUrl set ServerUrl=(" + sUrl + ") where LongCode='" + LCode + "'";
                dal.Session.Execute(sql);
            }
            return DownFiles(Ip, EpsProjId, EpsProjCode, HumId, HumName);
            //return "OK";
        }

        public string DownFiles(string Ip, string EpsProjId, string EpsProjCode, string HumId, string HumName)
        {
            return Power.KISENPMS.Control.MainControl.DownFiles(Ip, EpsProjId, EpsProjCode, HumId, HumName);
        }

        public string GetLongCode_A()
        {
            string sResult = string.Empty;
            string sSQL = "select LongCode from PB_Menu where name='项目级'";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            System.Data.DataTable dataTable = dal.Session.Query(sSQL).Tables[0];
            if (dataTable.Rows.Count > 0 || dataTable.Rows[0]["LongCode"] != null)
            {
                sResult = dataTable.Rows[0]["LongCode"].ToString();
            }
            return sResult;
        }

        public string GetLongCode_B()
        {
            string sResult = string.Empty;
            string sSQL = "select LongCode from PB_Menu where name='系统管理' and ParentId = (select id from PB_Menu where name='项目级')";
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            System.Data.DataTable dataTable = dal.Session.Query(sSQL).Tables[0];
            if (dataTable.Rows.Count > 0 || dataTable.Rows[0]["LongCode"] != null)
            {
                sResult = dataTable.Rows[0]["LongCode"].ToString();
            }
            return sResult;
        }




        //形象进度曲线（按月）  NPS_PERSONAL_Passport
        public string GetWBSEVM(string plan_guid, string task_guid)
        {
            //ViewResultModel viewResultModel = ViewResultModel.Create(success: true, "");
            ViewResultModel viewResultModel = ViewResultModel.Create(true, "");
            viewResultModel.list = GetWBSEVM_NB(plan_guid.ToString().ToLower(), task_guid.ToString().ToLower());
            viewResultModel.message = GetWBSEVM_NB(plan_guid.ToString().ToLower(), task_guid.ToString().ToLower()).Count.ToString();
            return viewResultModel.ToJson();
        }

        public List<Hashtable> GetWBSEVM_NB(string plan_guid, string task_guid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (task_guid.ToString().ToUpper() == "ALL")
            {
                XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
                string sSQL = @" select * from pln_projwbs where plan_guid='" + plan_guid + "' and parent_wbs_guid = '00000000-0000-0000-0000-000000000000' ";
                string wbs_guid = string.Empty;
                DataSet dataSet = dal.Session.Query(sSQL);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    wbs_guid = dataSet.Tables[0].Rows[0]["wbs_guid"].ToString();
                }
                if (string.IsNullOrEmpty(wbs_guid))
                {
                    stringBuilder.AppendFormat("plan_guid='{0}' and 1<>1 ", plan_guid);
                }
                else
                {
                    stringBuilder.AppendFormat("plan_guid='{0}' and task_guid='{1}'", plan_guid, wbs_guid);
                }

            }
            else
            {
                XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
                string sSQL = @" select wbs_short_name, wbs_guid, wbs_guid_before, plan_guid from pln_projwbs where wbs_guid = '" + task_guid + "' ";
                string wbs_guid_before = string.Empty;
                DataSet dataSet = dal.Session.Query(sSQL);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    wbs_guid_before = dataSet.Tables[0].Rows[0]["wbs_guid_before"].ToString();
                }
                if (string.IsNullOrEmpty(wbs_guid_before))
                {
                    stringBuilder.AppendFormat("plan_guid='{0}' and 1<>1 ", plan_guid);
                }
                else
                {
                    stringBuilder.AppendFormat("plan_guid='{0}' and task_guid='{1}'", plan_guid, wbs_guid_before);
                }
            }
            EntityList<TaskBCWSDO> entityList = Entity<TaskBCWSDO>.FindAll(stringBuilder.ToString(), "periodstrat", "period_guid,periodstrat,periodend,plan_complete_pct,act_complete_pct");
            List<Hashtable> list = new List<Hashtable>();
            double num = 0.0;
            decimal num2 = default(decimal);
            for (int i = 0; i < entityList.Count; i++)
            {
                TaskBCWSDO val = entityList[i];
                Hashtable hashtable = new Hashtable();
                //hashtable.Add("startdate", ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_periodstrat());
                //hashtable.Add("enddate", ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_periodend());
                //hashtable.Add("period_guid", ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_period_guid());
                //decimal num3 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_act_complete_pct();
                //double num4 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_plan_complete_pct();
                //decimal num5 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_act_complete_pct();
                //double num6 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).get_plan_complete_pct();

                hashtable.Add("startdate", ((TaskBCWSDO<TaskBCWSDO>)(object)val).periodstrat);
                hashtable.Add("enddate", ((TaskBCWSDO<TaskBCWSDO>)(object)val).periodend);
                hashtable.Add("period_guid", ((TaskBCWSDO<TaskBCWSDO>)(object)val).period_guid);
                decimal num3 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).act_complete_pct;
                double num4 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).plan_complete_pct;
                decimal num5 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).act_complete_pct;
                double num6 = ((TaskBCWSDO<TaskBCWSDO>)(object)val).plan_complete_pct;
                if (i != 0)
                {
                    if (num3 != 0m)
                    {
                        num5 = num3 - num2;
                    }
                    else
                    {
                        num3 = num2;
                    }

                    if (num4 == 0.0)
                    {
                        num4 = num;
                    }

                    num6 = num4 - num;
                }

                hashtable.Add("actsumpct", num3);
                hashtable.Add("plnsumpct", num4);
                hashtable.Add("actcurpct", num5);
                hashtable.Add("plncurpct", num6);
                num = num4;
                num2 = num3;
                list.Add(hashtable);
            }

            return list;
        }

        //进度对比曲线
        public string GetWBSBCWPCompare(string plan_guid, string wbs_guid, string actv_guid, string type)
        {
            Power.IBaseCore.ISession session = null;
            if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();

            //ViewResultModel viewResultModel = ViewResultModel.Create(success: true, "");
            ViewResultModel viewResultModel = ViewResultModel.Create(true, "");
            //viewResultModel.list = GetWBSBCACWPCompare_NB(((BaseControl)this).get_session().EpsProjId, plan_guid, wbs_guid, actv_guid, type);
            viewResultModel.list = GetWBSBCACWPCompare_NB(session.EpsProjId, plan_guid, wbs_guid, actv_guid, type);
            return viewResultModel.ToJson();
        }

        public List<Hashtable> GetWBSBCACWPCompare_NB(string proj_guid, string plan_guid, string wbs_guid, string actv_guid, string type)
        {
            List<Hashtable> result = new List<Hashtable>();
            EntityList<PS_ProjPeriodDO> entityList = Entity<PS_ProjPeriodDO>.FindAll(" proj_guid='" + proj_guid + "'", "periodstart", "*");
            if (entityList != null && entityList.Count > 0)
            {
                string text = plan_guid;
                bool isPrimary = true;
                IBusinessOperate businessOperate = BusinessFactory.CreateBusinessOperate("ProjectPlan");
                IBusinessList businessList = businessOperate.FindAll("proj_plan_guid='" + plan_guid + "'", "", "primaryVersion_guid");
                if (businessList != null && businessList.Count > 0 && businessList[0]["primaryVersion_guid"] != null)
                {
                    isPrimary = false;
                    text = businessList[0]["primaryVersion_guid"].ToString();
                }

                string text2 = " proj_guid='" + proj_guid + "' and plan_guid='" + text + "' ";
                List<string> values = new List<string>();
                if (!string.IsNullOrEmpty(wbs_guid))
                {
                    values = PlusGanttService.GetWbsChildsAndSelf(wbs_guid);
                }

                List<string> list = new List<string>();
                if (!string.IsNullOrEmpty(actv_guid) && !string.IsNullOrEmpty(type) && type.ToLower() == "task")
                {
                    IBusinessOperate businessOperate2 = BusinessFactory.CreateBusinessOperate("PLN_TASKACTV");
                    string whereClause = text2 + " and ActvCode_guid='" + actv_guid + "'";
                    IBusinessList businessList2 = businessOperate2.FindAll(whereClause, "", "task_guid");
                    foreach (IBaseBusiness item in businessList2)
                    {
                        if (item["task_guid"] != null)
                        {
                            list.Add(item["task_guid"].ToString());
                        }
                    }
                }

                Dictionary<string, decimal> taskPlanLevelEstWtPct = Power.Service.PlanService.Plan.PlanService.GetTaskPlanLevelEstWtPct(text);
                if (!string.IsNullOrEmpty(wbs_guid))
                {
                    text2 = text2 + " and  wbs_guid in ('" + string.Join("','", values) + "') ";
                }

                if (!string.IsNullOrEmpty(actv_guid))
                {
                    text2 = text2 + " and  task_guid in ('" + string.Join("','", list) + "') ";
                }

                List<Hashtable> taskPlanAndActBcwsList = Power.Service.PlanService.Plan.PlanService.GetTaskPlanAndActBcwsList(proj_guid, plan_guid, text2, isPrimary, taskPlanLevelEstWtPct);
                result = Power.Service.PlanService.Plan.PlanService.GetTaskPeriodBCWSList(entityList, taskPlanAndActBcwsList);
            }

            return result;
        }


        /// <summary>
        /// 业务对象 NPS_PUR_PurPlan
        /// </summary>
        /// <param name="EpsProjID">项目ID</param>
        /// <param name="PurPlanType">表单分类</param>
        public void InsertEquipBOMByEps(string EpsProjID, string PurPlanType)
        {
            Power.Business.IBusinessOperate purPlanMX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan");
            Power.Business.IBusinessList purPlanMXList = purPlanMX.FindAll(" EpsProjID='" + EpsProjID + "' and PurPlanType = '" + PurPlanType + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            for (int i = 0; i < purPlanMXList.Count; i++)
            {
                string sID = purPlanMXList[i]["ID"].ToString().Trim();
                InsertEquipBOM(sID);
            }
        }

        //业务对象 NPS_PUR_PurPlan
        public void InsertEquipBOM(string ID)
        {
            string Code = string.Empty;
            string ProfName = string.Empty;
            string ProfCode = string.Empty;
            string ProfID = string.Empty;

            Power.Business.IBusinessOperate purPlanMain = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan");
            Power.Business.IBusinessList purPlanMainList = purPlanMain.FindAll(" ID='" + ID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (purPlanMainList.Count > 0)
            {
                Code = purPlanMainList[0]["Code"].ToString();
                ProfName = purPlanMainList[0]["ProfName"] == null ? "" : purPlanMainList[0]["ProfName"].ToString();
                ProfCode = purPlanMainList[0]["ProfCode"] == null ? "" : purPlanMainList[0]["ProfCode"].ToString();
                ProfID = purPlanMainList[0]["ProfID"] == null ? "" : purPlanMainList[0]["ProfID"].ToString();
            }

            Power.Business.IBusinessOperate purPlanMX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan_List");
            Power.Business.IBusinessList purPlanMXList = purPlanMX.FindAll(" FID='" + ID + "'  ", " EquipCode desc ", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            for (int i = 0; i < purPlanMXList.Count; i++)
            {
                string sParentID = purPlanMXList[i]["ID"].ToString();
                string sEquipCode = purPlanMXList[i]["EquipCode"] == null ? "" : purPlanMXList[i]["EquipCode"].ToString();
                string sMatchCode = purPlanMXList[i]["MatchCode"] == null ? "" : purPlanMXList[i]["MatchCode"].ToString();
                string sSubCode = purPlanMXList[i]["SubCode"] == null ? "" : purPlanMXList[i]["SubCode"].ToString();
                string sCompositionCode = sSubCode + sEquipCode + sMatchCode;
                if (!string.IsNullOrEmpty(sEquipCode))//主设备
                {
                    Power.Business.IBaseBusiness XZItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM");
                    Guid FID = Guid.NewGuid();
                    XZItem.SetItem("ID", FID);
                    //XZItem.SetItem("Code", "");
                    XZItem.SetItem("Version", "0");
                    XZItem.SetItem("PurStatus", "已上传");
                    XZItem.SetItem("ModifyStatus", "");
                    XZItem.SetItem("WorkShopCode", purPlanMXList[i]["WorkShopCode"] == null ? "" : purPlanMXList[i]["WorkShopCode"].ToString());
                    XZItem.SetItem("SubCode", sSubCode);
                    XZItem.SetItem("EquipCode", sEquipCode);
                    XZItem.SetItem("MatchCode", purPlanMXList[i]["MatchCode"] == null ? "" : purPlanMXList[i]["MatchCode"].ToString());
                    XZItem.SetItem("AttachEquipCode", "");
                    XZItem.SetItem("EquipName", purPlanMXList[i]["EquipName"] == null ? "" : purPlanMXList[i]["EquipName"].ToString());
                    XZItem.SetItem("EquipLongCode", purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString());
                    XZItem.SetItem("IsPur", purPlanMXList[i]["IsPur"] == null ? "" : purPlanMXList[i]["IsPur"].ToString());
                    XZItem.SetItem("Unit", purPlanMXList[i]["NextUnit"] == null ? "" : purPlanMXList[i]["NextUnit"].ToString());
                    XZItem.SetItem("Spec", purPlanMXList[i]["Spec"] == null ? "" : purPlanMXList[i]["Spec"].ToString());
                    XZItem.SetItem("Num", purPlanMXList[i]["Num"] == null ? "" : purPlanMXList[i]["Num"].ToString());
                    XZItem.SetItem("TotalWeight", purPlanMXList[i]["TotalWeight"] == null ? "" : purPlanMXList[i]["TotalWeight"].ToString());
                    XZItem.SetItem("TotalPower", purPlanMXList[i]["TotalPower"] == null ? "" : purPlanMXList[i]["TotalPower"].ToString());
                    XZItem.SetItem("UseGas", purPlanMXList[i]["UseGas"] == null ? "" : purPlanMXList[i]["UseGas"].ToString());
                    XZItem.SetItem("UseWater", purPlanMXList[i]["UseWater"] == null ? "" : purPlanMXList[i]["UseWater"].ToString());
                    XZItem.SetItem("Remark", purPlanMXList[i]["Remark"] == null ? "" : purPlanMXList[i]["Remark"].ToString());
                    //XZItem.SetItem("EquipID", "");
                    XZItem.SetItem("PurBomCode", Code);
                    XZItem.SetItem("PurBomID", ID);
                    XZItem.SetItem("OriginPurBomCode", Code);
                    XZItem.SetItem("OriginPurBomID", ID);
                    //XZItem.SetItem("MainEquipID", "");
                    XZItem.SetItem("ProfName", ProfName);
                    XZItem.SetItem("ProfCode", ProfCode);
                    XZItem.SetItem("ProfID", ProfID);
                    XZItem.SetItem("Sequ", purPlanMXList[i]["Sequ"] == null ? "0" : purPlanMXList[i]["Sequ"].ToString());
                    XZItem.SetItem("PurPlanListID", sParentID);
                    XZItem.SetItem("CompositionCode", sCompositionCode);
                    XZItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    int iCount = 100;
                    Power.Business.IBaseBusiness MXItemMain = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                    Guid MXMainID = Guid.NewGuid();
                    MXItemMain.SetItem("ID", MXMainID);
                    MXItemMain.SetItem("PropertyCode", (iCount * 100).ToString());
                    MXItemMain.SetItem("PropertyName", purPlanMXList[i]["FeatureName"] == null ? "" : purPlanMXList[i]["FeatureName"].ToString());
                    MXItemMain.SetItem("PropertyValue", purPlanMXList[i]["FeatureValue"] == null ? "" : purPlanMXList[i]["FeatureValue"].ToString());
                    MXItemMain.SetItem("Unit", purPlanMXList[i]["NextUnit"] == null ? "" : purPlanMXList[i]["NextUnit"].ToString());
                    MXItemMain.SetItem("AttachEquipCode", sEquipCode);
                    MXItemMain.SetItem("Num", purPlanMXList[i]["Num"] == null ? "" : purPlanMXList[i]["Num"].ToString());
                    MXItemMain.SetItem("Version", "0");
                    MXItemMain.SetItem("PropertyStatus", "");
                    MXItemMain.SetItem("OriginID", purPlanMXList[i]["ID"] == null ? "" : purPlanMXList[i]["ID"].ToString());
                    MXItemMain.SetItem("FID", FID);
                    MXItemMain.SetItem("Sequ", purPlanMXList[i]["Sequ"] == null ? "0" : purPlanMXList[i]["Sequ"].ToString());
                    MXItemMain.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan_List");
                    Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" parentID='" + sParentID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    string sEquipName_KH_List = string.Empty;
                    for (int j = 0; j < purPlanMX_MXList.Count; j++)
                    {
                        iCount++;
                        Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                        Guid MXID = Guid.NewGuid();
                        MXItem.SetItem("ID", MXID);
                        MXItem.SetItem("PropertyCode", (iCount * 100).ToString());
                        MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                        MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                        MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                        MXItem.SetItem("AttachEquipCode", sEquipCode);
                        MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                        MXItem.SetItem("Version", "0");
                        MXItem.SetItem("PropertyStatus", "");
                        MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                        MXItem.SetItem("FID", FID);
                        MXItem.SetItem("Sequ", purPlanMX_MXList[j]["Sequ"] == null ? "0" : purPlanMX_MXList[j]["Sequ"].ToString());
                        MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                        if (j == 0)
                        {
                            string sEquipName_KH = purPlanMX_MXList[j]["EquipName"] == null ? "" : purPlanMX_MXList[j]["EquipName"].ToString();
                            if (!string.IsNullOrEmpty(sEquipName_KH))
                            {
                                sEquipName_KH = sEquipName_KH.TrimEnd(")").TrimEnd("）").TrimStart("(").TrimStart("（");//去除括号
                                sEquipName_KH_List += sEquipName_KH + ",";
                            }
                        }
                    }
                    sEquipName_KH_List = sEquipName_KH_List.TrimEnd(',');
                    string sEquipName = XZItem["EquipName"] == null ? "" : XZItem["EquipName"].ToString();
                    if (!string.IsNullOrEmpty(sEquipName_KH_List) && !string.IsNullOrEmpty(sEquipName))
                    {
                        XZItem.SetItem("EquipName", sEquipName + "(" + sEquipName_KH_List + ")");
                        XZItem.Save(System.ComponentModel.DataObjectMethodType.Update);
                    }
                }
                else if (string.IsNullOrEmpty(sEquipCode) && !string.IsNullOrEmpty(sMatchCode))//配套设备
                {
                    string sFID = purPlanMXList[i]["ParentID"].ToString();
                    Power.Business.IBusinessOperate paranetItem = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan_List");
                    Power.Business.IBusinessList paranetItem_List = paranetItem.FindAll(" ID='" + sFID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    string sParentSubCode = string.Empty;
                    string sParentEquipCode = string.Empty;
                    string sPurPlanListID = string.Empty;
                    if (paranetItem_List.Count > 0)
                    {
                        sParentSubCode = paranetItem_List[0]["SubCode"] == null ? "" : paranetItem_List[0]["SubCode"].ToString();
                        sParentEquipCode = paranetItem_List[0]["EquipCode"] == null ? "" : paranetItem_List[0]["EquipCode"].ToString();
                        sPurPlanListID = paranetItem_List[0]["ID"] == null ? "" : paranetItem_List[0]["ID"].ToString();//查出对应的设备材料表的ID
                    }

                    Power.Business.IBusinessOperate paranetEquip = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                    Power.Business.IBusinessList paranetEquipList = paranetEquip.FindAll(" PurPlanListID='" + sPurPlanListID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    string sPUR_EquipBOMID = string.Empty;
                    if (paranetEquipList.Count > 0)
                    {
                        sPUR_EquipBOMID = paranetEquipList[0]["ID"] == null ? "" : paranetEquipList[0]["ID"].ToString();//查出配套设备对应的主设备的ID
                    }

                    Power.Business.IBaseBusiness XZItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM");
                    Guid FID = Guid.NewGuid();
                    XZItem.SetItem("ID", FID);
                    //XZItem.SetItem("Code", "");
                    XZItem.SetItem("Version", "0");
                    XZItem.SetItem("PurStatus", "已上传");
                    XZItem.SetItem("ModifyStatus", "");
                    XZItem.SetItem("WorkShopCode", purPlanMXList[i]["WorkShopCode"] == null ? "" : purPlanMXList[i]["WorkShopCode"].ToString());
                    XZItem.SetItem("SubCode", sParentSubCode);
                    XZItem.SetItem("EquipCode", sParentEquipCode);
                    XZItem.SetItem("MatchCode", sMatchCode);
                    XZItem.SetItem("AttachEquipCode", sParentEquipCode);//主设备编号
                    XZItem.SetItem("EquipName", purPlanMXList[i]["EquipName"] == null ? "" : purPlanMXList[i]["EquipName"].ToString());
                    XZItem.SetItem("EquipLongCode", purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString());
                    XZItem.SetItem("IsPur", purPlanMXList[i]["IsPur"] == null ? "" : purPlanMXList[i]["IsPur"].ToString());
                    XZItem.SetItem("Unit", purPlanMXList[i]["NextUnit"] == null ? "" : purPlanMXList[i]["NextUnit"].ToString());
                    XZItem.SetItem("Spec", purPlanMXList[i]["Spec"] == null ? "" : purPlanMXList[i]["Spec"].ToString());
                    XZItem.SetItem("Num", purPlanMXList[i]["Num"] == null ? "" : purPlanMXList[i]["Num"].ToString());
                    XZItem.SetItem("TotalWeight", purPlanMXList[i]["TotalWeight"] == null ? "" : purPlanMXList[i]["TotalWeight"].ToString());
                    XZItem.SetItem("TotalPower", purPlanMXList[i]["TotalPower"] == null ? "" : purPlanMXList[i]["TotalPower"].ToString());
                    XZItem.SetItem("UseGas", purPlanMXList[i]["UseGas"] == null ? "" : purPlanMXList[i]["UseGas"].ToString());
                    XZItem.SetItem("UseWater", purPlanMXList[i]["UseWater"] == null ? "" : purPlanMXList[i]["UseWater"].ToString());
                    XZItem.SetItem("Remark", purPlanMXList[i]["Remark"] == null ? "" : purPlanMXList[i]["Remark"].ToString());
                    //XZItem.SetItem("EquipID", "");
                    XZItem.SetItem("PurBomCode", Code);
                    XZItem.SetItem("PurBomID", ID);
                    XZItem.SetItem("OriginPurBomCode", Code);
                    XZItem.SetItem("OriginPurBomID", ID);
                    //XZItem.SetItem("MainEquipID", "");
                    XZItem.SetItem("ProfName", ProfName);
                    XZItem.SetItem("ProfCode", ProfCode);
                    XZItem.SetItem("ProfID", ProfID);
                    XZItem.SetItem("Sequ", purPlanMXList[i]["Sequ"] == null ? "0" : purPlanMXList[i]["Sequ"].ToString());
                    XZItem.SetItem("ParentID", sPUR_EquipBOMID);
                    XZItem.SetItem("PurPlanListID", sParentID);
                    XZItem.SetItem("CompositionCode", sCompositionCode);
                    XZItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    int iCount = 100;
                    Power.Business.IBaseBusiness MXItemMain = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                    Guid MXMainID = Guid.NewGuid();
                    MXItemMain.SetItem("ID", MXMainID);
                    MXItemMain.SetItem("PropertyCode", (iCount * 100).ToString());
                    MXItemMain.SetItem("PropertyName", purPlanMXList[i]["FeatureName"] == null ? "" : purPlanMXList[i]["FeatureName"].ToString());
                    MXItemMain.SetItem("PropertyValue", purPlanMXList[i]["FeatureValue"] == null ? "" : purPlanMXList[i]["FeatureValue"].ToString());
                    MXItemMain.SetItem("Unit", purPlanMXList[i]["NextUnit"] == null ? "" : purPlanMXList[i]["NextUnit"].ToString());
                    MXItemMain.SetItem("AttachEquipCode", sEquipCode);
                    MXItemMain.SetItem("Num", purPlanMXList[i]["Num"] == null ? "" : purPlanMXList[i]["Num"].ToString());
                    MXItemMain.SetItem("Version", "0");
                    MXItemMain.SetItem("PropertyStatus", "");
                    MXItemMain.SetItem("OriginID", purPlanMXList[i]["ID"] == null ? "" : purPlanMXList[i]["ID"].ToString());
                    MXItemMain.SetItem("FID", FID);
                    MXItemMain.SetItem("Sequ", purPlanMXList[i]["Sequ"] == null ? "0" : purPlanMXList[i]["Sequ"].ToString());
                    MXItemMain.Save(System.ComponentModel.DataObjectMethodType.Insert);

                    Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan_List");
                    Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" parentID='" + sParentID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    for (int j = 0; j < purPlanMX_MXList.Count; j++)
                    {
                        iCount++;
                        Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                        Guid MXID = Guid.NewGuid();
                        MXItem.SetItem("ID", MXID);
                        MXItem.SetItem("PropertyCode", (iCount * 100).ToString());
                        MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                        MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                        MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                        MXItem.SetItem("AttachEquipCode", sEquipCode);
                        MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                        MXItem.SetItem("Version", "0");
                        MXItem.SetItem("PropertyStatus", "");
                        MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                        MXItem.SetItem("FID", FID);
                        MXItem.SetItem("Sequ", purPlanMX_MXList[j]["Sequ"] == null ? "0" : purPlanMX_MXList[j]["Sequ"].ToString());
                        MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    }
                }
            }

        }

        //业务对象 NPS_PUR_PurPlan
        public string GetEquipBOMList(string sPurBomCode)
        {
            List<object> result = new List<object>();
            string SQL = "select * from NPS_PUR_EquipBOM where PurBomCode = '" + sPurBomCode + "'";
            DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                result.Add(dataTable.Rows[i]);
                string sID = dataTable.Rows[i]["ID"].ToString().Trim();
                string SQLList = "select * from NPS_PUR_EquipBOM_List where FID = '" + sID + "'";
                DataTable ds = XCode.DataAccessLayer.DAL.QuerySQL(SQLList);
                for (int j = 0; j < ds.Rows.Count; j++)
                {
                    result.Add(ds.Rows[j]);
                }
            }
            JsonSerializerSettings s_setting = new JsonSerializerSettings();
            string sResult = JsonConvert.SerializeObject(result, s_setting);
            return sResult;
        }

        //业务对象 NPS_PUR_PurPlanUploadModify
        public void UpdateEquipBOM(string ID)
        {
            string Code = string.Empty;
            string ProfName = string.Empty;
            string ProfCode = string.Empty;
            string ProfID = string.Empty;
            Power.Business.IBusinessOperate purPlanMain = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify");
            Power.Business.IBusinessList purPlanMainList = purPlanMain.FindAll(" ID='" + ID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (purPlanMainList.Count > 0)
            {
                Code = purPlanMainList[0]["Code"].ToString();
                ProfName = purPlanMainList[0]["ProfName"] == null ? "" : purPlanMainList[0]["ProfName"].ToString();
                ProfCode = purPlanMainList[0]["ProfCode"] == null ? "" : purPlanMainList[0]["ProfCode"].ToString();
                ProfID = purPlanMainList[0]["ProfID"] == null ? "" : purPlanMainList[0]["ProfID"].ToString();
            }

            Power.Business.IBusinessOperate purPlanMX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
            Power.Business.IBusinessList purPlanMXList = purPlanMX.FindAll(" FID='" + ID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            for (int i = 0; i < purPlanMXList.Count; i++)
            {
                string sParentID = purPlanMXList[i]["ID"].ToString();
                string sEquipCode = purPlanMXList[i]["EquipCode"] == null ? "" : purPlanMXList[i]["EquipCode"].ToString();
                string sMatchCode = purPlanMXList[i]["MatchCode"] == null ? "" : purPlanMXList[i]["MatchCode"].ToString();
                string sUploadDataType = purPlanMXList[i]["uploadDataType"] == null ? "" : purPlanMXList[i]["uploadDataType"].ToString();
                //string sFeatureName = purPlanMXList[i]["FeatureName"] == null ? "" : purPlanMXList[i]["FeatureName"].ToString();
                if (!string.IsNullOrEmpty(sEquipCode))//主设备
                {
                    //主设备新增，则认为其子表也都是新增
                    if (sUploadDataType.Equals("add"))
                    {
                        #region 主设备新增

                        Power.Business.IBaseBusiness XZItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM");
                        Guid FID = Guid.NewGuid();
                        XZItem.SetItem("ID", FID);
                        //XZItem.SetItem("Code", "");
                        XZItem.SetItem("Version", "0");
                        XZItem.SetItem("PurStatus", "已上传");
                        XZItem.SetItem("ModifyStatus", "新增");
                        XZItem.SetItem("WorkShopCode", purPlanMXList[i]["WorkShopCode"] == null ? "" : purPlanMXList[i]["WorkShopCode"].ToString());
                        XZItem.SetItem("SubCode", purPlanMXList[i]["SubCode"] == null ? "" : purPlanMXList[i]["SubCode"].ToString());
                        XZItem.SetItem("EquipCode", sEquipCode);
                        XZItem.SetItem("MatchCode", purPlanMXList[i]["MatchCode"] == null ? "" : purPlanMXList[i]["MatchCode"].ToString());
                        XZItem.SetItem("AttachEquipCode", "");
                        XZItem.SetItem("EquipName", purPlanMXList[i]["EquipName"] == null ? "" : purPlanMXList[i]["EquipName"].ToString());
                        XZItem.SetItem("EquipLongCode", purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString());
                        XZItem.SetItem("IsPur", purPlanMXList[i]["IsPur"] == null ? "" : purPlanMXList[i]["IsPur"].ToString());
                        XZItem.SetItem("Unit", purPlanMXList[i]["NextUnit"] == null ? "" : purPlanMXList[i]["NextUnit"].ToString());
                        XZItem.SetItem("Spec", purPlanMXList[i]["Spec"] == null ? "" : purPlanMXList[i]["Spec"].ToString());
                        XZItem.SetItem("Num", purPlanMXList[i]["Num"] == null ? "" : purPlanMXList[i]["Num"].ToString());
                        XZItem.SetItem("TotalWeight", purPlanMXList[i]["TotalWeight"] == null ? "" : purPlanMXList[i]["TotalWeight"].ToString());
                        XZItem.SetItem("TotalPower", purPlanMXList[i]["TotalPower"] == null ? "" : purPlanMXList[i]["TotalPower"].ToString());
                        XZItem.SetItem("UseGas", purPlanMXList[i]["UseGas"] == null ? "" : purPlanMXList[i]["UseGas"].ToString());
                        XZItem.SetItem("UseWater", purPlanMXList[i]["UseWater"] == null ? "" : purPlanMXList[i]["UseWater"].ToString());
                        XZItem.SetItem("Remark", purPlanMXList[i]["Remark"] == null ? "" : purPlanMXList[i]["Remark"].ToString());
                        XZItem.SetItem("PurBomCode", Code);
                        XZItem.SetItem("PurBomID", ID);
                        XZItem.SetItem("OriginPurBomCode", Code);
                        XZItem.SetItem("OriginPurBomID", ID);
                        XZItem.SetItem("ProfName", ProfName);
                        XZItem.SetItem("ProfCode", ProfCode);
                        XZItem.SetItem("ProfID", ProfID);
                        XZItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                        int iCount = 100;
                        Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                        Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" (parentID='" + sParentID + "' or ID = '" + sParentID + "'  )", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        for (int j = 0; j < purPlanMX_MXList.Count; j++)
                        {
                            Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                            Guid MXID = Guid.NewGuid();
                            MXItem.SetItem("ID", MXID);
                            MXItem.SetItem("PropertyCode", (iCount * 100).ToString());
                            MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                            MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                            MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                            MXItem.SetItem("AttachEquipCode", sEquipCode);
                            MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                            MXItem.SetItem("Version", "0");
                            MXItem.SetItem("PropertyStatus", "新增");
                            MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                            MXItem.SetItem("FID", FID);
                            MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }

                        #endregion
                    }
                    else if (sUploadDataType.Equals("remove"))
                    {
                        string sEquipLongCode = purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString();

                        Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                        Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        if (EquipBOMList.Count <= 0)
                        {
                            continue;
                        }
                        EquipBOMList[0].SetItem("ModifyStatus", "删除");
                        if (EquipBOMList[0]["PurBomCode"] == null || EquipBOMList[0]["PurBomCode"].ToString().Trim().Equals(""))
                        {
                            EquipBOMList[0].SetItem("PurBomCode", Code);
                        }
                        else
                        {
                            EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                        }
                        if (EquipBOMList[0]["PurBomID"] == null || EquipBOMList[0]["PurBomID"].ToString().Trim().Equals(""))
                        {
                            EquipBOMList[0].SetItem("PurBomID", ID);
                        }
                        else
                        {
                            EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                        }
                        //EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code); 
                        //EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID); 
                        EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);

                        Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                        Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" (parentID='" + sParentID + "' or ID = '" + sParentID + "'  )", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        for (int j = 0; j < purPlanMX_MXList.Count; j++)
                        {
                            string sFeatureName = purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString().Trim();

                            string sMaxPropertyCode = string.Empty;
                            string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                            SQL += "from NPS_PUR_EquipBOM_List A ";
                            SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                            SQL += "where B.EquipLongCode = '" + sEquipLongCode + "' and A.PropertyName = '" + sFeatureName + "' ";
                            SQL += "group by B.EquipLongCode,A.PropertyName ";
                            DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                            if (dataTable.Rows.Count > 0)
                            {
                                sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 1).ToString();
                            }

                            Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                            Guid MXID = Guid.NewGuid();
                            MXItem.SetItem("ID", MXID);
                            MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                            MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                            MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                            MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                            MXItem.SetItem("AttachEquipCode", sEquipCode);
                            MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                            MXItem.SetItem("Version", "0");
                            MXItem.SetItem("PropertyStatus", "删除");
                            MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                            MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString().Trim());
                            MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }

                    }
                    else if (sUploadDataType.Equals("modify"))
                    {
                        string sEquipLongCode = purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString();

                        Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                        Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        if (EquipBOMList.Count > 0)
                        {
                            EquipBOMList[0].SetItem("ModifyStatus", "修改");
                            if (EquipBOMList[0]["PurBomCode"] == null || EquipBOMList[0]["PurBomCode"].ToString().Trim().Equals(""))
                            {
                                EquipBOMList[0].SetItem("PurBomCode", Code);
                            }
                            else
                            {
                                EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                            }
                            if (EquipBOMList[0]["PurBomID"] == null || EquipBOMList[0]["PurBomID"].ToString().Trim().Equals(""))
                            {
                                EquipBOMList[0].SetItem("PurBomID", ID);
                            }
                            else
                            {
                                EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                            }
                            //EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                            //EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                            EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);

                            #region 修改子表 包括子表增删改

                            Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                            Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" (parentID='" + sParentID + "' or ID = '" + sParentID + "'  )", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            for (int j = 0; j < purPlanMX_MXList.Count; j++)
                            {
                                string sFeatureName = purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString();
                                string sUploadDataType_MX = purPlanMX_MXList[j]["uploadDataType"] == null ? "" : purPlanMX_MXList[j]["uploadDataType"].ToString();
                                if (sUploadDataType_MX.Equals("remove"))
                                {
                                    string sMaxPropertyCode = string.Empty;
                                    string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                                    SQL += "from NPS_PUR_EquipBOM_List A ";
                                    SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                                    SQL += "where B.EquipLongCode = '" + sEquipLongCode + "' and A.PropertyName = '" + sFeatureName + "' ";
                                    SQL += "group by B.EquipLongCode,A.PropertyName ";
                                    DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                                    if (dataTable.Rows.Count > 0)
                                    {
                                        sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 1).ToString();
                                    }

                                    Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                                    Guid MXID = Guid.NewGuid();
                                    MXItem.SetItem("ID", MXID);
                                    MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                                    MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                                    MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                                    MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                                    MXItem.SetItem("AttachEquipCode", sEquipCode);
                                    MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                                    MXItem.SetItem("Version", "0");
                                    MXItem.SetItem("PropertyStatus", "删除");
                                    MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                                    MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString().Trim());
                                    MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                }
                                else if (sUploadDataType_MX.Equals("add"))
                                {
                                    string sMaxPropertyCode = string.Empty;
                                    string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                                    SQL += "from NPS_PUR_EquipBOM_List A ";
                                    SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                                    SQL += "where B.EquipLongCode = '" + sEquipLongCode + "'";
                                    SQL += "group by B.EquipLongCode,A.PropertyName ";
                                    DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                                    if (dataTable.Rows.Count > 0)
                                    {
                                        sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 100).ToString();
                                    }

                                    Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                                    Guid MXID = Guid.NewGuid();
                                    MXItem.SetItem("ID", MXID);
                                    MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                                    MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                                    MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                                    MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                                    MXItem.SetItem("AttachEquipCode", sEquipCode);
                                    MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                                    MXItem.SetItem("Version", "0");
                                    MXItem.SetItem("PropertyStatus", "新增");
                                    MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                                    MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString());
                                    MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                }
                                else if (sUploadDataType_MX.Equals("modify"))
                                {
                                    string sMaxPropertyCode = string.Empty;
                                    string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                                    SQL += "from NPS_PUR_EquipBOM_List A ";
                                    SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                                    SQL += "where B.EquipLongCode = '" + sEquipLongCode + "' and A.PropertyName = '" + sFeatureName + "' ";
                                    SQL += "group by B.EquipLongCode,A.PropertyName ";
                                    DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                                    if (dataTable.Rows.Count > 0)
                                    {
                                        sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 1).ToString();
                                    }

                                    Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                                    Guid MXID = Guid.NewGuid();
                                    MXItem.SetItem("ID", MXID);
                                    MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                                    MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                                    MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                                    MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                                    MXItem.SetItem("AttachEquipCode", sEquipCode);
                                    MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                                    MXItem.SetItem("Version", "0");
                                    MXItem.SetItem("PropertyStatus", "修改");
                                    MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                                    MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString().Trim());
                                    MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                }
                            }

                            #endregion

                        }

                    }
                }
                else if (string.IsNullOrEmpty(sEquipCode) && !string.IsNullOrEmpty(sMatchCode))//配套设备
                {
                    //配套设备新增，则认为其子表也都是新增
                    if (sUploadDataType.Equals("add"))
                    {
                        #region 配套设备新增

                        string sFID = purPlanMXList[i]["ParentID"].ToString();
                        Power.Business.IBusinessOperate paranetItem = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan_List");
                        Power.Business.IBusinessList paranetItem_List = paranetItem.FindAll(" ID='" + sFID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        string sParentSubCode = string.Empty;
                        string sParentEquipCode = string.Empty;
                        if (paranetItem_List.Count > 0)
                        {
                            sParentSubCode = paranetItem_List[0]["SubCode"] == null ? "" : paranetItem_List[0]["SubCode"].ToString();
                            sParentEquipCode = paranetItem_List[0]["EquipCode"] == null ? "" : paranetItem_List[0]["EquipCode"].ToString();
                        }

                        Power.Business.IBaseBusiness XZItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM");
                        Guid FID = Guid.NewGuid();
                        XZItem.SetItem("ID", FID);
                        //XZItem.SetItem("Code", "");
                        XZItem.SetItem("Version", "0");
                        XZItem.SetItem("PurStatus", "已上传");
                        XZItem.SetItem("ModifyStatus", "新增");
                        XZItem.SetItem("WorkShopCode", purPlanMXList[i]["WorkShopCode"] == null ? "" : purPlanMXList[i]["WorkShopCode"].ToString());
                        XZItem.SetItem("SubCode", sParentSubCode);
                        XZItem.SetItem("EquipCode", sParentEquipCode);
                        XZItem.SetItem("MatchCode", purPlanMXList[i]["MatchCode"] == null ? "" : purPlanMXList[i]["MatchCode"].ToString());
                        XZItem.SetItem("AttachEquipCode", sParentEquipCode);//主设备编号
                        XZItem.SetItem("EquipName", purPlanMXList[i]["EquipName"] == null ? "" : purPlanMXList[i]["EquipName"].ToString());
                        XZItem.SetItem("EquipLongCode", purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString());
                        XZItem.SetItem("IsPur", purPlanMXList[i]["IsPur"] == null ? "" : purPlanMXList[i]["IsPur"].ToString());
                        XZItem.SetItem("Unit", purPlanMXList[i]["NextUnit"] == null ? "" : purPlanMXList[i]["NextUnit"].ToString());
                        XZItem.SetItem("Spec", purPlanMXList[i]["Spec"] == null ? "" : purPlanMXList[i]["Spec"].ToString());
                        XZItem.SetItem("Num", purPlanMXList[i]["Num"] == null ? "" : purPlanMXList[i]["Num"].ToString());
                        XZItem.SetItem("TotalWeight", purPlanMXList[i]["TotalWeight"] == null ? "" : purPlanMXList[i]["TotalWeight"].ToString());
                        XZItem.SetItem("TotalPower", purPlanMXList[i]["TotalPower"] == null ? "" : purPlanMXList[i]["TotalPower"].ToString());
                        XZItem.SetItem("UseGas", purPlanMXList[i]["UseGas"] == null ? "" : purPlanMXList[i]["UseGas"].ToString());
                        XZItem.SetItem("UseWater", purPlanMXList[i]["UseWater"] == null ? "" : purPlanMXList[i]["UseWater"].ToString());
                        XZItem.SetItem("Remark", purPlanMXList[i]["Remark"] == null ? "" : purPlanMXList[i]["Remark"].ToString());
                        //XZItem.SetItem("EquipID", "");
                        XZItem.SetItem("PurBomCode", Code);//变更的编号
                        XZItem.SetItem("PurBomID", ID);
                        XZItem.SetItem("OriginPurBomCode", Code);//最原始的编号
                        XZItem.SetItem("OriginPurBomID", ID);
                        //XZItem.SetItem("MainEquipID", "");
                        XZItem.SetItem("ProfName", ProfName);
                        XZItem.SetItem("ProfCode", ProfCode);
                        XZItem.SetItem("ProfID", ProfID);
                        XZItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                        int iCount = 100;
                        Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlan_List");
                        Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" (parentID='" + sParentID + "' or ID = '" + sParentID + "'  )", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        for (int j = 0; j < purPlanMX_MXList.Count; j++)
                        {
                            iCount++;
                            Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                            Guid MXID = Guid.NewGuid();
                            MXItem.SetItem("ID", MXID);
                            MXItem.SetItem("PropertyCode", (iCount * 100).ToString());
                            MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                            MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                            MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                            MXItem.SetItem("AttachEquipCode", sEquipCode);
                            MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                            MXItem.SetItem("Version", "0");
                            MXItem.SetItem("PropertyStatus", "新增");
                            MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                            MXItem.SetItem("FID", FID);
                            MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }

                        #endregion
                    }
                    else if (sUploadDataType.Equals("remove"))
                    {
                        string sEquipLongCode = purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString();

                        Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                        Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        if (EquipBOMList.Count <= 0)
                        {
                            continue;
                        }
                        EquipBOMList[0].SetItem("ModifyStatus", "删除");

                        if (EquipBOMList[0]["PurBomCode"] == null || EquipBOMList[0]["PurBomCode"].ToString().Trim().Equals(""))
                        {
                            EquipBOMList[0].SetItem("PurBomCode", Code);
                        }
                        else
                        {
                            EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                        }
                        if (EquipBOMList[0]["PurBomID"] == null || EquipBOMList[0]["PurBomID"].ToString().Trim().Equals(""))
                        {
                            EquipBOMList[0].SetItem("PurBomID", ID);
                        }
                        else
                        {
                            EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                        }
                        //EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                        //EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                        EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);

                        Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                        Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" (parentID='" + sParentID + "' or ID = '" + sParentID + "'  )", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        for (int j = 0; j < purPlanMX_MXList.Count; j++)
                        {
                            string sFeatureName = purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString().Trim();

                            string sMaxPropertyCode = string.Empty;
                            string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                            SQL += "from NPS_PUR_EquipBOM_List A ";
                            SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                            SQL += "where B.EquipLongCode = '" + sEquipLongCode + "' and A.PropertyName = '" + sFeatureName + "' ";
                            SQL += "group by B.EquipLongCode,A.PropertyName ";
                            DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                            if (dataTable.Rows.Count > 0)
                            {
                                sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 1).ToString();
                            }

                            Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                            Guid MXID = Guid.NewGuid();
                            MXItem.SetItem("ID", MXID);
                            MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                            MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                            MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                            MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                            MXItem.SetItem("AttachEquipCode", sEquipCode);
                            MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                            MXItem.SetItem("Version", "0");
                            MXItem.SetItem("PropertyStatus", "删除");
                            MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                            MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString().Trim());
                            MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                        }
                    }
                    else if (sUploadDataType.Equals("modify"))
                    {
                        string sEquipLongCode = purPlanMXList[i]["EquipLongCode"] == null ? "" : purPlanMXList[i]["EquipLongCode"].ToString();

                        Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                        Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                        if (EquipBOMList.Count > 0)
                        {
                            EquipBOMList[0].SetItem("ModifyStatus", "修改");
                            if (EquipBOMList[0]["PurBomCode"] == null || EquipBOMList[0]["PurBomCode"].ToString().Trim().Equals(""))
                            {
                                EquipBOMList[0].SetItem("PurBomCode", Code);
                            }
                            else
                            {
                                EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                            }
                            if (EquipBOMList[0]["PurBomID"] == null || EquipBOMList[0]["PurBomID"].ToString().Trim().Equals(""))
                            {
                                EquipBOMList[0].SetItem("PurBomID", ID);
                            }
                            else
                            {
                                EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                            }
                            //EquipBOMList[0].SetItem("PurBomCode", EquipBOMList[0]["PurBomCode"].ToString().Trim() + "," + Code);
                            //EquipBOMList[0].SetItem("PurBomID", EquipBOMList[0]["PurBomID"].ToString().Trim() + "," + ID);
                            EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);

                            #region 修改子表 包括子表增删改

                            Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                            Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" (parentID='" + sParentID + "' or ID = '" + sParentID + "'  )", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                            for (int j = 0; j < purPlanMX_MXList.Count; j++)
                            {
                                string sFeatureName = purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString();
                                string sUploadDataType_MX = purPlanMX_MXList[j]["uploadDataType"] == null ? "" : purPlanMX_MXList[j]["uploadDataType"].ToString();
                                if (sUploadDataType_MX.Equals("remove"))
                                {
                                    string sMaxPropertyCode = string.Empty;
                                    string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                                    SQL += "from NPS_PUR_EquipBOM_List A ";
                                    SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                                    SQL += "where B.EquipLongCode = '" + sEquipLongCode + "' and A.PropertyName = '" + sFeatureName + "' ";
                                    SQL += "group by B.EquipLongCode,A.PropertyName ";
                                    DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                                    if (dataTable.Rows.Count > 0)
                                    {
                                        sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 1).ToString();
                                    }

                                    Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                                    Guid MXID = Guid.NewGuid();
                                    MXItem.SetItem("ID", MXID);
                                    MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                                    MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                                    MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                                    MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                                    MXItem.SetItem("AttachEquipCode", sEquipCode);
                                    MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                                    MXItem.SetItem("Version", "0");
                                    MXItem.SetItem("PropertyStatus", "删除");
                                    MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                                    MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString().Trim());
                                    MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                }
                                else if (sUploadDataType_MX.Equals("add"))
                                {
                                    string sMaxPropertyCode = string.Empty;
                                    string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                                    SQL += "from NPS_PUR_EquipBOM_List A ";
                                    SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                                    SQL += "where B.EquipLongCode = '" + sEquipLongCode + "'";
                                    SQL += "group by B.EquipLongCode,A.PropertyName ";
                                    DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                                    if (dataTable.Rows.Count > 0)
                                    {
                                        sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 100).ToString();
                                    }

                                    Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                                    Guid MXID = Guid.NewGuid();
                                    MXItem.SetItem("ID", MXID);
                                    MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                                    MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                                    MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                                    MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                                    MXItem.SetItem("AttachEquipCode", sEquipCode);
                                    MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                                    MXItem.SetItem("Version", "0");
                                    MXItem.SetItem("PropertyStatus", "新增");
                                    MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                                    MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString());
                                    MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                }
                                else if (sUploadDataType_MX.Equals("modify"))
                                {
                                    string sMaxPropertyCode = string.Empty;
                                    string SQL = "select Max(A.PropertyCode) as MaxPropertyCode ";
                                    SQL += "from NPS_PUR_EquipBOM_List A ";
                                    SQL += "left join NPS_PUR_EquipBOM B on B.ID = A.FID ";
                                    SQL += "where B.EquipLongCode = '" + sEquipLongCode + "' and A.PropertyName = '" + sFeatureName + "' ";
                                    SQL += "group by B.EquipLongCode,A.PropertyName ";
                                    DataTable dataTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                                    if (dataTable.Rows.Count > 0)
                                    {
                                        sMaxPropertyCode = (Convert.ToInt32(dataTable.Rows[0]["MaxPropertyCode"].ToString().Trim()) + 1).ToString();
                                    }

                                    Power.Business.IBaseBusiness MXItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PUR_EquipBOM_List");
                                    Guid MXID = Guid.NewGuid();
                                    MXItem.SetItem("ID", MXID);
                                    MXItem.SetItem("PropertyCode", sMaxPropertyCode);
                                    MXItem.SetItem("PropertyName", purPlanMX_MXList[j]["FeatureName"] == null ? "" : purPlanMX_MXList[j]["FeatureName"].ToString());
                                    MXItem.SetItem("PropertyValue", purPlanMX_MXList[j]["FeatureValue"] == null ? "" : purPlanMX_MXList[j]["FeatureValue"].ToString());
                                    MXItem.SetItem("Unit", purPlanMX_MXList[j]["NextUnit"] == null ? "" : purPlanMX_MXList[j]["NextUnit"].ToString());
                                    MXItem.SetItem("AttachEquipCode", sEquipCode);
                                    MXItem.SetItem("Num", purPlanMX_MXList[j]["Num"] == null ? "" : purPlanMX_MXList[j]["Num"].ToString());
                                    MXItem.SetItem("Version", "0");
                                    MXItem.SetItem("PropertyStatus", "修改");
                                    MXItem.SetItem("OriginID", purPlanMX_MXList[j]["ID"] == null ? "" : purPlanMX_MXList[j]["ID"].ToString());
                                    MXItem.SetItem("FID", EquipBOMList[0]["ID"].ToString().Trim());
                                    MXItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
                                }
                            }

                            #endregion

                        }
                    }
                }

            }
        }

        /// <summary>
        /// 业务对象 NPS_PUR_EquipBOM
        /// 设备材料表库 主表 更新设备按钮
        /// </summary>
        /// <param name="IDList">选择的ID集合 例子:ID1,ID2,ID3</param>
        public void UpdateEquipVersion(string IDList)
        {
            if (string.IsNullOrEmpty(IDList))
            {
                return;
            }
            string[] IDArry = IDList.Split(',');
            for (int i = 0; i < IDArry.Length; i++)
            {
                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" ID='" + IDArry[i] + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (EquipBOMList.Count <= 0)
                {
                    continue;
                }
                EquipBOMList[0].SetItem("Version", Convert.ToInt32(EquipBOMList[0]["Version"].ToString()) + 1);//版本+1
                EquipBOMList[0].SetItem("ModifyStatus", "");//清空 设备修改状态
                EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
            }
        }

        /// <summary>
        ///  业务对象 NPS_PUR_EquipBOM
        /// 设备材料表库 设备参数属性（子表） 更新设备按钮
        /// </summary>
        /// <param name="ID">主表ID</param>
        public void UpdateEquipList(string ID)
        {
            Power.Business.IBusinessOperate EquipBOMMX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM_List");
            Power.Business.IBusinessList EquipBOMMXList = EquipBOMMX.FindAll(" FID='" + ID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            //var Equiplist = EquipBOMMXList.Where(x => Convert.ToInt32(x["PropertyCode"].ToString()) % 100 == 0);
            for (int i = 0; i < EquipBOMMXList.Count; i++)
            {
                int iPropertyCode = Convert.ToInt32(EquipBOMMXList[i]["PropertyCode"].ToString().Trim());
                if (iPropertyCode % 100 == 0)//筛选主记录
                {
                    int iMaxPropertyCode = iPropertyCode + 100;

                    //找出最大的版本
                    string SQL = "select * from NPS_PUR_EquipBOM_List where FID='" + ID + "' and PropertyCode > " + iPropertyCode + " and PropertyCode<" + iMaxPropertyCode + "  order by PropertyCode desc ";
                    DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
                    if (dsTable.Rows.Count > 0)
                    {
                        EquipBOMMXList[i].SetItem("PropertyName", dsTable.Rows[0]["PropertyName"].ToString());
                        EquipBOMMXList[i].SetItem("PropertyValue", dsTable.Rows[0]["PropertyValue"].ToString());
                        EquipBOMMXList[i].SetItem("Unit", dsTable.Rows[0]["Unit"].ToString());
                        EquipBOMMXList[i].SetItem("Num", dsTable.Rows[0]["Num"].ToString());
                        EquipBOMMXList[i].SetItem("Version", Convert.ToInt32(EquipBOMMXList[i]["Version"].ToString()) + 1);      //版本为当前版本+1
                        EquipBOMMXList[i].SetItem("PropertyStatus", dsTable.Rows[0]["PropertyStatus"].ToString());        //属性状态不需要更新
                        //EquipBOMMXList[i].SetItem("OriginID", dsTable.Rows[0]["OriginID"].ToString());        //来源ID暂时不更新
                        EquipBOMMXList[i].Save(System.ComponentModel.DataObjectMethodType.Update);
                    }
                }
            }
        }

        /// <summary>
        /// 主机设备招标资料、辅机设备招标资料 业务对象 NPS_PUR_PurPlan
        /// 更新设备材料表库状态 为“已提资”
        /// </summary>
        /// <param name="ID"></param>
        public void UpdatePurStatusByZBZL(string ID)
        {
            string SQL = "select Z.ID,Z.Code,MX.EquipLongCode from NPS_PUR_PurPlan_List MX left join NPS_PUR_PurPlan Z on Z.ID = MX.FID where Z.ID = '" + ID + "' ";
            DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                string sEquipLongCode = dsTable.Rows[i]["EquipLongCode"].ToString();
                string sID = dsTable.Rows[i]["ID"].ToString();
                string sCode = dsTable.Rows[i]["Code"].ToString();
                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (EquipBOMList.Count > 0)
                {
                    EquipBOMList[0].SetItem("PurStatus", "已提资");
                    EquipBOMList[0].SetItem("OriginPurPlanID", sID);
                    EquipBOMList[0].SetItem("OriginPurPlanCode", sCode);
                    EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
        }

        /// <summary>
        /// 招标申请 业务对象 NPS_BID_BidApply
        /// 更新设备材料表库状态 为“已招标”
        /// </summary>
        /// <param name="ID"></param>
        public void UpdatePurStatusByZBSQ(string ID)
        {
            string SQL = "select Z.ID,Z.Code,MX.EquipLongCode from NPS_BID_BidApply_MatList MX left join NPS_BID_BidApply Z on Z.ID = MX.FID where Z.ID = '" + ID + "' ";
            DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                string sEquipLongCode = dsTable.Rows[i]["EquipLongCode"].ToString();
                string sID = dsTable.Rows[i]["ID"].ToString();
                string sCode = dsTable.Rows[i]["Code"].ToString();
                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (EquipBOMList.Count > 0)
                {
                    EquipBOMList[0].SetItem("PurStatus", "已招标");
                    EquipBOMList[0].SetItem("BidID", sID);
                    EquipBOMList[0].SetItem("BidCode", sCode);
                    EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
        }

        /// <summary>
        /// 商务评标记录 业务对象 NPS_BID_BusinessReviewRecord
        /// 更新设备材料表库状态 为“已定标”
        /// </summary>
        /// <param name="ID"></param>
        public void UpdatePurStatusBySWPBJL(string ID)
        {
            string SQL = "select BidID from NPS_BID_BusinessReviewRecord where ID = '" + ID + "' ";
            DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            if (dsTable.Rows.Count > 0)
            {
                string sBidID = dsTable.Rows[0]["BidID"].ToString();
                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" BidID='" + sBidID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                for (int i = 0; i < EquipBOMList.Count; i++)
                {
                    EquipBOMList[i].SetItem("PurStatus", "已定标");
                    EquipBOMList[i].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
        }

        /// <summary>
        /// 采购合同评审 业务对象 NPS_SUBCON_PurConReview
        /// 更新设备材料表库状态 为“已合同评审”
        /// </summary>
        /// <param name="ID"></param>
        public void UpdatePurStatusByCGHTPS(string ID)
        {
            string SQL = "select ID,Code,BidID from NPS_SUBCON_PurConReview where ID = '" + ID + "' ";
            DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            if (dsTable.Rows.Count > 0)
            {
                string sBidID = dsTable.Rows[0]["BidID"].ToString();
                string sID = dsTable.Rows[0]["ID"].ToString();
                string sCode = dsTable.Rows[0]["Code"].ToString();

                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" BidID='" + sBidID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                for (int i = 0; i < EquipBOMList.Count; i++)
                {
                    EquipBOMList[i].SetItem("PurStatus", "已合同评审");
                    EquipBOMList[i].SetItem("ConReviewID", sID);
                    EquipBOMList[i].SetItem("ConReviewCode", sCode);
                    EquipBOMList[i].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
        }

        /// <summary>
        /// 采购合同台账 业务对象 NPS_SUBCON_PurCon
        /// 更新设备材料表库状态 为“已签订合同”
        /// </summary>
        /// <param name="ID"></param>
        public void UpdatePurStatusByCGHTTZ(string ID)
        {
            string SQL = "select ID,Code,ConCode,ConReviewID,ConReviewCode from NPS_SUBCON_PurCon where ID = '" + ID + "' ";
            DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            if (dsTable.Rows.Count > 0)
            {
                string sConReviewID = dsTable.Rows[0]["ConReviewID"].ToString();
                string sID = dsTable.Rows[0]["ID"].ToString();
                string sConCode = dsTable.Rows[0]["ConCode"].ToString();

                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" ConReviewID='" + sConReviewID + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                for (int i = 0; i < EquipBOMList.Count; i++)
                {
                    EquipBOMList[i].SetItem("PurStatus", "已签订合同");
                    EquipBOMList[i].SetItem("ConID", sID);
                    EquipBOMList[i].SetItem("ConCode", sConCode);
                    EquipBOMList[i].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
        }

        /// <summary>
        /// 辅机设备招标资料 “更新确认”按钮 业务对象 NPS_PUR_PurPlanUploadModify
        /// 更新设备材料表库状态 为“已提资” 同时更新外键
        /// </summary>
        /// <param name="ID">主表ID</param>
        public void UpdatePurStatusByFJSBZBZL(string ID)
        {
            string SQL = "select Z.ID,Z.Code,MX.EquipLongCode from NPS_PUR_PurPlanUploadModify_List MX left join NPS_PUR_PurPlanUploadModify Z on Z.ID = MX.FID where Z.ID = '" + ID + "' ";
            DataTable dsTable = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                string sEquipLongCode = dsTable.Rows[i]["EquipLongCode"].ToString();
                string sID = dsTable.Rows[i]["ID"].ToString();
                string sCode = dsTable.Rows[i]["Code"].ToString();
                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" EquipLongCode='" + sEquipLongCode + "'  ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (EquipBOMList.Count > 0)
                {
                    EquipBOMList[0].SetItem("PurStatus", "已提资");
                    if (EquipBOMList[0]["PurPlanID"] == null || EquipBOMList[0]["PurPlanID"].ToString().Trim().Equals(""))
                    {
                        EquipBOMList[0].SetItem("PurPlanID", sID);
                    }
                    else
                    {
                        EquipBOMList[0].SetItem("PurPlanID", EquipBOMList[0]["PurPlanID"].ToString().Trim() + "," + sID);
                    }
                    if (EquipBOMList[0]["PurPlanCode"] == null || EquipBOMList[0]["PurPlanCode"].ToString().Trim().Equals(""))
                    {
                        EquipBOMList[0].SetItem("PurPlanCode", sCode);
                    }
                    else
                    {
                        EquipBOMList[0].SetItem("PurPlanCode", EquipBOMList[0]["PurPlanCode"].ToString().Trim() + "," + sCode);
                    }
                    EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                }
            }
        }

        /// <summary>
        /// 更新已有招标申请 业务对象 NPS_PUR_PurPlanUploadModify
        /// </summary>
        /// <param name="IDList">选中的子表集合</param>
        /// <param name="BidID">招标申请ID</param>
        /// <param name="BidCode">招标申请Code</param>
        public void InsertBidList(string IDList, string BidID, string BidCode)
        {
            string[] IDArr = IDList.Split(',');
            for (int i = 0; i < IDArr.Length; i++)
            {
                Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" ID = '" + IDArr[i] + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (purPlanMX_MXList.Count <= 0)
                {
                    continue;
                }
                Power.Business.IBaseBusiness XZItem = Power.Business.BusinessFactory.CreateBusiness("NPS_BID_BidApply_MatList");
                Guid ID = Guid.NewGuid();
                XZItem.SetItem("ID", ID);
                XZItem.SetItem("FID", BidID);
                XZItem.SetItem("EquipCode", purPlanMX_MXList[0]["EquipCode"] == null ? "" : purPlanMX_MXList[0]["EquipCode"].ToString().Trim());
                XZItem.SetItem("EquipName", purPlanMX_MXList[0]["EquipName"] == null ? "" : purPlanMX_MXList[0]["EquipName"].ToString().Trim());
                XZItem.SetItem("EquipLongCode", purPlanMX_MXList[0]["EquipLongCode"] == null ? "" : purPlanMX_MXList[0]["EquipLongCode"].ToString().Trim());
                XZItem.SetItem("MatCode", purPlanMX_MXList[0]["MatCode"] == null ? "" : purPlanMX_MXList[0]["MatCode"].ToString().Trim());
                XZItem.SetItem("MatName", purPlanMX_MXList[0]["MatName"] == null ? "" : purPlanMX_MXList[0]["MatName"].ToString().Trim());
                XZItem.SetItem("Unit", purPlanMX_MXList[0]["Unit"] == null ? "" : purPlanMX_MXList[0]["Unit"].ToString().Trim());
                XZItem.SetItem("Num", purPlanMX_MXList[0]["Num"] == null ? "0" : purPlanMX_MXList[0]["Num"].ToString().Trim());
                XZItem.SetItem("SubCode", purPlanMX_MXList[0]["SubCode"] == null ? "" : purPlanMX_MXList[0]["SubCode"].ToString().Trim());
                XZItem.SetItem("MatchCode", purPlanMX_MXList[0]["MatchCode"] == null ? "" : purPlanMX_MXList[0]["MatchCode"].ToString().Trim());
                XZItem.SetItem("IsPur", purPlanMX_MXList[0]["IsPur"] == null ? "" : purPlanMX_MXList[0]["IsPur"].ToString().Trim());
                XZItem.SetItem("OriginDataType", "变更");
                XZItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                if (purPlanMX_MXList[0]["OriginID"] == null || purPlanMX_MXList[0]["OriginID"].ToString().Trim().Equals(""))
                {
                    continue;
                }
                string sOriginID = purPlanMX_MXList[0]["OriginID"].ToString().Trim();

                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" ID = '" + sOriginID + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (EquipBOMList.Count <= 0)
                {
                    continue;
                }
                EquipBOMList[0].SetItem("PurStatus", "已招标");
                EquipBOMList[0].SetItem("BidID", BidID);
                EquipBOMList[0].SetItem("BidCode", BidCode);
                EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
            }
        }

        /// <summary>
        /// 更新已有合同评审 业务对象 NPS_PUR_PurPlanUploadModify
        /// </summary>
        /// <param name="IDList">选中的子表集合</param>
        /// <param name="ConReviewID">合同评审ID</param>
        /// <param name="ConReviewCode">合同评审Code</param>
        public void InsertConReviewList(string IDList, string ConReviewID, string ConReviewCode)
        {
            string[] IDArr = IDList.Split(',');
            for (int i = 0; i < IDArr.Length; i++)
            {
                Power.Business.IBusinessOperate purPlanMX_MX = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_PurPlanUploadModify_List");
                Power.Business.IBusinessList purPlanMX_MXList = purPlanMX_MX.FindAll(" ID = '" + IDArr[i] + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (purPlanMX_MXList.Count <= 0)
                {
                    continue;
                }
                Power.Business.IBaseBusiness XZItem = Power.Business.BusinessFactory.CreateBusiness("NPS_SUBCON_PurConReview_MatList");
                Guid ID = Guid.NewGuid();
                XZItem.SetItem("ID", ID);
                XZItem.SetItem("FID", ConReviewID);
                XZItem.SetItem("EquipCode", purPlanMX_MXList[0]["EquipCode"] == null ? "" : purPlanMX_MXList[0]["EquipCode"].ToString().Trim());
                XZItem.SetItem("EquipName", purPlanMX_MXList[0]["EquipName"] == null ? "" : purPlanMX_MXList[0]["EquipName"].ToString().Trim());
                XZItem.SetItem("EquipLongCode", purPlanMX_MXList[0]["EquipLongCode"] == null ? "" : purPlanMX_MXList[0]["EquipLongCode"].ToString().Trim());
                XZItem.SetItem("MatCode", purPlanMX_MXList[0]["MatCode"] == null ? "" : purPlanMX_MXList[0]["MatCode"].ToString().Trim());
                XZItem.SetItem("MatName", purPlanMX_MXList[0]["MatName"] == null ? "" : purPlanMX_MXList[0]["MatName"].ToString().Trim());
                XZItem.SetItem("Unit", purPlanMX_MXList[0]["Unit"] == null ? "" : purPlanMX_MXList[0]["Unit"].ToString().Trim());
                XZItem.SetItem("Num", purPlanMX_MXList[0]["Num"] == null ? "0" : purPlanMX_MXList[0]["Num"].ToString().Trim());
                XZItem.SetItem("SubCode", purPlanMX_MXList[0]["SubCode"] == null ? "" : purPlanMX_MXList[0]["SubCode"].ToString().Trim());
                XZItem.SetItem("MatchCode", purPlanMX_MXList[0]["MatchCode"] == null ? "" : purPlanMX_MXList[0]["MatchCode"].ToString().Trim());
                XZItem.SetItem("IsPur", purPlanMX_MXList[0]["IsPur"] == null ? "" : purPlanMX_MXList[0]["IsPur"].ToString().Trim());
                XZItem.SetItem("OriginDataType", "变更");
                XZItem.Save(System.ComponentModel.DataObjectMethodType.Insert);

                if (purPlanMX_MXList[0]["OriginID"] == null || purPlanMX_MXList[0]["OriginID"].ToString().Trim().Equals(""))
                {
                    continue;
                }
                string sOriginID = purPlanMX_MXList[0]["OriginID"].ToString().Trim();

                Power.Business.IBusinessOperate EquipBOM = Power.Business.BusinessFactory.CreateBusinessOperate("NPS_PUR_EquipBOM");
                Power.Business.IBusinessList EquipBOMList = EquipBOM.FindAll(" ID = '" + sOriginID + "' ", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                if (EquipBOMList.Count <= 0)
                {
                    continue;
                }
                EquipBOMList[0].SetItem("PurStatus", "已合同评审");
                EquipBOMList[0].SetItem("ConReviewID", ConReviewID);
                EquipBOMList[0].SetItem("ConReviewCode", ConReviewCode);
                EquipBOMList[0].Save(System.ComponentModel.DataObjectMethodType.Update);
            }
        }

        #endregion

        #region 中心代码

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
                    SubResult.Delete();
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

            return "ok";
        }

        /// <summary>
        /// 按规则获取CompanyCode   SPI-四位流水号 NPS_TEAM_PersonOpt
        /// </summary>
        /// <returns></returns>
        public string GetCompanyCode()
        {
            string sResult = string.Empty;
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = @" select Max(CompanyCode) as CompanyCode from NPS_TEAM_PersonOpt where CompanyCode like 'SPI-%' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["CompanyCode"].ToString().Trim()))
            {
                string sCompanyCode = dataSet.Tables[0].Rows[0]["CompanyCode"].ToString().Trim();
                int MAXCount = Convert.ToInt32(sCompanyCode.Split('-')[1]) + 1;
                sResult = "SPI-" + MAXCount.ToString().Trim().PadLeft(4, '0'); ;
            }
            else
            {
                sResult = "SPI-0001";
            }
            return sResult;
        }


        //NPS_TEAM_PersonOpt
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

        public string ImportExcel(string KeyWord, string KeyValue, string FID)
        {
            DataTable data = ReslutData(KeyWord, KeyValue);
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();

            List<string> CodeList = new List<string>();
            string sSQL = "select PersonName from NPS_TEAM_PersonOpt_List ";
            DataTable CwData = XCode.DataAccessLayer.DAL.QuerySQL(sSQL);
            for (int i = 0; i < CwData.Rows.Count; i++)
            {
                CodeList.Add(CwData.Rows[i]["PersonName"].ToString());
            }

            foreach (DataRow item in data.Rows)
            {
                if (CodeList.Contains(item["姓名"].ToString()))
                {
                    continue;
                }
                Power.Business.IBaseBusiness LbItem = Power.Business.BusinessFactory.CreateBusiness("NPS_TEAM_PersonOpt_List");
                string LbZj = Guid.NewGuid().ToString();
                LbItem.SetItem("ID", LbZj);
                LbItem.SetItem("PersonCode", GetPersonCode(FID));
                LbItem.SetItem("PersonName", item["姓名"] == null ? "" : item["姓名"].ToString());
                if (item["性别"].ToString().Equals("男"))
                {
                    LbItem.SetItem("Sex", "1");
                }
                else
                {
                    LbItem.SetItem("PersonName", "0");
                }
                LbItem.SetItem("IdNumber", item["身份证号"] == null ? "" : item["身份证号"].ToString());
                LbItem.SetItem("BurthDate", item["出生年月"] == null ? "" : item["出生年月"].ToString());
                LbItem.SetItem("WorkDate", item["工作时间"] == null ? "" : item["工作时间"].ToString());
                LbItem.SetItem("WorkType", item["人员工种"] == null ? "" : item["人员工种"].ToString());
                LbItem.SetItem("CompanyJob", item["企业岗位"] == null ? "" : item["企业岗位"].ToString());
                LbItem.SetItem("PolitiLand", item["政治面貌"] == null ? "" : item["政治面貌"].ToString());
                LbItem.SetItem("SchoolRecord", item["学历"] == null ? "" : item["学历"].ToString());
                LbItem.SetItem("Profess", item["职称"] == null ? "" : item["职称"].ToString());
                LbItem.SetItem("IsPubed", item["是否已生成"] == null ? "" : item["是否已生成"].ToString());
                LbItem.SetItem("FID", FID);
                LbItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            return "ok";
        }

        public string GetPersonCode(string FID)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = @" select * from NPS_TEAM_PersonOpt where ID = '" + FID + "' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count <= 0)
            {
                return "未查询出数据！";
            }
            string sCompanyCode = dataSet.Tables[0].Rows[0]["CompanyCode"] == null ? "" : dataSet.Tables[0].Rows[0]["CompanyCode"].ToString().Trim();
            if (string.IsNullOrEmpty(sCompanyCode))
            {
                return "单位编号为空！";
            }

            string sResult = string.Empty;
            sSQL = @" select Max(PersonCode) as PersonCode from NPS_TEAM_PersonOpt_List b left join NPS_TEAM_PersonOpt a on a.ID = b.FID where a.CompanyCode = '" + sCompanyCode + "' ";
            dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["PersonCode"].ToString().Trim()))
            {
                string sPersonCode = dataSet.Tables[0].Rows[0]["PersonCode"].ToString().Trim();
                int MAXCount = Convert.ToInt32(sPersonCode.Split('-')[1]) + 1;
                sResult = sCompanyCode + "-" + MAXCount.ToString().Trim().PadLeft(3, '0');
            }
            else
            {
                sResult = sCompanyCode + "-001";
            }
            return sResult;
        }





        //NPS_PROJLIST_MainInfo
        public string ImportExcelMainInfo(string KeyWord, string KeyValue, string MainCode)
        {
            string flag = AllCanDelete(MainCode);
            if (flag == "no")
            {
                return "no";
            }

            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = @" select * from NPS_PROJLIST_MainInfo where MainCode = '" + MainCode + "' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            string sID = string.Empty;
            if (dataSet.Tables[0].Rows.Count >= 0)
            {
                sID = dataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
            }

            DataTable data = ReslutData(KeyWord, KeyValue);
            foreach (DataRow item in data.Rows)
            {
                Power.Business.IBaseBusiness LbItem = Power.Business.BusinessFactory.CreateBusiness("NPS_PROJLIST_MainInfo");
                string LbZj = Guid.NewGuid().ToString();
                LbItem.SetItem("ID", LbZj);
                LbItem.SetItem("MainCode", item["主项编号"] == null ? "" : item["主项编号"].ToString());
                LbItem.SetItem("MainName", item["主项名称"] == null ? "" : item["主项名称"].ToString());
                LbItem.SetItem("ParentID", sID);
                LbItem.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            return "ok";
        }

        //所有子层级是否可操作
        public string AllCanDelete(string MainCode)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = @" select * from NPS_PROJLIST_MainInfo where MainCode = '" + MainCode + "' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            string sParentID = string.Empty;
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                sParentID = dataSet.Tables[0].Rows[0]["ID"].ToString().Trim();
            }

            sSQL = @" select * from NPS_PROJLIST_MainInfo where ParentID = '" + sParentID + "' ";
            dataSet = dal.Session.Query(sSQL);
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                string ID = dataSet.Tables[0].Rows[i]["ID"].ToString().Trim();
                CanDelete(ID);
            }
            return "ok";
        }

        //单条记录能否操作
        //NPS_PROJLIST_MainInfo
        public string CanDelete(string ID)
        {
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();

            string sSQL = @" select * from NPS_PROJLIST_MainInfo where isnull(OwnProjGroup,'')<>'' and ID = '" + ID + "' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                return "no";
            }

            sSQL = @" select * from NPS_PROJLIST_MainInfo_List A left join NPS_PROJLIST_MainInfo B on B.ID=A.FID where B.ID = '" + ID + "' ";
            dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                return "no";
            }
            sSQL = @" select * from NPS_PROJLIST_MainInfo_Design A left join NPS_PROJLIST_MainInfo B on B.ID=A.FID where B.ID = '" + ID + "' ";
            dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                return "no";
            }
            sSQL = @" select * from NPS_PROJLIST_MainInfo_Est A left join NPS_PROJLIST_MainInfo B on B.ID=A.FID where B.ID = '" + ID + "' ";
            dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                return "no";
            }
            sSQL = @" select * from NPS_PROJLIST_MainInfo_Buy A left join NPS_PROJLIST_MainInfo B on B.ID=A.FID where B.ID = '" + ID + "' ";
            dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                return "no";
            }

            return "ok";
        }




        public string ImportExcel(string KeyWord, string KeyValue, string FID, string ZTable, string STable)
        {
            DataTable data = ReslutData(KeyWord, KeyValue);

            Dictionary<string, string> ZxID = new Dictionary<string, string>();

            Dictionary<string, string> ZyID = new Dictionary<string, string>();

            Dictionary<string, string> LbID = new Dictionary<string, string>();

            Power.Business.IBusinessList SbIns = Power.Business.BusinessFactory.CreateBusinessOperate(STable).FindAll("Sequ", -1);
            Power.Business.IBusinessList ZxIns = Power.Business.BusinessFactory.CreateBusinessOperate(ZTable).FindAll("Sequ", -1);

            int maxZY = GetMajorCode("ZY");
            int maxLB = GetMajorCode("LB");
            int maxSB = GetEquipCode();

            foreach (DataRow item in data.Rows)
            {
                if (ZxID.ContainsKey(item["主项名称"].ToString().Trim()))
                {
                    if (ZyID.ContainsKey(item["专业"].ToString().Trim()))
                    {
                        if (LbID.ContainsKey(item["类别"].ToString().Trim()))
                        {
                            string LbZj = LbID[item["类别"].ToString().Trim()];
                            //判断设备
                            Power.Business.IBaseBusiness Bus = Power.Business.BusinessFactory.CreateBusiness(STable);
                            maxSB++;
                            Bus.SetItem("EquipCode", "SB-" + maxSB.ToString().Trim().PadLeft(5, '0'));
                            Bus.SetItem("EquipName", item["设备名称"].ToString().Trim());
                            Bus.SetItem("EstiAmountOne", string.IsNullOrEmpty(item["概算工程量1"].ToString().Trim()) ? "0" : item["概算工程量1"].ToString().Trim());
                            Bus.SetItem("UnitOne", item["单位1"].ToString().Trim());
                            Bus.SetItem("EstiAmountTwo", string.IsNullOrEmpty(item["概算工程量2"].ToString().Trim()) ? "0" : item["概算工程量2"].ToString().Trim());
                            Bus.SetItem("UnitTwo", item["单位2"].ToString().Trim());
                            Bus.SetItem("FID", LbZj);
                            SbIns.Add(Bus);
                        }
                        else
                        {
                            //类别
                            Power.Business.IBaseBusiness LbItem = Power.Business.BusinessFactory.CreateBusiness(ZTable);
                            string LbZj = Guid.NewGuid().ToString();
                            maxLB++;
                            LbItem.SetItem("MajorCode", "LB-" + maxLB.ToString().Trim().PadLeft(5, '0'));
                            LbItem.SetItem("MajorName", item["类别"].ToString().Trim());
                            LbItem.SetItem("ID", LbZj);
                            LbItem.SetItem("ParentID", ZyID[item["专业"].ToString().Trim()]);
                            LbItem.SetItem("FID", FID);
                            LbItem.SetItem("Flag", "类别");
                            LbItem.SetItem("FromExcel", "是");
                            ZxIns.Add(LbItem);
                            LbID.Add(item["类别"].ToString().Trim(), LbZj);

                            //设备
                            Power.Business.IBaseBusiness Bus = Power.Business.BusinessFactory.CreateBusiness(STable);
                            maxSB++;
                            Bus.SetItem("EquipCode", "SB-" + maxSB.ToString().Trim().PadLeft(5, '0'));
                            Bus.SetItem("EquipName", item["设备名称"].ToString().Trim());
                            Bus.SetItem("EstiAmountOne", string.IsNullOrEmpty(item["概算工程量1"].ToString().Trim()) ? "0" : item["概算工程量1"].ToString().Trim());
                            Bus.SetItem("UnitOne", item["单位1"].ToString().Trim());
                            Bus.SetItem("EstiAmountTwo", string.IsNullOrEmpty(item["概算工程量2"].ToString().Trim()) ? "0" : item["概算工程量2"].ToString().Trim());
                            Bus.SetItem("UnitTwo", item["单位2"].ToString().Trim());
                            Bus.SetItem("FID", LbZj);
                            SbIns.Add(Bus);
                        }
                    }
                    else
                    {
                        //主项
                        Power.Business.IBaseBusiness ZyItem = Power.Business.BusinessFactory.CreateBusiness(ZTable);
                        string ZyZj = Guid.NewGuid().ToString();

                        maxZY++;
                        ZyItem.SetItem("MajorCode", "ZY-" + maxZY.ToString().Trim().PadLeft(5, '0'));
                        ZyItem.SetItem("MajorName", item["专业"].ToString().Trim());
                        ZyItem.SetItem("ID", ZyZj);
                        ZyItem.SetItem("ParentID", ZxID[item["主项名称"].ToString().Trim()]);
                        ZyItem.SetItem("FID", FID);
                        ZyItem.SetItem("Flag", "专业");
                        ZyItem.SetItem("FromExcel", "是");
                        ZxIns.Add(ZyItem);
                        ZyID.Add(item["专业"].ToString().Trim(), ZyZj);

                        //类别
                        Power.Business.IBaseBusiness LbItem = Power.Business.BusinessFactory.CreateBusiness(ZTable);
                        string LbZj = Guid.NewGuid().ToString();

                        maxLB++;
                        LbItem.SetItem("MajorCode", "LB-" + maxLB.ToString().Trim().PadLeft(5, '0'));
                        LbItem.SetItem("MajorName", item["类别"].ToString().Trim());
                        LbItem.SetItem("ID", LbZj);
                        LbItem.SetItem("ParentID", ZyZj);
                        LbItem.SetItem("FID", FID);
                        LbItem.SetItem("Flag", "类别");
                        LbItem.SetItem("FromExcel", "是");
                        ZxIns.Add(LbItem);
                        LbID.Add(item["类别"].ToString().Trim(), LbZj);

                        //设备
                        Power.Business.IBaseBusiness Bus = Power.Business.BusinessFactory.CreateBusiness(STable);
                        maxSB++;
                        Bus.SetItem("EquipCode", "SB-" + maxSB.ToString().Trim().PadLeft(5, '0'));
                        Bus.SetItem("EquipName", item["设备名称"].ToString().Trim());
                        Bus.SetItem("EstiAmountOne", string.IsNullOrEmpty(item["概算工程量1"].ToString().Trim()) ? "0" : item["概算工程量1"].ToString().Trim());
                        Bus.SetItem("UnitOne", item["单位1"].ToString().Trim());
                        Bus.SetItem("EstiAmountTwo", string.IsNullOrEmpty(item["概算工程量2"].ToString().Trim()) ? "0" : item["概算工程量2"].ToString().Trim());
                        Bus.SetItem("UnitTwo", item["单位2"].ToString().Trim());
                        Bus.SetItem("FID", LbZj);
                        SbIns.Add(Bus);
                    }
                }
                else
                {
                    //主项
                    Power.Business.IBaseBusiness ZxItem = Power.Business.BusinessFactory.CreateBusiness(ZTable);
                    string ZxZj = Guid.NewGuid().ToString();

                    ZxItem.SetItem("MajorCode", item["主项号"].ToString().Trim());
                    ZxItem.SetItem("MajorName", item["主项名称"].ToString().Trim());
                    ZxItem.SetItem("Flag", "主项");
                    ZxItem.SetItem("FromExcel", "是");
                    ZxItem.SetItem("ID", ZxZj);
                    ZxItem.SetItem("FID", FID);

                    ZxIns.Add(ZxItem);
                    ZxID.Add(item["主项名称"].ToString().Trim(), ZxZj);

                    //主项
                    Power.Business.IBaseBusiness ZyItem = Power.Business.BusinessFactory.CreateBusiness(ZTable);
                    string ZyZj = Guid.NewGuid().ToString();

                    maxZY++;
                    ZyItem.SetItem("MajorCode", "ZY-" + maxZY.ToString().Trim().PadLeft(5, '0'));
                    ZyItem.SetItem("MajorName", item["专业"].ToString().Trim());
                    ZyItem.SetItem("ID", ZyZj);
                    ZyItem.SetItem("ParentID", ZxZj);
                    ZyItem.SetItem("FID", FID);
                    ZxItem.SetItem("Flag", "专业");
                    ZxItem.SetItem("FromExcel", "是");
                    ZxIns.Add(ZyItem);
                    ZyID.Add(item["专业"].ToString().Trim(), ZyZj);

                    //类别
                    Power.Business.IBaseBusiness LbItem = Power.Business.BusinessFactory.CreateBusiness(ZTable);
                    string LbZj = Guid.NewGuid().ToString();

                    maxLB++;
                    LbItem.SetItem("MajorCode", "LB-" + maxLB.ToString().Trim().PadLeft(5, '0'));
                    LbItem.SetItem("MajorName", item["类别"].ToString().Trim());
                    LbItem.SetItem("ID", LbZj);
                    LbItem.SetItem("ParentID", ZyZj);
                    LbItem.SetItem("FID", FID);
                    ZxItem.SetItem("Flag", "类别");
                    ZxItem.SetItem("FromExcel", "是");
                    ZxIns.Add(LbItem);
                    LbID.Add(item["类别"].ToString().Trim(), LbZj);

                    //设备
                    Power.Business.IBaseBusiness Bus = Power.Business.BusinessFactory.CreateBusiness(STable);
                    maxSB++;
                    Bus.SetItem("EquipCode", "SB-" + maxSB.ToString().Trim().PadLeft(5, '0'));
                    Bus.SetItem("EquipName", item["设备名称"].ToString().Trim());
                    Bus.SetItem("EstiAmountOne", string.IsNullOrEmpty(item["概算工程量1"].ToString().Trim()) ? "0" : item["概算工程量1"].ToString().Trim());
                    Bus.SetItem("UnitOne", item["单位1"].ToString().Trim());
                    Bus.SetItem("EstiAmountTwo", string.IsNullOrEmpty(item["概算工程量2"].ToString().Trim()) ? "0" : item["概算工程量2"].ToString().Trim());
                    Bus.SetItem("UnitTwo", item["单位2"].ToString().Trim());
                    Bus.SetItem("FID", LbZj);
                    SbIns.Add(Bus);
                }
            }

            foreach (Power.Business.IBaseBusiness item in ZxIns)
            {
                item.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            foreach (Power.Business.IBaseBusiness item in SbIns)
            {
                item.Save(System.ComponentModel.DataObjectMethodType.Insert);
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">ZY 专业,LB 类别/param>
        /// <returns></returns>
        public int GetMajorCode(string Type)
        {
            int MAXCount = 0;
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sResult = string.Empty;
            string sSQL = @" select Max(MajorCode) as MajorCode from  NPS_INV_BaseDesEstimate_List where MajorCode like '" + Type + "-%' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["MajorCode"].ToString().Trim()))
            {
                string sPersonCode = dataSet.Tables[0].Rows[0]["MajorCode"].ToString().Trim();
                MAXCount = Convert.ToInt32(sPersonCode.Split('-')[1]);
            }
            else
            {
                MAXCount = 0;
            }
            return MAXCount;
        }

        public int GetEquipCode()
        {
            int MAXCount = 0;
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string sSQL = @" select Max(EquipCode) as EquipCode from  NPS_INV_BaseDesEstimate_ListS where EquipCode like 'SB-%' ";
            DataSet dataSet = dal.Session.Query(sSQL);
            if (dataSet.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["EquipCode"].ToString().Trim()))
            {
                string sPersonCode = dataSet.Tables[0].Rows[0]["EquipCode"].ToString().Trim();
                MAXCount = Convert.ToInt32(sPersonCode.Split('-')[1]);
            }
            else
            {
                MAXCount = 0;
            }
            return MAXCount;
        }

        #endregion
    }

}