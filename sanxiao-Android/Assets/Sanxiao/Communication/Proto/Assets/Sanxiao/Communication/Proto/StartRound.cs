using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 开始一局。
	/// </summary>
	public class StartRound : ISendable, IReceiveable
	{
		private bool HasRoundTimeout{get;set;}
		private int roundTimeout;
		/// <summary>
		/// 本局最长的时间。如果超过此时间，则认为两边平局。（单位：秒）
		/// </summary>
		public int RoundTimeout
		{
			get
			{
				return roundTimeout;
			}
			set
			{
				HasRoundTimeout = true;
				roundTimeout = value;
			}
		}

		/// <summary>
		/// 开始一局。
		/// </summary>
		public StartRound()
		{
		}

		/// <summary>
		/// 开始一局。
		/// </summary>
		public StartRound
		(
			int roundTimeout
		):this()
		{
			RoundTimeout = roundTimeout;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,RoundTimeout);
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
						RoundTimeout = obj.Value;
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
			sb.Append("RoundTimeout : " + RoundTimeout);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
