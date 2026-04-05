using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using GS2GC.p004_PlayerOp;

namespace GOE
{

    /// <summary>
    /// 玩家信息主界面
    /// </summary>
    public class GMainGUIAddScenePlayerInfo : _ANPGMainGUIAddSceneResBar<GGUIWndPlayerInfo>
    {
        private static GMainGUIAddScenePlayerInfo _g_instance = new GMainGUIAddScenePlayerInfo();
        public static GMainGUIAddScenePlayerInfo instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddScenePlayerInfo();

                return _g_instance;
            }
        }
        
        protected override GGUIWndPlayerInfo _m_wnd { get { return GGUIWndPlayerInfo.instance; } }

        /// <summary>
        /// 查看玩家详情
        /// </summary>
        public void showPlayerDetail(long _cid,RectTransform _targetRectTransform,float _interval, RectTransform _rangRectTrans)
        {
            GCommon.reqPlayerInfo(_cid, (_info) =>
            {
                GCommon.showPlayerInfoWndTip(_info, _targetRectTransform, _interval, _rangRectTrans);
            });
        }   
    }
}
