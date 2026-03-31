using UnityEngine;

namespace Fisherman.Audio
{
    /// <summary>
    /// Simple audio manager with separate channels for BGM, SFX, and ambient sounds.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _ambienceSource;

        [Header("Volume")]
        [Range(0, 1)] public float MasterVolume = 1f;
        [Range(0, 1)] public float BGMVolume = 0.5f;
        [Range(0, 1)] public float SFXVolume = 0.8f;
        [Range(0, 1)] public float AmbienceVolume = 0.6f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip, SFXVolume * MasterVolume);
        }

        public void PlayBGM(AudioClip clip)
        {
            if (_bgmSource.clip == clip && _bgmSource.isPlaying) return;
            _bgmSource.clip = clip;
            _bgmSource.volume = BGMVolume * MasterVolume;
            _bgmSource.loop = true;
            _bgmSource.Play();
        }

        public void PlayAmbience(AudioClip clip)
        {
            if (_ambienceSource.clip == clip && _ambienceSource.isPlaying) return;
            _ambienceSource.clip = clip;
            _ambienceSource.volume = AmbienceVolume * MasterVolume;
            _ambienceSource.loop = true;
            _ambienceSource.Play();
        }

        public void StopBGM()
        {
            _bgmSource.Stop();
        }

        public void StopAmbience()
        {
            _ambienceSource.Stop();
        }
    }
}
