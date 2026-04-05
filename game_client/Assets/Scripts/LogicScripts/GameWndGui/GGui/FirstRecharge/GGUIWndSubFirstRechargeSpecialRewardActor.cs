using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 首充特殊奖励形象展示
    /// </summary>
    public class GGUIWndSubFirstRechargeSpecialRewardActor : _ATALBasicUISubWnd<GGUIMonoSubFirstRechargeSpecialRewardActor>
    {
        //形象展示
        private NPGGUIWndCommonShowCase _m_wShowCase;
        //首充天数配置
        private FirstRechargeDayRefObj _m_firstRechargeDayRef;
        //形象
        private NPGGuiWndTexture _m_wActor;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;

        public GGUIWndSubFirstRechargeSpecialRewardActor(GGUIMonoSubFirstRechargeSpecialRewardActor _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _pushBackQualityGo();
            _m_wShowCase?.hideWnd();
            _m_wActor?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wShowCase?.resetWnd();
            _m_wActor?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wShowCase?.discard();
            _m_wShowCase = null;
            _m_wActor?.discard();
            _m_wActor = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreview);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShowCase != null)
                _m_wShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCase);

            if(wnd.imgActor != null)
                _m_wActor = new NPGGuiWndTexture(wnd.imgActor);

            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreview);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(FirstRechargeDayRefObj _firstRechargeDay)
        {
            _m_firstRechargeDayRef = _firstRechargeDay;
            _refreshWnd();
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null || _m_firstRechargeDayRef == null)
                return;

            NPCommonCostItem specialItem = _m_firstRechargeDayRef.special_item;
            if (specialItem == null)
                return;

            //提示文本
            ALUGUICommon.setLabelTxt(wnd.txtGainTip, TextTranslate.instance.getLanguage(_m_firstRechargeDayRef.special_item_gain_tip));

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //形象 名称
            NPGGoIndex tdShow = null;
            NPGTextureIndex cardImage = null;
            switch (specialItem.getItemType())
            {
                case ENPItemType.HERO:
                    HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(specialItem.subId);
                    if (heroRef != null)
                    {
                        tdShow = heroRef.td_show;
                        cardImage = heroRef.card_image;
                        ALUGUICommon.setLabelTxt(wnd.txtName, heroRef.transName);
                    }
                    break;
                case ENPItemType.CONSORT:
                    GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(specialItem.subId);
                    if (consortRef != null)
                    {
                        tdShow = consortRef.td_show;
                        cardImage = consortRef.card_image;
                        ALUGUICommon.setLabelTxt(wnd.txtName, consortRef.transName);
                    }
                    break;
            }
            _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[1];
            showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(tdShow), 0);
            _m_wShowCase?.showWnd(showCaseUnitInfoObjList);
            _m_wActor?.showWnd();
            _m_wActor?.setTexture(cardImage);

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHeroShowList, specialItem.getItemType() == ENPItemType.HERO);
            ALUGUICommon.setGameObjEnable(wnd.goConsortShowList, specialItem.getItemType() == ENPItemType.CONSORT);
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_firstRechargeDayRef == null)
                return;

            NPCommonCostItem specialItem = _m_firstRechargeDayRef.special_item;
            if (specialItem == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(specialItem.getItemType(), specialItem.subId);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityIconParent == null)
                        return;
            
                    _go.transform.SetParent(wnd.goQualityIconParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }

        //点击预览
        private void _onClickPreview(GameObject _go)
        {
            if (_m_firstRechargeDayRef == null)
                return;

            NPCommonCostItem specialItem = _m_firstRechargeDayRef.special_item;
            if (specialItem == null)
                return;

            switch (specialItem.getItemType())
            {
                case ENPItemType.HERO:
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(specialItem.subId);
                    HeroCardShowInfo showInfo = new HeroCardShowInfo(heroInfo, GRefdataCoreMgr.instance.heroRefCore.getRef(specialItem.subId));
                    if (heroInfo == null)
                        QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
                    else
                        QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null));
                    break;
                case ENPItemType.CONSORT:
                    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(specialItem.subId);
                    if (consortInfo == null)
                        QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(specialItem.subId)));
                    else
                        GNodeUnLockConsortDetail.addConsortNode(new List<GGottenConsortInfo>() { consortInfo }, consortInfo.consortId);
                    break;
            }
        }
    }
}
