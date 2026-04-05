using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MathExtend
{
    public class PointInfo
    {
    public Vector3 pos;
    public Quaternion rot;
    public float scale;
    public int index;
    }

    public enum ColorChanel
    {
    Red = 0,
    Green = 2,
    Blue = 4,
    Alpha = 8,
    }

    public static class PoissonDiscSampling
    {


    private static bool IsInRect(Vector3 candidate, Vector2 sampleRegionSize)
    {
        return candidate.x >= 0f && candidate.x  < sampleRegionSize.x && candidate.z >= 0f &&
               candidate.z  < sampleRegionSize.y;
    }

    private static bool IsNeighbourhood(Vector3 candidate, float cellSize, float minDistance, float scale, List<PointInfo> points, List<int>[,] grid)
    {
        int cellX = Mathf.RoundToInt(candidate.x / cellSize);
        int cellZ = Mathf.RoundToInt(candidate.z / cellSize);
        int searchStartX = Mathf.Max(0, cellX - 3);
        int searchEndX = Mathf.Min(cellX + 3, grid.GetLength(0) - 1);
        int searchStartZ = Mathf.Max(0, cellZ - 3);
        int searchEndZ = Mathf.Min(cellZ + 3, grid.GetLength(1) - 1);
        //如果要检测其它格子内的球，需要遍历周围6个格子

        for (int x = searchStartX; x <= searchEndX; x++)
        {
            for (int z = searchStartZ; z <= searchEndZ; z++)
            {
                var griditem = grid[x, z];
                if (griditem != null)
                {
                    for (int i = 0; i < grid[x, z].Count; i++)
                    {
                        int pointIndex = grid[x, z][i] - 1;//存长度不存索引，取时减1,0就变成了-1，不需要初始化数组了
                        if (pointIndex != -1)
                        {
                            PointInfo point = points[pointIndex];
                            float dst = (candidate - points[pointIndex].pos).magnitude;
                            if (dst < minDistance || dst < (point.scale + scale)/2)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private static float GetPixel(Texture2D tex, float x, float y, ColorChanel chanel, float defaultValue = 1)
    {
        if (tex == null)
            return defaultValue;

        var col = tex.GetPixelBilinear(x, y);
        switch (chanel)
        {
            case ColorChanel.Alpha:
                return col.a;
            case ColorChanel.Blue:
                return col.b;
            case ColorChanel.Green:
                return col.g;
            case ColorChanel.Red:
                return col.r;
            default:
                return defaultValue;
        }
    }
    private static float GetPixelBilinear(Texture2D tex, float x, float y, ColorChanel chanel, float defaultValue = 1)
    {
        if (tex == null)
            return defaultValue;

        switch (chanel)
        {
            case ColorChanel.Alpha:
                return tex.GetPixelBilinear(x, y).a;
            case ColorChanel.Blue:
                return tex.GetPixelBilinear(x, y).b;
            case ColorChanel.Green:
                return tex.GetPixelBilinear(x, y).g;
            case ColorChanel.Red:
                return tex.GetPixelBilinear(x, y).r;
            default:
                return defaultValue;
        }
    }
    public static List<PointInfo> GeneratePoints(int seed, float globalDensity, float minDistance, float maxDistance,Vector2 scaleRange, float scaleSpread, int newPointsCount, Vector2 sampleRegionSize, Vector2 centerOffset, Vector2 terrainSize, Texture2D densityTex,  ColorChanel densityChanel, Texture2D maskTex, ColorChanel maskChanel, int numSamplesBeforeRejection = 32)
    {
        Random.InitState(seed);

        float cellSize = maxDistance / Mathf.Sqrt(2);

        int tGridX = Mathf.CeilToInt(sampleRegionSize.x / cellSize);
        int tGridY = Mathf.CeilToInt(sampleRegionSize.y / cellSize);
        List<int>[,] grid = new List<int>[tGridX, tGridY];
        List<PointInfo> points = new List<PointInfo>();
        List<Vector3> spawnPoints = new List<Vector3>();

        spawnPoints.Add(new Vector3(sampleRegionSize.x / 2f, 0f, sampleRegionSize.y / 2f));
        int spCount = 3;
        Vector2 length = sampleRegionSize / spCount;
        for (int i = 0; i < spCount; i++)
        {
            for (int j = 0; j < spCount; j++)
            {
                spawnPoints.Add(new Vector3(length.x*0.5f + i* length.x , 0f, length.y*0.5f + j* length.y ));
            }
        }

        
        while (spawnPoints.Count > 0 && points.Count < newPointsCount)
        {
            int spawnIndex = Mathf.FloorToInt(Random.value * spawnPoints.Count);
            if (spawnIndex >= spawnPoints.Count)
                spawnIndex = spawnPoints.Count - 1;
            Vector3 spawnCenter = spawnPoints[spawnIndex];
            Vector2 texPos = new Vector2((spawnCenter.x + centerOffset.x) / terrainSize.x, (spawnCenter.z + centerOffset.y) / terrainSize.y);
            float sourceDensity = GetPixel(densityTex, texPos.x, texPos.y, densityChanel, 0);
            float sourceMask = GetPixel(maskTex,texPos.x, texPos.y, maskChanel, 0);
            bool candidateAccepted = false;
            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2f;
                Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
                
                // float newMinDistance = minDistance + (maxDistance - minDistance) * (1-sourceDensity);
                float newMinDistance = minDistance ;

                if (sourceDensity == 0)
                    newMinDistance = 9999999;
                else
                    newMinDistance += (1 / sourceDensity / globalDensity) * 0.001f ;
                float radius = (Random.value + 1) * newMinDistance;
                Vector3 candidate = spawnCenter + dir * radius;
                // float tscale = Random.Range(scaleRange.x, scaleRange.y);
                float tscale = MathExtend.NextGaussian(scaleRange.x , scaleSpread, scaleRange.x, scaleRange.y);

                Vector2 cTexPos = new Vector2((candidate.x + centerOffset.x) / terrainSize.x, (candidate.z + centerOffset.y) / terrainSize.y);
                float mask = GetPixel(maskTex, cTexPos.x, cTexPos.y, maskChanel, 0);
                float density = GetPixel(densityTex, cTexPos.x, cTexPos.y, densityChanel, 0);

                if (IsInRect(candidate, sampleRegionSize) && !IsNeighbourhood(candidate, cellSize, newMinDistance, tscale, points, grid) )
                {
                    spawnPoints.Add(candidate);

                    // if (mask < 0.001f)
                    // {
                        PointInfo pointInfo = new PointInfo();
                        pointInfo.pos = candidate;
                        pointInfo.scale = tscale;
                        pointInfo.index = points.Count + 1;
                       
                        // if (mask > 0.001f)
                        //     pointInfo.scale = 0.1f;
                        
                        points.Add(pointInfo);
                        int gridx = Mathf.FloorToInt(candidate.x / cellSize);
                        int gridy = Mathf.FloorToInt(candidate.z / cellSize);
                        
                        // if(gridx >= grid.GetLength(0) || gridy >= grid.GetLength(1))
                        //     Debug.LogError($"{gridx},{gridy},{grid.GetLength(0)}, {grid.GetLength(1)},{candidate.x},{candidate.z}," +
                        //                    $"{cellSize},,{candidate.x/cellSize},{candidate.z/cellSize}");
                        
                        var gridItem = grid[gridx, gridy];
                        if (gridItem == null)
                        {
                            gridItem = new List<int>();
                            grid[gridx, gridy] = gridItem;
                        }
                        gridItem.Add(points.Count);
                        candidateAccepted = true;
                    // }
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        for (var i = points.Count - 1; i >= 0; i--)
        {
            var point = points[i];
            Vector2 texPos = new Vector2((point.pos.x + centerOffset.x) / terrainSize.x, (point.pos.z + centerOffset.y) / terrainSize.y);
            float mask = GetPixel(maskTex,texPos.x, texPos.y, maskChanel, 0);
            float density = GetPixel(densityTex, texPos.x, texPos.y, densityChanel, 0);
        
            if(mask > 0.01f )
                points.RemoveAt(i);
        }

        return points;
    }
    }
}