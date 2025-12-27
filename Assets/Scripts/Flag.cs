using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {
    
    Animator anim;
    BoxCollider2D bc;
    public bool catched;
    public  bool appointPlayer;
    public string playerTag;

    /// <summary>
    /// 为了解耦，我并没有在Flag直接写玩家的胜利结果。
    /// 由于Flag可能有多个，需要全部被catch才算胜利，所以Flag.cs仅仅实现Flag的简单内容和重复内容。
    /// 例如：flag被抓取后的动画、音效、如何处理player等
    /// 详细内容将在FlagManager.cs详细介绍
    /// </summary>


    // 事件系统
    public System.Action<Flag> OnFlagCatched;

    void Start()
    {
        FlagManager.Instance.RegisterFlag(this);
        anim = gameObject.GetComponent<Animator>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        catched = false;
        bc.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        
        if (!appointPlayer && collision.gameObject.layer == LayerMask.NameToLayer("Player") && !catched)
        {

            Fun();

            p.anim.SetTrigger("OnWin");
            p.rb.bodyType = RigidbodyType2D.Static;
            p.Invoke("DisableSelf", 0.25f);

        }
        else if (appointPlayer)
        {
            if (collision.gameObject.tag == playerTag && !catched)
            {
                Fun();

                p.anim.SetTrigger("OnWin");
                p.rb.bodyType = RigidbodyType2D.Static;
                p.Invoke("DisableSelf", 0.25f);
            }
        }
    }
    void Fun()
    {
        catched = true;
        anim.SetTrigger("OnFlagTrigger");
        bc.enabled = false;
        OnFlagCatched?.Invoke(this);
        AudioManager.Win();
    }


    void OnDestroy()
    {
        if (FlagManager.Instance != null)
        {
            FlagManager.Instance.UnregisterFlag(this);
        }
    }
}