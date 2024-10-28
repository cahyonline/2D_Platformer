using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState { Idle, Running, Jumping }
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 300f;
    private bool isGrounded = true;
    private bool isDead = false;
    private int idMove = 0;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerState playerState = PlayerState.Idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        UpdateAnimations();
        CheckFall();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            Idle();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (playerState != PlayerState.Running)
                SetState(PlayerState.Idle);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            SetState(PlayerState.Jumping);
        }
    }

    private void SetState(PlayerState state)
    {
        playerState = state;
        anim.ResetTrigger("idle");
        anim.ResetTrigger("run");
        anim.ResetTrigger("jump");

        switch (state)
        {
            case PlayerState.Idle:
                anim.SetTrigger("idle");
                break;
            case PlayerState.Running:
                anim.SetTrigger("run");
                break;
            case PlayerState.Jumping:
                anim.SetTrigger("jump");
                break;
        }
    }

    private void MoveRight()
    {
        idMove = 1;
        SetState(PlayerState.Running);
    }

    private void MoveLeft()
    {
        idMove = -1;
        SetState(PlayerState.Running);
    }

    private void Idle()
    {
        idMove = 0;
        if (isGrounded)
            SetState(PlayerState.Idle);
    }

    private void UpdateAnimations()
    {
        if (idMove != 0 && isGrounded)
        {
            rb.velocity = new Vector2(idMove * moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(idMove, 1f, 1f);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            SetState(PlayerState.Jumping);
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            // Tambah skor atau efek lainnya
            Destroy(collision.gameObject);
        }
    }

    private void CheckFall()
    {
        if (transform.position.y < -20f)
        {
            isDead = true;
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
