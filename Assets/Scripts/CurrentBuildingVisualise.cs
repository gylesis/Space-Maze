using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentBuildingVisualise : MonoBehaviour {
    public static CurrentBuildingVisualise Instance;
    
    [HideInInspector]
    public Image currentBuildingImage;

    [SerializeField]
    public List<Sprite> buildingImages;

    private void Start() {
        Instance = this;
        currentBuildingImage = GetComponent<Image>();
        currentBuildingImage.sprite = buildingImages[GameLogic.currentBuildingToBuild];
    }
}
