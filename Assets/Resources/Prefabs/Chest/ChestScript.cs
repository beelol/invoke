using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestScript : MonoBehaviour {

    Material[] materials;
    bool opened = false;
    bool dropItem = true;
    public List<GameObject> drops;
    public List<float> dropProbabilities;

	// Use this for initialization
	void Start () {
        materials = GetComponent<Renderer>().materials;
	}
	
	// Update is called once per frame
	void Update () {
        if (opened)
        {
            foreach (Material mat in materials)
            {
                Color c = mat.color;
                c.a -= 0.05f;
                mat.color = c;

                if (dropItem)
                {
                    if (mat.color.a < 0.05)
                    {
                        dropItem = false;
                        DropItem();
                        Destroy(gameObject);
                    }
                }
            }
        }
	}

    void DropItem()
    {
        GameObject drop = PickItem();
        if (drop != null)
        {
            GameObject dropInstance = Instantiate(drop, transform.position, Quaternion.identity) as GameObject;
        }
    }

    GameObject PickItem()
    {
        float choice = Random.Range(0f, 1f);

        float cumulative = 0f;

        for (int i = 0; i < dropProbabilities.Count; i++)
        {
            cumulative += dropProbabilities[i];

            if (choice < cumulative)
            {
                return drops[i];
            }
        }

        return null;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag.Equals("Player"))
        {
            opened = true;
        }
    }
}
