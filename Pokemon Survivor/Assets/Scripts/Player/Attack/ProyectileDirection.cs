using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileDirection : MonoBehaviour
{
    private Camera cam;
    public Transform player;
    public Rigidbody2D rb;

    Vector2 mousePos;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Vector2 lookAtDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookAtDir.y, lookAtDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        rb.position = player.position;
    }
}
