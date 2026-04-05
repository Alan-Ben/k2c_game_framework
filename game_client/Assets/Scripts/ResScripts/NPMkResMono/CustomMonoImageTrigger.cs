using System;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class CustomMonoImageTrigger : MonoBehaviour
    {
        [Header("图片")]
        public Image img;

        [ALHeader("触发点击事件的最小透明度")]
        public float alphaHitTestMinimumThreshold = 0.1f;
        
        private void Awake()
        {
            if (img == null)
                return;
            
            img.raycastTarget = true;
            img.alphaHitTestMinimumThreshold = Mathf.Clamp01(alphaHitTestMinimumThreshold);
        }

        private void OnDestroy()
        {
        }
    }
}