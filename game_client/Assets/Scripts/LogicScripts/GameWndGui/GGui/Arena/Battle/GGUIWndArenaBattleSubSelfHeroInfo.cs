using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗自己伙伴信息附加窗口
    /// </summary>
    public class GGUIWndArenaBattleSubSelfHeroInfo : _ATALBasicUISubWnd<GGUIMonoArenaBattleSubSelfHeroInfo>
    {
        //战斗信息
        private ArenaBattleInfo _m_battleInfo;
        //伙伴形象
        private NPGGuiWndTexture _m_wHeroTexture;

        public GGUIWndArenaBattleSubSelfHeroInfo(GGUIMonoArenaBattleSubSelfHeroInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroTexture?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroTexture?.discardShowTexture();
        }

        protected override void _onDiscard()
        {
            _m_wHeroTexture?.discard();
            _m_wHeroTexture = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnPowerDetail, _onClickPowerDetail);//点击查看实力详情
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgHero != null)
                _m_wHeroTexture = new NPGGuiWndTexture(wnd.imgHero);

            ALUGUICommon.combineBtnClick(wnd.btnPowerDetail, _onClickPowerDetail);//点击查看实力详情
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo()
        {
            _m_battleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
            _refreshWnd();
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null || _m_battleInfo == null) 
                return;

            //伙伴信息
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_battleInfo.heroId);
            //总增益万分比
            long totalBuffAddPer = _m_battleInfo.getTotalBuffAddPer();
            //总血量
            long totalHp = _m_battleInfo.getTotalPower();

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtPlayerName, NPPlayer.instance.playerInfo.PlayerName);

            //设置伙伴形象
            if (_m_wHeroTexture != null)
            {
                _m_wHeroTexture.showWnd();
                _m_wHeroTexture.setTexture(heroInfo?.getCardImage());
            }

            //设置实力
            ALUGUICommon.setLabelTxt(wnd.txtHeroPower, totalHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            //设置临时加成百分比
            ALUGUICommon.setLabelTxt(wnd.txtBuffAddPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, totalBuffAddPer / 100f));

            //设置当前血量值
            long curHp = _m_battleInfo.getCurLeftPower();
            if (curHp < 0)
                curHp = 0;
            if (totalHp == 0)
                ALUGUICommon.setSliderScale(wnd.sldBlood, 0);
            else
                ALUGUICommon.setSliderScale(wnd.sldBlood, curHp * 1.0f / totalHp);
            ALUGUICommon.setLabelTxt(wnd.txtHP,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_curBlood_num_num,
                    curHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), totalHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }

        //点击实力详情
        private void _onClickPowerDetail(GameObject _go)
        {
            if (wnd == null || _m_battleInfo == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(5219,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_battleHeroTemporaryAddPowerPer_num, _m_battleInfo.getTotalBuffAddPer()/100f),
                (RectTransform)_go.transform, wnd.powerDetailIntervalX, wnd.powerDetailIntervalY));
        }
    }
}
