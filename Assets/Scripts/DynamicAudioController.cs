using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicAudioController : MonoBehaviour
{
    public RandomSoundEffect footsteps;
    public RandomSoundEffect yoyo;
    public SoundEffect[] soundLibrary;

    private static DynamicAudioController instance = null;
    private static int MAX_SOURCES = 12;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        if (!instance) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    static void CreateInstanceIfNull()
    {
        if (!instance)
            instance = Instantiate(Resources.Load<DynamicAudioController>("DynamicAudioController"));
    }

    public static void Play(string audioClipName)
    {
        CreateInstanceIfNull();
        SoundEffect sound = null;
        // find sound
        foreach (SoundEffect s in instance.soundLibrary)
            if (s.clip.name.Contains(audioClipName)) { sound = s; break; }
        // if sound not found
        if (sound == null) {
            Debug.Log("Sound effect '" + audioClipName + "' not found!");
            return;
        }
        Play(sound);
    }

    public static void Play(SoundEffect sound)
    {
        CreateInstanceIfNull();
        // find/create source to play
        AudioSource source = null;
        AudioSource[] sourcePool = instance.GetComponents<AudioSource>();
        foreach (AudioSource s in sourcePool)
            if (!s.isPlaying) {
                source = s;
                break;
            }
        if (!source)
        {
            if (sourcePool.Length >= MAX_SOURCES)
                return;
            else
                source = instance.gameObject.AddComponent<AudioSource>();
        }
        // play sound
        source.PlayOneShot(sound.clip, sound.volume);
    }

    public static void PlayFootstep()
    {
        CreateInstanceIfNull();
        Play(instance.footsteps.Get());
    }

    public static void PlayYoyo()
    {
        CreateInstanceIfNull();
        Play(instance.yoyo.Get());
    }

    [System.Serializable]
    public class SoundEffect
    {
        public AudioClip clip;
        [Range(0, 1.0f)]
        public float volume = 1.0f;
    }

    [System.Serializable]
    public class RandomSoundEffect
    {
        [SerializeField]
        private SoundEffect[] soundBank;
        public SoundEffect Get() { return soundBank[Random.Range(0, soundBank.Length - 1)]; }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}
