using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioClip> clips;
    public static List<AudioClip> sClips = new List<AudioClip>();
    private static AudioSource sfx_;

    void Start() {
        sfx_ = GameObject.Find("SFX").GetComponent<AudioSource>();

        foreach (AudioClip clip in clips) {
            sClips.Add(clip);
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
}
