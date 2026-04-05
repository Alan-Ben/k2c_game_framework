using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤item
    /// </summary>
    public class GGUIWndHeroSkinContainerItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoHeroSkinContainerItem, GGUIWndHeroSkinContainerItem>
    {
        //皮肤图标
        private NPGGuiWndTexture _m_wIcon;
        //皮肤配置
        private HeroSkinRefObj _m_skinRef;
        //星级
        private GGUIWndHeroCommonStar _m_wStar;

        /// <summary>
        /// 皮肤配置
        /// </summary>
        public HeroSkinRefObj skinRef { get { return _m_skinRef; } }

        public GGUIWndHeroSkinContainerItem(GGUIMonoHeroSkinContainerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_SKIN_CHG, _onSkinChg);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_CUR_SKIN_ID_CHG, _onSkinChg);
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_SKIN_CHG, _onSkinChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_CUR_SKIN_ID_CHG, _onSkinChg);
            _m_wIcon?.hideWnd();
            _m_wStar?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_wIcon?.discardTexture();
            _m_wStar?.resetWnd();
        }

        protected override void _onDiscardEx()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wStar?.discard();
            _m_wStar = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroSkinRef"></param>
        public void setInfo(HeroSkinRefObj _heroSkinRef)
        {
            if (_heroSkinRef == null)
                return;

            _m_skinRef = _heroSkinRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_skinRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_skinRef.hero_id);

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, _m_skinRef.id));
            }

            //是否是当前穿戴中
            bool isCurWear = heroInfo != null && heroInfo.curSkinId == _m_skinRef.id;
            ALUGUICommon.setGameObjEnable(wnd.goCurWear, isCurWear);

            //星级
            NPQualityExtRefObj qualityExtRef = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) GCommon.getItemQuality(ENPItemType.HERO_SKIN, _m_skinRef.id));
            if (_m_wStar != null)
            {
                _m_wStar.showWnd();
                _m_wStar.setInfo(qualityExtRef != null ? qualityExtRef.hero_skin_star : 0);
            }

            //是否默认
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_skinRef.hero_id);
            bool isDefault = heroRef != null && heroRef.default_skin_id == _m_skinRef.id;
            ALUGUICommon.setGameObjEnable(wnd.goDefaultHideList, !isDefault);
            ALUGUICommon.setGameObjEnable(wnd.goDefaultShowList, isDefault);

            //是否解锁
            HeroSkinInfo skinInfo = heroInfo != null ? heroInfo.heroSkinInfoMgr.getSkinInfo(_m_skinRef.id) : null;
            bool isUnlock = isDefault || skinInfo != null;
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);
            if(isUnlock)
                GGameCommonInfo.disgrayImage(wnd.goLockGrayList);
            else
                GGameCommonInfo.grayImage(wnd.goLockGrayList);

            //设置等级
            if (skinInfo != null && !isDefault)
                ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, skinInfo.level));
            else
                ALUGUICommon.setLabelTxt(wnd.txtLevel, "");
        }

        //皮肤数据更新
        private void _onSkinChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            HeroInfo heroInfo = _objects[0] as HeroInfo;

            if (heroInfo == null || _m_skinRef == null || _m_skinRef.hero_id != heroInfo.id)
                return;

            _refreshWnd();
        }
    }
}
