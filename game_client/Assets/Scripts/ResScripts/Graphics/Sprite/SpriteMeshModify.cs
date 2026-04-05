using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace GOE
{
    /// <summary>
    /// 用于控制大地图Sprite摇摆，阴影参数
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteMeshModify : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public float minWaveStartPosY;

        public SpriteModifyInfo spriteModifyInfo = new SpriteModifyInfo();
        private void Awake()
        {
            if (spriteModifyInfo != null && spriteModifyInfo.sprite != null)
            {
                if (spriteRenderer == null)
                    spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.sprite = SpriteMeshModifyMgr.instance.getModifySprite(spriteModifyInfo);
            }
        }

        public void OnValidate()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            if(spriteRenderer == null)
                return;
            if (spriteModifyInfo == null)
                spriteModifyInfo = new SpriteModifyInfo();
            spriteModifyInfo.sprite = spriteRenderer.sprite;
            if (spriteModifyInfo.sprite != null)
            {
                var vertices = spriteModifyInfo.sprite.vertices;
                float maxY = -99999;
                float minY = 99999;
                // string info = "";
                for (int i = 0; i < vertices.Length; i++)
                {
                    float vy = vertices[i].y;
                    if (maxY < vy)
                        maxY = vy;
                    if (minY > vy)
                        minY = vy;
                    // info += $"{vertices[i]},";
                }
                // Debug.Log(info);

                minY = Mathf.Max(minY, minWaveStartPosY);
                float dis = maxY - minY;
                spriteModifyInfo.waveStartPosY = minY;
                spriteModifyInfo.waveHeight = dis;
                // if (Application.isPlaying)
                // {
                //     SpriteMeshModifyMgr.instance.tryModifySprite(sprite, waveType, waveStrength, receiveShadow, waveStartPosY, waveHeight, waveScale, waveSpeed);
                // }

            }
            
          
        }
    }
}

