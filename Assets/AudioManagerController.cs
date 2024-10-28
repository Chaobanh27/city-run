using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public static AudioManagerController instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private void Awake()
    {
        instance = this;
    }

    private int bgmIndex;

    // Update is called once per frame
    void Update()
    {
        if (!bgm[bgmIndex].isPlaying)
        {
            PlayRandomBGM();
        }
    }

    private void PlayRandomBGM()
    {
        bgmIndex = UnityEngine.Random.Range(0, bgm.Length);

        PlayBGM(bgmIndex);
    }

    public void PlaySFX(int index)
    {
        if(index < sfx.Length)
        {
            sfx[index].pitch = UnityEngine.Random.Range(.85f, 1.1f);
            sfx[index].Play();
        }
    }

    public void StopSFX(int index)
    {
        sfx[index].Stop();
    }

    public void PlayBGM(int index)
    {
        StopBGM();
        bgm[index].Play();
    }

    public void StopBGM()
    {
        for(int i = 0;i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
