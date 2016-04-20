using UnityEngine;
using System.Collections;

public class Item_UnholyFragment : Item {
    
    public AudioClip pickupSound;

    void Start()
    {
        base.Start();
        name = "Unholy Fragment";

        transform.rotation = Quaternion.Euler(0, 45, 0);
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        base.OnPickedUp(holder);
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);
        holder.damage++;
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
