using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会举办结算
    /// </summary>
    public class GGUIWndDinnerCreateResult : _ATALBasicUIWnd<GGUIMonoDinnerCreateResult>
    {
        private static GGUIWndDinnerCreateResult _g_instance = new GGUIWndDinnerCreateResult();
        private Action _m_onClose;
        private Dinner_ResultInfo _m_resultInfo;
        private NPGGUIWndCommonItemContainer _m_itemContainer;
        private List<GDinnerGuestInfo> _m_guestList;
    
        private GGUIWndDinnerCreateResultJoinItemContainer _m_joinItemContainer;

        public static GGUIWndDinnerCreateResult instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerCreateResult();
                return _g_instance;
            }
        }
    
        public GGUIWndDinnerCreateResult() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get => GGUIMonoDinnerCreateResult.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerCreateResult.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
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
            _m_itemContainer?.discard();
            _m_itemContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnJoinerDetail, _onClickJoinerDetail);
            if (null != wnd.itemContainer)
            {
                _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
            }
            if (null != wnd.joinItemContainer)
            {
                _m_joinItemContainer = new GGUIWndDinnerCreateResultJoinItemContainer(wnd.joinItemContainer);
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_chanleList"></param>
        public void setInfo(Dinner_ResultInfo _resultInfo, Action _onClose)
        {
            if(_resultInfo == null)
                return;
            _m_resultInfo = _resultInfo;
        
            if(_m_guestList == null)
                _m_guestList = new List<GDinnerGuestInfo>();
            _m_guestList.Clear();
            foreach (var info in _m_resultInfo.getGuestLog())
            {
                _m_guestList.Add(new GDinnerGuestInfo(info));
            }
            _m_onClose = _onClose;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            //显示获得的积分
            long gainScore = _m_resultInfo.getGainScore();
            long scoreAddPer = _m_resultInfo.getScoreAddPer();
            ALUGUICommon.setLabelTxt(wnd.txtDinnerScore, TextTranslate.instance.getLanguage(TransKeyConst.dinner_result_score,gainScore, scoreAddPer/100));
        
            //显示获得的金币
            long coin = _m_resultInfo.getGainCoin();
            NPCommonCostItem costItem = new NPCommonCostItem(ENPItemType.CURRENCY, (long)ECurrency.DINNER_COIN, coin);
            List<NPCommonCostItem> costItems = new List<NPCommonCostItem>();
            costItems.Add(costItem);
            _m_itemContainer?.showWnd();
            _m_itemContainer?.showItemList(costItems);
            //显示宾客信息
            _m_joinItemContainer?.showWnd();
            _m_joinItemContainer?.showItemList(_m_guestList);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            _m_onClose?.Invoke();
            _m_onClose = null;
        }

        /// <summary>
        /// 点击参会者详情
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickJoinerDetail(GameObject obj)
        {
            if(_m_resultInfo == null)
                return;
            List<GDinnerGuestInfo> guestInfos = new List<GDinnerGuestInfo>();
            foreach (var joiner in _m_resultInfo.getGuestLog())
            {
                guestInfos.Add(new GDinnerGuestInfo(joiner));
            }
            GGUIWndDinnerGuestInfoList.instance.setInfo(guestInfos);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerGuestInfoList.instance, GGUIWndDinnerGuestInfoList.instance.showWnd, UINodeTagConst.C_DINNER_GUEST_LIST);
        }
    }
}