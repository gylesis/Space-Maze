using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentBuildingVisualise : MonoBehaviour
{
    public static CurrentBuildingVisualise Instance;
    private GameLogic _gameLogic;

    [HideInInspector] public Image currentBuildingImage;

    [SerializeField] public List<Sprite> buildingImages;

    private void Awake()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    private void Start()
    {
        Instance = this;
        currentBuildingImage = GetComponent<Image>();
        currentBuildingImage.sprite = buildingImages[_gameLogic.currentBuildingToBuild];
    }
}