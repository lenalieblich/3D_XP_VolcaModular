using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public void PlayAudioClip(AudioClip audio)
    {
        AudioPeer aud = GetComponent<AudioPeer>();
        aud._audioClip = audio;
        aud.StartPlaying();
    }
}
