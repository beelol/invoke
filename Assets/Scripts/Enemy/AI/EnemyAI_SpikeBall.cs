using UnityEngine;
using System.Collections;
using Apex.Steering.Components;
using Apex.Steering;


public class EnemyAI_SpikeBall : EnemyAI {

    public GameObject explosionPrefab;

    public LayerMask layer;

    #region Functions

    /* Attacks the target. */
    public override void Attack()
    {
        Die();
    }

    void Update()
    {
        transform.Rotate(1, 0, 0);
    }

    /* Kills the unit. */
    public override void Die()
    {
        Explode();

        DisableComponents();

        GetComponent<Renderer>().enabled = false;

        Invoke("Destroy", 1f);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackDistance + .1f, layer);

        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                if (!collider.gameObject.Equals(gameObject))
                {
                    collider.GetComponent<Stats>().ChangeHealth(-damage);
                }
            }
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion
}
