//------------------------------------------------------------------------------
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
	/// 实体类DCWTRECLIN。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("DCWTRECLIN")]
	[Serializable]
	[DataContract]
	public partial class DCWTRECLIN : Entity
	{
		#region Model
		private int _RECL_NO;
		private int? _REC_NO;
		private string _WTLX_NO;
		private string _PROJECT_NAM;
		private string _PROJECT_NO;
		private int? _KGREP_NO;
		private string _WBS_ID;
		private int? _W1;
		private int? _W2;
		private int? _W3;
		private int? _W4;
		private int? _W5;
		private int? _SUMWT;
		private int? _JBWT;
		private int? _SKL_NO;
		private int? _WPLACE;
		private int? _W6;
		private int? _W7;
		private int? _J1;
		private int? _J2;
		private int? _J3;
		private int? _J4;
		private int? _J5;
		private int? _J6;
		private int? _J7;
		private string _DHGS_STA;

		/// <summary>
		/// 
		/// </summary>
		[Field("RECL_NO")]
		[DataMember]
		public int RECL_NO
		{
			get { return _RECL_NO; }
			set
			{
				this.OnPropertyValueChange("RECL_NO");
				this._RECL_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("REC_NO")]
		[DataMember]
		public int? REC_NO
		{
			get { return _REC_NO; }
			set
			{
				this.OnPropertyValueChange("REC_NO");
				this._REC_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("WTLX_NO")]
		[DataMember]
		public string WTLX_NO
		{
			get { return _WTLX_NO; }
			set
			{
				this.OnPropertyValueChange("WTLX_NO");
				this._WTLX_NO = value;
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
		[Field("PROJECT_NO")]
		[DataMember]
		public string PROJECT_NO
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
		[Field("KGREP_NO")]
		[DataMember]
		public int? KGREP_NO
		{
			get { return _KGREP_NO; }
			set
			{
				this.OnPropertyValueChange("KGREP_NO");
				this._KGREP_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("WBS_ID")]
		[DataMember]
		public string WBS_ID
		{
			get { return _WBS_ID; }
			set
			{
				this.OnPropertyValueChange("WBS_ID");
				this._WBS_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W1")]
		[DataMember]
		public int? W1
		{
			get { return _W1; }
			set
			{
				this.OnPropertyValueChange("W1");
				this._W1 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W2")]
		[DataMember]
		public int? W2
		{
			get { return _W2; }
			set
			{
				this.OnPropertyValueChange("W2");
				this._W2 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W3")]
		[DataMember]
		public int? W3
		{
			get { return _W3; }
			set
			{
				this.OnPropertyValueChange("W3");
				this._W3 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W4")]
		[DataMember]
		public int? W4
		{
			get { return _W4; }
			set
			{
				this.OnPropertyValueChange("W4");
				this._W4 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W5")]
		[DataMember]
		public int? W5
		{
			get { return _W5; }
			set
			{
				this.OnPropertyValueChange("W5");
				this._W5 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SUMWT")]
		[DataMember]
		public int? SUMWT
		{
			get { return _SUMWT; }
			set
			{
				this.OnPropertyValueChange("SUMWT");
				this._SUMWT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("JBWT")]
		[DataMember]
		public int? JBWT
		{
			get { return _JBWT; }
			set
			{
				this.OnPropertyValueChange("JBWT");
				this._JBWT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("SKL_NO")]
		[DataMember]
		public int? SKL_NO
		{
			get { return _SKL_NO; }
			set
			{
				this.OnPropertyValueChange("SKL_NO");
				this._SKL_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("WPLACE")]
		[DataMember]
		public int? WPLACE
		{
			get { return _WPLACE; }
			set
			{
				this.OnPropertyValueChange("WPLACE");
				this._WPLACE = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W6")]
		[DataMember]
		public int? W6
		{
			get { return _W6; }
			set
			{
				this.OnPropertyValueChange("W6");
				this._W6 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("W7")]
		[DataMember]
		public int? W7
		{
			get { return _W7; }
			set
			{
				this.OnPropertyValueChange("W7");
				this._W7 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J1")]
		[DataMember]
		public int? J1
		{
			get { return _J1; }
			set
			{
				this.OnPropertyValueChange("J1");
				this._J1 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J2")]
		[DataMember]
		public int? J2
		{
			get { return _J2; }
			set
			{
				this.OnPropertyValueChange("J2");
				this._J2 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J3")]
		[DataMember]
		public int? J3
		{
			get { return _J3; }
			set
			{
				this.OnPropertyValueChange("J3");
				this._J3 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J4")]
		[DataMember]
		public int? J4
		{
			get { return _J4; }
			set
			{
				this.OnPropertyValueChange("J4");
				this._J4 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J5")]
		[DataMember]
		public int? J5
		{
			get { return _J5; }
			set
			{
				this.OnPropertyValueChange("J5");
				this._J5 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J6")]
		[DataMember]
		public int? J6
		{
			get { return _J6; }
			set
			{
				this.OnPropertyValueChange("J6");
				this._J6 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("J7")]
		[DataMember]
		public int? J7
		{
			get { return _J7; }
			set
			{
				this.OnPropertyValueChange("J7");
				this._J7 = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("DHGS_STA")]
		[DataMember]
		public string DHGS_STA
		{
			get { return _DHGS_STA; }
			set
			{
				this.OnPropertyValueChange("DHGS_STA");
				this._DHGS_STA = value;
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
				_.RECL_NO,
			};
		}
		/// <summary>
		/// 获取列信息
		/// </summary>
		public override Field[] GetFields()
		{
			return new Field[] {
				_.RECL_NO,
				_.REC_NO,
				_.WTLX_NO,
				_.PROJECT_NAM,
				_.PROJECT_NO,
				_.KGREP_NO,
				_.WBS_ID,
				_.W1,
				_.W2,
				_.W3,
				_.W4,
				_.W5,
				_.SUMWT,
				_.JBWT,
				_.SKL_NO,
				_.WPLACE,
				_.W6,
				_.W7,
				_.J1,
				_.J2,
				_.J3,
				_.J4,
				_.J5,
				_.J6,
				_.J7,
				_.DHGS_STA,
			};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._RECL_NO,
				this._REC_NO,
				this._WTLX_NO,
				this._PROJECT_NAM,
				this._PROJECT_NO,
				this._KGREP_NO,
				this._WBS_ID,
				this._W1,
				this._W2,
				this._W3,
				this._W4,
				this._W5,
				this._SUMWT,
				this._JBWT,
				this._SKL_NO,
				this._WPLACE,
				this._W6,
				this._W7,
				this._J1,
				this._J2,
				this._J3,
				this._J4,
				this._J5,
				this._J6,
				this._J7,
				this._DHGS_STA,
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
			public readonly static Field All = new Field("*", "DCWTRECLIN");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field RECL_NO = new Field("RECL_NO", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field REC_NO = new Field("REC_NO", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field WTLX_NO = new Field("WTLX_NO", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PROJECT_NAM = new Field("PROJECT_NAM", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field PROJECT_NO = new Field("PROJECT_NO", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field KGREP_NO = new Field("KGREP_NO", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field WBS_ID = new Field("WBS_ID", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W1 = new Field("W1", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W2 = new Field("W2", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W3 = new Field("W3", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W4 = new Field("W4", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W5 = new Field("W5", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SUMWT = new Field("SUMWT", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field JBWT = new Field("JBWT", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field SKL_NO = new Field("SKL_NO", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field WPLACE = new Field("WPLACE", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W6 = new Field("W6", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field W7 = new Field("W7", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J1 = new Field("J1", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J2 = new Field("J2", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J3 = new Field("J3", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J4 = new Field("J4", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J5 = new Field("J5", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J6 = new Field("J6", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field J7 = new Field("J7", "DCWTRECLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field DHGS_STA = new Field("DHGS_STA", "DCWTRECLIN", "");
		}
		#endregion
	}
}