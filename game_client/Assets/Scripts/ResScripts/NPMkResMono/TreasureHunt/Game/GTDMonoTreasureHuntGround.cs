using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GTDMonoTreasureHuntGround : MonoBehaviour
    {
        [ALHeader("地面拼接对象")]
        public List<Transform> groundPieceParts;
        [ALHeader("单个部件长度")]
        public float partLength = 10f;
        [ALHeader("移动方向")]
        public Vector3 moveDirection = Vector3.back;

       
        public void setOffset(float _offset)
        {
            if (groundPieceParts == null || groundPieceParts.Count == 0)
                return;
            
            // 更新所有部件位置
            Vector3 basePos = transform.localPosition;
            Vector3 direction = moveDirection.normalized;
            float totalLength = groundPieceParts.Count * partLength;
            
            // 确保总偏移量在0到totalLength之间
            _offset %= totalLength;
            if (_offset < 0) _offset += totalLength;
            
            for (int i = 0; i < groundPieceParts.Count; i++)
            {
                // 计算每个部件的虚拟位置（考虑循环）
                float virtualPos = (i * partLength - _offset) % totalLength;
                if (virtualPos < 0) virtualPos += totalLength;
                
                // 如果虚拟位置超出可见范围，将其移到另一端继续循环
                if (virtualPos > totalLength - partLength)
                    virtualPos -= totalLength;
                    
                groundPieceParts[i].localPosition = basePos + direction * virtualPos;
            }
        }
    }
}