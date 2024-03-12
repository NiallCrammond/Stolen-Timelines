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

    public Sound[] jumps;

    public Sound dash;

    public Sound music;

   public bool canPlayFootsteps = true;
    public bool canPlayJumps = true;

    public bool canPlayDash = true;

    [Range(0, 1)]
    public float footStepVolume;
    [Range(0, 1)]
    public float jumpVolume;






    [Range(0, 1)]
    public float stepDelay;

    public static AudioManager instance;
    public AudioMixerGroup mainMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

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
            s.source.volume = footStepVolume;
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
            s.source.volume = jumpVolume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = sfxMixer;

        }

        dash.source = gameObject.AddComponent<AudioSource>();
        dash.source.clip = dash.clip;
        dash.source.volume = dash.volume;
        dash.source.pitch = dash.pitch;
        dash.source.outputAudioMixerGroup = sfxMixer;

        music.source = gameObject.AddComponent<AudioSource>();
        music.source.clip = music.clip;
        music.source.volume = music.volume;
        music.source.pitch = music.pitch;
        music.source.outputAudioMixerGroup = musicMixer;

    }

    private void Update()
    {
        foreach (Sound s in footsteps)
        {
 
            s.source.volume = footStepVolume;

        }

        foreach (Sound s in jumps)
        {

            s.source.volume = jumpVolume;

        }


    }

    // Update is called once per frame


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

    public void playJumpSound()
    {

        if(canPlayJumps)
        {
            canPlayJumps = false;
        Sound jumpSound = jumps[Random.Range(0, jumps.Length)];
        jumpSound.source.Play();
        //Debug.Log("Jump Sound Played: " + jumpSound.name);
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

        if (canPlayFootsteps)
        {
            playFootsteps();
           canPlayFootsteps = false;
            yield return new WaitForSeconds(stepDelay);
            canPlayFootsteps = true;
        }
        else
        {

        }
    }
}
