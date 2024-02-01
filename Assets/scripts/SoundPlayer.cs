using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Written by : Smoky Shadow
 * This script is a Manager for sounds
*/
public class SoundPlayer : MonoBehaviour
{
    #region STATIC FIELDS
    private static SoundPlayer instance;
    #endregion

    #region SERIALIZED FIELDS
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource loopaudioSource;
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private AudioClip[] backAudioClips;
    #endregion

    #region PRIVATE FIELDS
    Dictionary<SoundClip, AudioClip> clips = new Dictionary<SoundClip, AudioClip>();
    int audioIndex = 0;
    #endregion

    #region PROPERTIES
    public static SoundPlayer Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<SoundPlayer>();
            return instance;
        }
    }
    #endregion

    #region ENUMS
    public enum SoundClip { Laugh = 0, PressingNose, UI_Click};
    #endregion

    #region MONO BEHAVIOURS
    private void Start()
    {
        instance = this;
        SetAudioDictionary();
    }

    private void Update()
    {
        if (!loopaudioSource.isPlaying)
        {
            if (audioIndex == backAudioClips.Length)
                audioIndex = 0;
            loopaudioSource.clip = backAudioClips[audioIndex++];
            loopaudioSource.Play();
        }

    }
    #endregion

    #region PUBLIC METHODS
    public void PlaySound(SoundClip audio)
    {
        audioSource.PlayOneShot(clips[audio]);
    }

    public void MuteSound(bool enable)
    {
        audioSource.enabled = !enable;
    }
    #endregion

    #region PRIVATE METHODS
    void SetAudioDictionary()
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            clips.Add((SoundClip)i, audioClips[i]);
        }
    }
    #endregion
}
