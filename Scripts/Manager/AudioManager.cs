using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public static AudioManager instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private int bgmRandomIndex;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null) {
            instance = this;
        }    
        else {
            Destroy(this.gameObject);
        }
        InvokeRepeating(nameof(TurnOnBGM), 0, 2f);
    }

    // Play Audio
    public void PlaySFX(int sfxIndenx, bool randomPitch = false) {
        if (sfxIndenx > sfx.Length)
            return;
        if (randomPitch)
            sfx[sfxIndenx].pitch = Random.Range(.85f, 1.15f);
        sfx[sfxIndenx].Play();
    }

    // Stop Audio
    public void StopSFX(int sfxIndenx) {
        if (sfxIndenx > sfx.Length)
            return;
        sfx[sfxIndenx].Stop();
    }

    // Turn on BGM 
    public void TurnOnBGM() {
        if (bgm[bgmRandomIndex].isPlaying == false) {
            PlayBGMRandom();
        }
    }
    // Play Background Music random
    public void PlayBGMRandom() {
        bgmRandomIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmRandomIndex);
    }

    // Play Background Music
    private void PlayBGM(int bgmToPlay) {
        for (int i = 0; i < bgm.Length; i++) {
            bgm[i].Stop();
        }
        bgmRandomIndex = bgmToPlay;
        bgm[bgmToPlay].Play();
    }
}
