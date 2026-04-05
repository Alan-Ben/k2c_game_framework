using UnityEngine;
using ALPackage;
using NPEnum;
using CommonEnum;

namespace GOE
{
    // 玩家等级预览逻辑
    public class GGUIWndPlayerLvPreview : _ANPGGUIBasicWnd<GGUIMonoPlayerLvPreview>
    {
        private static GGUIWndPlayerLvPreview _g_instance = new GGUIWndPlayerLvPreview();

        public static GGUIWndPlayerLvPreview instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndPlayerLvPreview();

                return _g_instance;
            }
        }

        public GGUIWndPlayerLvPreview() : base(EALUIWndLayer.ADDITION)
        {
        }

        //等级信息
        private GGUIWndPlayerLv _m_lvWnd;
        //等级预览grid
        private GGUIWndPlayerLvPreviewGrid _m_lvPreviewGridWnd;
        //经验进度
        private NPGGUIWndProgress _m_expProgressWnd;
        //等级配置
        private PlayerLvlRefObj _m_lvRefObj;
        //奖励列表
        private GGUIWndCommonRewardContainer _m_itemContainer;

        protected override string _monoAssetPath { get { return GGUIMonoPlayerLvPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerLvPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        #region override
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.lvPreviewGridMono)
            {
                _m_lvPreviewGridWnd = new GGUIWndPlayerLvPreviewGrid(wnd.lvPreviewGridMono);
                _m_lvPreviewGridWnd.setChgLvRefAction(_chgLvRefDelegate);
            }

            if (null != wnd.lvMono)
                _m_lvWnd = new GGUIWndPlayerLv(wnd.lvMono);

            if (null != wnd.itemContainer)
                _m_itemContainer = new GGUIWndCommonRewardContainer(wnd.itemContainer);

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }
        protected override void _onShowWnd()
        {
            if (null != _m_lvPreviewGridWnd)
                _m_lvPreviewGridWnd.showWnd();

            if (null != _m_expProgressWnd)
                _m_expProgressWnd.showWnd();

            _setLvRefObj(NPPlayer.instance.playerInfo.curLevelRef);
        }
        protected override void _onHideWnd()
        {
            if (null != _m_lvPreviewGridWnd)
                _m_lvPreviewGridWnd.hideWnd();

            if (null != _m_expProgressWnd)
                _m_expProgressWnd.hideWnd();

            if(null != _m_itemContainer)
                _m_itemContainer.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_lvPreviewGridWnd)
                _m_lvPreviewGridWnd.resetWnd();

            if (null != _m_expProgressWnd)
                _m_expProgressWnd.resetWnd();

            if (null != _m_itemContainer)
                _m_itemContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_lvWnd)
                _m_lvWnd.discard();
            _m_lvWnd = null;

            if (null != _m_lvPreviewGridWnd)
                _m_lvPreviewGridWnd.discard();
            _m_lvPreviewGridWnd = null;

            if (null != _m_expProgressWnd)
                _m_expProgressWnd.discard();
            _m_expProgressWnd = null;

            if (null != _m_itemContainer)
                _m_itemContainer.discard();
            _m_itemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        #endregion

        private void _setLvRefObj(PlayerLvlRefObj _refObj)
        {
            if (null == _refObj)
                return;

            _m_lvRefObj = _refObj;
            _refresh();
        }

        //设置等级
        private void _refresh()
        {
            if (null == _m_lvRefObj || null == wnd)
                return;

            if (null != _m_lvWnd)
                _m_lvWnd.setLvRefObj(_m_lvRefObj);

            bool isCurLv = _m_lvRefObj.lvl == NPPlayer.instance.playerInfo.getCurrentLevel();
            if(null != wnd.lvMono)
            {
                if (isCurLv)
                    ALUGUICommon.setUIObjColor(wnd.lvMono.lvNameTxt, wnd.curLvShowColor);
                else
                    ALUGUICommon.setUIObjColor(wnd.lvMono.lvNameTxt, wnd.otherLvShowColor);
            }
            GGameCommonInfo.grayImage(wnd.grayImgList, !isCurLv);

            ALUGUICommon.setGameObjEnable(wnd.curLvShowGoList, isCurLv);

            if (null != _m_expProgressWnd)
            {
                PlayerLvlRefObj nextRefObj = NPPlayer.instance.playerInfo.nextLevelRef;
                long curLvExp = NPPlayer.instance.playerInfo.curLevelRef.exp;
                ALUGUICommon.setGameObjEnable(wnd.lvMaxShowGoList, null == nextRefObj);
                if (null == nextRefObj)
                    nextRefObj = NPPlayer.instance.playerInfo.curLevelRef;
                _m_expProgressWnd.setProgress(GCommon.getItemCount(ENPItemType.CURRENCY, (int)ECurrency.P_EXP) - curLvExp, nextRefObj.exp - curLvExp, EValueFormatType.NORMAL_NOT_LARGE_STR, TransKeyConst.playerInfo_expProgress_num_num);

            }

            //刷新奖励列表
            if (null != _m_itemContainer)
            {
                ECommonRewardType rewardType = _m_lvRefObj.lvl > NPPlayer.instance.playerInfo.getCurrentLevel()?ECommonRewardType.NOT_GET_REWARD: ECommonRewardType.HAS_GET_REWARD;
                _m_itemContainer.showWnd();
                _m_itemContainer.setRewardList(_m_lvRefObj.show_reward_item_list, rewardType);
            }
        }

        private void _chgLvRefDelegate(PlayerLvlRefObj _refObj)
        {
            _setLvRefObj(_refObj);
        }

        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_LV_PREVIEW_NODE);
        }
    }
}
