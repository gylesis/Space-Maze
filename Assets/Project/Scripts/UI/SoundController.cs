using Project.Scripts.Game;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class SoundController : MonoBehaviour {
        [SerializeField]
        AudioSource audio;

        int direction;

        [SerializeField]
        float radiusToHear = 30;

        private void Start() {
            audio.Play();
        }

        private void Update() {
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 5) {
                if (audio)
                    audio.volume -= 0.01f;
            }
            audio.volume = Mathf.Clamp((radiusToHear - (Vector3.Distance(transform.position, Player.Instance.transform.position) / 2)) / 100 - 0.25f, 0, 0.05f);
            audio.panStereo = -direction * (Vector3.Distance(transform.position, Player.Instance.transform.position)) / 100;
        }

        private void CalculateDirection() {
            if(transform.position.x > Player.Instance.transform.position.x) {
                direction = 1;
            }
            else {
                direction = -1;
            }
        }



    }
}