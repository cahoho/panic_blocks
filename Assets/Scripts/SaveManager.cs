using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices; 
/// <summary>
/// 游戏的数据保存系统
/// 部分内容为AI辅助生成。
/// 由于不同的平台对保存的要求和SDK不同，因此这里的适配较为繁琐。
/// </summary>
[System.Serializable]
public class LevelResult
{
    public int level;      // 关卡编号
    public int stars;      // 星级 0~3
    public float time;     // 通关时间
}

[System.Serializable]
public class GameSaveData// 获得数据
{
    public List<int> unlockedLevels = new List<int>();  // 解锁的关卡
    public List<LevelResult> results = new List<LevelResult>(); // 关卡结果（星级和时间）
}

public class SaveManager : Singleton<SaveManager>
{
    public List<LevelStarSetting> starSettings = new List<LevelStarSetting>(); // 星级设置
    private GameSaveData saveData; // 游戏存档数据
    private string savePath;

    // 声明从 .jslib 导入的 JavaScript 函数
    // 注意：这里的函数名必须与 .jslib 文件中 mergeInto 的键名完全一致
    [DllImport("__Internal")]
    private static extern void SaveGameDataToWeChat(string jsonString);

    [DllImport("__Internal")]
    private static extern string LoadGameDataFromWeChat();

    protected override void Awake()
    {
        base.Awake();
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame();
    }

    public void LoadGame()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadFromWeChat();
#else
        LoadFromLocal();
#endif
    }

    public void SaveGame()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveToWeChat();
#else
        SaveToLocal();
#endif
    }

    private void SaveToWeChat()//仅仅在webgl实现
    {
        try
        {
            string json = JsonUtility.ToJson(saveData);
            // 调用.jslib中的js函数来保存数据
            SaveGameDataToWeChat(json);
            Debug.Log("数据保存到微信存储成功");
        }
        catch (System.Exception e)
        {
            Debug.LogError("保存数据到微信存储失败: " + e.Message);
        }
    }

    private void LoadFromWeChat()
    {
        try
        {
            // 调用 .jslib 中的js函数来加载数据
            string json = LoadGameDataFromWeChat();

            if (!string.IsNullOrEmpty(json))
            {
                saveData = JsonUtility.FromJson<GameSaveData>(json);
                Debug.Log("从微信存储加载数据成功");
            }
            else
            {
                CreateDefaultData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载微信存储数据失败: " + e.Message);
            CreateDefaultData();
        }
    }

    // 本地文件存储方法（用于 Windows、Mac、Android、iOS 等平台）
    private void LoadFromLocal()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                saveData = JsonUtility.FromJson<GameSaveData>(json);
                Debug.Log($"从本地文件加载数据成功: {savePath}");
                
                // 验证数据完整性
                if (saveData.unlockedLevels == null || saveData.unlockedLevels.Count == 0)
                {
                    Debug.LogWarning("存档数据不完整，创建默认数据");
                    CreateDefaultData();
                }
            }
            else
            {
                Debug.Log("本地存档文件不存在，创建默认数据");
                CreateDefaultData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载本地存档失败: {e.Message}");
            CreateDefaultData();
        }
    }

    private void SaveToLocal()
    {
        try
        {
            // 确保目录存在
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"数据保存到本地文件成功: {savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存本地存档失败: {e.Message}");
        }
    }

    private void CreateDefaultData()
    {
        saveData = new GameSaveData();
        saveData.unlockedLevels.Add(1);
        SaveGame(); // 创建默认数据后立即保存
        Debug.Log("创建默认存档数据");
    }

    // 以下是你的原有方法，保持不变
    public void UnlockLevel(int level)
    {
        if (!saveData.unlockedLevels.Contains(level))
        {
            saveData.unlockedLevels.Add(level);
            SaveGame();
            Debug.Log($"解锁关卡: {level}");
        }
    }

    public bool IsLevelUnlocked(int level)
    {
        if (level == 1) return true;
        bool unlocked = saveData.unlockedLevels.Contains(level);
        Debug.Log($"检查关卡{level}是否解锁: {unlocked}");
        return unlocked;
    }

    public void SaveLevelResult(int level, float timeInSeconds)
    {
        Debug.Log($"保存关卡结果: 关卡{level}, 时间{timeInSeconds}秒");

        int stars = CalculateStars(level, timeInSeconds);
        Debug.Log($"计算获得星级: {stars}星");

        LevelResult existing = saveData.results.Find(r => r.level == level);
        if (existing != null)
        {
            Debug.Log($"更新已有记录: 原星级{existing.stars}, 新星级{stars}");
            existing.stars = Mathf.Max(existing.stars, stars);
            existing.time = Mathf.Min(existing.time, timeInSeconds);
        }
        else
        {
            Debug.Log("创建新记录");
            saveData.results.Add(new LevelResult
            {
                level = level,
                stars = stars,
                time = timeInSeconds
            });
        }

        // 解锁下一关
        int nextLevel = level + 1;
        if (!saveData.unlockedLevels.Contains(nextLevel))
        {
            Debug.Log($"解锁下一关: {nextLevel}");
            UnlockLevel(nextLevel);
        }

        SaveGame();

        // 调试输出当前保存的数据
        DebugSaveData();
    }

    // 调试方法：打印当前保存的数据
    private void DebugSaveData()
    {
        Debug.Log("=== 当前存档数据 ===");
        Debug.Log($"已解锁关卡: {string.Join(",", saveData.unlockedLevels)}");
        foreach (var result in saveData.results)
        {
            Debug.Log($"关卡{result.level}: {result.stars}星, 时间{result.time}秒");
        }
        Debug.Log("===================");
    }

    public int GetStarsForLevel(int level)
    {
        LevelResult result = saveData.results.Find(r => r.level == level);
        int stars = result != null ? result.stars : 0;
        Debug.Log($"获取关卡{level}的星级: {stars}");
        return stars;
    }

    public float GetTimeForLevel(int level)
    {
        LevelResult result = saveData.results.Find(r => r.level == level);
        float time = result != null ? result.time : -1f;
        Debug.Log($"获取关卡{level}的时间: {time}");
        return time;
    }

    private int CalculateStars(int level, float time)
    {
        LevelStarSetting setting = starSettings.Find(s => s.level == level);
        if (setting != null)
        {
            if (time <= setting.threeStarTime) return 3;
            if (time <= setting.twoStarTime) return 2;
            if (time <= setting.oneStarTime) return 1;
            return 0;
        }
        else
        {
            // 默认星级标准
            if (time <= 30f) return 3;
            if (time <= 60f) return 2;
            if (time <= 90f) return 1;
            return 0;
        }
    }

    public void ResetProgress()
    {
        saveData = new GameSaveData();
        saveData.unlockedLevels.Add(1);
        SaveGame();
        Debug.Log("游戏进度已重置");
    }

    // 新增：手动导出存档（用于调试和备份）
    public void ExportSaveData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        string exportPath = Path.Combine(Application.persistentDataPath, "save_export.json");
        File.WriteAllText(exportPath, json);
        Debug.Log($"存档已导出到: {exportPath}");
    }

    // 新增：手动导入存档
    public void ImportSaveData(string json)
    {
        try
        {
            saveData = JsonUtility.FromJson<GameSaveData>(json);
            SaveGame();
            Debug.Log("存档导入成功");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"存档导入失败: {e.Message}");
        }
    }

#if UNITY_EDITOR
    [ContextMenu("打印存档路径")]
    void PrintPath()
    {
        Debug.Log("存档路径：" + savePath);
        Debug.Log("持久化数据路径：" + Application.persistentDataPath);
    }

    [ContextMenu("测试保存数据")]
    void TestSaveData()
    {
        // 测试保存一些数据
        SaveLevelResult(1, 25f);
        SaveLevelResult(2, 45f);
    }

    [ContextMenu("查看当前存档")]
    void ViewCurrentSave()
    {
        DebugSaveData();
    }

    [ContextMenu("导出存档")]
    void ExportSave()
    {
        ExportSaveData();
    }

    [ContextMenu("重置存档")]
    void ResetSave()
    {
        ResetProgress();
    }
#endif
}