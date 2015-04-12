using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 对服务器的同步数据的反馈。
	/// </summary>
	public class SyncDataResponse : ISendable, IReceiveable
	{
		private bool HasMyEnergy{get;set;}
		private int myEnergy;
		/// <summary>
		/// 当前我的怒气值。
		/// </summary>
		public int MyEnergy
		{
			get
			{
				return myEnergy;
			}
			set
			{
				HasMyEnergy = true;
				myEnergy = value;
			}
		}

		/// <summary>
		/// 对服务器的同步数据的反馈。
		/// </summary>
		public SyncDataResponse()
		{
		}

		/// <summary>
		/// 对服务器的同步数据的反馈。
		/// </summary>
		public SyncDataResponse
		(
			int myEnergy
		):this()
		{
			MyEnergy = myEnergy;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MyEnergy);
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
						MyEnergy = obj.Value;
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
			sb.Append("MyEnergy : " + MyEnergy);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
