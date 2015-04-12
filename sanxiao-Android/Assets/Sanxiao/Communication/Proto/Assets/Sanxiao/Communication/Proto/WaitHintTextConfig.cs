using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 等待提示的文本配置。
	/// </summary>
	public class WaitHintTextConfig : ISendable, IReceiveable
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

		private List<string> hintList;
		/// <summary>
		/// 等待提示的列表。
		/// </summary>
		public List<string> HintList
		{
			get
			{
				return hintList;
			}
			set
			{
				if(value != null)
				{
					hintList = value;
				}
			}
		}

		/// <summary>
		/// 等待提示的文本配置。
		/// </summary>
		public WaitHintTextConfig()
		{
			HintList = new List<string>();
		}

		/// <summary>
		/// 等待提示的文本配置。
		/// </summary>
		public WaitHintTextConfig
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
			foreach(string v in HintList)
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
						HintList.Add(obj.Value);
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
			sb.Append("HintList : [");
			for(int i = 0; i < HintList.Count;i ++)
			{
				if(i == HintList.Count -1)
				{
					sb.Append(HintList[i]);
				}else
				{
					sb.Append(HintList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
