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
using Dos.ORM;

namespace Power.SPMEMS.Services
{
	/// <summary>
	/// 实体类NPS_PUR_PurBook。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("NPS_PUR_PurBook")]
	[Serializable]
	public partial class NPS_PUR_PurBook : Entity
	{
		#region Model
		private Guid _ID;
		private string _Code;
		private string _Title;
		private string _DeptName;
		private string _CellCode;
		private string _PurchaseNo;
		private string _Version;
		private string _PurchaseName;
		private DateTime? _ERPTime;
		private string _ISEquipSiteCode;
		private string _EquipSiteCode;
		private string _EquipName;
		private string _Spec;
		private string _SPMCode;
		private string _Unit;
		private float? _Num;
		private float? _BudEstPrice;
		private string _CMSIConCode;
		private string _SubcontCode;
		private string _SubcontName;
		private float? _ConMoney;
		private DateTime? _PreDelPlanDate;
		private DateTime? _PreDelEstDate;
		private DateTime? _PreDelActDate;
		private DateTime? _PreStaPlanDate;
		private DateTime? _PreStaEstDate;
		private DateTime? _PreStaActDate;
		private DateTime? _PreSignPlanDate;
		private DateTime? _PreSignEstDate;
		private DateTime? _PreSignActDate;
		private DateTime? _SignInqPlanDate;
		private DateTime? _SignInqEstDate;
		private DateTime? _SignInqActDate;
		private DateTime? _SignWinPlanDate;
		private DateTime? _SignWinEstDate;
		private DateTime? _SignWinActDate;
		private DateTime? _SignTecPlanDate;
		private DateTime? _SignTecEstDate;
		private DateTime? _SignTecActDate;
		private DateTime? _SignConPlanDate;
		private DateTime? _SignConEstDate;
		private DateTime? _SignConActDate;
		private string _TableName;
		private Guid? _BizAreaId;
		private int? _Sequ;
		private int? _Status;
		private Guid? _RegHumId;
		private string _RegHumName;
		private DateTime? _RegDate;
		private Guid? _RegPosiId;
		private Guid? _RegDeptId;
		private Guid? _EpsProjId;
		private Guid? _RecycleHumId;
		private Guid? _UpdHumId;
		private string _UpdHumName;
		private DateTime? _UpdDate;
		private Guid? _ApprHumId;
		private string _ApprHumName;
		private DateTime? _ApprDate;
		private string _Remark;
		private Guid? _OwnProjId;
		private string _OwnProjName;
		private string _EpsProjCode;
		private string _EpsProjName;
		private Guid? _CompanyID;
		private string _CompanyCode;
		private string _CompanyName;
		private DateTime? _DesPlanDeliDate;
		private string _ImportTag;
		private string _LastChangeHumName;
		private string _LastChangeHumCode;
		private Guid? _LastChangeHumID;
		private DateTime? _LastChangeRegDate;

		/// <summary>
		/// 主键
		/// </summary>
		[Field("ID")]
		public Guid ID
		{
			get { return _ID; }
			set
			{
				this.OnPropertyValueChange("ID");
				this._ID = value;
			}
		}
		/// <summary>
		/// 编号
		/// </summary>
		[Field("Code")]
		public string Code
		{
			get { return _Code; }
			set
			{
				this.OnPropertyValueChange("Code");
				this._Code = value;
			}
		}
		/// <summary>
		/// 标题
		/// </summary>
		[Field("Title")]
		public string Title
		{
			get { return _Title; }
			set
			{
				this.OnPropertyValueChange("Title");
				this._Title = value;
			}
		}
		/// <summary>
		/// 所属项目部
		/// </summary>
		[Field("DeptName")]
		public string DeptName
		{
			get { return _DeptName; }
			set
			{
				this.OnPropertyValueChange("DeptName");
				this._DeptName = value;
			}
		}
		/// <summary>
		/// 主项单元号
		/// </summary>
		[Field("CellCode")]
		public string CellCode
		{
			get { return _CellCode; }
			set
			{
				this.OnPropertyValueChange("CellCode");
				this._CellCode = value;
			}
		}
		/// <summary>
		/// 请购文件编号
		/// </summary>
		[Field("PurchaseNo")]
		public string PurchaseNo
		{
			get { return _PurchaseNo; }
			set
			{
				this.OnPropertyValueChange("PurchaseNo");
				this._PurchaseNo = value;
			}
		}
		/// <summary>
		/// 版本
		/// </summary>
		[Field("Version")]
		public string Version
		{
			get { return _Version; }
			set
			{
				this.OnPropertyValueChange("Version");
				this._Version = value;
			}
		}
		/// <summary>
		/// 请购文件名称
		/// </summary>
		[Field("PurchaseName")]
		public string PurchaseName
		{
			get { return _PurchaseName; }
			set
			{
				this.OnPropertyValueChange("PurchaseName");
				this._PurchaseName = value;
			}
		}
		/// <summary>
		/// 推送ERP时间
		/// </summary>
		[Field("ERPTime")]
		public DateTime? ERPTime
		{
			get { return _ERPTime; }
			set
			{
				this.OnPropertyValueChange("ERPTime");
				this._ERPTime = value;
			}
		}
		/// <summary>
		/// 是否采用设备位号统计
		/// </summary>
		[Field("ISEquipSiteCode")]
		public string ISEquipSiteCode
		{
			get { return _ISEquipSiteCode; }
			set
			{
				this.OnPropertyValueChange("ISEquipSiteCode");
				this._ISEquipSiteCode = value;
			}
		}
		/// <summary>
		/// 设备位号
		/// </summary>
		[Field("EquipSiteCode")]
		public string EquipSiteCode
		{
			get { return _EquipSiteCode; }
			set
			{
				this.OnPropertyValueChange("EquipSiteCode");
				this._EquipSiteCode = value;
			}
		}
		/// <summary>
		/// 设备名称
		/// </summary>
		[Field("EquipName")]
		public string EquipName
		{
			get { return _EquipName; }
			set
			{
				this.OnPropertyValueChange("EquipName");
				this._EquipName = value;
			}
		}
		/// <summary>
		/// 型号规格及主要参数
		/// </summary>
		[Field("Spec")]
		public string Spec
		{
			get { return _Spec; }
			set
			{
				this.OnPropertyValueChange("Spec");
				this._Spec = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SPMCode")]
		public string SPMCode
		{
			get { return _SPMCode; }
			set
			{
				this.OnPropertyValueChange("SPMCode");
				this._SPMCode = value;
			}
		}
		/// <summary>
		/// 计量单位
		/// </summary>
		[Field("Unit")]
		public string Unit
		{
			get { return _Unit; }
			set
			{
				this.OnPropertyValueChange("Unit");
				this._Unit = value;
			}
		}
		/// <summary>
		/// 数量
		/// </summary>
		[Field("Num")]
		public float? Num
		{
			get { return _Num; }
			set
			{
				this.OnPropertyValueChange("Num");
				this._Num = value;
			}
		}
		/// <summary>
		/// 上报概算
		/// </summary>
		[Field("BudEstPrice")]
		public float? BudEstPrice
		{
			get { return _BudEstPrice; }
			set
			{
				this.OnPropertyValueChange("BudEstPrice");
				this._BudEstPrice = value;
			}
		}
		/// <summary>
		/// CMSI合同号
		/// </summary>
		[Field("CMSIConCode")]
		public string CMSIConCode
		{
			get { return _CMSIConCode; }
			set
			{
				this.OnPropertyValueChange("CMSIConCode");
				this._CMSIConCode = value;
			}
		}
		/// <summary>
		/// 供应商编号
		/// </summary>
		[Field("SubcontCode")]
		public string SubcontCode
		{
			get { return _SubcontCode; }
			set
			{
				this.OnPropertyValueChange("SubcontCode");
				this._SubcontCode = value;
			}
		}
		/// <summary>
		/// 供应商名称
		/// </summary>
		[Field("SubcontName")]
		public string SubcontName
		{
			get { return _SubcontName; }
			set
			{
				this.OnPropertyValueChange("SubcontName");
				this._SubcontName = value;
			}
		}
		/// <summary>
		/// 合同金额
		/// </summary>
		[Field("ConMoney")]
		public float? ConMoney
		{
			get { return _ConMoney; }
			set
			{
				this.OnPropertyValueChange("ConMoney");
				this._ConMoney = value;
			}
		}
		/// <summary>
		/// 订货前准备订货技术资料交付计划时间
		/// </summary>
		[Field("PreDelPlanDate")]
		public DateTime? PreDelPlanDate
		{
			get { return _PreDelPlanDate; }
			set
			{
				this.OnPropertyValueChange("PreDelPlanDate");
				this._PreDelPlanDate = value;
			}
		}
		/// <summary>
		/// 订货前准备订货技术资料交付预计时间
		/// </summary>
		[Field("PreDelEstDate")]
		public DateTime? PreDelEstDate
		{
			get { return _PreDelEstDate; }
			set
			{
				this.OnPropertyValueChange("PreDelEstDate");
				this._PreDelEstDate = value;
			}
		}
		/// <summary>
		/// 订货前准备订货技术资料交付实际时间
		/// </summary>
		[Field("PreDelActDate")]
		public DateTime? PreDelActDate
		{
			get { return _PreDelActDate; }
			set
			{
				this.OnPropertyValueChange("PreDelActDate");
				this._PreDelActDate = value;
			}
		}
		/// <summary>
		/// 订货前准备技术交流开始计划时间
		/// </summary>
		[Field("PreStaPlanDate")]
		public DateTime? PreStaPlanDate
		{
			get { return _PreStaPlanDate; }
			set
			{
				this.OnPropertyValueChange("PreStaPlanDate");
				this._PreStaPlanDate = value;
			}
		}
		/// <summary>
		/// 订货前准备技术交流开始预计时间
		/// </summary>
		[Field("PreStaEstDate")]
		public DateTime? PreStaEstDate
		{
			get { return _PreStaEstDate; }
			set
			{
				this.OnPropertyValueChange("PreStaEstDate");
				this._PreStaEstDate = value;
			}
		}
		/// <summary>
		/// 订货前准备技术交流开始实际时间
		/// </summary>
		[Field("PreStaActDate")]
		public DateTime? PreStaActDate
		{
			get { return _PreStaActDate; }
			set
			{
				this.OnPropertyValueChange("PreStaActDate");
				this._PreStaActDate = value;
			}
		}
		/// <summary>
		/// 订货前准备技术协议签订计划时间
		/// </summary>
		[Field("PreSignPlanDate")]
		public DateTime? PreSignPlanDate
		{
			get { return _PreSignPlanDate; }
			set
			{
				this.OnPropertyValueChange("PreSignPlanDate");
				this._PreSignPlanDate = value;
			}
		}
		/// <summary>
		/// 订货前准备技术协议签订预计时间
		/// </summary>
		[Field("PreSignEstDate")]
		public DateTime? PreSignEstDate
		{
			get { return _PreSignEstDate; }
			set
			{
				this.OnPropertyValueChange("PreSignEstDate");
				this._PreSignEstDate = value;
			}
		}
		/// <summary>
		/// 订货前准备技术协议签订实际时间
		/// </summary>
		[Field("PreSignActDate")]
		public DateTime? PreSignActDate
		{
			get { return _PreSignActDate; }
			set
			{
				this.OnPropertyValueChange("PreSignActDate");
				this._PreSignActDate = value;
			}
		}
		/// <summary>
		/// 商务合同商务询价发出计划时间
		/// </summary>
		[Field("SignInqPlanDate")]
		public DateTime? SignInqPlanDate
		{
			get { return _SignInqPlanDate; }
			set
			{
				this.OnPropertyValueChange("SignInqPlanDate");
				this._SignInqPlanDate = value;
			}
		}
		/// <summary>
		/// 商务合同商务询价发出预计时间
		/// </summary>
		[Field("SignInqEstDate")]
		public DateTime? SignInqEstDate
		{
			get { return _SignInqEstDate; }
			set
			{
				this.OnPropertyValueChange("SignInqEstDate");
				this._SignInqEstDate = value;
			}
		}
		/// <summary>
		/// 商务合同商务询价发出实际时间
		/// </summary>
		[Field("SignInqActDate")]
		public DateTime? SignInqActDate
		{
			get { return _SignInqActDate; }
			set
			{
				this.OnPropertyValueChange("SignInqActDate");
				this._SignInqActDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订中标通知发出计划时间
		/// </summary>
		[Field("SignWinPlanDate")]
		public DateTime? SignWinPlanDate
		{
			get { return _SignWinPlanDate; }
			set
			{
				this.OnPropertyValueChange("SignWinPlanDate");
				this._SignWinPlanDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订中标通知发出预计时间
		/// </summary>
		[Field("SignWinEstDate")]
		public DateTime? SignWinEstDate
		{
			get { return _SignWinEstDate; }
			set
			{
				this.OnPropertyValueChange("SignWinEstDate");
				this._SignWinEstDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订中标通知发出实际时间
		/// </summary>
		[Field("SignWinActDate")]
		public DateTime? SignWinActDate
		{
			get { return _SignWinActDate; }
			set
			{
				this.OnPropertyValueChange("SignWinActDate");
				this._SignWinActDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订技术协议签订计划时间
		/// </summary>
		[Field("SignTecPlanDate")]
		public DateTime? SignTecPlanDate
		{
			get { return _SignTecPlanDate; }
			set
			{
				this.OnPropertyValueChange("SignTecPlanDate");
				this._SignTecPlanDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订技术协议签订预计时间
		/// </summary>
		[Field("SignTecEstDate")]
		public DateTime? SignTecEstDate
		{
			get { return _SignTecEstDate; }
			set
			{
				this.OnPropertyValueChange("SignTecEstDate");
				this._SignTecEstDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订技术协议签订实际时间
		/// </summary>
		[Field("SignTecActDate")]
		public DateTime? SignTecActDate
		{
			get { return _SignTecActDate; }
			set
			{
				this.OnPropertyValueChange("SignTecActDate");
				this._SignTecActDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订商务合同签订计划时间
		/// </summary>
		[Field("SignConPlanDate")]
		public DateTime? SignConPlanDate
		{
			get { return _SignConPlanDate; }
			set
			{
				this.OnPropertyValueChange("SignConPlanDate");
				this._SignConPlanDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订商务合同签订预计时间
		/// </summary>
		[Field("SignConEstDate")]
		public DateTime? SignConEstDate
		{
			get { return _SignConEstDate; }
			set
			{
				this.OnPropertyValueChange("SignConEstDate");
				this._SignConEstDate = value;
			}
		}
		/// <summary>
		/// 商务合同签订商务合同签订实际时间
		/// </summary>
		[Field("SignConActDate")]
		public DateTime? SignConActDate
		{
			get { return _SignConActDate; }
			set
			{
				this.OnPropertyValueChange("SignConActDate");
				this._SignConActDate = value;
			}
		}
		/// <summary>
		/// 数据所属表名
		/// </summary>
		[Field("TableName")]
		public string TableName
		{
			get { return _TableName; }
			set
			{
				this.OnPropertyValueChange("TableName");
				this._TableName = value;
			}
		}
		/// <summary>
		/// 数据录入业务域Id
		/// </summary>
		[Field("BizAreaId")]
		public Guid? BizAreaId
		{
			get { return _BizAreaId; }
			set
			{
				this.OnPropertyValueChange("BizAreaId");
				this._BizAreaId = value;
			}
		}
		/// <summary>
		/// 序号
		/// </summary>
		[Field("Sequ")]
		public int? Sequ
		{
			get { return _Sequ; }
			set
			{
				this.OnPropertyValueChange("Sequ");
				this._Sequ = value;
			}
		}
		/// <summary>
		/// 表单状态
		/// </summary>
		[Field("Status")]
		public int? Status
		{
			get { return _Status; }
			set
			{
				this.OnPropertyValueChange("Status");
				this._Status = value;
			}
		}
		/// <summary>
		/// 录入人Id
		/// </summary>
		[Field("RegHumId")]
		public Guid? RegHumId
		{
			get { return _RegHumId; }
			set
			{
				this.OnPropertyValueChange("RegHumId");
				this._RegHumId = value;
			}
		}
		/// <summary>
		/// 录入人名称
		/// </summary>
		[Field("RegHumName")]
		public string RegHumName
		{
			get { return _RegHumName; }
			set
			{
				this.OnPropertyValueChange("RegHumName");
				this._RegHumName = value;
			}
		}
		/// <summary>
		/// 录入日期
		/// </summary>
		[Field("RegDate")]
		public DateTime? RegDate
		{
			get { return _RegDate; }
			set
			{
				this.OnPropertyValueChange("RegDate");
				this._RegDate = value;
			}
		}
		/// <summary>
		/// 录入人岗位Id
		/// </summary>
		[Field("RegPosiId")]
		public Guid? RegPosiId
		{
			get { return _RegPosiId; }
			set
			{
				this.OnPropertyValueChange("RegPosiId");
				this._RegPosiId = value;
			}
		}
		/// <summary>
		/// 录入人部门Id
		/// </summary>
		[Field("RegDeptId")]
		public Guid? RegDeptId
		{
			get { return _RegDeptId; }
			set
			{
				this.OnPropertyValueChange("RegDeptId");
				this._RegDeptId = value;
			}
		}
		/// <summary>
		/// 记录所属EPS节点Id
		/// </summary>
		[Field("EpsProjId")]
		public Guid? EpsProjId
		{
			get { return _EpsProjId; }
			set
			{
				this.OnPropertyValueChange("EpsProjId");
				this._EpsProjId = value;
			}
		}
		/// <summary>
		/// 删除人Id
		/// </summary>
		[Field("RecycleHumId")]
		public Guid? RecycleHumId
		{
			get { return _RecycleHumId; }
			set
			{
				this.OnPropertyValueChange("RecycleHumId");
				this._RecycleHumId = value;
			}
		}
		/// <summary>
		/// 最后更新人Id
		/// </summary>
		[Field("UpdHumId")]
		public Guid? UpdHumId
		{
			get { return _UpdHumId; }
			set
			{
				this.OnPropertyValueChange("UpdHumId");
				this._UpdHumId = value;
			}
		}
		/// <summary>
		/// 最后更新人名称
		/// </summary>
		[Field("UpdHumName")]
		public string UpdHumName
		{
			get { return _UpdHumName; }
			set
			{
				this.OnPropertyValueChange("UpdHumName");
				this._UpdHumName = value;
			}
		}
		/// <summary>
		/// 最后更新日期
		/// </summary>
		[Field("UpdDate")]
		public DateTime? UpdDate
		{
			get { return _UpdDate; }
			set
			{
				this.OnPropertyValueChange("UpdDate");
				this._UpdDate = value;
			}
		}
		/// <summary>
		/// 批准人Id
		/// </summary>
		[Field("ApprHumId")]
		public Guid? ApprHumId
		{
			get { return _ApprHumId; }
			set
			{
				this.OnPropertyValueChange("ApprHumId");
				this._ApprHumId = value;
			}
		}
		/// <summary>
		/// 批准人名称
		/// </summary>
		[Field("ApprHumName")]
		public string ApprHumName
		{
			get { return _ApprHumName; }
			set
			{
				this.OnPropertyValueChange("ApprHumName");
				this._ApprHumName = value;
			}
		}
		/// <summary>
		/// 批准日期
		/// </summary>
		[Field("ApprDate")]
		public DateTime? ApprDate
		{
			get { return _ApprDate; }
			set
			{
				this.OnPropertyValueChange("ApprDate");
				this._ApprDate = value;
			}
		}
		/// <summary>
		/// 备注
		/// </summary>
		[Field("Remark")]
		public string Remark
		{
			get { return _Remark; }
			set
			{
				this.OnPropertyValueChange("Remark");
				this._Remark = value;
			}
		}
		/// <summary>
		/// 所属项目Id
		/// </summary>
		[Field("OwnProjId")]
		public Guid? OwnProjId
		{
			get { return _OwnProjId; }
			set
			{
				this.OnPropertyValueChange("OwnProjId");
				this._OwnProjId = value;
			}
		}
		/// <summary>
		/// 管理层级名称
		/// </summary>
		[Field("OwnProjName")]
		public string OwnProjName
		{
			get { return _OwnProjName; }
			set
			{
				this.OnPropertyValueChange("OwnProjName");
				this._OwnProjName = value;
			}
		}
		/// <summary>
		/// EPS编号
		/// </summary>
		[Field("EpsProjCode")]
		public string EpsProjCode
		{
			get { return _EpsProjCode; }
			set
			{
				this.OnPropertyValueChange("EpsProjCode");
				this._EpsProjCode = value;
			}
		}
		/// <summary>
		/// EPS名称
		/// </summary>
		[Field("EpsProjName")]
		public string EpsProjName
		{
			get { return _EpsProjName; }
			set
			{
				this.OnPropertyValueChange("EpsProjName");
				this._EpsProjName = value;
			}
		}
		/// <summary>
		/// 单位ID
		/// </summary>
		[Field("CompanyID")]
		public Guid? CompanyID
		{
			get { return _CompanyID; }
			set
			{
				this.OnPropertyValueChange("CompanyID");
				this._CompanyID = value;
			}
		}
		/// <summary>
		/// 单位编号
		/// </summary>
		[Field("CompanyCode")]
		public string CompanyCode
		{
			get { return _CompanyCode; }
			set
			{
				this.OnPropertyValueChange("CompanyCode");
				this._CompanyCode = value;
			}
		}
		/// <summary>
		/// 单位名称
		/// </summary>
		[Field("CompanyName")]
		public string CompanyName
		{
			get { return _CompanyName; }
			set
			{
				this.OnPropertyValueChange("CompanyName");
				this._CompanyName = value;
			}
		}
		/// <summary>
		/// 推送ERP时间(真)
		/// </summary>
		[Field("DesPlanDeliDate")]
		public DateTime? DesPlanDeliDate
		{
			get { return _DesPlanDeliDate; }
			set
			{
				this.OnPropertyValueChange("DesPlanDeliDate");
				this._DesPlanDeliDate = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ImportTag")]
		public string ImportTag
		{
			get { return _ImportTag; }
			set
			{
				this.OnPropertyValueChange("ImportTag");
				this._ImportTag = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("LastChangeHumName")]
		public string LastChangeHumName
		{
			get { return _LastChangeHumName; }
			set
			{
				this.OnPropertyValueChange("LastChangeHumName");
				this._LastChangeHumName = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("LastChangeHumCode")]
		public string LastChangeHumCode
		{
			get { return _LastChangeHumCode; }
			set
			{
				this.OnPropertyValueChange("LastChangeHumCode");
				this._LastChangeHumCode = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("LastChangeHumID")]
		public Guid? LastChangeHumID
		{
			get { return _LastChangeHumID; }
			set
			{
				this.OnPropertyValueChange("LastChangeHumID");
				this._LastChangeHumID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("LastChangeRegDate")]
		public DateTime? LastChangeRegDate
		{
			get { return _LastChangeRegDate; }
			set
			{
				this.OnPropertyValueChange("LastChangeRegDate");
				this._LastChangeRegDate = value;
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
				_.ID,
			};
		}
		/// <summary>
		/// 获取列信息
		/// </summary>
		public override Field[] GetFields()
		{
			return new Field[] {
				_.ID,
				_.Code,
				_.Title,
				_.DeptName,
				_.CellCode,
				_.PurchaseNo,
				_.Version,
				_.PurchaseName,
				_.ERPTime,
				_.ISEquipSiteCode,
				_.EquipSiteCode,
				_.EquipName,
				_.Spec,
				_.SPMCode,
				_.Unit,
				_.Num,
				_.BudEstPrice,
				_.CMSIConCode,
				_.SubcontCode,
				_.SubcontName,
				_.ConMoney,
				_.PreDelPlanDate,
				_.PreDelEstDate,
				_.PreDelActDate,
				_.PreStaPlanDate,
				_.PreStaEstDate,
				_.PreStaActDate,
				_.PreSignPlanDate,
				_.PreSignEstDate,
				_.PreSignActDate,
				_.SignInqPlanDate,
				_.SignInqEstDate,
				_.SignInqActDate,
				_.SignWinPlanDate,
				_.SignWinEstDate,
				_.SignWinActDate,
				_.SignTecPlanDate,
				_.SignTecEstDate,
				_.SignTecActDate,
				_.SignConPlanDate,
				_.SignConEstDate,
				_.SignConActDate,
				_.TableName,
				_.BizAreaId,
				_.Sequ,
				_.Status,
				_.RegHumId,
				_.RegHumName,
				_.RegDate,
				_.RegPosiId,
				_.RegDeptId,
				_.EpsProjId,
				_.RecycleHumId,
				_.UpdHumId,
				_.UpdHumName,
				_.UpdDate,
				_.ApprHumId,
				_.ApprHumName,
				_.ApprDate,
				_.Remark,
				_.OwnProjId,
				_.OwnProjName,
				_.EpsProjCode,
				_.EpsProjName,
				_.CompanyID,
				_.CompanyCode,
				_.CompanyName,
				_.DesPlanDeliDate,
				_.ImportTag,
				_.LastChangeHumName,
				_.LastChangeHumCode,
				_.LastChangeHumID,
				_.LastChangeRegDate,
			};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._Code,
				this._Title,
				this._DeptName,
				this._CellCode,
				this._PurchaseNo,
				this._Version,
				this._PurchaseName,
				this._ERPTime,
				this._ISEquipSiteCode,
				this._EquipSiteCode,
				this._EquipName,
				this._Spec,
				this._SPMCode,
				this._Unit,
				this._Num,
				this._BudEstPrice,
				this._CMSIConCode,
				this._SubcontCode,
				this._SubcontName,
				this._ConMoney,
				this._PreDelPlanDate,
				this._PreDelEstDate,
				this._PreDelActDate,
				this._PreStaPlanDate,
				this._PreStaEstDate,
				this._PreStaActDate,
				this._PreSignPlanDate,
				this._PreSignEstDate,
				this._PreSignActDate,
				this._SignInqPlanDate,
				this._SignInqEstDate,
				this._SignInqActDate,
				this._SignWinPlanDate,
				this._SignWinEstDate,
				this._SignWinActDate,
				this._SignTecPlanDate,
				this._SignTecEstDate,
				this._SignTecActDate,
				this._SignConPlanDate,
				this._SignConEstDate,
				this._SignConActDate,
				this._TableName,
				this._BizAreaId,
				this._Sequ,
				this._Status,
				this._RegHumId,
				this._RegHumName,
				this._RegDate,
				this._RegPosiId,
				this._RegDeptId,
				this._EpsProjId,
				this._RecycleHumId,
				this._UpdHumId,
				this._UpdHumName,
				this._UpdDate,
				this._ApprHumId,
				this._ApprHumName,
				this._ApprDate,
				this._Remark,
				this._OwnProjId,
				this._OwnProjName,
				this._EpsProjCode,
				this._EpsProjName,
				this._CompanyID,
				this._CompanyCode,
				this._CompanyName,
				this._DesPlanDeliDate,
				this._ImportTag,
				this._LastChangeHumName,
				this._LastChangeHumCode,
				this._LastChangeHumID,
				this._LastChangeRegDate,
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
			public readonly static Field All = new Field("*", "NPS_PUR_PurBook");
			/// <summary>
			/// 主键
			/// </summary>
			public readonly static Field ID = new Field("ID", "NPS_PUR_PurBook", "主键");
			/// <summary>
			/// 编号
			/// </summary>
			public readonly static Field Code = new Field("Code", "NPS_PUR_PurBook", "编号");
			/// <summary>
			/// 标题
			/// </summary>
			public readonly static Field Title = new Field("Title", "NPS_PUR_PurBook", "标题");
			/// <summary>
			/// 所属项目部
			/// </summary>
			public readonly static Field DeptName = new Field("DeptName", "NPS_PUR_PurBook", "所属项目部");
			/// <summary>
			/// 主项单元号
			/// </summary>
			public readonly static Field CellCode = new Field("CellCode", "NPS_PUR_PurBook", "主项单元号");
			/// <summary>
			/// 请购文件编号
			/// </summary>
			public readonly static Field PurchaseNo = new Field("PurchaseNo", "NPS_PUR_PurBook", "请购文件编号");
			/// <summary>
			/// 版本
			/// </summary>
			public readonly static Field Version = new Field("Version", "NPS_PUR_PurBook", "版本");
			/// <summary>
			/// 请购文件名称
			/// </summary>
			public readonly static Field PurchaseName = new Field("PurchaseName", "NPS_PUR_PurBook", "请购文件名称");
			/// <summary>
			/// 推送ERP时间
			/// </summary>
			public readonly static Field ERPTime = new Field("ERPTime", "NPS_PUR_PurBook", "推送ERP时间");
			/// <summary>
			/// 是否采用设备位号统计
			/// </summary>
			public readonly static Field ISEquipSiteCode = new Field("ISEquipSiteCode", "NPS_PUR_PurBook", "是否采用设备位号统计");
			/// <summary>
			/// 设备位号
			/// </summary>
			public readonly static Field EquipSiteCode = new Field("EquipSiteCode", "NPS_PUR_PurBook", "设备位号");
			/// <summary>
			/// 设备名称
			/// </summary>
			public readonly static Field EquipName = new Field("EquipName", "NPS_PUR_PurBook", "设备名称");
			/// <summary>
			/// 型号规格及主要参数
			/// </summary>
			public readonly static Field Spec = new Field("Spec", "NPS_PUR_PurBook", "型号规格及主要参数");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SPMCode = new Field("SPMCode", "NPS_PUR_PurBook", "");
			/// <summary>
			/// 计量单位
			/// </summary>
			public readonly static Field Unit = new Field("Unit", "NPS_PUR_PurBook", "计量单位");
			/// <summary>
			/// 数量
			/// </summary>
			public readonly static Field Num = new Field("Num", "NPS_PUR_PurBook", "数量");
			/// <summary>
			/// 上报概算
			/// </summary>
			public readonly static Field BudEstPrice = new Field("BudEstPrice", "NPS_PUR_PurBook", "上报概算");
			/// <summary>
			/// CMSI合同号
			/// </summary>
			public readonly static Field CMSIConCode = new Field("CMSIConCode", "NPS_PUR_PurBook", "CMSI合同号");
			/// <summary>
			/// 供应商编号
			/// </summary>
			public readonly static Field SubcontCode = new Field("SubcontCode", "NPS_PUR_PurBook", "供应商编号");
			/// <summary>
			/// 供应商名称
			/// </summary>
			public readonly static Field SubcontName = new Field("SubcontName", "NPS_PUR_PurBook", "供应商名称");
			/// <summary>
			/// 合同金额
			/// </summary>
			public readonly static Field ConMoney = new Field("ConMoney", "NPS_PUR_PurBook", "合同金额");
			/// <summary>
			/// 订货前准备订货技术资料交付计划时间
			/// </summary>
			public readonly static Field PreDelPlanDate = new Field("PreDelPlanDate", "NPS_PUR_PurBook", "订货前准备订货技术资料交付计划时间");
			/// <summary>
			/// 订货前准备订货技术资料交付预计时间
			/// </summary>
			public readonly static Field PreDelEstDate = new Field("PreDelEstDate", "NPS_PUR_PurBook", "订货前准备订货技术资料交付预计时间");
			/// <summary>
			/// 订货前准备订货技术资料交付实际时间
			/// </summary>
			public readonly static Field PreDelActDate = new Field("PreDelActDate", "NPS_PUR_PurBook", "订货前准备订货技术资料交付实际时间");
			/// <summary>
			/// 订货前准备技术交流开始计划时间
			/// </summary>
			public readonly static Field PreStaPlanDate = new Field("PreStaPlanDate", "NPS_PUR_PurBook", "订货前准备技术交流开始计划时间");
			/// <summary>
			/// 订货前准备技术交流开始预计时间
			/// </summary>
			public readonly static Field PreStaEstDate = new Field("PreStaEstDate", "NPS_PUR_PurBook", "订货前准备技术交流开始预计时间");
			/// <summary>
			/// 订货前准备技术交流开始实际时间
			/// </summary>
			public readonly static Field PreStaActDate = new Field("PreStaActDate", "NPS_PUR_PurBook", "订货前准备技术交流开始实际时间");
			/// <summary>
			/// 订货前准备技术协议签订计划时间
			/// </summary>
			public readonly static Field PreSignPlanDate = new Field("PreSignPlanDate", "NPS_PUR_PurBook", "订货前准备技术协议签订计划时间");
			/// <summary>
			/// 订货前准备技术协议签订预计时间
			/// </summary>
			public readonly static Field PreSignEstDate = new Field("PreSignEstDate", "NPS_PUR_PurBook", "订货前准备技术协议签订预计时间");
			/// <summary>
			/// 订货前准备技术协议签订实际时间
			/// </summary>
			public readonly static Field PreSignActDate = new Field("PreSignActDate", "NPS_PUR_PurBook", "订货前准备技术协议签订实际时间");
			/// <summary>
			/// 商务合同商务询价发出计划时间
			/// </summary>
			public readonly static Field SignInqPlanDate = new Field("SignInqPlanDate", "NPS_PUR_PurBook", "商务合同商务询价发出计划时间");
			/// <summary>
			/// 商务合同商务询价发出预计时间
			/// </summary>
			public readonly static Field SignInqEstDate = new Field("SignInqEstDate", "NPS_PUR_PurBook", "商务合同商务询价发出预计时间");
			/// <summary>
			/// 商务合同商务询价发出实际时间
			/// </summary>
			public readonly static Field SignInqActDate = new Field("SignInqActDate", "NPS_PUR_PurBook", "商务合同商务询价发出实际时间");
			/// <summary>
			/// 商务合同签订中标通知发出计划时间
			/// </summary>
			public readonly static Field SignWinPlanDate = new Field("SignWinPlanDate", "NPS_PUR_PurBook", "商务合同签订中标通知发出计划时间");
			/// <summary>
			/// 商务合同签订中标通知发出预计时间
			/// </summary>
			public readonly static Field SignWinEstDate = new Field("SignWinEstDate", "NPS_PUR_PurBook", "商务合同签订中标通知发出预计时间");
			/// <summary>
			/// 商务合同签订中标通知发出实际时间
			/// </summary>
			public readonly static Field SignWinActDate = new Field("SignWinActDate", "NPS_PUR_PurBook", "商务合同签订中标通知发出实际时间");
			/// <summary>
			/// 商务合同签订技术协议签订计划时间
			/// </summary>
			public readonly static Field SignTecPlanDate = new Field("SignTecPlanDate", "NPS_PUR_PurBook", "商务合同签订技术协议签订计划时间");
			/// <summary>
			/// 商务合同签订技术协议签订预计时间
			/// </summary>
			public readonly static Field SignTecEstDate = new Field("SignTecEstDate", "NPS_PUR_PurBook", "商务合同签订技术协议签订预计时间");
			/// <summary>
			/// 商务合同签订技术协议签订实际时间
			/// </summary>
			public readonly static Field SignTecActDate = new Field("SignTecActDate", "NPS_PUR_PurBook", "商务合同签订技术协议签订实际时间");
			/// <summary>
			/// 商务合同签订商务合同签订计划时间
			/// </summary>
			public readonly static Field SignConPlanDate = new Field("SignConPlanDate", "NPS_PUR_PurBook", "商务合同签订商务合同签订计划时间");
			/// <summary>
			/// 商务合同签订商务合同签订预计时间
			/// </summary>
			public readonly static Field SignConEstDate = new Field("SignConEstDate", "NPS_PUR_PurBook", "商务合同签订商务合同签订预计时间");
			/// <summary>
			/// 商务合同签订商务合同签订实际时间
			/// </summary>
			public readonly static Field SignConActDate = new Field("SignConActDate", "NPS_PUR_PurBook", "商务合同签订商务合同签订实际时间");
			/// <summary>
			/// 数据所属表名
			/// </summary>
			public readonly static Field TableName = new Field("TableName", "NPS_PUR_PurBook", "数据所属表名");
			/// <summary>
			/// 数据录入业务域Id
			/// </summary>
			public readonly static Field BizAreaId = new Field("BizAreaId", "NPS_PUR_PurBook", "数据录入业务域Id");
			/// <summary>
			/// 序号
			/// </summary>
			public readonly static Field Sequ = new Field("Sequ", "NPS_PUR_PurBook", "序号");
			/// <summary>
			/// 表单状态
			/// </summary>
			public readonly static Field Status = new Field("Status", "NPS_PUR_PurBook", "表单状态");
			/// <summary>
			/// 录入人Id
			/// </summary>
			public readonly static Field RegHumId = new Field("RegHumId", "NPS_PUR_PurBook", "录入人Id");
			/// <summary>
			/// 录入人名称
			/// </summary>
			public readonly static Field RegHumName = new Field("RegHumName", "NPS_PUR_PurBook", "录入人名称");
			/// <summary>
			/// 录入日期
			/// </summary>
			public readonly static Field RegDate = new Field("RegDate", "NPS_PUR_PurBook", "录入日期");
			/// <summary>
			/// 录入人岗位Id
			/// </summary>
			public readonly static Field RegPosiId = new Field("RegPosiId", "NPS_PUR_PurBook", "录入人岗位Id");
			/// <summary>
			/// 录入人部门Id
			/// </summary>
			public readonly static Field RegDeptId = new Field("RegDeptId", "NPS_PUR_PurBook", "录入人部门Id");
			/// <summary>
			/// 记录所属EPS节点Id
			/// </summary>
			public readonly static Field EpsProjId = new Field("EpsProjId", "NPS_PUR_PurBook", "记录所属EPS节点Id");
			/// <summary>
			/// 删除人Id
			/// </summary>
			public readonly static Field RecycleHumId = new Field("RecycleHumId", "NPS_PUR_PurBook", "删除人Id");
			/// <summary>
			/// 最后更新人Id
			/// </summary>
			public readonly static Field UpdHumId = new Field("UpdHumId", "NPS_PUR_PurBook", "最后更新人Id");
			/// <summary>
			/// 最后更新人名称
			/// </summary>
			public readonly static Field UpdHumName = new Field("UpdHumName", "NPS_PUR_PurBook", "最后更新人名称");
			/// <summary>
			/// 最后更新日期
			/// </summary>
			public readonly static Field UpdDate = new Field("UpdDate", "NPS_PUR_PurBook", "最后更新日期");
			/// <summary>
			/// 批准人Id
			/// </summary>
			public readonly static Field ApprHumId = new Field("ApprHumId", "NPS_PUR_PurBook", "批准人Id");
			/// <summary>
			/// 批准人名称
			/// </summary>
			public readonly static Field ApprHumName = new Field("ApprHumName", "NPS_PUR_PurBook", "批准人名称");
			/// <summary>
			/// 批准日期
			/// </summary>
			public readonly static Field ApprDate = new Field("ApprDate", "NPS_PUR_PurBook", "批准日期");
			/// <summary>
			/// 备注
			/// </summary>
			public readonly static Field Remark = new Field("Remark", "NPS_PUR_PurBook", "备注");
			/// <summary>
			/// 所属项目Id
			/// </summary>
			public readonly static Field OwnProjId = new Field("OwnProjId", "NPS_PUR_PurBook", "所属项目Id");
			/// <summary>
			/// 管理层级名称
			/// </summary>
			public readonly static Field OwnProjName = new Field("OwnProjName", "NPS_PUR_PurBook", "管理层级名称");
			/// <summary>
			/// EPS编号
			/// </summary>
			public readonly static Field EpsProjCode = new Field("EpsProjCode", "NPS_PUR_PurBook", "EPS编号");
			/// <summary>
			/// EPS名称
			/// </summary>
			public readonly static Field EpsProjName = new Field("EpsProjName", "NPS_PUR_PurBook", "EPS名称");
			/// <summary>
			/// 单位ID
			/// </summary>
			public readonly static Field CompanyID = new Field("CompanyID", "NPS_PUR_PurBook", "单位ID");
			/// <summary>
			/// 单位编号
			/// </summary>
			public readonly static Field CompanyCode = new Field("CompanyCode", "NPS_PUR_PurBook", "单位编号");
			/// <summary>
			/// 单位名称
			/// </summary>
			public readonly static Field CompanyName = new Field("CompanyName", "NPS_PUR_PurBook", "单位名称");
			/// <summary>
			/// 推送ERP时间(真)
			/// </summary>
			public readonly static Field DesPlanDeliDate = new Field("DesPlanDeliDate", "NPS_PUR_PurBook", "推送ERP时间(真)");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ImportTag = new Field("ImportTag", "NPS_PUR_PurBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LastChangeHumName = new Field("LastChangeHumName", "NPS_PUR_PurBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LastChangeHumCode = new Field("LastChangeHumCode", "NPS_PUR_PurBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LastChangeHumID = new Field("LastChangeHumID", "NPS_PUR_PurBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field LastChangeRegDate = new Field("LastChangeRegDate", "NPS_PUR_PurBook", "");
		}
		#endregion
	}
}