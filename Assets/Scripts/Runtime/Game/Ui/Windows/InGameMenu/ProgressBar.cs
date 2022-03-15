using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Utils.UiExtensions;
using Random = UnityEngine.Random;

namespace Game.Ui.InGameMenu
{
    public class ProgressBar : MonoBehaviour
    {
        [System.Serializable]
        private struct Stage
        {
            public Color color;
            [Range(0, 1)]
            public float ratio;

            public Stage(Color color, float ratio)
            {
                this.color = color;
                this.ratio = ratio;
            }
        }

        [SerializeField] private SlicedFilledImage progress;
        [SerializeField] private Stage[] stages;

        public void SetFillAmount(float ratio)
        {
            progress.fillAmount = ratio;
            progress.color = stages.Find(x => ratio <= x.ratio).color;
        }
        
        public void Repaint(float ratio)
        {
            progress.DOKill();
            progress.DOFillAmount(ratio, 0.1f);
            progress.DOColor(stages.Find(x => ratio <= x.ratio).color, 0.1f);
        }
    }
}