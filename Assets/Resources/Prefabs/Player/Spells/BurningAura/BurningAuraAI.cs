using UnityEngine;
using System.Collections;

public class BurningAuraAI : MonoBehaviour
{

    public LayerMask layer;

    public int damage = 1;

    public float radius = 0.5f;

    public float delay = 2f;

    public string enemyTag = "Enemy";

    // Use this for initialization
    void Start()
    {
        StartCoroutine(BurnEnemies());
    }

    /* Burns enemy every two seconds. */
    public virtual IEnumerator BurnEnemies()
    {
        // we all random wait, so our AI frames are staggered:
        yield return new WaitForSeconds(Random.value * delay);
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layer);
            foreach (Collider c in colliders)
            {
                if (c.tag.Equals(enemyTag))
                {
                    c.GetComponent<Stats>().ChangeHealth(-damage);
                }
            }
            // Delay before continuing next iteration of the loop.
            yield return new WaitForSeconds(delay);
        } 
    }
}