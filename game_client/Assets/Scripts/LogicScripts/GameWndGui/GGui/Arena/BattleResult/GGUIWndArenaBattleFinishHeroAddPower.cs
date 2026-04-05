using ALPackage;
using System;
using Common.ArenaObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场指定谈判结束伙伴获得实力弹窗
    /// </summary>
    public class GGUIWndArenaBattleFinishHeroAddPower : _ANPGGUIBasicWnd<GGUIMonoArenaBattleFinishHeroAddPower>
    {
        private static GGUIWndArenaBattleFinishHeroAddPower _g_instance;
        public static GGUIWndArenaBattleFinishHeroAddPower instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleFinishHeroAddPower();
                return _g_instance;
            }
        }

        //头像
        private NPGGuiWndTexture _m_wIconWnd;
        //头像背景
        private GGuiWndSprite _m_wIconBg;
        //关闭回调
        private Action _m_aOnClose;

        public GGUIWndArenaBattleFinishHeroAddPower() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleFinishHeroAddPower.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleFinishHeroAddPower.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconWnd?.hideWnd();
            _m_wIconBg?.hideWnd();

            _m_aOnClose?.Invoke();
            _m_aOnClose = null;
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
            _m_wIconBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconWnd?.discard();
            _m_wIconWnd = null;
            _m_wIconBg?.discard();
            _m_wIconBg = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgHeroIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgHeroIcon);

            if(wnd.imgHeroIconBg != null)
                _m_wIconBg = new GGuiWndSprite(wnd.imgHeroIconBg);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_arenaBattleInfo"></param>
        /// <param name="_finalResult"></param>
        /// <param name="_onClose"></param>
        public void setInfo(ArenaBattleInfo _arenaBattleInfo, Arena_BattleResult _finalResult, Action _onClose)
        {
            if (wnd == null || _arenaBattleInfo == null || _finalResult == null)
                return;

            _m_aOnClose = _onClose;
            ArenaSelectAttackConsumeRefObj selectAttackConsumeRef = GRefdataCoreMgr.instance.arenaSelectAttackConsumeRefCore.getRef(_finalResult.getSelectAttackItemId());

            //伙伴信息
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_arenaBattleInfo.heroId);
            if (heroInfo != null)
            {
                _m_wIconWnd?.showWnd();
                _m_wIconWnd?.setTexture(heroInfo.getIcon());

                _m_wIconBg?.showWnd();
                _m_wIconBg?.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, _arenaBattleInfo.heroId)?.hero_head_bg);
            }

            //设置增加实力
            ALUGUICommon.setLabelTxt(wnd.txtAddPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _finalResult.getAddPower()));

            //设置增加实力描述
            //实力提升={0}(身份效果)X{1}(击败伙伴数)X{2}(指名谈判)
            ALUGUICommon.setLabelTxt(wnd.txtDesc,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_battleFinishAddHeroPowerDesc_num_num_num, 
                    NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.ARENA_IDENTITY_EFFECT_VALUE), _finalResult.getHadDefeatNum(), selectAttackConsumeRef?.ratio));
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_FINAL_HERO_ADD_POWER);
        }
    }
}