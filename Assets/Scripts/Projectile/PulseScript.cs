using UnityEngine;
using System.Collections;

public class PulseScript : MonoBehaviour {

    public GameObject shooter;
    public float speed;
    public AudioClip[] impactSounds;
    public GameObject spark;
    private float damage = 10f;
    AudioClip impactSoundChosen;

    void Start()
    {
        int impactSoundChosenInt = Random.Range(0, impactSounds.Length);
        impactSoundChosen = impactSounds[impactSoundChosenInt];

        if (shooter)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), shooter.GetComponent<Collider>());
        }
            //Physics.IgnoreCollision(gameObject.collider, GameController.raycastPlane.collider);
        Invoke("DisableWithoutDestroy", 2f);
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.transform.tag.Equals("raycastPlane"))
            return;

            //audio.PlayOneShot(impactSoundChosen);
            Instantiate(spark, transform.position, Quaternion.identity);

            //if ((transform.tag.Equals("Ally") && hit.transform.tag.Equals("enemy")) || 
            //    (transform.tag.Equals("enemy") && hit.transform.tag.Equals("Ally")) ||
            //    hit.transform.tag.Equals ("Player"))
            //{
            if (hit.transform.GetComponent<Stats>())
            {
                if (transform.tag.Equals("Ally") || hit.transform.tag.Equals("Player") || transform.tag.Equals("enemy"))
                {
                    //hit.transform tries to take damage but is null after dying.
                    //hit.transform.GetComponent<Stats>().TakeDamage(damage, "dildoGun", shooter.GetComponent<Stats>());
                }
            }
            DisableWithoutDestroy();
            //move this to game manager to play the sound at the asteroid so it doesn't have a problem playing
            //Invoke("DisableWithoutDestroy", 0.3f);

        
    }

    void DisableWithoutDestroy()
    {
        CancelInvoke();
        transform.GetChild(1).gameObject.SetActive(false);
        gameObject.SetActive(false);
        //collider.enabled = false;
        //rigidbody.isKinematic = true;
        //transform.GetChild(0).renderer.enabled = false;
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
