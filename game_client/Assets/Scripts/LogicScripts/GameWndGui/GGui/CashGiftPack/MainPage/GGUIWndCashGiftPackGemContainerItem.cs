using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 钻石商店列表item
    /// </summary>
    public class GGUIWndCashGiftPackGemContainerItem : _ATALBasicUISubWnd<GGUIMonoCashGiftPackGemContainerItem>
    {
        //配置
        private GiftPackRefObj _m_giftPackRef;
        //图标
        private NPGGuiWndTexture _m_wIcon;

        public GGUIWndCashGiftPackGemContainerItem(GGUIMonoCashGiftPackGemContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setInfo(GiftPackRefObj _refObj)
        {
            if (_refObj == null)
                return;

            _m_giftPackRef = _refObj;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_giftPackRef == null)
                return;

            //是否首充
            bool isFirst = NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(_m_giftPackRef.id) == 0;
            //获得的物品
            NPCommonCostItem gainItem = _m_giftPackRef.item_list != null && _m_giftPackRef.item_list.Count > 0 ? _m_giftPackRef.item_list[0] : null;

            //显示图标
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_giftPackRef.icon);

            //获得的钻石数量
            ALUGUICommon.setLabelTxt(wnd.txtGainNum, gainItem != null ? gainItem.count : 0);

            //额外获得数量
            GiftPackExtraGainRefObj giftPackExtraRef = GRefdataCoreMgr.instance.getGiftPackExtraGainRef(
                _m_giftPackRef.id, NPPlayer.instance.giftPackComp.getGiftPackHadBuyCount(_m_giftPackRef.id) + 1);
            NPCommonCostItem extraItem = giftPackExtraRef != null && giftPackExtraRef.extra_gain_list != null && giftPackExtraRef.extra_gain_list.Count > 0 ? giftPackExtraRef.extra_gain_list[0] : null;
            long extraNum = extraItem != null ? extraItem.count : 0;
            ALUGUICommon.setLabelTxt(wnd.txtAddNum, TextTranslate.instance.getLanguage(TransKeyConst.giftPack_extraGemCount_num, extraNum));
            ALUGUICommon.setGameObjEnable(wnd.goNoAddNumShowList, extraNum <= 0);
            ALUGUICommon.setGameObjEnable(wnd.goNoAddNumHideList, extraNum > 0);

            //VIP经验及价格
            if (_m_giftPackRef.cost_list != null &&
                _m_giftPackRef.cost_list.Count > 0 &&
                _m_giftPackRef.cost_list[0] != null &&
                _m_giftPackRef.cost_list[0].getItemType() == ENPItemType.PAY)
            {
                //VIP经验
                PayRefObj payRef = GRefdataCoreMgr.instance.payRefCore.getRef(_m_giftPackRef.cost_list[0].subId);
                if (payRef != null && payRef.vip_exp > 0)
                    ALUGUICommon.setLabelTxt(wnd.txtVIPExp, TextTranslate.instance.getLanguage(TransKeyConst.common_VIPExp_str, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, payRef.vip_exp)));
                else
                    ALUGUICommon.setLabelTxt(wnd.txtVIPExp, "");

                //价格
                ALUGUICommon.setLabelTxt(wnd.txtPrice, SDKMgr.instance.getLocalShowPrice(_m_giftPackRef.cost_list[0].subId));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtVIPExp, "");
                ALUGUICommon.setLabelTxt(wnd.txtPrice, "");
            }

            //首充显隐
            ALUGUICommon.setGameObjEnable(wnd.goFirstHideList, !isFirst);
            ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, isFirst);
        }

        //点击item
        private void _onClickItem(GameObject _go)
        {
            GCommon.reqPay(_m_giftPackRef, null, null);
        }

        //充值记录变更
        private void _onGiftPackChg(params object[] _objects)
        {
            _refreshWnd();
        }
    }
}