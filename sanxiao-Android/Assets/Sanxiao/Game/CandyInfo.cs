using System.Text;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    public struct CandyInfo
    {
        /// <summary>
        /// 唯一标识。
        /// <para>0~9:Normal</para>
        /// <para>10~19:H</para>
        /// <para>20~29:V</para>
        /// <para>30~39:Bomb</para>
        /// <para>40:Colorful</para>
        /// <para>101:Stone; 102:Chest</para>
        /// <para>202:血瓶；203:魔瓶；204:钱袋</para>
        /// </summary>
        public int Code;

        public Candy.CandyType Type
        {
            get
            {
                if (Code < 0)
                {
                    Debug.LogError("Code不能小于0.Code == " + Code);
                    return (Candy.CandyType) (-1);
                }
                if (Code < 10) return Candy.CandyType.Normal;
                if (Code < 20) return Candy.CandyType.H;
                if (Code < 30) return Candy.CandyType.V;
                if (Code < 40) return Candy.CandyType.Bomb;
                if (Code == 40) return Candy.CandyType.Colorful;
                if (Code == 101) return Candy.CandyType.Stone;
                if (Code == 102) return Candy.CandyType.Chest;
                if (Code/100 == 2) return Candy.CandyType.Item;
                Debug.LogError("Code不在有效范围内.Code == " + Code);
                return (Candy.CandyType) (-1);
            }
        }

        public void SetCode(Candy.CandyType type, int genre)
        {
            if (type == Candy.CandyType.Normal) Code = genre % 10;
            else if (type == Candy.CandyType.H) Code = 10 + genre % 10;
            else if (type == Candy.CandyType.V) Code = 20 + genre % 10;
            else if (type == Candy.CandyType.Bomb) Code = 30 + genre % 10;
            else if (type == Candy.CandyType.Colorful) Code = 40;
            else if (type == Candy.CandyType.Stone) Code = 101;
            else if (type == Candy.CandyType.Chest) Code = 102;
            else if (type == Candy.CandyType.Item)
            {
                Code = Mathf.Clamp(genre, 201, 299);
            }
            else Debug.LogError("未考虑的情况type:" + type + ", genre:" + genre);
        }

        /// <summary>
        /// 0<=value<=9【当且仅当】是Normal、H、V、Bomb，即能三消的
        /// [201,299]【当且仅当】是Item。202:血瓶 203:魔瓶 204:钱袋
        /// 其余皆-1
        /// </summary>
        public int Genre
        {
            get
            {

                if (Code < 0)
                {
                    Debug.LogError("Code不能小于0.Code == " + Code);
                    return -1;
                }
                if (Code < 40) return Code%10;
                if (Code == 40) return -1;
                if (Code/100 == 2) return Code;
                return -1;
            }
        }


        /// <summary>
        /// 自爆炸弹状态下表示：威力，正方形区域的边长，目前只有3,5两种取值
        /// 自爆彩色糖状态下表示：不参与随机抽取的Genre，-1表示不排除任何Genre
        /// </summary>
        public int FiredCandyExtraData;

        public CandyInfo(int code)
        {
            Code = code;
            FiredCandyExtraData = 3;
        }

        public CandyInfo(Candy.CandyType type, int genre, int firedCandyExtraData = 3)
        {
            Code = -1;
            FiredCandyExtraData = firedCandyExtraData;
            SetCode(type, genre);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("");
            sb.Append("Type:").Append(Type).Append(",");
            sb.Append("Genre:").Append(Genre).Append(",");
            sb.Append("FiredCandyExtraData:").Append(FiredCandyExtraData);
            return sb.ToString();
        }

        public static CandyInfo GetRandom(int genreCount)
        {
            return new CandyInfo(Candy.CandyType.Normal, Random.Range(0, genreCount));
        }
    }
}