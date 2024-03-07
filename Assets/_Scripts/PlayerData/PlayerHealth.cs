using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 200;
    private int MAX_HEALTH;

    public bool isImmune = false;
    public HealthBar healthBar;

    public Animator animator;
    public TMP_Text popupText;
    private int poisonCount = 0;
    private bool isPoisoning = false;
    public GameObject popupDamage;

    void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        SetHealth(health);
        animator.SetBool("isDead", false);
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(health);
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
        if (isImmune == false)
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
            }
            popupText.text = amount.ToString();

            GameObject newObject = Instantiate(popupDamage, this.transform.position, Quaternion.identity);

            newObject.transform.SetParent(transform);
            this.health -= amount;

            if (health <= 0)
            {
                Die();
            }
            healthBar.SetHealth(this.health);
        }
        else
        {
            popupText.text = amount.ToString();

            GameObject newObject = Instantiate(popupDamage, this.transform.position, Quaternion.identity);

            newObject.transform.SetParent(transform);
        }

    }

    public void Poison()
    {
        if (isPoisoning) return;

        StartCoroutine(PoisonCoroutine());
    }

    IEnumerator PoisonCoroutine()
    {
        isPoisoning = true;

        for (int i = 0; i < 4; i++)
        {
            Damage(5);
            poisonCount++;

            yield return new WaitForSeconds(1f);
        }

        isPoisoning = false;
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
        StartCoroutine(DelayedDeath());
    }

    IEnumerator DelayedDeath()
    {
        

        // Tắt các component và thiết lập isDead
        this.gameObject.GetComponent<PlayerMovent>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        animator.SetBool("isDead", true);

        Debug.Log("I am Dead!");
        yield return new WaitForSeconds(5f); // Đợi 5 giây
        // Load Scene "Die"
        SceneManager.LoadScene("Die");
    }
}
