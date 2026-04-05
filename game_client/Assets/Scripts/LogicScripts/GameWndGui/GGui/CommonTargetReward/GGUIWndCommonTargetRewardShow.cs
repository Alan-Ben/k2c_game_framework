using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 通用目标奖励形象窗口
    public class GGUIWndCommonTargetRewardShow : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoCommonTargetRewardShow>
    {
        private readonly long _m_uiPathId;
        public GGUIWndCommonTargetRewardShow(long _uiPathId, Transform _parent)
            : base(_parent)
        {
            _m_uiPathId = _uiPathId;
        }
        
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }



        protected override void _onWndInitDone()
        {
            
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        { 
            
        }
        protected override void _onDiscard()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rewardType"></param>
        public void setInfo(ECommonRewardType _rewardType)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goAlreadyGetShowList, _rewardType == ECommonRewardType.HAS_GET_REWARD);
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyGetHideList, _rewardType != ECommonRewardType.HAS_GET_REWARD);
        }
    }
}
