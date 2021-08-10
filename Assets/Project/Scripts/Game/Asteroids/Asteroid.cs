using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project.Scripts.Game.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        private Text _text;

        public enum TypeOfAsteroid
        {
            Enzima,
            Chromium,
            linonium
        }

        private AsteroidData _data;

        public readonly Subject<Unit> OnDestroy = new Subject<Unit>();

        public void Construct(AsteroidData data)
        {
            _data = data;
            _data.Construct();

            _text = GetComponentInChildren<Text>();

            
            // StartCoroutine(IdInitialization());

            Subscription();
        }

        private void Subscription()
        {
            _data.OnOreEvaluate
                .TakeUntilDestroy(this)
                .Subscribe(UpdateOreAmount);
        }

        private void UpdateOreAmount(float value)
        {
            _text.text = value.ToString();

            if (value <= 0)
            {
                OnDestroy.OnNext(Unit.Default);
                Destroy(this);
            }
        }

    }
}