using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 升级技能成功。
	/// </summary>
	public class UpgradeSkillOk : ISendable, IReceiveable
	{
		private bool HasCurrentSkill{get;set;}
		private UserSkill currentSkill;
		/// <summary>
		/// 用户当前的技能数据。
		/// </summary>
		public UserSkill CurrentSkill
		{
			get
			{
				return currentSkill;
			}
			set
			{
				if(value != null)
				{
					HasCurrentSkill = true;
					currentSkill = value;
				}
			}
		}

		private bool HasCurrentUser{get;set;}
		private User currentUser;
		/// <summary>
		/// 用户当前的数据。
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
		/// 升级技能成功。
		/// </summary>
		public UpgradeSkillOk()
		{
		}

		/// <summary>
		/// 升级技能成功。
		/// </summary>
		public UpgradeSkillOk
		(
			UserSkill currentSkill,
			User currentUser
		):this()
		{
			CurrentSkill = currentSkill;
			CurrentUser = currentUser;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CurrentSkill);
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
						CurrentSkill = new UserSkill();
						CurrentSkill.ParseFrom(obj.Value);
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
			sb.Append("CurrentSkill : " + CurrentSkill + ",");
			sb.Append("CurrentUser : " + CurrentUser);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
