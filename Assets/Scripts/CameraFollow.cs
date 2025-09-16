using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, -10f);
    }
}
