using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        Debug.Log($"Bubble OnTriggerEnter2D {target.name}");

        var enemy = target.GetComponent<Enemy2>();
        if (enemy != null)
        {
            enemy.HitByBubble();
            Destroy(gameObject);
        }
    }
}
