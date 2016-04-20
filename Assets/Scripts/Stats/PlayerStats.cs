using UnityEngine;
using System.Collections;

public class PlayerStats : Stats {

    private bool invulnurable;

    public int extraSpellDamage = 0;

    public static bool spawnedBurningAura = false;

    #region Functions 

    /* Adds the passed amount of damage onto extra spell damage. */
    public void ChangeSpellDamage(int damage)
    {
        extraSpellDamage += damage;
    }

    /* Adds the passed amount of health onto current health. */
    public override void ChangeHealth(int amount)
    {
        if (dead) return;

        if (amount < 0 && invulnurable)
        {
            return;
        }


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
            invulnurable = true;
            Invoke("DisableInvulnurability", 1);

            if (hurtSound != null)
            {
                GetComponent<AudioSource>().pitch = Random.Range(1f, 1.25f);
                GetComponent<AudioSource>().PlayOneShot(hurtSound);
            }

            if (hurtTextPrefab != null)
            {
                SpawnHurtText(amount.ToString(), transform.position, Color.red);
            }
        }
        else
        {
            if (hurtTextPrefab != null)
            {
                SpawnHurtText("+" + amount.ToString(), transform.position, Color.green);
            }
        }

        GetComponent<StatDisplay>().UpdateCurrentHealth(oldHealth);
    }

    /* Disable invulnurability. */
    private void DisableInvulnurability()
    {
        invulnurable = false;
    }

    public void EnableInvulnurability()
    {
        invulnurable = true;
    }
    /* Adds passed amount of health onto max health. */
    public override void ChangeMaxHealth(int amount)
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

        GetComponent<StatDisplay>().UpdateMaxHealth(oldHealth);

    }

    /* Heal to full health. */
    public override void FillHealth()
    {
        ChangeHealth(currentHealth - maxHealth);
    }

    /* Kills the unit. */
    public override void Die()
    {
        //AudioPlayer.Stop();
        //AudioPlayer.PlayLoseClip();

        dead = true;
        GameController.EndGame();
        if (deathSound != null) 
        {
            GetComponent<AudioSource>().pitch = 1;
            GetComponent<AudioSource>().PlayOneShot(deathSound); 
        }
       
        GetComponent<PlayerAI>().Die();
        GetComponent<UnitAnimator>().Die();
    }

    /* Called in the update function to check for status effects. */
    public override void HandleStatusEffects()
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

    /* Runs status effects. */
    public override IEnumerator RunStatusEffects()
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

    #endregion
}
