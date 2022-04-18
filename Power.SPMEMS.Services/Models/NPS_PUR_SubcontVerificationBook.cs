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
	/// 实体类NPS_PUR_SubcontVerificationBook。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("NPS_PUR_SubcontVerificationBook")]
	[Serializable]
	public partial class NPS_PUR_SubcontVerificationBook : Entity
	{
		#region Model
		private Guid _ID;
		private string _Code;
		private string _Title;
		private string _SubcontName;
		private string _SubItemName;
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
		private string _UMatCode;
		private string _Ident;
		private string _MatCode;
		private string _MatName;
		private string _MatSpec;
		private string _Unit;
		private float? _Demand_Sum;
		private float? _Demand_Demand;
		private float? _Demand_Change;
		private float? _CollectNum;
		private float? _OverNum;
		private string _Analysis;
		private string _ProfName;
		private string _Classify;
		private string _Device;
		private string _Element;

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
		/// 分承包商名称
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
		/// 分部分项名称
		/// </summary>
		[Field("SubItemName")]
		public string SubItemName
		{
			get { return _SubItemName; }
			set
			{
				this.OnPropertyValueChange("SubItemName");
				this._SubItemName = value;
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
		/// 
		/// </summary>
		[Field("UMatCode")]
		public string UMatCode
		{
			get { return _UMatCode; }
			set
			{
				this.OnPropertyValueChange("UMatCode");
				this._UMatCode = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Ident")]
		public string Ident
		{
			get { return _Ident; }
			set
			{
				this.OnPropertyValueChange("Ident");
				this._Ident = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("MatCode")]
		public string MatCode
		{
			get { return _MatCode; }
			set
			{
				this.OnPropertyValueChange("MatCode");
				this._MatCode = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("MatName")]
		public string MatName
		{
			get { return _MatName; }
			set
			{
				this.OnPropertyValueChange("MatName");
				this._MatName = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("MatSpec")]
		public string MatSpec
		{
			get { return _MatSpec; }
			set
			{
				this.OnPropertyValueChange("MatSpec");
				this._MatSpec = value;
			}
		}
		/// <summary>
		/// 
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
		/// 
		/// </summary>
		[Field("Demand_Sum")]
		public float? Demand_Sum
		{
			get { return _Demand_Sum; }
			set
			{
				this.OnPropertyValueChange("Demand_Sum");
				this._Demand_Sum = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Demand_Demand")]
		public float? Demand_Demand
		{
			get { return _Demand_Demand; }
			set
			{
				this.OnPropertyValueChange("Demand_Demand");
				this._Demand_Demand = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Demand_Change")]
		public float? Demand_Change
		{
			get { return _Demand_Change; }
			set
			{
				this.OnPropertyValueChange("Demand_Change");
				this._Demand_Change = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("CollectNum")]
		public float? CollectNum
		{
			get { return _CollectNum; }
			set
			{
				this.OnPropertyValueChange("CollectNum");
				this._CollectNum = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("OverNum")]
		public float? OverNum
		{
			get { return _OverNum; }
			set
			{
				this.OnPropertyValueChange("OverNum");
				this._OverNum = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Analysis")]
		public string Analysis
		{
			get { return _Analysis; }
			set
			{
				this.OnPropertyValueChange("Analysis");
				this._Analysis = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ProfName")]
		public string ProfName
		{
			get { return _ProfName; }
			set
			{
				this.OnPropertyValueChange("ProfName");
				this._ProfName = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Classify")]
		public string Classify
		{
			get { return _Classify; }
			set
			{
				this.OnPropertyValueChange("Classify");
				this._Classify = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Device")]
		public string Device
		{
			get { return _Device; }
			set
			{
				this.OnPropertyValueChange("Device");
				this._Device = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Element")]
		public string Element
		{
			get { return _Element; }
			set
			{
				this.OnPropertyValueChange("Element");
				this._Element = value;
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
				_.SubcontName,
				_.SubItemName,
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
				_.UMatCode,
				_.Ident,
				_.MatCode,
				_.MatName,
				_.MatSpec,
				_.Unit,
				_.Demand_Sum,
				_.Demand_Demand,
				_.Demand_Change,
				_.CollectNum,
				_.OverNum,
				_.Analysis,
				_.ProfName,
				_.Classify,
				_.Device,
				_.Element,
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
				this._SubcontName,
				this._SubItemName,
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
				this._UMatCode,
				this._Ident,
				this._MatCode,
				this._MatName,
				this._MatSpec,
				this._Unit,
				this._Demand_Sum,
				this._Demand_Demand,
				this._Demand_Change,
				this._CollectNum,
				this._OverNum,
				this._Analysis,
				this._ProfName,
				this._Classify,
				this._Device,
				this._Element,
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
			public readonly static Field All = new Field("*", "NPS_PUR_SubcontVerificationBook");
			/// <summary>
			/// 主键
			/// </summary>
			public readonly static Field ID = new Field("ID", "NPS_PUR_SubcontVerificationBook", "主键");
			/// <summary>
			/// 编号
			/// </summary>
			public readonly static Field Code = new Field("Code", "NPS_PUR_SubcontVerificationBook", "编号");
			/// <summary>
			/// 标题
			/// </summary>
			public readonly static Field Title = new Field("Title", "NPS_PUR_SubcontVerificationBook", "标题");
			/// <summary>
			/// 分承包商名称
			/// </summary>
			public readonly static Field SubcontName = new Field("SubcontName", "NPS_PUR_SubcontVerificationBook", "分承包商名称");
			/// <summary>
			/// 分部分项名称
			/// </summary>
			public readonly static Field SubItemName = new Field("SubItemName", "NPS_PUR_SubcontVerificationBook", "分部分项名称");
			/// <summary>
			/// 数据所属表名
			/// </summary>
			public readonly static Field TableName = new Field("TableName", "NPS_PUR_SubcontVerificationBook", "数据所属表名");
			/// <summary>
			/// 数据录入业务域Id
			/// </summary>
			public readonly static Field BizAreaId = new Field("BizAreaId", "NPS_PUR_SubcontVerificationBook", "数据录入业务域Id");
			/// <summary>
			/// 序号
			/// </summary>
			public readonly static Field Sequ = new Field("Sequ", "NPS_PUR_SubcontVerificationBook", "序号");
			/// <summary>
			/// 表单状态
			/// </summary>
			public readonly static Field Status = new Field("Status", "NPS_PUR_SubcontVerificationBook", "表单状态");
			/// <summary>
			/// 录入人Id
			/// </summary>
			public readonly static Field RegHumId = new Field("RegHumId", "NPS_PUR_SubcontVerificationBook", "录入人Id");
			/// <summary>
			/// 录入人名称
			/// </summary>
			public readonly static Field RegHumName = new Field("RegHumName", "NPS_PUR_SubcontVerificationBook", "录入人名称");
			/// <summary>
			/// 录入日期
			/// </summary>
			public readonly static Field RegDate = new Field("RegDate", "NPS_PUR_SubcontVerificationBook", "录入日期");
			/// <summary>
			/// 录入人岗位Id
			/// </summary>
			public readonly static Field RegPosiId = new Field("RegPosiId", "NPS_PUR_SubcontVerificationBook", "录入人岗位Id");
			/// <summary>
			/// 录入人部门Id
			/// </summary>
			public readonly static Field RegDeptId = new Field("RegDeptId", "NPS_PUR_SubcontVerificationBook", "录入人部门Id");
			/// <summary>
			/// 记录所属EPS节点Id
			/// </summary>
			public readonly static Field EpsProjId = new Field("EpsProjId", "NPS_PUR_SubcontVerificationBook", "记录所属EPS节点Id");
			/// <summary>
			/// 删除人Id
			/// </summary>
			public readonly static Field RecycleHumId = new Field("RecycleHumId", "NPS_PUR_SubcontVerificationBook", "删除人Id");
			/// <summary>
			/// 最后更新人Id
			/// </summary>
			public readonly static Field UpdHumId = new Field("UpdHumId", "NPS_PUR_SubcontVerificationBook", "最后更新人Id");
			/// <summary>
			/// 最后更新人名称
			/// </summary>
			public readonly static Field UpdHumName = new Field("UpdHumName", "NPS_PUR_SubcontVerificationBook", "最后更新人名称");
			/// <summary>
			/// 最后更新日期
			/// </summary>
			public readonly static Field UpdDate = new Field("UpdDate", "NPS_PUR_SubcontVerificationBook", "最后更新日期");
			/// <summary>
			/// 批准人Id
			/// </summary>
			public readonly static Field ApprHumId = new Field("ApprHumId", "NPS_PUR_SubcontVerificationBook", "批准人Id");
			/// <summary>
			/// 批准人名称
			/// </summary>
			public readonly static Field ApprHumName = new Field("ApprHumName", "NPS_PUR_SubcontVerificationBook", "批准人名称");
			/// <summary>
			/// 批准日期
			/// </summary>
			public readonly static Field ApprDate = new Field("ApprDate", "NPS_PUR_SubcontVerificationBook", "批准日期");
			/// <summary>
			/// 备注
			/// </summary>
			public readonly static Field Remark = new Field("Remark", "NPS_PUR_SubcontVerificationBook", "备注");
			/// <summary>
			/// 所属项目Id
			/// </summary>
			public readonly static Field OwnProjId = new Field("OwnProjId", "NPS_PUR_SubcontVerificationBook", "所属项目Id");
			/// <summary>
			/// 管理层级名称
			/// </summary>
			public readonly static Field OwnProjName = new Field("OwnProjName", "NPS_PUR_SubcontVerificationBook", "管理层级名称");
			/// <summary>
			/// EPS编号
			/// </summary>
			public readonly static Field EpsProjCode = new Field("EpsProjCode", "NPS_PUR_SubcontVerificationBook", "EPS编号");
			/// <summary>
			/// EPS名称
			/// </summary>
			public readonly static Field EpsProjName = new Field("EpsProjName", "NPS_PUR_SubcontVerificationBook", "EPS名称");
			/// <summary>
			/// 单位ID
			/// </summary>
			public readonly static Field CompanyID = new Field("CompanyID", "NPS_PUR_SubcontVerificationBook", "单位ID");
			/// <summary>
			/// 单位编号
			/// </summary>
			public readonly static Field CompanyCode = new Field("CompanyCode", "NPS_PUR_SubcontVerificationBook", "单位编号");
			/// <summary>
			/// 单位名称
			/// </summary>
			public readonly static Field CompanyName = new Field("CompanyName", "NPS_PUR_SubcontVerificationBook", "单位名称");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field UMatCode = new Field("UMatCode", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Ident = new Field("Ident", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MatCode = new Field("MatCode", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MatName = new Field("MatName", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MatSpec = new Field("MatSpec", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Unit = new Field("Unit", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Demand_Sum = new Field("Demand_Sum", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Demand_Demand = new Field("Demand_Demand", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Demand_Change = new Field("Demand_Change", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field CollectNum = new Field("CollectNum", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field OverNum = new Field("OverNum", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Analysis = new Field("Analysis", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ProfName = new Field("ProfName", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Classify = new Field("Classify", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Device = new Field("Device", "NPS_PUR_SubcontVerificationBook", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Element = new Field("Element", "NPS_PUR_SubcontVerificationBook", "");
		}
		#endregion
	}
}