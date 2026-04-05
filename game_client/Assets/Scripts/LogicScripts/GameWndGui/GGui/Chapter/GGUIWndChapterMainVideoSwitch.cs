using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡主视频界面
    /// </summary>
    public class GGUIWndChapterMainVideoSwitch : _ATALBasicUIWnd<GGUIMonoChapterMainVideoSwitch>, _IShakable
    {
        private static GGUIWndChapterMainVideoSwitch _g_instance = new GGUIWndChapterMainVideoSwitch();

        public static GGUIWndChapterMainVideoSwitch instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterMainVideoSwitch();
                return _g_instance;
            }
        }
        
        private NPGGUIWndCommonShowCase _m_showCaseVideoA;//展示视频A
        private NPGGUIWndCommonShowCase _m_showCaseVideoB;//展示视频B
        private GGuiWndSprite _m_maskImgA;//遮罩A
        private GGuiWndSprite _m_maskImgB;//遮罩B
        
        private long _m_serializeId = 0;
        private Tweener _m_tweener;

        //当前主显示的showcase
        private bool _m_curMainShowcaseIsB;
        
        // 震动任务的序列号
        private int _m_iShakeTaskSerialize;
        // 震动的原点坐标
        private Vector3 _m_vShakeOriginPos;
        //屏幕特效，只能存在1个
        private CommonTDSfxObj _m_screenSfxObj;
        
        public int taskSerialize { get { return _m_iShakeTaskSerialize; } }
        public Vector3 shakeOriginPos { get { return _m_vShakeOriginPos; } }
        public RectTransform shakeRoot { get { return wnd?.transShakeRoot; } }
        
        public GGUIWndChapterMainVideoSwitch() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoChapterMainVideoSwitch.assetPath; }
        protected override string _monoObjName { get => GGUIMonoChapterMainVideoSwitch.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }


        protected override void _onShowWnd()
        {       
            _m_serializeId = ALSerializeOpMgr.next();
            
            _reset();
        }

        protected override void _onHideWnd()
        {
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();

            _m_serializeId = ALSerializeOpMgr.next();
            _m_tweener?.Kill(true);
            
            _m_showCaseVideoA?.hideWnd();
            _m_showCaseVideoB?.hideWnd();
            
            _m_maskImgA?.hideWnd();
            _m_maskImgB?.hideWnd();

            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = null;
            
            _reset();
        }

        protected override void _onReset()
        {
            _m_serializeId = ALSerializeOpMgr.next();
            
            _m_showCaseVideoA?.resetWnd();
            _m_showCaseVideoB?.resetWnd();
            
            _m_maskImgA?.discardTexture();
            _m_maskImgB?.discardTexture();

            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = null;
            
            _reset();
        }

        protected override void _onDiscard()
        {
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            _m_serializeId = ALSerializeOpMgr.next();
            
            if (_m_showCaseVideoA != null) 
                _m_showCaseVideoA.discard();
            _m_showCaseVideoA = null;
            
            if (_m_showCaseVideoB != null) 
                _m_showCaseVideoB.discard();
            _m_showCaseVideoB = null;
            
            _m_maskImgA?.discard();
            _m_maskImgB?.discard();
            
            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            _m_screenSfxObj = null;
            
            _m_tweener?.Kill(true);
            
            _reset();
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            _m_serializeId = ALSerializeOpMgr.next();

            if (wnd.monoShowcaseA != null) 
                _m_showCaseVideoA = new NPGGUIWndCommonShowCase(wnd.monoShowcaseA);
            if (wnd.monoShowcaseB != null) 
                _m_showCaseVideoB = new NPGGUIWndCommonShowCase(wnd.monoShowcaseB);
            
            if (wnd.transShakeRoot != null)
                _m_vShakeOriginPos = wnd.transShakeRoot.anchoredPosition;

            _reset();
        }

        //设置主showcase的显示
        public void setShowcaseInfo(ShowCaseCommonResUnitInfoObj[] _commonResUnit, NPGSpriteIndex _maskIndex, int _id, bool _isMain)
        {
            if(null == wnd)
                return;
            
            if(_m_curMainShowcaseIsB && _isMain || !_m_curMainShowcaseIsB && !_isMain)
            {
                if (_m_showCaseVideoB != null) _m_showCaseVideoB.showWnd(_commonResUnit);
                if (_m_maskImgB != null && null != _m_maskImgB.image)
                {
                    if(null != _maskIndex)
                    {
                        _m_maskImgB.setTexture(_maskIndex);
                        _m_maskImgB.image.type = UnityEngine.UI.Image.Type.Filled;
                        _m_maskImgB.image.fillMethod = UnityEngine.UI.Image.FillMethod.Vertical;
                        _m_maskImgB.image.fillOrigin = (int)UnityEngine.UI.Image.OriginVertical.Top;
                        _m_maskImgB.showWnd();
                    }
                    else
                    {
                        _m_maskImgB.hideWnd();
                    }
                }
            }
            else
            {
                if (_m_showCaseVideoA != null) _m_showCaseVideoA.showWnd(_commonResUnit);
                if (_m_maskImgA != null && null != _m_maskImgA.image)
                {
                    if(null != _maskIndex)
                    {
                        _m_maskImgA.setTexture(_maskIndex);
                        _m_maskImgA.image.type = UnityEngine.UI.Image.Type.Filled;
                        _m_maskImgA.image.fillMethod = UnityEngine.UI.Image.FillMethod.Vertical;
                        _m_maskImgA.image.fillOrigin = (int)UnityEngine.UI.Image.OriginVertical.Top;
                        _m_maskImgA.showWnd();
                    }
                    else
                    {
                        _m_maskImgA.hideWnd();
                    }
                }
            }

            _setVideoControl(_id);
        }

        private void _setVideoControl(int _id)
        {
            if(null == wnd || null == wnd.videoAnimControllerList)
                return;

            foreach (ChapterVideoAnimatorController chapterVideoAnimatorController in wnd.videoAnimControllerList)
            {
                if(null != chapterVideoAnimatorController && chapterVideoAnimatorController.id == _id)
                {
                    if(wnd.forwardStateAnimA != null && null != wnd.forwardStateAnimA.animator)
                        wnd.forwardStateAnimA.animator.runtimeAnimatorController = chapterVideoAnimatorController.controller;
                    if(wnd.forwardStateAnimB != null && null != wnd.forwardStateAnimB.animator)
                        wnd.forwardStateAnimB.animator.runtimeAnimatorController = chapterVideoAnimatorController.controller;
                    break;
                }
            }
        }
        
        //播放放大动画
        public void sampleLargeAniFirstFrame(bool _isMain)
        {
            if(null == wnd)
                return;
            
            if(_m_curMainShowcaseIsB && _isMain || !_m_curMainShowcaseIsB && !_isMain)
            {
                wnd.switchAnim.Sample(wnd.enlargeAnimNameB, 0);
            }
            else
            {
                wnd.switchAnim.Sample(wnd.enlargeAnimNameA, 0);
            }
        }
        
        //播放放大动画
        public void playLargeAni(bool _isMain, Action _action)
        {
            if(null == wnd)
                return;
            
            if(_m_curMainShowcaseIsB && _isMain || !_m_curMainShowcaseIsB && !_isMain)
            {
                wnd.switchAnim.ForcePlay(wnd.enlargeAnimNameB, 0, _action);
            }
            else
            {
                wnd.switchAnim.ForcePlay(wnd.enlargeAnimNameA, 0, _action);
            }
        }

        public void playSfx(long _sfxId)
        {
            //播放屏幕特效
            if (_m_screenSfxObj != null) 
                _m_screenSfxObj.forceDiscard();
            
            if(_m_curMainShowcaseIsB )
            {
                _m_screenSfxObj = PlaySfxMgr.instance.playTDSfx(_sfxId, wnd.sfxParentB);
            }
            else
            {
                _m_screenSfxObj = PlaySfxMgr.instance.playTDSfx(_sfxId, wnd.sfxParentA);
            }
        }
        
        //播放轮换动画
        public void playTurnAnim(Action _action)
        {
            if(null == wnd || null == wnd.switchAnim)
            {
                if (_action != null) 
                    _action();
                return;
            }

            if (_m_curMainShowcaseIsB)
            {
                wnd.switchAnim.Play(wnd.switchAnimName2A, _action);
            }
            else
            {
                wnd.switchAnim.Play(wnd.switchAnimName2B, _action);
            }

            //修改标记位
            _m_curMainShowcaseIsB = !_m_curMainShowcaseIsB;
        }
        
        //播放轮换动画
        public void playTurnBossAnim(Action _action)
        {
            if(null == wnd || null == wnd.switchAnim)
            {
                if (_action != null) 
                    _action();
                return;
            }

            if (_m_curMainShowcaseIsB)
            {
                wnd.switchAnim.Play(wnd.switchBossAnimName2A, _action);
            }
            else
            {
                wnd.switchAnim.Play(wnd.switchBossAnimName2B, _action);
            }

            //修改标记位
            _m_curMainShowcaseIsB = !_m_curMainShowcaseIsB;
        }
        
        //播放前进动画
        public void playForwardAni(EChapterVideoForwardState _state)
        {
            if(null == wnd)
                return;
            
            wnd.forwardStateAnimA?.setShowData(_state);
            wnd.forwardStateAnimB?.setShowData(_state);

            
            // if((_m_curMainShowcaseIsB && _isMain) || (!_m_curMainShowcaseIsB && !_isMain))
            // {
            //     wnd.forwardStateAnimB?.setShowData(_state);
            // }
            // else
            // {
            //     wnd.forwardStateAnimA?.setShowData(_state);
            // }
        }
        
        /// <summary>
        /// 切换主试图动画名称
        /// </summary>
        /// <param name="_name"></param>
        public void playMainAniName(string _name)
        {
            //这边就是需要强切
            if(_m_curMainShowcaseIsB)
            {
                if (_m_showCaseVideoB != null) _m_showCaseVideoB.forceSetAni(0, _name);
            }
            else
            {
                if (_m_showCaseVideoA != null) _m_showCaseVideoA.forceSetAni(0, _name);
            }
        }
        
        /// <summary>
        /// 展示替换形象
        /// </summary>
        /// <param name="_name"></param>
        public void showReplaceUnit(_AShowCaseUnitInfoObj _unitInfo, float _time)
        {
            //这边就是需要强切
            if(_m_curMainShowcaseIsB)
            {
                if (_m_showCaseVideoB != null)
                {
                    _m_showCaseVideoB.enableRender(0, false);
                    _m_showCaseVideoB.additionLoadUnitAutoRemove(_unitInfo, 2, _time, () =>
                    {
                        _m_showCaseVideoB.enableRender(0, true);
                    });
                }
            }
            else
            {
                if (_m_showCaseVideoA != null)
                {
                    _m_showCaseVideoA.enableRender(0, false);
                    _m_showCaseVideoA.additionLoadUnitAutoRemove(_unitInfo, 2, _time, () =>
                    {
                        _m_showCaseVideoA.enableRender(0, true);
                    });
                }
            }
        }
        
        public void shake()
        {
            if (null == wnd)
                return;
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();

            new ShakeTask(this, wnd.fShakeIntensity, wnd.fShakeDuration, true).deal();
        }

        private void _reset()
        {
            if(null == wnd || null == wnd.switchAnim)
                return;
            
            _m_curMainShowcaseIsB = false;
            wnd.switchAnim.Sample(wnd.switchAnimName2B, 0);
        }
    }
}