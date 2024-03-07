using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField]
    private float Vision = 8f;
    [SerializeField]
    private float FireRate = 1f;
    private float NextFireTime = 1f;
    private GameObject target;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public Animator animator;

    private float JumpTimeCounter = 1f;
    [SerializeField]
    private float JumpRate = 10f;
    [SerializeField]
    public float fallForce = 2f;
    public float jumpHeight = 1f;
    private Rigidbody2D rb;
    private bool isJumping = false;
    public GameObject jumpHitBox;
    void Start()
    {
        target = FindTarget.FindSingleTarget("Player");
        //Physics2D.IgnoreLayerCollision(10, 10);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        target = FindTarget.FindSingleTarget("Player");
        if (target != null)
        {
            float distanceFromPlayer = Vector2.Distance(target.transform.position, transform.position);

            if (distanceFromPlayer < Vision && JumpTimeCounter >= JumpRate && target.transform != null)
            {
                if (!isJumping)
                {
                    Jump();
                }
                JumpTimeCounter = 0f;
            }
            else if (distanceFromPlayer <= Vision && NextFireTime > FireRate && animator.GetBool("isJump") == false)
            {
                StartCoroutine(Attack());
                NextFireTime = 0f;
            }

            JumpTimeCounter += Time.deltaTime;
        }
        NextFireTime += Time.deltaTime;
        Debug.Log(NextFireTime.ToString());
    }

    IEnumerator Attack()
    {
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            float angleStep = 120f / 4f;
            float startingAngle = -60f;
            for (int j = 0; j < 5; j++)
            {
                float angle = startingAngle + j * angleStep;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
                GameObject bulletZ = bullet1;
                if (i <= 0)
                {
                    bulletZ = bullet3;
                }
                else if (i > 0 && i <= 2)
                {
                    bulletZ = bullet2;
                }
                else if (i > 2 && i <= 4)
                {
                    bulletZ = bullet1;
                }
                GameObject bullet = Instantiate(bulletZ, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction;
                //bullet.GetComponent<Bullet>().SetDirection(direction);
            }
        }
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isAttack", false);
    }

    void Jump()
    {
        animator.SetBool("isJump", true);
        //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        isJumping = true;
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        float jumpVelocity = Mathf.Sqrt(2 * fallForce * jumpHeight);
        float jumpTime = jumpVelocity / fallForce;
        float horizontalVelocity = distance / jumpTime;
        rb.velocity = new Vector2(direction.x * horizontalVelocity, jumpVelocity);
        Invoke("Fall", jumpTime);
    }

    void Fall()
    {
        rb.velocity = new Vector2(rb.velocity.x, -fallForce);
        Invoke("ResetJump", 1f);
    }

    void ResetJump()
    {
        Instantiate(jumpHitBox, transform.position, Quaternion.identity);
        animator.SetBool("isJump", false);
        isJumping = false;
        //gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

}