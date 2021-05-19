using MalbersAnimations.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class MaterialShow : MonoBehaviour
    {
        [SerializeField] private IntVar _value;

        private Text _text;

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
            _value.OnValueChanged += UpdateValue;
        }

        private void UpdateValue(int value) => 
            _text.text = value.ToString();
    }
}