using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndCommonRewardPreview : NPGGUIWndCommonRewardPreview<NPGGUIMonoCommonRewardPreview>
    {
        public NPGGUIWndCommonRewardPreview(long _uiPathId) : base(_uiPathId)
        {
        }
    }
    
    /// <summary>
    /// 通用奖励预览窗口
    /// </summary>
    public class NPGGUIWndCommonRewardPreview<T_MONO> : _ATALBasicUIWnd<T_MONO> where T_MONO : NPGGUIMonoCommonRewardPreview
    {
        private GGUIWndCommonRewardContainer _m_itemContainer;
        private List<_IItem> _m_itemList;
        private long _m_uiPathId;
        private string _m_titleKey;
        private string _m_contentStr;
        private ECommonRewardType _m_type; //奖励类型
        private NPGGuiWndTexture _m_wBoxIcon;//宝箱图标
        private NPGTextureIndex _m_tNotGetBoxIcon;//未领取宝箱图标资源
        private NPGTextureIndex _m_tAlreadyGetBoxIcon;//已领取宝箱图标资源

        public NPGGUIWndCommonRewardPreview(long _uiPathId)
            : base(EALUIWndLayer.ADDITION)
        {
            _m_uiPathId = _uiPathId;
        }

        protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(_m_uiPathId); }
        protected override string _monoObjName { get => UIResPathAssistant.getObjName(_m_uiPathId); }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        
        protected override void _onShowWnd()
        {
            // _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wBoxIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBoxIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null != _m_itemContainer)
            {
                _m_itemContainer.discard();
                _m_itemContainer = null;
            }
            _m_itemList?.Clear();
            _m_itemList = null;

            _m_wBoxIcon?.discard();
            _m_wBoxIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose,_onClickClose);

            if (null != wnd.itemContainer)
            {
                _m_itemContainer = new GGUIWndCommonRewardContainer(wnd.itemContainer);
            }
            _m_itemList = new List<_IItem>();

            if (wnd.imgBoxIcon != null)
                _m_wBoxIcon = new NPGGuiWndTexture(wnd.imgBoxIcon);
        }
        
        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_rewardId"></param>
        public void setData(
            long _rewardId,
            string _titleKey = TransKeyConst.common_reward_preview_title,
            string _contentStr = null,
            ECommonRewardType _type = ECommonRewardType.NONE,
            NPGTextureIndex _notGetBoxIcon = null,
            NPGTextureIndex _alreadyGetBoxIcon = null, bool _needSortByQuality = true)
        {
            List<NPCommonCostItem> itemList = GCommon.getShowListByReawrdId(_rewardId);
            if (null != itemList)
            {
                setData(itemList.toItemDataList(),_titleKey,_contentStr, _type, _notGetBoxIcon, _alreadyGetBoxIcon, _needSortByQuality);
            }
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemList"></param>
        public void setData(
            List<NPCommonCostItem> _itemList,
            string _titleKey = TransKeyConst.common_reward_preview_title,
            string _contentStr = null, ECommonRewardType _type = ECommonRewardType.NONE,
            NPGTextureIndex _notGetBoxIcon = null,
            NPGTextureIndex _alreadyGetBoxIcon = null, bool _needSortByQuality = true)
        {
            
            if (null != _itemList)
            {
                setData(_itemList.toItemDataList(),_titleKey,_contentStr, _type, _notGetBoxIcon, _alreadyGetBoxIcon, _needSortByQuality);   
            }
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_uiPathId"></param>
        public void setData(
            List<_IItem> _itemList,
            string _titleKey = TransKeyConst.common_reward_preview_title,
            string _contentStr = null, 
            ECommonRewardType _type = ECommonRewardType.NONE,
            NPGTextureIndex _notGetBoxIcon = null,
            NPGTextureIndex _alreadyGetBoxIcon = null, bool _needSortByQuality = true)
        {
            if(null == wnd)
                return;
            _m_itemList.Clear();
            GCommon.itemListTONoRewardItemList(_itemList, _m_itemList, 0);
            
            if(_needSortByQuality)
                _m_itemList.Sort(_sortByQuality);
            _m_titleKey = _titleKey;
            _m_contentStr = _contentStr;
            _m_type = _type;
            _m_tNotGetBoxIcon = _notGetBoxIcon;
            _m_tAlreadyGetBoxIcon = _alreadyGetBoxIcon;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            //刷新列表
            if (null != _m_itemContainer)
            {
                _m_itemContainer.setRewardList(_m_itemList,_m_type);
            }

            //设置标题描述
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_titleKey));
            ALUGUICommon.setLabelTxt(wnd.txtContentDesc, TextTranslate.instance.getLanguage(_m_contentStr));

            //设置宝箱图标
            if (_m_wBoxIcon != null)
            {
                _m_wBoxIcon.showWnd();
                switch (_m_type)
                {
                    case ECommonRewardType.NONE:
                    case ECommonRewardType.NOT_GET_REWARD:
                    case ECommonRewardType.CAN_GET_REWARD:
                        _m_wBoxIcon.setTexture((_m_tNotGetBoxIcon != null && _m_tNotGetBoxIcon.isValid()) ? _m_tNotGetBoxIcon : GRefdataCoreMgr.instance.npGeneral.reward_preview_default_not_get_box_icon);
                        break;
                    case ECommonRewardType.HAS_GET_REWARD:
                        _m_wBoxIcon.setTexture((_m_tAlreadyGetBoxIcon != null && _m_tAlreadyGetBoxIcon.isValid()) ? _m_tAlreadyGetBoxIcon : GRefdataCoreMgr.instance.npGeneral.reward_preview_default_already_get_box_icon);
                        break;
                }
            }
        }


        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }

        protected int _sortByQuality(_IItem _x, _IItem _y)
        {
            if (_x.getQuality() > _y.getQuality())
                return -1;
            if (_x.getQuality() < _y.getQuality())
                return 1;

            if (_x.subId < _y.subId)
                return -1;
            if (_x.subId > _y.subId)
                return 1;
            return 0;
        }
    }
}