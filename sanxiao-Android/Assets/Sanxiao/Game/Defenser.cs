using System.Collections;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// Ԯ������ֵ�AI������ս��
    /// </summary>
    public class Defenser : MonoBehaviour
    {
        public GameManager GameManager;
        public bool IsRival;
        public int Index;
        private DefenseData _defenseData;
        private float _n;
        private float _delayEndTime;
        private float _energyCapacity;

        public int CurrenctRivalHealth
        {
            get { return IsRival ? GameData.RivalHealthList[Index] : GameData.OurHealthList[Index]; }
            set
            {
                if (IsRival) GameData.RivalHealthList[Index] = value;
                else GameData.OurHealthList[Index] = value;
            }
        }

        public void StartPlaying(GameManager gameManager, bool isRival, int index, DefenseData defenseData)
        {
            GameManager = gameManager;
            IsRival = isRival;
            Index = index;
            _defenseData = defenseData;
            StartCoroutine(PlayingChallenge());
        }

        private IEnumerator PlayingChallenge()
        {
            _energyCapacity = 1000;
            while (GameManager.Instance.GameState != GameManager.GameStateEnum.Ending)
            {
                foreach (UseSkillEvent e in _defenseData.SkillEventList)
                {
                    _n = 0f;
                    var nSpeed = 1f / (e.TimeDelta * 0.001f);
                    while (true)
                    {
                        if (_n >= 1f)
                        {
                            break;
                        }
                        if (Time.time >= _delayEndTime)
                        {
                            _n += nSpeed*Time.deltaTime;
                            if (IsRival)
                            {
                                GameData.RivalEnergyList[Index] = Mathf.RoundToInt(_n*_energyCapacity);
                            }
                            else
                            {
                                GameData.OurEnergyList[Index] = Mathf.RoundToInt(_n*_energyCapacity);
                            }
                        }
                        yield return new WaitForEndOfFrame();//һ֡һ֡�صȴ�
                    }
                    var physicalDamages = new int[GameData.TeamMaxNumber];
                    physicalDamages[0] = e.PhysicalDamage;
                    for (int i = 0; i < GameData.FellowDataList.Count; i++)
                    {
                        var teamAdd = GameData.FellowDataList[i];
                        physicalDamages[1+i] = teamAdd.AttackAdd;
                    }
                    GameManager.Instance.UseSkillInfoQueue.Enqueue(new GameManager.UseSkillInfo(!IsRival, Index,
                                                                                                (SkillEnum)
                                                                                                e.SkillCode,
                                                                                                physicalDamages));
                }
                yield return new WaitForSeconds(1);//����֮���м������ֹ����Ϊ��ʱ��ѭ��
            }
        }
        /// <summary>
        /// �ܵ����ţ�ŭ�������ӳ�delay��
        /// </summary>
        /// <param name="delay"></param>
        public void Disturb(float delay)
        {
            _delayEndTime = Mathf.Max(Time.time, _delayEndTime) + delay;
        }
        /// <summary>
        /// ֱ�Ӽ���ŭ��������209����
        /// </summary>
        /// <param name="energyReduce"></param>
        public void ReduceEnergy(int energyReduce)
        {
            _n = Mathf.Max(0, _n - energyReduce*1f/_energyCapacity);
        }

        public void DieAndEnd()
        {
            StopAllCoroutines();
        }
    }
}