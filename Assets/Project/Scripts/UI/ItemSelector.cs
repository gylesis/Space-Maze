using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private int buildingId;

    private Image BG;
    private GameLogic _gameLogic;

    private void Awake()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    private void Start()
    {
        switch (name)
        {
            case "Mining Drill":
                buildingId = 0;
                break;

            case "Bridge":
                buildingId = 1;
                break;

            case "Turret":
                buildingId = 2;
                break;
        }

        BG = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameLogic.currentBuildingToBuild = buildingId;
        CurrentBuildingVisualise.Instance.currentBuildingImage.sprite =
            CurrentBuildingVisualise.Instance.buildingImages[_gameLogic.currentBuildingToBuild];
        _gameLogic.isStateToCreate = true;
    }

    private void OnEnable()
    {
        Color color = Color.black;
        color.a = .5f;
        BG.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = Color.gray;
        color.a = .5f;
        BG.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = Color.black;
        color.a = .5f;
        BG.color = color;
    }
}