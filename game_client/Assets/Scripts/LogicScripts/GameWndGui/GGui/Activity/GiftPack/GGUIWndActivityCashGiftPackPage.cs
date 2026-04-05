using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面现金页面
    /// </summary>
    public class GGUIWndActivityCashGiftPackPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoActivityCashGiftPackPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //现金礼包组id
        private long _m_lCashGiftPackGroupId;
        //现金礼包列表
        private GGUIWndCashGiftPackGroupGoodsContainer _m_wCrystalGiftPackContainer;
        //横幅图片
        private NPGGuiWndTexture _m_wBanner;

        public GGUIWndActivityCashGiftPackPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
            _m_wCrystalGiftPackContainer?.hideWnd();
            _m_wBanner?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wCrystalGiftPackContainer?.resetWnd();
            _m_wBanner?.discardTexture();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCrystalGiftPackContainer?.discard();
            _m_wCrystalGiftPackContainer = null;

            _m_wBanner?.discard();
            _m_wBanner = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoCrystalGiftPackContainer != null)
                _m_wCrystalGiftPackContainer = new GGUIWndCashGiftPackGroupGoodsContainer(wnd.monoCrystalGiftPackContainer);

            if (wnd.imgBanner != null)
                _m_wBanner = new NPGGuiWndTexture(wnd.imgBanner);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_ABaseActivityInfo _activityInfo, long _cashGiftPackGroupId)
        {
            _m_activityInfo = _activityInfo;
            _m_lCashGiftPackGroupId = _cashGiftPackGroupId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_activityInfo == null)
                return;

            GiftPackGroupRefObj giftPackGroupRef = GRefdataCoreMgr.instance.giftPackGroupRefCore.getRef(_m_lCashGiftPackGroupId);
            if (giftPackGroupRef == null)
                return;

            //名称、刷新描述
            ALUGUICommon.setLabelTxt(wnd.txtGiftPackName, TextTranslate.instance.getLanguage(giftPackGroupRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtRefreshDesc, TextTranslate.instance.getLanguage(giftPackGroupRef.refresh_desc));

            //显示横幅图片
            _m_wBanner?.showWnd();
            _m_wBanner?.setTexture(giftPackGroupRef.tex_banner);

            //显示礼包列表
            _m_wCrystalGiftPackContainer?.showWnd();
            _m_wCrystalGiftPackContainer?.showItemList(giftPackGroupRef.gift_pack_id_list);
        }

        //礼包变化
        private void _onGiftPackChg(params object[] _objects)
        {
            //刷新窗口
            _refreshWnd();
        }
    }
}