using System;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.Rendering.Universal;


/// <summary>
/// 截屏的工具类
/// </summary>
public class ScreenShotUtil
{

    private static RenderTexture _m_cameraRenderTexture;

    /// <summary>
    /// 获取某个camera的显示截图， 不用RenderTexture 要执行这个releaseMainCameraRenderTexture释放掉
    /// </summary>
    /// <param name="_camera"></param>
    /// <param name="_size"> RenderTexture大小</param>
    /// <param name="_viewClip"> renderTexture在屏幕中的归一话的范围（值为：0-1）</param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public static void getMainCameraRenderTexture(Vector2 _size, Vector4 _viewClip, Action<RenderTexture> _callBack)
    {
        // _m_cameraRenderTexture = new RenderTexture(Mathf.CeilToInt(_size.x), Mathf.CeilToInt(_size.y));
        _m_cameraRenderTexture = RenderTexture.GetTemporary(Mathf.CeilToInt(_size.x), Mathf.CeilToInt(_size.y));

        ScreenBlurMgr.instance.getMainRenderTexture(_m_cameraRenderTexture, _viewClip);
        // 延迟一帧是因为需要下一帧才能拿到有效的RenderTexture
         ALCommonTaskController.CommonActionAddNextFrameTask(() =>
         {
             _callBack?.Invoke(_m_cameraRenderTexture);
         });
    }
    
    /// <summary>
    /// 释放cameraRenderTexture
    /// </summary>
    public static void releaseMainCameraRenderTexture()
    {
        if (_m_cameraRenderTexture != null)
        {
            RenderTexture.ReleaseTemporary(_m_cameraRenderTexture);
            _m_cameraRenderTexture = null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_renderTexture"></param>
    /// <param name="shareRTSize"></param>
    /// <returns></returns>
    public static Texture2D convertRenderTextureToTexture2D(RenderTexture _renderTexture, Vector2Int shareRTSize)
    {
        RenderTexture shareRt = RenderTexture.GetTemporary(shareRTSize.x, shareRTSize.y);
        Graphics.Blit(_renderTexture, shareRt);
        RenderTexture temp = RenderTexture.active;
        RenderTexture.active = shareRt;
                        
        Texture2D texture2D = new Texture2D(shareRTSize.x, shareRTSize.y, TextureFormat.ARGB32, false);
        texture2D.ReadPixels(new Rect(0, 0, texture2D.width, texture2D.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = temp;
        RenderTexture.ReleaseTemporary(shareRt);

        return texture2D;
    }

    public static byte[] convertRenderTextureToJPGBytes(RenderTexture _renderTexture, Vector2Int shareRTSize)
    {
        Texture2D texture2D = ScreenShotUtil.convertRenderTextureToTexture2D(_renderTexture, shareRTSize);
        return texture2D.EncodeToJPG();
    }

    public static void getCaptureScreenshotIntoRenderTextureByDealer(Action<RenderTexture> _callBack)
    {
        if(_callBack == null)
            return;
        
        ALCoroutineDealerMgr.instance.addCoroutine(new ScreenShotIntoRenderTextureDealer((_tex) =>
        {
            _callBack.Invoke(_tex);
        }));
    }

    public static void releaseCaptureScreenshotIntoRenderTextureByDealer(RenderTexture _renderTexture)
    {
        if (_renderTexture != null)
        {
            RenderTexture.ReleaseTemporary(_renderTexture);
        }
    }
}
