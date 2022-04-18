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
	/// 实体类NPS_DES_HumanTime。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("NPS_DES_HumanTime")]
	[Serializable]
	[DataContract]
	public partial class NPS_DES_HumanTime : Entity
	{
		#region Model
		private Guid _ID;
		private string _Code;
		private string _Title;
		private string _ProjId;
		private int? _Sequ1;
		private string _ProjName;
		private string _ReportYear;
		private string _ReportWeek;
		private string _ReportMonth;
		private string _Tag;
		private string _ManageName;
		private float? _Process;
		private float? _Security;
		private float? _Technical;
		private float? _Environmental;
		private float? _Piping;
		private float? _Mill;
		private float? _Tube;
		private float? _Electrical;
		private float? _Instrument;
		private float? _Telecom;
		private float? _StaticEquip;
		private float? _MovingEquip;
		private float? _Building;
		private float? _Structure;
		private float? _Fire;
		private float? _Hvac;
		private float? _Thermal;
		private float? _Drainage;
		private float? _Transport;
		private float? _Powder;
		private float? _MachinePart;
		private float? _Estimate;
		private float? _Amount;
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

		/// <summary>
		/// 主键
		/// </summary>
		[Field("ID")]
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		/// 工程号
		/// </summary>
		[Field("ProjId")]
		[DataMember]
		public string ProjId
		{
			get { return _ProjId; }
			set
			{
				this.OnPropertyValueChange("ProjId");
				this._ProjId = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Sequ1")]
		[DataMember]
		public int? Sequ1
		{
			get { return _Sequ1; }
			set
			{
				this.OnPropertyValueChange("Sequ1");
				this._Sequ1 = value;
			}
		}
		/// <summary>
		/// 项目名称
		/// </summary>
		[Field("ProjName")]
		[DataMember]
		public string ProjName
		{
			get { return _ProjName; }
			set
			{
				this.OnPropertyValueChange("ProjName");
				this._ProjName = value;
			}
		}
		/// <summary>
		/// 报告年度
		/// </summary>
		[Field("ReportYear")]
		[DataMember]
		public string ReportYear
		{
			get { return _ReportYear; }
			set
			{
				this.OnPropertyValueChange("ReportYear");
				this._ReportYear = value;
			}
		}
		/// <summary>
		/// 报告周次
		/// </summary>
		[Field("ReportWeek")]
		[DataMember]
		public string ReportWeek
		{
			get { return _ReportWeek; }
			set
			{
				this.OnPropertyValueChange("ReportWeek");
				this._ReportWeek = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ReportMonth")]
		[DataMember]
		public string ReportMonth
		{
			get { return _ReportMonth; }
			set
			{
				this.OnPropertyValueChange("ReportMonth");
				this._ReportMonth = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Tag")]
		[DataMember]
		public string Tag
		{
			get { return _Tag; }
			set
			{
				this.OnPropertyValueChange("Tag");
				this._Tag = value;
			}
		}
		/// <summary>
		/// 项目经理
		/// </summary>
		[Field("ManageName")]
		[DataMember]
		public string ManageName
		{
			get { return _ManageName; }
			set
			{
				this.OnPropertyValueChange("ManageName");
				this._ManageName = value;
			}
		}
		/// <summary>
		/// 工艺
		/// </summary>
		[Field("Process")]
		[DataMember]
		public float? Process
		{
			get { return _Process; }
			set
			{
				this.OnPropertyValueChange("Process");
				this._Process = value;
			}
		}
		/// <summary>
		/// 安全
		/// </summary>
		[Field("Security")]
		[DataMember]
		public float? Security
		{
			get { return _Security; }
			set
			{
				this.OnPropertyValueChange("Security");
				this._Security = value;
			}
		}
		/// <summary>
		/// 技术经济
		/// </summary>
		[Field("Technical")]
		[DataMember]
		public float? Technical
		{
			get { return _Technical; }
			set
			{
				this.OnPropertyValueChange("Technical");
				this._Technical = value;
			}
		}
		/// <summary>
		/// 环保
		/// </summary>
		[Field("Environmental")]
		[DataMember]
		public float? Environmental
		{
			get { return _Environmental; }
			set
			{
				this.OnPropertyValueChange("Environmental");
				this._Environmental = value;
			}
		}
		/// <summary>
		/// 管道
		/// </summary>
		[Field("Piping")]
		[DataMember]
		public float? Piping
		{
			get { return _Piping; }
			set
			{
				this.OnPropertyValueChange("Piping");
				this._Piping = value;
			}
		}
		/// <summary>
		/// 管机
		/// </summary>
		[Field("Mill")]
		[DataMember]
		public float? Mill
		{
			get { return _Mill; }
			set
			{
				this.OnPropertyValueChange("Mill");
				this._Mill = value;
			}
		}
		/// <summary>
		/// 管材
		/// </summary>
		[Field("Tube")]
		[DataMember]
		public float? Tube
		{
			get { return _Tube; }
			set
			{
				this.OnPropertyValueChange("Tube");
				this._Tube = value;
			}
		}
		/// <summary>
		/// 电气
		/// </summary>
		[Field("Electrical")]
		[DataMember]
		public float? Electrical
		{
			get { return _Electrical; }
			set
			{
				this.OnPropertyValueChange("Electrical");
				this._Electrical = value;
			}
		}
		/// <summary>
		/// 仪表
		/// </summary>
		[Field("Instrument")]
		[DataMember]
		public float? Instrument
		{
			get { return _Instrument; }
			set
			{
				this.OnPropertyValueChange("Instrument");
				this._Instrument = value;
			}
		}
		/// <summary>
		/// 电信
		/// </summary>
		[Field("Telecom")]
		[DataMember]
		public float? Telecom
		{
			get { return _Telecom; }
			set
			{
				this.OnPropertyValueChange("Telecom");
				this._Telecom = value;
			}
		}
		/// <summary>
		/// 静设备
		/// </summary>
		[Field("StaticEquip")]
		[DataMember]
		public float? StaticEquip
		{
			get { return _StaticEquip; }
			set
			{
				this.OnPropertyValueChange("StaticEquip");
				this._StaticEquip = value;
			}
		}
		/// <summary>
		/// 动设备
		/// </summary>
		[Field("MovingEquip")]
		[DataMember]
		public float? MovingEquip
		{
			get { return _MovingEquip; }
			set
			{
				this.OnPropertyValueChange("MovingEquip");
				this._MovingEquip = value;
			}
		}
		/// <summary>
		/// 建筑
		/// </summary>
		[Field("Building")]
		[DataMember]
		public float? Building
		{
			get { return _Building; }
			set
			{
				this.OnPropertyValueChange("Building");
				this._Building = value;
			}
		}
		/// <summary>
		/// 结构
		/// </summary>
		[Field("Structure")]
		[DataMember]
		public float? Structure
		{
			get { return _Structure; }
			set
			{
				this.OnPropertyValueChange("Structure");
				this._Structure = value;
			}
		}
		/// <summary>
		/// 消防
		/// </summary>
		[Field("Fire")]
		[DataMember]
		public float? Fire
		{
			get { return _Fire; }
			set
			{
				this.OnPropertyValueChange("Fire");
				this._Fire = value;
			}
		}
		/// <summary>
		/// 暖通
		/// </summary>
		[Field("Hvac")]
		[DataMember]
		public float? Hvac
		{
			get { return _Hvac; }
			set
			{
				this.OnPropertyValueChange("Hvac");
				this._Hvac = value;
			}
		}
		/// <summary>
		/// 热工
		/// </summary>
		[Field("Thermal")]
		[DataMember]
		public float? Thermal
		{
			get { return _Thermal; }
			set
			{
				this.OnPropertyValueChange("Thermal");
				this._Thermal = value;
			}
		}
		/// <summary>
		/// 给排水
		/// </summary>
		[Field("Drainage")]
		[DataMember]
		public float? Drainage
		{
			get { return _Drainage; }
			set
			{
				this.OnPropertyValueChange("Drainage");
				this._Drainage = value;
			}
		}
		/// <summary>
		/// 总图运输
		/// </summary>
		[Field("Transport")]
		[DataMember]
		public float? Transport
		{
			get { return _Transport; }
			set
			{
				this.OnPropertyValueChange("Transport");
				this._Transport = value;
			}
		}
		/// <summary>
		/// 粉体
		/// </summary>
		[Field("Powder")]
		[DataMember]
		public float? Powder
		{
			get { return _Powder; }
			set
			{
				this.OnPropertyValueChange("Powder");
				this._Powder = value;
			}
		}
		/// <summary>
		/// 机修
		/// </summary>
		[Field("MachinePart")]
		[DataMember]
		public float? MachinePart
		{
			get { return _MachinePart; }
			set
			{
				this.OnPropertyValueChange("MachinePart");
				this._MachinePart = value;
			}
		}
		/// <summary>
		/// 概算
		/// </summary>
		[Field("Estimate")]
		[DataMember]
		public float? Estimate
		{
			get { return _Estimate; }
			set
			{
				this.OnPropertyValueChange("Estimate");
				this._Estimate = value;
			}
		}
		/// <summary>
		/// 合计
		/// </summary>
		[Field("Amount")]
		[DataMember]
		public float? Amount
		{
			get { return _Amount; }
			set
			{
				this.OnPropertyValueChange("Amount");
				this._Amount = value;
			}
		}
		/// <summary>
		/// 数据所属表名
		/// </summary>
		[Field("TableName")]
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
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
		[DataMember]
		public string CompanyName
		{
			get { return _CompanyName; }
			set
			{
				this.OnPropertyValueChange("CompanyName");
				this._CompanyName = value;
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
				_.ProjId,
				_.Sequ1,
				_.ProjName,
				_.ReportYear,
				_.ReportWeek,
				_.ReportMonth,
				_.Tag,
				_.ManageName,
				_.Process,
				_.Security,
				_.Technical,
				_.Environmental,
				_.Piping,
				_.Mill,
				_.Tube,
				_.Electrical,
				_.Instrument,
				_.Telecom,
				_.StaticEquip,
				_.MovingEquip,
				_.Building,
				_.Structure,
				_.Fire,
				_.Hvac,
				_.Thermal,
				_.Drainage,
				_.Transport,
				_.Powder,
				_.MachinePart,
				_.Estimate,
				_.Amount,
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
				this._ProjId,
				this._Sequ1,
				this._ProjName,
				this._ReportYear,
				this._ReportWeek,
				this._ReportMonth,
				this._Tag,
				this._ManageName,
				this._Process,
				this._Security,
				this._Technical,
				this._Environmental,
				this._Piping,
				this._Mill,
				this._Tube,
				this._Electrical,
				this._Instrument,
				this._Telecom,
				this._StaticEquip,
				this._MovingEquip,
				this._Building,
				this._Structure,
				this._Fire,
				this._Hvac,
				this._Thermal,
				this._Drainage,
				this._Transport,
				this._Powder,
				this._MachinePart,
				this._Estimate,
				this._Amount,
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
			public readonly static Field All = new Field("*", "NPS_DES_HumanTime");
			/// <summary>
			/// 主键
			/// </summary>
			public readonly static Field ID = new Field("ID", "NPS_DES_HumanTime", "主键");
			/// <summary>
			/// 编号
			/// </summary>
			public readonly static Field Code = new Field("Code", "NPS_DES_HumanTime", "编号");
			/// <summary>
			/// 标题
			/// </summary>
			public readonly static Field Title = new Field("Title", "NPS_DES_HumanTime", "标题");
			/// <summary>
			/// 工程号
			/// </summary>
			public readonly static Field ProjId = new Field("ProjId", "NPS_DES_HumanTime", "工程号");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Sequ1 = new Field("Sequ1", "NPS_DES_HumanTime", "");
			/// <summary>
			/// 项目名称
			/// </summary>
			public readonly static Field ProjName = new Field("ProjName", "NPS_DES_HumanTime", "项目名称");
			/// <summary>
			/// 报告年度
			/// </summary>
			public readonly static Field ReportYear = new Field("ReportYear", "NPS_DES_HumanTime", "报告年度");
			/// <summary>
			/// 报告周次
			/// </summary>
			public readonly static Field ReportWeek = new Field("ReportWeek", "NPS_DES_HumanTime", "报告周次");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ReportMonth = new Field("ReportMonth", "NPS_DES_HumanTime", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Tag = new Field("Tag", "NPS_DES_HumanTime", "");
			/// <summary>
			/// 项目经理
			/// </summary>
			public readonly static Field ManageName = new Field("ManageName", "NPS_DES_HumanTime", "项目经理");
			/// <summary>
			/// 工艺
			/// </summary>
			public readonly static Field Process = new Field("Process", "NPS_DES_HumanTime", "工艺");
			/// <summary>
			/// 安全
			/// </summary>
			public readonly static Field Security = new Field("Security", "NPS_DES_HumanTime", "安全");
			/// <summary>
			/// 技术经济
			/// </summary>
			public readonly static Field Technical = new Field("Technical", "NPS_DES_HumanTime", "技术经济");
			/// <summary>
			/// 环保
			/// </summary>
			public readonly static Field Environmental = new Field("Environmental", "NPS_DES_HumanTime", "环保");
			/// <summary>
			/// 管道
			/// </summary>
			public readonly static Field Piping = new Field("Piping", "NPS_DES_HumanTime", "管道");
			/// <summary>
			/// 管机
			/// </summary>
			public readonly static Field Mill = new Field("Mill", "NPS_DES_HumanTime", "管机");
			/// <summary>
			/// 管材
			/// </summary>
			public readonly static Field Tube = new Field("Tube", "NPS_DES_HumanTime", "管材");
			/// <summary>
			/// 电气
			/// </summary>
			public readonly static Field Electrical = new Field("Electrical", "NPS_DES_HumanTime", "电气");
			/// <summary>
			/// 仪表
			/// </summary>
			public readonly static Field Instrument = new Field("Instrument", "NPS_DES_HumanTime", "仪表");
			/// <summary>
			/// 电信
			/// </summary>
			public readonly static Field Telecom = new Field("Telecom", "NPS_DES_HumanTime", "电信");
			/// <summary>
			/// 静设备
			/// </summary>
			public readonly static Field StaticEquip = new Field("StaticEquip", "NPS_DES_HumanTime", "静设备");
			/// <summary>
			/// 动设备
			/// </summary>
			public readonly static Field MovingEquip = new Field("MovingEquip", "NPS_DES_HumanTime", "动设备");
			/// <summary>
			/// 建筑
			/// </summary>
			public readonly static Field Building = new Field("Building", "NPS_DES_HumanTime", "建筑");
			/// <summary>
			/// 结构
			/// </summary>
			public readonly static Field Structure = new Field("Structure", "NPS_DES_HumanTime", "结构");
			/// <summary>
			/// 消防
			/// </summary>
			public readonly static Field Fire = new Field("Fire", "NPS_DES_HumanTime", "消防");
			/// <summary>
			/// 暖通
			/// </summary>
			public readonly static Field Hvac = new Field("Hvac", "NPS_DES_HumanTime", "暖通");
			/// <summary>
			/// 热工
			/// </summary>
			public readonly static Field Thermal = new Field("Thermal", "NPS_DES_HumanTime", "热工");
			/// <summary>
			/// 给排水
			/// </summary>
			public readonly static Field Drainage = new Field("Drainage", "NPS_DES_HumanTime", "给排水");
			/// <summary>
			/// 总图运输
			/// </summary>
			public readonly static Field Transport = new Field("Transport", "NPS_DES_HumanTime", "总图运输");
			/// <summary>
			/// 粉体
			/// </summary>
			public readonly static Field Powder = new Field("Powder", "NPS_DES_HumanTime", "粉体");
			/// <summary>
			/// 机修
			/// </summary>
			public readonly static Field MachinePart = new Field("MachinePart", "NPS_DES_HumanTime", "机修");
			/// <summary>
			/// 概算
			/// </summary>
			public readonly static Field Estimate = new Field("Estimate", "NPS_DES_HumanTime", "概算");
			/// <summary>
			/// 合计
			/// </summary>
			public readonly static Field Amount = new Field("Amount", "NPS_DES_HumanTime", "合计");
			/// <summary>
			/// 数据所属表名
			/// </summary>
			public readonly static Field TableName = new Field("TableName", "NPS_DES_HumanTime", "数据所属表名");
			/// <summary>
			/// 数据录入业务域Id
			/// </summary>
			public readonly static Field BizAreaId = new Field("BizAreaId", "NPS_DES_HumanTime", "数据录入业务域Id");
			/// <summary>
			/// 序号
			/// </summary>
			public readonly static Field Sequ = new Field("Sequ", "NPS_DES_HumanTime", "序号");
			/// <summary>
			/// 表单状态
			/// </summary>
			public readonly static Field Status = new Field("Status", "NPS_DES_HumanTime", "表单状态");
			/// <summary>
			/// 录入人Id
			/// </summary>
			public readonly static Field RegHumId = new Field("RegHumId", "NPS_DES_HumanTime", "录入人Id");
			/// <summary>
			/// 录入人名称
			/// </summary>
			public readonly static Field RegHumName = new Field("RegHumName", "NPS_DES_HumanTime", "录入人名称");
			/// <summary>
			/// 录入日期
			/// </summary>
			public readonly static Field RegDate = new Field("RegDate", "NPS_DES_HumanTime", "录入日期");
			/// <summary>
			/// 录入人岗位Id
			/// </summary>
			public readonly static Field RegPosiId = new Field("RegPosiId", "NPS_DES_HumanTime", "录入人岗位Id");
			/// <summary>
			/// 录入人部门Id
			/// </summary>
			public readonly static Field RegDeptId = new Field("RegDeptId", "NPS_DES_HumanTime", "录入人部门Id");
			/// <summary>
			/// 记录所属EPS节点Id
			/// </summary>
			public readonly static Field EpsProjId = new Field("EpsProjId", "NPS_DES_HumanTime", "记录所属EPS节点Id");
			/// <summary>
			/// 删除人Id
			/// </summary>
			public readonly static Field RecycleHumId = new Field("RecycleHumId", "NPS_DES_HumanTime", "删除人Id");
			/// <summary>
			/// 最后更新人Id
			/// </summary>
			public readonly static Field UpdHumId = new Field("UpdHumId", "NPS_DES_HumanTime", "最后更新人Id");
			/// <summary>
			/// 最后更新人名称
			/// </summary>
			public readonly static Field UpdHumName = new Field("UpdHumName", "NPS_DES_HumanTime", "最后更新人名称");
			/// <summary>
			/// 最后更新日期
			/// </summary>
			public readonly static Field UpdDate = new Field("UpdDate", "NPS_DES_HumanTime", "最后更新日期");
			/// <summary>
			/// 批准人Id
			/// </summary>
			public readonly static Field ApprHumId = new Field("ApprHumId", "NPS_DES_HumanTime", "批准人Id");
			/// <summary>
			/// 批准人名称
			/// </summary>
			public readonly static Field ApprHumName = new Field("ApprHumName", "NPS_DES_HumanTime", "批准人名称");
			/// <summary>
			/// 批准日期
			/// </summary>
			public readonly static Field ApprDate = new Field("ApprDate", "NPS_DES_HumanTime", "批准日期");
			/// <summary>
			/// 备注
			/// </summary>
			public readonly static Field Remark = new Field("Remark", "NPS_DES_HumanTime", "备注");
			/// <summary>
			/// 所属项目Id
			/// </summary>
			public readonly static Field OwnProjId = new Field("OwnProjId", "NPS_DES_HumanTime", "所属项目Id");
			/// <summary>
			/// 管理层级名称
			/// </summary>
			public readonly static Field OwnProjName = new Field("OwnProjName", "NPS_DES_HumanTime", "管理层级名称");
			/// <summary>
			/// EPS编号
			/// </summary>
			public readonly static Field EpsProjCode = new Field("EpsProjCode", "NPS_DES_HumanTime", "EPS编号");
			/// <summary>
			/// EPS名称
			/// </summary>
			public readonly static Field EpsProjName = new Field("EpsProjName", "NPS_DES_HumanTime", "EPS名称");
			/// <summary>
			/// 单位ID
			/// </summary>
			public readonly static Field CompanyID = new Field("CompanyID", "NPS_DES_HumanTime", "单位ID");
			/// <summary>
			/// 单位编号
			/// </summary>
			public readonly static Field CompanyCode = new Field("CompanyCode", "NPS_DES_HumanTime", "单位编号");
			/// <summary>
			/// 单位名称
			/// </summary>
			public readonly static Field CompanyName = new Field("CompanyName", "NPS_DES_HumanTime", "单位名称");
		}
		#endregion
	}
}