using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class ChangeTerrain : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// 核心玩法之一：
    /// 切换底层机制。
    /// 地层由不同的GameObject（在这里是Tilemap）决定。玩家按下切换底层代码的时候，将会根据数组切换。
    /// </summary>
    [System.Serializable]
    public struct TerrainLayer
    {
        public GameObject obj;
        public bool hasChildren;

        [HideInInspector] public Color initialColor;
        [HideInInspector] public Tilemap renderer;
        [HideInInspector] public TilemapCollider2D collider;

        [HideInInspector] public List<SpriteRenderer> childRenderers;
        [HideInInspector] public List<PolygonCollider2D> childColliders;
        [HideInInspector] public List<Color> childInitialColors;
    }

    [SerializeField] private TerrainLayer[] terrains;
    [SerializeField] private Color unableColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private int currentIndex = 0;

    void Start()
    {
        InitializeTerrains();

        for (int i = 0; i < terrains.Length; i++)// 初始化时，默认只开启第一个地层
        {
            if (i == 0)
                ActivateTerrain(i);
            else
                DeactivateTerrain(i);
        }
    }

    public void OnPointerDown(PointerEventData eventData)// 玩家按下
    {
        DeactivateTerrain(currentIndex);
        currentIndex = (currentIndex + 1) % terrains.Length;
        ActivateTerrain(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeactivateTerrain(currentIndex);
            currentIndex = (currentIndex + 1) % terrains.Length;
            ActivateTerrain(currentIndex);
        }
    }

    private void InitializeTerrains()
    {
        for (int i = 0; i < terrains.Length; i++)
        {
            if (terrains[i].obj != null)
            {
                terrains[i].renderer = terrains[i].obj.GetComponent<Tilemap>();
                terrains[i].collider = terrains[i].obj.GetComponent<TilemapCollider2D>();

                if (terrains[i].renderer != null)
                    terrains[i].initialColor = terrains[i].renderer.color;

                // 如果包含子物体（虽然当前游戏没有子物体，但考虑到代码可扩展性，在这里也进行了测试和完整开发）
                if (terrains[i].hasChildren)
                {
                    terrains[i].childRenderers = new List<SpriteRenderer>();
                    terrains[i].childColliders = new List<PolygonCollider2D>();
                    terrains[i].childInitialColors = new List<Color>();

                    for (int j = 0; j < terrains[i].obj.transform.childCount; j++)// 如果有子物体，则子物体的所有都是新地形。这里进行遍历全部操作
                    {
                        Transform child = terrains[i].obj.transform.GetChild(j);
                        SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
                        PolygonCollider2D childCollider = child.GetComponent<PolygonCollider2D>();

                        if (childRenderer != null)
                        {
                            terrains[i].childRenderers.Add(childRenderer);
                            terrains[i].childInitialColors.Add(childRenderer.color);
                        }

                        if (childCollider != null)
                        {
                            terrains[i].childColliders.Add(childCollider);
                        }
                    }
                }
            }
        }
    }

    private void ActivateTerrain(int index)// 显示
    {
        if (index < 0 || index >= terrains.Length) return;

        var t = terrains[index];

        if (t.renderer != null)
            t.renderer.color = t.initialColor;

        if (t.collider != null)
            t.collider.enabled = true;

        if (t.hasChildren)
        {
            for (int i = 0; i < t.childRenderers.Count; i++)
            {
                if (t.childRenderers[i] != null)
                    t.childRenderers[i].color = t.childInitialColors[i];
            }

            foreach (var col in t.childColliders)
            {
                if (col != null)
                    col.enabled = true;
            }
        }
    }

    private void DeactivateTerrain(int index)// 隐藏
    {
        if (index < 0 || index >= terrains.Length) return;

        var t = terrains[index];

        if (t.renderer != null)
            t.renderer.color = unableColor;

        if (t.collider != null)
            t.collider.enabled = false;

        if (t.hasChildren)
        {
            foreach (var r in t.childRenderers)
            {
                if (r != null)
                    r.color = unableColor;
            }

            foreach (var col in t.childColliders)
            {
                if (col != null)
                    col.enabled = false;
            }
        }
    }
}
