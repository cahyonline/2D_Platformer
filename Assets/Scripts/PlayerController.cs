using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = true;
    bool isDead = false;
    int idMove = 0;
    Animator anim;
    public float moveSpeed;

    public GameObject Projectile; 
    public Vector2 projectileVelocity;
    public Vector2 projectileOffset; 
    public float cooldown = 0.5f;
    bool isCanShoot = true;

    
    bool isGrounded; 
    int jumpCount = 0; 
    public int maxJumpCount = 1; 
    private void Start()
    {
        anim = GetComponent<Animator>();
        isCanShoot = true;
        EnemyController.EnemyKilled = 0;
    }

    private void Update()
    {
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
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }

        //if (Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log("Z");
            //Fire();
        }
        Move();
        Dead();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump || collision.gameObject.CompareTag("Ground")) 
        {
            //isGrounded = true; 
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
            isGrounded = false; 
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
            if (transform.localScale.x < 0) 
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else if (idMove == 2 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.localScale.x > 0) 
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public void Jump()
    {
        if (isGrounded || jumpCount < maxJumpCount) 
        {
            anim.SetTrigger("jump");
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300f);
            jumpCount++; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Coin"))
        {
            Data.score += 15;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground")){
            isGrounded = true;
        }
        if (collision.transform.CompareTag("Peluru"))
        {
            isCanShoot = true;
        }
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("water"))
        {
            Debug.Log("Karakter terkena musuh atau air!");
            isDead = true;
            SceneManager.LoadScene("Game Over");
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

    private void Dead()
    {
        if (!isDead)
        {
            if (transform.position.y < -10f)
            {
                isDead = true;
            }
        }
    }

    void Fire()
    {
        if (isCanShoot)
        {
            GameObject bullet = Instantiate(Projectile, (Vector2)transform.position - projectileOffset * transform.localScale.x, Quaternion.identity);

            Vector2 velocity = new Vector2(projectileVelocity.x * transform.localScale.x, projectileVelocity.y);
            bullet.GetComponent<Rigidbody2D>().velocity = velocity * -1;

            Vector3 scale = transform.localScale;
            bullet.transform.localScale = scale * -1;

            StartCoroutine(CanShoot()); // Mulai cooldown
            anim.SetTrigger("shoot"); 
        }
    }

    IEnumerator CanShoot()
    {
        anim.SetTrigger("shoot");
        isCanShoot = false;
        yield return new WaitForSeconds(cooldown);
        isCanShoot = true;
    }
}
