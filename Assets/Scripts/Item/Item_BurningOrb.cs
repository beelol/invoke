using UnityEngine;
using System.Collections;

public class Item_BurningOrb : Item {

    public AudioClip pickupSound;
    public GameObject burningAuraPrefab;

    void Start()
    {
        base.Start();
        name = "Burning Orb";
        Vector3 newPos = transform.position;
        newPos.y = .5f;
        transform.position = newPos;
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        base.OnPickedUp(holder);
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);

        if (!PlayerStats.spawnedBurningAura)
        {
            GameObject burningAura = Instantiate(burningAuraPrefab, holder.transform.position, Quaternion.identity) as GameObject;
            burningAura.transform.SetParent(holder.transform);
            PlayerStats.spawnedBurningAura = true;
        }
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
