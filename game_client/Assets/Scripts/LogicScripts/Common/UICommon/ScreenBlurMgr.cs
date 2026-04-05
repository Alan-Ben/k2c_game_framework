using System;

using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


/// <summary>
/// 全局的屏幕模糊管理
/// </summary>
public class ScreenBlurMgr
{
    private static ScreenBlurMgr _g_instance = new ScreenBlurMgr();
    public static ScreenBlurMgr instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new ScreenBlurMgr();
            return _g_instance;
        }
    }
    private RenderTexture _m_rt;

    private bool _m_bIsOpenBlurStatic = false; // 当前是否开启了静态模型，用于刷新静态模糊的时判断是否需要刷新

    private NPPGUIWndInstanceTransparentBk _m_wCurTranBkWnd = null; // 当前开启模糊的窗口

    private long _m_lSerializeOp = 1; // 不用实时渲染时开启，延迟一帧关闭相机渲染的序列号
    
    private bool _m_bOpenRealtimeBlur = false; // 是否开启实时模糊

    private NPURPSetting _m_urpSetting = null; // URP设置

    private bool _m_bCurIsOpenBlur = false; // 当前是否开启了模糊，
    
    
    private NPURPSetting urpSetting
    {
        get
        {
            if (_m_urpSetting == null)
                _m_urpSetting = Game.instance.mainCamera.urpSetting;
            return _m_urpSetting;
        }
    }
    
    public RenderTexture curBlurRT => _m_rt;
    
    public bool openRealtimeBlur
    {
        get { return _m_bOpenRealtimeBlur; }
    }
    /// <summary>
    /// 当前是否已经开启了模糊，并且还没有关
    /// </summary>
    public bool curIsOpenBlur
    {
        get { return _m_bCurIsOpenBlur; }
    }
    /// <summary>
    /// 是否支持模糊
    /// </summary>
    private bool isBlurValid
    {
        get
        {
            return Game.instance.mainCamera.uiCameraData != null;
        }
    }

    /// <summary>
    /// 游戏退出的时候调用，用于还原urp的设置
    /// </summary>
    public void discard()
    {
        _m_lSerializeOp++;
        if(!isBlurValid)
            return;
        MJUniversalRenderAPI.uiBlurState = UIBlurState.Default;
        
        MJUniversalRenderAPI.uiBlurRenderTexture = null;
    }

    /// <summary>
    /// 显示模糊效果
    /// </summary>
    public void showBlur(NPPGUIWndInstanceTransparentBk _wnd, Action _afterBlurDelegate)
    {
        _m_lSerializeOp++;
        _m_wCurTranBkWnd = _wnd;
        
        if(!isBlurValid)
            return;
        _m_bCurIsOpenBlur = true;
        //渲染模糊背景
        if (openRealtimeBlur)
        {
            showBlurBGLive();

            //实时的直接处理
            if (null != _afterBlurDelegate)
                _afterBlurDelegate();
        }
        else
        {
            showBlurStatic(_wnd, _afterBlurDelegate);
        }

        //这里需要直接设置图片，在渲染的时候会将图片直接显示在透明背景上，这样才不会因为无图片被渲染为纯色
        if (_wnd != null)
            _wnd.showBlur(curBlurRT);
    }
    
    /// <summary>
    /// 隐藏模糊效果
    /// </summary>
    public void hideBlur()
    {
        _m_lSerializeOp++;
        
        if(!isBlurValid)
            return;

        _m_bCurIsOpenBlur = false;

        if(openRealtimeBlur)
            hideBlurBGLive();
        else
            hideBlurStatic();
        _m_wCurTranBkWnd = null;
    }
    
    /// <summary>
    /// 刷新模糊贴图，用于相机可能在模糊之后设置，开启相机的时候手动刷新一下
    /// </summary>
    public void refreshBlurRT()
    {
        _m_lSerializeOp++;
        
        if(!isBlurValid)
            return;
        if(!openRealtimeBlur)
            refreshBlurStatic(null);
    }
    
    /// <summary>
    /// 显示静止的背景模糊效果
    /// </summary>
    private void showBlurStatic(NPPGUIWndInstanceTransparentBk _wnd, Action _afterBlurDelegate)
    {
        _m_bIsOpenBlurStatic = true;

        //获取UI渲染尺寸
        Vector2Int screenSize = new Vector2Int(Mathf.RoundToInt(Game.instance.mainCamera.uiCamera.rect.width * Screen.width)
            , Mathf.RoundToInt(Game.instance.mainCamera.uiCamera.rect.height * Screen.height));

        if (null != _m_rt)
        {
            if (_m_rt.width != screenSize.x || _m_rt.height != screenSize.y)
            {
                //只有在尺寸不匹配的时候才释放图片
                _releaseRT();
            }
        }

        //如果无数据则重新创建
        if(null == _m_rt)
            _m_rt = RenderTexture.GetTemporary(screenSize.x, screenSize.y);
        
        MJUniversalRenderAPI.uiBlurRenderTexture = _m_rt;

        // 逻辑上行保证窗口再blur之后才显示，没有法线上层窗口被模糊进去的问题，所以这里先不隐藏了
        // //隐藏关联UI对象
        // if(null != _m_wCurTranBkWnd && null != _m_wCurTranBkWnd.uiObj)
        //     _m_wCurTranBkWnd.uiObj.hideUI();

        //刷新模糊贴图
        _freshBlurRT(_wnd, _afterBlurDelegate);
    }

    /// <summary>
    /// 刷新模糊贴图的过程处理，渲染的那一帧，需要把RT得渲染关闭，不然会导致渲染空rt，部分情况出现黑屏bug
    /// </summary>
    private void _freshBlurRT(NPPGUIWndInstanceTransparentBk _wnd, Action _afterBlurDelegate)
    {
        MJUniversalRenderAPI.uiBlurState = UIBlurState.StaticBlur;
        
        // 因为RT的层级放到了UI_Ignore_RT,渲染RT的时候会屏蔽这个层级，所以可以不用隐藏RT了，避免闪帧
        // //渲染的那一帧，需要把RT的渲染关闭，不然会导致渲染空rt，部分情况出现黑屏bug
        // if(_wnd != null)
        //     _wnd.disableShow();
        long serializeOp = _m_lSerializeOp;
        // NPGame.instance.mainCamera.setToOverlayCamera();
        ALCommonActionMonoTask.addNextFrameTask(() =>
        {
            if (serializeOp != _m_lSerializeOp)
            {
                //调用回调
                if (null != _afterBlurDelegate)
                    _afterBlurDelegate();
                return;
            }
            // 因为RT的层级放到了UI_Ignore_RT,渲染RT的时候会屏蔽这个层级，所以可以不用隐藏RT了
            // //渲染的那一帧，需要把RT的渲染关闭，不然会导致渲染空rt，部分情况出现黑屏bug。下一帧再开启
            // if(_wnd != null)
            //     _wnd.enableShow();

            // 逻辑上行保证窗口再blur之后才显示，没有法线上层窗口被模糊进去的问题，所以这里先不隐藏了
            // //显示UI对象
            // if(null != _m_wCurTranBkWnd && null != _m_wCurTranBkWnd.uiObj)
            //     _m_wCurTranBkWnd.uiObj.showUI();

            _staticBlurCloseNormalRender();

            //调用回调
            if (null != _afterBlurDelegate)
                _afterBlurDelegate();
        });
    }
    
    /// <summary>
    /// 在进行模糊背景渲染的时候，延迟一帧关闭其他渲染
    /// </summary>
    private void _staticBlurCloseNormalRender()
    {
        // 管线处理了状态切换，这里不做处理
        // MJUniversalRenderAPI.uiBlurState = UIBlurState.OpenBlur;
    }
    /// <summary>
    /// 隐藏静止的背景模糊效果
    /// </summary>
    private void hideBlurStatic()
    {

        // 当前开启了静态渲染，要把关联的ui显示一下，因为之前把他隐藏了
        // 后面还有问题考虑改成状态机管理（渲染中 渲染完 隐藏）三个状态
        if (_m_bIsOpenBlurStatic && null != _m_wCurTranBkWnd && null != _m_wCurTranBkWnd.uiObj)
        {
            _m_wCurTranBkWnd.uiObj.showUI();
        }
        
        _m_wCurTranBkWnd = null;
        _m_bIsOpenBlurStatic = false;
        
        MJUniversalRenderAPI.uiBlurState = UIBlurState.Default;
        
        MJUniversalRenderAPI.uiBlurRenderTexture = null;
        _releaseRT();
    }

    private void refreshBlurStatic(Action _afterBlurDelegate)
    {
        if (!_m_bIsOpenBlurStatic)
            return;

        //刷新模糊贴图
        _freshBlurRT(_m_wCurTranBkWnd, _afterBlurDelegate);
    }
    /// <summary>
    /// 打开动态的模糊效果
    /// </summary>
    private void showBlurBGLive()
    {
        MJUniversalRenderAPI.uiBlurState = UIBlurState.RealTimeBlur;
    }

    /// <summary>
    /// 关闭动态的模糊效果
    /// </summary>
    private void hideBlurBGLive()
    {
        MJUniversalRenderAPI.uiBlurState = UIBlurState.Default;
    }

    /// <summary>
    /// 释放RT资源
    /// </summary>
    private void _releaseRT()
    {
        if (_m_rt != null)
        {
            RenderTexture.ReleaseTemporary(_m_rt);
            _m_rt = null;
        }
    }



    private RenderTexture _m_uiRenderTexture;
    
    /// <summary>
    /// 获取这一帧将渲染结果给到的RenderTexture，用完需要释放掉
    /// </summary>
    /// <returns></returns>
    public RenderTexture getUIRenderTexture()
    {
        //获取UI渲染尺寸
        Vector2Int screenSize = new Vector2Int(Mathf.RoundToInt(Game.instance.mainCamera.uiCamera.rect.width * Screen.width)
            , Mathf.RoundToInt(Game.instance.mainCamera.uiCamera.rect.height * Screen.height));

        if (null != _m_uiRenderTexture)
        {
            // 如果尺寸不匹配的时候释放图片，重新申请
            if (_m_uiRenderTexture.width != screenSize.x || _m_uiRenderTexture.height != screenSize.y)
                releaseUIRenderTexture();
        }

        //如果无数据则重新创建
        if(null == _m_uiRenderTexture)
            _m_uiRenderTexture = RenderTexture.GetTemporary(screenSize.x, screenSize.y);
        
        // MJUniversalRenderAPI.uiRenderTexture = _m_uiRenderTexture;
        // MJUniversalRenderAPI.requireUIRenderTexture = true;
        return _m_uiRenderTexture;
    }

    /// <summary>
    /// 释放UIRenderTexture
    /// </summary>
    public void releaseUIRenderTexture()
    {
        if (_m_uiRenderTexture != null)
        {
            RenderTexture.ReleaseTemporary(_m_uiRenderTexture);
            _m_uiRenderTexture = null;
        }
    }

    /// <summary>
    /// 获取这一帧将主相机的渲染结果给到的RenderTexture，用完需要释放掉
    /// </summary>
    /// <returns></returns>
    public void getMainRenderTexture(RenderTexture _renderTexture, Vector4 _viewClip)
    {
        // MJUniversalRenderAPI.mainRenderTexture = _renderTexture;
        // MJUniversalRenderAPI.mainRenderClip = _viewClip;
        // // MJUniversalRenderAPI.uiRenderTexture = _m_uiRenderTexture;
        // MJUniversalRenderAPI.requireMainRenderTexture = true;
    }

}
