using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheLonelyOne
{
  public class AudioManager : MonoBehaviour
  {
    [Serializable]
    private struct Sound
    {
      public string    name;
      public AudioClip clip;
    }

    #region PARAMETERS
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<Sound> musicSoundsList;
    [SerializeField] private List<Sound> sfxSoundsList;

    private Dictionary<string, AudioClip> musicSounds;
    private Dictionary<string, AudioClip> sfxSounds;
    #endregion

    #region LIFECYCLE
    private void Awake()
    {
      musicSounds = new Dictionary<string, AudioClip>();
      sfxSounds   = new Dictionary<string, AudioClip>();

      foreach (var sound in musicSoundsList)
        musicSounds.Add(sound.name, sound.clip);
      foreach (var sound in sfxSoundsList)
        sfxSounds.Add(sound.name, sound.clip);
    }
    #endregion

    #region INTERFACE
    public void PlayMusic(string _name, bool _loop = true)
    {
      if (musicSounds.TryGetValue(_name, out AudioClip clip))
      {
        musicSource.clip = clip;
        musicSource.loop = _loop;
        musicSource.Play();
      }
    }

    public void PlaySFX(string _name)
    {
      if (sfxSounds.TryGetValue(_name, out AudioClip clip))
        sfxSource.PlayOneShot(clip);
    }

    public void ToggleMusic()
    {
      musicSource.mute = !musicSource.mute;
    }

    public void ToggleMusicLoop()
    {
      musicSource.loop = !musicSource.loop;
    }

    public void ToggleSFX()
    {
      sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float _volume)
    {
      musicSource.volume = _volume;
    }

    public void SFXVolume(float _volume)
    {
      sfxSource.volume = _volume;
    }
    #endregion
  }
}
