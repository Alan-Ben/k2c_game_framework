using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用购买点击按钮，支持现金、道具购买、免费
    /// </summary>
    public class GGUIWndCommonBuyButton : _ANPGGUIBasicSubWnd<GGUIMonoCommonBuyButton>
    {
        //道具数据
        private NPCommonCostItem _m_costItem;
        //是否售罄
        private bool _m_bIsSellOut;
        //点击按钮事件,<消耗道具，是否售罄>
        private Action<NPCommonCostItem, bool> _m_aClickButton;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;

        /// <summary>
        /// 点击按钮事件
        /// </summary>
        public Action<NPCommonCostItem, bool> onClickButton { get { return _m_aClickButton; } set { _m_aClickButton = value; } }

        public GGUIWndCommonBuyButton(GGUIMonoCommonBuyButton _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_aClickButton = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_aClickButton = null;
            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        /// <summary>
        /// 设置消耗道具列表
        /// </summary>
        /// <param name="_itemList">道具列表</param>
        /// <param name="_isSellOut">是否售罄</param>
        public void setInfoList(List<NPCommonCostItem> _itemList, bool _isSellOut = false)
        {
            if (_itemList == null || _itemList.Count == 0)
                setInfo(null, _isSellOut);
            else
                setInfo(_itemList[0], _isSellOut);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_item">道具</param>
        /// <param name="_isSellOut">是否售罄</param>
        public void setInfo(NPCommonCostItem _item, bool _isSellOut = false)
        {
            _m_costItem = _item;
            _m_bIsSellOut = _isSellOut;
            _refreshWnd();
        }

        /// <summary>
        /// 设置是否售罄
        /// </summary>
        /// <param name="_isSellOut"></param>
        public void setSellOut(bool _isSellOut)
        {
            _m_bIsSellOut = _isSellOut;
            _refreshWnd();
        }

        //刷新状态
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            bool isFree = _m_costItem == null || _m_costItem.item == null || _m_costItem.item.itemType == ENPItemType.NONE;
            bool isCash = !isFree && _m_costItem.item.itemType == ENPItemType.PAY;
            ALUGUICommon.setLabelTxt(wnd.txtVIPExp, "");

            //不是免费的设置显示内容
            if (!isFree)
            {
                if (isCash)
                {
                    //展示价格
                    ALUGUICommon.setLabelTxt(wnd.txtCashPrice, SDKMgr.instance.getLocalShowPrice(_m_costItem.subId));
                    //展示VIP经验
                    PayRefObj payRef = GRefdataCoreMgr.instance.payRefCore.getRef(_m_costItem.subId);
                    if (payRef != null && payRef.vip_exp > 0)
                        ALUGUICommon.setLabelTxt(wnd.txtVIPExp, TextTranslate.instance.getLanguage(TransKeyConst.common_VIPExp_str, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, payRef.vip_exp)));
                    else
                        ALUGUICommon.setLabelTxt(wnd.txtVIPExp, "");
                }
                else
                {
                    //展示消耗道具
                    _m_wCostItem?.showWnd();
                    _m_wCostItem?.setItem(_m_costItem);
                }
            }

            //设置状态
            if (_m_bIsSellOut)
            {
                //已售罄
                GGameCommonInfo.grayImage(wnd.sellOutGrayList);
                _setShowState(ECommonBuyButtonType.SELL_OUT);
            }
            else
            {
                //未售罄
                GGameCommonInfo.disgrayImage(wnd.sellOutGrayList);

                if (isFree)
                {
                    //免费类型
                    _setShowState(ECommonBuyButtonType.FREE);
                }
                else
                {
                    if (isCash)
                    {
                        //现金类型
                        _setShowState(ECommonBuyButtonType.CASH);
                    }
                    else
                    {
                        //道具类型
                        _setShowState(ECommonBuyButtonType.ITEM);
                    }
                }
            }
        }

        //设置显示状态
        private void _setShowState(ECommonBuyButtonType _type)
        {
            if (wnd == null || wnd.buttonShowStateList == null)
                return;

            List<GameObject> targetList = null;
            for (int i = 0; i < wnd.buttonShowStateList.Count; i++)
            {
                if(wnd.buttonShowStateList[i] == null)
                    continue;

                if (wnd.buttonShowStateList[i].type == _type)
                    targetList = wnd.buttonShowStateList[i].goShowList;
                else
                    ALUGUICommon.setGameObjEnable(wnd.buttonShowStateList[i].goShowList, false);
            }
            ALUGUICommon.setGameObjEnable(targetList, true);
        }

        //点击事件
        private void _onClick(GameObject _go)
        {
            if (wnd == null)
                return;

            //售罄提示
            if (_m_bIsSellOut && wnd.isShowSellOutTip)
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.activity_itemSellOutTip_none);

            //点击回调
            _m_aClickButton?.Invoke(_m_costItem, _m_bIsSellOut);
        }
    }
}