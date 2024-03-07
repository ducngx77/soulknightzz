using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    GameObject target;

    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private int damage = 6;
    [SerializeField]
    private float spreadAngle = 0f;
    [SerializeField]
    private bool poision = false;

    private Rigidbody2D bulletRB;
    //private FindTarget findTarget;
    // Start is called before the first frame update
    void Start()
    {
        float angleOffset = Random.Range(-spreadAngle / 2, spreadAngle / 2);
        bulletRB = GetComponent<Rigidbody2D>();
        target = FindTarget.FindSingleTarget("Player");
        Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * target.transform.position - transform.position;
        Vector2 moveDir = (direction).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        Vector2 velocity = bulletRB.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerHealth playerhealth = collider.GetComponent<PlayerHealth>();
            playerhealth.Damage(damage);
            if (poision == true) {
                playerhealth.Poison();
            }
            Destroy(this.gameObject);
        }
        if (collider.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

    //internal void SetDirection(Vector3 direction)
    //{
    //    bulletRB.velocity = new Vector2(direction.x, direction.y);
    //}
}
