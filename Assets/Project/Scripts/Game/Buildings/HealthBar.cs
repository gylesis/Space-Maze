using UnityEngine.UI;

namespace Project.Scripts.Game.Buildings
{
    public class HealthBar
    {
        private Slider _slider;

        public HealthBar(Slider slider)
        {
            _slider = slider;
        }

        public void UpdateBar(float value) => 
            _slider.value = value;
    }
}