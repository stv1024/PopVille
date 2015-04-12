using System.Collections.Generic;
using Assets.Sanxiao.Game;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.AI
{
    public class SanxiaoAI
    {
        //Grid _grid;

        public SanxiaoAI(Grid grid)
        {
            if (grid == null) Debug.LogError("grid == null Error! SanxiaoAI must have grid.");
            //_grid = grid;
        }

        readonly List<CandyExchange> _exchangeList = new List<CandyExchange>();

        readonly List<IntVector2> _cornerList = new List<IntVector2>
                {
                    new IntVector2(-1, -1),
                    new IntVector2(-1, 1),
                    new IntVector2(1, 1),
                    new IntVector2(1, -1),
                    new IntVector2(-1, -1)
                };

        /// <summary>
        /// Very heavy. Set onlyFirstOne = true if only want to check death
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="onlyFirstOne"></param>
        /// <returns></returns>
        public List<CandyExchange> GetAllHintExchanges(Grid grid, bool onlyFirstOne = false)
        {
            _exchangeList.Clear();

            for (int i = 0; i < Grid.Height; i++)
            {
                for (int j = 0; j < Grid.Width; j++)
                {
                    IntVector2 ij;
                    ij.i = i;
                    ij.j = j;
                    var cell = grid[ij];
                    if (!cell.CanHoldCandy) continue;
                    var candy = cell.MyCandy;
                    if (candy == null) continue;
                    #region \/
                    // le    ri
                    //    O
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            var le = ij + _cornerList[k];
                            var ri = ij + _cornerList[k + 1];
                            if (grid.IsSameGenre(ij, le, ri) && grid.IsIJNormalAndHasStaticCandy(le) && grid.IsIJNormalAndHasStaticCandy((le + ri) / 2))
                            {
                                if (grid[ij].MyState == Cell.State.Normal &&
                                    grid[(le + ri) / 2].MyState == Cell.State.Normal) //滤除枷锁
                                {
                                    _exchangeList.Add(new CandyExchange(ij, (le + ri)/2, new[] {le, ij, ri}));
                                    if (onlyFirstOne) return _exchangeList;
                                }
                            }
                        }
                    }
                    #endregion

                    #region -\,-/
                    //a0  O  开口 |         a2
                    //      a1    |  a0  0  开口
                    {
                        IntVector2 a0;
                        a0.i = 0;
                        a0.j = -1;

                        IntVector2 a1;
                        a1.i = -1;
                        a1.j = 1;

                        IntVector2 a2;
                        a2.i = 1;
                        a2.j = 1;

                        for (int k = 0; k < 4; k++)
                        {
                            if (grid.IsSameGenre(ij, ij + a0, ij + a1) && grid.IsIJNormalAndHasStaticCandy(ij + a1) && grid.IsIJNormalAndHasStaticCandy(ij - a0))
                            {
                                if (grid[ij - a0].MyState == Cell.State.Normal &&
                                    grid[ij + a1].MyState == Cell.State.Normal)//滤除枷锁
                                {
                                    _exchangeList.Add(new CandyExchange(ij - a0, ij + a1, new[] {ij + a0, ij, ij + a1}));
                                    if (onlyFirstOne) return _exchangeList;
                                }
                            }
                            if (grid.IsSameGenre(ij, ij + a0, ij + a2) && grid.IsIJNormalAndHasStaticCandy(ij + a2) && grid.IsIJNormalAndHasStaticCandy(ij - a0))
                            {
                                if (grid[ij - a0].MyState == Cell.State.Normal &&
                                    grid[ij + a2].MyState == Cell.State.Normal)//滤除枷锁
                                {
                                    _exchangeList.Add(new CandyExchange(ij - a0, ij + a2, new[] {ij + a0, ij, ij + a2}));
                                    if (onlyFirstOne) return _exchangeList;
                                }
                            }
                            a0 = RotateClockwise(a0);
                            a1 = RotateClockwise(a1);
                            a2 = RotateClockwise(a2);
                        }
                    }
                    #endregion

                    #region - 。
                    //a0 O    a1
                    {
                        IntVector2 a0;
                        a0.i = 0;
                        a0.j = -1;

                        IntVector2 a1;
                        a1.i = 0;
                        a1.j = 2;

                        for (int k = 0; k < 4; k++)
                        {
                            if (grid.IsSameGenre(ij, ij + a0, ij + a1) && grid.IsIJNormalAndHasStaticCandy(ij + a1 / 2) && grid.IsIJNormalAndHasStaticCandy(ij + a1))
                            {
                                if (grid[ij + a1/2].MyState == Cell.State.Normal &&
                                    grid[ij + a1].MyState == Cell.State.Normal)//滤除枷锁
                                {
                                    _exchangeList.Add(new CandyExchange(ij + a1/2, ij + a1, new[] {ij + a0, ij, ij + a1}));
                                    if (onlyFirstOne) return _exchangeList;
                                }
                            }
                            a0 = RotateClockwise(a0);
                            a1 = RotateClockwise(a1);
                        }
                    }

                    #endregion

                    #region 融合(仅向右或下搜索)

                    if (i < Grid.Height - 1 && j < Grid.Width - 1) //最右，最下不需要检测
                    {
                        var ij0 = ij;
                        ij0.j += 1;
                        if (grid[ij].MyState == Cell.State.Normal && grid[ij0].MyState == Cell.State.Normal &&
                            grid[ij0].MyCandy) //两个格子都是Normal态，滤除枷锁
                        {
                            if (((grid[ij0].MyCandy.Genre >= 0 || grid[ij0].MyCandy.MyType == Candy.CandyType.Colorful) &&
                                 candy.MyType == Candy.CandyType.Colorful) ||
                                (candy.IsSpecial && grid[ij0].MyCandy.IsSpecial)) //有一个是彩色糖or两个都是特殊糖果
                            {
                                _exchangeList.Add(new CandyExchange(ij, ij0));
                                if (onlyFirstOne) return _exchangeList;
                            }
                        }

                        ij0 = ij;
                        ij0.i += 1;
                        if (grid[ij].MyState == Cell.State.Normal && grid[ij0].MyState == Cell.State.Normal &&
                            grid[ij0].MyCandy) //两个格子都是Normal态，滤除枷锁
                        {
                            if (((grid[ij0].MyCandy.Genre >= 0 || grid[ij0].MyCandy.MyType == Candy.CandyType.Colorful) &&
                                 candy.MyType == Candy.CandyType.Colorful) ||
                                (candy.IsSpecial && grid[ij0].MyCandy.IsSpecial)) //有一个是彩色糖or两个都是特殊糖果
                            {
                                _exchangeList.Add(new CandyExchange(ij, ij0));
                                if (onlyFirstOne) return _exchangeList;
                            }
                        }
                    }

                    #endregion
                }
            }
            return _exchangeList;
        }

        public static IntVector2 RotateClockwise(IntVector2 ij)
        {
            return new IntVector2(ij.j, -ij.i);
        }
    }

    public struct CandyExchange
    {
        public IntVector2 IJ;

        public IntVector2 IJ1;

        /// <summary>
        /// 如果是融合，则此项为null
        /// </summary>
        public IntVector2[] SameGenreCells;

        public CandyExchange(IntVector2 ij, IntVector2 ij1, IntVector2[] sameGenreCells)
        {
            IJ = ij;
            IJ1 = ij1;
            SameGenreCells = sameGenreCells;
        }
        public CandyExchange(IntVector2 ij, IntVector2 ij1)
        {
            IJ = ij;
            IJ1 = ij1;
            SameGenreCells = null;
        }
    }
}