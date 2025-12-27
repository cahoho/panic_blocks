using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 游戏暂停逻辑。
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settings; 

    void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.FindWithTag("PauseUI");
        }
        if (settings == null)
        {
            settings = GameObject.FindWithTag("SettingsUI");
        }

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (settings != null) settings.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settings != null && settings.activeSelf)
            {
                CloseSettings(); 
            }
            else if (pauseMenu != null && pauseMenu.activeSelf)
            {
                PauseClose();
            }
            else
            {
                PauseOpen();
            }
        }
    }

    public void PauseOpen()
    {
        AudioManager.PlayClickAudio();
        if (pauseMenu != null) pauseMenu.SetActive(true);
        Time.timeScale = 0;

        if (AudioManager.Instance != null)
        {
            //调用AudioManager的方法来降低音乐音量
            AudioManager.Instance.ApplyMusicReduction();
        }
    }

    public void PauseClose()
    {
        AudioManager.PlayClickAudio();
        if (pauseMenu != null) pauseMenu.SetActive(false);
        //确保在关闭暂停菜单时，如果设置菜单还开着，也一并关闭
        if (settings != null && settings.activeSelf)
        {
            settings.SetActive(false);
        }
        Time.timeScale = 1;

        if (AudioManager.Instance != null)
        {
            //调用AudioManager的方法来恢复音乐音量
            AudioManager.Instance.RemoveMusicReduction();
        }
    }

    public void ReGame()
    {
        AudioManager.PlayClickAudio();
        Time.timeScale = 1;
        //确保在重新开始游戏前恢复音乐音量
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RemoveMusicReduction();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Settings()
    {
        AudioManager.PlayClickAudio();
        if (settings != null) settings.SetActive(true); 
    }

    public void CloseSettings()
    {
        AudioManager.PlayClickAudio();
        if (settings != null) settings.SetActive(false);
    }

    public void Home()
    {
        AudioManager.PlayClickAudio();
        Time.timeScale = 1;
        //确保在返回主页前恢复音乐音量
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RemoveMusicReduction();
        }
        SceneManager.LoadScene(1);
    }
}
