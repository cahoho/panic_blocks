using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

/// <summary>
/// 
/// 关卡选择，并没有使用UI，而是采用了可点击的精灵来实现
/// </summary>

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ClickableSprites : MonoBehaviour
{
    public int level;           
    public int gap;              
    private int index;

    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    public GameObject[] starIcons;           // 长度3，星星 GameObject 容器
    public Sprite fullStarSprite;            // 亮星
    public Sprite emptyStarSprite;           // 灰星

    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;
    public bool enableAd=false;
    public GameObject lockTipPanel;
    public GameObject control; 

    private SpriteRenderer spriteRenderer;
    private bool isUnlocked;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
    
        collider.size = spriteRenderer.bounds.size;
        collider.isTrigger = true; 
    }

/// <summary>
/// 星星图标，在unity编辑器中实现防止好位置，并准备好已经填充好的星星和空星星两个sprite
///
/// </summary>
    void Start()
    {
        index = level + gap;
        isUnlocked = SaveManager.Instance.IsLevelUnlocked(level);
        for (int i = 0; i < transform.childCount; i++)
        {
            starIcons[i] = transform.GetChild(i).gameObject;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isUnlocked ? unlockedSprite : lockedSprite;
            spriteRenderer.color = normalColor;
        }
        if (isUnlocked)
        {
            int stars = SaveManager.Instance.GetStarsForLevel(level);
            UpdateStars(stars); 
        }
        else
        {
            // 未解锁的关卡，隐藏所有星星图标
            if (starIcons != null)
            {
                foreach (var star in starIcons)
                {
                    if (star != null)
                    {
                        var img = star.GetComponent<SpriteRenderer>();
                        if (img != null)
                        {
                            img.enabled = false;
                        }
                    }
                }
            }
        }
    }

    void OnMouseDown()
    {

        if (spriteRenderer != null)
            spriteRenderer.color = pressedColor;
    }

    void OnMouseUp()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        if (!isUnlocked)
        {
            if (!enableAd)
            {
                return;
            }
            if (control != null && control.GetComponent<Menu>() != null)
            {
                control.GetComponent<Menu>().OpenPanel(level);
            }
            else if (lockTipPanel != null && enableAd)
            {
                lockTipPanel.SetActive(true);
            }
            return;
        }
        
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (index >= 0 && index < sceneCount)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogError("场景索引 " + index + " 超出范围", this);
        }
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null && Input.GetMouseButton(0))
        {
            spriteRenderer.color = normalColor;
        }
    }

    public void UpdateStars(int stars)// 遍历星星。直到遍历个数达到获得的星星个数
    {
        if (starIcons != null && starIcons.Length == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                var star = starIcons[i];
                if (star != null)
                {
                    var img = star.GetComponent<SpriteRenderer>();
                    if (img != null)
                    {
                        img.sprite = (i < stars) ? fullStarSprite : emptyStarSprite;// 三目运算符
                    }
                }
            }
        }
    }
}
