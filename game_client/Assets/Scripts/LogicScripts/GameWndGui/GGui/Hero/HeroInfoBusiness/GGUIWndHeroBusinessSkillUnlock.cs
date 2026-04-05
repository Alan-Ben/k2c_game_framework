using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴经营技能自动解锁弹窗
    /// </summary>
    public class GGUIWndHeroBusinessSkillUnlock : _ANPGGUIBasicWnd<GGUIMonoHeroBusinessSkillUnlock>
    {
        private static GGUIWndHeroBusinessSkillUnlock _g_instance;
        public static GGUIWndHeroBusinessSkillUnlock instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroBusinessSkillUnlock();
                return _g_instance;
            }
        }

        //伙伴id
        private long _m_lHeroId;
        //经营技能id
        private long _m_lBusinessSkillId;
        //伙伴头像
        private NPGGuiWndTexture _m_wHeroIcon;
        //伙伴品质背景
        private GGuiWndSprite _m_wQualityBg;

        public GGUIWndHeroBusinessSkillUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroBusinessSkillUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroBusinessSkillUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroIcon?.hideWnd();
            _m_wQualityBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroIcon?.discardTexture();
            _m_wQualityBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;
            _m_wQualityBg?.discard();
            _m_wQualityBg = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgHeroIcon != null)
                _m_wHeroIcon = new NPGGuiWndTexture(wnd.imgHeroIcon);

            if (wnd.imgIconBg != null)
                _m_wQualityBg = new GGuiWndSprite(wnd.imgIconBg);

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_businessSkillId"></param>
        public void setInfo(long _heroId, long _businessSkillId)
        {
            _m_lHeroId = _heroId;
            _m_lBusinessSkillId = _businessSkillId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId);
            if (heroInfo == null || heroInfo.heroRefObj == null)
                return;

            //设置头像
            if (_m_wHeroIcon != null)
            {
                _m_wHeroIcon.showWnd();
                _m_wHeroIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, heroInfo.curSkinId));
            }

            //设置背景图
            if (_m_wQualityBg != null)
            {
                NPQualityExtRefObj qualityExRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, heroInfo.id);
                _m_wQualityBg.showWnd();
                _m_wQualityBg.setTexture(qualityExRef?.hero_head_bg);
            }

            //设置解锁描述
            WCGPairInt tempPairInt = null;
            if (heroInfo.heroRefObj.extra_business_skill_id_list != null)
            {
                for (int i = 0; i < heroInfo.heroRefObj.extra_business_skill_id_list.Count; i++)
                {
                    if (heroInfo.heroRefObj.extra_business_skill_id_list[i].second() == _m_lBusinessSkillId)
                    {
                        tempPairInt = heroInfo.heroRefObj.extra_business_skill_id_list[i];
                        break;
                    }
                }
            }
            ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_businessSkillAutoUnlockDesc_name_level, 
                heroInfo.heroRefObj.transName,
                tempPairInt?.first()));

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtHeroName, GCommon.getItemName(ENPItemType.HERO, heroInfo.id));

            //设置技能描述
            HeroBusinessSkillRefObj businessSkillRef = GRefdataCoreMgr.instance.heroBusinessSkillRefCore.getRef(_m_lBusinessSkillId);
            if (businessSkillRef != null && businessSkillRef.bonus_prop_modifier != null)
                ALUGUICommon.setLabelTxt(wnd.txtSkillDesc, TextTranslate.instance.getLanguage(businessSkillRef.desc, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, businessSkillRef.bonus_prop_modifier.getPropValue(EBonusPropertyType.BUILDING_PROFIT_ADD_PER)/100f)));
        }

        #region 点击事件

        //点击确定
        private void _onClickConfirm(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_BUSINESS_SKILL_UNLOCK);
        }

        //点击查看
        private void _onClickGoTo(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_BUSINESS_SKILL_UNLOCK);
            //通知界面页签点击引导
            WinMsg.SendMsg(WinMsgType.HERO_INFO_GUIDE_TAB, EHeroInfoTabType.BUSINESS);
        }

        #endregion
    }
}
