using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 10);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.gameObject.CompareTag("Ground"))
        Destroy(this.gameObject);

        if (col.gameObject.CompareTag("Enemy")){
            col.gameObject.SendMessage("TakeDamage", 1);
            Destroy(this.gameObject);
        }
        
    }
}
