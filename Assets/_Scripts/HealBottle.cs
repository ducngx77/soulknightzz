using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealBottle : MonoBehaviour
{

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float Range = 10f;
    public bool AutoCollect = false;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < Range && AutoCollect == true)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && AutoCollect == true)
        {
            PlayerHealth playerhealth = collider.GetComponent<PlayerHealth>();
            playerhealth.Heal(50);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && AutoCollect == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerHealth playerhealth = collider.GetComponent<PlayerHealth>();
                playerhealth.Heal(50);
                Destroy(gameObject);
            }
        }
    }
}