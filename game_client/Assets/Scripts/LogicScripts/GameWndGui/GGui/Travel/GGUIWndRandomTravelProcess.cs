using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 随机游历表现流程窗口
    /// </summary>
    public class GGUIWndRandomTravelProcess : _ANPGGUIBasicWnd<GGUIMonoRandomTravelProcess>
    {
        private static GGUIWndRandomTravelProcess _g_instance;
        public static GGUIWndRandomTravelProcess instance { get { return _g_instance ??= new GGUIWndRandomTravelProcess(); } }
        
        private RenderTexture _m_rBgTexture;//背景纹理
        private _ATravelEventInfo _m_rTravelEventInfo;//游历事件信息
        
        private long _m_lShowSerialize;//显示序列

        private NPGGuiWndTexture _m_wBgTexture;
        private GGUISubWndCommonVideo _m_wVideoWnd;//视频展示窗口
        private NPGGuiWndTexture _m_wTravelPosBannerImg;//游历地点Banner图片
        
        public GGUIWndRandomTravelProcess() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoRandomTravelProcess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRandomTravelProcess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.bgImg != null)
                _m_wBgTexture = new NPGGuiWndTexture(wnd.bgImg);
            
            if (wnd.showVideoMono != null)
                _m_wVideoWnd = new GGUISubWndCommonVideo(wnd.showVideoMono);

            if (wnd.travelPosBannerImg != null)
                _m_wTravelPosBannerImg = new NPGGuiWndTexture(wnd.travelPosBannerImg);
        }
        
        protected override void _onDiscard()
        {
            _m_wBgTexture?.discard();
            _m_wBgTexture = null;
            
            _m_wVideoWnd?.discard();
            _m_wVideoWnd = null;
            
            _m_wTravelPosBannerImg?.discard();
            _m_wTravelPosBannerImg = null;
            
            _destroyBgTexture();
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wBgTexture?.hideWnd();
            _m_wVideoWnd?.hideWnd();
            _m_wTravelPosBannerImg?.hideWnd();
            
            _destroyBgTexture();
        }

        protected override void _onReset()
        {
            _m_wBgTexture?.discardTexture();
            _m_wVideoWnd?.resetWnd();
            _m_wTravelPosBannerImg?.discardTexture();

            _destroyBgTexture();
        }

        public void setData(RenderTexture _bgTexture, _ATravelEventInfo _travelEventInfo)
        {
            _m_rBgTexture = _bgTexture;
            _m_rTravelEventInfo = _travelEventInfo;
            
            _startShowProcess();
        }

        private void _destroyBgTexture()
        {
            _m_rBgTexture = null;
        }
        
        /// <summary>
        /// 开始表现流程
        /// </summary>
        private void _startShowProcess()
        {
            long serialize = _m_lShowSerialize;

            // if (wnd != null && wnd.chgBgDelayTimeS >= 0 && _m_rTravelEventInfo != null)
            // {
            //     ALCommonTaskController.CommonActionAddMonoTask(() =>
            //     {
            //         if (wnd == null || serialize != _m_lShowSerialize || _m_rTravelEventInfo == null)
            //         {
            //             return;
            //         }
            //         
            //     }, wnd.chgBgDelayTimeS);
            // }

            if (_m_wBgTexture != null)
            {
                _m_wBgTexture.showWnd();
                _m_wBgTexture.setTexture(_m_rBgTexture);
            }
            
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize)
                    {
                        return;
                    }

                    _m_wVideoWnd?.hideWnd();
                    if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.beforeVideoShowAniName))
                    {
                        wnd.wndAnimation.Play(wnd.beforeVideoShowAniName, _done);
                    }
                    else
                    {
                        _done?.Invoke();
                    }
                })
                .addDelegateProcess((_done) =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize)
                    {
                        return;
                    }

                    if (_m_wVideoWnd != null && wnd.videoConfigList != null)
                    {
                        RandomTravelVideoConfig videoConfig = wnd.videoConfigList.GetRandomItem(); //随机一个视频配置
                        if (videoConfig == null || videoConfig.videoAssetPathInfo == null || !videoConfig.videoAssetPathInfo.enable)
                        {
                            _done?.Invoke();
                            return;
                        }
                        
                        _m_wVideoWnd.showWnd();
                        _m_wVideoWnd.playVideo(videoConfig.videoAssetPathInfo);

                        ALCommonTaskController.CommonActionAddMonoTask(_done, videoConfig.videoPlayTime);
                    }
                    else
                    {
                        _done?.Invoke();
                    }
                })
                .addProcess(() =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize)
                    {
                        return;
                    }

                    // 显示游历地点信息
                    if (_m_rTravelEventInfo != null && _m_rTravelEventInfo.travelPosRefObj != null)
                    {
                        if (_m_wTravelPosBannerImg != null)
                        {
                            _m_wTravelPosBannerImg.showWnd();
                            _m_wTravelPosBannerImg.setTexture(_m_rTravelEventInfo.travelPosRefObj.banner_img);
                        }

                        string travelPosNameKey = wnd.txtTravelPosNameKey == null
                            ? TransKeyConst.common_value
                            : wnd.txtTravelPosNameKey;
                        if (wnd.txtTravelPosNameList != null)
                        {
                            foreach (var tmpText in wnd.txtTravelPosNameList)
                            {
                                if(tmpText != null)
                                    tmpText.text = TextTranslate.instance.getLanguage(travelPosNameKey, _m_rTravelEventInfo.travelPosRefObj.name);
                            }
                        }
                    }
                })
                .addDelegateProcess((_done) =>
                {
                    if(wnd == null || serialize != _m_lShowSerialize)
                        return;
                    
                    if(wnd.wndAnimation == null || _m_rTravelEventInfo == null || _m_rTravelEventInfo.travelPosRefObj == null || 
                       string.IsNullOrEmpty(_m_rTravelEventInfo.travelPosRefObj.travel_process_ani_name))
                    {
                        _done?.Invoke();
                        return;
                    }
                    
                    wnd.wndAnimation.Play(_m_rTravelEventInfo.travelPosRefObj.travel_process_ani_name, _done);
                })
                .addDelegateProcess((_done) =>
                {
                    if (wnd == null || serialize != _m_lShowSerialize)
                    {
                        return;
                    }

                    if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.afterVideoShowPosInfoAniName))
                    {
                        wnd.wndAnimation.Play(wnd.afterVideoShowPosInfoAniName, _done);
                    }
                    else
                    {
                        _done?.Invoke();
                    }
                })
                .addProcess(() =>
                {
                    _dealCloseWnd();
                });
            
            process.deal();
        }

        private void _dealCloseWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_RANDOM_TRAVEL_PROCESS);
        }
    }
}