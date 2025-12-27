using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : Singleton<FlagManager>
{
    /// <summary>
    /// 订阅游戏胜利事件。这个事件将在Flag.cs发出。
    /// 每次抓取到flag，都检查一次是否所有的Flag都被catch？ 游戏胜利  ： 不做任何操作
    /// </summary>
    public List<Flag> allFlags = new List<Flag>();
    
    // 游戏胜利事件
    public System.Action OnAllFlagsCaptured;

    public void RegisterFlag(Flag flag)
    {
        if (!allFlags.Contains(flag))
        {
            allFlags.Add(flag);
            // 订阅
            flag.OnFlagCatched += OnFlagCatched;
        }
    }
    
    public void UnregisterFlag(Flag flag)
    {
        if (allFlags.Contains(flag))
        {
            // 取消订阅
            flag.OnFlagCatched -= OnFlagCatched;
            allFlags.Remove(flag);
        }
    }
    
    private void OnFlagCatched(Flag capturedFlag)
    {
        CheckAllFlagsCaptured();
    }
    
    private void CheckAllFlagsCaptured()
    {
        foreach (Flag flag in allFlags)
        {
            if (!flag.catched)
                return; 
        }
        OnAllFlagsCaptured?.Invoke();
        GameManager.Instance.GameWin();//调用GameManager
    }
    
}