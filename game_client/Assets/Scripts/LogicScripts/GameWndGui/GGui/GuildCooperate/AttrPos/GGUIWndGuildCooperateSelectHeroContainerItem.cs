using ALPackage;
using CommonEnum;
using System;
using UnityEngine;
using static GOE.PrimitiveExtension;

namespace GOE
{
    /// <summary>
    /// 公会协作选择伙伴列表Item
    /// </summary>
    public class GGUIWndGuildCooperateSelectHeroContainerItem : _ATALBasicUISubWnd<GGUIMonoGuildCooperateSelectHeroContainerItem>
    {
        // 伙伴信息
        private HeroInfo _m_heroInfo;
        // 属性据点类型
        private ESpecAttrType _m_ePosAttrType;
        // 选中伙伴回调
        private Action<GGUIWndGuildCooperateSelectHeroContainerItem> _m_aOnSelectItem;
        // 伙伴卡牌
        private GGUIWndHeroCommonCardItem _m_wHeroCard;

        /// <summary>
        /// 伙伴信息
        /// </summary>
        public HeroInfo heroInfo { get { return _m_heroInfo; } }

        public GGUIWndGuildCooperateSelectHeroContainerItem(GGUIMonoGuildCooperateSelectHeroContainerItem _mono, Action<GGUIWndGuildCooperateSelectHeroContainerItem> _onSelectItem) : base(_mono)
        {
            initWnd();
            _m_aOnSelectItem = _onSelectItem;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroCard?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroCard?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_aOnSelectItem = null;
            _m_wHeroCard?.discard();
            _m_wHeroCard = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClickItem, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroCard != null)
                _m_wHeroCard = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);

            ALUGUICommon.combineBtnClick(wnd.btnClickItem, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(HeroInfo _info, ESpecAttrType _posAttrType)
        {
            _m_heroInfo = _info;
            _m_ePosAttrType = _posAttrType;
            refreshWnd();
            setSelect(false);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            wnd.selectShow?.setShowData(_isSelect ? ECommonSelectState.SELECTED : ECommonSelectState.UNSELECTED);
        }

        /// <summary>
        /// 设置派遣
        /// </summary>
        public void playDispath(Action _onDone)
        {
            if (wnd == null || wnd.aniDispatch == null)
            {
                _onDone?.Invoke();
                return;
            }
            wnd.aniDispatch.forcePlay(_onDone);
        }

        /// <summary>
        /// 设置恢复
        /// </summary>
        public void playRecover(Action _onDone)
        {
            if (wnd == null || wnd.aniRecover == null)
            {
                _onDone?.Invoke();
                return;
            }
            wnd.aniRecover.forcePlay();
        }

        //刷新窗口
        public void refreshWnd()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            // 伙伴卡牌
            _m_wHeroCard?.showWnd();
            _m_wHeroCard?.setInfo(_m_heroInfo);

            // 建设值
            float addPercent = 0;
            if(_m_heroInfo.specAttrType == _m_ePosAttrType)
                addPercent = GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_same_property_add_per / 10000f;
            long constructionValue = (long)Math.Floor(_m_heroInfo.power * (1 + addPercent));
            ALUGUICommon.setLabelTxt(wnd.txtTotalConstructionValue, TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_heroConstructionValue_num, constructionValue.ToLargeString(ELargeStringType.DEFAULT)));

            // 恢复描述
            long leftCount = NPPlayer.instance.fixedCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_time_reset_limit_fix_cd_id);
            long limitCount = NPPlayer.instance.fixedCdComp.getLimitCount(GRefdataCoreMgr.instance.npGeneral.guild_cooperate_dispatch_time_reset_limit_fix_cd_id);
            if (leftCount > 0)
                //道具恢复：{0}/{1}
                ALUGUICommon.setLabelTxt(wnd.txtRecoverDesc, TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_useItemRecoverHeroLeftCount_num_num, leftCount, limitCount));
            else
                //今日无法使用道具恢复
                ALUGUICommon.setLabelTxt(wnd.txtRecoverDesc, TextTranslate.instance.getLanguage(TransKeyConst.guildCooperate_canNotUseItemRecoverHero_none));

            // 显示状态
            bool canUse = NPPlayer.instance.guildCooperateComp.isHeroCanUse(_m_heroInfo.id);
            wnd.heroStateShow?.setShowData(canUse ? EGuildCooperateHeroShowState.CAN_DISPATCH : EGuildCooperateHeroShowState.CAN_NOT_DISPATCH);
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickItem(GameObject _go)
        {
            _m_aOnSelectItem?.Invoke(this);
        }
    }
}