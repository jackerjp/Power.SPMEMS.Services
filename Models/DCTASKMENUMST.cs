﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://ITdos.com/Dos/ORM/Index.html
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using Dos.ORM;

namespace Power.SPMEMS.Services.Models
{
	/// <summary>
	/// 实体类DCTASKMENUMST。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("DCTASKMENUMST")]
	[Serializable]
	[DataContract]
	public partial class DCTASKMENUMST : Entity
	{
		#region Model
		private int _TM_NO;
		private int? _ORG_NO;
		private string _TM_ID;
		private string _TM_NAM;
		private string _TMJSXZ;
		private DateTime? _TM_DTM;
		private DateTime? _TM_BEGIN;
		private DateTime? _TM_END;
		private string _TM_GQYQ;
		private int? _CST_NO;
		private string _ZBGCS;
		private string _TM_DSC;
		private string _TM_SHT;
		private int? _PJBAR_NO;
		private string _PJBARPAY_TYP;
		private string _CBKXX;
		private string _KXX;
		private string _CBSJ;
		private string _SGT;
		private string _JGT;
		private string _SGTYS;
		private string _EXT1;
		private string _EXT2;
		private string _EXT3;
		private string _FSTUSR_ID;
		private DateTime? _FSTUSR_DTM;
		private string _LSTUSR_ID;
		private DateTime? _LSTUSR_DTM;
		private string _TM_STA;
		private int? _TM_SRC;
		private string _TPGCLB;
		private string _YXHF;
		private string _SGJD;
		private string _CGJD;
		private string _TSJD;
		private string _BFJD;
		private string _XMJD;
		private int? _PROJECT_NO;
		private int? _JDLB;
		private int? _AREA_NO;
		private string _AREAEXT_SHT;
		private int? _PRJTYP_NO;
		private int? _SRVKIND_NO;
		private string _TM_ISFIRST;
		private string _TMD_STA;
		private string _TM_NOT;
		private string _BARTZ_ID;
		private string _PRJJD_ID;
		private string _BAR_USR;
		private string _SJ_DSC;
		private string _BAR_JD;
		private string _PRJ_ID;
		private string _PLAPRJ_USR;
		private string _ENPROJECT_NAM;
		private DateTime? _PLABEG_DTM;
		private DateTime? _PLARK_DTM;
		private string _PROJECT_GM;
		private string _PROJECT_NAM;
		private string _PRJMGR_ID;
		private string _SJMGRZ_ID;
		private string _SJMGRF_ID;
		private string _PRJFZR_ID;
		private string _SJZL_ID;
		private DateTime? _TMXD_DTM;
		private string _PROJ_LEVEL;
		private string _SKL_NOS;
		private string _USRBG_DSC;
		private string _SKL_NAMS;
		private string _WDSUR_ID;
		private int? _PRJPLAN_NO;
		private DateTime? _SJWG_DTM;
		private string _TMPROSTA;
		private string _QYBH_ID;

		/// <summary>
		/// 
		/// </summary>
		[Field("TM_NO")]
		[DataMember]
		public int TM_NO
		{
			get { return _TM_NO; }
			set
			{
				this.OnPropertyValueChange("TM_NO");
				this._TM_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ORG_NO")]
		[DataMember]
		public int? ORG_NO
		{
			get { return _ORG_NO; }
			set
			{
				this.OnPropertyValueChange("ORG_NO");
				this._ORG_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_ID")]
		[DataMember]
		public string TM_ID
		{
			get { return _TM_ID; }
			set
			{
				this.OnPropertyValueChange("TM_ID");
				this._TM_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_NAM")]
		[DataMember]
		public string TM_NAM
		{
			get { return _TM_NAM; }
			set
			{
				this.OnPropertyValueChange("TM_NAM");
				this._TM_NAM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TMJSXZ")]
		[DataMember]
		public string TMJSXZ
		{
			get { return _TMJSXZ; }
			set
			{
				this.OnPropertyValueChange("TMJSXZ");
				this._TMJSXZ = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_DTM")]
		[DataMember]
		public DateTime? TM_DTM
		{
			get { return _TM_DTM; }
			set
			{
				this.OnPropertyValueChange("TM_DTM");
				this._TM_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_BEGIN")]
		[DataMember]
		public DateTime? TM_BEGIN
		{
			get { return _TM_BEGIN; }
			set
			{
				this.OnPropertyValueChange("TM_BEGIN");
				this._TM_BEGIN = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_END")]
		[DataMember]
		public DateTime? TM_END
		{
			get { return _TM_END; }
			set
			{
				this.OnPropertyValueChange("TM_END");
				this._TM_END = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_GQYQ")]
		[DataMember]
		public string TM_GQYQ
		{
			get { return _TM_GQYQ; }
			set
			{
				this.OnPropertyValueChange("TM_GQYQ");
				this._TM_GQYQ = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("CST_NO")]
		[DataMember]
		public int? CST_NO
		{
			get { return _CST_NO; }
			set
			{
				this.OnPropertyValueChange("CST_NO");
				this._CST_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ZBGCS")]
		[DataMember]
		public string ZBGCS
		{
			get { return _ZBGCS; }
			set
			{
				this.OnPropertyValueChange("ZBGCS");
				this._ZBGCS = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_DSC")]
		[DataMember]
		public string TM_DSC
		{
			get { return _TM_DSC; }
			set
			{
				this.OnPropertyValueChange("TM_DSC");
				this._TM_DSC = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_SHT")]
		[DataMember]
		public string TM_SHT
		{
			get { return _TM_SHT; }
			set
			{
				this.OnPropertyValueChange("TM_SHT");
				this._TM_SHT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PJBAR_NO")]
		[DataMember]
		public int? PJBAR_NO
		{
			get { return _PJBAR_NO; }
			set
			{
				this.OnPropertyValueChange("PJBAR_NO");
				this._PJBAR_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PJBARPAY_TYP")]
		[DataMember]
		public string PJBARPAY_TYP
		{
			get { return _PJBARPAY_TYP; }
			set
			{
				this.OnPropertyValueChange("PJBARPAY_TYP");
				this._PJBARPAY_TYP = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("CBKXX")]
		[DataMember]
		public string CBKXX
		{
			get { return _CBKXX; }
			set
			{
				this.OnPropertyValueChange("CBKXX");
				this._CBKXX = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("KXX")]
		[DataMember]
		public string KXX
		{
			get { return _KXX; }
			set
			{
				this.OnPropertyValueChange("KXX");
				this._KXX = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("CBSJ")]
		[DataMember]
		public string CBSJ
		{
			get { return _CBSJ; }
			set
			{
				this.OnPropertyValueChange("CBSJ");
				this._CBSJ = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SGT")]
		[DataMember]
		public string SGT
		{
			get { return _SGT; }
			set
			{
				this.OnPropertyValueChange("SGT");
				this._SGT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("JGT")]
		[DataMember]
		public string JGT
		{
			get { return _JGT; }
			set
			{
				this.OnPropertyValueChange("JGT");
				this._JGT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SGTYS")]
		[DataMember]
		public string SGTYS
		{
			get { return _SGTYS; }
			set
			{
				this.OnPropertyValueChange("SGTYS");
				this._SGTYS = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("EXT1")]
		[DataMember]
		public string EXT1
		{
			get { return _EXT1; }
			set
			{
				this.OnPropertyValueChange("EXT1");
				this._EXT1 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("EXT2")]
		[DataMember]
		public string EXT2
		{
			get { return _EXT2; }
			set
			{
				this.OnPropertyValueChange("EXT2");
				this._EXT2 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("EXT3")]
		[DataMember]
		public string EXT3
		{
			get { return _EXT3; }
			set
			{
				this.OnPropertyValueChange("EXT3");
				this._EXT3 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("FSTUSR_ID")]
		[DataMember]
		public string FSTUSR_ID
		{
			get { return _FSTUSR_ID; }
			set
			{
				this.OnPropertyValueChange("FSTUSR_ID");
				this._FSTUSR_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("FSTUSR_DTM")]
		[DataMember]
		public DateTime? FSTUSR_DTM
		{
			get { return _FSTUSR_DTM; }
			set
			{
				this.OnPropertyValueChange("FSTUSR_DTM");
				this._FSTUSR_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("LSTUSR_ID")]
		[DataMember]
		public string LSTUSR_ID
		{
			get { return _LSTUSR_ID; }
			set
			{
				this.OnPropertyValueChange("LSTUSR_ID");
				this._LSTUSR_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("LSTUSR_DTM")]
		[DataMember]
		public DateTime? LSTUSR_DTM
		{
			get { return _LSTUSR_DTM; }
			set
			{
				this.OnPropertyValueChange("LSTUSR_DTM");
				this._LSTUSR_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_STA")]
		[DataMember]
		public string TM_STA
		{
			get { return _TM_STA; }
			set
			{
				this.OnPropertyValueChange("TM_STA");
				this._TM_STA = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_SRC")]
		[DataMember]
		public int? TM_SRC
		{
			get { return _TM_SRC; }
			set
			{
				this.OnPropertyValueChange("TM_SRC");
				this._TM_SRC = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TPGCLB")]
		[DataMember]
		public string TPGCLB
		{
			get { return _TPGCLB; }
			set
			{
				this.OnPropertyValueChange("TPGCLB");
				this._TPGCLB = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("YXHF")]
		[DataMember]
		public string YXHF
		{
			get { return _YXHF; }
			set
			{
				this.OnPropertyValueChange("YXHF");
				this._YXHF = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SGJD")]
		[DataMember]
		public string SGJD
		{
			get { return _SGJD; }
			set
			{
				this.OnPropertyValueChange("SGJD");
				this._SGJD = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("CGJD")]
		[DataMember]
		public string CGJD
		{
			get { return _CGJD; }
			set
			{
				this.OnPropertyValueChange("CGJD");
				this._CGJD = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TSJD")]
		[DataMember]
		public string TSJD
		{
			get { return _TSJD; }
			set
			{
				this.OnPropertyValueChange("TSJD");
				this._TSJD = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("BFJD")]
		[DataMember]
		public string BFJD
		{
			get { return _BFJD; }
			set
			{
				this.OnPropertyValueChange("BFJD");
				this._BFJD = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("XMJD")]
		[DataMember]
		public string XMJD
		{
			get { return _XMJD; }
			set
			{
				this.OnPropertyValueChange("XMJD");
				this._XMJD = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PROJECT_NO")]
		[DataMember]
		public int? PROJECT_NO
		{
			get { return _PROJECT_NO; }
			set
			{
				this.OnPropertyValueChange("PROJECT_NO");
				this._PROJECT_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("JDLB")]
		[DataMember]
		public int? JDLB
		{
			get { return _JDLB; }
			set
			{
				this.OnPropertyValueChange("JDLB");
				this._JDLB = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("AREA_NO")]
		[DataMember]
		public int? AREA_NO
		{
			get { return _AREA_NO; }
			set
			{
				this.OnPropertyValueChange("AREA_NO");
				this._AREA_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("AREAEXT_SHT")]
		[DataMember]
		public string AREAEXT_SHT
		{
			get { return _AREAEXT_SHT; }
			set
			{
				this.OnPropertyValueChange("AREAEXT_SHT");
				this._AREAEXT_SHT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PRJTYP_NO")]
		[DataMember]
		public int? PRJTYP_NO
		{
			get { return _PRJTYP_NO; }
			set
			{
				this.OnPropertyValueChange("PRJTYP_NO");
				this._PRJTYP_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SRVKIND_NO")]
		[DataMember]
		public int? SRVKIND_NO
		{
			get { return _SRVKIND_NO; }
			set
			{
				this.OnPropertyValueChange("SRVKIND_NO");
				this._SRVKIND_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_ISFIRST")]
		[DataMember]
		public string TM_ISFIRST
		{
			get { return _TM_ISFIRST; }
			set
			{
				this.OnPropertyValueChange("TM_ISFIRST");
				this._TM_ISFIRST = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TMD_STA")]
		[DataMember]
		public string TMD_STA
		{
			get { return _TMD_STA; }
			set
			{
				this.OnPropertyValueChange("TMD_STA");
				this._TMD_STA = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TM_NOT")]
		[DataMember]
		public string TM_NOT
		{
			get { return _TM_NOT; }
			set
			{
				this.OnPropertyValueChange("TM_NOT");
				this._TM_NOT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("BARTZ_ID")]
		[DataMember]
		public string BARTZ_ID
		{
			get { return _BARTZ_ID; }
			set
			{
				this.OnPropertyValueChange("BARTZ_ID");
				this._BARTZ_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PRJJD_ID")]
		[DataMember]
		public string PRJJD_ID
		{
			get { return _PRJJD_ID; }
			set
			{
				this.OnPropertyValueChange("PRJJD_ID");
				this._PRJJD_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("BAR_USR")]
		[DataMember]
		public string BAR_USR
		{
			get { return _BAR_USR; }
			set
			{
				this.OnPropertyValueChange("BAR_USR");
				this._BAR_USR = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SJ_DSC")]
		[DataMember]
		public string SJ_DSC
		{
			get { return _SJ_DSC; }
			set
			{
				this.OnPropertyValueChange("SJ_DSC");
				this._SJ_DSC = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("BAR_JD")]
		[DataMember]
		public string BAR_JD
		{
			get { return _BAR_JD; }
			set
			{
				this.OnPropertyValueChange("BAR_JD");
				this._BAR_JD = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PRJ_ID")]
		[DataMember]
		public string PRJ_ID
		{
			get { return _PRJ_ID; }
			set
			{
				this.OnPropertyValueChange("PRJ_ID");
				this._PRJ_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PLAPRJ_USR")]
		[DataMember]
		public string PLAPRJ_USR
		{
			get { return _PLAPRJ_USR; }
			set
			{
				this.OnPropertyValueChange("PLAPRJ_USR");
				this._PLAPRJ_USR = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ENPROJECT_NAM")]
		[DataMember]
		public string ENPROJECT_NAM
		{
			get { return _ENPROJECT_NAM; }
			set
			{
				this.OnPropertyValueChange("ENPROJECT_NAM");
				this._ENPROJECT_NAM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PLABEG_DTM")]
		[DataMember]
		public DateTime? PLABEG_DTM
		{
			get { return _PLABEG_DTM; }
			set
			{
				this.OnPropertyValueChange("PLABEG_DTM");
				this._PLABEG_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PLARK_DTM")]
		[DataMember]
		public DateTime? PLARK_DTM
		{
			get { return _PLARK_DTM; }
			set
			{
				this.OnPropertyValueChange("PLARK_DTM");
				this._PLARK_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PROJECT_GM")]
		[DataMember]
		public string PROJECT_GM
		{
			get { return _PROJECT_GM; }
			set
			{
				this.OnPropertyValueChange("PROJECT_GM");
				this._PROJECT_GM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PROJECT_NAM")]
		[DataMember]
		public string PROJECT_NAM
		{
			get { return _PROJECT_NAM; }
			set
			{
				this.OnPropertyValueChange("PROJECT_NAM");
				this._PROJECT_NAM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PRJMGR_ID")]
		[DataMember]
		public string PRJMGR_ID
		{
			get { return _PRJMGR_ID; }
			set
			{
				this.OnPropertyValueChange("PRJMGR_ID");
				this._PRJMGR_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SJMGRZ_ID")]
		[DataMember]
		public string SJMGRZ_ID
		{
			get { return _SJMGRZ_ID; }
			set
			{
				this.OnPropertyValueChange("SJMGRZ_ID");
				this._SJMGRZ_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SJMGRF_ID")]
		[DataMember]
		public string SJMGRF_ID
		{
			get { return _SJMGRF_ID; }
			set
			{
				this.OnPropertyValueChange("SJMGRF_ID");
				this._SJMGRF_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PRJFZR_ID")]
		[DataMember]
		public string PRJFZR_ID
		{
			get { return _PRJFZR_ID; }
			set
			{
				this.OnPropertyValueChange("PRJFZR_ID");
				this._PRJFZR_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SJZL_ID")]
		[DataMember]
		public string SJZL_ID
		{
			get { return _SJZL_ID; }
			set
			{
				this.OnPropertyValueChange("SJZL_ID");
				this._SJZL_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TMXD_DTM")]
		[DataMember]
		public DateTime? TMXD_DTM
		{
			get { return _TMXD_DTM; }
			set
			{
				this.OnPropertyValueChange("TMXD_DTM");
				this._TMXD_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PROJ_LEVEL")]
		[DataMember]
		public string PROJ_LEVEL
		{
			get { return _PROJ_LEVEL; }
			set
			{
				this.OnPropertyValueChange("PROJ_LEVEL");
				this._PROJ_LEVEL = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SKL_NOS")]
		[DataMember]
		public string SKL_NOS
		{
			get { return _SKL_NOS; }
			set
			{
				this.OnPropertyValueChange("SKL_NOS");
				this._SKL_NOS = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("USRBG_DSC")]
		[DataMember]
		public string USRBG_DSC
		{
			get { return _USRBG_DSC; }
			set
			{
				this.OnPropertyValueChange("USRBG_DSC");
				this._USRBG_DSC = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SKL_NAMS")]
		[DataMember]
		public string SKL_NAMS
		{
			get { return _SKL_NAMS; }
			set
			{
				this.OnPropertyValueChange("SKL_NAMS");
				this._SKL_NAMS = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("WDSUR_ID")]
		[DataMember]
		public string WDSUR_ID
		{
			get { return _WDSUR_ID; }
			set
			{
				this.OnPropertyValueChange("WDSUR_ID");
				this._WDSUR_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PRJPLAN_NO")]
		[DataMember]
		public int? PRJPLAN_NO
		{
			get { return _PRJPLAN_NO; }
			set
			{
				this.OnPropertyValueChange("PRJPLAN_NO");
				this._PRJPLAN_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SJWG_DTM")]
		[DataMember]
		public DateTime? SJWG_DTM
		{
			get { return _SJWG_DTM; }
			set
			{
				this.OnPropertyValueChange("SJWG_DTM");
				this._SJWG_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TMPROSTA")]
		[DataMember]
		public string TMPROSTA
		{
			get { return _TMPROSTA; }
			set
			{
				this.OnPropertyValueChange("TMPROSTA");
				this._TMPROSTA = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("QYBH_ID")]
		[DataMember]
		public string QYBH_ID
		{
			get { return _QYBH_ID; }
			set
			{
				this.OnPropertyValueChange("QYBH_ID");
				this._QYBH_ID = value;
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// 获取实体中的主键列
		/// </summary>
		public override Field[] GetPrimaryKeyFields()
		{
			return new Field[] {
				_.TM_NO,
			};
		}
		/// <summary>
		/// 获取列信息
		/// </summary>
		public override Field[] GetFields()
		{
			return new Field[] {
				_.TM_NO,
				_.ORG_NO,
				_.TM_ID,
				_.TM_NAM,
				_.TMJSXZ,
				_.TM_DTM,
				_.TM_BEGIN,
				_.TM_END,
				_.TM_GQYQ,
				_.CST_NO,
				_.ZBGCS,
				_.TM_DSC,
				_.TM_SHT,
				_.PJBAR_NO,
				_.PJBARPAY_TYP,
				_.CBKXX,
				_.KXX,
				_.CBSJ,
				_.SGT,
				_.JGT,
				_.SGTYS,
				_.EXT1,
				_.EXT2,
				_.EXT3,
				_.FSTUSR_ID,
				_.FSTUSR_DTM,
				_.LSTUSR_ID,
				_.LSTUSR_DTM,
				_.TM_STA,
				_.TM_SRC,
				_.TPGCLB,
				_.YXHF,
				_.SGJD,
				_.CGJD,
				_.TSJD,
				_.BFJD,
				_.XMJD,
				_.PROJECT_NO,
				_.JDLB,
				_.AREA_NO,
				_.AREAEXT_SHT,
				_.PRJTYP_NO,
				_.SRVKIND_NO,
				_.TM_ISFIRST,
				_.TMD_STA,
				_.TM_NOT,
				_.BARTZ_ID,
				_.PRJJD_ID,
				_.BAR_USR,
				_.SJ_DSC,
				_.BAR_JD,
				_.PRJ_ID,
				_.PLAPRJ_USR,
				_.ENPROJECT_NAM,
				_.PLABEG_DTM,
				_.PLARK_DTM,
				_.PROJECT_GM,
				_.PROJECT_NAM,
				_.PRJMGR_ID,
				_.SJMGRZ_ID,
				_.SJMGRF_ID,
				_.PRJFZR_ID,
				_.SJZL_ID,
				_.TMXD_DTM,
				_.PROJ_LEVEL,
				_.SKL_NOS,
				_.USRBG_DSC,
				_.SKL_NAMS,
				_.WDSUR_ID,
				_.PRJPLAN_NO,
				_.SJWG_DTM,
				_.TMPROSTA,
				_.QYBH_ID,
			};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._TM_NO,
				this._ORG_NO,
				this._TM_ID,
				this._TM_NAM,
				this._TMJSXZ,
				this._TM_DTM,
				this._TM_BEGIN,
				this._TM_END,
				this._TM_GQYQ,
				this._CST_NO,
				this._ZBGCS,
				this._TM_DSC,
				this._TM_SHT,
				this._PJBAR_NO,
				this._PJBARPAY_TYP,
				this._CBKXX,
				this._KXX,
				this._CBSJ,
				this._SGT,
				this._JGT,
				this._SGTYS,
				this._EXT1,
				this._EXT2,
				this._EXT3,
				this._FSTUSR_ID,
				this._FSTUSR_DTM,
				this._LSTUSR_ID,
				this._LSTUSR_DTM,
				this._TM_STA,
				this._TM_SRC,
				this._TPGCLB,
				this._YXHF,
				this._SGJD,
				this._CGJD,
				this._TSJD,
				this._BFJD,
				this._XMJD,
				this._PROJECT_NO,
				this._JDLB,
				this._AREA_NO,
				this._AREAEXT_SHT,
				this._PRJTYP_NO,
				this._SRVKIND_NO,
				this._TM_ISFIRST,
				this._TMD_STA,
				this._TM_NOT,
				this._BARTZ_ID,
				this._PRJJD_ID,
				this._BAR_USR,
				this._SJ_DSC,
				this._BAR_JD,
				this._PRJ_ID,
				this._PLAPRJ_USR,
				this._ENPROJECT_NAM,
				this._PLABEG_DTM,
				this._PLARK_DTM,
				this._PROJECT_GM,
				this._PROJECT_NAM,
				this._PRJMGR_ID,
				this._SJMGRZ_ID,
				this._SJMGRF_ID,
				this._PRJFZR_ID,
				this._SJZL_ID,
				this._TMXD_DTM,
				this._PROJ_LEVEL,
				this._SKL_NOS,
				this._USRBG_DSC,
				this._SKL_NAMS,
				this._WDSUR_ID,
				this._PRJPLAN_NO,
				this._SJWG_DTM,
				this._TMPROSTA,
				this._QYBH_ID,
			};
		}
		/// <summary>
		/// 是否是v1.10.5.6及以上版本实体。
		/// </summary>
		/// <returns></returns>
		public override bool V1_10_5_6_Plus()
		{
			return true;
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// * 
			/// </summary>
			public readonly static Field All = new Field("*", "DCTASKMENUMST");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_NO = new Field("TM_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ORG_NO = new Field("ORG_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_ID = new Field("TM_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_NAM = new Field("TM_NAM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TMJSXZ = new Field("TMJSXZ", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_DTM = new Field("TM_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_BEGIN = new Field("TM_BEGIN", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_END = new Field("TM_END", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_GQYQ = new Field("TM_GQYQ", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field CST_NO = new Field("CST_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ZBGCS = new Field("ZBGCS", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_DSC = new Field("TM_DSC", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_SHT = new Field("TM_SHT", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PJBAR_NO = new Field("PJBAR_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PJBARPAY_TYP = new Field("PJBARPAY_TYP", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field CBKXX = new Field("CBKXX", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field KXX = new Field("KXX", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field CBSJ = new Field("CBSJ", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SGT = new Field("SGT", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field JGT = new Field("JGT", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SGTYS = new Field("SGTYS", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field EXT1 = new Field("EXT1", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field EXT2 = new Field("EXT2", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field EXT3 = new Field("EXT3", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field FSTUSR_ID = new Field("FSTUSR_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field FSTUSR_DTM = new Field("FSTUSR_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LSTUSR_ID = new Field("LSTUSR_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LSTUSR_DTM = new Field("LSTUSR_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_STA = new Field("TM_STA", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_SRC = new Field("TM_SRC", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TPGCLB = new Field("TPGCLB", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field YXHF = new Field("YXHF", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SGJD = new Field("SGJD", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field CGJD = new Field("CGJD", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TSJD = new Field("TSJD", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field BFJD = new Field("BFJD", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field XMJD = new Field("XMJD", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PROJECT_NO = new Field("PROJECT_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field JDLB = new Field("JDLB", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field AREA_NO = new Field("AREA_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field AREAEXT_SHT = new Field("AREAEXT_SHT", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PRJTYP_NO = new Field("PRJTYP_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SRVKIND_NO = new Field("SRVKIND_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_ISFIRST = new Field("TM_ISFIRST", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TMD_STA = new Field("TMD_STA", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TM_NOT = new Field("TM_NOT", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field BARTZ_ID = new Field("BARTZ_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PRJJD_ID = new Field("PRJJD_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field BAR_USR = new Field("BAR_USR", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SJ_DSC = new Field("SJ_DSC", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field BAR_JD = new Field("BAR_JD", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PRJ_ID = new Field("PRJ_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PLAPRJ_USR = new Field("PLAPRJ_USR", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ENPROJECT_NAM = new Field("ENPROJECT_NAM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PLABEG_DTM = new Field("PLABEG_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PLARK_DTM = new Field("PLARK_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PROJECT_GM = new Field("PROJECT_GM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PROJECT_NAM = new Field("PROJECT_NAM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PRJMGR_ID = new Field("PRJMGR_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SJMGRZ_ID = new Field("SJMGRZ_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SJMGRF_ID = new Field("SJMGRF_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PRJFZR_ID = new Field("PRJFZR_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SJZL_ID = new Field("SJZL_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TMXD_DTM = new Field("TMXD_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PROJ_LEVEL = new Field("PROJ_LEVEL", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SKL_NOS = new Field("SKL_NOS", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field USRBG_DSC = new Field("USRBG_DSC", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SKL_NAMS = new Field("SKL_NAMS", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field WDSUR_ID = new Field("WDSUR_ID", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PRJPLAN_NO = new Field("PRJPLAN_NO", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SJWG_DTM = new Field("SJWG_DTM", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field TMPROSTA = new Field("TMPROSTA", "DCTASKMENUMST", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field QYBH_ID = new Field("QYBH_ID", "DCTASKMENUMST", "");
		}
		#endregion
	}
}