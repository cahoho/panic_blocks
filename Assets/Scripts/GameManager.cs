using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 游戏管家
    /// 核心控制游戏的胜利逻辑、失败逻辑、游戏计时器（判断几颗星）
    /// </summary>
    private GameObject moveZone; 
    private float levelTimer = 0f;
    private bool isTiming = false;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeUI();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        InitializeUI();

        levelTimer = 0f;
        isTiming = true;
        StartCoroutine(LevelTimerRoutine());
    }

    IEnumerator LevelTimerRoutine()
    {
        while (isTiming)
        {
            levelTimer += Time.deltaTime;
            yield return null;
        }
    }

    void InitializeUI()
    {
        moveZone = GameObject.FindWithTag("MoveZone");
        Debug.Log($"场景 {SceneManager.GetActiveScene().name} UI初始化完成 - MoveZone: {moveZone != null}");
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }

    public void GameFail()
    {
        PlayerManager.Instance.AllPlayersStatic();
        HideMoveZone();
        AudioManager.Fail();

        isTiming = false;
        levelTimer = 0f;

        Invoke("ReloadScene", 1f);
    }

    public void GameWin()
    {
        HideMoveZone();

        isTiming = false;

        int currentLevel = SceneManager.GetActiveScene().buildIndex-1;
        float timeUsed = levelTimer;
        SaveManager.Instance.SaveLevelResult(currentLevel, timeUsed); 
        Invoke("LoadNextScene", 1.1f);
    }


    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(1); 
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HideMoveZone()
    {
        if (moveZone != null)
        {
            moveZone.SetActive(false);
        }
        else
        {
            moveZone = GameObject.FindWithTag("MoveZone");
            moveZone?.SetActive(false);
        }
    }

    public void ShowMoveZone()
    {
        if (moveZone != null)
            moveZone.SetActive(true);
        else
        {
            moveZone = GameObject.FindWithTag("MoveZone");
            moveZone?.SetActive(true);
        }
    }
}
