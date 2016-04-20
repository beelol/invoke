using UnityEngine;
using System.Collections;

/// <summary>
/// This script tells the camera to smoothly follow a given target, which
/// is usually the main player character.
/// 
/// It is attached to the main camera.
/// </summary>
public class SmoothFollow : MonoBehaviour
{

    #region Variables

    // The target we are following
    public Transform target;

    // Changes how close the camera is (lower farther) and how smooth the camera follows since this is the lerp value.
    public float smoothing = 10f; 

    // Camera position offset on x Axis
    public float xOffset = -17;

    // Camera position offset on y Axis
    public float yOffset = 0;

    // Camera position offset on z Axis
    public float zOffset = -17;

    // The camera's normal height.
    public float cameraHeight = 29;

    // Position offset on all axes
    Vector3 offset = new Vector3();

    // Camera position offset on x Axis
    public float xRotation = 50;

    // Camera position offset on y Axis
    public float yRotation = 45;

    // Camera position offset on z Axis
    public float zRotation = 0;

    // The camera's normal rotation.
    Quaternion rotation = new Quaternion();

    // This transform. (Performance improvemetn)
    Transform _transform;

    // Determines whether camera is following player or free moving.
    public bool followPlayer = true;

    #endregion

    #region Functions
    void Start()
    {
        _transform = transform;

        
        rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        _transform.rotation = rotation;
    }

    void FixedUpdate()
    {
        offset = new Vector3(xOffset, yOffset, zOffset);
        // Do nothing if the target does not exist.
        if (!target)
            return;

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    followPlayer = !followPlayer;
        //}

        if (followPlayer)
        {
            // Smoothly follow the player.
            Vector3 targetCamPos = new Vector3(target.position.x, cameraHeight, target.position.z) + offset;
            _transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
        else
        {
            // floating point value from 0 to 1 representing input of keyboard (time)
            float hAxis = Input.GetAxis("Horizontal");

            // floating point value from 0 to 1 representing input of keyboard (time)
            float vAxis = Input.GetAxis("Vertical");

            // Follow keyboard controls
            _transform.Translate(new Vector3(hAxis, vAxis, 0));
        }
    }

    //float Zoom()
    //{
    //    return 0;

        //if (Interface.showInventory)
        //    return transform.position.y;

        //if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    //if (Camera.main.transform.position.y + zoomVal < 30 + zoomVal)
        //    //{
        //        return (Camera.main.transform.position + Vector3.up * zoomVal).y;
        //    //}
        //}

        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    if (Camera.main.transform.position.y - zoomVal > 10 - zoomVal)
        //    {
        //        return (Camera.main.transform.position - Vector3.up * zoomVal).y;
        //    }
        //}
        //return Camera.main.transform.position.y;
    //}

    #endregion
}
