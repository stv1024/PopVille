using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.Data;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// Summary
    /// </summary>
    public class IceSkillMask : BaseSkillMask
    {
        public int TimesToBreak = 1;
        private int _count;

        private int[] _timesKnocked;

        /// <summary>
        /// 设置成inactive
        /// </summary>
        public GameObject IceBrickTemplate;

        private IntVector2[] _occupiedIJs;

        public Sprite[] SpriteIceBrickFrames;
        public SpriteRenderer[] IceBricks;

        /// <summary>
        /// 少量小碎屑
        /// </summary>
        public GameObject IceKnockEffectPrefab;

        /// <summary>
        /// 大量大碎片
        /// </summary>
        public GameObject IceBreakEffectPrefab;

        public GameObject ColdAirPtcSys;

        public AudioClip ColdAirAudio, IceBreakAudio;

        private void Awake()
        {
            Destroy(ColdAirPtcSys, 8);

            //_needTutorial = ServerUserPrefs.GetBool(ServerUserPrefs.Key.IceTute) ?? false;
        }
        
        //private bool _needTutorial = false;

        public override void Show(params int[] parameters)
        {
            StartCoroutine(_Show(parameters));
        }

        private IEnumerator _Show(params int[] parameters)
        {
            AudioManager.PlayOneShot(ColdAirAudio);

            _count = parameters[0];
            if (_count <= 0) yield break;
            TimesToBreak = 1;

            yield return new WaitForSeconds(1f); //让冷气吹上来

            _occupiedIJs = new IntVector2[_count];
            IceBricks = new SpriteRenderer[_count];
            _timesKnocked = new int[_count];
            var list = new List<IntVector2>();
            var i = 0;
            while (true)
            {
                var tm = PrefabHelper.InstantiateAndReset<UIButtonMessage>(IceBrickTemplate, transform);
                tm.gameObject.SetActive(true);
                IntVector2 ij;
                if (i == 0)
                {
                    //得到IJ
                    ij = new IntVector2(Random.Range(Grid.Height*5/10, Grid.Height*9/10),
                                        Random.Range(Grid.Width*3/8, Grid.Width*6/8));
                }
                else
                {
                    var tryTimes = 0;
                    //得到IJ
                    while (true)
                    {
                        tryTimes++;
                        if (tryTimes > 20) yield break;//防止死循环
                        int ind = Random.Range(0, list.Count);
                        var curIJ = list[ind];
                        var dirList = new[] {new IntVector2(0, -1), new IntVector2(0, 1), new IntVector2(-1, 0)};
                        var dir = dirList[Random.Range(0, 3)];
                        ij = curIJ + dir;
                        if (GameManager.Instance.MyGrid.IsIJInGrid(ij) && !list.Contains(ij)) break;
                    }
                }
                tm.name = string.Format("{0}", i);
                list.Add(ij);

                tm.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);
                _occupiedIJs[i] = ij;
                IceBricks[i] = tm.GetComponent<SpriteRenderer>();
                i++;
                if (i >= _count) break;
                yield return new WaitForSeconds(0.2f); //下一层冰生成间隔
            }

            Destroy(IceBrickTemplate);
        }

        private void OnIceBrickClick(Object sender)
        {
            int index;
            if (!int.TryParse(sender.name, out index)) return;
            if (index < 0 || index >= _count)
            {
                Debug.LogError("index ERROR. index = " + index + ", while _count = " + _count);
                return;
            }
            _timesKnocked[index]++;
            if (_timesKnocked[index] >= TimesToBreak)//敲碎一块
            {
                var go = PrefabHelper.InstantiateAndReset(IceBreakEffectPrefab, transform);
                go.transform.localPosition = IceBricks[index].transform.localPosition;
                Destroy(go, go.particleSystem.duration);

                AudioManager.PlayOneShot(IceBreakAudio);//音效
                GameManager.Instance.CellEffectContainer.CreateAddEnergyFloatingLabel(IceBricks[index].transform.localPosition, 10);//加蓄力值
                EnergyLightSpot.Create(IceBricks[index].transform.localPosition, 10, 1.5f);

                Destroy(IceBricks[index].gameObject);
                IceBricks[index] = null;
                CheckBreak();
            }
            else//还未敲碎
            {
                var go = PrefabHelper.InstantiateAndReset(IceKnockEffectPrefab, transform); //播放小粒子，增强打击感
                go.transform.localPosition = IceBricks[index].transform.localPosition;
                Destroy(go, go.particleSystem.duration);

                var curFrameNo = _timesKnocked[index]*SpriteIceBrickFrames.Length/TimesToBreak;
                IceBricks[index].sprite = SpriteIceBrickFrames[curFrameNo];
            }
        }

        private void CheckBreak()
        {
            if (IceBricks.All(x => x == null)) //冰块都敲碎了
            {
                GameManager.Instance.SkillMaskContainer.DidDestroy(this);
                Destroy(gameObject, 5f);
            }
        }

        public override bool CanTouchThroughThisMask(Vector2 stdPos)
        {
            if (_occupiedIJs == null) return true; //最早的时候，还未初始化，此时肯定能穿透

            var ij = GameManager.Instance.MyGrid.GetCellIJ(stdPos);
            for (int i = 0; i < _count; i++)
            {
                if (_occupiedIJs[i] == ij && IceBricks[i] != null) return false; //这个位置如果有IceBrick就不能穿透
            }
            return true;
        }

        //private static int GetCount(int level)
        //{
        //    var iceConfig = DesignConfigHelper.GetSkillParameterItem(SkillEnum.Ice);
        //    if (iceConfig == null)
        //    {
        //        Debug.LogError("没有Ice的参数配置");
        //        return level*2 + 2;
        //    }
        //    if (iceConfig.ConstantList.Count < 2)
        //    {
        //        Debug.LogError("Ice的参数配置长度不对");
        //        return level*2 + 2;
        //    }
        //    return Mathf.FloorToInt(level*iceConfig.ConstantList[1] + iceConfig.ConstantList[0]);
        //}

        //private static int GetKnockingTimes(int level)
        //{
        //    return 1;
        //}
    }
}