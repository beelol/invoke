using UnityEngine;
using System.Collections;
using Apex.Steering.Components;
using Apex.Steering;

/// <summary>
/// Basic AI class for all enemy monsters that chases the target.
/// Monster specific AI classes will inherit from this one.
/// 
/// This class can be used for any basic melee ranged monster that
/// attacks as soon as it is in range of the player and its attack
/// cooldown is over.
/// </summary>
public class EnemyAI_Slime : EnemyAI
{

    #region Functions

    /* Attacks the target. */
    public override void Attack()
    {        
        agent.Stop();
        GetComponent<UnitAnimator>().Attack();
        target.GetComponent<Stats>().ChangeHealth(-damage);

        if (attackSound != null)
        {
            GetComponent<AudioSource>().pitch = Random.Range(1f, 1.5f);
            GetComponent<AudioSource>().PlayOneShot(attackSound);
        }
    }

    /* Follows the target. */
    public override void Follow()
    {
        agent.MoveTo(new Vector3(target.position.x, transform.position.y, target.position.z), false);
        GetComponent<UnitAnimator>().Move();
    }

    /* Cancels movement. */
    public override void Stop()
    {
        GetComponent<UnitAnimator>().Stop();
    }

    /* Kills the unit. */
    public override void Die()
    {
        if (PlayerAI.targetToAttack == transform) PlayerAI.DeselectTarget();
        GetComponent<SteerableUnitComponent>().Stop();
        GetComponent<UnitAnimator>().Die();

        DisableComponents();
    }

    /* Fades if dead. */
    void Update()
    {
        if (GetComponent<Stats>().dead) FadeMaterial();
    }

    private void FadeMaterial()
    {
        Color c = transform.GetChild(1).GetComponent<Renderer>().material.color;
        c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime);
        transform.GetChild(1).GetComponent<Renderer>().material.color = c;

        if (c.a < 0.05f)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
