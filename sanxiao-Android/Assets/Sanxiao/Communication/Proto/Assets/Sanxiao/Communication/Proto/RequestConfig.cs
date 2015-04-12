using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求服务器端的文本配置。
	/// </summary>
	public class RequestConfig : ISendable, IReceiveable
	{
		private List<int> configIdList;
		/// <summary>
		/// 需要请求的配置id列表。
		/// </summary>
		public List<int> ConfigIdList
		{
			get
			{
				return configIdList;
			}
			set
			{
				if(value != null)
				{
					configIdList = value;
				}
			}
		}

		/// <summary>
		/// 请求服务器端的文本配置。
		/// </summary>
		public RequestConfig()
		{
			ConfigIdList = new List<int>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(int v in ConfigIdList)
			{
				writer.Write(1,v);
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
						ConfigIdList.Add(obj.Value);
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
			sb.Append("ConfigIdList : [");
			for(int i = 0; i < ConfigIdList.Count;i ++)
			{
				if(i == ConfigIdList.Count -1)
				{
					sb.Append(ConfigIdList[i]);
				}else
				{
					sb.Append(ConfigIdList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
