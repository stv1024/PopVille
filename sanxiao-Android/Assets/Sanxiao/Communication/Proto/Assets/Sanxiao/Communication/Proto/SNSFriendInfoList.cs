using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求好友信息列表。
	/// RequestSNSFriendInfoList
	/// 社交好友信息列表。
	/// </summary>
	public class SNSFriendInfoList : ISendable, IReceiveable
	{
		private List<SNSFriendInfo> friendList;
		/// <summary>
		/// </summary>
		public List<SNSFriendInfo> FriendList
		{
			get
			{
				return friendList;
			}
			set
			{
				if(value != null)
				{
					friendList = value;
				}
			}
		}

		/// <summary>
		/// 请求好友信息列表。
		/// RequestSNSFriendInfoList
		/// 社交好友信息列表。
		/// </summary>
		public SNSFriendInfoList()
		{
			FriendList = new List<SNSFriendInfo>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(SNSFriendInfo v in FriendList)
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
						 var friend= new SNSFriendInfo();
						friend.ParseFrom(obj.Value);
						FriendList.Add(friend);
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
			sb.Append("FriendList : [");
			for(int i = 0; i < FriendList.Count;i ++)
			{
				if(i == FriendList.Count -1)
				{
					sb.Append(FriendList[i]);
				}else
				{
					sb.Append(FriendList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
