using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 加速蔬菜升级成功。
	/// </summary>
	public class SpeedUpVegetableUpgradeOk : ISendable, IReceiveable
	{
		private bool HasCurrentVegetable{get;set;}
		private UserVegetable currentVegetable;
		/// <summary>
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

		/// <summary>
		/// 加速蔬菜升级成功。
		/// </summary>
		public SpeedUpVegetableUpgradeOk()
		{
		}

		/// <summary>
		/// 加速蔬菜升级成功。
		/// </summary>
		public SpeedUpVegetableUpgradeOk
		(
			UserVegetable currentVegetable
		):this()
		{
			CurrentVegetable = currentVegetable;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,CurrentVegetable);
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
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("CurrentVegetable : " + CurrentVegetable);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
