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
	/// 实体类DCWTDURLIN。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Table("DCWTDURLIN")]
	[Serializable]
	[DataContract]
	public partial class DCWTDURLIN : Entity
	{
		#region Model
		private int _DURL_NO;
		private int? _DUR_NO;
		private int? _MONTHSEQ;
		private int? _WEEKSEQ;
		private DateTime? _BEGDTM;
		private DateTime? _ENDDTM;
		private int? _FDNUM;
		private int? _FDTIME;
		private string _DURL_NOT;

		/// <summary>
		/// 
		/// </summary>
		[Field("DURL_NO")]
		[DataMember]
		public int DURL_NO
		{
			get { return _DURL_NO; }
			set
			{
				this.OnPropertyValueChange("DURL_NO");
				this._DURL_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("DUR_NO")]
		[DataMember]
		public int? DUR_NO
		{
			get { return _DUR_NO; }
			set
			{
				this.OnPropertyValueChange("DUR_NO");
				this._DUR_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("MONTHSEQ")]
		[DataMember]
		public int? MONTHSEQ
		{
			get { return _MONTHSEQ; }
			set
			{
				this.OnPropertyValueChange("MONTHSEQ");
				this._MONTHSEQ = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("WEEKSEQ")]
		[DataMember]
		public int? WEEKSEQ
		{
			get { return _WEEKSEQ; }
			set
			{
				this.OnPropertyValueChange("WEEKSEQ");
				this._WEEKSEQ = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("BEGDTM")]
		[DataMember]
		public DateTime? BEGDTM
		{
			get { return _BEGDTM; }
			set
			{
				this.OnPropertyValueChange("BEGDTM");
				this._BEGDTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ENDDTM")]
		[DataMember]
		public DateTime? ENDDTM
		{
			get { return _ENDDTM; }
			set
			{
				this.OnPropertyValueChange("ENDDTM");
				this._ENDDTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("FDNUM")]
		[DataMember]
		public int? FDNUM
		{
			get { return _FDNUM; }
			set
			{
				this.OnPropertyValueChange("FDNUM");
				this._FDNUM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("FDTIME")]
		[DataMember]
		public int? FDTIME
		{
			get { return _FDTIME; }
			set
			{
				this.OnPropertyValueChange("FDTIME");
				this._FDTIME = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("DURL_NOT")]
		[DataMember]
		public string DURL_NOT
		{
			get { return _DURL_NOT; }
			set
			{
				this.OnPropertyValueChange("DURL_NOT");
				this._DURL_NOT = value;
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
				_.DURL_NO,
			};
		}
		/// <summary>
		/// 获取列信息
		/// </summary>
		public override Field[] GetFields()
		{
			return new Field[] {
				_.DURL_NO,
				_.DUR_NO,
				_.MONTHSEQ,
				_.WEEKSEQ,
				_.BEGDTM,
				_.ENDDTM,
				_.FDNUM,
				_.FDTIME,
				_.DURL_NOT,
			};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._DURL_NO,
				this._DUR_NO,
				this._MONTHSEQ,
				this._WEEKSEQ,
				this._BEGDTM,
				this._ENDDTM,
				this._FDNUM,
				this._FDTIME,
				this._DURL_NOT,
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
			public readonly static Field All = new Field("*", "DCWTDURLIN");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field DURL_NO = new Field("DURL_NO", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field DUR_NO = new Field("DUR_NO", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field MONTHSEQ = new Field("MONTHSEQ", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field WEEKSEQ = new Field("WEEKSEQ", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field BEGDTM = new Field("BEGDTM", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field ENDDTM = new Field("ENDDTM", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field FDNUM = new Field("FDNUM", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field FDTIME = new Field("FDTIME", "DCWTDURLIN", "");
			/// <summary>
			/// 
			/// </summary>
			public readonly static Field DURL_NOT = new Field("DURL_NOT", "DCWTDURLIN", "");
		}
		#endregion
	}
}