using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRevealing : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            menu.SetActive(true);
            GameLogic.inMenu = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            menu.SetActive(false);
            GameLogic.inMenu = false;
        }
    }
}