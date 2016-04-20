using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    public AudioClip[] blasterSounds;
    public AudioClip thrusterSound;
	public float speed;
	public float tilt;

    public static bool controllerMode = false;
    public static bool keyboardMode = true;

	public Transform shotSpawn;
    public Transform laserSpawn;
    public static float hAxis;
    public static float vAxis;
	private float nextFire;

    public GameObject LaserSpark;
    public GameObject shot;
    public int pooledAmount = 100;
    public bool willGrow = true;
    List<GameObject> shots;
    LineRenderer lr;
    RaycastHit hit;
    Transform laserSmoke;
    public List<Transform> bolters = new List<Transform>();

    bool laserEquipped = false;
    bool bolterEquipped = true;

    public float maxLaserLength = 13;
    public float currentLength = 0.1f;
    public float laserExtensionPerSecond = 50;
    float nextAdd=0;
    public float accelerationRate;
    public float decelerationRate;
    float maxVelocity = 60;
    Stats statsScript;

    void Start()
    {                
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
       // accelerationRate = GetComponent<Stats>().accelerationRate;
        //decelerationRate = GetComponent<Stats>().decelerationRate;
        //statsScript = GetComponent<Stats>();
        //LaserSpark = transform.GetChild(7).gameObject;
        //foreach (Transform child in transform) //adds all guns to list so they all shoot at once when firing. 
        //{
        //    if (child.tag.Equals("shotSpawn"))
        //    {
        //        bolters.Add(child);
        //    }
        //}

        Control.allowPlayerControls = true;

        //lr = laserSpawn.GetComponent<LineRenderer>();
        //toggleLineRenderer(false);
        //shots = new List<GameObject>();
        //laserSmoke = transform.FindChild("laserSmoke");

        //for (int i = 0; i < pooledAmount; i++)
        //{
        //    GameObject shotInstance = Instantiate(shot) as GameObject;
        //    shotInstance.SetActive(false);
        //    shotInstance.transform.GetChild(1).gameObject.SetActive(false);
        //    shotInstance.GetComponent<PulseScript>().shooter = gameObject;
        //    shots.Add(shotInstance);
        //    shotInstance.hideFlags = HideFlags.HideInHierarchy;
        //}

        //ignore collisions between player and raycastPlane.
        //Physics.IgnoreCollision(gameObject.collider, GameController.raycastPlane.collider);

        Physics.IgnoreLayerCollision(8, 8);
    }

    void Update()
    {
        
        //if (Input.GetKeyDown(KeyCode.Insert))
        //{
        //    keyboardMode = !keyboardMode;
        //    controllerMode = !controllerMode;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    laserEquipped = false;
        //    bolterEquipped = true;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    laserEquipped = true;
        //    bolterEquipped = false;
        //}
	}

    void FixedUpdate()
    {
        // Store direction for player to look (toward movement)
        Vector3 dir = new Vector3(hAxis,
            0, vAxis);

        //If we can move, look where we can move. Otherwise just look forward.
        if (Control.allowPlayerControls)
        {
            // If we're moving, face that direction
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * 10);
            }

            //Move by wasd
            Vector3 movement = (new Vector3(hAxis * (2 - Mathf.Abs(vAxis)), 0, vAxis * (2 - Mathf.Abs(hAxis))));

            // Move in the direction of wasd with gravity.
            GetComponent<CharacterController>().SimpleMove(movement*3);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(Vector3.forward),
                    Time.deltaTime * 10);
        }

        //Axes
        if (keyboardMode)
        {
            hAxis = Input.GetAxis("Horizontal"); //floating point value from 0 to 1 representing input of keyboard (time)
            vAxis = Input.GetAxis("Vertical"); //floating point value from 0 to 1 representing input of keyboard (time)
        }
        else if (controllerMode)
        {
            hAxis = Input.GetAxis("360_ControlStick_X"); //floating point value from 0 to 1 representing input of xbox left stick (position)
            vAxis = Input.GetAxis("360_ControlStick_Y"); //floating point value from 0 to 1 representing input of xbox left stick (position)
        }



        //Movement
        //if (!Control.inStrafeMode)
        //{
        //    if (vAxis > 0 && GetComponent<Rigidbody>().velocity.magnitude < maxVelocity)
        //        GetComponent<Rigidbody>().AddForce(transform.forward * statsScript.accelerationRate, ForceMode.Acceleration);
        //    else if (vAxis < 0 && GetComponent<Rigidbody>().velocity.magnitude < maxVelocity)
        //        GetComponent<Rigidbody>().AddForce(-transform.forward * statsScript.accelerationRate, ForceMode.Acceleration);
        //    //else
        //        //rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.zero, Time.deltaTime);

        //}
        //else //if !strafing, don't move left/right
        //{
        //    rigidbody.velocity =
        //        transform.TransformDirection(new Vector3(0, rigidbody.velocity.y, vAxis * (2 - Mathf.Abs(hAxis)) * Player.entity.GetSpeed()));
        //}

        //if (Input.GetAxis("360_R_Trigger") > 0 || Input.GetButton("Fire1") || (Input.GetButton("Fire2")))
        //{
        //    if (laserEquipped)
        //    {
        //        FireLaser();
        //    }
        //    else if (bolterEquipped)
        //    {
        //        if (Time.time > nextFire)
        //        {
        //            nextFire = Time.time + Player.entity.GetFireRate();
        //            if (bolterEquipped)
        //                Fire();
        //        }

        //    }

        //}

        //if (Input.GetAxis("360_CameraStick_X") > 0 || Input.GetAxis("360_CameraStick_X") < -0.1)
        //{
        //    Debug.Log("360_CameraStick_X Axis Value: " + Input.GetAxis("360_CameraStick_X"));
        //}
        //if (Input.GetAxis("360_CameraStick_Y") > 0 || Input.GetAxis("360_CameraStick_Y") < -0.1)
        //{
        //    Debug.Log("360_CameraStick_Y Axis Value: " + Input.GetAxis("360_CameraStick_Y"));
        //}

        //if (!laserEquipped || (!Input.GetButton("Fire1") && !Input.GetButton("Fire2") && Input.GetAxis("360_R_Trigger") <= 0))
        //{
        //    currentLength = 0;
        //    toggleLineRenderer(false);
        //    laserSmoke.GetComponent<ParticleEmitter>().emit = false;
        //    LaserSpark.SetActive(false);
        //}
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rigidbody.velocity.x * -tilt);
        //transform.Rotate(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
        
    }

    void Fire()
    {
        foreach (Transform bolter in bolters)
        {
            if (bolter.gameObject.activeSelf)
            {
                GameObject shotInstance = GetPooledObject();
                if (shotInstance == null) return; //don't fire if there is no bolt. this should never happen. kills function for testing.
                shotInstance.transform.position = bolter.position;
                shotInstance.transform.rotation = bolter.rotation;
                shotInstance.SetActive(true);
                shotInstance.transform.GetChild(1).gameObject.SetActive(true);
                shotInstance.GetComponent<Rigidbody>().velocity = shotInstance.transform.forward * shotInstance.GetComponent<PulseScript>().speed;
                //shotInstance.GetComponent<FirstBoltScript>().Invoke("DisableWithoutDestroy", 2f);

                playShotSound();
            }
        }
    }

    private void playShotSound()
    {
        int clipChosen = Random.Range(0, blasterSounds.Length);
        GetComponent<AudioSource>().pitch = Random.Range(1f, 1.5f);
        GetComponent<AudioSource>().PlayOneShot(blasterSounds[clipChosen]);
    }

    //Chooses the next inactive shot to be set active to be fired.\
    //part of object pooling system
    public GameObject GetPooledObject()
    {
        //if there is an inactive shot
        for (int i = 0; i < shots.Count; i++)
        {
            if (!shots[i].activeInHierarchy)
            {
                //return it
                return shots[i];
            }

        }

        //otherwise, make a new one and return it after adding it to list.
        if (willGrow)
        {
            GameObject shotInstance = Instantiate(shot) as GameObject;
            shots.Add(shotInstance);
            return shotInstance;
        }
        return null;
    }

    void toggleLineRenderer(bool state)
    {
        lr.enabled = state;
    }
}

