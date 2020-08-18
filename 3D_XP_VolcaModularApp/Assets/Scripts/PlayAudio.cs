using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private bool isTriggered;
    private AudioClip myAud;


    public void PlayAudioClip(AudioClip audio)
    {
        if (myAud != audio && myAud != null)
        {
            isTriggered = false;
        }

        if (isTriggered == false)
        {
            AudioPeer aud = GetComponent<AudioPeer>();
            aud._audioClip = audio;
            aud.StartPlaying();
            myAud = audio; 
            isTriggered = true; 
        }
    }
}
