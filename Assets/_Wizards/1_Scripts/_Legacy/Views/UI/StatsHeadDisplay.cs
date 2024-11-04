using UnityEngine;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal interface IStatsHeadDisplay
    {
        void DisplayHealth(float health);
        void Activate();
        void Deactivate();
    }

    internal class StatsHeadDisplay : View, IStatsHeadDisplay
    {
        [SerializeField] private Slider healthSlider;
        private Vector3 scale;
        public void DisplayHealth(float health)
        {
            if (health < 0) healthSlider.value = 0;
            else healthSlider.value = health < healthSlider.maxValue ? health : healthSlider.maxValue;
        }

        public void Activate() => SetActiveDisplay(true);
        public void Deactivate() => SetActiveDisplay(false);
        private void SetActiveDisplay(bool active) => gameObject.SetActive(active);
    }
}
