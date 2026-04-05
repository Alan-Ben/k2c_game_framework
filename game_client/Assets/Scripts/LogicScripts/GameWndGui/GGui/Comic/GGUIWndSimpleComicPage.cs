using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    //漫画的每一页
    public class GGUIWndSimpleComicPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoSimpleComicPage>
    {
        //资源id
        private readonly long _m_uiPathId;
        //序列号
        private long _m_serialNum;
        
        //播放结束回调
        public event Action onPlayEnd;
        
        public GGUIWndSimpleComicPage(long _uiPathId, Transform _parent)
            : base(_parent)
        {
            _m_uiPathId = _uiPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            _m_serialNum = 0;
        }
        
        protected override void _onShowWnd()
        {
            _m_serialNum = 0;
        }
        
        protected override void _onHideWnd()
        {
            _m_serialNum = 0;
        }

        protected override void _onReset()
        {
            _m_serialNum = 0;
        }

        protected override void _onDiscard()
        {
            _m_serialNum = 0;
        }
        
        //后续可能有播放动画需求
        public void startPlay()
        {
            if(null == wnd)
                return;

            _m_serialNum++;
            long serialNum = _m_serialNum;

            if (wnd.next_time >= 0)
            {
                //先临时用时间控制，后面可以改成动画播放完毕的回调
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(serialNum != _m_serialNum)
                        return;
                
                    if(null != onPlayEnd)
                        onPlayEnd();
                }, wnd.next_time);   
            }
        }
    }
}
