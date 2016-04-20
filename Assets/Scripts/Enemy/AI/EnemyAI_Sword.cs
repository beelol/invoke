using UnityEngine;
using System.Collections;
using Apex.Steering.Components;
using Apex.Steering;

public class EnemyAI_Sword : EnemyAI {

    #region Functions

    /* Attacks the target. */
    public override void Attack()
    {
        agent.Stop();
        GetComponent<UnitAnimator>().Attack();
        target.GetComponent<Stats>().ChangeHealth(-1);

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
        Material[] mats = GetComponent<Renderer>().materials;

        foreach (Material mat in mats)
        {
            Color c = mat.color;
            c.a -= 0.05f;
            mat.color = c;
            if (c.a < 0.05f)
            {
                Destroy(gameObject);
            }
        }        
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.transform.tag.Equals("Player"))
        {
            c.transform.GetComponent<Stats>().ChangeHealth(-damage);
        }
    }

    #endregion
}
