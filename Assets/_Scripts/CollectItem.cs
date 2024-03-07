using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectItem : MonoBehaviour
{
    private bool isCollected = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            isCollected = true;
        }
        else
        {
            isCollected = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isCollected && collision.gameObject.tag == "Player")
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 transformPosition = transform.position;
                Vector3 transformScale = transform.localScale;
                transform.SetParent(collision.transform);

                //Vector3 transformPosition = new Vector3(0.12f, -0.095f, 0).normalized;
                //Vector3 transformScale = transform.localScale;
                //transform.SetParent(collision.transform);

                transform.position = transformPosition;
                transform.localScale = transformScale;

                foreach (Transform child in transform.parent)
                {
                    if (child.CompareTag(gameObject.tag) && child != transform)
                    {
                        Vector3 childPosition = child.position;
                        Vector3 childScale = child.localScale;
                        child.transform.SetParent(null);
                        child.position = childPosition;
                        child.localScale = childScale;
                        Debug.Log("Child with the same tag has been destroyed.");
                        break;
                    }
                }
            }
        }
    }

    public void DropItem()
    {
        if (isCollected)
        {
            //Vector3 DropPosition = transform.position;
            //Vector3 DropScale = transform.localScale;
            transform.SetParent(null);
            //transform.position = DropPosition;
            //transform.localScale = DropScale;

        }
    }
}
