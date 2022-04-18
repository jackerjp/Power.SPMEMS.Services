using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power.SPMEMS.Services
{
    class TQBYJYSCode
    {
         public DataTable PettyCash(string Year,string swhere)
        {
            string SQL = @"select * from (
                                (select N.BorrowYear,M.BorrowerId,M.Borrower,sum(isnull(M.Amount,0)) BorrowAmount,sum(isnull(M.PaymentAmount,0)) ReturnAmount,sum((isnull(M.Amount,0) - isnull(M.PaymentAmount,0))) BalanceAmount from NPS_HFS_PettyCash_list M
                                left join NPS_HFS_PettyCash N on N.ID = M.FID group by M.BorrowerId,N.BorrowYear,M.Borrower)
                                union all (select '-'BorrowYear,'00000000-0000-0000-0000-000000000000' BorrowerId,'总计' Borrower,sum(isnull(M.Amount,0)) BorrowAmount,sum(isnull(M.PaymentAmount,0)) ReturnAmount,sum((isnull(M.Amount,0) - isnull(M.PaymentAmount,0))) BalanceAmount from NPS_HFS_PettyCash_list M
                                left join NPS_HFS_PettyCash N on N.ID= M.FID)
                            ) LL
                            where BorrowYear='" + Year + "' and " + swhere;
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }
            
            
        public DataTable PettyCashList(string Year,string swhere)
        {
            string SQL = @"select * from (
                                select N.BorrowYear,M.BorrowerId,M.Borrower,M.CompanyName,M.Amount,M.BorrowingCauses,M.BorrowingDate,M.PaymentAmount,M.PaymentDate,(isnull(M.Amount,0) - isnull(M.PaymentAmount,0)) Balance
                                from NPS_HFS_PettyCash_list M left join NPS_HFS_PettyCash N on N.ID= M.FID
                            ) PP
                            where BorrowYear='" + Year + "' and " + swhere;
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }


        public DataTable PettyCashList1(string Year, string BorrowerId)
        {
            string SQL = @"select * from (
                                (select N.BorrowYear,M.BorrowerId,M.Borrower,M.CompanyName,M.Amount,M.BorrowingCauses,M.BorrowingDate,M.PaymentAmount,M.PaymentDate,(isnull(M.Amount,0) - isnull(M.PaymentAmount,0)) Balance
                                from NPS_HFS_PettyCash_list M left join NPS_HFS_PettyCash N on N.ID= M.FID)
                                union all
                                (select '总计'BorrowYear,'00000000-0000-0000-0000-000000000000'BorrowerId,'-' Borrower,'-'CompanyName,sum(isnull(M.Amount,0)) Amount,'-' BorrowingCauses,'-' BorrowingDate,sum(isnull(M.PaymentAmount,0)) PaymentAmount,'-' PaymentDate,sum((isnull(M.Amount,0) - isnull(M.PaymentAmount,0))) Balance
                                from NPS_HFS_PettyCash_list M left join NPS_HFS_PettyCash N on N.ID= M.FID group by M.Borrower)
                            ) HH
                            where BorrowYear='" + Year + "' and BorrowerId='" + BorrowerId + "' order by BorrowingDate";
            DataTable Dt = XCode.DataAccessLayer.DAL.QuerySQL(SQL);
            return Dt;//返回的查询结果
        }
    }
}
