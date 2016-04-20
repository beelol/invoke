using UnityEngine;
using System.Collections;

public class SelfSpellAI : MonoBehaviour {

    // How long the spell will last before being destroyed.
    public float lifetime = 3f;

    // Radius in which enemies will be hit.
    public float range = 1f;

    // GameObject layer to hit if AoE spell. Only works through inspector because you know, unity.
    public LayerMask layer;

    // Damage the spell will deal.
    public int damage = 1;

	// Use this for initialization
	void Start () {

        if (range > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, layer);

            foreach (Collider collider in colliders)
            {
                if (collider.isTrigger)
                {
                    if (collider.transform.tag.Equals("Enemy") && transform.tag.Equals("Player") ||
                        transform.tag.Equals("Enemy") && collider.transform.tag.Equals("Player"))
                    {
                        collider.GetComponent<Stats>().ChangeHealth(-damage);
                    }
                }
            }
        }

        Invoke("Destroy", lifetime);
	}

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
