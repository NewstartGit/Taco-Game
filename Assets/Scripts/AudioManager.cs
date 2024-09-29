using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource ambience;
    public AudioSource abyss;
    public AudioSource SFXSource;
    public AudioSource player;


    public AudioClip background;
    public AudioClip hum;
    public AudioClip scream;
    public AudioClip breathing;
    public AudioClip footsteps;


    // Start is called before the first frame update
    void Start()
    {
        music.clip = background;
        ambience.clip = hum;
        abyss.clip = scream;
        player.clip = breathing;

        ambience.time = 6f;
        abyss.time = 20.5f;

        music.Play();
        ambience.Play();
        player.Play();
    }

    public void StopAllMusic()
    {
        music.Stop();
        ambience.Stop();
    }

    public void Scream()
    {
        abyss.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
