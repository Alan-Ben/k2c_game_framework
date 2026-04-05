using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using GS2GC.p004_PlayerOp;

namespace GOE
{

    /// <summary>
    /// 其他玩家信息主界面
    /// </summary>
    public class GMainGUIAddSceneOtherPlayerInfo : _ANPGMainGUIAddSceneResBar<GGUIWndOtherPlayerInfo>
    {
        private static GMainGUIAddSceneOtherPlayerInfo _g_instance = new GMainGUIAddSceneOtherPlayerInfo();
        public static GMainGUIAddSceneOtherPlayerInfo instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneOtherPlayerInfo();

                return _g_instance;
            }
        }
        
        protected override GGUIWndOtherPlayerInfo _m_wnd { get { return GGUIWndOtherPlayerInfo.instance; } }

        public void setData(NPCommonSimplePlayerInfo _info)
        {
            if (null != _m_wnd)
                _m_wnd.setData(_info);
        }
    }
}
