using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 请求成功的时候，直接发送UserVegetable消息给客户端。
	/// 请求玩家蔬菜信息失败。
	/// </summary>
	public class RequestUserVegetableFail : ISendable, IReceiveable
	{
		private bool HasResult{get;set;}
		private MsgResult result;
		/// <summary>
		/// </summary>
		public MsgResult Result
		{
			get
			{
				return result;
			}
			set
			{
				if(value != null)
				{
					HasResult = true;
					result = value;
				}
			}
		}

		private bool HasVegetableCode{get;set;}
		private int vegetableCode;
		/// <summary>
		/// </summary>
		public int VegetableCode
		{
			get
			{
				return vegetableCode;
			}
			set
			{
				HasVegetableCode = true;
				vegetableCode = value;
			}
		}

		/// <summary>
		/// 请求成功的时候，直接发送UserVegetable消息给客户端。
		/// 请求玩家蔬菜信息失败。
		/// </summary>
		public RequestUserVegetableFail()
		{
		}

		/// <summary>
		/// 请求成功的时候，直接发送UserVegetable消息给客户端。
		/// 请求玩家蔬菜信息失败。
		/// </summary>
		public RequestUserVegetableFail
		(
			MsgResult result,
			int vegetableCode
		):this()
		{
			Result = result;
			VegetableCode = vegetableCode;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Result);
			writer.Write(2,VegetableCode);
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
						Result = new MsgResult();
						Result.ParseFrom(obj.Value);
						break;
					case 2:
						VegetableCode = obj.Value;
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
			sb.Append("Result : " + Result + ",");
			sb.Append("VegetableCode : " + VegetableCode);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
