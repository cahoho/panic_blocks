
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 单独处理UI输入的代码。
/// </summary>
public class Menu : MonoBehaviour
{
    int unlocklevel;
    public GameObject settings;
    public GameObject levels;
    public GameObject panel;
    void Start()
    {
        settings = GameObject.FindWithTag("SettingsUI");
        settings.SetActive(false); 

    }
    public void ChooseLevel()
    {
        AudioManager.PlayClickAudio();
        SceneManager.LoadScene(1);
    }
    public void Settings()
    {
        AudioManager.PlayClickAudio();

        settings.SetActive(true); 
    }
    public void CloseSettings()
    {
        AudioManager.PlayClickAudio();

        settings.SetActive(false);

    }
    public void ClosePanel()
    {
        panel.SetActive(false);
        levels.SetActive(true);
    }
    public void OpenPanel(int index)
    {
        unlocklevel = index;
        panel.SetActive(true);
        levels.SetActive(false);

    }
    public void PressUnlock()
    {
        SaveManager.Instance.UnlockLevel(unlocklevel);
        ClosePanel();
        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackHome()
    {
        SceneManager.LoadScene(0);
    }
        public void Quit()
    {
        Application.Quit();
    }
}
