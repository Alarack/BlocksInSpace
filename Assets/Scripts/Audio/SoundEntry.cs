using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEntry {

    public string soundName;
    public float soundVolume;
    [Space(10)]
    public bool randomizePitch;
    public float pitchModifier;
    public bool looped;

    public void PlaySound() {
        if (string.IsNullOrEmpty(soundName))
            return;

        if (looped) {
            PlayLoopedsound();
            return;
        }

        SoundManager.PlaySound(soundName, soundVolume, randomizePitch, pitchModifier);
    }

    private void PlayLoopedsound() {

        SoundManager.PlaySound(soundName, soundVolume, randomizePitch, pitchModifier, true);
    }


}
