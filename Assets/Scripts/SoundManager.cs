using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public static SoundManager soundManager;
    public AudioSource sfxSource;
    //public AudioSource musicSource;
    [Header("Mixer Snapshots")]
    
    public List<AudioSnapShotHolder> snapshots = new List<AudioSnapShotHolder>();

    public AudioMixerSnapshot CurrentSnapshot { get; private set; }
    public AudioMixerSnapshot PreviousSnapshot { get; private set; }

    [Header("Sounds")]
    public List<SoundEntry> sounds = new List<SoundEntry>();
    [Header("Pitch Range")]
    public static float lowPitchRange = 0.95f;              //The lowest a sound effect will be randomly pitched.
    public static float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

    private void Awake() {
        if (soundManager == null)
            soundManager = this;
        else if (soundManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //if(sfxSource == null)
        //    sfxSource = GetComponent<AudioSource>();
    }


    public static void SwapMusic(string targetMusic, float time = 1f, bool previous = false) {
        if (previous) {
            if(soundManager.PreviousSnapshot != null) {
                soundManager.PreviousSnapshot.TransitionTo(time);

                AudioMixerSnapshot tempPrev = soundManager.PreviousSnapshot;
                AudioMixerSnapshot tempCurrent = soundManager.CurrentSnapshot;

                soundManager.CurrentSnapshot = tempPrev;
                soundManager.PreviousSnapshot = tempCurrent;
                return;
            }
        }

        AudioMixerSnapshot targetShot = null;

        for(int i = 0; i < soundManager.snapshots.Count; i++) {
            if(soundManager.snapshots[i].audioName == targetMusic) {
                targetShot = soundManager.snapshots[i].snapshot;
                
                break;
            }
        }

        if(targetShot != null) {
            targetShot.TransitionTo(time);
            soundManager.PreviousSnapshot = soundManager.CurrentSnapshot;
            soundManager.CurrentSnapshot = targetShot;
        }

    }

    public static void RestartMusic(string targetMusic) {
        AudioSnapShotHolder target = GetSnapshotHolder(targetMusic);

        if(target != null) {
            target.source.Stop();
            target.source.Play();
        }

    }


    public static void PlaySound(string soundName, float volume = 1f, bool variance = true) {
        AudioClip targetClip = null;

        for (int i = 0; i < soundManager.sounds.Count; i++) {
            if (soundManager.sounds[i].soundName == soundName) {
                targetClip = soundManager.sounds[i].clip;
                break;
            }
        }
        if (targetClip != null) {
            if (variance)
                RandomizeSfx(soundManager.sfxSource).PlayOneShot(targetClip, volume);
            else
                soundManager.sfxSource.PlayOneShot(targetClip, volume);
        }
    }

    private static AudioSource RandomizeSfx(AudioSource source) {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        source.pitch = randomPitch;
        return source;
    }

    private static AudioSnapShotHolder GetSnapshotHolder(string holderName) {
        AudioSnapShotHolder result = null;

        for (int i = 0; i < soundManager.snapshots.Count; i++) {
            if (soundManager.snapshots[i].audioName == holderName) {
                result = soundManager.snapshots[i];

                break;
            }
        }


        return result;
    }


    [System.Serializable]
    public class SoundEntry {
        public string soundName;
        public AudioClip clip;
    }

    [System.Serializable]
    public class AudioSnapShotHolder {
        public AudioMixerSnapshot snapshot;
        public AudioSource source;
        public string audioName;
    }

}
