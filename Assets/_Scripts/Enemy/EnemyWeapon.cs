using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float holdingAngle = 45f;
    [SerializeField]
    private float AttackRange = 7f;
    [SerializeField]
    private float FireRate = 1f;
    private float NextFireTime = 1f;
    private GameObject target;
    public GameObject bullet;
    void Start()
    {
        target = FindTarget.FindSingleTarget("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            target = FindTarget.FindSingleTarget("Player");
            if (target != null)
            {
                Vector2 direction = target.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (angle > 90 || angle < -90)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, -angle - holdingAngle));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(new Vector3(180, 180, angle - holdingAngle));
                }
                float distanceFromPlayer = Vector2.Distance(target.transform.position, transform.position);
                if (distanceFromPlayer <= AttackRange && NextFireTime < Time.time)
                {
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    NextFireTime = Time.time + FireRate;
                }
            }
        }
    }
}
