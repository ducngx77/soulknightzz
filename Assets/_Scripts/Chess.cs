using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chess : MonoBehaviour
{
    public bool AutoCollect = false;
    private Transform player;
    public Animator animator;
    [SerializeField]
    public GameObject weaponPrefab1;
    [SerializeField]
    public GameObject weaponPrefab2;
    [SerializeField]
    public GameObject weaponPrefab3;
    [SerializeField]
    public GameObject weaponPrefab4;
    private GameObject[] gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        gameObjects = new GameObject[3];
        gameObjects[0] = weaponPrefab1;
        gameObjects[1] = weaponPrefab2;
        gameObjects[2] = weaponPrefab3;
        gameObjects[3] = weaponPrefab4;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && AutoCollect == true)
        {
            if (animator.GetBool("isOpened") == false && animator.GetBool("isOpening") == false)
            {
                animator.SetBool("isOpening", true);
                Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], this.transform.position, Quaternion.identity);
                Invoke("DoneOpening", 2f);
            }
        }
    }

    void DoneOpening()
    {
        animator.SetBool("isOpening", false);
        animator.SetBool("isOpened", true);
    }

    //private void OnTriggerStay2D(Collider2D collider)
    //{
    //    if (collider.gameObject.tag == "Player" && AutoCollect == false)
    //    { 
    //        if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("isOpened") == false && animator.GetBool("isOpening") == false)
    //        {
    //            animator.SetBool("isOpening", true);
    //            Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], this.transform.position, Quaternion.identity);
    //            Invoke("DoneOpening", 2f);
    //        }
    //    }
    //}

}
