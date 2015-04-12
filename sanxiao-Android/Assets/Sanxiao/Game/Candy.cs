using System.Text;
using Assets.Sanxiao.Data;
using Fairwood.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 方块,宝石,糖果
    /// </summary>
    public class Candy : MonoBehaviour
    {
        public const int MaxGenreCount = 6;//Max=6

        public enum CandyType
        {
            Normal = 0,
            H = 1,
            V = 2,
            Bomb = 3,
            /// <summary>
            /// 已经点燃的炸弹，CC里的闪烁糖果，在下一个消除阶段会自动炸掉
            /// </summary>
            Colorful,//c
            Stone,//s 可以移动，不可消除，不可炸掉，底部漏出？的方块
            Chest,//t 可以移动，不可三消，可以炸掉
            Item//血瓶b、魔瓶e、钱袋m
        }

        /// <summary>
        /// 用CandyInfo设置Type、Genre、FiredBombRange
        /// </summary>
        public CandyInfo MyInfo;

        public void Set(CandyType type, int genre)
        {
            MyInfo.SetCode(type, genre);
        }

        public CandyType MyType
        {
            get { return MyInfo.Type; }
        }

        /// <summary>
        /// 0<=value<=9【当且仅当】是Normal、H、V、Bomb，即能三消的
        /// [201,299]【当且仅当】是Item。202:血瓶 203:魔瓶 204:钱袋
        /// 其余皆-1
        /// </summary>
        public int Genre
        {
            get { return MyInfo.Genre; }
        }
        /// <summary>
        /// 用于三消的，有颜色的，即Normal/H/V/Bomb状态的
        /// </summary>
        public bool IsForThreeMatching
        {
            get
            {
                return MyType == CandyType.Normal || MyType == CandyType.H || MyType == CandyType.V ||
                       MyType == CandyType.Bomb;
            }
        }

        public bool IsNormalOrSpecial
        {
            get { return MyType == CandyType.Normal || IsSpecial; }
        }

        /// <summary>
        /// 是特殊糖果，包括Colorful,H,V,Bomb
        /// </summary>
        public bool IsSpecial
        {
            get
            {
                return MyType == CandyType.Colorful || MyType == CandyType.H || MyType == CandyType.V ||
                       MyType == CandyType.Bomb;
            }
        }
        /// <summary>
        /// 是不是条纹糖，H或V
        /// </summary>
        public bool IsStripe
        {
            get { return MyType == CandyType.H || MyType == CandyType.V; }
        }
        /// <summary>
        /// 可以被特殊消除的，包含可以被三消的，但不处于Popping状态的
        /// </summary>
        public bool CanBeSpecialPopped
        {
            get { return (IsNormalOrSpecial || MyType == CandyType.Chest) && !IsPopping; }
        }



        public CandyRenderer CandyRenderer;

        /// <summary>
        /// 每次使用时调用，从CandyPool中拿出来时，不调用Awake只调用OnEnable
        /// </summary>
        void OnEnable()
        {
            //RespawnTime = Time.time;
            name = "Candy";
            Fired = false;
        }

        /// <summary>
        /// 糖果生成的时间，使用Time.time。自爆炸弹在生成后3秒自爆
        /// </summary>
        //public float RespawnTime;

        /// <summary>
        /// (仅自爆炸弹有效)这一帧是否该自爆了，在生成后有1秒延时才自爆
        /// </summary>
        public bool ShouldFiredCandyPopNow
        {
            get { return Time.time > FiredStartTime + FiredWaitingTime; }
        }

        public enum CandyState
        {
            Static,
            Falling,
            Popping,
            Exchange,
        }

        //点燃与否
        /// <summary>
        /// 点燃与否，状态，与CandyState、CandyType不是一层。目前用于H,V,Bomb,Colorful。
        /// </summary>
        public bool Fired { get; private set; }
        public float FiredStartTime { get; private set; }//何时点燃
        public float FiredWaitingTime { get; private set; }//点燃到爆炸的延迟
        /// <summary>
        /// 自爆炸弹的威力，正方形区域的边长，仅Fired Bomb用到，目前只有3,5两种取值
        /// </summary>
        public int FiredBombRange
        {
            get { return MyInfo.FiredCandyExtraData; }
            set { MyInfo.FiredCandyExtraData = value; }
        }
        /// <summary>
        /// 变成点燃状态，使用默认引燃时间
        /// </summary>
        public void BecomeFired()
        {
            Fired = true;
            FiredStartTime = Time.time;
            FiredWaitingTime = GameData.DefaultFiredWaitingTime;
        }
        /// <summary>
        /// 变成点燃状态，给定引燃时间
        /// </summary>
        public void BecomeFired(float firedWaitingTime)
        {
            Fired = true;
            FiredStartTime = Time.time;
            FiredWaitingTime = firedWaitingTime;
        }

        /// <summary>
        /// 彩色糖随机消一种颜色要排除哪个Genre，仅Fired Colorful用到，-1表示不排除任何
        /// </summary>
        public int ColorfulRandomExclude
        {
            get { return MyInfo.FiredCandyExtraData; }
            set { MyInfo.FiredCandyExtraData = value; }
        }

        #region Fall

        public CandyState State;

        public bool IsStatic
        {
            get { return State == CandyState.Static; }
        }

        public bool IsPopping { get { return State == CandyState.Popping; } }

        public Grid MyGrid;

        /// <summary>
        /// 当且仅当IsFalling时有效,必定在CurIJ的左上、上、右上，1格的距离
        /// </summary>
        public IntVector2 FromIJ;
        public IntVector2 CurIJ;

        private bool IsCrossColumn
        {
            get { return FromIJ.j != CurIJ.j; }
        }

        /// <summary>
        /// 以格为单位，向下为正，不可能是负的
        /// </summary>
        public float FallingSpeed;
        /// <summary>
        /// 以格为单位，向下为正
        /// </summary>
        public const float Acceleration = 30;

        /// <summary>
        /// 在交换里，是直接由玩家操作的糖果，而非被动糖果
        /// </summary>
        private bool _isMainInExchange;
        public IntVector2 ExchangeFromIJ, ExchangeToIJ;
        /// <summary>
        /// 仅Exchange时有效。交换成功与否，两种情况不同的处理
        /// </summary>
        public bool ExchangeSuccessful;
        /// <summary>
        /// 仅Exchange时有效。交换的相位。成功时0~1只有单程，失败时0~2包括往返
        /// </summary>
        public float ExchangePhase;

        public Animation CushionAnimation;
        public AudioClip CushionAudio;

        static readonly IntVector2[] CheckGoOnFallingList = new[] { new IntVector2(1, 0), new IntVector2(1, 1), new IntVector2(1, -1) };

        /// <summary>
        /// 由UpdateMoving统一调用的Update下落
        /// </summary>
        public void UpdateFalling()
        {
            if (State == CandyState.Falling) //在下落状态
            {
                FallingSpeed += Acceleration*Time.deltaTime*(IsCrossColumn ? 0.5f : 1);
                transform.localPosition +=
                    new Vector3(IsCrossColumn ? (CurIJ.j - FromIJ.j)*Grid.CellSize.x : 0,
                                -(CurIJ.i - FromIJ.i)*Grid.CellSize.y)
                    *FallingSpeed*Time.deltaTime;

                CandyRenderer.HasSupport = false;
            }

            if ((State == CandyState.Falling && transform.localPosition.y < MyGrid.GetCellPosition(CurIJ).y) ||
                State == CandyState.Static)
            {
                //检测是否需要下落
                int fallingType = 2; //1:正常下落 2:停止 3:石头漏出
                var nextCell = IntVector2.zero;
                var isReachTheLowest = false;
                if (MyGrid[CurIJ].MyState == Cell.State.Normal) //所在格子是Normal才可以下落，否则肯定Lock住
                {
                    if (CurIJ.i < Grid.Height - 1) //不在网格底部
                    {
                        //检测下方
                        for (int index = 0; index < CheckGoOnFallingList.Length; index++)
                        {
                            var fallingDir = CheckGoOnFallingList[index];

                            nextCell = CurIJ + fallingDir;

                            if (index == 0) //↓
                            {
                                if (!MyGrid.IsIJInGrid(nextCell)) continue; //目标格子超出范围就放弃这个方向
                                while (true)
                                {
                                    if (MyGrid[nextCell].MyState != Cell.State.Null) break; //找到下方非Null格子中最靠上的
                                    nextCell.i++;
                                    if (!MyGrid.IsIJInGrid(nextCell)) //如果已经超出网格范围，则说明下面全是Null，即已经在列的底部了
                                    {
                                        isReachTheLowest = true;
                                        break;
                                    }
                                }
                                if (isReachTheLowest) continue; //在列的底部肯定不能下落，不过可以看看是不是可以侧滑
                                //nextCell是CurIJ的下方非Null格子中最靠上的
                                if (!MyGrid[nextCell].CanHoldCandy || MyGrid[nextCell].MyCandy != null) //如果目标格不能持有糖果就放弃
                                {
                                    continue;
                                }
                                fallingType = 1;//确定下落方向为↓
                                break;//不再检测其他方向
                            }
                            else //↙、↘
                            {
                                if (!MyGrid.IsIJInGrid(nextCell)) continue; //目标格子超出范围就放弃这个方向
                                if (!MyGrid[nextCell].CanHoldCandy || MyGrid[nextCell].MyCandy != null) //如果目标格不能持有糖果就放弃
                                {
                                    continue;
                                }
                                //可以继续向dir下落
                                var theCellAboveNextCell = nextCell;
                                theCellAboveNextCell.i--;
                                do //验证目标格子上方是否有可以落下来的
                                {
                                    if (!MyGrid.IsIJInGrid(theCellAboveNextCell)) break; //目标格子超出网格范围

                                    if (MyGrid[theCellAboveNextCell].MyState == Cell.State.Brick ||
                                        MyGrid[theCellAboveNextCell].MyState == Cell.State.Lock)
                                        //如果那个空格的正上方是不可穿透的，就可以落进去。唯一确定能下落的出口！
                                    {
                                        fallingType = 1;
                                        break;
                                    }
                                    if (MyGrid[theCellAboveNextCell].MyState == Cell.State.Normal &&
                                        MyGrid[theCellAboveNextCell].MyCandy) //这个Candy会落到这个格子里的，所以不需要我侧滑
                                    {
                                        break;
                                    }
                                    //剩余的情况是Normal && 没有Candy
                                    theCellAboveNextCell.i--;
                                } while (theCellAboveNextCell.i >= 0);
                                if (fallingType == 1) break; //已经确定朝哪个方向下落了(↙、↓、↘)，就跳出循环
                            }
                        }
                    }
                    else
                    {
                        isReachTheLowest = true;
                    }
                    if (fallingType == 2 && isReachTheLowest && MyType == CandyType.Stone)//石头
                    {
                        fallingType = 3;
                    }
                }

                if (fallingType == 1) //还能继续下滑
                {
                    if (State == CandyState.Falling)
                    {
                        if (!IsCrossColumn && nextCell.j != CurIJ.j) FallingSpeed *= 0.707f; //从向下变成斜向滑动时速度损耗

                        //Grid的逻辑层，释放旧的Cell，占领自己的Cell
                        if (MyGrid.IsIJInGrid(CurIJ)) MyGrid[CurIJ].MyCandy = null;
                        FromIJ = CurIJ;
                        CurIJ = nextCell;
                        MyGrid[CurIJ].MyCandy = this;
                    }
                    else if (State == CandyState.Static)
                    {
                        State = CandyState.Falling;

                        //Grid的逻辑层，占领自己的Cell
                        FromIJ = CurIJ;
                        CurIJ = nextCell;
                        MyGrid[CurIJ].MyCandy = this;
                        MyGrid[FromIJ].MyCandy = null;
                    }
                }
                else if (fallingType == 2) //该停止了
                {
                    if (State == CandyState.Falling)
                    {
                        State = CandyState.Static;
                        transform.localPosition = MyGrid.GetCellPosition(CurIJ);
                        CushionAnimation.Play(); //缓冲，如果跳帧就用CrossFade
                        AudioManager.PlayOneShot(CushionAudio, Mathf.Clamp01((FallingSpeed - 5)/25));
                        FallingSpeed = 0;
                        CandyRenderer.HasSupport = true;
                    }
                    else if (State == CandyState.Static)
                    {
                    }
                }
                else if (fallingType == 3) //石头坠落
                {
                    // 漏出三消区
                    if (MyGrid[CurIJ].MyCandy == this) MyGrid[CurIJ].MyCandy = null;
                    _isFallingOut = true;
                    CandyPool.Enqueue(gameObject, 1f);
                }
            }
        }

        private bool _isFallingOut = false;
        void Update()
        {
            if (_isFallingOut)
            {
                FallingSpeed += Acceleration * Time.deltaTime;
                transform.localPosition += new Vector3(0, -FallingSpeed*Grid.CellSize.y*Time.deltaTime);
            }
        }

        #endregion

        #region Exchange交换

        /// <summary>
        /// 交换时移动速度，单位是一格
        /// </summary>
        private const float ExchangeSpeed = 1/Grid.ExchangeSuccessTime;

        /// <summary>
        /// 转换成交换状态。如果成功，还需要设置CurIJ，和Grid[IJ]含有的糖果
        /// </summary>
        /// <param name="fromCell">此时必须和CurIJ相同才对</param>
        /// <param name="toCell">马上就要将CurIJ设置成与这相同</param>
        /// <param name="isMain">是玩家操作的那个，会播放跃起放大动画</param>
        /// <param name="successful"></param>
        public void TransitionToExchange(IntVector2 fromCell, IntVector2 toCell, bool isMain, bool successful)
        {
            if (State != CandyState.Static) Debug.LogError("怎么可能从非Static状态变为Exchange状态呢,务必检查");
            State = CandyState.Exchange;

            _isMainInExchange = isMain;
            ExchangeFromIJ = fromCell;
            ExchangeToIJ = toCell;
            ExchangeSuccessful = successful;
            ExchangePhase = 0;

            if (isMain) CandyRenderer.Exchange();
        }
        /// <summary>
        /// 由UpdateMoving统一调用的Update交换
        /// </summary>
        public void UpdateExchange()
        {
            if (State == CandyState.Exchange)
            {
                ExchangePhase += ExchangeSpeed*Time.deltaTime;
                if (ExchangeSuccessful && ExchangePhase >= 1) //成功的Exchange结束了
                {
                    State = CandyState.Static;
                    transform.localPosition = MyGrid.GetCellPosition(ExchangeToIJ);
                    if (_isMainInExchange) MyGrid.AddFinishExchangeInfo(new Grid.ExchangeInfo(ExchangeToIJ, ExchangeFromIJ));
                    return;
                }
                if (!ExchangeSuccessful && ExchangePhase >= 2) //失败的Exchange结束了
                {
                    State = CandyState.Static;
                    transform.localPosition = MyGrid.GetCellPosition(ExchangeFromIJ);
                    return;
                }
                var f = ExchangePhase > 1 ? 2 - ExchangePhase : ExchangePhase;
                transform.localPosition = MyGrid.GetCellFloatPosition(Mathf.Lerp(ExchangeFromIJ.i, ExchangeToIJ.i, f),
                                                                      Mathf.Lerp(ExchangeFromIJ.j, ExchangeToIJ.j, f));
            }
        }
        #endregion

        /// <summary>
        /// 由Grid统一调用的Update移动，下落或者交换，都在这里面
        /// </summary>
        public void UpdateMoving()
        {
            UpdateFalling();
            UpdateExchange();
        }

        public void Refresh()
        {
            CandyRenderer.Refresh();
        }

        public void RandomAndRefresh()
        {
            Set(CandyType.Normal, Random.Range(0, MaxGenreCount));
            Refresh();
        }

        #region Pop

        public AudioClip[] PopAudios;

        /// <summary>
        /// 开始消除动作，立即标记为Popping状态
        /// </summary>
        /// <param name="delay"></param>
        public void Pop(float delay)
        {
            name = "Candy " + CurIJ;
            State = CandyState.Popping;

            PopExtraDelay = delay;
            Invoke("_Pop", delay + 0.1f);

            CandyRenderer.Pop();
        }
        /// <summary>
        /// 真正的消除了
        /// </summary>
        private void _Pop()
        {
            //计分
            if (IsNormalOrSpecial)
            {
                MyGrid.AddComboAmountOne();
                int addEnergy;
                if (0 <= Genre && Genre < GameData.CandyEnergyList.Length)
                {
                    addEnergy = GameData.CandyEnergyList[Genre]*MyGrid.CurrentComboMultiple;
                }
                else
                {
                    addEnergy = 1;
                }
                GameManager.Instance.PopEffectContainer.NormalPopEffect(CurIJ, Genre); //普通消除特效
                EnergyLightSpot.Create(CurIJ, addEnergy);//能量光点
                //if (MyGrid.CurrentComboMultiple > 1)
                //{
                //    GameManager.Instance.CellEffectContainer.CreateAddComboLabel(MyGrid.GetCellPosition(CurIJ),
                //                                                                 MyGrid.CurrentComboMultiple);
                //}
            }
            else if (MyType == CandyType.Item)
            {
                switch (Genre)
                {
                    case 202:
                        GameData.MyHealth += 300;
                        break;
                    case 203:
                        EnergyLightSpot.Create(CurIJ, 50);
                        break;
                    case 204:
                        //TODO:钱袋
                        break;
                }
            }

            if (MyGrid[CurIJ].MyCandy == this) MyGrid[CurIJ].MyCandy = null;

            //if (MyType == CandyType.H)
            //{
            //    GameManager.Instance.PopEffectContainer.StripeHSpecialPopEffect(CurIJ, Genre);
            //}
            //else if (MyType == CandyType.V)
            //{
            //    GameManager.Instance.PopEffectContainer.StripeVSpecialPopEffect(CurIJ, Genre);
            //}
            //else if (MyType == CandyType.Bomb)
            //{
            //    if (FiredBombRange < 4)
            //    {
            //        GameManager.Instance.PopEffectContainer.Bomb3SpecialPopEffect(CurIJ, Genre);
            //    }
            //    else
            //    {
            //        GameManager.Instance.PopEffectContainer.Bomb5SpecialPopEffect(CurIJ, Genre);
            //    }
            //}

            //消除音效
            AudioManager.PlayRandomOneShot(0.2f, PopAudios);

            CandyPool.Enqueue(gameObject, 1f);
        }

        public float PopExtraDelay { get; private set; }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder("Candy:{").Append(MyInfo).Append(",");
            sb.Append("CurIJ:").Append(CurIJ);
            sb.Append("}");
            return sb.ToString();
        }
    }
}