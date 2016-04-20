using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellAI_ChainLightning : SpellAI {

    // Number of jumps between enemies before dying.
    public int jumps = 5;

    private int currentJumps = 0;

    private List<Collider> hitEnemies = new List<Collider>();

    void OnTriggerEnter(Collider c)
    {
        if (hitEnemies.Contains(c)) return;

        if (currentJumps >= jumps) return;

        hitEnemies.Add(c);
       
        currentJumps++;

        // If we hit anything that is not the player, then destroy this object
        if (!c.transform.tag.Equals(transform.tag))
        {
            // If this is a single target spell:
            if (blastRadius <= 0)
            {
                GameObject impactInstance = Instantiate(impact, transform.position, Quaternion.identity) as GameObject;
                impactInstance.transform.SetParent(c.transform);
                // If we hit an enemy.
                if (c.transform.tag.Equals("Enemy") && transform.tag.Equals("Player") ||
                    transform.tag.Equals("Enemy") && c.transform.tag.Equals("Player"))
                {
                    if (c.transform.GetComponent<Stats>())
                    {
                        //Hurt it.
                        c.transform.GetComponent<Stats>().ChangeHealth(-damage);
                    }
                }
            }

            // If this is an AoE spell:
            else
            {
                // Then get all units within range and hurt them
                Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius, layer);

                foreach (Collider collider in colliders)
                {
                    // So the character controller is not considered
                    // because you can't disable that...
                    if (collider.isTrigger)
                    {
                        if (collider.transform.tag.Equals("Enemy") && transform.tag.Equals("Player") ||
                            transform.tag.Equals("Enemy") && collider.transform.tag.Equals("Player"))
                        {
                            collider.GetComponent<Stats>().ChangeHealth(-damage);
                            GameObject impactInstance = Instantiate(impact, collider.transform.position, Quaternion.identity) as GameObject;
                        }
                    }
                }
            }

            Collider[] cols = Physics.OverlapSphere(transform.position, 3f, layer);

            foreach (Collider col in cols)
            {
                if (col.tag.Equals("Enemy"))
                {
                    target = col.transform;
                    break;
                }

                // If we hit no enemies.
                Destroy(gameObject);
            }
            
            if (currentJumps > jumps)
            {
                Destroy(gameObject);
            }
        }
    }
}
