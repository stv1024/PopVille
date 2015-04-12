using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 升级蔬菜成功。
	/// </summary>
	public class UpgradeVegetableOk : ISendable, IReceiveable
	{
		private bool HasCurrentVegetable{get;set;}
		private UserVegetable currentVegetable;
		/// <summary>
		/// 玩家当前的蔬菜数据。
		/// </summary>
		public UserVegetable CurrentVegetable
		{
			get
			{
				return currentVegetable;
			}
			set
			{
				if(value != null)
				{
					HasCurrentVegetable = true;
					currentVegetable = value;
				}
			}
		}

		private bool HasCurrentUser{get;set;}
		private User currentUser;
		/// <summary>
		/// 玩家当前的用户数据。
		/// </summary>
		public User CurrentUser
		{
			get
			{
				return currentUser;
			}
			set
			{
				if(value != null)
				{
					HasCurrentUser = true;
					currentUser = value;
				}
			}
		}

		/// <summary>
		/// 升级蔬菜成功。
		/// </summary>
		public UpgradeVegetableOk()
		{
		}

		/// <summary>
		/// 升级蔬菜成功。
		/// </summary>
		public UpgradeVegetableOk
		(
			UserVegetable currentVegetable,
			User currentUser
		):this()
		{
			CurrentVegetable = currentVegetable;
			CurrentUser = currentUser;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CurrentVegetable);
			writer.Write(2,CurrentUser);
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
						CurrentVegetable = new UserVegetable();
						CurrentVegetable.ParseFrom(obj.Value);
						break;
					case 2:
						CurrentUser = new User();
						CurrentUser.ParseFrom(obj.Value);
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
			sb.Append("CurrentVegetable : " + CurrentVegetable + ",");
			sb.Append("CurrentUser : " + CurrentUser);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
