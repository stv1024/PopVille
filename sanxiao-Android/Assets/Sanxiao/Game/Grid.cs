using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.AI;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.Game.Skill;
using UnityEngine;
using Fairwood.Math;
using Random = UnityEngine.Random;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 三消网格
    /// </summary>
    public class Grid : MonoBehaviour//, IEnumerable
    {

        /// <summary>
        /// 三消区域的高宽
        /// </summary>
        public static int Width = 7, Height = 7;//should be 8
        /// <summary>
        /// 最大变长
        /// </summary>
        public static int MaxSide { get { return Mathf.Max(Height, Width); } }
        public static Vector2 CellSize = new Vector2(75, 75)*8/Width;

        public GameManager GameManager;

        public GameObject CellPrefab;
        public Transform CellContainer;

        /// <summary>
        /// 格子矩阵[行号↓][列号→],
        /// </summary>
        public Cell[][] Cells;

        public Cell this[int i, int j]
        {
            get
            {
                return Cells[i][j];
            }
        }

        public Cell this[IntVector2 ij]
        {
            get { return Cells[ij.i][ij.j]; }
        }

        public TextAsset TextLevel;

        bool _canControl;

        public void ResetCells(int height, int width, List<int> gridConfigList)
        {
            Height = height;
            Width = width;
            CellSize = new Vector2(75, 75)*8/Mathf.Max(7, MaxSide);//调整格子大小
            Debug.Log("Grid Size:" + Height + "×" + Width);
            GenerateAllCells();
            if (gridConfigList != null)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        var s = gridConfigList[i*Width + j];
                        if (Enum.IsDefined(typeof (Cell.State), s))
                        {
                            this[i, j].MyState = (Cell.State) s;
                        }
                        else
                        {
                            this[i,j].MyState = Cell.State.Normal;
                        }
                        this[i, j].MyCandy = null;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        var candy = this[i, j].MyCandy;
                        if (candy != null) Destroy(candy.gameObject);
                        this[i, j].MyState = Cell.State.Normal;
                        this[i, j].MyCandy = null;
                    }
                }
            }

            _cellRecords = new CellRecord[Height][];
            for (var i = 0; i < Height; i++)
            {
                _cellRecords[i] = new CellRecord[Width];
                for (int j = 0; j < Width; j++)
                {
                    _cellRecords[i][j].HasStaticCandy = false;
                }
            }
        }
        private void GenerateAllCells()
        {
            if (Cells != null)//确保清除旧的Cells和里面的糖果
            {
                for (var i = 0; i < Cells.Length; i++)
                {
                    if (Cells[i] == null) continue;
                    for (int j = 0; j < Cells[i].Length; j++)
                    {
                        Destroy(Cells[i][j].gameObject);
                    }
                }
            }
            Cells = new Cell[Height][];
            for (var i = 0; i < Height; i++)
            {
                Cells[i] = new Cell[Width];
                for (int j = 0; j < Width; j++)
                {
                    Cells[i][j] = PrefabHelper.InstantiateAndReset<Cell>(CellPrefab, CellContainer);
                    Cells[i][j].name = CellPrefab.name + " " + i + "," + j;
                    Cells[i][j].transform.localPosition = GetCellPosition(i, j);
                }
            }
        }

        void Start()
        {
            if (TextLevel)
            {
                CreateCandysAsPreset(TextLevel.text);
            }
        }
        public void CreateCandysAsPreset(string text)
        {
            var lines = text.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }
            if (lines.Length < Height) throw new ArgumentOutOfRangeException("行数不对");
            for (int i = 0; i < Height; i++)
            {
                var chars = lines[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (chars.Length < Width) throw new ArgumentOutOfRangeException("chars");
                for (int j = 0; j < Width; j++)
                {
                    var c = chars[j];
                    int g;
                    Candy candy;
                    if (c == "c")
                    {
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.Set(Candy.CandyType.Colorful, -1);
                    }
                    else if (c == "s")
                    {
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.Set(Candy.CandyType.Stone, -1);
                    }
                    else if (c == "t")
                    {
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.Set(Candy.CandyType.Chest, -1);
                    }
                    else if (c == "b")
                    {
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.Set(Candy.CandyType.Item, -2);
                    }
                    else if (c == "e")
                    {
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.Set(Candy.CandyType.Item, -3);
                    }
                    else if (c == "m")
                    {
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.MyInfo.Code = 204;
                    }
                    else if (int.TryParse(c, out g))
                    {
                        if (g < 0) continue;
                        candy = CandyPool.Dequeue().GetComponent<Candy>();
                        candy.transform.ResetTransform(transform);
                        candy.Set((Candy.CandyType)(g / 6), g % 6);
                    }
                    else continue;

                    candy.MyGrid = this;
                    candy.Refresh();
                    candy.transform.localPosition = GetCellPosition(i, j);
                    this[i, j].MyCandy = candy;
                    candy.CurIJ = new IntVector2(i, j);
                    candy.State = Candy.CandyState.Static;
                }
            }
        }

        private readonly List<Candy> totalSpecialPopList = new List<Candy>();
        private readonly List<Candy> firedBombs = new List<Candy>();

        public void StartRound()
        {
            _canControl = true;
            enabled = true;
            ResetComboAmountZero();
        }
        public void EndRound()
        {
            _canControl = false;
        }

        private void Update()
        {
            #region 首行投放

            for (int j = 0; j < Width; j++)
            {
                var i = 0;
                while (this[i,j].MyState == Cell.State.Null)
                {
                    i++;
                    if (!IsIJInGrid(i, j)) break;
                }
                if (!IsIJInGrid(i,j)) continue;
                if (this[i, j].CanHoldCandy && this[i, j].MyCandy == null)
                {
                    var candy = CandyPool.Dequeue().GetComponent<Candy>();
                    candy.transform.ResetTransform(transform);

                    candy.MyGrid = this;
                    if (DropQueueList[j].Count > 0)
                    {
                        candy.MyInfo = DropQueueList[j].Dequeue();
                    }
                    else
                    {
                        if (GameData.LastSubLevelData != null && GameData.LastSubLevelData.HasHealthBottleProbability &&
                            Random.value < GameData.LastSubLevelData.HealthBottleProbability*0.0001f)
                        {
                            candy.MyInfo = new CandyInfo(202)
                                {
                                    FiredCandyExtraData =
                                        GameData.LastSubLevelData.HasHealthBottleCapacity
                                            ? GameData.LastSubLevelData.HealthBottleCapacity
                                            : 0
                                };
                        }
                        else if (GameData.LastSubLevelData != null &&
                                 GameData.LastSubLevelData.HasEnergyBottleProbability &&
                                 Random.value < GameData.LastSubLevelData.EnergyBottleProbability*0.0001f)
                        {
                            candy.MyInfo = new CandyInfo(203)
                                {
                                    FiredCandyExtraData =
                                        GameData.LastSubLevelData.HasEnergyBottleCapacity
                                            ? GameData.LastSubLevelData.EnergyBottleCapacity
                                            : 0
                                };
                        }
                        else if (GameData.LastSubLevelData != null && GameData.LastSubLevelData.HasCoinBagProbability &&
                                 Random.value < GameData.LastSubLevelData.CoinBagProbability*0.0001f)
                        {
                            candy.MyInfo = new CandyInfo(204)
                                {
                                    FiredCandyExtraData =
                                        GameData.LastSubLevelData.HasCoinBagCapacity
                                            ? GameData.LastSubLevelData.CoinBagCapacity
                                            : 0
                                };
                        }
                        else
                        {
                            candy.MyInfo = CandyInfo.GetRandom(GameManager.CurCandyGenreCount);
                        }
                    }
                    candy.Refresh();
                    candy.transform.localPosition = GetCellPosition(i - 1, j);
                    this[i, j].MyCandy = candy;
                    candy.FromIJ.i = i - 1;
                    candy.FromIJ.j = j;
                    candy.CurIJ.i = i;
                    candy.CurIJ.j = j;
                    candy.State = Candy.CandyState.Falling;
                }
            }

            #endregion

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    IntVector2 ij;
                    ij.i = i;
                    ij.j = j;
                    if (this[ij].MyCandy) this[ij].MyCandy.UpdateMoving();
                }
            }

            #region Popping

            //其实是一瞬间的事

            totalSpecialPopList.Clear(); //所有要消掉的Special
            firedBombs.Clear();

            //var isColorfulOrSpecialFusion = false;
            if (_finishedExchanges != null)
            {
                #region 有完成的交换

                foreach (var finishedExchange in _finishedExchanges)
                {
                    if (finishedExchange.MainCell == finishedExchange.SubCell)
                    {
                        Debug.LogError("怎么交换的两个是同一个格子的:" + finishedExchange.MainCell);
                        continue;
                    }
                    if (!this[finishedExchange.MainCell].MyCandy || !this[finishedExchange.SubCell])
                    {
                        Debug.LogError("怎么交换完了两个还是空格");
                        continue;
                    }

                    //检测是否特殊融合
                    //isColorfulOrSpecialFusion = true; //提前设为true，要不然每个if里面都要写
                    ExchangeInfo exchangeInfo = finishedExchange;
                    Action pop2FusionCandys = () =>
                        {
                            if (this[exchangeInfo.MainCell].MyCandy != null)
                            {
                                this[exchangeInfo.MainCell].MyCandy.Pop(0);
                                this[exchangeInfo.MainCell].MyCandy = null;
                            }
                            else
                            {
                                Debug.LogError("不都有Candy怎么融合");
                            }
                            if (this[exchangeInfo.SubCell].MyCandy != null)
                            {
                                this[exchangeInfo.SubCell].MyCandy.Pop(0);
                                this[exchangeInfo.SubCell].MyCandy = null;
                            }
                            else
                            {
                                Debug.LogError("不都有Candy怎么融合");
                            }
                        }; //把融合的两个糖果Pop掉，之后的特殊消除里不再包含。这个操作要复用所以用Action
                    if (this[exchangeInfo.MainCell].MyCandy.IsStripe && this[exchangeInfo.SubCell].MyCandy.IsStripe) //条纹-条纹
                    {
                        //特效，在pop之前才能知道颜色哦
                        GameManager.PopEffectContainer.StripeHSpecialPopEffect(exchangeInfo.MainCell, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.StripeVSpecialPopEffect(exchangeInfo.MainCell, this[exchangeInfo.MainCell].MyCandy.Genre);

                        pop2FusionCandys();

                        #region 十字直消

                        var center = exchangeInfo.MainCell; //松开点为十字消除中心
                        var ijs = new[]
                            {
                                new IntVector2(), new IntVector2(),
                                new IntVector2(), new IntVector2()
                            };
                        for (int d = 1; d < Mathf.Max(Width, Height); d++) //d：到中心的int距离
                        {
                            ijs[0].i = center.i + d;
                            ijs[0].j = center.j;
                            ijs[1].i = center.i - d;
                            ijs[1].j = center.j;
                            ijs[2].i = center.i;
                            ijs[2].j = center.j + d;
                            ijs[3].i = center.i;
                            ijs[3].j = center.j - d;

                            foreach (var ij in ijs.Where(IsIJInGrid))
                            {
                                this[ij].BePopped();
                                var candy = this[ij].MyCandy;
                                if (candy == null || !candy.CanBeSpecialPopped) continue;
                                if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                candy.Pop(ExchangeSuccessTime + DelayPerCellOfStripe*d);
                            }
                        }

                        #endregion
                    }
                    else if ((this[exchangeInfo.MainCell].MyCandy.IsStripe &&
                              this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Bomb) ||
                             ((this[exchangeInfo.SubCell].MyCandy.IsStripe &&
                               this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Bomb)))
                        //条纹-炸弹||炸弹-条纹
                    {
                        //特效，在pop之前才能知道颜色哦
                        GameManager.PopEffectContainer.StripeHSpecialPopEffect(exchangeInfo.MainCell, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.StripeHSpecialPopEffect(exchangeInfo.MainCell + IntVector2.up, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.StripeHSpecialPopEffect(exchangeInfo.MainCell - IntVector2.up, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.StripeVSpecialPopEffect(exchangeInfo.MainCell, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.StripeVSpecialPopEffect(exchangeInfo.MainCell + IntVector2.right, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.StripeVSpecialPopEffect(exchangeInfo.MainCell - IntVector2.right, this[exchangeInfo.MainCell].MyCandy.Genre);

                        pop2FusionCandys();

                        #region 十字宽直消

                        var center = exchangeInfo.MainCell; //松开点为十字消除中心
                        for (int k = 0; k < 9; k++) //先消除中心3*3区域
                        {
                            if (k == 4) continue;
                            IntVector2 ij;
                            ij.i = center.i + k/3 - 1;
                            ij.j = center.j + k%3 - 1;

                            if (!IsIJInGrid(ij)) continue;
                            this[ij].BePopped();
                            var candy = this[ij].MyCandy;
                            if (candy == null || !candy.CanBeSpecialPopped) continue;
                            if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                            candy.Pop(ExchangeSuccessTime);
                        }
                        var ijs = new IntVector2[12];
                        for (int d = 2; d < Mathf.Max(Width, Height); d++) //消除3*3之外的4个带状区域 d：到中心的int距离
                        {
                            for (int k = 0; k <= 2; k++) //宽十字延伸
                            {
                                var x = k - 1;
                                ijs[k].i = center.i - d;
                                ijs[k].j = center.j + x;
                                ijs[3 + k].i = center.i + x;
                                ijs[3 + k].j = center.j + d;
                                ijs[6 + k].i = center.i + d;
                                ijs[6 + k].j = center.j - x;
                                ijs[9 + k].i = center.i - x;
                                ijs[9 + k].j = center.j - d;
                            }
                            foreach (var ij in ijs.Where(IsIJInGrid))
                            {
                                this[ij].BePopped();
                                var candy = this[ij].MyCandy;
                                if (candy == null || !candy.CanBeSpecialPopped) continue;
                                if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                candy.Pop(ExchangeSuccessTime + DelayPerCellOfStripe*d + 0.2f); //延时0.7秒再开始延伸
                            }
                        }

                        #endregion
                    }
                    else if (this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Bomb &&
                             this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Bomb) //炸弹-炸弹
                    {
                        var recordGenres = new[]
                            {
                                this[exchangeInfo.MainCell].MyCandy.Genre,
                                this[exchangeInfo.SubCell].MyCandy.Genre
                            };
                        //记录颜色

                        //特效，在pop之前才能知道颜色哦
                        GameManager.PopEffectContainer.Bomb5SpecialPopEffect(exchangeInfo.MainCell, this[exchangeInfo.MainCell].MyCandy.Genre);
                        GameManager.PopEffectContainer.Bomb5SpecialPopEffect(exchangeInfo.SubCell, this[exchangeInfo.MainCell].MyCandy.Genre);

                        pop2FusionCandys();

                        #region 5*6消除

                        var array = new [] {exchangeInfo.MainCell, exchangeInfo.SubCell};
                        for (int index = 0; index < array.Length; index++)
                        {
                            var lastExchangeCell = array[index];
                            for (int k = 0; k < 25; k++)
                            {
                                IntVector2 ij;
                                ij.i = lastExchangeCell.i + (k/5) - 2;
                                ij.j = lastExchangeCell.j + (k%5) - 2;
                                if (!IsIJInGrid(ij)) continue;
                                this[ij].BePopped();
                                var candy = this[ij].MyCandy;
                                if (candy == null || !candy.CanBeSpecialPopped) continue;
                                if (candy.IsSpecial)
                                {
                                    totalSpecialPopList.Add(candy);
                                }
                                candy.Pop(ExchangeSuccessTime);
                            }
                            //生成FiredBomb
                            var firedBombCandy = CandyPool.Dequeue().GetComponent<Candy>();
                            firedBombCandy.transform.ResetTransform(transform);
                            firedBombCandy.MyGrid = this;
                            firedBombCandy.name = "Fired Bomb Candy " + lastExchangeCell;
                            firedBombCandy.Set(Candy.CandyType.Bomb, recordGenres[index]);
                            firedBombCandy.BecomeFired();
                            firedBombCandy.FiredBombRange = 5;
                            firedBombCandy.Refresh();
                            firedBombCandy.transform.localPosition = GetCellPosition(lastExchangeCell);
                            firedBombCandy.CurIJ = lastExchangeCell;
                            firedBombCandy.State = Candy.CandyState.Static;
                            firedBombs.Add(firedBombCandy);
                        }

                        #endregion
                    }
                    else if ((this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Colorful &&
                              this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Normal) ||
                             (this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Colorful &&
                              this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Normal)) //普通-彩色
                    {
                        var normalCandyIJ = this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Normal
                                                ? exchangeInfo.MainCell
                                                : exchangeInfo.SubCell;
                        //普通糖的ij
                        var genre = this[normalCandyIJ].MyCandy.Genre; //消除的种类

                        pop2FusionCandys();

                        #region 一色消除
                        
                        var list = new List<IntVector2>();//记录除交换的两个的所有被消除的同色糖果格子，用于显示散落特效
                        for (int i0 = 0; i0 < Height; i0++)
                        {
                            for (int j0 = 0; j0 < Width; j0++)
                            {
                                IntVector2 ij;
                                ij.i = i0;
                                ij.j = j0;

                                var candy = this[ij].MyCandy;
                                if (candy == null || candy.Genre != genre) continue;
                                this[ij].BePopped();
                                if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                candy.Pop(0.4f);

                                if (ij != exchangeInfo.MainCell && ij != exchangeInfo.SubCell)
                                {
                                    list.Add(ij);
                                }
                            }
                        }
                        GameManager.Instance.PopEffectContainer.ColorfulScatterEffect(exchangeInfo.MainCell, genre, list);

                        #endregion
                    }
                    else if ((this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Colorful &&
                              this[exchangeInfo.SubCell].MyCandy.IsStripe) ||
                             (this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Colorful &&
                              this[exchangeInfo.MainCell].MyCandy.IsStripe))
                        //条纹-彩色
                    {
                        var normalCandyIJ = this[exchangeInfo.MainCell].MyCandy.IsStripe
                                                ? exchangeInfo.MainCell
                                                : exchangeInfo.SubCell;
                        //条纹糖的ij
                        var genre = this[normalCandyIJ].MyCandy.Genre; //消除的种类

                        pop2FusionCandys();

                        #region 一色转换成条纹并消除

                        var list = new List<IntVector2>();//记录除交换的两个的所有被消除的同色糖果格子，用于显示散落特效
                        for (int i0 = 0; i0 < Height; i0++)
                        {
                            for (int j0 = 0; j0 < Width; j0++)
                            {
                                IntVector2 ij;
                                ij.i = i0;
                                ij.j = j0;

                                var candy = this[ij].MyCandy;
                                if (candy == null || candy.Genre != genre || !candy.CanBeSpecialPopped || candy.MyType != Candy.CandyType.Normal) continue;
                                candy.Set(Random.value < 0.5f ? Candy.CandyType.H : Candy.CandyType.V, candy.Genre);
                                //转换Type，随机HV
                                candy.Refresh();
                                candy.BecomeFired(GameData.DefaultFiredWaitingTime + list.Count * 0.1f);
                                if (ij != exchangeInfo.MainCell && ij != exchangeInfo.SubCell)
                                {
                                    list.Add(ij);
                                }
                            }
                        }
                        GameManager.Instance.PopEffectContainer.ColorfulScatterEffect(exchangeInfo.MainCell, genre, list);

                        #endregion

                    }
                    else if ((this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Bomb || this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Bomb) &&
                             (this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Colorful || this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Colorful))
                        //炸弹-彩色
                    {
                        var normalCandyIJ = this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Bomb
                                                ? exchangeInfo.MainCell
                                                : exchangeInfo.SubCell;
                        //条纹糖的ij
                        var genre = this[normalCandyIJ].MyCandy.Genre; //消除的种类

                        pop2FusionCandys();

                        #region 完全仿造CC，我就是可以！

                        var list = new List<IntVector2>();//记录所有被消除的同色糖果格子，用于显示散落特效
                        for (int i0 = 0; i0 < Height; i0++)
                        {
                            for (int j0 = 0; j0 < Width; j0++)
                            {
                                IntVector2 ij;
                                ij.i = i0;
                                ij.j = j0;
                                var candy = this[ij].MyCandy;
                                if (candy == null || (candy.Genre != genre)) continue;
                                this[ij].BePopped();
                                if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                candy.Pop(ExchangeSuccessTime);
                                list.Add(ij);
                            }
                        }
                        GameManager.Instance.PopEffectContainer.ColorfulScatterEffect(exchangeInfo.MainCell, genre, list);

                        //生成Fired Colorful
                        var firedColorfulCandy = CandyPool.Dequeue().GetComponent<Candy>();
                        firedColorfulCandy.transform.ResetTransform(transform);
                        firedColorfulCandy.MyGrid = this;
                        firedColorfulCandy.name = "Fired Colorful Candy " + exchangeInfo.MainCell;
                        firedColorfulCandy.Set(Candy.CandyType.Colorful, -1);
                        firedColorfulCandy.BecomeFired();
                        firedColorfulCandy.ColorfulRandomExclude = genre;
                        firedColorfulCandy.Refresh();
                        firedColorfulCandy.transform.localPosition = GetCellPosition(exchangeInfo.MainCell);
                        firedColorfulCandy.CurIJ = exchangeInfo.MainCell;
                        firedColorfulCandy.State = Candy.CandyState.Static;
                        firedBombs.Add(firedColorfulCandy);

                        #endregion
                    }
                    else if (this[exchangeInfo.MainCell].MyCandy.MyType == Candy.CandyType.Colorful && this[exchangeInfo.SubCell].MyCandy.MyType == Candy.CandyType.Colorful)
                        //彩色-彩色
                    {
                        GameManager.PopEffectContainer.Colorful_ColorfulEffect(exchangeInfo.MainCell);

                        pop2FusionCandys();

                        #region 全部消除，这个简单，哈哈！

                        for (int i0 = 0; i0 < Height; i0++)
                        {
                            for (int j0 = 0; j0 < Width; j0++)
                            {
                                IntVector2 ij;
                                ij.i = i0;
                                ij.j = j0;

                                this[ij].BePopped();
                                var candy = this[ij].MyCandy;
                                if (candy == null || !candy.CanBeSpecialPopped) continue; //石头竟然还在
                                candy.Pop(ExchangeSuccessTime*ij.j*0.05f); //TODO:这里有个常数
                            }
                        }

                        #endregion
                    }
                    else //不是融合，就不在这里处理了
                    {
                        //isColorfulOrSpecialFusion = false;
                    }
                }

                #endregion
            }

            #region 监视可消除集合是否发生了变化，变化需要标注出来，之后调用开销很大的东西

            bool someChangedSinceLastFrame = false; //需要检测三消if true；false时不需要再检测提示和死局

            var curCellRecord = new CellRecord(); //为了减少垃圾
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    IntVector2 ij;
                    ij.i = i;
                    ij.j = j;

                    if (!someChangedSinceLastFrame)
                    {
                        curCellRecord.HasStaticCandy = this[ij].MyCandy && this[ij].MyCandy.IsStatic;
                        if (curCellRecord.HasStaticCandy)
                        {
                            curCellRecord.CandyType = this[ij].MyCandy.MyType;
                            curCellRecord.CandyGenre = this[ij].MyCandy.Genre;
                        }
                        if (CellRecord.HasChanged(_cellRecords[ij.i][ij.j], curCellRecord))
                        {
                            someChangedSinceLastFrame = true;
                        }
                    }

                    _cellRecords[ij.i][ij.j].HasStaticCandy = this[ij].MyCandy && this[ij].MyCandy.IsStatic;
                    if (_cellRecords[ij.i][ij.j].HasStaticCandy)
                    {
                        _cellRecords[ij.i][ij.j].CandyType = this[ij].MyCandy.MyType;
                        _cellRecords[ij.i][ij.j].CandyGenre = this[ij].MyCandy.Genre;
                    }
                }
            }

            #endregion

            #region 三消，寻找消除域

            if (someChangedSinceLastFrame)
            {
                var canPopArray = GetBoolGrid();
                var hasSearchedArray = GetBoolGrid();
                var maybeS5Array = GetBoolGrid();
                var specialRespawnCell = IntVector2.zero;//减少垃圾
                var curGenrePopList = new List<IntVector2>();//此连通域转换成列表表示，减少垃圾
                for (int i0 = 0; i0 < Height; i0++)
                {
                    for (int j0 = 0; j0 < Width; j0++)
                    {
                        IntVector2 ij;
                        ij.i = i0;
                        ij.j = j0;

                        if (!this[ij].CanHoldCandy) continue;

                        var candy = this[ij].MyCandy;
                        if (candy == null) continue;

                        //有Candy

                        if (!candy.IsStatic || candy.Genre < 0) continue;

                        ResetBoolGrid(canPopArray);
                        ResetBoolGrid(hasSearchedArray);
                        SearchPop(ij, canPopArray, hasSearchedArray);
                        //至此，canPopArray就是这一点出发的三消连通域

                        curGenrePopList.Clear(); //计算新的连通域，此连通域转换成列表表示
                        for (int i1 = 0; i1 < Height; i1++)
                        {
                            for (int j1 = 0; j1 < Width; j1++)
                            {
                                if (canPopArray[i1][j1])
                                {
                                    IntVector2 ij1;
                                    ij1.i = i1;
                                    ij1.j = j1;
                                    curGenrePopList.Add(ij1);
                                }
                            }
                        }

                        #region 计算生成的特殊方块

                        Candy newCandy = null;

                        if (curGenrePopList.Count < 3) //没消除
                        {
                            if (curGenrePopList.Count != 0)
                                Debug.LogError("curGenrePopList.Count==" + curGenrePopList.Count + " 这还是三消吗");
                            continue;
                        }
                        else if (curGenrePopList.Count == 3) //单3
                        {
                        }
                        else
                        {
                            if (curGenrePopList.Count == 4) //直4
                            {
                                if (_finishedExchanges.Count > 0)
                                {
                                    var ind = curGenrePopList.FindIndex(x => _finishedExchanges.Exists(ex=>ex.MainCell == x || ex.SubCell == x));//寻找一个刚刚参与交换的格点
                                    if (ind >= 0) specialRespawnCell = curGenrePopList[ind];
                                    else specialRespawnCell = curGenrePopList[Random.Range(0, curGenrePopList.Count)];
                                }
                                else
                                {
                                    specialRespawnCell = curGenrePopList[Random.Range(0, curGenrePopList.Count)];
                                }
                                newCandy = CandyPool.Dequeue().GetComponent<Candy>();
                                newCandy.transform.ResetTransform(transform);
                                newCandy.Set(
                                    curGenrePopList[0].i == curGenrePopList[1].i ? Candy.CandyType.V : Candy.CandyType.H,
                                    candy.Genre);//横4出竖条纹，竖4出横条纹
                                newCandy.State = Candy.CandyState.Static;
                            }
                            else //>= 5
                            {
                                #region 检测是否有S5，有则填上newCandy

                                ResetBoolGrid(maybeS5Array);
                                foreach (var ij1 in curGenrePopList)
                                {
                                    maybeS5Array[ij1.i][ij1.j] = true;
                                }

                                for (int i1 = 0; i1 < Height && newCandy == null; i1++)
                                {
                                    for (int j1 = 0; j1 < Width && newCandy == null; j1++)
                                    {
                                        if (maybeS5Array[i1][j1]) //验证往右5个和往下5个┏
                                        {
                                            var allV5CanPop = true;
                                            var allH5CanPop = true;
                                            for (int k1 = 0; k1 < 5; k1++) //验证竖着
                                            {
                                                var k = i1 + k1;
                                                if (k >= Height || !maybeS5Array[k][j1])
                                                {
                                                    allV5CanPop = false;
                                                    break;
                                                }
                                            }
                                            for (int k1 = 0; k1 < 5; k1++) //验证横着
                                            {
                                                var k = j1 + k1;
                                                if (k >= Width || !maybeS5Array[i1][k])
                                                {
                                                    allH5CanPop = false;
                                                    break;
                                                }
                                            }
                                            if (allV5CanPop || allH5CanPop) //直5
                                            {
                                                //中的那个生成彩色糖
                                                if (allV5CanPop)
                                                {
                                                    specialRespawnCell.i = i1 + 2;
                                                    specialRespawnCell.j = j1;
                                                }
                                                else
                                                {
                                                    specialRespawnCell.i = i1;
                                                    specialRespawnCell.j = j1 + 2;
                                                }

                                                newCandy = CandyPool.Dequeue().GetComponent<Candy>();
                                                newCandy.transform.ResetTransform(transform);
                                                newCandy.Set(Candy.CandyType.Colorful, -1);
                                                newCandy.MyInfo.FiredCandyExtraData = -1;
                                                newCandy.State = Candy.CandyState.Static;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                if (!newCandy) //没有S5，则是LT5，生成位置应该是拐弯点
                                {
                                    foreach (var ij1 in curGenrePopList) //寻找拐点
                                    {
                                        if (((ij1.i + 1 >= Height || !canPopArray[ij1.i + 1][ij1.j]) &&
                                             (ij1.i - 1 < 0 || !canPopArray[ij1.i - 1][ij1.j])) ||
                                            ((ij1.j + 1 >= Width || !canPopArray[ij1.i][ij1.j + 1]) &&
                                             (ij1.j - 1 < 0 || !canPopArray[ij1.i][ij1.j - 1])))
                                            continue; //如果上，下都不是同色或左，右都不是同色，则不是拐点
                                        specialRespawnCell = ij1;
                                        break;
                                    }

                                    newCandy = CandyPool.Dequeue().GetComponent<Candy>();
                                    newCandy.transform.ResetTransform(transform);
                                    newCandy.Set(Candy.CandyType.Bomb, candy.Genre);
                                    newCandy.MyInfo.FiredCandyExtraData = 3;
                                    newCandy.State = Candy.CandyState.Static;
                                }
                            }
                        }

                        #endregion

                        #region 消除应该消除的同色方块

                        foreach (var ij1 in curGenrePopList)
                        {
                            this[ij1].BePopped();//告诉Cell被消除了
                            var cell = this[ij1];
                            if (cell.MyCandy == null)
                            {
                                Debug.LogError("MyCandy == null!");
                                continue;
                            }
                            if (cell.MyCandy.IsSpecial) totalSpecialPopList.Add(cell.MyCandy);
                            cell.MyCandy.Pop(0);
                        }

                        #endregion

                        #region 计算相邻影响格（边界），用于清除砖块

                        var adjacentCellArray = canPopArray;//借用一个阵列，减少垃圾
                        ResetBoolGrid(adjacentCellArray);
                        foreach (var popCell in curGenrePopList)
                        {
                            var ij1 = popCell + new IntVector2(0, -1);
                            if (IsIJInGrid(ij1)) adjacentCellArray[ij1.i][ij1.j] = true;
                            ij1 = popCell + new IntVector2(0, 1);
                            if (IsIJInGrid(ij1)) adjacentCellArray[ij1.i][ij1.j] = true;
                            ij1 = popCell + new IntVector2(-1, 0);
                            if (IsIJInGrid(ij1)) adjacentCellArray[ij1.i][ij1.j] = true;
                            ij1 = popCell + new IntVector2(1, 0);
                            if (IsIJInGrid(ij1)) adjacentCellArray[ij1.i][ij1.j] = true;
                        }
                        for (int i1 = 0; i1 < Height; i1++)
                        {
                            for (int j1 = 0; j1 < Width; j1++)
                            {
                                if (adjacentCellArray[i1][j1])
                                {
                                    IntVector2 ij1;
                                    ij1.i = i1;
                                    ij1.j = j1;
                                    this[ij1].AdjacentBeThreePopped();//告诉Cell邻格被正常消除了，如果是砖块，就消掉砖块
                                }
                            }
                        }

                        #endregion

                        if (newCandy) //如果生成了新的特殊糖果，放进格子里，(在消除同色方块之后，防止刚生成就被删除)
                        {
                            newCandy.Refresh();
                            newCandy.MyGrid = this;
                            this[specialRespawnCell].MyCandy = newCandy;
                            newCandy.transform.localPosition = GetCellPosition(specialRespawnCell);
                            newCandy.CurIJ = specialRespawnCell;
                            newCandy.State = Candy.CandyState.Static;
                        }

                    }
                }
            }

            #endregion

            #region 自爆

            List<IntVector2> curBombZone = null;//确定被波及的格子//减少垃圾
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    IntVector2 ij;
                    ij.i = i;
                    ij.j = j;

                    if (this[ij].CanHoldCandy && this[ij].MyCandy && this[ij].MyCandy.IsStatic
                        && this[ij].MyCandy.Fired && this[ij].MyCandy.ShouldFiredCandyPopNow) //挨个处理到时间的自爆炸弹
                    {
                        this[ij].MyCandy.Pop(0);
                        totalSpecialPopList.Add(this[ij].MyCandy);
                        this[ij].MyCandy = null;
                    }
                }
            }

            #endregion

            #region 特殊消除

            IntVector2[] tmpHvList = null;
            for (int index = 0; index < totalSpecialPopList.Count; index++)
            {
                var popCandy = totalSpecialPopList[index];
                if (!popCandy.IsSpecial) continue;
                switch (popCandy.MyType)
                {
                    case Candy.CandyType.H:

                        #region H

                        {
                            if (tmpHvList == null) tmpHvList = new IntVector2[2];
                            for (int k = 0; k < Width; k++)
                            {
                                tmpHvList[0].i = popCandy.CurIJ.i;
                                tmpHvList[0].j = popCandy.CurIJ.j + k;

                                tmpHvList[1].i = popCandy.CurIJ.i;
                                tmpHvList[1].j = popCandy.CurIJ.j - k;

                                foreach (var ij in tmpHvList)
                                {
                                    if (!IsIJInGrid(ij)) continue;
                                    this[ij].BePopped();
                                    var candy = this[ij].MyCandy;
                                    if (candy == null || !candy.CanBeSpecialPopped) continue;
                                    if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                    candy.Pop(popCandy.PopExtraDelay + k*DelayPerCellOfStripe);
                                }
                            }
                            GameManager.PopEffectContainer.StripeHSpecialPopEffect(popCandy.CurIJ, popCandy.Genre);
                        }
                        break;

                        #endregion

                    case Candy.CandyType.V:

                        #region V

                        {
                            if (tmpHvList == null) tmpHvList = new IntVector2[2];
                            for (int k = 0; k < Height; k++)
                            {
                                tmpHvList[0].i = popCandy.CurIJ.i + k;
                                tmpHvList[0].j = popCandy.CurIJ.j;

                                tmpHvList[1].i = popCandy.CurIJ.i - k;
                                tmpHvList[1].j = popCandy.CurIJ.j;

                                foreach (var ij in tmpHvList)
                                {
                                    if (!IsIJInGrid(ij)) continue;
                                    this[ij].BePopped();
                                    var candy = this[ij].MyCandy;
                                    if (candy == null || !candy.CanBeSpecialPopped) continue;
                                    if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                    candy.Pop(popCandy.PopExtraDelay + k * DelayPerCellOfStripe);
                                }
                            }
                            GameManager.PopEffectContainer.StripeVSpecialPopEffect(popCandy.CurIJ, popCandy.Genre);
                        }
                        break;

                        #endregion

                    case Candy.CandyType.Bomb:

                        #region 炸弹

                        {
                            if (curBombZone == null)
                            {
                                curBombZone = new List<IntVector2>();
                            }
                            else
                            {
                                curBombZone.Clear();
                            }
                            var bombRange = popCandy.FiredBombRange; //考虑3,5等各种威力的自爆炸弹
                            for (int k = 0; k < bombRange * bombRange; k++)
                            {
                                IntVector2 ij1;
                                ij1.i = popCandy.CurIJ.i + k / bombRange - (bombRange / 2);
                                ij1.j = popCandy.CurIJ.j + k % bombRange - (bombRange / 2);

                                if (!IsIJInGrid(ij1)) continue;
                                curBombZone.Add(ij1);
                            }

                            #region 波及区域消除

                            foreach (var ij in curBombZone)
                            {
                                this[ij].BePopped();
                                var cell = this[ij];
                                var candy = cell.MyCandy;
                                if (candy == null || !candy.CanBeSpecialPopped) continue;
                                if (candy.IsSpecial) //包括自爆炸弹的特殊方块
                                {
                                    totalSpecialPopList.Add(candy);
                                }
                                candy.Pop(popCandy.PopExtraDelay);
                            }

                            #endregion

                            if (!popCandy.Fired)//不是点燃炸弹就生成一个次生点燃的炸弹
                            {
                                //生成Fired Bomb
                                var firedBombCandy = CandyPool.Dequeue().GetComponent<Candy>();
                                firedBombCandy.transform.ResetTransform(transform);
                                firedBombCandy.MyGrid = this;
                                firedBombCandy.name = "Fired Bomb Candy " + popCandy.CurIJ;
                                firedBombCandy.Set(Candy.CandyType.Bomb, popCandy.Genre);
                                firedBombCandy.BecomeFired();
                                firedBombCandy.FiredBombRange = 3;
                                firedBombCandy.Refresh();
                                firedBombCandy.transform.localPosition = GetCellPosition(popCandy.CurIJ);
                                firedBombCandy.CurIJ = popCandy.CurIJ;
                                firedBombCandy.State = Candy.CandyState.Static;
                                firedBombs.Add(firedBombCandy);

                            }
                            if (popCandy.FiredBombRange < 4)
                            {
                                GameManager.Instance.PopEffectContainer.Bomb3SpecialPopEffect(popCandy.CurIJ, popCandy.Genre);
                            }
                            else
                            {
                                GameManager.Instance.PopEffectContainer.Bomb5SpecialPopEffect(popCandy.CurIJ, popCandy.Genre);
                            }
                        }
                        break;

                        #endregion

                    case Candy.CandyType.Colorful:

                        #region 随机一色消除

                        {
                            IntVector2 tmpij;
                            int genre = 0;
                            var tryTimes = 0;
                            while (true) //随机找一个方块。尝试超过30次则用0，防止死循环
                            {
                                tryTimes++;
                                if (tryTimes > 30){Debug.LogWarning("超过随机次数了，默认使用Genre0"); break;}
                                tmpij.i = Random.Range(0, Height);
                                tmpij.j = Random.Range(0, Width);
                                if (this[tmpij].MyCandy && this[tmpij].MyCandy.Genre >= 0 &&
                                    this[tmpij].MyCandy.Genre != popCandy.ColorfulRandomExclude)
                                {
                                    genre = this[tmpij].MyCandy.Genre;
                                    break;
                                }
                            }

                            var list = new List<IntVector2>();//记录所有被消除的同色糖果格子，用于显示散落特效
                            for (int i0 = 0; i0 < Height; i0++)
                            {
                                for (int j0 = 0; j0 < Width; j0++)
                                {
                                    IntVector2 ij;
                                    ij.i = i0;
                                    ij.j = j0;

                                    var candy = this[ij].MyCandy;
                                    if (candy == null || candy.Genre != genre || !candy.CanBeSpecialPopped) continue;
                                    this[ij].BePopped();
                                    if (candy.IsSpecial) totalSpecialPopList.Add(candy);
                                    candy.Pop(popCandy.PopExtraDelay);
                                    list.Add(ij);
                                }
                            }

                            GameManager.Instance.PopEffectContainer.ColorfulScatterEffect(popCandy.CurIJ, genre, list);
                        }

                        #endregion

                        break;
                    default:
                        Debug.LogError("不可能的CandyType");
                        break;
                }

            }

            foreach (Candy t in firedBombs)
            {
                this[t.CurIJ].MyCandy = t;
            }
            #endregion

            #endregion

            #region 检测死局

            if (someChangedSinceLastFrame)
            {
                var isAllStatic = true;
                for (int i0 = 0; i0 < Height; i0++)
                {
                    for (int j0 = 0; j0 < Width; j0++)
                    {
                        IntVector2 ij;
                        ij.i = i0;
                        ij.j = j0;
                        var curCandy = this[ij].MyCandy;
                        if (curCandy &&
                            (curCandy.State != Candy.CandyState.Static || curCandy.MyType == Candy.CandyType.Item ||
                             curCandy.Fired))
                        {
                            isAllStatic = false;
                            break;
                        }
                    }
                }
                
                //if (shouldCheckDead) shouldCheckDead = !Cells[0].Any(x => x.CanHoldCandy && x.MyCandy == null); 旧的不严谨
                if (isAllStatic) isAllStatic = Cells.All(x =>
                {
                    var firstCanHoldCandy = x.FirstOrDefault(y => y.CanHoldCandy);
                    if (firstCanHoldCandy != null) return firstCanHoldCandy.MyCandy != null;//此列最高的CanHoldCandy格子有Candy，则true，否则false
                    return true;//此列没有CanHoldeCandy的格子
                });//每一列的最上面的CanHoldCandy的格子都不是空着的

                //如果所有糖果都静止了，没有任何道具糖果，没有任何点燃糖果，顶部都没有空着，才需要检测
                if (isAllStatic)
                {
                    if (GameManager.AI != null)
                    {
                        var hintList = GameManager.AI.GetAllHintExchanges(this, true); //只要有一个即可判断是否死局
                        if (hintList.Count == 0)
                        {
                            //TODO:死局
                            Debug.LogWarning("死局@" + Time.frameCount);
                            Shuffle();
                        }
                    }

                    ResetComboAmountZero();//Combo归0
                }
            }

            #endregion

            if (_IsGoldenFingerTime)//金手指
            {
                var touchedPoss = new List<Vector2>();
                if (Input.GetMouseButton(0))
                {
                    var curLocalTouPos =
                        transform.InverseTransformPoint(ControlCamera.ScreenToWorldPoint(Input.mousePosition))
                                 .ToVector2();
                    if (GameManager.SkillMaskContainer.CanTouchThroughAllMasks(curLocalTouPos))
                    {
                        touchedPoss.Add(curLocalTouPos);
                    }
                }
                if (Input.touchCount > 0)
                {
                    touchedPoss.AddRange(
                        Input.touches.Where(x => x.phase != TouchPhase.Ended && x.phase != TouchPhase.Canceled)
                             .Select(
                                 touch =>
                                 transform.InverseTransformPoint(ControlCamera.ScreenToWorldPoint(touch.position))
                                          .ToVector2())
                             .Where(
                                 curLocalTouPos =>
                                 GameManager.SkillMaskContainer.CanTouchThroughAllMasks(curLocalTouPos)));
                }
                foreach (var ij in from vector2 in touchedPoss
                                   select GetCellIJ(vector2)
                                   into ij where IsIJInGrid(ij) where this[ij].MyCandy
                                   where
                                       this[ij].MyCandy.State == Candy.CandyState.Falling ||
                                       this[ij].MyCandy.State == Candy.CandyState.Static select ij)
                {
                    this[ij].MyCandy.Pop(0);
                    this[ij].MyCandy = null;
                }
            }
            else
            {
                UpdateControlling();//三消操作
            }


            _finishedExchanges.Clear();//清空。这个东西只在这一帧有用

            #region 测试接口
            //测试提示
            if (Input.GetKeyUp(KeyCode.H))
            {
                var hintList = GameManager.Instance.AI.GetAllHintExchanges(this);
                Debug.Log("Hint.Count:" + hintList.Count + "\n" +
                          hintList.Select(x => string.Format("({0}-{1})", x.IJ, x.IJ1)).Aggregate((c, x) => c + x + ","));
                if (hintList.Count > 0)
                {
                    var index = Random.Range(0, hintList.Count);
                    //foreach (var candyExchange in hintList)
                    //{
                    var candyExchange = hintList[index];
                    Debug.LogWarning(candyExchange.IJ + "-" + candyExchange.IJ1);
                    this[candyExchange.IJ].MyCandy.CushionAnimation.Play();
                    this[candyExchange.IJ1].MyCandy.CushionAnimation.Play();
                    //foreach (var cellWillPop in candyExchange.SameGenreCells)
                    //{
                    //    var ta = this[cellWillPop].MyCandy.GetComponent<TweenAlpha>() ??
                    //             this[cellWillPop].MyCandy.gameObject.AddComponent<TweenAlpha>();
                    //    ta.method = UITweener.Method.Linear;
                    //    ta.style = UITweener.Style.PingPong;
                    //    ta.duration = 0.5f;
                    //    ta.from = 1;
                    //    ta.to = 0.5f;
                    //    Destroy(ta, 2f);
                    //}
                }
                //}
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                AddItemToQueue(new CandyInfo(Candy.CandyType.Item, 202), new CandyInfo(Candy.CandyType.Stone, -1),
                               new CandyInfo(Candy.CandyType.Item, 203));
            }
            #endregion
        }

        #region 消除阶段

        private const float PoppingTime = 0.2f;//0.3
        /// <summary>
        /// 不考虑上一步操作的能否消除，在验证操作时还要额外验证是否是彩色糖或特殊糖融合
        /// </summary>
        /// <returns></returns>
        private bool CheckCanPop()
        {
            for (int i0 = 0; i0 < Height; i0++)
            {
                for (int j0 = 0; j0 < Width; j0++)
                {
                    IntVector2 ij;
                    ij.i = i0;
                    ij.j = j0;

                    if (!this[ij].CanHoldCandy) continue;

                    var candy = this[ij].MyCandy;
                    if (candy == null) continue;

                    //有Candy

                    if (candy.State != Candy.CandyState.Static) continue;
                    if (!candy.IsForThreeMatching) continue;

                    var i = ij.i;
                    var j = ij.j;
                    if (i + 2 < Height) //有可能竖3
                    {
                        if (IsSameGenre(ij, ij + new IntVector2(1, 0), ij + new IntVector2(2, 0)))
                        {
                            return true;
                        }
                    }
                    if (j + 2 < Width) //有可能横3
                    {
                        if (IsSameGenre(ij, ij + new IntVector2(0, 1), ij + new IntVector2(0, 2)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 不考虑融合的能否消除，ijList里只要有一个能消除，就返回true，用于检验操作是否有效
        /// </summary>
        /// <returns></returns>
        private bool CheckCanSpecificCellsPop(params IntVector2[] ijList)
        {
            foreach (IntVector2 ij in ijList)
            {
                if (!this[ij].CanHoldCandy) continue;

                var candy = this[ij].MyCandy;
                if (candy == null) continue;

                //有Candy
                if (candy.State != Candy.CandyState.Static) continue;
                if (!candy.IsForThreeMatching) continue;

                var i = ij.i;
                var j = ij.j;

                for (int k = -2; k <= 0; k++)
                {
                    if (i + k >= 0 && i + k + 2 < Height) //检测竖3
                    {
                        if (IsSameGenre(ij + new IntVector2(k, 0), ij + new IntVector2(k + 1, 0),
                                        ij + new IntVector2(k + 2, 0)))
                        {
                            return true;
                        }
                    }
                    if (j + k >= 0 && j + k + 2 < Width) //检测横3
                    {
                        if (IsSameGenre(ij + new IntVector2(0, k), ij + new IntVector2(0, k + 1),
                                        ij + new IntVector2(0, k + 2)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        readonly IntVector2[] _list = new IntVector2[3];
        void SearchPop(IntVector2 start, bool[][] canPopArray, bool[][] hasSearchedArray)
        {
            if (hasSearchedArray[start.i][start.j]) return;//如果出发点已探索就返回
            for (int i = -1; i <= 1; i++)
            {
                //竖
                for (int j = 0; j < 3; j++)
                {
                    var k = j - 1;//-1,0,1
                    _list[j].i = start.i + i + k;
                    _list[j].j = start.j;
                }
                if (IsSameGenre(_list))
                {
                    foreach (var ij in _list)
                    {
                        canPopArray[ij.i][ij.j] = true;
                    }
                }
                //横
                for (int j = 0; j < 3; j++)
                {
                    var k = j - 1;//-1,0,1
                    _list[j].i = start.i;
                    _list[j].j = start.j + i + k;
                }
                if (IsSameGenre(_list))
                {
                    foreach (var ij in _list)
                    {
                        canPopArray[ij.i][ij.j] = true;
                    }
                }
            }
            //至此，所有含有此点的三消线段的所有点都被标记成了canPop
            hasSearchedArray[start.i][start.j] = true;//标记出发点为已探索
            for (int i = -2; i <= 2; i++) //上下，左右分别延伸2格，共9点的十字形
            {
                if (i == 0) continue; //略过自己，省一点是一点
                var k = start.i + i;
                if (k >= 0 && k < Height && canPopArray[k][start.j]) SearchPop(new IntVector2(k, start.j), canPopArray, hasSearchedArray);
                k = start.j + i;
                if (k >= 0 && k < Width && canPopArray[start.i][k]) SearchPop(new IntVector2(start.i, k), canPopArray, hasSearchedArray);
            }
        }

        /// <summary>
        /// 是否静止，同种基本方块，若某ij在区域外则算作Null
        /// </summary>
        /// <param name="ijs"></param>
        /// <returns></returns>
        public bool IsSameGenre(params IntVector2[] ijs)
        {
            var genre = -1;
            foreach (var ij in ijs)
            {
                if (!IsIJInGrid(ij)) return false;
                if (!this[ij].CanHoldCandy) return false;
                if (null == this[ij].MyCandy) return false;
                if (!this[ij].MyCandy.IsStatic || this[ij].MyCandy.Genre < 0) return false;
                if (genre == -1)
                {
                    genre = this[ij].MyCandy.Genre;
                }
                else if (genre != this[ij].MyCandy.Genre)
                {
                    return false;
                }
            }
            return true;
        }

        private const float DelayPerCellOfStripe = 0.1f;

        #region ShouldCheckPop监视器

        struct CellRecord
        {
            public bool HasStaticCandy;
            public Candy.CandyType CandyType;
            public int CandyGenre;
            /// <summary>
            /// 这个格子的某种信息变了：有没有静止糖果，若都有，则看糖果Type是否变了，糖果Genre是否变了
            /// </summary>
            /// <param name="r0"></param>
            /// <param name="r1"></param>
            /// <returns></returns>
            public static bool HasChanged(CellRecord r0, CellRecord r1)
            {
                if (!r0.HasStaticCandy && !r1.HasStaticCandy) return false;
                if (r0.HasStaticCandy == r1.HasStaticCandy && r0.CandyType == r1.CandyType && r0.CandyGenre == r1.CandyGenre) return false;
                return true;
            }
        }

        CellRecord[][] _cellRecords;
        #endregion
        #endregion

        #region 操作阶段

        public Camera ControlCamera;
        /// <summary>
        /// 交换两个方块的动画时间，如果失败则要×2
        /// </summary>
        public const float ExchangeSuccessTime = 0.2f;
        /// <summary>
        /// 参照于我的transform
        /// </summary>
        private Vector2? _lastLocalTouPos;

        /// <summary>
        /// 刚刚交换的两个格子，仅在操作后的消除阶段用于特殊方块生成的位置，消除阶段结束时设为null 
        /// <para>通过现有的Cells和这个变量可以得知刚刚的操作的全貌，0是按下的位置，1是松开的</para>
        /// <para>判断是否有彩色糖或特殊糖融合，抑或只是点击了一下而已</para>
        /// </summary>
        readonly List<ExchangeInfo> _finishedExchanges = new List<ExchangeInfo>();
        public struct ExchangeInfo
        {
            /// <summary>
            /// 手指抬起的格子
            /// </summary>
            public IntVector2 MainCell;
            /// <summary>
            /// 手指按下的格子
            /// </summary>
            public IntVector2 SubCell;

            public ExchangeInfo(IntVector2 mainCell, IntVector2 subCell)
            {
                MainCell = mainCell;
                SubCell = subCell;
            }
        }
        public void AddFinishExchangeInfo(ExchangeInfo exchangeInfo)
        {
            _finishedExchanges.Add(exchangeInfo);
        }

        private void UpdateControlling()
        {
            var touching = false;
            var curLocalTouPos = new Vector2();

            if (_canControl)//一旦不能Control，则中断事件
            {
                if (Input.GetMouseButton(0))
                {
                    var ray = ControlCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (GetComponent<Collider>().Raycast(ray, out hitInfo, 2000))
                    {
                        curLocalTouPos = transform.InverseTransformPoint(hitInfo.point).ToVector2();
                        if (GameManager.SkillMaskContainer.CanTouchThroughAllMasks(curLocalTouPos))
                        {
                            touching = true;
                        }
                    }
                }
                if (!touching && Input.touchCount > 0)
                {
                    var ray = ControlCamera.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit hitInfo;
                    if (GetComponent<Collider>().Raycast(ray, out hitInfo, 2000))
                    {
                        curLocalTouPos = transform.InverseTransformPoint(hitInfo.point).ToVector2();
                        if (GameManager.SkillMaskContainer.CanTouchThroughAllMasks(curLocalTouPos))
                        {
                            touching = true;
                        }
                    }
                }
            }

            #region 点击拾起道具糖果

            if (touching)
            {
                var curTouchedIJ = GetCellIJ(curLocalTouPos);
                if (IsIJInGrid(curTouchedIJ) && this[curTouchedIJ].MyState == Cell.State.Normal
                    && this[curTouchedIJ].MyCandy && this[curTouchedIJ].MyCandy.MyType == Candy.CandyType.Item)//点击了正常格子里的道具糖果
                {
                    this[curTouchedIJ].MyCandy.Pop(0);
                    this[curTouchedIJ].MyCandy = null;
                }
            }
            #endregion
            if (_lastLocalTouPos != null)
            {
                if (touching)
                {
                    var lastTouchedIJ = GetCellIJ((Vector2) _lastLocalTouPos);
                    var curTouchedIJ = GetCellIJ(curLocalTouPos);

                    if (IsIJInGrid(lastTouchedIJ) && IsIJInGrid(curTouchedIJ)
                        && lastTouchedIJ != curTouchedIJ &&
                        ((Vector2) (lastTouchedIJ - curTouchedIJ)).magnitude < 1.1f //如果两个IJ相邻，curTouchedIJ在有效范围内
                        &&
                        (this[lastTouchedIJ].MyState == Cell.State.Normal && this[lastTouchedIJ].MyCandy &&
                         this[lastTouchedIJ].MyCandy.IsStatic) //排除阻挡的，砖块，枷锁，空格，不静止的
                        &&
                        (this[curTouchedIJ].MyState == Cell.State.Normal && this[curTouchedIJ].MyCandy &&
                         this[curTouchedIJ].MyCandy.IsStatic)) //都满足，则交换试试，一定有动画
                    {
                            //逻辑交换，用于检测是否valid，如果失败，则还要恢复
                            var tmp = this[lastTouchedIJ].MyCandy;
                            this[lastTouchedIJ].MyCandy = this[curTouchedIJ].MyCandy;
                            this[curTouchedIJ].MyCandy = tmp;

                            var valid = CheckCanSpecificCellsPop(lastTouchedIJ, curTouchedIJ); //能直接消除,只跟这步相关才行
                            if (!valid)
                            {
                                //有彩色糖
                                valid = (this[lastTouchedIJ].MyCandy &&
                                         this[lastTouchedIJ].MyCandy.MyType == Candy.CandyType.Colorful) ||
                                        (this[curTouchedIJ].MyCandy &&
                                         this[curTouchedIJ].MyCandy.MyType == Candy.CandyType.Colorful);
                            }
                            if (!valid)
                            {
                                //两个特殊糖融合
                                valid = (this[lastTouchedIJ].MyCandy &&
                                         this[lastTouchedIJ].MyCandy.IsSpecial) &&
                                        (this[curTouchedIJ].MyCandy &&
                                         this[curTouchedIJ].MyCandy.IsSpecial);
                            }

                        if (valid) //操作有效
                        {
                            this[lastTouchedIJ].MyCandy.CurIJ = lastTouchedIJ;//进一步确保Candy.CurIJ也正确
                            this[curTouchedIJ].MyCandy.CurIJ = curTouchedIJ;

                            this[curTouchedIJ].MyCandy.TransitionToExchange(lastTouchedIJ, curTouchedIJ, true, true);
                            this[lastTouchedIJ].MyCandy.TransitionToExchange(curTouchedIJ, lastTouchedIJ, false, true);
                        }
                        else //不能消除，操作无效，额外换回动画
                        {
                            #region 换回

                            this[curTouchedIJ].MyCandy.TransitionToExchange(lastTouchedIJ, curTouchedIJ, true, false);
                            this[lastTouchedIJ].MyCandy.TransitionToExchange(curTouchedIJ, lastTouchedIJ, false, false);

                            //换回去
                            tmp = this[lastTouchedIJ].MyCandy;
                            this[lastTouchedIJ].MyCandy = this[curTouchedIJ].MyCandy;
                            this[curTouchedIJ].MyCandy = tmp;

                            #endregion
                        }
                        //中断操作
                        _lastLocalTouPos = null;
                    }
                    else
                    {
                        _lastLocalTouPos = curLocalTouPos;
                    }
                }
                else
                {
                    _lastLocalTouPos = null;
                }
                //TODO:应该要对TouPos轨迹插值
            }
            else //上一帧未按下
            {
                if (touching)
                {
                    //TODO:道具方块
                    if (IsIJInGrid(GetCellIJ(curLocalTouPos)))
                    {
                        _lastLocalTouPos = curLocalTouPos;
                    }
                }
            }
        }

        #endregion

        #region Util

        /// <summary>
        /// 计算localPosition，i,j可以超出[0,8]，比如可用于从-1层掉落
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public Vector2 GetCellPosition(int i, int j)
        {
            return new Vector2((j - (Width - 1)*0.5f)*CellSize.x, (-i + (Height - 1)*0.5f)*CellSize.y);
        }

        /// <summary>
        /// 计算localPosition，i,j可以超出[0,8]，比如可用于从-1层掉落
        /// </summary>
        /// <param name="ij"></param>
        /// <returns></returns>
        public Vector2 GetCellPosition(IntVector2 ij)
        {
            return GetCellPosition(ij.i, ij.j);
        }

        /// <summary>
        /// 计算localPosition，i,j可以超出[0,8]，比如可用于从-1层掉落，适用于处于两格之间，返回精确值
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCellFloatPosition(float i, float j)
        {
            return new Vector2((j - (Width - 1)*0.5f)*CellSize.x, (-i + (Height - 1)*0.5f)*CellSize.y);
        }

        /// <summary>
        /// 计算localPosition，i,j可以超出[0,8]，比如可用于从-1层掉落，适用于处于两格之间，返回精确值
        /// </summary>
        /// <param name="ij"></param>
        /// <returns></returns>
        public Vector2 GetCellFloatPosition(Vector2 ij)
        {
            return GetCellFloatPosition(ij.x, ij.y);
        }

        public IntVector2 GetCellIJ(Vector2 pos)
        {
            var j = Mathf.RoundToInt(pos.x/CellSize.x + (Width - 1)*0.5f);
            var i = Mathf.RoundToInt(-pos.y/CellSize.y + (Height - 1)*0.5f);
            return new IntVector2(i, j);
        }

        public bool IsIJInGrid(IntVector2 ij)
        {
            return ij.i >= 0 && ij.i < Height && ij.j >= 0 && ij.j < Width;
        }

        public bool IsIJInGrid(int i, int j)
        {
            return i >= 0 && i < Height && j >= 0 && j < Width;
        }

        public bool IsIJNormalAndHasStaticCandy(IntVector2 ij)
        {
            return this[ij].MyState == Cell.State.Normal && this[ij].MyCandy != null && this[ij].MyCandy.IsStatic;
        }

        public bool[][] GetBoolGrid()
        {
            var re = new bool[Height][];
            for (int i = 0; i < Height; i++)
            {
                re[i] = new bool[Width];
                for (int j = 0; j < Width; j++)
                {
                    re[i][j] = false;
                }
            }
            return re;
        }

        /// <summary>
        /// 将矩阵刷新成全false。重复使用矩阵，减少垃圾
        /// </summary>
        /// <param name="boolGrid"></param>
        /// <returns></returns>
        public bool[][] ResetBoolGrid(bool[][] boolGrid)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    boolGrid[i][j] = false;
                }
            }
            return boolGrid;
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return new GridEnumerator(this);
        //}

        //public class GridEnumerator : IEnumerator
        //{
        //    private IntVector2 _ij = new IntVector2(0, -1);

        //    public GridEnumerator(Grid grid)
        //    {
        //    }

        //    public bool MoveNext()
        //    {
        //        _ij.j ++;
        //        if (_ij.j >= Width)
        //        {
        //            _ij.i++;
        //            _ij.j = 0;
        //            if (_ij.i >= Height) return false;
        //        }
        //        return true;
        //    }

        //    public void Reset()
        //    {
        //        _ij.i = 0;
        //        _ij.j = -1;
        //    }

        //    public object Current
        //    {
        //        get { return _ij; }
        //    }
        //}

        #endregion

        #region 刷新，死局
        /// <summary>
        /// 刷新，只刷新Normal的，随机。如果刷新了10次还是死局就全部清除（极其罕见，仅为保险）
        /// </summary>
        public void Shuffle()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int i0 = 0; i0 < Height; i0++)
                {
                    for (int j0 = 0; j0 < Width; j0++)
                    {
                        IntVector2 ij;
                        ij.i = i0;
                        ij.j = j0;

                        if (this[ij].MyState == Cell.State.Normal && this[ij].MyCandy != null &&
                            this[ij].MyCandy.MyType == Candy.CandyType.Normal)
                        {
                            this[ij].MyCandy.MyInfo = CandyInfo.GetRandom(GameManager.CurCandyGenreCount);
                            this[ij].MyCandy.Refresh();
                        }
                    }
                }
                var hints = GameManager.Instance.AI.GetAllHintExchanges(this, true);
                if (hints.Count > 0) return;
            }
            //TODO:Shuffle也于事无补，直接全部清除
            Debug.LogWarning("刷新也于事无补，全部清除");
            for (int i0 = 0; i0 < Height; i0++)
            {
                for (int j0 = 0; j0 < Width; j0++)
                {
                    IntVector2 ij;
                    ij.i = i0;
                    ij.j = j0;

                    this[ij].MyState = Cell.State.Normal;
                    if (this[ij].MyCandy != null)
                    {
                        CandyPool.Enqueue(this[ij].MyCandy.gameObject);
                        this[ij].MyCandy = null;
                    }
                }
            }
        }
        #endregion

        #region 道具投放

        readonly Queue<CandyInfo>[] _dropQueueList = new Queue<CandyInfo>[8];

        public Queue<CandyInfo>[] DropQueueList
        {
            get
            {
                if (_dropQueueList[0] == null)
                {
                    for (int i = 0; i < Width; i++)
                    {
                        _dropQueueList[i] = new Queue<CandyInfo>();
                    }
                }
                return _dropQueueList;
            }
        }

        /// <summary>
        /// 投放道具、宝箱，石头不是这么投放的，石头是变出来的；
        /// </summary>
        /// <param name="itemCandys"></param>
        public void AddItemToQueue(params CandyInfo[] itemCandys)
        {
            foreach (var itemCandy in itemCandys)
            {
                var col = Random.Range(0, Width);
                DropQueueList[col].Enqueue(itemCandy);
            }
        }
        #endregion

        #region 正面技能
        public List<IntVector2> CreateStripe(int count)
        {
            var res = new List<IntVector2>();
            for (int i = 0; i < count; i++)
            {
                IntVector2 ij;
                var tryTimes = 0;
                while (true)
                {
                    if (tryTimes > 10) return res; //防止死循环
                    ij = GetRandomIJ();
                    tryTimes++;
                    if (this[ij].CanHoldCandy && this[ij].MyCandy != null &&
                        this[ij].MyCandy.MyType == Candy.CandyType.Normal)
                    {
                        break;
                    }
                }

                var candy = this[ij].MyCandy;
                candy.Set(Random.value < 0.5f ? Candy.CandyType.H : Candy.CandyType.V, candy.Genre);
                candy.Refresh();
                candy.transform.localPosition = GetCellPosition(ij);
                candy.CurIJ = ij;
                candy.State = Candy.CandyState.Static;

                this[ij].MyCandy = candy;
                res.Add(ij);
            }
            return res;
        }
        public List<IntVector2> CreateBomb(int count)
        {
            var res = new List<IntVector2>();
            for (int i = 0; i < count; i++)
            {
                IntVector2 ij;
                var tryTimes = 0;
                while (true)
                {
                    if (tryTimes > 10) return res; //防止死循环
                    ij = GetRandomIJ();
                    tryTimes++;
                    if (this[ij].CanHoldCandy && this[ij].MyCandy != null &&
                        this[ij].MyCandy.MyType == Candy.CandyType.Normal)
                    {
                        break;
                    }
                }

                var candy = this[ij].MyCandy;
                candy.Set(Candy.CandyType.Bomb, candy.Genre);
                candy.Refresh();
                candy.transform.localPosition = GetCellPosition(ij);
                candy.CurIJ = ij;
                candy.State = Candy.CandyState.Static;

                this[ij].MyCandy = candy;
                res.Add(ij);
            }
            return res;
        }
        public List<IntVector2> CreateColorful(int count)
        {
            var res = new List<IntVector2>();
            for (int i = 0; i < count; i++)
            {
                IntVector2 ij;
                var tryTimes = 0;
                while (true)
                {
                    if (tryTimes > 10) return res; //防止死循环
                    ij = GetRandomIJ();
                    tryTimes++;
                    if (this[ij].MyState == Cell.State.Normal && (this[ij].MyCandy == null ||
                        this[ij].MyCandy.MyType == Candy.CandyType.Normal))
                    {
                        break;
                    }
                }

                if (this[ij].MyCandy != null)
                {
                    CandyPool.Enqueue(this[ij].MyCandy.gameObject);
                    this[ij].MyCandy = null;
                }

                var candy = CandyPool.Dequeue().GetComponent<Candy>();
                candy.transform.ResetTransform(transform);
                candy.Set(Candy.CandyType.Colorful, -1);
                candy.MyGrid = this;
                candy.Refresh();
                candy.transform.localPosition = GetCellPosition(ij);
                candy.CurIJ = ij;
                candy.State = Candy.CandyState.Static;

                this[ij].MyCandy = candy;
                res.Add(ij);
            }
            return res;
        }

        private bool _IsGoldenFingerTime { get { return Time.time < _goldenFingerEndTime; } }
        private float _goldenFingerEndTime;
        public void StartGoldenFinger(float time)
        {
            _goldenFingerEndTime = Mathf.Max(_goldenFingerEndTime, Time.time + time);
        }
        #endregion

        #region 技能干扰
        public void AddStoneToQueue(int count)
        {
            var stones = new CandyInfo[count];
            for (int i = 0; i < count; i++)
            {
                stones[i] = new CandyInfo(Candy.CandyType.Stone, -1);
            }
            AddItemToQueue(stones);
        }
        /// <summary>
        /// 随机生成若干个砖块，只会将Normal糖果变成砖块
        /// </summary>
        /// <param name="count"></param>
        public List<IntVector2> FormBricks(int count)
        {
            var res = new List<IntVector2>();
            for (int i = 0; i < count; i++)
            {
                IntVector2 ij;
                var tryTimes = 0;
                do
                {
                    if (tryTimes > 10) return res;//防止死循环
                    ij = GetRandomIJ();
                    tryTimes++;
                } while (this[ij].MyState != Cell.State.Normal);
                if (this[ij].MyCandy != null)
                {
                    CandyPool.Enqueue(this[ij].MyCandy.gameObject);
                    this[ij].MyCandy = null;
                }
                this[ij].MyState = Cell.State.Brick;
                res.Add(ij);
            }

            //TODO:检测死局

            return res;
        }
        /// <summary>
        /// 206上锁
        /// </summary>
        /// <param name="count"></param>
        public List<IntVector2> FormLocks(int count)
        {
            var res = new List<IntVector2>();
            for (int i = 0; i < count; i++)
            {
                IntVector2 ij;
                var tryTimes = 0;
                do
                {
                    if (tryTimes > 10) return res;//防止死循环
                    ij = GetRandomIJ();
                    tryTimes++;
                } while (this[ij].MyState != Cell.State.Normal);

                this[ij].MyState = Cell.State.Lock;
                res.Add(ij);
            }

            //TODO:检测死局

            return res;
        }
        #endregion

        #region Combo
        private int _curComboAmount;
        /// <summary>
        /// 每消除一个糖果就调用一下加1
        /// </summary>
        public void AddComboAmountOne()
        {
            _curComboAmount++;
            if (_curComboAmount > Thr(CurrentComboMultiple))
            {
                CurrentComboMultiple++;

                GameManager.CellEffectContainer.CreatePraiseLabel(CurrentComboMultiple);
            }
        }
        void ResetComboAmountZero()
        {
            _curComboAmount = 0;
            CurrentComboMultiple = 1;
        }
        //m 0   1   2   3   4
        //t 0   3   9   18  30
        int Thr(int m)
        {
            return 3*m*(m + 1)/2;
        }

        /// <summary>
        /// 由Combo导致的蓄力值获取翻倍。第1~3个基元消除是1倍，第4~9个基元消除是2倍，第10~18个基元消除是3倍，依此类推。
        /// </summary>
        public int CurrentComboMultiple;
        #endregion

        public static IntVector2 GetRandomIJ()
        {
            return new IntVector2(Random.Range(0, Height), Random.Range(0, Width));
        }

        public AudioClip LockBreakAudio, BrickBreadAudio;
    }
}