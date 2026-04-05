using ALPackage;
using Common.HeroObj;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴资质列表item
    /// </summary>
    public class GGUIWndHeroTalentSkillContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroTalentSkillContainerItem>
    {
        //资质图标
        private NPGGuiWndTexture _m_wIconWnd;
        //伙伴id
        private long _m_heroId;
        //资质技能配置
        private HeroTalentSkillRefObj _m_talentSkillRef;
        //选中item事件
        private Action<GGUIWndHeroTalentSkillContainerItem> _m_aOnSelectItem;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;


        /// <summary>
        /// 资质技能配置
        /// </summary>
        public HeroTalentSkillRefObj talentSkillRef { get { return _m_talentSkillRef; } }
        /// <summary>
        /// 选中item事件
        /// </summary>
        public Action<GGUIWndHeroTalentSkillContainerItem> onSelectItem { get { return _m_aOnSelectItem; } set { _m_aOnSelectItem = value; } }

        public GGUIWndHeroTalentSkillContainerItem(GGUIMonoHeroTalentSkillContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconWnd?.hideWnd();
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickItem);
            ALUGUICommon.uncombineBtnClick(wnd.btnClickDetail, _onClickDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.texIcon);

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickItem);
            ALUGUICommon.combineBtnClick(wnd.btnClickDetail, _onClickDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_id"></param>
        public void setInfo(long _heroId, long _id)
        {
            if (wnd == null)
                return;

            _m_heroId = _heroId;
            _m_talentSkillRef = GRefdataCoreMgr.instance.heroTalentSkillRefCore.getRef(_id);
            //刷新界面
            refreshWnd();
            //默认设置不选中
            setSelect(false);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (_m_talentSkillRef == null)
                return;

            //重置一下动画
            wnd?.aniUnlock?.resetAni();

            //设置图标
            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.showWnd();
                _m_wIconWnd.setTexture(_m_talentSkillRef.icon);
            }

            //是否解锁
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_heroId);
            bool isUnlock = heroInfo != null && heroInfo.heroTalentSkillInfoMgr.isTalentSkillUnlock(_m_talentSkillRef.id);
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            if (isUnlock)
                GGameCommonInfo.disgrayImage(wnd.lockGrayList);
            else
                GGameCommonInfo.grayImage(wnd.lockGrayList);

            //刷新升级红点
            _refreshUpgradeRedTip();
        }

        //刷新升级红点
        private void _refreshUpgradeRedTip()
        {
            if (wnd == null || _m_talentSkillRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_heroId);
            bool needShow = false;
            if (heroInfo != null)
            {
                HeroTalentSkillInfo talentSkillInfo = heroInfo.heroTalentSkillInfoMgr.getTalentSkillInfo(_m_talentSkillRef.id);
                needShow = talentSkillInfo != null && talentSkillInfo.canUpgrade();
            }
            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip, needShow);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public void playSfx()
        {
            //播放特效
            if (_m_lSfxObjList == null)
                _m_lSfxObjList = new List<CommonUISfxObj>();

            if (wnd != null && wnd.upgradeSfxId > 0 && wnd.upgradeSfxParent != null)
            {
                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxId, wnd.upgradeSfxParent);
                _m_lSfxObjList.Add(sfxObj);
            }

            //刷新红点
            _refreshUpgradeRedTip();
        }

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        public void playUnlockAni()
        {
            wnd?.aniUnlock?.forcePlay();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            //刷新红点
            _refreshUpgradeRedTip();
        }

        //点击按钮
        private void _onClickItem(GameObject _go)
        {
            _m_aOnSelectItem?.Invoke(this);
        }

        //点击详情按钮
        private void _onClickDetail(GameObject _go)
        {
            if (_m_talentSkillRef == null)
                return;

            QueueMgr.instance.AddNode(new GNodeHeroTalentSkillDetailToolTip(_m_heroId, _m_talentSkillRef.id, rectTransform, 0, 0));
        }
    }
}