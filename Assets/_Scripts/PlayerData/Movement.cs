using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float speed;

    private GameObject target;

    [SerializeField]
    public Animator animator;

    //get input from player
    //apply movement to sprite

    private void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical);

        AnimateMovement(direction);
        transform.position += direction * speed * Time.deltaTime;
        target = FindTarget.FindNearestTargetInRange("Enemy", transform, 10f);
        if (target != null)
        {
            if (target.transform.position.x < this.transform.position.x)
            {
                this.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            }
            else if (target.transform.position.x > this.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
        else
        {
                        if (direction.x < 0)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            if (direction.x > 0)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

    }

    void AnimateMovement(Vector3 direction)
    {
        if (animator != null)
        {
            if (direction.magnitude > 0)
            {

                animator.SetBool("isMoving", true);

                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);

            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
