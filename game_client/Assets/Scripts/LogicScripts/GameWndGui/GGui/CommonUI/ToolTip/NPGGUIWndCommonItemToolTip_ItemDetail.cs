using System;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIWndCommonItemToolTip_ItemDetail : _ATNPGGUIWndCommonItemToolTipCheckPosition<NPGGUIMonoCommonToolTip_ItemDetail>
    {
        private ENPItemType _m_eItemType;//物品类型
        private long _m_lItemId;//物品id
        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private GGuiWndSprite _m_wQualityWnd;//品质图片
        private Action _m_aOnClickDetail;//点击详情回调
        private GGUIWndPrefabSubDressItem _m_wDressItem;//加载的装扮item

        public NPGGUIWndCommonItemToolTip_ItemDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {

        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            _m_wTexIconWnd?.hideWnd();
            _m_wQualityWnd?.hideWnd();
            _m_wDressItem?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();

            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discardTexture();

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discardTexture();

            if (_m_wDressItem != null)
                _m_wDressItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discard();
            _m_wTexIconWnd = null;

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discard();
            _m_wQualityWnd = null;

            if (_m_wDressItem != null)
                _m_wDressItem.discard();
            _m_wDressItem = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;

            if (wnd.imgItemIcon != null)
            {
                _m_wTexIconWnd = new NPGGuiWndTexture(wnd.imgItemIcon);
            }

            if (wnd.imgQualityBg != null)
            {
                _m_wQualityWnd = new GGuiWndSprite(wnd.imgQualityBg);
            }
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_ItemDetail));
        }


        #region 窗体事件

        /// <summary>
        /// 刷新窗体显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //物品名称
            ALUGUICommon.setLabelTxt(wnd.txtItemName, GCommon.getItemName(_m_eItemType, _m_lItemId));
            //持有数量
            ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(TransKeyConst.common_resource_haveNum, GCommon.getItemCount(_m_eItemType, _m_lItemId).ToLargeString(_m_eItemType.getLargeStringType(_m_lItemId))));
            //获取途径
            ALUGUICommon.setLabelTxt(wnd.txtAccess, TextTranslate.instance.getLanguage(TransKeyConst.common_resource_production_tip, GCommon.getItemSource(_m_eItemType, _m_lItemId)));
            //物品描述
            ALUGUICommon.setLabelTxt(wnd.txtItemDesc, GCommon.getItemDesc(_m_eItemType, _m_lItemId));

            //物品图片
            if (_m_wTexIconWnd != null)
            {
                _m_wTexIconWnd.setTexture(GCommon.getItemTexIcon(_m_eItemType, _m_lItemId));
                _m_wTexIconWnd.showWnd();
            }

            //品质底图
            if (_m_wQualityWnd != null)
            {
                _m_wQualityWnd.setTexture(GCommon.getItemQualityIcon(_m_eItemType, _m_lItemId));
                _m_wQualityWnd.showWnd();
            }

            //装扮物品特殊加载处理
            Transform goDressParent = wnd.getDressParent(_m_eItemType);
            if (goDressParent != null)
            {
                long uiResId = 0;
                NPGSpriteIndex sptIcon = null;
                switch (_m_eItemType)
                {
                    case ENPItemType.BUBBLE:
                        NPPlayerBubbleRefObj bubbleRefObj = GRefdataCoreMgr.instance.playerBubbleCore.getRef(_m_lItemId);
                        uiResId = bubbleRefObj == null ? 0 : bubbleRefObj.asset_path_id;
                        sptIcon = bubbleRefObj?.spt_icon;
                        break;
                    case ENPItemType.ICON:
                        PlayerIconRefObj iconRefObj = GRefdataCoreMgr.instance.playerIconCore.getRef(_m_lItemId);
                        uiResId = iconRefObj == null ? 0 : iconRefObj.asset_path_id;
                        break;
                    case ENPItemType.ICON_BGK:
                        PlayerIconBgkRefObj iconBgkRefObj = GRefdataCoreMgr.instance.iconBgkCore.getRef(_m_lItemId);
                        uiResId = iconBgkRefObj == null ? 0 : iconBgkRefObj.asset_path_id;
                        break;
                    case ENPItemType.TITLE:
                        PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_lItemId);
                        uiResId = GCommon.getPlayerTitleUIResId(_m_lItemId, titleInfo == null ? 1 : titleInfo.gainCount);
                        break;
                }

                //资源id有效则加载预制体
                if (uiResId > 0)
                {
                    GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wDressItem, uiResId, goDressParent, (_item) =>
                    {
                        _m_wDressItem = _item;
                        _m_wDressItem?.showWnd();
                        _m_wDressItem?.setIcon(_m_eItemType, _m_lItemId);
                        if (_m_eItemType == ENPItemType.TITLE)
                            _m_wDressItem?.setName(GCommon.getItemName(ENPItemType.TITLE, _m_lItemId));
                        else if (_m_eItemType == ENPItemType.BUBBLE)
                            _m_wDressItem?.setSptIcon(sptIcon);
                    });
                }
            }
        }

        #endregion


        private void _onBtnDetailClick(GameObject _obj)
        {
            GCommon.showItemDetail(_m_eItemType, _m_lItemId);
        }

        #region 外部调用

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_itemId"></param>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_interval"></param>
        public void setShowData(ENPItemType _itemType, long _itemId, RectTransform _targetTransRoot, float _interval)
        {
            //设置信息
            _m_eItemType = _itemType;
            _m_lItemId = _itemId;

            //刷新窗体
            _refreshWnd();

            if (wnd.txtItemDesc != null) LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtItemDesc.rectTransform);

            //设置位置
            setPos(_targetTransRoot, _interval);
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_interval"></param>
        public void setShowData(_IItem _item, RectTransform _targetTransRoot, float _interval)
        {
            if (_item == null)
                return;

            //设置显示数据
            setShowData(_item.getItemType(), _item.subId, _targetTransRoot, _interval);
        }

        #endregion
    }
}
