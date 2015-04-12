using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 兑换成功。
	/// </summary>
	public class ExchangeOk : ISendable, IReceiveable
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
		/// 兑换的数量。
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

		private bool HasUser{get;set;}
		private User user;
		/// <summary>
		/// 玩家当前的状态。
		/// </summary>
		public User User
		{
			get
			{
				return user;
			}
			set
			{
				if(value != null)
				{
					HasUser = true;
					user = value;
				}
			}
		}

		/// <summary>
		/// 兑换成功。
		/// </summary>
		public ExchangeOk()
		{
		}

		/// <summary>
		/// 兑换成功。
		/// </summary>
		public ExchangeOk
		(
			string name,
			int count,
			User user
		):this()
		{
			Name = name;
			Count = count;
			User = user;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Name);
			writer.Write(2,Count);
			writer.Write(3,User);
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
					case 3:
						User = new User();
						User.ParseFrom(obj.Value);
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
			sb.Append("Count : " + Count + ",");
			sb.Append("User : " + User);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
