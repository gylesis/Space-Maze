using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {
    public Vector2 currentPos;

    [SerializeField]
    public SpriteRenderer _currentSprite;

    [SerializeField]
    private Text currentIndexShow;

    public int gridIndex;

    public static Cell currentCell;

    public void OnSpawn(Vector2 pos, int _gridIndex) {
        currentPos = pos;
        gridIndex = _gridIndex;
        GridGeneration.onSwitch += OnSwitch;
        GridGeneration.onSwitch.Invoke();
    }

    public void OnSwitch() {
        ResetColor();
        if (GridGeneration.currentPos == currentPos) {
            SetSpriteColor(Color.yellow);
            currentCell = this;
        }
        else {
            SetSpriteColor(Color.white);
        }
        currentIndexShow.text = gridIndex.ToString();
    }

    public void SetSpriteColor(Color color) {
        _currentSprite.color = color;
    }

    public static void ResetColor() {
        foreach (var cel in GridGeneration.cells) {
            if (!(cel == currentCell)) {
                cel.SetSpriteColor(Color.white);
            }
        }
    }

}
