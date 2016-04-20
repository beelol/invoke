using UnityEngine;
using System.Collections;

public class ExitBehavior : MonoBehaviour {

	void OnTriggerEnter(Collider c)
    {
        if (c.tag.Equals("Player"))
        {
            transform.parent.GetComponent<ceDungeonGenerator>().CreateDungeon();
        }
    }
}
