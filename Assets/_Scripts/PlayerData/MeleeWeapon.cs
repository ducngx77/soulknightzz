using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField]
    private int damage = 6;
    [SerializeField]
    private float AttackRate = 1f;
    public KeyCode swingKey = KeyCode.Space;
    //[SerializeField]
    //public float swingAngle = 45f;
    [SerializeField]
    private Animator animator;
    public bool swirl;
    public static bool IsAttacking { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            //Vector3 transformPosition = new Vector3(0.12f * Mathf.Deg2Rad, -0.095f * Mathf.Deg2Rad, 0);
            //transform.position = transformPosition;
            if (Input.GetKeyDown(swingKey) && IsAttacking == false)
            {
                IsAttacking = true;
                animator.SetBool("isAttacking", true);
                animator.SetBool("Swirl", swirl);
                StartCoroutine(ResetSwordRotation());
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 35));
        }
    }

    IEnumerator ResetSwordRotation()
    {
        yield return new WaitForSeconds(AttackRate);
        IsAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsAttacking == true && collider.gameObject.tag == "Enemy")
        {
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            health.Damage(damage);
        }
        else if (IsAttacking == true && collider.gameObject.tag == "Bullet")

        {
            Destroy(collider.gameObject);
        }
    }
}
