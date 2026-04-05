using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴升阶成功界面
    /// </summary>
    public class GGUIWndHeroStepUpgradeSuc : _ANPGGUIBasicWnd<GGUIMonoHeroStepUpgradeSuc>
    {
        private static GGUIWndHeroStepUpgradeSuc _g_instance;
        public static GGUIWndHeroStepUpgradeSuc instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroStepUpgradeSuc();
                return _g_instance;
            }
        }

        //卡牌信息
        private GGUIWndHeroCommonCardItem _m_wCardItem;

        public GGUIWndHeroStepUpgradeSuc() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroStepUpgradeSuc.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroStepUpgradeSuc.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCardItem?.discard();
            _m_wCardItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCardItem != null)
                _m_wCardItem = new GGUIWndHeroCommonCardItem(wnd.monoCardItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            if (wnd == null || _heroInfo == null)
                return;

            if (_m_wCardItem != null)
            {
                _m_wCardItem.showWnd();
                _m_wCardItem.setInfo(_heroInfo);
            }

            //上个等级上限
            HeroStepRefObj lastStepRef = GRefdataCoreMgr.instance.heroStepRefCore.getRef(_heroInfo.curStep - 1);
            ALUGUICommon.setLabelTxt(wnd.txtLastLevelLimit, lastStepRef != null ? lastStepRef.level_limit:0);
            //当前等级上限
            ALUGUICommon.setLabelTxt(wnd.txtCurLevelLimit, _heroInfo.curHeroStepRef != null ? _heroInfo.curHeroStepRef.level_limit:0);
            //资质
            long curTalent = _heroInfo.getTotalTalent();
            ALUGUICommon.setLabelTxt(wnd.txtCurTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, curTalent));

            curTalent += HeroCommon.calStepTalentSkillDifferentValueWithLevelDiffer(_heroInfo, -1);
            ALUGUICommon.setLabelTxt(wnd.txtLastTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, curTalent));

            //播放升阶成功语音
            HeroVoiceMgr.instance.playVoice(_heroInfo.id, EHeroVoiceType.PROMOTION);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_STEP_UPGRADE_SUC);
        }
    }
}