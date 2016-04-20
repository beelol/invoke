using UnityEngine;
using System.Collections;
using Apex.Steering.Components;
using Apex.Steering;

using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Basic AI class for all enemy monsters that chases the target.
/// Monster specific AI classes will inherit from this one.
/// 
/// This class can be used for any basic melee ranged monster that
/// attacks as soon as it is in range of the player and its attack
/// cooldown is over.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    #region Variables

    #region Attack variables

    // Amount of damage dealt with one attack.
    public int damage = 1;

    // Amount of damage dealt with one attack.
    public AttackType attackType = AttackType.Melee;

    #endregion

    #region Movement Variables

    // Apex path agent
    public SteerForPathComponent agent = null;

    // Target to attack. 
    public Transform target;

    // Distance from which the target can be detected.
    public float detectDistance = 3;

    // Distance from which the target can be attacked.
    public float attackDistance = 2;

    // Cooldown of the enemy's attack.
    public float attackCooldown = 2;

    // Next time the player can attack based on CD.
    private float nextAttackTime = 0;

    // Delay before the AI is run again.
    private float AI_DELAY = 0.5f;

    // Renderer of object to fade. Need this as it's on a child object.
    public Renderer _renderer;

    // Projectile to launch.
    public GameObject rangedAttack;

    // Speed of projectile
    public float projectileSpeed = 0;

    #endregion

    #region Audio Variables

    public AudioClip attackSound;

    #endregion

    #endregion

    #region Functions

    /* Assigns target and agent. */
    void Start()
    {
        agent = GetComponent<SteerForPathComponent>();

        if (GameController.player != null)
        {
            if (GameController.player.transform)
                target = GameController.player.transform;
        }

        StartCoroutine(RunAI());
    }

    /* Attacks the target. */
    public virtual void Attack()
    {
        agent.Stop();
        GetComponent<UnitAnimator>().Attack();

        if (attackType == AttackType.Melee)
        {
            target.GetComponent<Stats>().ChangeHealth(-damage);
        }

        GameObject projectile = null;

        switch (attackType)
        {            
            case AttackType.Melee:
                target.GetComponent<Stats>().ChangeHealth(-damage);
                break;
            case AttackType.Ranged:
                projectile = Instantiate(rangedAttack, transform.position, Quaternion.identity) as GameObject;
                projectile.GetComponent<SpellAI>().homing = false;
                projectile.tag = transform.tag;
                projectile.GetComponent<SpellAI>().target = target;                
                break;
            case AttackType.RangedHoming:
                projectile = Instantiate(rangedAttack, transform.position, Quaternion.identity) as GameObject;
                projectile.GetComponent<SpellAI>().homing = true;
                projectile.tag = transform.tag;
                projectile.GetComponent<SpellAI>().target = target;
                break;
            case AttackType.Collision:
                break;
            default:
                target.GetComponent<Stats>().ChangeHealth(-damage);
                break;
        }

        if (projectileSpeed > 0 && projectile != null)
        {
            projectile.GetComponent<SpellAI>().speed = projectileSpeed;
        }

        if (attackSound != null)
        {
            GetComponent<AudioSource>().pitch = Random.Range(1f, 1.5f);
            GetComponent<AudioSource>().PlayOneShot(attackSound);
        }
    }

    /* Follows the target. */
    public virtual void Follow()
    {
        agent.MoveTo(new Vector3(target.position.x, transform.position.y, target.position.z), false);
        GetComponent<UnitAnimator>().Move();
    }

    /* Cancels movement. */
    public virtual void Stop()
    {
        GetComponent<UnitAnimator>().Stop();
    }

    /* Kills the unit. */
    public virtual void Die()
    {
        if (PlayerAI.targetToAttack == transform) PlayerAI.DeselectTarget();

        GetComponent<SteerableUnitComponent>().Stop();
        GetComponent<UnitAnimator>().Die();

        DisableComponents();
    }

    /* Disable components and monobehaviours. */
    public virtual void DisableComponents()
    {
        MonoBehaviour[] comps = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour c in comps)
        {
            if (!c.Equals(this) && !c.Equals(GetComponent<Stats>())) c.enabled = false;            
        }

        GetComponent<CapsuleCollider>().enabled = false;

        if (GetComponent<CharacterController>())
        {
            GetComponent<CharacterController>().enabled = false;
        }
    }

    /* Detects the target and attacks it if in range. */
    public virtual IEnumerator RunAI()
    {
        // we all random wait, so our AI frames are staggered:
        yield return new WaitForSeconds(Random.value * AI_DELAY);
        while (true)
        {
            float Distance = 0;
            if (target)
            {
                Distance = Vector3.Distance(transform.position, target.position);

                if (Distance <= attackDistance)
                {
                    if (Time.time > nextAttackTime)
                    {
                        nextAttackTime = Time.time + attackCooldown;
                        Attack();
                    }
                }
                else if (Distance <= detectDistance)
                {
                    Follow();
                }
                else
                {
                    Stop();
                }
            }

            // optional playing with delay:
            float delay = AI_DELAY;
            if (Distance > detectDistance) delay *= 2;
            else delay /= 2;
            yield return new WaitForSeconds(delay);

            if (GetComponent<Stats>())
            {
                if (GetComponent<Stats>().dead)
                {
                    break;
                }
            }

        } // AI loop
    }

    #endregion

    public enum AttackType { RangedHoming, Ranged, Melee, Collision }

}
