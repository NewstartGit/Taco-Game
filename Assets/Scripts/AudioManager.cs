using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR;

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

    //SFX
    public AudioClip correctOrder;
    public AudioClip incorrectOrder;


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
    
    public void StopAllAudio()
    {
        music.Stop();
        ambience.Stop();
        player.Stop();
    }

    public void Scream()
    {
        abyss.Play();
    }

    public void Order(bool correct)
    {
        if (correct)
        {
            SFXSource.clip = correctOrder;
            SFXSource.Play();
        }
        else
        {
            SFXSource.clip = incorrectOrder;
            SFXSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StopAllAudio();
        }
    }
}
