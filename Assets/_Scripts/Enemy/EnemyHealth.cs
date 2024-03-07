using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int valueCoin = 3;
    [SerializeField] private Animator animator;
    public GameObject popupDamage;
    public TMP_Text popupText;
    private int MAX_HEALTH;
    
    public GameObject coin;

    void Start()
    {
        SetHealth(health);
        animator.SetBool("isDead", false);
    }

    void Update()
    {

    }

    public void SetHealth(int health)
    {
        this.MAX_HEALTH = health;
        this.health = health;
    }

    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        popupText.text = amount.ToString();
        
        GameObject newObject = Instantiate(popupDamage, this.transform.position, Quaternion.identity);

        newObject.transform.SetParent(transform);

        this.health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    private void Die()
    {
        DestroyChildren(this.transform);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }
        animator.SetBool("isDead", true);
        for (int i = 0; i < valueCoin; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 2f;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            Instantiate(coin, spawnPosition, Quaternion.identity);
        }
        Destroy(gameObject, 2f);
    }

    void DestroyChildren(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            Destroy(child.gameObject);
        }
        parentTransform.DetachChildren();
    }
}
