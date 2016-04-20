using UnityEngine;
using System.Collections;

public class SpellAI : MonoBehaviour {
    
    // Target to follow. Assigned upon instantiation.
    public Transform target;

    // Impact particle system prefab. Assign in inspector.
    public GameObject impact;

    // Range of the spell. Assign in inspector.
    public float blastRadius = 0;

    // Damage of the spell. Assign in inspector.
    public int damage = 1;

    // How fast the spell will move. Assign in inspector.
    public float speed = 1;

    // Flag used so that trigger enter is called once.
    private bool hit = false;

    // Determines whether spell should follow target or just shoot in its direction.
    public bool homing = true;

    // If it moves or not.
    public bool mobile = true;

    // How many times it should hit something.
    public int numberOfHits = 1;

    // If we are not homing, we will move to this position.
    private Vector3 destination;

    // How long before destroying in case it never reaches it's end.
    public float lifetime = 10f;

    // GameObject layer to hit if AoE spell. Only works through inspector because you know, unity.
    public LayerMask layer;

    void Start()
    {
        if (!homing)
        {
            destination = target.position;
        }

        if (!mobile)
        {
            StartCoroutine(Hit());
        }

        Invoke("Destroy", lifetime);
    }

    private IEnumerator Hit()
    {
        int hits = 0;

        while (hits < numberOfHits)
        {
            if (target) target.GetComponent<Stats>().ChangeHealth(-damage);
            hits++;
            yield return new WaitForSeconds(.2f);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    // Move toward the enemy this was shot at.
	void Update () {
        if (mobile)
        {
            if (homing)
            {
                if (target)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            }
        }
        else
        {
            if (target)
            {
                Vector3 lookPos = target.position;
                lookPos.y = transform.position.y;
                transform.LookAt(lookPos);
            }
        }
	}

    // Upon entering a trigger.
    void OnTriggerEnter(Collider c)
    {
        if (hit) return;

        // If we hit anything that is not the player, then destroy this object
        if (!c.transform.tag.Equals(transform.tag))
        {
            hit = true;
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
            Destroy(gameObject);
        }
    }
}
