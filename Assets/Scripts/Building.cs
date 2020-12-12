using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    protected float hp;
    protected float maxHp;

    protected string name;

    public int enzimaPrice;
    public int chromiumPrice;
    public int linoniumPrice;


    public Asteroid locatingAsteroid;

    protected int idOfCurrentAsteroid { get; set; }

    protected void SetPrice(int enzima, int chromium, int linonium) {
        enzimaPrice = enzima;
        chromiumPrice = chromium;
        linoniumPrice = linonium;
    }


    protected virtual void Update() {


    }

    public void TakeDamage(int damage, GameObject obj) {
        hp -= damage;
        if (hp <= 0) {
            OnDeath();
            obj.GetComponent<Animator>().SetTrigger("Leave");
        }
    }

    public void OnDeath() {
        Destroy(Instantiate(GameLogic.Instance.explosionPrefab,transform.position,Quaternion.identity), 2f);
        Destroy(gameObject);
    }

}
