using UnityEngine;
using System.Collections;

public class Item_CrystallizedBlood : Item {

    public AudioClip pickupSound;

    void Start()
    {
        base.Start();
        name = "Crystallized Blood";
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        base.OnPickedUp(holder);
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);
        holder.GetComponent<Stats>().ChangeMaxHealth(3);
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
