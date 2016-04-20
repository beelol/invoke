using UnityEngine;
using System.Collections;

public class Item_LimburgerCheese : Item {

    public AudioClip pickupSound;

    void Start()
    {
        base.Start();
        name = "Cheese";

        transform.rotation = Quaternion.Euler(270, 0, 0);
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        base.OnPickedUp(holder);
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);
        holder.ChangeHealth(3);
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
