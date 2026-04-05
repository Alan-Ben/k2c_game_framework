using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 现金礼包组商品列表item
    /// </summary>
    public class GGUIWndCashGiftPackGroupGoodsContainerItem : _ATALBasicUISubWnd<GGUIMonoCashGiftPackPageGoodsContainerItem>
    {
        //礼包信息
        private GiftPackRefObj _m_giftPackRef;
        //奖励道具列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;
        //特殊道具
        private NPGGUIWndCommonItem _m_wSpecialItem;
        //通用购买按钮
        private GGUIWndCommonBuyButton _m_wBuyButton;

        public GGUIWndCashGiftPackGroupGoodsContainerItem(GGUIMonoCashGiftPackPageGoodsContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItemContainer?.hideWnd();
            _m_wSpecialItem?.hideWnd();
            _m_wBuyButton?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
            _m_wSpecialItem?.resetWnd();
            _m_wBuyButton?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            _m_wSpecialItem?.discard();
            _m_wSpecialItem = null;

            _m_wBuyButton?.discard();
            _m_wBuyButton = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            if(wnd.monoSpecialItem != null)
                _m_wSpecialItem = new NPGGUIWndCommonItem(wnd.monoSpecialItem);

            if (wnd.monoBuyButton != null)
            {
                _m_wBuyButton = new GGUIWndCommonBuyButton(wnd.monoBuyButton);
                _m_wBuyButton.onClickButton += _onClickBuyButton;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _giftPackId)
        {
            _m_giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_giftPackId);
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_giftPackRef == null)
                return;

            //是否免费
            bool isFree = _m_giftPackRef.cost_list == null || _m_giftPackRef.cost_list.Count == 0;

            //礼包名称
            ALUGUICommon.setLabelTxt(wnd.txtGiftPackName, TextTranslate.instance.getLanguage(_m_giftPackRef.name, _m_giftPackRef.name_args));
            //礼包价值百分比
            ALUGUICommon.setLabelTxt(wnd.txtProfitPer, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_giftPackRef.profit_per/100));
            ALUGUICommon.setGameObjEnable(wnd.goNoProfitPerHideList, _m_giftPackRef.profit_per > 0);
            //剩余限购次数
            long leftLimitTimes = NPPlayer.instance.giftPackComp.getGiftPackLeftCount(_m_giftPackRef.id);
            bool isSellOut = leftLimitTimes <= 0 && !_m_giftPackRef.isNotLimit;
            if (_m_giftPackRef.isNotLimit)
            {
                //如果没有限购次数，则不显示限购次数文本
                ALUGUICommon.setGameObjEnable(wnd.txtLimit, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.txtLimit, true);
                ALUGUICommon.setLabelTxt(wnd.txtLimit, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, leftLimitTimes, _m_giftPackRef.buy_limit_count));
            }
            //购买按钮
            _m_wBuyButton?.showWnd();
            _m_wBuyButton?.setInfoList(_m_giftPackRef.cost_list, isSellOut);
            //可购买状态显隐
            ALUGUICommon.setGameObjEnable(wnd.goCanBuyHideList, isSellOut);
            ALUGUICommon.setGameObjEnable(wnd.goCanBuyShowList, !isSellOut);

            //是否免费显隐
            ALUGUICommon.setGameObjEnable(wnd.goFreeShowList, isFree);
            ALUGUICommon.setGameObjEnable(wnd.goFreeHideList, !isFree);
            if (isFree)
            {
                _m_wSpecialItem?.hideWnd();
                _m_wItemContainer?.showWnd();
                _m_wItemContainer?.showItemList(_m_giftPackRef.item_list);
            }
            else
            {
                if (_m_giftPackRef.item_list == null || _m_giftPackRef.item_list.Count == 0)
                {
                    _m_wSpecialItem?.hideWnd();
                    _m_wItemContainer?.hideWnd();
                }
                else
                {
                    NPCommonCostItem specialItem = _m_giftPackRef.specialShowRewardItem;
                    List<NPCommonCostItem> itemList = _m_giftPackRef.normalShowRewardItemList;

                    //展示特殊道具
                    _m_wSpecialItem?.showWnd();
                    _m_wSpecialItem?.setItem(specialItem);

                    //展示奖励列表
                    _m_wItemContainer?.showWnd();
                    _m_wItemContainer?.showItemList(itemList);
                }
            }
        }

        //点击购买按钮
        private void _onClickBuyButton(NPCommonCostItem _costItem, bool _isSellOut)
        {
            if (_m_giftPackRef == null || _isSellOut)
                return;

            if (_costItem == null || !_costItem.IsValid)
            {
                //免费购买或道具购买
                NPPlayer.instance.giftPackComp.reqBuyGiftPack(_m_giftPackRef.id);
            }
            else if(_costItem.getItemType() != ENPItemType.PAY)
            {
                if (!GCommon.isItemEnough(_costItem, true))
                    return;

                //道具购买确认框
                GCommon.showCommonBuyItemConfirm(_costItem, (_count) =>
                {
                    NPPlayer.instance.giftPackComp.reqBuyGiftPack(_m_giftPackRef.id);
                }, null, 1, TextTranslate.instance.getLanguage(_m_giftPackRef.name, _m_giftPackRef.name_args));
            }
            else
            {
                //支付流程
                GCommon.reqPay(_m_giftPackRef, _refreshWnd, null);
            }
        }
    }
}