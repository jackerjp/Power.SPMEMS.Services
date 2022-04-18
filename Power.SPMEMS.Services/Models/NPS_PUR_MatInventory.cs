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
	/// 实体类NPS_PUR_MatInventory。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("NPS_PUR_MatInventory")]
	[Serializable]
	public partial class NPS_PUR_MatInventory : Entity
	{
		#region Model
		private Guid _ID;
		private string _Code;
		private Guid? _Project_Guid;
		private string _Title;
		private string _FillingUnit;
		private DateTime? _FillingDate;
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
		private string _WareName;
		private string _MatCode;
		private string _UMatCode;
		private string _IDENT;
		private string _MatName;
		private string _MatDescribe;
		private string _Unit;
		private float? _Stock;
		private float? _ActStock;
		private float? _InvSurplus;
		private float? _InvLoss;
		private string _ClassCode;
		private string _Type;

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
		/// 项目Guid
		/// </summary>
		[Field("Project_Guid")]
		public Guid? Project_Guid
		{
			get { return _Project_Guid; }
			set
			{
				this.OnPropertyValueChange("Project_Guid");
				this._Project_Guid = value;
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
		/// 填报单位
		/// </summary>
		[Field("FillingUnit")]
		public string FillingUnit
		{
			get { return _FillingUnit; }
			set
			{
				this.OnPropertyValueChange("FillingUnit");
				this._FillingUnit = value;
			}
		}
		/// <summary>
		/// 填报时间
		/// </summary>
		[Field("FillingDate")]
		public DateTime? FillingDate
		{
			get { return _FillingDate; }
			set
			{
				this.OnPropertyValueChange("FillingDate");
				this._FillingDate = value;
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
		[Field("WareName")]
		public string WareName
		{
			get { return _WareName; }
			set
			{
				this.OnPropertyValueChange("WareName");
				this._WareName = value;
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
		[Field("IDENT")]
		public string IDENT
		{
			get { return _IDENT; }
			set
			{
				this.OnPropertyValueChange("IDENT");
				this._IDENT = value;
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
		[Field("MatDescribe")]
		public string MatDescribe
		{
			get { return _MatDescribe; }
			set
			{
				this.OnPropertyValueChange("MatDescribe");
				this._MatDescribe = value;
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
		[Field("Stock")]
		public float? Stock
		{
			get { return _Stock; }
			set
			{
				this.OnPropertyValueChange("Stock");
				this._Stock = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ActStock")]
		public float? ActStock
		{
			get { return _ActStock; }
			set
			{
				this.OnPropertyValueChange("ActStock");
				this._ActStock = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("InvSurplus")]
		public float? InvSurplus
		{
			get { return _InvSurplus; }
			set
			{
				this.OnPropertyValueChange("InvSurplus");
				this._InvSurplus = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("InvLoss")]
		public float? InvLoss
		{
			get { return _InvLoss; }
			set
			{
				this.OnPropertyValueChange("InvLoss");
				this._InvLoss = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ClassCode")]
		public string ClassCode
		{
			get { return _ClassCode; }
			set
			{
				this.OnPropertyValueChange("ClassCode");
				this._ClassCode = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("Type")]
		public string Type
		{
			get { return _Type; }
			set
			{
				this.OnPropertyValueChange("Type");
				this._Type = value;
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
				_.Project_Guid,
				_.Title,
				_.FillingUnit,
				_.FillingDate,
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
				_.WareName,
				_.MatCode,
				_.UMatCode,
				_.IDENT,
				_.MatName,
				_.MatDescribe,
				_.Unit,
				_.Stock,
				_.ActStock,
				_.InvSurplus,
				_.InvLoss,
				_.ClassCode,
				_.Type,
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
				this._Project_Guid,
				this._Title,
				this._FillingUnit,
				this._FillingDate,
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
				this._WareName,
				this._MatCode,
				this._UMatCode,
				this._IDENT,
				this._MatName,
				this._MatDescribe,
				this._Unit,
				this._Stock,
				this._ActStock,
				this._InvSurplus,
				this._InvLoss,
				this._ClassCode,
				this._Type,
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
			public readonly static Field All = new Field("*", "NPS_PUR_MatInventory");
			/// <summary>
			/// 主键
			/// </summary>
			public readonly static Field ID = new Field("ID", "NPS_PUR_MatInventory", "主键");
			/// <summary>
			/// 编号
			/// </summary>
			public readonly static Field Code = new Field("Code", "NPS_PUR_MatInventory", "编号");
			/// <summary>
			/// 项目Guid
			/// </summary>
			public readonly static Field Project_Guid = new Field("Project_Guid", "NPS_PUR_MatInventory", "项目Guid");
			/// <summary>
			/// 标题
			/// </summary>
			public readonly static Field Title = new Field("Title", "NPS_PUR_MatInventory", "标题");
			/// <summary>
			/// 填报单位
			/// </summary>
			public readonly static Field FillingUnit = new Field("FillingUnit", "NPS_PUR_MatInventory", "填报单位");
			/// <summary>
			/// 填报时间
			/// </summary>
			public readonly static Field FillingDate = new Field("FillingDate", "NPS_PUR_MatInventory", "填报时间");
			/// <summary>
			/// 数据所属表名
			/// </summary>
			public readonly static Field TableName = new Field("TableName", "NPS_PUR_MatInventory", "数据所属表名");
			/// <summary>
			/// 数据录入业务域Id
			/// </summary>
			public readonly static Field BizAreaId = new Field("BizAreaId", "NPS_PUR_MatInventory", "数据录入业务域Id");
			/// <summary>
			/// 序号
			/// </summary>
			public readonly static Field Sequ = new Field("Sequ", "NPS_PUR_MatInventory", "序号");
			/// <summary>
			/// 表单状态
			/// </summary>
			public readonly static Field Status = new Field("Status", "NPS_PUR_MatInventory", "表单状态");
			/// <summary>
			/// 录入人Id
			/// </summary>
			public readonly static Field RegHumId = new Field("RegHumId", "NPS_PUR_MatInventory", "录入人Id");
			/// <summary>
			/// 录入人名称
			/// </summary>
			public readonly static Field RegHumName = new Field("RegHumName", "NPS_PUR_MatInventory", "录入人名称");
			/// <summary>
			/// 录入日期
			/// </summary>
			public readonly static Field RegDate = new Field("RegDate", "NPS_PUR_MatInventory", "录入日期");
			/// <summary>
			/// 录入人岗位Id
			/// </summary>
			public readonly static Field RegPosiId = new Field("RegPosiId", "NPS_PUR_MatInventory", "录入人岗位Id");
			/// <summary>
			/// 录入人部门Id
			/// </summary>
			public readonly static Field RegDeptId = new Field("RegDeptId", "NPS_PUR_MatInventory", "录入人部门Id");
			/// <summary>
			/// 记录所属EPS节点Id
			/// </summary>
			public readonly static Field EpsProjId = new Field("EpsProjId", "NPS_PUR_MatInventory", "记录所属EPS节点Id");
			/// <summary>
			/// 删除人Id
			/// </summary>
			public readonly static Field RecycleHumId = new Field("RecycleHumId", "NPS_PUR_MatInventory", "删除人Id");
			/// <summary>
			/// 最后更新人Id
			/// </summary>
			public readonly static Field UpdHumId = new Field("UpdHumId", "NPS_PUR_MatInventory", "最后更新人Id");
			/// <summary>
			/// 最后更新人名称
			/// </summary>
			public readonly static Field UpdHumName = new Field("UpdHumName", "NPS_PUR_MatInventory", "最后更新人名称");
			/// <summary>
			/// 最后更新日期
			/// </summary>
			public readonly static Field UpdDate = new Field("UpdDate", "NPS_PUR_MatInventory", "最后更新日期");
			/// <summary>
			/// 批准人Id
			/// </summary>
			public readonly static Field ApprHumId = new Field("ApprHumId", "NPS_PUR_MatInventory", "批准人Id");
			/// <summary>
			/// 批准人名称
			/// </summary>
			public readonly static Field ApprHumName = new Field("ApprHumName", "NPS_PUR_MatInventory", "批准人名称");
			/// <summary>
			/// 批准日期
			/// </summary>
			public readonly static Field ApprDate = new Field("ApprDate", "NPS_PUR_MatInventory", "批准日期");
			/// <summary>
			/// 备注
			/// </summary>
			public readonly static Field Remark = new Field("Remark", "NPS_PUR_MatInventory", "备注");
			/// <summary>
			/// 所属项目Id
			/// </summary>
			public readonly static Field OwnProjId = new Field("OwnProjId", "NPS_PUR_MatInventory", "所属项目Id");
			/// <summary>
			/// 管理层级名称
			/// </summary>
			public readonly static Field OwnProjName = new Field("OwnProjName", "NPS_PUR_MatInventory", "管理层级名称");
			/// <summary>
			/// EPS编号
			/// </summary>
			public readonly static Field EpsProjCode = new Field("EpsProjCode", "NPS_PUR_MatInventory", "EPS编号");
			/// <summary>
			/// EPS名称
			/// </summary>
			public readonly static Field EpsProjName = new Field("EpsProjName", "NPS_PUR_MatInventory", "EPS名称");
			/// <summary>
			/// 单位ID
			/// </summary>
			public readonly static Field CompanyID = new Field("CompanyID", "NPS_PUR_MatInventory", "单位ID");
			/// <summary>
			/// 单位编号
			/// </summary>
			public readonly static Field CompanyCode = new Field("CompanyCode", "NPS_PUR_MatInventory", "单位编号");
			/// <summary>
			/// 单位名称
			/// </summary>
			public readonly static Field CompanyName = new Field("CompanyName", "NPS_PUR_MatInventory", "单位名称");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field WareName = new Field("WareName", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MatCode = new Field("MatCode", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field UMatCode = new Field("UMatCode", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field IDENT = new Field("IDENT", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MatName = new Field("MatName", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MatDescribe = new Field("MatDescribe", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Unit = new Field("Unit", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Stock = new Field("Stock", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ActStock = new Field("ActStock", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field InvSurplus = new Field("InvSurplus", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field InvLoss = new Field("InvLoss", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ClassCode = new Field("ClassCode", "NPS_PUR_MatInventory", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field Type = new Field("Type", "NPS_PUR_MatInventory", "");
		}
		#endregion
	}
}