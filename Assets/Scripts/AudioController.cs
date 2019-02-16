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
    private static AudioSource yoyoSfx_;
    private static AudioSource fireSfx_;
    private static AudioSource jumpSfx_;
    private static AudioSource bounceSfx_;
    private static AudioSource deathSfx_;
    private static AudioSource spawnSfx_;

    void Start() {
        sfx_ = GameObject.Find("SFX").GetComponent<AudioSource>();
        footstepSfx_ = GameObject.Find("Footstep SFX").GetComponent<AudioSource>();
        yoyoSfx_ = GameObject.Find("Yoyo SFX").GetComponent<AudioSource>();
        fireSfx_ = GameObject.Find("Fire SFX").GetComponent<AudioSource>();
        jumpSfx_ = GameObject.Find("Jump SFX").GetComponent<AudioSource>();
        bounceSfx_ = GameObject.Find("Bounce SFX").GetComponent<AudioSource>();
        deathSfx_ = GameObject.Find("Death SFX").GetComponent<AudioSource>();
        spawnSfx_ = GameObject.Find("Spawn SFX").GetComponent<AudioSource>();

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
        if (!yoyoSfx_.isPlaying)
            yoyoSfx_.PlayOneShot(sYoyo[Random.Range(0, sYoyo.Count - 1)]);
    }   

    public static void PlayFootsteps() {
        /*
        if (!footstepSfx_.isPlaying)
            footstepSfx_.PlayOneShot(sFootsteps[Random.Range(0, sFootsteps.Count - 1)]);
            */
        footstepSfx_.PlayOneShot(sFootsteps[Random.Range(0, sFootsteps.Count - 1)]);
    }       

    public static void PlayFire() {
        foreach (AudioClip clip in sClips) {
            if (clip.name.Contains("fire")) {
                if (!fireSfx_.isPlaying)
                    fireSfx_.PlayOneShot(clip);
                break;
            }
        }        
    }

    public static void PlayJump() {
        foreach (AudioClip clip in sClips) {
            if (clip.name.Contains("jump")) {
                if (!jumpSfx_.isPlaying)
                    jumpSfx_.PlayOneShot(clip);
                break;
            }
        }        
    }

    public static void PlayBounce() {
        foreach (AudioClip clip in sClips) {
            if (clip.name.Contains("bounce")) {
                bounceSfx_.PlayOneShot(clip);
                break;
            }
        }        
    }    

    public static void PlayDeath() {
        foreach (AudioClip clip in sClips) {
            if (clip.name.Contains("death")) {
                if (!deathSfx_.isPlaying)
                    deathSfx_.PlayOneShot(clip);
                break;
            }
        }        
    }

    public static void PlaySpawn() {
        foreach (AudioClip clip in sClips) {
            if (clip.name.Contains("spawn")) {
                if (!spawnSfx_.isPlaying)
                    spawnSfx_.PlayOneShot(clip);
                break;
            }
        }        
    }
}
