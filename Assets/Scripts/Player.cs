using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    float timer;

    [SerializeField]
    private float speed = 1;
    [HideInInspector]
    public Rigidbody2D Rigidbody;
    private CapsuleCollider2D collider;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        Instance = this;
    }

    void FixedUpdate()
    {
        Move();
        AlignBySurface();
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(xInput, 0) * speed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn") && Input.GetKey(KeyCode.Space) && timer > 1)
        {
            timer = 0;
            BridgeRender br = collision.gameObject.GetComponent<BridgeRender>();
            if ((br.pos1 - transform.position).magnitude < (br.pos2 - transform.position).magnitude)
            {          
                Rigidbody.AddForce((br.pos2 - br.pos1).normalized * 500);
            }
            else
            {
                Rigidbody.AddForce((br.pos1 - br.pos2).normalized * 500);
            }
        }
    }

    void AlignBySurface()
    {
        if (Asteroid.CurrentAsteroid == null) return;
        RaycastHit2D rayToGround = Physics2D.Raycast(transform.position, Asteroid.CurrentAsteroid.transform.position - transform.position);
        Vector3 rotation = Vector3.zero;

        if (-rayToGround.normal.y >= 0)
        {
            rotation.z = 180 - Mathf.Asin(-rayToGround.normal.x) * 180 / Mathf.PI;
        }
        else
        {
            rotation.z = Mathf.Asin(-rayToGround.normal.x) * 180 / Mathf.PI;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(rotation), 0.25f);
    }
    private void OnDrawGizmos()
    {
        RaycastHit2D rayToGround = Physics2D.Raycast(transform.position, -transform.up);

        Debug.DrawLine(transform.position, transform.position + new Vector3(rayToGround.normal.x, rayToGround.normal.y));
    }
}
