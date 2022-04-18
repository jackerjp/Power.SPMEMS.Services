using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power.SPMEMS.Services
{
    class TQYScode
    {
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
                                left join (select sum(isnull(PrivRefund,0)) as PrivRefund,Year(RegDate) as year,ConCode from NPS_CON_CloseApplicatio where (Status='35' or Status='50')  group by Year(RegDate),ConCode) B on A.ConEpsCode=B.ConCode
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
                                select '累计收入合同额' as Name,sum(isnull(ConMoney,0)) as Money,'―'  percentage,Year(RegDate) as year from NPS_CON_RevenueContract where (Status=50 or Status=35) group by Year(RegDate)
                                union all
                                select '累计已收款' as Name,sum(isnull(B.ActRecMoney,0)) as Money,
                                case when sum(isnull(A.ConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.ActRecMoney,0)),0)/sum(isnull(A.ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (select sum(isnull(ConMoney,0)) as ConMoney,Year(RegDate) as year from NPS_CON_RevenueContract where (Status=50 or Status=35) group by Year(RegDate) ) A
                                left join (select sum(isnull(ActRecMoney,0)) as ActRecMoney,Year(RegDate) as year from NPS_CON_ContractCollection where (Status=50 or Status=35) group by Year(RegDate)) B on A.year=B.year
                                group by A.year
                                union all
                                select '累计应开票' as Name,sum(isnull(ConMoney,0)) as Money,'―'  percentage,A.year from
                                    (select sum(isnull(N.ConMoney,0)) as ConMoney,YEAR(M.RegDate) as year from NPS_CON_CloseApplicatio M
                                        left join NPS_CON_RevenueContract N on M.ConID=N.ID
                                     where M.NotInvoice='0' and  (M.Status=50 or M.Status=35)
                                     group by Year(M.RegDate)) A
                                group by A.year
                                union all
                                select '累计已开票' as Name,sum(isnull(B.InvMoney,0)) as Money,
                                case when sum(isnull(A.ConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.InvMoney,0)),0)/sum(isnull(A.ConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year from
                                    (select sum(isnull(N.ConMoney,0)) as ConMoney,YEAR(M.RegDate) as year from NPS_CON_CloseApplicatio M
                                        left join NPS_CON_RevenueContract N on M.ConID=N.ID
                                     where M.NotInvoice='0' and (M.Status=50 or M.Status=35)
                                     group by Year(M.RegDate)) A
                                left join (select sum(isnull(InvMoney,0)) as InvMoney,Year(RegDate) as year from NPS_CON_ContractInvoiceApplication where  (Status=50 or Status=35) group by Year(RegDate)) B on A.year=B.year
                                group by A.year
                                union all
                                select '累计应付信息费/私退款' as Name,sum(isnull(InfoFee,0)) as Money,'―' percentage,A.year from
                                (select sum(isnull(InfoFee + PrivRefund,0)) as InfoFee,YEAR(RegDate) as year from NPS_CON_CloseApplicatio where NotInvoice='0' and  (Status=50 or Status=35) group by Year(RegDate)) A
                                group by A.year
                                union all
                                select '累计已付信息费/私退款' as Name,sum(isnull(B.InfoFee,0)) as Money,
                                  case when sum(isnull(A.InfoFee,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.InfoFee,0)),0)/sum(isnull(A.InfoFee,0)),2)*100 as nvarchar) + '%' end percentage,A.year from
                                    (select sum(isnull(InfoFee+PrivRefund,0)) as InfoFee,YEAR(RegDate) as year from NPS_CON_CloseApplicatio where NotInvoice='0' and (Status=50 or Status=35)  group by Year(RegDate)) A
                                left join (select sum(isnull(CurrentInfoFee+CurrentPrivRefund,0)) as InfoFee,Year(RegDate) as year from NPS_CON_RevenueContract_OtherExpend where NotInvoice='0' and (Status=50 or Status=35) group by Year(RegDate)) B on A.year=B.year
                                 group by A.year
                                union all
                                select '累计分包合同额' as Name,sum(isnull(SubConMoney,0)) as Money,'―'  percentage,Year(RegDate) as year from NPS_CON_Subcontract group by Year(RegDate)
                                union all
                                select '累计分包支付额' as Name,sum(isnull(B.PayMoney,0)) as Money,
                                case when sum(isnull(A.SubConMoney,0))=0 then '0' else cast(ROUND(isnull(sum(isnull(B.PayMoney,0)),0)/sum(isnull(A.SubConMoney,0)),2)*100 as nvarchar) + '%' end percentage,A.year
                                from (select sum(isnull(SubConMoney,0)) as SubConMoney,Year(RegDate) as year from NPS_CON_Subcontract group by Year(RegDate) ) A
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
                                (select Year(RegDate) as year,ConType,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract group by Year(RegDate),ConType) A
                                --每年的收入合同总额
                                left join (select Year(RegDate) as year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract group by Year(RegDate)) B on A.year=B.year
                                --每年的累计应付信息费、私退款总额
                                left join (select sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(RegDate) as year from NPS_CON_CloseApplicatio group by Year(RegDate)) D on A.year=D.year
                                --累计每年累计应付信息费、私退款
                                left join (
                                    select sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(M.RegDate) as year,N.ConType from NPS_CON_CloseApplicatio M
                                    left join NPS_CON_RevenueContract N on M.ConID=N.ID where M.NotInvoice='0' group by Year(M.RegDate),N.ConType
                                ) C on A.year=C.year and A.ConType=C.ConType
                                union all
                                select '总计' ConType,isnull(A.ConMoney,0) as ConMoney,
                                        case when isnull(B.ConMoney,0)=0 then '-' else '-' end percentage,
                                        C.InfoFee,C.PrivRefund,
                                        A.ConMoney-C.InfoFee-C.PrivRefund as ActSR,
                                        case when isnull(B.ConMoney-D.InfoFee-D.PrivRefund,0)=0 then '-' else '-' end ActSRpercentage,
                                        A.year
                                from
                                (select Year(RegDate) as year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract group by Year(RegDate)) A
                                --每年的收入合同总额
                                left join (select Year(RegDate) as year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract group by Year(RegDate)) B on A.year=B.year
                                --每年的累计应付信息费、私退款总额
                                left join (select sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(RegDate) as year from NPS_CON_CloseApplicatio group by Year(RegDate)) D on A.year=D.year
                                --累计每年累计应付信息费、私退款
                                left join (
                                    select sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(M.RegDate) as year from NPS_CON_CloseApplicatio M
                                    left join NPS_CON_RevenueContract N on M.ConID=N.ID where M.NotInvoice='0' group by Year(M.RegDate)
                                ) C on A.year=C.year
                            ) LL
                            where year='" + Year + "'";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
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











        
        public DataTable ContractML(string Year)
        {
            string SQL = @"select *,concat(round((ML/ReML) * 100,2),'' , '%') as MLpercentage from (
                                select case when A.ConType='HJ' then '环境'
                                                when A.ConType='QT' then '其他'
                                                when A.ConType='ZW' then '职卫非煤'
                                                when A.ConType='ZM' then '职卫煤矿' end ConType,A.year,A.ConMoney,B.InfoFee,B.PrivRefund,isnull(A.ConMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0) as ActSR,C.Tax,isnull(D.Money,0) as PSF,
                                isnull(E.SubConMoney,0) SubConMoney,F.DeductionAmount,isnull(A.ConMoney,0)-isnull(B.InfoFee,0)-isnull(B.PrivRefund,0)-isnull(C.Tax,0)-isnull(D.Money,0)-isnull(E.SubConMoney,0)-isnull(F.DeductionAmount,0) as ML from
                                (select ConType,Year(RegDate) year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract group by ConType,Year(RegDate)) A
                                left join
                                (
                                    select sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(M.RegDate) as year,N.ConType from NPS_CON_CloseApplicatio M
                                        left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate),N.ConType
                                ) B on A.year=B.year and A.ConType=B.ConType
                                left join
                                (
                                    select sum(isnull(Tax,0)) as Tax,Year(M.RegDate) as year,N.ConType from NPS_CON_ContractInvoiceApplication M
                                        left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate),N.ConType  
                                ) C on A.year=C.year and A.ConType=C.ConType
                                left join
                                (
                                    select Year(P.RegDate) as year,P.ConType,sum(isnull(Money,0)) as Money from NPS_HFS_Other_List K
                                    left join NPS_HFS_Other L on K.FID=L.ID
                                    left join NPS_CON_RevenueContract P on L.ConID=P.ID where L.ConID is not null and K.FeeProject='专家评审费' group by Year(P.RegDate),P.ConType  
                                ) D on A.year=D.year and A.ConType=D.ConType
                                left join
                                (
                                    select sum(isnull(SubConMoney,0)) as SubConMoney,Year(M.RegDate) as year,N.ConType from NPS_CON_Subcontract M
                                        left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate),N.ConType  
                                ) E on A.year=E.year and A.ConType=E.ConType
                                left join
                                (
                                    select sum(isnull(DeductionAmount,0)) as DeductionAmount,Year(M.RegDate) as year,N.ConType from NPS_CON_RevenueContract_OtherExpend M
                                        left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate),N.ConType  
                                ) F on A.year=F.year and A.ConType=F.ConType
                            ) HH
                            left join(
                                (select 
                                    A.year Reyear,A.ConMoney-B.InfoFee-B.PrivRefund-C.Tax-isnull(D.Money,0)-isnull(E.SubConMoney,0)-isnull(F.DeductionAmount,0) as ReML from 
                                (select  Year(RegDate) year,sum(isnull(ConMoney,0)) as ConMoney from NPS_CON_RevenueContract group by Year(RegDate)) A
                                left join(select sum(isnull(InfoFee,0)) as InfoFee,sum(isnull(PrivRefund,0)) as PrivRefund,Year(M.RegDate) as year from NPS_CON_CloseApplicatio M
                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate)) B on A.year=B.year 
                                left join(select sum(isnull(Tax,0)) as Tax,Year(M.RegDate) as year from NPS_CON_ContractInvoiceApplication M
                                    left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate) ) C on A.year=C.year 
                                left join(select Year(P.RegDate) as year,sum(isnull(Money,0)) as Money from NPS_HFS_Other_List K
                                    left join NPS_HFS_Other L on K.FID=L.ID
                                left join NPS_CON_RevenueContract P on L.ConID=P.ID where L.ConID is not null and K.FeeProject='专家评审费' group by Year(P.RegDate)) D on A.year=D.year  
                                left join(select sum(isnull(SubConMoney,0)) as SubConMoney,Year(M.RegDate) as year from NPS_CON_Subcontract M
                                    left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate)) E on A.year=E.year
                                left join(select sum(isnull(DeductionAmount,0)) as DeductionAmount,Year(M.RegDate) as year from NPS_CON_RevenueContract_OtherExpend M
                                left join NPS_CON_RevenueContract N on M.ConID=N.ID group by Year(M.RegDate)) F on A.year=F.year) 
                                ) X on X.Reyear = HH.year
                            where year='" + Year + "'";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }
    }
}
