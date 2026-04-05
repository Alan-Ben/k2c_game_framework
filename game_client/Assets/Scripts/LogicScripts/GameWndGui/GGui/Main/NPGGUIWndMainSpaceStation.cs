using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 空间站界面
    /// </summary>
    public class NPGGUIWndMainSpaceStation : _ANPGGUIBasicResBarWnd<NPGGUIMonoMainSpaceStation>
    {
        private static NPGGUIWndMainSpaceStation _g_instance = new NPGGUIWndMainSpaceStation();
        public static NPGGUIWndMainSpaceStation instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndMainSpaceStation();
                return _g_instance;
            }
        }

        //显示序列
        private long _m_lShowSerialize;


        private NPGGUISubWndMiniChat _m_chatMiniWnd; // 聊天入口
        
        public NPGGUIWndMainSpaceStation() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoMainSpaceStation.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoMainSpaceStation.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            //显示聊天栏
            if (null != _m_chatMiniWnd)
                _m_chatMiniWnd.showWnd();
            
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (null != _m_chatMiniWnd)
            {
                _m_chatMiniWnd.discard();
            }

        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if (null != wnd.chatMiniWndMono)
            {
                _m_chatMiniWnd = new NPGGUISubWndMiniChat(wnd.chatMiniWndMono);
            }

        }
    }
}
