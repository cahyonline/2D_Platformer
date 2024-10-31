using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public bool isGrounded = false;
    public bool isFacingRight = false;
    public Transform batas1;
    public Transform batas2;
    public float speed = 02f;
    Rigidbody2D rigid;
    Animator anim;
    public float health = 5;  
    bool isDie = false;
    public static int EnemyKilled = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (isGrounded && !isDie)
        {
            if (isFacingRight)
            {
                //Debug.Log("Moving right");
                MoveRight();
            }
            else
            {
                //Debug.Log("Moving left");
                MoveLeft();
            }

            if (transform.position.x >= batas2.position.x && isFacingRight)
            {
                //Debug.Log("Reached batas2, flipping direction");
                Flip();
                isFacingRight = false;
            }
            else if (transform.position.x <= batas1.position.x && !isFacingRight)
            {
                //Debug.Log("Reached batas1, flipping direction");
                Flip();
                isFacingRight = true;
            }
        }
    }
    void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
        if (!isFacingRight){
            Flip();
        }
    }
    void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;
        if (isFacingRight){
            Flip();
        }
    }
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }    

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Debug.Log("ground");
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Debug.Log("notground");
            isGrounded = false;
        }
    }
        public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetTrigger("IsDie");   
            //Instantiate(redParticles, transform.position, transform.rotation);
            Destroy (this.gameObject);
        }
    }
}
