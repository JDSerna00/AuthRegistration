using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource; //Este pa la bgm
    [SerializeField] private AudioClip backgroundMusicClip;
    [SerializeField] private static AudioManager instance;
    public static AudioManager GetInstance() => instance;

    private void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            musicSource.clip = backgroundMusicClip;
            musicSource.loop = true;
            musicSource.Play();
            DontDestroyOnLoad(gameObject);
        }
    }
}
