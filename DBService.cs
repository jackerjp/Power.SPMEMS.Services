using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Power.SPMEMS.Services
{
    public class DBService
    {
        public static readonly DbSession Context = new DbSession("YY");
        public static readonly DbSession EMS = new DbSession("EMS");
    }
}