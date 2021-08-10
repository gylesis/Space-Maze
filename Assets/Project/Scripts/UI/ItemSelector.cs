using Project.Scripts.Game.Buildings;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class ItemSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BuildingData _building;
        
        private Image _backgroundImage;
        private Subject<BuildingData> OnClick = new Subject<BuildingData>();

        public void Construct(Subject<BuildingData> onClick)
        {
            _backgroundImage = GetComponentInChildren<Image>();
            SetOpacity(Color.black);
            OnClick = onClick;
        }
    
        public void OnPointerClick(PointerEventData eventData) => 
            OnClick.OnNext(_building);

        public void OnPointerEnter(PointerEventData eventData) => 
            SetOpacity(Color.gray);

        public void OnPointerExit(PointerEventData eventData) => 
            SetOpacity(Color.black);

        private void SetOpacity(Color clor)
        {
            var color = clor;
            color.a = .5f;
            _backgroundImage.color = color;
        }
    }
}