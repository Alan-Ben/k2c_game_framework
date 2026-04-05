using System;
using ALPackage;
using UnityEngine;

namespace Hotfix
{
    public class GGUIWndTileMatchStepRewardBox : _AHotfixBaseSubPrefabWnd<GGUIMonoTileMatchStepRewardBox>
    {
        private NPCommonAssetPathInfo _m_AssetPathInfo;
        
        public GGUIWndTileMatchStepRewardBox(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_AssetPathInfo = _assetPathInfo;
        }

        public NPCommonAssetPathInfo boxAssetPathInfo => _m_AssetPathInfo;
        
        protected override string _monoAssetPath { get { return _m_AssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_AssetPathInfo?.obj_name; } }
        
        public event Action onDrawBtnClick;
        
        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;
         
            ALUGUICommon.combineBtnClick(hotfixWnd.btnDraw, _onDrawBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (hotfixWnd != null)
            {
                ALUGUICommon.combineBtnClick(hotfixWnd.btnDraw, _onDrawBtnClick);
            }

            onDrawBtnClick = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            ALMsgSys.RegisterMsgAct(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG, _onTileMatchCanDrawStepRewardChg);
        }

        protected override void _onHideWnd()
        {
            ALMsgSys.UnregisterMsgAct(HotfixMsgType.ON_TILEMATCH_CAN_DRAW_STEP_REWARD_INFO_CHG, _onTileMatchCanDrawStepRewardChg);
        }

        protected override void _onReset()
        {
        }

        private void _refreshWnd()
        {
            if(hotfixWnd == null)
                return;
            
            bool hasCanReward = HotfixNPPlayer.instance.tileMatchComponent.canDrawStepRewardCount > 0;//是否有可领取奖励
            ALUGUICommon.setGameObjEnable(hotfixWnd.hasRewardCanDrawShow, hasCanReward);
            ALUGUICommon.setGameObjEnable(hotfixWnd.noRewardCanDrawShow, !hasCanReward);

            ALUGUICommon.setLabelTxt(hotfixWnd.txtCanDrawReward, HotfixNPPlayer.instance.tileMatchComponent.canDrawStepRewardCount);
        }

        /// <summary>
        /// 点击领取奖励按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onDrawBtnClick(GameObject _go)
        {
            onDrawBtnClick?.Invoke();
        }
        
        private void _onTileMatchCanDrawStepRewardChg()
        {
            _refreshWnd();
        }
    }
}