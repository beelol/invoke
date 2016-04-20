using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public float speed = 40;

    void Update()
    {
        // Rotate constantly.
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
