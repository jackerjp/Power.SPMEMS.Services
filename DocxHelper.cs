using System.Linq;
using System.Reflection;
using Aspose.Words;
using Spire.Doc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace Power.SPMEMS.Services
{
    class DocxHelper
    {
        /// <summary>
        /// 搜索docx文件中的信息
        /// </summary>
        /// <param name="searchvalue"></param>
        /// <returns></returns>
        public string Search(string searchvalue)
        {
            Power.IBaseCore.ISession session = null;
            if (Meta.SessionUtil != null) session = Meta.SessionUtil.getSession();
            ArrayList arrayList = new ArrayList();
            Spire.Doc.Document doc = new Spire.Doc.Document();//新建Word文档
            XCode.DataAccessLayer.DAL dal = XCode.DataAccessLayer.DAL.Create();
            string NSQL = "select Type,Code,Title,UpdDate,DocName from NPS_MAD_Cover where 1=1 and EpsProjId='" + session.EpsProjId + "'";
            DataTable OldDt = XCode.DataAccessLayer.DAL.QuerySQL(NSQL);
            for (int i = 0; i < OldDt.Rows.Count; i++)
            {
                string code = OldDt.Rows[i]["code"].ToString();
                string filepath = "F:\\PM\\PowerPMS\\PowerPMS\\Combine\\" + code + ".docx";//Word文档地址

                if (System.IO.File.Exists(filepath))
                {
                    doc.LoadFromFile(filepath, FileFormat.Docx);//读取Word文档
                    Spire.Doc.Document doc01 = new Spire.Doc.Document(filepath, FileFormat.Docx);//也可以初始化时读取Word文档
                    string DocValue = doc01.GetText();
                    if (DocValue.Contains(searchvalue))
                    {
                        JObject obj = new JObject();
                        arrayList.Add(obj);
                    }
                }
            }

            JsonSerializerSettings s_setting = new JsonSerializerSettings();
            return JsonConvert.SerializeObject(arrayList, s_setting);
        }

        /// <summary>
        /// 将两份doc文件合并成一个新的doc格式文件示例
        /// </summary>
        /// <param name="CoverCode"></param>
        /// <param name="CoverId"></param>
        /// <param name="IdGroup"></param>
        public void NewCombine(string CoverCode, string CoverId, string IdGroup)
        {
            List<string> files = new List<string>();
            string tempDoc;
            //查询有哪些附件
            Power.Business.IBusinessOperate DOC = Power.Business.BusinessFactory.CreateBusinessOperate("DocFile");
            string[] IdArr = IdGroup.Split("/");
            if (IdArr.Length > 0)
            {
                foreach (string Id in IdArr)
                {
                    Power.Business.IBusinessList DOCBO = DOC.FindAll("Id='" + Id + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
                    if (DOCBO.Count > 0)
                    {
                        if (DOCBO[0]["FileExt"].ToString().IndexOf("doc") != -1)
                        {
                            files.Add("F:\\FTP" + DOCBO[0]["ServerUrl"].ToString().Replace("/", "//"));
                            DOCBO[0].SetItem("FolderId", "9375EAFA-23F9-13C3-7FF6-08EEAC81A07B");
                            DOCBO[0].SetItem("BOKeyWord", "NPS_MAD_Doc");
                            DOCBO[0].Save(System.ComponentModel.DataObjectMethodType.Update);
                        }
                    }
                }
            }

            Power.Business.IBusinessList doc = DOC.FindAll("FolderId='" + CoverId + "'", "", "", 0, 0, Power.Business.SearchFlag.IgnoreRight);
            if (doc.Count > 0)
            {
                if (doc[0]["FileExt"].ToString().IndexOf("doc") != -1)
                {
                    tempDoc = "F:\\FTP" + doc[0]["ServerUrl"].ToString().Replace("/", "//");

                    Aspose.Words.Document dos = new Aspose.Words.Document(tempDoc);
                    Aspose.Words.Document tmp;
                    foreach (string str in files)
                    {
                        tmp = new Aspose.Words.Document(str);
                        dos.AppendDocument(tmp, ImportFormatMode.UseDestinationStyles);
                    }
                    dos.Save("F:\\PM\\PowerPMS\\PowerPMS//Combine//" + CoverCode + ".docx");
                }
            }
        }
    }
}
