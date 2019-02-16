using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudio : MonoBehaviour {

    // public GameObject audioControl;
    // new public AudioSource audio;

    public void PlayFootstep(float volume)
    {
        AudioController.PlayFootsteps();
    }

}
