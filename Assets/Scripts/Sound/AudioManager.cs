using System.Collections;
using UnityEngine;

/// <summary>
/// Audio Player class that allows any class to call upon it to play SFX and BGM
/// features different overload methods for 3D, 2D, and BGM audio playing.
/// Written by: Sean
/// Modified by: 
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Variables
    public static AudioManager instance;

    [SerializeField] private int _sfxSourceLength;

    private AudioSource[] _sfxSources;
    private AudioSource _bgm;
    private int _curSFXIndex = 0;
    #endregion

    #region CodeBase
    #region Initialize
    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager.instance = this;
        }
        else if (AudioManager.instance != this)
        {
            Destroy(this);
        }

        /*_bgm = GetComponent<AudioSource>();
        if (_bgm == null)
        {
            _bgm = gameObject.AddComponent<AudioSource>();
        }*/

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _sfxSources = new AudioSource[_sfxSourceLength];
        for (int i = 0; i < _sfxSourceLength; i++)
        {
            _sfxSources[i] = gameObject.AddComponent<AudioSource>();
            _sfxSources[i].spatialBlend = 0;
        }
    }
    #endregion

    #region SFX2D
    /// <summary>
    /// Base SFX method for playing a sound globally.
    /// </summary>
    public void PlaySFX2D(AudioClip clipToPlay)
    {
        _sfxSources[_curSFXIndex].clip = clipToPlay;
        _sfxSources[_curSFXIndex].volume = 1;
        _sfxSources[_curSFXIndex].Play();

        _curSFXIndex++;
        if (_curSFXIndex > _sfxSourceLength - 1)
        {
            _curSFXIndex = 0;
        }
    }

    /// <summary>
    /// Overide method that allows for 2DSFX to have pitch varriance.
    /// </summary>
    public void PlaySFX2D(AudioClip clipToPlay, float pitchVariance = 0f)
    {
        _sfxSources[_curSFXIndex].clip = clipToPlay;
        _sfxSources[_curSFXIndex].pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);
        _sfxSources[_curSFXIndex].volume = 1;//AudioPlayerPref.instance.SfxVolume;
        _sfxSources[_curSFXIndex].Play();

        _curSFXIndex++;
        if (_curSFXIndex > _sfxSourceLength - 1)
        {
            _curSFXIndex = 0;
        }
    }
    #endregion

    #region SFX3D
    /// <summary>
    /// Base SFX method for playing a sound in space.
    /// </summary>
    public void PlaySFX3D(AudioClip clipToPlay, Transform origin)
    {
        AudioSource temp = origin.gameObject.AddComponent<AudioSource>();
        temp.clip = clipToPlay;
        temp.spatialBlend = 1f;
        temp.volume = 1;
        temp.Play();
        StartCoroutine(DestroySourceAfterDuration(temp, clipToPlay.length));
    }

    /// <summary>
    /// Overide method that allows for 3DSFX to have pitch varriance.
    /// </summary>
    public void PlaySFX3D(AudioClip clipToPlay, Transform origin, float pitchVariance)
    {
        AudioSource temp = origin.gameObject.AddComponent<AudioSource>();
        temp.clip = clipToPlay;
        temp.spatialBlend = 1f;
        temp.pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);
        temp.volume = 1;
        temp.Play();
        StartCoroutine(DestroySourceAfterDuration(temp, clipToPlay.length));
    }
    #endregion

    #region BGM
    /// <summary>
    /// NEEDS HEAVY WORK. CURRENTLY WORKS BUT BREAKS EASY
    /// </summary>
    public void PlayBGM(AudioClip musicToPlay, float fadeDuration)
    {
        StartCoroutine(PlayBGMCo(musicToPlay, fadeDuration));
    }

    private IEnumerator PlayBGMCo(AudioClip musicToPlay, float fadeDuration)
    {
        float t = 0;

        if (_bgm == null)
        {
            _bgm = gameObject.AddComponent<AudioSource>();
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = musicToPlay;
        newSource.loop = true;
        newSource.Play();

        while (t <= fadeDuration)
        {
            t += Time.deltaTime;
            _bgm.volume = Mathf.Lerp(1/*AudioPlayerPref.instance.MusicVolume*/, 0, t / fadeDuration);
            newSource.volume = Mathf.Lerp(0, 1/*AudioPlayerPref.instance.MusicVolume*/, t / fadeDuration);
            yield return null;
        }

        newSource.volume = 1;//AudioPlayerPref.instance.MusicVolume;

        /*if (_bgm != null)
        {
            Destroy(_bgm);
        }*/
        Destroy(_bgm);
        _bgm = newSource;
    }

    #region Logic
    /*private IEnumerator DestroySourceAfterDuration(AudioSource sourceToDestroy, float duration, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(duration);
        Destroy(sourceToDestroy);
        Destroy(objectToDestroy);
    }*/

    private IEnumerator DestroySourceAfterDuration(AudioSource sourceToDestroy, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(sourceToDestroy);
    }
    #endregion
    #endregion
    #endregion
}
