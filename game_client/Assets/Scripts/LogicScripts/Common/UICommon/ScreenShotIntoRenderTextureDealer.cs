using System;
using System.Collections;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class ScreenShotIntoRenderTextureDealer : _IALCoroutineDealer
    {
        private Action<RenderTexture> _m_aOnDone;
        
        public ScreenShotIntoRenderTextureDealer(Action<RenderTexture> _done)
        {
            _m_aOnDone = _done;
        }
        
        public IEnumerator dealCoroutine()
        {
            // 關鍵！等待，直到這一幀的所有渲染都完成
            yield return new WaitForEndOfFrame();

            Rect uiCameraRect = Game.instance.mainCamera.uiCamera.rect;
            
            RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
            
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_IOS
            
            // 在编辑器和PC平台上，截图会是上下颠倒的，这里进行翻转, 并且考虑到uiCamera的裁剪导致的偏移和缩放
            RenderTexture flippedRT = RenderTexture.GetTemporary(Mathf.RoundToInt(Screen.width * uiCameraRect.width), Mathf.RoundToInt(Screen.height * uiCameraRect.height), 0);
            // scale.y = -1, offset.y = 1 实现上下翻转, 并且考虑到考虑到uiCamera的裁剪导致的偏移和缩放
            Graphics.Blit(renderTexture, flippedRT, new Vector2(1f * uiCameraRect.width, -1f * uiCameraRect.height), new Vector2(uiCameraRect.x, 1f - uiCameraRect.y));
            
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = flippedRT;
            
            #else
            
            // 移动平台上截图是正常的，不需要翻转, 只需要考虑到uiCamera的裁剪导致的偏移和缩放
            RenderTexture flippedRT = RenderTexture.GetTemporary(Mathf.RoundToInt(Screen.width * uiCameraRect.width), Mathf.RoundToInt(Screen.height * uiCameraRect.height), 0);
            // 考虑到考虑到uiCamera的裁剪导致的偏移和缩放
            Graphics.Blit(renderTexture, flippedRT, new Vector2(uiCameraRect.width, uiCameraRect.height), new Vector2(uiCameraRect.x, uiCameraRect.y));

            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = flippedRT;
            
            #endif
            
            _m_aOnDone?.Invoke(renderTexture);
        }
    }
}