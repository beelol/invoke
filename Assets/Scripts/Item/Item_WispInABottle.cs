using UnityEngine;
using System.Collections;

public class Item_WispInABottle : Item {

    public AudioClip pickupSound;

    void Start()
    {
        base.Start();
        name = "Wisp In A Bottle";
        effect = "+1 Spell Damage";
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        base.OnPickedUp(holder);
        GameController.player.GetComponent<PlayerStats>().ChangeSpellDamage(1);
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
