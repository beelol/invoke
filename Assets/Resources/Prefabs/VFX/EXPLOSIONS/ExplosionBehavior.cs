using UnityEngine;
using System.Collections;

public class ExplosionBehavior : MonoBehaviour {

    void Start()
    {
        Invoke("Destroy", 1f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
