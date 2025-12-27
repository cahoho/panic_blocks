using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
/// <summary>
/// 第八关的旋转关卡
/// </summary>
public class Rotating : MonoBehaviour
{
    public float rotationSpeed = 90f;
    private Tilemap tilemap;
    private Vector3 pivotPoint;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        pivotPoint = CalculateTileCenter();
    }

    void Update()
    {
        transform.RotateAround(pivotPoint, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    Vector3 CalculateTileCenter()
    {
        if (tilemap == null) return transform.position;

        List<Vector3> tilePositions = new List<Vector3>();
        
        // 获取所有有瓦片的位置
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(position);
                tilePositions.Add(worldPos);
            }
        }

        // 计算所有瓦片位置的平均值
        if (tilePositions.Count > 0)
        {
            Vector3 sum = Vector3.zero;
            foreach (Vector3 pos in tilePositions)
            {
                sum += pos;
            }
            return sum / tilePositions.Count;
        }

        return transform.position;
    }
}
