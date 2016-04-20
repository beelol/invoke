using UnityEngine;
using System.Collections;

public class AudioLooper : MonoBehaviour {

    public AudioClip loop;

	// Use this for initialization
	void Start () {
        LoopAudio();
	}

    void LoopAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(loop);
        Invoke("LoopAudio", loop.length);
    }
}
