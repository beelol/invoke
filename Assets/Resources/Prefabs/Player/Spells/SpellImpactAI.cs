using UnityEngine;
using System.Collections;

public class SpellImpactAI : MonoBehaviour {

    public float lifetime = 1f;

	// Use this for initialization
	void Start () {        
        Invoke("Die", lifetime);
	}

    private void Die(){
        Destroy(gameObject);
    }
}

