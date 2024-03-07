using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target;
    private Transform player;
    private float AttackTimeCounter = 1f;
    [SerializeField]
    private float AttackRate = 1f;
    [SerializeField]
    private int numOfBullets = 1;
    public GameObject bullet;

    [SerializeField]
    private float range = 10f;

    //MeleeWeapon MeleeWeapon;
    void Start()
    {
        player = FindTarget.FindSingleTarget("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            //Vector3 transformPosition = new Vector3(0.12f * Mathf.Deg2Rad, -0.095f * Mathf.Deg2Rad, 0);
            //transform.position = transformPosition;
            player = FindTarget.FindSingleTarget("Player").transform;
            target = FindTarget.FindNearestTargetInRange("Enemy", player.transform, range);
            if (target != null)
            {
                Vector2 direction = target.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (angle > 90 || angle < -90)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(180, 0, -angle));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }
            //else
            //{

            //    else
            //    {
            //        transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.y -180, 0));
            //    } 
            //}
            if (Input.GetKeyDown(KeyCode.Space) && AttackTimeCounter >= AttackRate && target != null)
            {
                StartCoroutine(Attack(numOfBullets));
                AttackTimeCounter = 0f;
            }

            AttackTimeCounter += Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


    IEnumerator Attack(int numOfBullets)
    {
        for (int i = 0; i < numOfBullets; i++)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
