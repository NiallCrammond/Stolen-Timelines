using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Sound[] footsteps;

   public
        bool canPlay = true;



    [Range(0, 1)]
    public float stepDelay;

    public static AudioManager instance;
    public AudioMixerGroup mainMixer;
    
    void Awake()
    {
        if (instance == null)
        {
           instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in footsteps)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
      s.source.outputAudioMixerGroup = mainMixer;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = mainMixer;

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

    }

    public void playFootsteps()
    {

        Sound currentFootStep = footsteps[Random.Range(0, footsteps.Length)];
        currentFootStep.source.Play();
        Debug.Log("Playing: " + currentFootStep.name);

    }

    public IEnumerator randomFootSteps()
    {

        if (canPlay)
        {
            playFootsteps();
            canPlay = false;
            yield return new WaitForSeconds(stepDelay);
            canPlay = true;
        }
        else
        {

        }
    }
}
