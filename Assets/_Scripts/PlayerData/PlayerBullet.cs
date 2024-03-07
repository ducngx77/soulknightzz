using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    GameObject target;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private int damage = 6;

    [SerializeField]
    private float range = 10f;

    private Rigidbody2D bulletRB;

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        target = FindTarget.FindNearestTargetInRange("Enemy", transform, range);
        if (target != null)
        {
            Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
            bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
            Vector2 velocity = bulletRB.velocity;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        Destroy(gameObject, 4f);
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            EnemyHealth health = collider.GetComponent<EnemyHealth>();
            health.Damage(damage);
            Destroy(this.gameObject);
        }
        if (collider.gameObject.tag == "Boss")
        {
            BossHealth health = collider.GetComponent<BossHealth>();
            health.Damage(damage);
            Destroy(this.gameObject);
        }
        if (collider.gameObject.tag == "EditorOnly")
        {
            Destroy(this.gameObject);
        }
        if(collider.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
