using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    public static Asteroid CurrentAsteroid;

    [SerializeField] SpriteRenderer spriteOfCurrentMaterial;

    public float oreAmount;

    private Text oreText;
    private bool isCaptured = false;
    private int Id;
    private GameLogic _gameLogic;

    public enum TypeOfAsteroid
    {
        enzima,
        chromium,
        linonium
    }

    public
        TypeOfAsteroid typeOfAsteroid;

    [SerializeField] private float forceCoefficient = 1f;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private float radius;

    private void Awake()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
        
        _gameLogic.Asteroids.Add(gameObject);
        oreText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        oreAmount = Random.value * 100 + 200;

        int randomType = Random.Range(1, 4);
        switch (randomType)
        {
            case 1:
                typeOfAsteroid = TypeOfAsteroid.enzima;
                spriteOfCurrentMaterial.sprite = _gameLogic.spriteOfMaterials[0];
                break;

            case 2:
                typeOfAsteroid = TypeOfAsteroid.chromium;
                spriteOfCurrentMaterial.sprite = _gameLogic.spriteOfMaterials[1];
                break;

            case 3:
                typeOfAsteroid = TypeOfAsteroid.linonium;
                spriteOfCurrentMaterial.sprite = _gameLogic.spriteOfMaterials[2];
                break;
        }

        StartCoroutine(IdInitialization());
    }

    private void Update()
    {
        ApplyGravityOnPlayer();

        if (name != "DestinationPlanet")
        {
            oreText.text = ((int) Mathf.Max(oreAmount, 0)).ToString();
        }
    }

    IEnumerator IdInitialization()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _gameLogic.Asteroids.Count; i++)
        {
            if (_gameLogic.Asteroids[i] == gameObject)
            {
                Id = i;
            }
        }
    }

    private void ApplyGravityOnPlayer()
    {
        Vector2 distance = transform.position - Player.Instance.transform.position;
        Vector2 force = (forceCoefficient / distance.magnitude * distance.magnitude) * distance.normalized;

        if (distance.magnitude < radius && isEnabled)
        {
            CurrentAsteroid = this;
            _gameLogic.currentIdOfAsteroid = Id;
            Player.Instance.Rigidbody.AddForce(force);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnMouseEnter()
    {
        _gameLogic.selectedAsteroid = Id;
        BlueprintCreator.Instance.ShowBlueprint();
    }

    private void OnMouseOver()
    {
        _gameLogic.selectedAsteroid = Id;
    }

    private void OnMouseExit()
    {
        BlueprintCreator.Instance.DeleteBluepint();
    }
}