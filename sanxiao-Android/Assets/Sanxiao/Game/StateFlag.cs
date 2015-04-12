using UnityEngine;

namespace Assets.Sanxiao.Game
{
    public class StateFlag : MonoBehaviour
    {
        public Sprite Ready, Go, TimeUp, GameOver;
        public SpriteRenderer SprLabel;
        public Animation Animation;

        void Awake()
        {
            gameObject.SetActive(false);
        }
        public void Clear()
        {
            gameObject.SetActive(false);
        }
        public void ShowReady()
        {
            SprLabel.sprite = Ready;
            gameObject.SetActive(true);
            Animation.Play();
            CancelInvoke("Clear");
            Invoke("Clear", 2);
        }
        public void ShowGo()
        {
            SprLabel.sprite = Go;
            gameObject.SetActive(true);
            Animation.Play();
            CancelInvoke("Clear");
            Invoke("Clear", 2);
        }
        public void ShowTimeUp()
        {
            SprLabel.sprite = TimeUp;
            gameObject.SetActive(true);
            Animation.Play();
            CancelInvoke("Clear");
            Invoke("Clear", 2);
        }
        public void ShowGameOver()
        {
            SprLabel.sprite = GameOver;
            gameObject.SetActive(true);
            Animation.Play();
            CancelInvoke("Clear");
            Invoke("Clear", 2);
        }
    }
}