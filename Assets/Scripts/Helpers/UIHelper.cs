using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public class UIHelper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthField;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image sliderImage;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void ConfigureSelf(PoolReadyEvent e)
        {
            healthField.text = $"Health: {Utilities.TOWER_HEALTH}";
            healthSlider.value = 1;
        }

        private void OnTowerTookDamage(DamageTakenEvent e)
        {
            healthField.text = $"Health: {e.newHealth}";
            float ratio = Mathf.Clamp(e.newHealth / Utilities.TOWER_HEALTH, 0f, 1f);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(healthSlider.transform.DOShakeScale(0.35f, .15f));
            sequence.Join(sliderImage.DOColor(Color.red, 0.25f).OnComplete(() =>
            {
                DOTween.To(
                    () => healthSlider.value,
                    x => healthSlider.value = x,
                    ratio,
                    .2f
                );
                if (e.newHealth > 0)
                    sliderImage.DOColor(Color.green, 0.25f);
            }));
        }

        private void AddListeners()
        {
            EventBus.Instance.Register<PoolReadyEvent>(ConfigureSelf);
            EventBus.Instance.Register<DamageTakenEvent>(OnTowerTookDamage);
        }

        private void RemoveListeners()
        {
            if (EventBus.Instance != null)
            {
                EventBus.Instance.Unregister<PoolReadyEvent>(ConfigureSelf);
                EventBus.Instance.Unregister<DamageTakenEvent>(OnTowerTookDamage);
            }
        }
    }
}
