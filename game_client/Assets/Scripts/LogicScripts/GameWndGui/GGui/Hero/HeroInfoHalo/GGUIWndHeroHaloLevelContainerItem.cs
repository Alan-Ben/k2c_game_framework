using ALPackage;
using Common.HeroObj;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴星辉等级列表item
    /// </summary>
    public class GGUIWndHeroHaloLevelContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroHaloLevelContainerItem>
    {
        //伙伴id
        private long _m_lHeroId;
        //光环等级数据
        private HeroHaloLevelRefObj _m_haloLevelRef;
        //最大光环等级数据
        private HeroHaloLevelRefObj _m_haloMaxLevelRef;
        //等级图标
        private NPGGuiWndTexture _m_wIcon;
        // 选中回调
        private Action<GGUIWndHeroHaloLevelContainerItem> _m_aOnSelectItem;


        /// <summary>
        /// 光环等级数据
        /// </summary>
        public HeroHaloLevelRefObj haloLevelRef { get => _m_haloLevelRef; }

        public GGUIWndHeroHaloLevelContainerItem(GGUIMonoHeroHaloLevelContainerItem _wnd, Action<GGUIWndHeroHaloLevelContainerItem> _onSelectItem) : base(_wnd)
        {
            initWnd();
            _m_aOnSelectItem = _onSelectItem;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_aOnSelectItem = null;

            _m_wIcon?.discard();
            _m_wIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClickItem, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClickItem, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_haloLevelRef"></param>
        /// <param name="_haloMaxLevelRef"></param>
        /// <param name="_heroId"></param>
        public void setInfo(HeroHaloLevelRefObj _haloLevelRef, HeroHaloLevelRefObj _haloMaxLevelRef, long _heroId)
        {
            _m_lHeroId = _heroId;
            _m_haloLevelRef = _haloLevelRef;
            _m_haloMaxLevelRef = _haloMaxLevelRef;
            wnd?.aniUpgrade?.resetAni();
            _refreshWnd();
            setSelect(false);
        }

        /// <summary>
        /// 设置选中相关显示
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _refreshLevel(_isSelect);
            _refreshState();
            _refreshRedTip();
        }

        /// <summary>
        /// 播放升级激活动画
        /// </summary>
        public void playUpgradeAni()
        {
            wnd?.aniUpgrade?.forcePlay();
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        private void _refreshWnd()
        {
            _refreshLevel(false);
            _refreshState();
            _refreshRedTip();
        }

        //刷新等级展示
        private void _refreshLevel(bool _isSelect)
        {
            if (wnd == null || _m_haloLevelRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId);
            if (heroInfo == null)
                return;
            long curLevel = heroInfo.heroHaloInfo == null ? 0 : heroInfo.heroHaloInfo.level;
            bool isUnlock = heroInfo.heroHaloInfo != null && heroInfo.heroHaloInfo.isUnlock;

            string levelStr = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, _m_haloLevelRef.level),
                _isSelect ? wnd.curSelectTextColor : wnd.notSelectTextColor);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, levelStr);

            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                if(_m_haloLevelRef.level <= curLevel && isUnlock)
                    _m_wIcon.setTexture(_m_haloLevelRef.unlock_icon);
                else
                    _m_wIcon.setTexture(_m_haloLevelRef.lock_icon);
            }
        }

        //刷新状态
        private void _refreshState()
        {
            if (wnd == null || _m_haloLevelRef == null || _m_haloMaxLevelRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId);
            if (heroInfo == null)
                return;

            long curLevel = heroInfo.heroHaloInfo == null ? 0 : heroInfo.heroHaloInfo.level;

            //设置激活显隐
            ALUGUICommon.setGameObjEnable(wnd.goActivateShowList, _m_haloLevelRef.level <= curLevel);
            ALUGUICommon.setGameObjEnable(wnd.goActivateHideList, _m_haloLevelRef.level > curLevel);

            //设置进度条显隐
            ALUGUICommon.setGameObjEnable(wnd.goProcessUp, _m_haloLevelRef.level <= curLevel);
            ALUGUICommon.setGameObjEnable(wnd.goProcessDown, _m_haloLevelRef.level < curLevel);

            //首尾等级需要多隐藏的东西
            if (_m_haloLevelRef.level == 1)
                ALUGUICommon.setGameObjEnable(wnd.goProcessUpParent, false);
            if(_m_haloMaxLevelRef.level == _m_haloLevelRef.level)
                ALUGUICommon.setGameObjEnable(wnd.goProcessDownParent, false);
        }

        //刷新红点
        private void _refreshRedTip()
        {
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_lHeroId);
            if (heroInfo == null)
                return;

            //刷新可升级红点
            bool canShow = (!heroInfo.heroHaloInfo.isUnlock && _m_haloLevelRef.level == 0) ||
                           (heroInfo.heroHaloInfo.isUnlock &&
                            heroInfo.heroHaloInfo.nextHaloLevelRef != null &&
                            GCommon.isItemEnough(heroInfo.heroHaloInfo.curHaloLevelRef.upgrade_cost, false) &&
                            _m_haloLevelRef.level == heroInfo.heroHaloInfo.nextHaloLevelRef.level);
            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip, canShow);

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
