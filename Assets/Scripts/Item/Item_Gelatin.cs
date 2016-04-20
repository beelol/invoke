using UnityEngine;
using System.Collections;

public class Item_Gelatin : Item {

    public AudioClip pickupSound;

    void Start()
    {
        base.Start();
        name = "Gelatin";
        effect = "+1 Max Health";
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        base.OnPickedUp(holder);
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);
        holder.GetComponent<Stats>().ChangeMaxHealth(1);
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
