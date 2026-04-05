using System.Collections.Generic;
using ALPackage;
using Common.NpChatObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴分享详情弹窗
    /// </summary>
    public class GGUIWndShareHeroDetail  : _ANPGGUIBasicWnd<GGUIMonoShareHeroDetail>
    {
        private static GGUIWndShareHeroDetail _g_instance;

        public static GGUIWndShareHeroDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareHeroDetail();
                return _g_instance;
            }
        }

        //伙伴分享信息
        private NPCommon_ChatContent_HeroShare _m_heroShareInfo;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_commonShowcaseWnd;
        //觉醒技能列表
        private GGUIWndHeroStarSkillContainer _m_wStarSkillContainer;
        //资质技能
        private GGUIWndShareHeroTalentSkillContainer _m_wTalentSkillContainer;
        //星级附加窗口
        private GGUIWndHeroCommonStar _m_wStar;
        //相性图标
        private NPGGuiWndTexture _m_wSpecAttrIcon;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;

        public GGUIWndShareHeroDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareHeroDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareHeroDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_commonShowcaseWnd?.hideWnd();
            _m_wStarSkillContainer?.hideWnd();
            _m_wTalentSkillContainer?.hideWnd();
            _m_wStar?.hideWnd();
            _m_wSpecAttrIcon?.hideWnd();
            _pushBackQualityGo();
        }

        protected override void _onReset()
        {
            _m_commonShowcaseWnd?.resetWnd();
            _m_wStarSkillContainer?.resetWnd();
            _m_wTalentSkillContainer?.resetWnd();
            _m_wStar?.resetWnd();
            _m_wSpecAttrIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_commonShowcaseWnd?.discard();
            _m_commonShowcaseWnd = null;
            _m_wStarSkillContainer?.discard();
            _m_wStarSkillContainer = null;
            _m_wTalentSkillContainer?.discard();
            _m_wTalentSkillContainer = null;
            _m_wStar?.discard();
            _m_wStar = null;
            _m_wSpecAttrIcon?.discard();
            _m_wSpecAttrIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnInfo, _onClickInfo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShowCaseWnd != null)
                _m_commonShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowCaseWnd);

            if (wnd.monoStarSkillContainer != null)
                _m_wStarSkillContainer = new GGUIWndHeroStarSkillContainer(wnd.monoStarSkillContainer);

            if (wnd.monoTalentSkillContainer != null)
                _m_wTalentSkillContainer = new GGUIWndShareHeroTalentSkillContainer(wnd.monoTalentSkillContainer);

            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);

            if (wnd.imgSpecAttrIcon != null)
                _m_wSpecAttrIcon = new NPGGuiWndTexture(wnd.imgSpecAttrIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnInfo, _onClickInfo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroShow"></param>
        public void setInfo(NPCommon_ChatContent_HeroShare _heroShow)
        {
            _m_heroShareInfo = _heroShow;
            
            _refreshWnd();
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (null == wnd || _m_heroShareInfo == null)
                return;

            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroShareInfo.getHeroId());
            HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_heroShareInfo.getSkinId());

            if(heroRef == null || heroSkinRef == null)
                return;

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }
            //相性图标
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)heroRef.spec_attr_type);
            if (_m_wSpecAttrIcon != null)
            {
                _m_wSpecAttrIcon.showWnd();
                _m_wSpecAttrIcon.setTexture(basicAttrRef?.icon);
            }
            //相性名称
            ALUGUICommon.setLabelTxt(wnd.txtSpecAttrName, TextTranslate.instance.getLanguage(basicAttrRef?.name));
            //伙伴名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, _m_heroShareInfo.getHeroId()));
            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, _m_heroShareInfo.getLevel());
            //设置实力
            ALUGUICommon.setLabelTxt(wnd.txtPower, _m_heroShareInfo.getPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //设置资质
            ALUGUICommon.setLabelTxt(wnd.txtTalent, _m_heroShareInfo.getTalent().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //设置星级
            _m_wStar?.showWnd();
            _m_wStar?.setInfo(_m_heroShareInfo.getStar());
            //展示形象
            if (_m_commonShowcaseWnd != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[4];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(heroSkinRef.td_show), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(heroRef.td_bg_index), 3);
                _m_commonShowcaseWnd.showWnd(showCaseUnitInfoObjList);
            }

            //刷新觉醒技能列表
            _refreshStarSkill();
            //刷新资质技能列表
            _refreshTalentSkill();
        }

        //刷新觉醒技能列表
        private void _refreshStarSkill()
        {
            if (wnd == null || _m_heroShareInfo == null)
                return;

            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroShareInfo.getHeroId());
            if(heroRef == null)
                return;

            if (heroRef.star_skill_id_list != null && heroRef.star_skill_id_list.Count > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoStarHideList, true);
                _m_wStarSkillContainer?.showWnd();
                _m_wStarSkillContainer?.showItemList(null, heroRef.star_skill_id_list);
                _m_wStarSkillContainer?.setCustomLevel(_m_heroShareInfo.getStar() + 1);
            }
            else
                ALUGUICommon.setGameObjEnable(wnd.goNoStarHideList, false);
        }

        //刷新资质技能列表
        private void _refreshTalentSkill()
        {
            if (wnd == null || _m_heroShareInfo == null)
                return;

            List<HeroShareTalentSkillInfo> shareTalentSkillList = new List<HeroShareTalentSkillInfo>();
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroShareInfo.getHeroId());
            if (heroRef == null)
                return;

            //获取全部资质技能id
            List<long> talentSkillIdList = new List<long>();
            //添加默认资质技能列表
            if (heroRef.default_talent_skill_id_list != null)
                talentSkillIdList.AddRange(heroRef.default_talent_skill_id_list);
            //添加觉醒额外解锁的资质技能
            if (heroRef.extra_talent_skill_id_list != null)
            {
                for (int i = 0; i < heroRef.extra_talent_skill_id_list.Count; i++)
                {
                    if (heroRef.extra_talent_skill_id_list[i] != null)
                    {
                        talentSkillIdList.Add(heroRef.extra_talent_skill_id_list[i].second());
                    }
                }
            }

            //构建资质技能列表数据
            for (int i = 0; i < talentSkillIdList.Count; i++)
            {
                HeroShareTalentSkillInfo talentSkillInfo = new HeroShareTalentSkillInfo(talentSkillIdList[i]);
                bool isUnlock = false;
                for (int j = 0; j < _m_heroShareInfo.getTalentSkillList().Count; j++)
                {
                    if (_m_heroShareInfo.getTalentSkillList()[j].getTealentSkillId() == talentSkillIdList[i])
                    {
                        talentSkillInfo.setInfo(_m_heroShareInfo.getTalentSkillList()[j].getLevel(), true);
                        break;
                    }
                }
                shareTalentSkillList.Add(talentSkillInfo);
            }

            //排序：已解锁的在前，未解锁的在后，id小的在前
            shareTalentSkillList.Sort((_a, _b) =>
            {
                if (_a.isUnlock.CompareTo(_b.isUnlock) != 0)
                    return -(_a.isUnlock.CompareTo(_b.isUnlock));

                return _a.talentSkillId.CompareTo(_b.talentSkillId);
            });

            //展示列表
            _m_wTalentSkillContainer?.showWnd();
            _m_wTalentSkillContainer?.showItemList(shareTalentSkillList);
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_heroShareInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroShareInfo.getHeroId());
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

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_Chat_SHARE_HERO_DETAIL);
        }

        //点击简介
        private void _onClickInfo(GameObject _go)
        {
            if (_m_heroShareInfo == null)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroInfoIntroduction.instance, () =>
            {
                GGUIWndHeroInfoIntroduction.instance.showWnd();
                GGUIWndHeroInfoIntroduction.instance.setInfo(_m_heroShareInfo.getHeroId());
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_HERO_INTRODUCTION, false, false);
        }
    }

    /// <summary>
    /// 伙伴分享的资质技能信息
    /// </summary>
    public class HeroShareTalentSkillInfo
    {
        private long _m_lTalentSkillId;//技能id
        private long _m_lLevel;//等级
        private bool _m_bIsUnlock;//是否解锁

        public  long talentSkillId { get { return _m_lTalentSkillId; } }
        public  long level { get { return _m_lLevel; } }
        public  bool isUnlock { get { return _m_bIsUnlock; } }

        public HeroShareTalentSkillInfo(long _id)
        {
            _m_lTalentSkillId = _id;
            _m_lLevel = 0;
            _m_bIsUnlock = false;
        }

        public void setInfo(long _level, bool _isUnlock)
        {
            _m_lLevel = _level;
            _m_bIsUnlock = _isUnlock;
        }
    }
}