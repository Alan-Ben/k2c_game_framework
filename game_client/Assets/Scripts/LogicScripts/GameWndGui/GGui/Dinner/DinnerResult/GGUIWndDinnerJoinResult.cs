using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会举办结算
    /// </summary>
    public class GGUIWndDinnerJoinResult : _ATALBasicUIWnd<GGUIMonoDinnerJoinResult>
    {
        private static GGUIWndDinnerJoinResult _g_instance = new GGUIWndDinnerJoinResult();
        private Action _m_onClose;
        private List<NPCommon.NPCommon_ItemInfo> _m_itemList;
        private GDinnerInfo _m_dinnerInfo;
        private long _m_score;
        private NPGGUIWndCommonItemContainer _m_itemContainer;
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        public static GGUIWndDinnerJoinResult instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerJoinResult();
                return _g_instance;
            }
        }
    
        public GGUIWndDinnerJoinResult() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get => GGUIMonoDinnerJoinResult.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerJoinResult.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_playerIconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_playerIconWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_onClose = null;
            _m_itemList = null;
            _m_dinnerInfo = null;
            
            _m_itemContainer?.discard();
            _m_itemContainer = null;
            
            _m_playerIconWnd?.discard();
            _m_playerIconWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (null != wnd.itemContainer)
            {
                _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
            }
            if (null != wnd.playerInfo)
            {
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onClickClose);
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_resultInfo">奖励物品列表</param>
        /// <param name="_dinnerInfo">宴会信息</param>
        /// <param name="_score">获得积分</param>
        /// <param name="_onClose">关闭回调</param>
        public void setInfo(List<NPCommon.NPCommon_ItemInfo> _resultInfo, GDinnerInfo _dinnerInfo, long _score, Action _onClose)
        {
            _m_itemList = _resultInfo;
            _m_dinnerInfo = _dinnerInfo;
            _m_onClose = _onClose;
            _m_score = _score;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            // 设置开宴玩家信息
            if (null != _m_dinnerInfo && null != _m_playerIconWnd)
            {
                _m_playerIconWnd.setPlayer(_m_dinnerInfo.ownerCid, _m_playerIconWnd.showWnd);
            }
            
            // 设置宴会名称
            if (null != _m_dinnerInfo && null != _m_dinnerInfo.dinnerTypeRef)
            {
                ALUGUICommon.setLabelTxt(wnd.txtDinnerName, TextTranslate.instance.getLanguage(_m_dinnerInfo.dinnerTypeRef.name));
            }
            
            // 设置获得积分
            ALUGUICommon.setLabelTxt(wnd.txtScore, TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_add_score, _m_score));
            
            // 设置奖励物品列表
            if (null != _m_itemList && _m_itemList.Count > 0)
            {
                List<NPCommonCostItem> costItems = new List<NPCommonCostItem>();
                foreach (NPCommon_ItemInfo itemInfo in _m_itemList)
                {
                    costItems.Add(new NPCommonCostItem(itemInfo));
                }
                _m_itemContainer?.showWnd();
                _m_itemContainer?.showItemList(costItems);
            }
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            _m_onClose?.Invoke();
        }
    }
}