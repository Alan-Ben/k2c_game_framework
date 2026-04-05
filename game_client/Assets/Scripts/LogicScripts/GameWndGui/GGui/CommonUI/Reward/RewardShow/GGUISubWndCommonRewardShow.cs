using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    public class GGUISubWndCommonRewardShow : _ANPGGUIBasicSubWnd<GGUISubMonoCommonRewardShow>
    {
        private GGUIPrefabWndPlayerSkinRewardShow _m_wPlayerSkinRewardShow;//玩家形象展示窗口
        
        public GGUISubWndCommonRewardShow(GGUISubMonoCommonRewardShow _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

        }
        
        protected override void _onDiscard()
        {
            _m_wPlayerSkinRewardShow?.discard();
            _m_wPlayerSkinRewardShow = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _hideAllRewardShowWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerSkinRewardShow?.resetWnd();
        }

        public void setShowReward(NPCommonCostItem _commonCostItem)
        {
            setShowReward(_commonCostItem?.item);
        }

        public void setShowReward(NPCommonItem _commonItem)
        {
            _hideAllRewardShowWnd();
            if(_commonItem == null)
                return;

            switch (_commonItem.itemType)
            {
                case ENPItemType.PLAYER_SKIN:
                    _showPlayerSkin(_commonItem.itemId);
                    break;
                
                default:
                    break;
            }
        }

        /// <summary>
        /// 隐藏所有奖励展示窗口
        /// </summary>
        private void _hideAllRewardShowWnd()
        {
            _m_wPlayerSkinRewardShow?.hideWnd();

        }

        #region 玩家皮肤展示

        private void _showPlayerSkin(long _skinId)
        {
            if(wnd == null || wnd.rewardShowParent == null || 
               wnd.playerSkinShowAssetPath == null || !wnd.playerSkinShowAssetPath.enable)
                return;

            if (_m_wPlayerSkinRewardShow == null)
            {
                _m_wPlayerSkinRewardShow = new GGUIPrefabWndPlayerSkinRewardShow(wnd.playerSkinShowAssetPath, wnd.rewardShowParent);
                _m_wPlayerSkinRewardShow.load();
            }

            _m_wPlayerSkinRewardShow.regLoadDoneDelegate(() =>
            {
                _m_wPlayerSkinRewardShow?.showWnd();
                _m_wPlayerSkinRewardShow?.setData(_skinId);
            });
        }

        #endregion
    }
}