using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinCollider : MonoBehaviour
{
    GameObject pressToFinish;

    [SerializeField]
    GameObject princessPrefab;

    bool win = false;

    private void Start() {
        pressToFinish = GameLogic.Instance.victorySign;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F) && win) {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            princessPrefab.SetActive(true);
            pressToFinish.SetActive(true);
            win = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            pressToFinish.SetActive(false);
            win = false;
        }
    }

}
