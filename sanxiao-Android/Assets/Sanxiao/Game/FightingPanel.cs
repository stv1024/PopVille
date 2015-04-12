using System.Collections;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    public class FightingPanel : MonoBehaviour
    {
        #region 单例面板通用

        //private static FightingPanel _instance;

        //public static FightingPanel Instance
        //{
        //    get { return _instance; }
        //    private set
        //    {
        //        if (_instance && value)
        //        {
        //            Debug.LogError("more than 1 PanelNamePanel instance now!");
        //            Destroy(_instance.gameObject);
        //        }
        //        _instance = value;
        //    }
        //}

        //private void Awake()
        //{
        //    Instance = this;
        //}

        #endregion

        public GameObject GravePrefab;
        public GameObject DamageLabelPrefab;
        public Vector3 DamageLabelPosMine, DamageLabelPosOpponent;

        public void CreateDamagePanel(bool isMe, int damage)
        {
            var label = PrefabHelper.InstantiateAndReset<UILabel>(DamageLabelPrefab, transform);
            label.text = string.Format("-{0}", damage);
            label.transform.localPosition = isMe ? DamageLabelPosMine : DamageLabelPosOpponent;
            TweenPosition.Begin(label.gameObject, 2, label.transform.localPosition + new Vector3(0, 50, 0));
            Destroy(label.gameObject, 5);
        }

        public void CharacterDie(bool isOur, int characterIndex)
        {
            var go = PrefabHelper.InstantiateAndReset(GravePrefab, transform);
            go.transform.localPosition =
                (isOur ? GameManager.Instance.OurCharacterList : GameManager.Instance.RivalCharacterList)[characterIndex
                    ].transform.localPosition;
            StartCoroutine(HideCharacter(isOur, characterIndex));
        }
        IEnumerator HideCharacter(bool isOur, int characterIndex)
        {
            yield return new WaitForSeconds(1);
            (isOur ? GameManager.Instance.OurCharacterList : GameManager.Instance.RivalCharacterList)[characterIndex].ClearToEmpty();
        }

        //public void PlayKOEffect()
        //{
        //    //放大居中
        //    animation.Play();
        //    StartCoroutine(KOEffectCoroutine());

        //}

        //IEnumerator KOEffectCoroutine()
        //{
        //    //放大居中
        //    animation.Play();
        //    //等待放大完成
        //    yield return new WaitForSeconds(animation.clip.length);
        //    //慢镜头
        //    Time.timeScale = 0.5f;


        //    yield return new WaitForSeconds(5);
        //    Time.timeScale = 1;
        //}
    }
}