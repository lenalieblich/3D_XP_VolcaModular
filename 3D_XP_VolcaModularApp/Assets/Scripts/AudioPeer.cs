using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource _audioSource;

    //Microfon Input
    public AudioClip _audioClip; 
    public bool _useMicrofon;
    public string _selectedDevice;
    public AudioMixerGroup _mixerGroupMicrophone, MixerGroupMaster;

    public static float[] _samplesLeft = new float[512];
    public static float[] _samplesRight = new float[512];
    public static float[] _freqBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];

    float[] _freqBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];
    public float _audioProfile;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfile);

        if (_useMicrofon)
        {
            if(Microphone.devices.Length > 0)
            {
                _selectedDevice = Microphone.devices[0].ToString();
                _audioSource.clip = Microphone.Start(_selectedDevice, true, 30, AudioSettings.outputSampleRate);
                _audioSource.outputAudioMixerGroup = _mixerGroupMicrophone;
            } else
            {
                _useMicrofon = false;
            }
        }
        StartPlaying();
    }

    public void StartPlaying()
    {
        if (!_useMicrofon)
        {
            _audioSource.outputAudioMixerGroup = MixerGroupMaster;
            _audioSource.clip = _audioClip;
        }
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    void AudioProfile(float audioProfile)
    {
        for(int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman); 
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman); 
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2; 

            if( i == 7)
            {
                sampleCount += 0;
            }
            for(int j = 0; j < sampleCount; j++)
            {
                average += (_samplesLeft[count] + _samplesRight[count]) * (count+1);
                count++;
            }

            average /= sampleCount;
            _freqBand[i] = average * 10; 

        }
    }

    void CreateAudioBands()
    {
        for(int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]); 

        }
    }

    void BandBuffer()
    {
        for(int i = 0; i < 8; i++)
        {
            if(_freqBand[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = _freqBand[i];
                _bufferDecrease[i] = 0.005f; 
            } else
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= 1.2f; 

            }
        }
    }
}
