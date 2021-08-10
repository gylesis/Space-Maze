using UniRx;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class MenuRevealing : MonoBehaviour
    {
        [SerializeField] private GameObject menu;

        private bool _isActive;

        private void Awake()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Tab))
                .Subscribe(_ => SetMenuState());
        }

        private void SetMenuState()
        {
            _isActive = !_isActive;
            menu.SetActive(_isActive);
        }
       
    }
}