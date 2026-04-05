using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// VIP详情加载页面形象
    /// </summary>
    public class GGUIWndSubVIPDetailActor : _ATALBasicUISubWnd<GGUIMonoSubVIPDetailActor>
    {
        //形象展示
        private NPGGUIWndCommonShowCase _m_wShowCase;
        //特殊item
        private NPCommonItem _m_commonItem;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;

        public GGUIWndSubVIPDetailActor(GGUIMonoSubVIPDetailActor _wnd) : base(_wnd)
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
        }

        protected override void _onReset()
        {
            _m_wShowCase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wShowCase?.discard();
            _m_wShowCase = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreview);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShowCase != null)
            {
                _m_wShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCase);
                // _m_wShowCase.onClickItem += _onClickActor;
            }

            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreview);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(NPCommonItem _item)
        {
            _m_commonItem = _item;
            _refreshWnd();
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null || _m_commonItem == null)
                return;

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //形象 名称 称号
            NPGGoIndex tdShow = null;
            NPGGoIndex bgIndex = null;
            switch (_m_commonItem.itemType)
            {
                case ENPItemType.HERO:
                    HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_commonItem.itemId);
                    if (heroRef != null)
                    {
                        tdShow = heroRef.td_show;
                        bgIndex = heroRef.td_bg_index;
                        ALUGUICommon.setLabelTxt(wnd.txtName, heroRef.transName);
                        ALUGUICommon.setLabelTxt(wnd.txtTitle, GCommon.getItemName(ENPItemType.HERO_SKIN, heroRef.default_skin_id));
                    }
                    break;
                case ENPItemType.CONSORT:
                    GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_commonItem.itemId);
                    if (consortRef != null)
                    {
                        tdShow = consortRef.td_show;
                        bgIndex = consortRef.td_bg_index;
                        ALUGUICommon.setLabelTxt(wnd.txtName, consortRef.transName);
                        ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(consortRef.consort_title));
                    }
                    break;
            }
            int maxShowCaseUnitCount = Mathf.Max(wnd.actorInShowCaseIndex, wnd.bgInShowCaseIndex) + 1;
            if (maxShowCaseUnitCount > 0)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[maxShowCaseUnitCount];
                if (wnd.actorInShowCaseIndex >= 0)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(tdShow), wnd.actorInShowCaseIndex);
                if (wnd.bgInShowCaseIndex >= 0)
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(bgIndex), wnd.bgInShowCaseIndex);
                _m_wShowCase?.showWnd(showCaseUnitInfoObjList);
            }
            else
                _m_wShowCase?.hideWnd();

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHeroShowList, _m_commonItem.itemType == ENPItemType.HERO);
            ALUGUICommon.setGameObjEnable(wnd.goConsortShowList, _m_commonItem.itemType == ENPItemType.CONSORT);
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_commonItem == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(_m_commonItem.itemType, _m_commonItem.itemId);
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

        //点击形象
        private void _onClickActor(ShowcaseInfo _obj)
        {
            _onClickPreview(null);
        }

        //点击预览
        private void _onClickPreview(GameObject _go)
        {
            if (_m_commonItem == null)
                return;

            switch (_m_commonItem.itemType)
            {
                case ENPItemType.HERO:
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_commonItem.itemId);
                    HeroCardShowInfo showInfo = new HeroCardShowInfo(heroInfo, GRefdataCoreMgr.instance.heroRefCore.getRef(_m_commonItem.itemId));
                    if (heroInfo == null)
                        QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
                    else
                        QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null, false));
                    break;
                case ENPItemType.CONSORT:
                    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_commonItem.itemId);
                    if (consortInfo == null)
                        QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_commonItem.itemId)));
                    else
                        GNodeUnLockConsortDetail.addConsortNode(new List<GGottenConsortInfo>() { consortInfo }, consortInfo.consortId, EUnLockConsortDetailWndTabType.NONE, EConsortTdShowAniType.NONE);
                    break;
            }
        }
    }
}
