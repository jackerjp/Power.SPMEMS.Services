//------------------------------------------------------------------------------
// <auto-generated>
//     �˴����ɹ������ɡ�
//     ����ʱ�汾:4.0.30319.42000
//     Website: http://ITdos.com/Dos/ORM/Index.html
//     �Դ��ļ��ĸ��Ŀ��ܻᵼ�²���ȷ����Ϊ���������
//     �������ɴ��룬��Щ���Ľ��ᶪʧ��
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using Dos.ORM;

namespace Power.SPMEMS.Services.Models
{
    /// <summary>
    /// ʵ����DCWTTYPMST��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Table("DCWTTYPMST")]
    [Serializable]
	[DataContract]
    public partial class DCWTTYPMST : Entity
    {
        #region Model
		private int _TYP_NO;
		private int? _ORG_NO;
		private string _TYP_ID;
		private string _TYP_NAM;
		private string _DZ_ID;
		private string _PROFLAG;
		private string _VALID_STA;
		private string _TYP_NOT;
		private string _FSTUSR_ID;
		private DateTime? _FSTUSR_DTM;
		private string _LSTUSR_ID;
		private DateTime? _LSTUSR_DTM;
		private string _UP_TYP;

		/// <summary>
		/// 
		/// </summary>
		[Field("TYP_NO")]
		[DataMember]
		public int TYP_NO
		{
			get{ return _TYP_NO; }
			set
			{
				this.OnPropertyValueChange("TYP_NO");
				this._TYP_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("ORG_NO")]
		[DataMember]
		public int? ORG_NO
		{
			get{ return _ORG_NO; }
			set
			{
				this.OnPropertyValueChange("ORG_NO");
				this._ORG_NO = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TYP_ID")]
		[DataMember]
		public string TYP_ID
		{
			get{ return _TYP_ID; }
			set
			{
				this.OnPropertyValueChange("TYP_ID");
				this._TYP_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TYP_NAM")]
		[DataMember]
		public string TYP_NAM
		{
			get{ return _TYP_NAM; }
			set
			{
				this.OnPropertyValueChange("TYP_NAM");
				this._TYP_NAM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("DZ_ID")]
		[DataMember]
		public string DZ_ID
		{
			get{ return _DZ_ID; }
			set
			{
				this.OnPropertyValueChange("DZ_ID");
				this._DZ_ID = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("PROFLAG")]
		[DataMember]
		public string PROFLAG
		{
			get{ return _PROFLAG; }
			set
			{
				this.OnPropertyValueChange("PROFLAG");
				this._PROFLAG = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("VALID_STA")]
		[DataMember]
		public string VALID_STA
		{
			get{ return _VALID_STA; }
			set
			{
				this.OnPropertyValueChange("VALID_STA");
				this._VALID_STA = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("TYP_NOT")]
		[DataMember]
		public string TYP_NOT
		{
			get{ return _TYP_NOT; }
			set
			{
				this.OnPropertyValueChange("TYP_NOT");
				this._TYP_NOT = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("FSTUSR_ID")]
		[DataMember]
		public string FSTUSR_ID
		{
			get{ return _FSTUSR_ID; }
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
			get{ return _FSTUSR_DTM; }
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
			get{ return _LSTUSR_ID; }
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
			get{ return _LSTUSR_DTM; }
			set
			{
				this.OnPropertyValueChange("LSTUSR_DTM");
				this._LSTUSR_DTM = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[Field("UP_TYP")]
		[DataMember]
		public string UP_TYP
		{
			get{ return _UP_TYP; }
			set
			{
				this.OnPropertyValueChange("UP_TYP");
				this._UP_TYP = value;
			}
		}
		#endregion

		#region Method
        /// <summary>
        /// ��ȡʵ���е�������
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
				_.TYP_NO,
			};
        }
        /// <summary>
        /// ��ȡ����Ϣ
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.TYP_NO,
				_.ORG_NO,
				_.TYP_ID,
				_.TYP_NAM,
				_.DZ_ID,
				_.PROFLAG,
				_.VALID_STA,
				_.TYP_NOT,
				_.FSTUSR_ID,
				_.FSTUSR_DTM,
				_.LSTUSR_ID,
				_.LSTUSR_DTM,
				_.UP_TYP,
			};
        }
        /// <summary>
        /// ��ȡֵ��Ϣ
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._TYP_NO,
				this._ORG_NO,
				this._TYP_ID,
				this._TYP_NAM,
				this._DZ_ID,
				this._PROFLAG,
				this._VALID_STA,
				this._TYP_NOT,
				this._FSTUSR_ID,
				this._FSTUSR_DTM,
				this._LSTUSR_ID,
				this._LSTUSR_DTM,
				this._UP_TYP,
			};
        }
        /// <summary>
        /// �Ƿ���v1.10.5.6�����ϰ汾ʵ�塣
        /// </summary>
        /// <returns></returns>
        public override bool V1_10_5_6_Plus()
        {
            return true;
        }
        #endregion

		#region _Field
        /// <summary>
        /// �ֶ���Ϣ
        /// </summary>
        public class _
        {
			/// <summary>
			/// * 
			/// </summary>
			public readonly static Field All = new Field("*", "DCWTTYPMST");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field TYP_NO = new Field("TYP_NO", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field ORG_NO = new Field("ORG_NO", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field TYP_ID = new Field("TYP_ID", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field TYP_NAM = new Field("TYP_NAM", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field DZ_ID = new Field("DZ_ID", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field PROFLAG = new Field("PROFLAG", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field VALID_STA = new Field("VALID_STA", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field TYP_NOT = new Field("TYP_NOT", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field FSTUSR_ID = new Field("FSTUSR_ID", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field FSTUSR_DTM = new Field("FSTUSR_DTM", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field LSTUSR_ID = new Field("LSTUSR_ID", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field LSTUSR_DTM = new Field("LSTUSR_DTM", "DCWTTYPMST", "");
            /// <summary>
			/// 
			/// </summary>
			public readonly static Field UP_TYP = new Field("UP_TYP", "DCWTTYPMST", "");
        }
        #endregion
	}
}