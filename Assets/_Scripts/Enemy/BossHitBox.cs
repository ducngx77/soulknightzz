using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int damage = 10;
    private float AttackTimeCounter = 3f;
    [SerializeField]
    private float AttackRate = 1f;
    private bool hit = false;
    public Animator animator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = FindTarget.FindSingleTarget("Player");
        if (hit && AttackTimeCounter >= AttackRate)
        {
            PlayerHealth playerhealth = player.GetComponent<PlayerHealth>();
            playerhealth.Damage(damage);
            hit = false;
            AttackTimeCounter = 0f;
        }
        AttackTimeCounter += Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

        }
    }
    public void OnTriggerStay2D(Collider2D collider)
    {
        if (animator.GetBool("isJump") == true || animator.GetBool("isAttack") == true)
        {
            hit = true;
        }
    }
}
