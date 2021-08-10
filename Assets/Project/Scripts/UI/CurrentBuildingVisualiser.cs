using Project.Scripts.Game.Buildings;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class CurrentBuildingVisualiser : MonoBehaviour
    {
        private Image _image;

        public void Construct(Subject<BuildingData> onClick)
        {
            _image = GetComponent<Image>();
            
            onClick
                .TakeUntilDestroy(this)
                .Subscribe(value => _image.sprite = value.Icon);
        }

    }
}