using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 服务器发给客户端的同步数据。
	/// </summary>
	public class SyncData : ISendable, IReceiveable
	{
		private bool HasMyHealth{get;set;}
		private int myHealth;
		/// <summary>
		/// 我当前的血量值。
		/// </summary>
		public int MyHealth
		{
			get
			{
				return myHealth;
			}
			set
			{
				HasMyHealth = true;
				myHealth = value;
			}
		}

		private bool HasRivalHealth{get;set;}
		private int rivalHealth;
		/// <summary>
		/// 对手当前的血量值。
		/// </summary>
		public int RivalHealth
		{
			get
			{
				return rivalHealth;
			}
			set
			{
				HasRivalHealth = true;
				rivalHealth = value;
			}
		}

		private bool HasRivalEnergy{get;set;}
		private int rivalEnergy;
		/// <summary>
		/// 对手的能量值。
		/// </summary>
		public int RivalEnergy
		{
			get
			{
				return rivalEnergy;
			}
			set
			{
				HasRivalEnergy = true;
				rivalEnergy = value;
			}
		}

		/// <summary>
		/// 服务器发给客户端的同步数据。
		/// </summary>
		public SyncData()
		{
		}

		/// <summary>
		/// 服务器发给客户端的同步数据。
		/// </summary>
		public SyncData
		(
			int myHealth,
			int rivalHealth,
			int rivalEnergy
		):this()
		{
			MyHealth = myHealth;
			RivalHealth = rivalHealth;
			RivalEnergy = rivalEnergy;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MyHealth);
			writer.Write(2,RivalHealth);
			writer.Write(3,RivalEnergy);
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
						MyHealth = obj.Value;
						break;
					case 2:
						RivalHealth = obj.Value;
						break;
					case 3:
						RivalEnergy = obj.Value;
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
			sb.Append("MyHealth : " + MyHealth + ",");
			sb.Append("RivalHealth : " + RivalHealth + ",");
			sb.Append("RivalEnergy : " + RivalEnergy);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
