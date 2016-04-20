using UnityEngine;
using System.Collections;

public class Item_SpellTome : Item {

    public AudioClip pickupSound;

    void Start()
    {
        effect = "";

        transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);

        name = "Spell Tome";

        int nameIndex = Random.Range(0, 4);
        
        switch (nameIndex)
        {
            case 0:
                name = "Fire Ball";
                break;
            case 1:
                name = "Wind Wave";               
                break;
            case 2:
                name = "Arcane Beam";
                break;
            case 3:
                name = "Chain Lightning";
                break;
        }
        
        transform.rotation = Quaternion.Euler(0, 0, 135);
    }

    void Update()
    {
        // Rotate constantly.
        transform.Rotate(0, 20 * Time.deltaTime, 0, Space.World);
    }

    /* Called once the player picks up this item */
    public override void OnPickedUp(Stats holder)
    {
        holder.SpawnHurtText("Learned " + name + " spell!", transform.position, new Color(255, 0, 255));
        holder.GetComponent<AudioSource>().PlayOneShot(pickupSound);
        if(!name.Equals("Spell Tome")) holder.GetComponent<PlayerAI>().LearnSpell(name);
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public override void Use()
    {
        // Leave empty if activated upon pickup.
    }
}
