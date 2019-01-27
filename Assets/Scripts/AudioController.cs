using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public List<AudioClip> footSteps;
    public List<AudioClip> yoyo;
    public List<AudioClip> clips;
    public static List<AudioClip> sFootsteps = new List<AudioClip>();
    public static List<AudioClip> sYoyo = new List<AudioClip>();
    public static List<AudioClip> sClips = new List<AudioClip>();    
    private static AudioSource sfx_;
    private static AudioSource footstepSfx_;

    void Start() {
        sfx_ = GameObject.Find("SFX").GetComponent<AudioSource>();
        footstepSfx_ = GameObject.Find("Footstep SFX").GetComponent<AudioSource>();

        foreach (AudioClip clip in clips) {
            sClips.Add(clip);
        }        

        foreach (AudioClip clip in footSteps) {
            sFootsteps.Add(clip);
        }        

        foreach (AudioClip clip in yoyo) {
            sYoyo.Add(clip);
        }        

    }

    public static void PlaySFX(string name) {
        foreach (AudioClip clip in sClips) {
            if (clip.name == name) {
                if (!sfx_.isPlaying)   
                    sfx_.PlayOneShot(clip);
                break;
            }
        }        
    } 

    public static void PlayYoyo() {
        if (!sfx_.isPlaying)
            sfx_.PlayOneShot(sYoyo[Random.Range(0, sYoyo.Count - 1)]);
    }   

    public static void PlayFootsteps() {
        if (!footstepSfx_.isPlaying)
            footstepSfx_.PlayOneShot(sFootsteps[Random.Range(0, sFootsteps.Count - 1)]);
    }       
}
