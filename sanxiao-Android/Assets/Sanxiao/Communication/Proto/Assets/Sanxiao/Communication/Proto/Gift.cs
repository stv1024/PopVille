using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 解锁相关命令 End ----
	/// 礼物。
	/// </summary>
	public class Gift : ISendable, IReceiveable
	{
		private bool HasSn{get;set;}
		private int sn;
		/// <summary>
		/// 礼品序列号。
		/// </summary>
		public int Sn
		{
			get
			{
				return sn;
			}
			set
			{
				HasSn = true;
				sn = value;
			}
		}

		private bool HasType{get;set;}
		private int type;
		/// <summary>
		/// 礼品的类型。
		/// </summary>
		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				HasType = true;
				type = value;
			}
		}

		private bool HasCode{get;set;}
		private int code;
		/// <summary>
		/// 礼品的代码。
		/// </summary>
		public int Code
		{
			get
			{
				return code;
			}
			set
			{
				HasCode = true;
				code = value;
			}
		}

		private bool HasAmount{get;set;}
		private long amount;
		/// <summary>
		/// 数量。
		/// </summary>
		public long Amount
		{
			get
			{
				return amount;
			}
			set
			{
				HasAmount = true;
				amount = value;
			}
		}

		private bool HasIsObtained{get;set;}
		private bool isObtained;
		/// <summary>
		/// 是否已经领取。
		/// </summary>
		public bool IsObtained
		{
			get
			{
				return isObtained;
			}
			set
			{
				HasIsObtained = true;
				isObtained = value;
			}
		}

		/// <summary>
		/// ---- 解锁相关命令 End ----
		/// 礼物。
		/// </summary>
		public Gift()
		{
		}

		/// <summary>
		/// ---- 解锁相关命令 End ----
		/// 礼物。
		/// </summary>
		public Gift
		(
			int sn,
			int type,
			int code,
			long amount,
			bool isObtained
		):this()
		{
			Sn = sn;
			Type = type;
			Code = code;
			Amount = amount;
			IsObtained = isObtained;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Sn);
			writer.Write(2,Type);
			writer.Write(3,Code);
			writer.Write(4,Amount);
			writer.Write(5,IsObtained);
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
						Sn = obj.Value;
						break;
					case 2:
						Type = obj.Value;
						break;
					case 3:
						Code = obj.Value;
						break;
					case 4:
						Amount = obj.Value;
						break;
					case 5:
						IsObtained = obj.Value;
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
			sb.Append("Sn : " + Sn + ",");
			sb.Append("Type : " + Type + ",");
			sb.Append("Code : " + Code + ",");
			sb.Append("Amount : " + Amount + ",");
			sb.Append("IsObtained : " + IsObtained);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
