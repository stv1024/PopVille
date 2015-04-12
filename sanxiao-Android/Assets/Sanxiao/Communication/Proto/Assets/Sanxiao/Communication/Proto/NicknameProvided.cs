using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求可用昵称。
	/// RequestNickname
	/// 可选昵称。
	/// </summary>
	public class NicknameProvided : ISendable, IReceiveable
	{
		private List<string> nicknameList;
		/// <summary>
		/// </summary>
		public List<string> NicknameList
		{
			get
			{
				return nicknameList;
			}
			set
			{
				if(value != null)
				{
					nicknameList = value;
				}
			}
		}

		/// <summary>
		/// 请求可用昵称。
		/// RequestNickname
		/// 可选昵称。
		/// </summary>
		public NicknameProvided()
		{
			NicknameList = new List<string>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(string v in NicknameList)
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
						NicknameList.Add(obj.Value);
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
			sb.Append("NicknameList : [");
			for(int i = 0; i < NicknameList.Count;i ++)
			{
				if(i == NicknameList.Count -1)
				{
					sb.Append(NicknameList[i]);
				}else
				{
					sb.Append(NicknameList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
