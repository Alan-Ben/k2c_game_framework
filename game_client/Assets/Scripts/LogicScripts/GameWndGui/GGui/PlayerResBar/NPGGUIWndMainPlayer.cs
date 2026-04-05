using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

using NPEnum;

namespace GOE
{
    public class NPGGUIWndMainPlayer : _ANPGGUIBasicWnd<NPGGUIMonoMainPlayer>
    {

        //路径
        private string _m_assetPath;
        //名字
        private string _m_objName;
        
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;//玩家头像+头像框
        
        public NPGGUIWndMainPlayer(string _assetPath, string _objName) : base(EALUIWndLayer.NORMAL)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }
        
        protected override string _monoAssetPath { get { return _m_assetPath; } }
        protected override string _monoObjName { get { return _m_objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 玩家头像
            if (wnd.monoPlayerIcon != null)
            {
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
            }

        }
        protected override void _onShowWnd()
        {
            _refreshWindow();

            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refreshWindow);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refreshWindow);
        }

        protected override void _onReset()
        {
            if(_m_wPlayerIcon != null)
                _m_wPlayerIcon.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_wPlayerIcon != null)
                _m_wPlayerIcon.discard();
            _m_wPlayerIcon = null;


        }

        private void _refreshWindow()
        {
            // 设置玩家基本信息
            _setPlayerInfo();
        }

        //设置玩家信息
        private void _setPlayerInfo()
        {
            if(_m_wPlayerIcon != null)
                _m_wPlayerIcon.setSelfInfo();
        }
        

        //这边的表现可能需要优化，让进度条走的平缓点
        private void _onPlayerParamChanged(params object[] _objs)
        {
            ENPPlayerParam paramType = (ENPPlayerParam)_objs[0];
            if(paramType == ENPPlayerParam.ICON || paramType == ENPPlayerParam.ICON_BGK || paramType == ENPPlayerParam.LEVEL)
            {
                _refreshWindow();
            }
        }
    }
}
