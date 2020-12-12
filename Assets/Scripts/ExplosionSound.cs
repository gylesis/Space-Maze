using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    [SerializeField]
    AudioSource audio;

    float radiusToHear = 50f;

    private void Start() {
        audio.Play();
    }

    private void Update() {
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 5) {
            if (audio)
                audio.volume -= 0.01f;
        }
        audio.volume = Mathf.Clamp((radiusToHear - (Vector3.Distance(transform.position, Player.Instance.transform.position) / 2)) / 100 - 0.4f, 0, 0.3f);
    }

}
