using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;

public class PigMovement : MonoBehaviour
{

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float vision = 10f;
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float attackRange = 8f;
    [SerializeField]
    private float attackRate = 5;
    private float AttackTimeCounter = 1f;
    private bool isCharging;
    public Animator animator;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator.SetBool("isRun", false);
        if (animator.GetBool("isDead").Equals(false) && player != null)
        {
            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

            if (player.transform.position.x > this.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else if (player.transform.position.x < this.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }

            if (distanceFromPlayer < vision && distanceFromPlayer > attackRange && isCharging == false)
            {
                animator.SetBool("isRun", true);
                Vector3 moveDirection = (player.position - transform.position).normalized;
                transform.position += moveDirection * speed * Time.deltaTime;
            } else if (distanceFromPlayer < attackRange && isCharging == false)
            {
                ActivateAllChildren(true);
                animator.SetBool("isRun", true);
                Vector3 moveDirection = (player.position - transform.position).normalized;
                transform.position += moveDirection * speed * 4 * Time.deltaTime;
            }
            if (AttackTimeCounter >= attackRate)
            {
                Invoke(nameof(ChargAttack), attackRate);
                AttackTimeCounter = 0f;
            }
            AttackTimeCounter += Time.deltaTime;
        }
        Debug.Log(isCharging);
    }
    void ChargAttack()
    {
        isCharging = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && isCharging == false)
        {
            PlayerHealth playerhealth = collider.GetComponent<PlayerHealth>();
            playerhealth.Damage(damage);
            isCharging = true;
            ActivateAllChildren(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && isCharging == false)
        {
            isCharging = true;
            ActivateAllChildren(false);
        }
    }

    void ActivateAllChildren(bool activate)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(activate);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, vision);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
