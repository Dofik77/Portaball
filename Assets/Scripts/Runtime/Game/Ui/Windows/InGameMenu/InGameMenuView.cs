using CustomSelectables;
using DG.Tweening;
using SimpleUi.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.InGameMenu
{
    public class InGameMenuView : UiView
    {
        public CustomButton Play;
        public CustomButton Restart;
        public CustomButton FinishLevel;
        public TMP_Text coinCount;
        public ProgressBar progress;
        public FinishLevel finishLevel;

        [SerializeField] private Image proto;
        [SerializeField] private RectTransform targetPos;
        [SerializeField] private float durationCoinAdded;

        private static Vector3 WorldPosition(RectTransform rectTransform) => rectTransform.TransformPoint(rectTransform.rect.center);
        
        public void CoinAdded(Vector2 screenPosition, int newValue)
        {
            var pos = WorldPosition(targetPos);

            var coinEffect = Instantiate(proto, transform);
            var rectTransform = (RectTransform)coinEffect.transform;
            coinEffect.DOFade(0, 0);
            rectTransform.DOAnchorPos(screenPosition, 0);
            DOTween.Sequence()
                .Append(coinEffect.DOFade(1, 0.4f))
                .Append(rectTransform.DOMove(pos, durationCoinAdded))
                .OnComplete(() =>
                {
                    Destroy(coinEffect.gameObject);
                    coinCount.text = newValue.ToString();
                });
        }
    }
}