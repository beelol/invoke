using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The AudioPlayer script provides the ability to play every provided track randomly
/// and to allow making singleton calls to play audio.
/// 
/// This script is attached to the GameController gameObject.
/// </summary>
public class AudioPlayer : MonoBehaviour {

    // All audioclips that can be used.
    public List<AudioClip> audioClips = new List<AudioClip>();

    // Audio clips that have not yet been played.
    private List<AudioClip> unplayedAudioClips = new List<AudioClip>();

    public AudioClip deathSynth;

    public AudioClip winSynth;

    //sound to play upon hovering over any button
    //public AudioClip buttonHover;
    //sound to play upon clicking any button
    //public AudioClip buttonClick;

    // 2D music player.
    public static AudioSource music;

    // 2D audio player.
    private static AudioSource interfaceAudio;

    // Audio source of which to fade volume
    private static AudioSource sourceToFade;

    // Fade out music in update.
    static bool fadeOutMusic = false;
    
    // Fade in music in update.
    static bool fadeInMusic = false;

    // Audio clip to play next.
    AudioClip chosenAudioClip;

    // Length of the current track; used to know when to play next
    private float length;

    // Audio clip to play once game is over.
    AudioClip[] gameOver;

    public AudioClip bossMusic;

    private static GameObject audioPlayer;    

    //sets declared attributes
	void Start () {

        // Set 2D music player
        music = GetComponent<AudioSource>();

        //set 2D sound player
        interfaceAudio = transform.GetChild(0).GetComponent<AudioSource>();

        //refill audio clip list once all tracks have been played.
        RefreshAudioClipList();

        //randomly choose a clip, then play it
        ChooseAClip();

        audioPlayer = gameObject;

        
	}

    //check if music should fade
    void Update()
    {
        if (fadeOutMusic)
        {
            FadeOutMusic(music);
        }
        if (fadeInMusic)
        {
            FadeInMusic(music);
        }       
    }

    //upon clicking any button, this should be called. Set through unity inspector
    //public void PlayButtonClick()
    //{
    //    //Play2DSound(buttonClick);
    //}

    ////upon hovering over any button, this should be called. Set through unity inspector
    //public void PlayButtonHover()
    //{
    //    //Play2DSound(buttonHover);
    //}

    public static void PlayWinClip()
    {
        Play2DSound(audioPlayer.GetComponent<AudioPlayer>().winSynth);
    }

    public static void PlayLoseClip()
    {
        Play2DSound(audioPlayer.GetComponent<AudioPlayer>().deathSynth);
    }

    //play any sound in 2D
    public static void Play2DSound(AudioClip sound)
    {
        interfaceAudio.PlayOneShot(sound);
    }

    public void LoopBossMusic()
    {
        GetComponent<AudioSource>().PlayOneShot(bossMusic);
        Invoke("LoopBossMusic", bossMusic.length);
    }

    public void StopPlayingMusic()
    {
        GetComponent<AudioSource>().Stop();
        CancelInvoke();
    }

    public static void Stop()
    {
        audioPlayer.GetComponent<AudioSource>().Stop();
        audioPlayer.GetComponent<AudioPlayer>().CancelInvoke();
    }

    /* Fades in a specified audiosource */
    public static void FadeInMusic(AudioSource audioSource)
    {
        fadeInMusic = true;
        //sourceToFade = audioSource;
        if (audioSource.volume < 1)
            audioSource.volume += .005f;
        else
            fadeInMusic = false;
    }

    /* Fades out a specified audiosource. */
    public static void FadeOutMusic(AudioSource audioSource)
    {
        fadeOutMusic = true;
        //fadeOutMusic = true;
        //sourceToFade = audioSource;
        if (audioSource.volume > 0)
            audioSource.volume -= .005f;
        else
            fadeOutMusic = false;
    }

    /* Randomly chooses a clip out that has not yet been played. */
    void ChooseAClip()
    {
        if (unplayedAudioClips.Count == 0)
        {
            RefreshAudioClipList();
        }

        chosenAudioClip = unplayedAudioClips[Random.Range(0, unplayedAudioClips.Count)];
        GetComponent<AudioSource>().PlayOneShot(chosenAudioClip);
        length = chosenAudioClip.length;
        unplayedAudioClips.Remove(chosenAudioClip);
        Invoke("ChooseAClip", chosenAudioClip.length);
    }

    //resets audio to start state after all tracks are played (makes sure all are played, then replays them)
    void RefreshAudioClipList()
    {
        foreach (AudioClip clip in audioClips)
        {            
            unplayedAudioClips.Add(clip);
        }
    }
 
}
