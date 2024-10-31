using System.Xml.Serialization;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    [SerializeField] private GameObject greenParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity =  transform.right* speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.TakeDamage(10);
        }
        Instantiate(greenParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
