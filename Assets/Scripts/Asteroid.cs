using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour {

    private bool isCaptured = false;

    private int Id;

    public static Asteroid CurrentAsteroid;

    public float oreAmount;
    Text oreText;

    [SerializeField]
    SpriteRenderer spriteOfCurrentMaterial;

    public enum TypeOfAsteroid {
        enzima, chromium, linonium
    }

    public
    TypeOfAsteroid typeOfAsteroid;

    [SerializeField]
    private const float forceCoefficient = 1f;
    [SerializeField]
    private bool isEnabled = true;
    [SerializeField]
    private float radius;

    private void Awake() {
        GameLogic.Asteroids.Add(gameObject);
    }

    private void Start() {

        oreText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        oreAmount = Random.value * 100 + 200;

        int randomType = Random.Range(1, 4);
        switch (randomType) {
            case 1:
                typeOfAsteroid = TypeOfAsteroid.enzima;
                spriteOfCurrentMaterial.sprite = GameLogic.Instance.spriteOfMaterials[0];
                break;

            case 2:
                typeOfAsteroid = TypeOfAsteroid.chromium;
                spriteOfCurrentMaterial.sprite = GameLogic.Instance.spriteOfMaterials[1];
                break;

            case 3:
                typeOfAsteroid = TypeOfAsteroid.linonium;
                spriteOfCurrentMaterial.sprite = GameLogic.Instance.spriteOfMaterials[2];
                break;
        }

        StartCoroutine(IdInitialization());

    }
    private void Update() {
        ApplyGravityOnPlayer();

        if (name != "DestinationPlanet") {

            oreText.text = ((int)Mathf.Max(oreAmount, 0)).ToString();
        }
    }

    IEnumerator IdInitialization() {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < GameLogic.Asteroids.Count; i++) {
            if (GameLogic.Asteroids[i] == gameObject) {
                Id = i;
            }
        }
    }

    private void ApplyGravityOnPlayer() {
        Vector2 distance = transform.position - Player.Instance.transform.position;
        Vector2 force = (forceCoefficient / distance.magnitude * distance.magnitude) * distance.normalized;

        if (distance.magnitude < radius && isEnabled) {
            CurrentAsteroid = this;
            GameLogic.currentIdOfAsteroid = Id;
            Player.Instance.Rigidbody.AddForce(force);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnMouseEnter() {
        GameLogic.selectedAsteroid = Id;
        BlueprintCreator.Instance.ShowBlueprint();
    }

    private void OnMouseOver() {
        GameLogic.selectedAsteroid = Id;
    }

    private void OnMouseExit() {
        BlueprintCreator.Instance.DeleteBluepint();
    }
}
