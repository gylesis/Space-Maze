using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialShow : MonoBehaviour {
    [SerializeField]
    Text currentValue;

    [SerializeField]
    Image currentImage;

    [SerializeField]
    string nameOfMaterialThatIShow;

    private void Update() {
        switch (nameOfMaterialThatIShow) {
            case "Enzima":
                currentValue.text = GameLogic.enzimaAmount.ToString();
                break;

            case "Chromium":
                currentValue.text = GameLogic.chromiumAmount.ToString();
                break;

            case "Linonium":
                currentValue.text = GameLogic.linoniumAmount.ToString();
                break;
        }
    }

}
