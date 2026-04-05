using UnityEngine;
using ALPackage;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 炫耀性物品子窗口
    /// </summary>
    public class NPGGUIWndGetShowOffItemSub : _ANPGGUIBasicLoadPrefabSubWnd<NPGGUIMonoGetShowOffItemSub>
    {
        private NPGGUIWndCommonItem _m_commonItemWnd;

        private long _m_ui_path_id;
        private NPCommon_ItemInfo _m_item;
        //是否穿戴成功
        private bool _m_isPutOn;

        //穿戴道具预制体item
        private GGUIWndPrefabSubDressItem _m_wDressItem;

        public NPGGUIWndGetShowOffItemSub(long _pathId, Transform _parent)
            : base(_parent)
        {
            _m_ui_path_id = _pathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_ui_path_id); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_ui_path_id); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public long uiPathId { get => _m_ui_path_id; }


        protected override void _onDiscard()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.discard();
            _m_commonItemWnd = null;

            if (null != _m_wDressItem)
                _m_wDressItem.discard();
            _m_wDressItem = null;
        }

        protected override void _onReset()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.resetWnd();

            if(null != _m_wDressItem)
                _m_wDressItem.resetWnd();
        }

        protected override void _onHideWnd()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.hideWnd();

            if (null != _m_wDressItem)
                _m_wDressItem.hideWnd();
        }

        protected override void _onShowWnd()
        {
            //刷新界面
            setPutOn(false);
            _refreshWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            if (null != wnd.commonItemMono)
                _m_commonItemWnd = new NPGGUIWndCommonItem(wnd.commonItemMono);
        }

        public void setItem(NPCommon_ItemInfo _item)
        {
            if (null == _item)
                return;

            _m_item = _item;
            _refreshWnd();
        }

        public void setPutOn(bool _isPutOn)
        {
            _m_isPutOn = _isPutOn;
            ALUGUICommon.setGameObjEnable(wnd.putOnSucShowGoList, _m_isPutOn);
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            long uiResId = 0;
            NPGSpriteIndex sptIcon = null;
            if ((ENPItemType)_m_item.getItemType() == ENPItemType.BUBBLE)
            {
                NPPlayerBubbleRefObj refObj = GRefdataCoreMgr.instance.playerBubbleCore.getRef(_m_item.getSubId());
                uiResId = refObj == null ? 0 : refObj.asset_path_id;
                sptIcon = refObj?.spt_icon;
            }
            else if ((ENPItemType)_m_item.getItemType() == ENPItemType.ICON)
            {
                PlayerIconRefObj refObj = GRefdataCoreMgr.instance.playerIconCore.getRef(_m_item.getSubId());
                uiResId = refObj == null ? 0 : refObj.asset_path_id;
            }
            else if ((ENPItemType)_m_item.getItemType() == ENPItemType.ICON_BGK)
            {
                PlayerIconBgkRefObj refObj = GRefdataCoreMgr.instance.iconBgkCore.getRef(_m_item.getSubId());
                uiResId = refObj == null ? 0 : refObj.asset_path_id;
            }
            else if ((ENPItemType)_m_item.getItemType() == ENPItemType.TITLE)
            {
                PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_item.getSubId());
                uiResId = titleInfo == null ? 0 : titleInfo.getUiResId();
            }

            GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wDressItem, uiResId, wnd.parentPos, (_item) =>
            {
                _m_wDressItem = _item;
                _m_wDressItem?.showWnd();
                _m_wDressItem?.setIcon((ENPItemType)_m_item.getItemType(), _m_item.getSubId());
                if((ENPItemType)_m_item.getItemType() == ENPItemType.TITLE)
                    _m_wDressItem?.setName(GCommon.getItemName(ENPItemType.TITLE, _m_item.getSubId()));
                else if ((ENPItemType) _m_item.getItemType() == ENPItemType.BUBBLE)
                    _m_wDressItem?.setSptIcon(sptIcon);
            });

            if (null != _m_commonItemWnd)
            {
                _m_commonItemWnd.showWnd();
                NPCommonCostItem costItem = new NPCommonCostItem(_m_item);
                _m_commonItemWnd.setItem(costItem);
            }

        }
    }
}
