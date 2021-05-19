using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    protected float _hp;
    protected float _maxHp;

    protected string name;

    private int _enzimaPrice;
    private int _chromiumPrice;
    private int _linoniumPrice;

    public Asteroid LocatingAsteroid;

    protected int idOfCurrentAsteroid { get; set; }

    protected void SetPrice(int enzima, int chromium, int linonium)
    {
        _enzimaPrice = enzima;
        _chromiumPrice = chromium;
        _linoniumPrice = linonium;
    }

    protected virtual void Update()
    {
    }

    public void TakeDamage(int damage, GameObject obj)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            OnDeath();
            obj.GetComponent<Animator>().SetTrigger("Leave");
        }
    }

    public void OnDeath()
    {
        Destroy(Instantiate(GameLogic.Instance.explosionPrefab, transform.position, Quaternion.identity), 2f);
        Destroy(gameObject);
    }
}