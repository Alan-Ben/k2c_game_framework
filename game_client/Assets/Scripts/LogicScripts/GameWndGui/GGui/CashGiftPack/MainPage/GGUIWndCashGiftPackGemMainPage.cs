using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 钻石商店页面
    /// </summary>
    public class GGUIWndCashGiftPackGemMainPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoCashGiftPackGemMainPage>
    {
        //资源id
        private long _m_lUIResId;
        //VIP经验进度
        private NPGGUIWndProgress _m_wExpProgress;
        //钻石商品列表
        private GGUIWndCashGiftPackGemContainer _m_wGemContainer;

        public GGUIWndCashGiftPackGemMainPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onParamChg);
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onCurrencyChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onParamChg);
            _m_wGemContainer?.hideWnd();
            _m_wExpProgress?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wGemContainer?.resetWnd();
            _m_wExpProgress?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wGemContainer?.discard();
            _m_wGemContainer = null;
            _m_wExpProgress?.discard();
            _m_wExpProgress = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnVIPPreview, _onClickVIPPreview);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGemContainer != null)
                _m_wGemContainer = new GGUIWndCashGiftPackGemContainer(wnd.monoGemContainer);

            if(wnd.expProgress != null)
                _m_wExpProgress = new NPGGUIWndProgress(wnd.expProgress);
            ALUGUICommon.combineBtnClick(wnd.btnVIPPreview, _onClickVIPPreview);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            //刷新钻石列表
            _refreshGemList();
            //刷新VIP相关显示
            _refreshVIP();
        }

        /// <summary>
        /// 刷新钻石列表
        /// </summary>
        private void _refreshGemList()
        {
            if (wnd == null)
                return;

            //获取礼包列表
            List<GiftPackRefObj> targetGiftPackRefList = new List<GiftPackRefObj>();
            GRefdataCoreMgr.instance.giftPackGroupRefCore.dealAllRef(_groupRef =>
            {
                if (_groupRef != null && _groupRef.show_type == EGiftPackGroupShowType.GEM && _groupRef.gift_pack_id_list != null)
                {
                    for (int i = 0; i < _groupRef.gift_pack_id_list.Count; i++)
                    {
                        GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_groupRef.gift_pack_id_list[i]);
                        targetGiftPackRefList.Add(giftPackRef);
                    }
                }
            });
            targetGiftPackRefList.Sort((_a, _b) => _a.sort_id.CompareTo(_b.sort_id));

            //展示钻石列表
            _m_wGemContainer?.showWnd();
            _m_wGemContainer?.showItemList(targetGiftPackRefList);
        }

        /// <summary>
        /// 刷新VIP相关显示
        /// </summary>
        private void _refreshVIP()
        {
            long vipLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            VipRefObj curVIPLevelRef = GRefdataCoreMgr.instance.vipRefCore.getRef(vipLevel);
            VipRefObj nextVIPLevelRef = GRefdataCoreMgr.instance.vipRefCore.getRef(vipLevel + 1);
            bool isMaxLevel = nextVIPLevelRef == null;
            long curVIPExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long)ECurrency.VIP_EXP);
            ALUGUICommon.setLabelTxt(wnd.txtVIPLevel, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_vip_num, vipLevel));
            if (!isMaxLevel)
            {
                _m_wExpProgress?.showWnd();
                _m_wExpProgress?.setProgress(curVIPExp, nextVIPLevelRef.vip_exp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                ALUGUICommon.setLabelTxt(wnd.txtVIPUpgradeDesc, TextTranslate.instance.getLanguage(TransKeyConst.vip_nextLevelNeedExp_num_num, nextVIPLevelRef.vip_exp - curVIPExp, vipLevel + 1));
            }
            else
            {
                _m_wExpProgress?.showWnd();
                _m_wExpProgress?.setProgress(curVIPExp, curVIPLevelRef.vip_exp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                ALUGUICommon.setLabelTxt(wnd.txtVIPUpgradeDesc, TextTranslate.instance.getLanguage(TransKeyConst.vip_exceedMaxLevelDesc_none));
            }
        }

        //点击VIP预览
        private void _onClickVIPPreview(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndVIPLevelPreview.instance, GGUIWndVIPLevelPreview.instance.showWnd, UINodeTagConst.C_VIP_LEVEL_PREVIEW);
        }

        //资源变化
        private void _onCurrencyChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length <= 0)
                return;

            ECurrency changeCurrency = (ECurrency)_objects[0];
            if(changeCurrency == ECurrency.VIP_EXP)
                _refreshVIP();
        }

        //玩家参数变化
        private void _onParamChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            int paramIndex = (int)_objects[0];
            if ((ENPPlayerParam)paramIndex == ENPPlayerParam.VIP_LVL)
                _refreshVIP();
        }
    }
}
