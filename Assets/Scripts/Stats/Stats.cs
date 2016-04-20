using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Stats script should be attached to the root gameobject 
/// of any damageable entities such as players and enemies.
/// </summary>

public class Stats : MonoBehaviour
{
    #region Variables

    public int pointValue = 10;

    public AudioClip hurtSound = null;

    public AudioClip deathSound = null;

    public List<GameObject> drops;

    public List<float> dropProbabilities;

    public float LOOP_DELAY = 0.5f;

    #region Stat Variables

    public int currentHealth = 5;

	public int maxHealth = 5;

	public bool dead = false;

    public GameObject hurtTextPrefab;

    public int damage = 0;

    // Chance to drop an item.
    public float dropChance = .70f;

    #endregion

    #region Status Effect Variables

    // Stun variables
    public float stunDuration = 0;

    public bool isStunned = false;

    // Slow variables
    public float slowDuration = 0;

    public float slowPercentage;

    public bool isSlowed = false;

    // Silence variables
    public float silenceDuration = 0;

    public bool isSilenced = false;

    // Rooted variables

    public float rootDuration = 0;

    public bool isRooted = false;

    #endregion

    #endregion

    #region Functions

    public void SpawnHurtText(string text, Vector3 position, Color c){

     Vector3 v = Camera.main.WorldToViewportPoint(position);
     float x = v.x;
     float y = v.y;
     x = Mathf.Clamp(x, 0.05f, 0.95f); // clamp position to screen to ensure
     y = Mathf.Clamp(y, 0.05f, 0.9f);  // the string will be visible
     GameObject gui = Instantiate(hurtTextPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
     gui.GetComponent<GUIText>().material.color = c;
     gui.GetComponent<GUIText>().text = text;
 }

    /* Starts couroutine to check for status effects */
    void Start()
    {
        StartCoroutine(RunStatusEffects());
    }

    /* Adds the passed amount of health onto current health. */
    public virtual void ChangeHealth(int amount)
    {
        int oldHealth = currentHealth;

        if (currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth + amount <= 0)
        {
            currentHealth = 0;
            if (!dead)
            {
                Die();
            }
        }
        else
        {
            currentHealth += amount;
        }

        if (amount < 0)
        {
            if (hurtSound != null)
            {
                GetComponent<AudioSource>().pitch = Random.Range(1f, 1.25f);
                AudioSource.PlayClipAtPoint(hurtSound, transform.position, .5f);
            }

            if (hurtTextPrefab != null)
            {
                
                SpawnHurtText(amount.ToString(), transform.position, Color.yellow);
            }
        }
        else
        {
            if (hurtTextPrefab != null)
            {
                SpawnHurtText("+" + amount.ToString(), transform.position, Color.green);
            }
        }
    }

    public virtual void ChangeMaxHealth(int amount)
    {
        int oldHealth = maxHealth;

        if (maxHealth + amount < currentHealth)
        {
            maxHealth += amount;
            FillHealth();
        }
        else if (maxHealth + amount <= 0)
        {
            maxHealth = 0;
            FillHealth();
        }
        else
        {
            maxHealth += amount;
        }
    }

    public virtual void FillHealth()
    {
        ChangeHealth(currentHealth - maxHealth);
    }
    
    /* Kills the unit. */
    public virtual void Die()
    {       
        dead = true;
        if (deathSound != null)
        {
            GetComponent<AudioSource>().pitch = 1;
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1);
        }

        float num = Random.Range(0f,1f);
        if (num < dropChance) DropItem();
        if(transform.tag.Equals("Enemy")) GetComponent<EnemyAI>().Die();

        GameController.ChangePoints(pointValue);
    }

    /* Called in the update function to check for status effects. */
    public virtual void HandleStatusEffects()
    {
        //this method is only to organize the update function.

        //cc handlers

        //if CC time <= 0, make sure it stays at 0, set CC to false

        if (stunDuration <= 0)
        {
            isStunned = false;
            stunDuration = 0;
        }
        if (slowDuration <= 0)
        {
            isSlowed = false;
            slowDuration = 0;
        }
        if (silenceDuration <= 0)
        {
            isSilenced = false;
            silenceDuration = 0;
        }
        if (rootDuration <= 0)
        {
            isRooted = false;
            rootDuration = 0;
        }

        //if CC time > 0, count down to 0, and set CC to true

        if (stunDuration > 0)
        {
            isStunned = true;
            stunDuration -= Time.deltaTime;

        }
        if (slowDuration > 0)
        {
            isSlowed = true;
            slowDuration -= Time.deltaTime;

        }
        if (silenceDuration > 0)
        {
            isSilenced = true;
            silenceDuration -= Time.deltaTime;

        }
        if (rootDuration > 0)
        {
            isRooted = true;
            rootDuration -= Time.deltaTime;
        }
    }

    public virtual IEnumerator RunStatusEffects()
    {
        // we all random wait, so our AI frames are staggered:
        yield return new WaitForSeconds(Random.value * LOOP_DELAY);
        while (true)
        {
            HandleStatusEffects();
            if (dead)
            {
                break;
            }
            yield return new WaitForSeconds(LOOP_DELAY);
        } // Status effect loop
    }

    void DropItem()
    {
        if (drops.Count < 1)
        {
            return;
        }

        GameObject drop = PickItem();

        if (drop != null)
        {
            Instantiate(drop, transform.position, Quaternion.Euler(270,0,0));
        }
    }

    GameObject PickItem()
    {
        float choice = Random.Range(0f, 1f);

        float cumulative = 0f;

        for(int i =0; i < dropProbabilities.Count; i++)
        {
            cumulative += dropProbabilities[i];

            if (choice < cumulative)
            {
                return drops[i];
            }
        }

        return null;
    }
    #endregion
}