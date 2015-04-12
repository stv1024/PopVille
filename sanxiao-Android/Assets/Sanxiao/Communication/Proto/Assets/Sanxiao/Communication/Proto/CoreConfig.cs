using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 核心配置。
	/// </summary>
	public class CoreConfig : ISendable, IReceiveable
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

		private bool HasHeartMaxLimit{get;set;}
		private int heartMaxLimit;
		/// <summary>
		/// 爱心的上限值。
		/// </summary>
		public int HeartMaxLimit
		{
			get
			{
				return heartMaxLimit;
			}
			set
			{
				HasHeartMaxLimit = true;
				heartMaxLimit = value;
			}
		}

		private bool HasHeartRecoverTime{get;set;}
		private long heartRecoverTime;
		/// <summary>
		/// 回复一个爱心所需要的时间（单位：毫秒）。
		/// </summary>
		public long HeartRecoverTime
		{
			get
			{
				return heartRecoverTime;
			}
			set
			{
				HasHeartRecoverTime = true;
				heartRecoverTime = value;
			}
		}

		/// <summary>
		/// 核心配置。
		/// </summary>
		public CoreConfig()
		{
		}

		/// <summary>
		/// 核心配置。
		/// </summary>
		public CoreConfig
		(
			string hash,
			int heartMaxLimit,
			long heartRecoverTime
		):this()
		{
			Hash = hash;
			HeartMaxLimit = heartMaxLimit;
			HeartRecoverTime = heartRecoverTime;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Hash);
			writer.Write(2,HeartMaxLimit);
			writer.Write(3,HeartRecoverTime);
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
						HeartMaxLimit = obj.Value;
						break;
					case 3:
						HeartRecoverTime = obj.Value;
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
			sb.Append("HeartMaxLimit : " + HeartMaxLimit + ",");
			sb.Append("HeartRecoverTime : " + HeartRecoverTime);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
