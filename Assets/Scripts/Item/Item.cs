using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public string name {get; set;}

    public string effect { get; set; }

    public void Start()
    {
        effect = "";
        transform.position = new Vector3(transform.position.x, 0.15f, transform.position.z);
    }

    void Update()
    {
        // Rotate constantly.
        transform.Rotate(0, 0, 20 * Time.deltaTime);
    }

    /* Called once the player picks up this item */
    public virtual void OnPickedUp(Stats holder)
    {
        holder.SpawnHurtText("+1 " + name, transform.position, Color.yellow);
    }

    /* Called when the player uses this item. (Not sure if this will stay) */
    public virtual void Use()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag.Equals("Player"))
        {
            OnPickedUp(c.GetComponent<Stats>());
            Destroy(gameObject);
        }
    }

    //private IEnumerator printEffect(Stats holder)
    //{
        //print("piss");
        //yield return new WaitForSeconds(.1f);
        //print("piss");
        //if(effect.Length>0) holder.SpawnHurtText(effect, transform.position, Color.yellow);
    //    Destroy(gameObject);
    //}
}
