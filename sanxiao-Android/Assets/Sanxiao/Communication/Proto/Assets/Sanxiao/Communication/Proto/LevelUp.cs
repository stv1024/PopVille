using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 解锁相关命令 Start ----
	/// 玩家升级了。
	/// </summary>
	public class LevelUp : ISendable, IReceiveable
	{
		private bool HasFromLevel{get;set;}
		private int fromLevel;
		/// <summary>
		/// 升级前的等级。
		/// </summary>
		public int FromLevel
		{
			get
			{
				return fromLevel;
			}
			set
			{
				HasFromLevel = true;
				fromLevel = value;
			}
		}

		private bool HasToLevel{get;set;}
		private int toLevel;
		/// <summary>
		/// 升级后的等级。
		/// </summary>
		public int ToLevel
		{
			get
			{
				return toLevel;
			}
			set
			{
				HasToLevel = true;
				toLevel = value;
			}
		}

		private bool HasFromExp{get;set;}
		private long fromExp;
		/// <summary>
		/// 升级前的经验。
		/// </summary>
		public long FromExp
		{
			get
			{
				return fromExp;
			}
			set
			{
				HasFromExp = true;
				fromExp = value;
			}
		}

		private bool HasToExp{get;set;}
		private long toExp;
		/// <summary>
		/// 升级后的经验。
		/// </summary>
		public long ToExp
		{
			get
			{
				return toExp;
			}
			set
			{
				HasToExp = true;
				toExp = value;
			}
		}

		/// <summary>
		/// ---- 解锁相关命令 Start ----
		/// 玩家升级了。
		/// </summary>
		public LevelUp()
		{
		}

		/// <summary>
		/// ---- 解锁相关命令 Start ----
		/// 玩家升级了。
		/// </summary>
		public LevelUp
		(
			int fromLevel,
			int toLevel,
			long fromExp,
			long toExp
		):this()
		{
			FromLevel = fromLevel;
			ToLevel = toLevel;
			FromExp = fromExp;
			ToExp = toExp;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,FromLevel);
			writer.Write(2,ToLevel);
			writer.Write(3,FromExp);
			writer.Write(4,ToExp);
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
						FromLevel = obj.Value;
						break;
					case 2:
						ToLevel = obj.Value;
						break;
					case 3:
						FromExp = obj.Value;
						break;
					case 4:
						ToExp = obj.Value;
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
			sb.Append("FromLevel : " + FromLevel + ",");
			sb.Append("ToLevel : " + ToLevel + ",");
			sb.Append("FromExp : " + FromExp + ",");
			sb.Append("ToExp : " + ToExp);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
