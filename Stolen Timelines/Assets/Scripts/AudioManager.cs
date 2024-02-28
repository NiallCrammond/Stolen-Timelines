using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Sound[] footsteps;

<<<<<<< Updated upstream
    bool canPlay= true;
=======
    public Sound[] jumps;

    public Sound dash;

   public bool canPlayFootsteps = true;
    public bool canPlayJumps = true;

    public bool canPlayDash = true;


>>>>>>> Stashed changes

    [Range(0, 1)]
    public float stepDelay;

<<<<<<< Updated upstream

    void Start()
=======
    public static AudioManager instance;
    public AudioMixerGroup mainMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    void Awake()
>>>>>>> Stashed changes
    {

        foreach(Sound s in footsteps)
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
        
<<<<<<< Updated upstream
    }

    // Update is called once per frame
    void Update()
    {
        
    }
=======
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in footsteps)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
      s.source.outputAudioMixerGroup = sfxMixer;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = sfxMixer;

        }

        foreach (Sound s in jumps)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = sfxMixer;

        }

        dash.source = gameObject.AddComponent<AudioSource>();
        dash.source.clip = dash.clip;
        dash.source.volume = dash.volume;
        dash.source.pitch = dash.pitch;
        dash.source.outputAudioMixerGroup = sfxMixer;

    }

    // Update is called once per frame
    
>>>>>>> Stashed changes

    public void playSound(string name)
    {
      Sound s =  Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

    }

    public void playFootsteps()
    {

        Sound currentFootStep = footsteps[Random.Range(0, footsteps.Length)];
        Debug.Log("Playing: " + currentFootStep.name);

    }

    public void playJumpSound()
    {

        if(canPlayJumps)
        {
            canPlayJumps = false;
        Sound jumpSound = jumps[Random.Range(0, jumps.Length)];
        jumpSound.source.Play();
        Debug.Log("Jump Sound Played: " + jumpSound.name);
        }
    }

    public void playDashSound()
    {
       if(canPlayDash)
        {

            canPlayDash = false;
        dash.source.Play();
       Debug.Log("Dash Played");
        }
        

    }

    public IEnumerator randomFootSteps()
    {
<<<<<<< Updated upstream
        if(canPlay)
        {
        playFootsteps();
            canPlay = false;
=======

        if (canPlayFootsteps)
        {
            playFootsteps();
           canPlayFootsteps = false;
>>>>>>> Stashed changes
            yield return new WaitForSeconds(stepDelay);
            canPlayFootsteps = true;
        }
        else
        {
            
        }
    }
}
