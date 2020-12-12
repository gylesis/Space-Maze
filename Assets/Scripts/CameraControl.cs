using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    Transform player;

    private void Start()
    {
        player = Player.Instance.transform;
    }


    void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(player.position.x, player.position.y, player.position.z - 10);

        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation,.1f);
        transform.position = Vector3.Lerp(transform.position, newPosition, .1f);
    }
}
