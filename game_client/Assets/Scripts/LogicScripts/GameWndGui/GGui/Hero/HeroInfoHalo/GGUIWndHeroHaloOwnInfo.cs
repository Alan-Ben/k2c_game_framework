using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴光环已提升详情
    /// </summary>
    public class GGUIWndHeroHaloOwnInfo : _ANPGGUIBasicWnd<GGUIMonoHeroHaloOwnInfo>
    {
        private static GGUIWndHeroHaloOwnInfo _g_instance;
        public static GGUIWndHeroHaloOwnInfo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroHaloOwnInfo();
                return _g_instance;
            }
        }

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //套系技能列表
        private GGUIWndHeroHaloSuitSkillContainer _m_wSuitSkillContainer;

        public GGUIWndHeroHaloOwnInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroHaloOwnInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroHaloOwnInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSuitSkillContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSuitSkillContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSuitSkillContainer?.discard();
            _m_wSuitSkillContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.monoSuitSkillContainer != null)
                _m_wSuitSkillContainer = new GGUIWndHeroHaloSuitSkillContainer(wnd.monoSuitSkillContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }
        
        //设置信息
        public void setInfo(HeroInfo _heroInfo)
        {
            if(null == _heroInfo)
                return;

            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_heroInfo == null || _m_heroInfo.heroHaloInfo == null)
                return;

            //刷新提升值
            HeroHaloLevelRefObj curHaloLevelRef = _m_heroInfo.heroHaloInfo.curHaloLevelRef;
            long addValue = 0;
            long addPer = 0;
            if (curHaloLevelRef != null && curHaloLevelRef.self_attr_prop_modifier != null)
            {
                addValue = curHaloLevelRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER);
                addPer = curHaloLevelRef.self_attr_prop_modifier.getPropValue(EBasicAttrType.POWER_PER);
            }
            ALUGUICommon.setLabelTxt(wnd.txtAddValue, TextTranslate.instance.getLanguage(TransKeyConst.hero_selfPowerAddValue_num, addValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtAddPer,
                TextTranslate.instance.getLanguage(TransKeyConst.hero_selfPowerAddValue_num,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, addPer / 100f)));

            //刷新获得效果技能列表
            List<WCGPairInt> suitSkillList = GRefdataCoreMgr.instance.getOwnSuitSkillPairListByLevel(_m_heroInfo.heroHaloInfo.haloId, _m_heroInfo.heroHaloInfo.level);
            if (_m_wSuitSkillContainer != null)
            {
                if (_m_heroInfo.heroHaloInfo.isUnlock)
                {
                    _m_wSuitSkillContainer.showWnd();
                    _m_wSuitSkillContainer.showItemList(suitSkillList, true);
                }
                else
                    _m_wSuitSkillContainer.hideWnd();
            }
        }


        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _gameObject)
        {
            if(null == _m_heroInfo)
                return;
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_HALO_OWN_INFO);
        }

        #endregion
    }
}