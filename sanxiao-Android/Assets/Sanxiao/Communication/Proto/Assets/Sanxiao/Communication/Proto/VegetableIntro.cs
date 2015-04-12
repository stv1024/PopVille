using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 蔬菜的介绍。
	/// </summary>
	public class VegetableIntro : ISendable, IReceiveable
	{
		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
		/// 蔬菜的id。
		/// </summary>
		public int VegetableCode
		{
			get
			{
				return vegetableCode;
			}
			set
			{
				HasVegetableCode = true;
				vegetableCode = value;
			}
		}

		private bool HasIntroContent{get;set;}
		private string introContent;
		/// <summary>
		/// 介绍文本内容。
		/// </summary>
		public string IntroContent
		{
			get
			{
				return introContent;
			}
			set
			{
				if(value != null)
				{
					HasIntroContent = true;
					introContent = value;
				}
			}
		}

		/// <summary>
		/// 蔬菜的介绍。
		/// </summary>
		public VegetableIntro()
		{
		}

		/// <summary>
		/// 蔬菜的介绍。
		/// </summary>
		public VegetableIntro
		(
			int vegetableCode,
			string introContent
		):this()
		{
			VegetableCode = vegetableCode;
			IntroContent = introContent;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,VegetableCode);
			writer.Write(2,IntroContent);
			return writer.GetProtoBufferBytes();
		}
		public void ParseFrom(byte[] buffer)
		{
			 ParseFrom(buffer, 0, buffer.Length);
		}
		public void ParseFrom(byte[] buffer, int offset, int size)
		{
			if (buffer == null) return;
			ProtoBufferReader reader = new ProtoBufferReader(buffer,offset,size);
			foreach (ProtoBufferObject obj in reader.ProtoBufferObjs)
			{
				switch (obj.FieldNumber)
				{
					case 1:
						VegetableCode = obj.Value;
						break;
					case 2:
						IntroContent = obj.Value;
						break;
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("VegetableCode : " + VegetableCode + ",");
			sb.Append("IntroContent : " + IntroContent);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
