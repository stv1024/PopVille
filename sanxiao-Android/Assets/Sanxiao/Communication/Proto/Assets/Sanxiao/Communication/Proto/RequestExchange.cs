using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 玩家请求兑换。
	/// </summary>
	public class RequestExchange : ISendable, IReceiveable
	{
		private bool HasName{get;set;}
		private string name;
		/// <summary>
		/// 兑换包的名称。
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if(value != null)
				{
					HasName = true;
					name = value;
				}
			}
		}

		private bool HasCount{get;set;}
		private int count;
		/// <summary>
		/// 数量。
		/// </summary>
		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				HasCount = true;
				count = value;
			}
		}

		/// <summary>
		/// 玩家请求兑换。
		/// </summary>
		public RequestExchange()
		{
		}

		/// <summary>
		/// 玩家请求兑换。
		/// </summary>
		public RequestExchange
		(
			string name,
			int count
		):this()
		{
			Name = name;
			Count = count;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Name);
			writer.Write(2,Count);
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
						Name = obj.Value;
						break;
					case 2:
						Count = obj.Value;
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
			sb.Append("Name : " + Name + ",");
			sb.Append("Count : " + Count);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
