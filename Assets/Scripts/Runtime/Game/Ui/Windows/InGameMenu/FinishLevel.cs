using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui.InGameMenu
{
    public class FinishLevel : MonoBehaviour
    {
        [SerializeField] private TMP_Text coin;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private TMP_Text stageTitle;
        [SerializeField] private Image stageIcon;
        [SerializeField] private Sprite[] stageIcons;
        [SerializeField] private string[] stageTexts;

        public void FinishShow(int coinReward, int stage, Action onComplete = null)
        {
            stageIcon.sprite = stageIcons[stage];
            stageTitle.text = stageTexts[stage];
            
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, 0.2f).OnComplete(()=>onComplete?.Invoke());
            coin.text = "+" + coinReward;
        }
    }
}