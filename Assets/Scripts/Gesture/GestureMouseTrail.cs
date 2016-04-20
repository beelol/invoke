using UnityEngine;
using System.Collections;

public class GestureMouseTrail : MonoBehaviour {

    public float Distance = 10;

    Camera camera;

    void Start()
    {
        camera = GetComponent<GestureController>().camera;
    }

	// Update is called once per frame
	void Update () {
        Ray r = camera.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = r.GetPoint(Distance);
        transform.position = pos;
	}
}
