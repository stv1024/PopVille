using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 蔬菜介绍文本。
	/// </summary>
	public class VegetableIntroTextConfig : ISendable, IReceiveable
	{
		private bool HasHash{get;set;}
		private string hash;
		/// <summary>
		/// </summary>
		public string Hash
		{
			get
			{
				return hash;
			}
			set
			{
				if(value != null)
				{
					HasHash = true;
					hash = value;
				}
			}
		}

		private List<VegetableIntro> introList;
		/// <summary>
		/// 蔬菜的介绍。
		/// </summary>
		public List<VegetableIntro> IntroList
		{
			get
			{
				return introList;
			}
			set
			{
				if(value != null)
				{
					introList = value;
				}
			}
		}

		/// <summary>
		/// 蔬菜介绍文本。
		/// </summary>
		public VegetableIntroTextConfig()
		{
			IntroList = new List<VegetableIntro>();
		}

		/// <summary>
		/// 蔬菜介绍文本。
		/// </summary>
		public VegetableIntroTextConfig
		(
			string hash
		):this()
		{
			Hash = hash;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Hash);
			foreach(VegetableIntro v in IntroList)
			{
				writer.Write(2,v);
			}
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
						Hash = obj.Value;
						break;
					case 2:
						 var intro= new VegetableIntro();
						intro.ParseFrom(obj.Value);
						IntroList.Add(intro);
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
			sb.Append("Hash : " + Hash + ",");
			sb.Append("IntroList : [");
			for(int i = 0; i < IntroList.Count;i ++)
			{
				if(i == IntroList.Count -1)
				{
					sb.Append(IntroList[i]);
				}else
				{
					sb.Append(IntroList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
