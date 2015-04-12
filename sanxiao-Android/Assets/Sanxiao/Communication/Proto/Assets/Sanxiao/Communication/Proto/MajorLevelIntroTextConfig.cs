using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 大关卡介绍文本配置。
	/// </summary>
	public class MajorLevelIntroTextConfig : ISendable, IReceiveable
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

		private List<MajorLevelIntro> introList;
		/// <summary>
		/// </summary>
		public List<MajorLevelIntro> IntroList
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
		/// 大关卡介绍文本配置。
		/// </summary>
		public MajorLevelIntroTextConfig()
		{
			IntroList = new List<MajorLevelIntro>();
		}

		/// <summary>
		/// 大关卡介绍文本配置。
		/// </summary>
		public MajorLevelIntroTextConfig
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
			foreach(MajorLevelIntro v in IntroList)
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
						 var intro= new MajorLevelIntro();
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
