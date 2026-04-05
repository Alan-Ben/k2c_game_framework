using ALPackage;
using NPEnum;
using Spine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndFuncUnlockTip : _ANPGGUIBasicWnd<NPGGUIMonoFuncUnlockTip>
    {
        private static NPGGUIWndFuncUnlockTip _g_instance;
        public static NPGGUIWndFuncUnlockTip instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndFuncUnlockTip();
                return _g_instance;
            }
        }

        private NPGGUIWndFuncUnlockTipContainer _m_wItemContainer;//解锁的item容器
        private List<FuncUnlockInfo> _m_funcUnlockInfoList;
        private long _m_lOpenWndTimeMs;//窗口打开时间点
        private long _m_lEnterAniEndTimeMs;//进入动画结束时间点
        private float _m_fEnterAniLengthS;//进入动画时间

        private NPGGUIWndFuncUnlockTip() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return NPGGUIMonoFuncUnlockTip.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoFuncUnlockTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _refreshWnd();
            _m_lOpenWndTimeMs = ALCommon.getNowTimeMill();
            _m_lEnterAniEndTimeMs = _m_lOpenWndTimeMs + (long)_m_fEnterAniLengthS*1000;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (_m_wItemContainer != null) 
                _m_wItemContainer.resetWnd();
            
            if(null != _m_funcUnlockInfoList)
                _m_funcUnlockInfoList.Clear();
            _m_funcUnlockInfoList = null;
        }

        protected override void _onDiscard()
        {
            if (_m_wItemContainer != null) 
                _m_wItemContainer.discard();
            _m_wItemContainer = null;

            if(null != _m_funcUnlockInfoList)
                _m_funcUnlockInfoList.Clear();
            _m_funcUnlockInfoList = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //获取途径item容器
            if (wnd.monoContainer != null)
            {
                _m_wItemContainer = new NPGGUIWndFuncUnlockTipContainer(wnd.monoContainer);
            }

            if (wnd.animator != null && wnd.animator.runtimeAnimatorController != null && wnd.animator.runtimeAnimatorController.animationClips != null && !string.IsNullOrEmpty(wnd.enterAnimatorName))
            {
                foreach (AnimationClip clip in wnd.animator.runtimeAnimatorController.animationClips)
                {
                    if (clip != null && clip.name == wnd.enterAnimatorName)
                    {
                        _m_fEnterAniLengthS = clip.length;
                        break;
                    }
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_funcUnlockInfos"></param>
        public void setInfo(List<FuncUnlockInfo> _funcUnlockInfos)
        {
            if(null == _funcUnlockInfos)
                return;

            if (null == _m_funcUnlockInfoList)
                _m_funcUnlockInfoList = new List<FuncUnlockInfo>();
            _m_funcUnlockInfoList.AddRange(_funcUnlockInfos);

            _refreshWnd();
        }

        /// <summary>
        /// 获取入口解锁飞行特效开始位置
        /// </summary>
        /// <returns></returns>
        public Vector3 getFlySfxStartWorldPos()
        {
            if (wnd == null || wnd.flySfxStartTransform == null)
                return Vector3.zero;

            if (wnd != null && wnd.flySfxStartTransform != null)
            {
                Camera cTDCamera = CameraController.instance.controlCamera;
                Camera cUICamera = MainCameraMono.selfInstance.uiCamera;

                if (null == cTDCamera || null == cUICamera)
                    return Vector3.zero;

                Vector3 screenPos = cUICamera.WorldToScreenPoint(wnd.flySfxStartTransform.position);
                screenPos.z = CameraController.instance.cameraNearClipPlane;
                Vector3 worldPos = cTDCamera.ScreenToWorldPoint(screenPos);
                return worldPos;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 获取入口解锁飞行特效及镜头移动时间
        /// </summary>
        /// <returns></returns>
        public float getSfxFlyTimeSec()
        {
            if (wnd == null)
                return 0;

            return wnd.sfxFlyTimeSec;
        }

        //刷新
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_funcUnlockInfoList)
                return;

            if (_m_wItemContainer != null) 
                _m_wItemContainer.showItemList(_m_funcUnlockInfoList);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            if (wnd == null)
                return;

            //当前时间
            long curTimeMs = ALCommon.getNowTimeMill();
            //经过的时间
            float passedTimeS = (curTimeMs - _m_lOpenWndTimeMs) / 1000f;
            if (passedTimeS < _m_fEnterAniLengthS && wnd.animator != null)
            {
                //如果当前时间小于进入动画总时间，则直接切换到idle动画
                wnd.animator.Play(wnd.idleAnimatorName);
                _m_lEnterAniEndTimeMs = curTimeMs;
                _m_fEnterAniLengthS = 0;
                return;
            }

            //超过idle动画最少展示时间，直接关闭
            if ((curTimeMs - _m_lEnterAniEndTimeMs)/1000f >= wnd.idleMiniShowTimeS)
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUNC_UNLOCK_TIP);
        }
    }
}
