using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = false;
    bool isDead = false;
    int idMove = 0;
    Animator anim;
    int jumpCount = 0; 
    public float moveSpeed = 5f;
    public float jumpForce = 280f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;  // Stop any movement if the player is dead

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }
        Move();
        CheckFall();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("tanah");
            anim.ResetTrigger("jump");
            if (idMove == 0) anim.SetTrigger("idle");
            isJump = false;
            jumpCount = 0; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetTrigger("jump");
            anim.ResetTrigger("run");
            anim.ResetTrigger("idle");
            isJump = true;
        }
    }

    public void MoveRight()
    {
        idMove = 1;
    }

    public void MoveLeft()
    {
        idMove = 2;
    }

    private void Move()
    {
        if (idMove == 1 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (idMove == 2 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void Jump()
    {
        if (jumpCount < 1) 
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
            anim.SetTrigger("jump");
            jumpCount++;
            isJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("water"))
        {
            Debug.Log("aer");
            Die();
        }
    }

    public void Idle()
    {
        if (!isJump)
        {
            anim.ResetTrigger("jump");
            anim.ResetTrigger("run");
            anim.SetTrigger("idle");
        }
        idMove = 0;
    }

    private void CheckFall()
    {
        if (transform.position.y < -30f && !isDead)
        {
            Die();
        }
    }

    private void Die()
{
    isDead = true;
    anim.SetTrigger("die"); // Trigger the dead animation

    // Hentikan waktu
    Time.timeScale = 0.5f; // Melambatkan waktu agar terlihat dramatis, bisa diubah sesuai kebutuhan

    // Beri gaya dorong ke bawah agar karakter terlihat jatuh
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    rb.velocity = Vector2.zero; // Hentikan gerakan horizontal
    rb.AddForce(Vector2.down * 200f); // Gaya dorong ke bawah, bisa disesuaikan

    // Matikan collider agar tidak ada interaksi lagi saat mati
    GetComponent<Collider2D>().enabled = false;
}

}
