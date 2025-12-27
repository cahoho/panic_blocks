using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// AudioManager.cs
/// 
/// 由于不同场景的audio背景音乐不同，因此我采取使用数组存储不同背景音乐的方式。数组的类型是一个自定义类，在一定程度保证了代码的可读性与安全性
/// 在类中，可以存储每个audio所存在的场景index以实现自动化控制。
/// 
/// 其他clip则采用了clip变量的形式存储，并在合适的时机进行调用。
/// 作为manager，采用单例模式，并重写awake方法，单独进行音频方面的初始化
/// </summary>

[System.Serializable]
public class SceneIndexMusicEntry
{
    public int startSceneIndex;
    public int endSceneIndex;
    public AudioClip musicClip;
}
public class AudioManager : Singleton<AudioManager>
{
    public SceneIndexMusicEntry[] sceneMusicSettings;

    [Header("主角声音")]
    public AudioClip jumpClip;

    [Header("系统声音")]
    public AudioClip winClip;
    public AudioClip failClip;
    public AudioClip clickClip;

    [Header("音量调节")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.7f;   

    private AudioSource music;
    private AudioSource player;
    private AudioSource system;

    private float _currentMusicPlaybackMultiplier = 1.0f; 
    [Tooltip("暂停时音乐音量的降低比例")]
    public float pauseMusicReductionFactor = 0.6f;

    // 标记是否已经初始化
    private bool _isInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioManager();
    }

    protected void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeAudioManager()
    {
        if (_isInitialized && Instance == this) return;

        // 清理现有的 AudioSource 组件
        CleanupAudioSources();

        // 创建新的 AudioSource
        music = gameObject.AddComponent<AudioSource>();
        player = gameObject.AddComponent<AudioSource>();
        system = gameObject.AddComponent<AudioSource>();

        // 配置 AudioSource 属性
        music.loop = true; 
        player.loop = false; 
        system.loop = false; 

        // 初始设置音量
        music.volume = musicVolume * _currentMusicPlaybackMultiplier;
        player.volume = sfxVolume;
        system.volume = sfxVolume;

        _isInitialized = true;

        PlayMusicForCurrentSceneIndex();
    }

    // 清理现有的 AudioSource 组件
    private void CleanupAudioSources()
    {
        AudioSource[] existingSources = GetComponents<AudioSource>();
        foreach (var source in existingSources)
        {
            if (source != null)
            {
                source.Stop();
                DestroyImmediate(source);
            }
        }

        music = null;
        player = null;
        system = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != this) return;
        RemoveMusicReduction(); 
        PlayMusicForCurrentSceneIndex();
    }

    private void PlayMusicForCurrentSceneIndex()// 方法：保证音乐可以在正确的场景下播放（根据sceneindex）
    {
        if (Instance != this) return;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AudioClip newMusicClip = null;

        foreach (var entry in sceneMusicSettings)
        {
            if (currentSceneIndex >= entry.startSceneIndex && currentSceneIndex <= entry.endSceneIndex)
            {
                newMusicClip = entry.musicClip;
                break; 
            }
        }

        if (newMusicClip != null)
        {
            if (music.clip != newMusicClip || !music.isPlaying)
            {
                music.Stop(); 
                music.clip = newMusicClip; 
                music.volume = musicVolume * _currentMusicPlaybackMultiplier; 
                music.Play(); 
            }
        }
        else
        {
            if (music.isPlaying)
            {
                music.Stop();
                music.clip = null; 
            }
        }
    }
    // 完全刷新 AudioManager
    public void RefreshAudioManager()
    {
        if (Instance != this) return;

        Debug.Log("刷新 AudioManager...");
        
        // 停止所有声音
        StopAllAudio();
        
        // 重新初始化
        _isInitialized = false;
        InitializeAudioManager();
        
        Debug.Log("AudioManager 刷新完成");
    }

    // 只刷新背景音乐
    public void RefreshBackgroundMusic()
    {
        if (Instance != this) return;

        Debug.Log("刷新背景音乐...");
        
        if (music != null)
        {
            music.Stop();
            music.clip = null;
        }
        
        PlayMusicForCurrentSceneIndex();
    }

    // 停止所有音频
    public void StopAllAudio()
    {
        if (music != null) music.Stop();
        if (player != null) player.Stop();
        if (system != null) system.Stop();
    }

    // 重置所有音量设置
    public void ResetVolumeSettings()
    {
        musicVolume = 0.7f;
        sfxVolume = 0.7f;
        _currentMusicPlaybackMultiplier = 1.0f;

        if (music != null) music.volume = musicVolume;
        if (player != null) player.volume = sfxVolume;
        if (system != null) system.volume = sfxVolume;
    }

    // 不同的 Volume 进行单独的方法设置：

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (music != null && Instance == this)
        {
            music.volume = musicVolume * _currentMusicPlaybackMultiplier;
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        if (player != null && Instance == this)
        {
            player.volume = sfxVolume;
        }
        if (system != null && Instance == this)
        {
            system.volume = sfxVolume;
        }
    }

    public void ApplyMusicReduction()
    {
        if (Instance != this) return;
        _currentMusicPlaybackMultiplier = pauseMusicReductionFactor;
        if (music != null) music.volume = musicVolume * _currentMusicPlaybackMultiplier;
    }

    public void RemoveMusicReduction()
    {
        if (Instance != this) return;
        _currentMusicPlaybackMultiplier = 1.0f;
        if (music != null) music.volume = musicVolume * _currentMusicPlaybackMultiplier;
    }

    protected override void OnDestroy()
    {
        if (Instance == this)
        {
            StopAllAudio();
        }
        base.OnDestroy();
    }
    public static void Refresh()
    {
        if (Instance == null) return;
        Instance.RefreshAudioManager();
    }

    public static void RefreshMusic()
    {
        if (Instance == null) return;
        Instance.RefreshBackgroundMusic();
    }

    public static void StopAll()
    {
        if (Instance == null) return;
        Instance.StopAllAudio();
    }

    public static void Fail()
    {
        if (Instance == null) return;
        Instance.system.clip = Instance.failClip;
        Instance.system.volume = Instance.sfxVolume;
        Instance.system.Play();
    }

    public static void Win()
    {
        if (Instance == null) return;
        Instance.system.clip = Instance.winClip;
        Instance.system.volume = Instance.sfxVolume;
        Instance.system.Play();
    }

    public static void PlayJumpAudio()
    {
        if (Instance == null) return;
        Instance.player.clip = Instance.jumpClip;
        Instance.player.volume = Instance.sfxVolume;
        Instance.player.Play();
    }

    public static void PlayClickAudio()
    {
        if (Instance == null) return;
        Instance.system.clip = Instance.clickClip;
        Instance.system.volume = Instance.sfxVolume;
        Instance.system.Play();
    }
}