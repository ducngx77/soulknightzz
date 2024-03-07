using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int valueCoin = 30;
    [SerializeField] private Animator animator;
    public GameObject popupDamage;
    public TMP_Text popupText;
    public HealthBar healthBar;
    private int MAX_HEALTH;

    public GameObject coin;
    void Start()
    {
        healthBar = GameObject.Find("HealthBarBoss").GetComponent<HealthBar>();
        SetHealth(health);
        animator.SetBool("isDead", false);
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(health);
        
    }

    void Update()
    {
        GameObject target = FindTarget.FindSingleTarget("Player");
        if (target != null)
        {
            float distanceFromPlayer = Vector2.Distance(target.transform.position, transform.position);
            if (distanceFromPlayer <= 10f)
            {
                healthBar.gameObject.SetActive(true);
            } else
            {
                healthBar.gameObject.SetActive(false);
            }
        }
        
        
    }

    public void SetHealth( int health)
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
        healthBar.SetHealth(this.health);
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
        healthBar.SetHealth(this.health);
    }

    private void Die()
    {
        DestroyChildren(this.transform);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }
        for (int i = 0; i < valueCoin; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 2f;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            Instantiate(coin, spawnPosition, Quaternion.identity);
        }
        animator.SetBool("isDead", true);
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
