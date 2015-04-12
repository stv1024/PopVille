using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// 社交平台参数配置。
	/// </summary>
	public class OAuthParamConfig : ISendable, IReceiveable
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

		private List<OAuthParam> paramList;
		/// <summary>
		/// </summary>
		public List<OAuthParam> ParamList
		{
			get
			{
				return paramList;
			}
			set
			{
				if(value != null)
				{
					paramList = value;
				}
			}
		}

		/// <summary>
		/// 社交平台参数配置。
		/// </summary>
		public OAuthParamConfig()
		{
			ParamList = new List<OAuthParam>();
		}

		/// <summary>
		/// 社交平台参数配置。
		/// </summary>
		public OAuthParamConfig
		(
			string hash
		):this()
		{
			Hash = hash;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Hash);
			foreach(OAuthParam v in ParamList)
			{
				writer.Write(2,v);
			}
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
						 var param= new OAuthParam();
						param.ParseFrom(obj.Value);
						ParamList.Add(param);
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
			sb.Append("ParamList : [");
			for(int i = 0; i < ParamList.Count;i ++)
			{
				if(i == ParamList.Count -1)
				{
					sb.Append(ParamList[i]);
				}else
				{
					sb.Append(ParamList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
