using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Sanxiao.Communication.Proto
{
	/// <summary>
	/// ---- 推图相关配置 End ----
	/// 各种配置的hash值列表。
	/// 客户端登陆的时候发给客户端。
	/// 如果客户端发现某个配置的hash值与缓存的hash值不一致，则请求下载该配置。
	/// 0:全局配置。
	/// 1:充值包的配置。
	/// 2:技能数据。
	/// 3:技能配置。
	/// 4:兑换包配置。
	/// 5:蔬菜配置。
	/// 6:推图关卡配置。
	/// 7:技能简介的文本。
	/// 8:技能等级详情文本。
	/// 9:蔬菜介绍的文本。
	/// 10:等待的时候显示的提示文本。
	/// 11:角色配置。
	/// 12:装备配置。
	/// 13:大关介绍文本。
	/// 14:装备介绍文本。
	/// 15:角色介绍文本。
	/// 16:社交平台参数配置。
	/// </summary>
	public class ConfigHash : ISendable, IReceiveable
	{
		private List<string> configHashList;
		/// <summary>
		/// </summary>
		public List<string> ConfigHashList
		{
			get
			{
				return configHashList;
			}
			set
			{
				if(value != null)
				{
					configHashList = value;
				}
			}
		}

		/// <summary>
		/// ---- 推图相关配置 End ----
		/// 各种配置的hash值列表。
		/// 客户端登陆的时候发给客户端。
		/// 如果客户端发现某个配置的hash值与缓存的hash值不一致，则请求下载该配置。
		/// 0:全局配置。
		/// 1:充值包的配置。
		/// 2:技能数据。
		/// 3:技能配置。
		/// 4:兑换包配置。
		/// 5:蔬菜配置。
		/// 6:推图关卡配置。
		/// 7:技能简介的文本。
		/// 8:技能等级详情文本。
		/// 9:蔬菜介绍的文本。
		/// 10:等待的时候显示的提示文本。
		/// 11:角色配置。
		/// 12:装备配置。
		/// 13:大关介绍文本。
		/// 14:装备介绍文本。
		/// 15:角色介绍文本。
		/// 16:社交平台参数配置。
		/// </summary>
		public ConfigHash()
		{
			ConfigHashList = new List<string>();
		}

		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			foreach(string v in ConfigHashList)
			{
				writer.Write(1,v);
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
						ConfigHashList.Add(obj.Value);
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
			sb.Append("ConfigHashList : [");
			for(int i = 0; i < ConfigHashList.Count;i ++)
			{
				if(i == ConfigHashList.Count -1)
				{
					sb.Append(ConfigHashList[i]);
				}else
				{
					sb.Append(ConfigHashList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
