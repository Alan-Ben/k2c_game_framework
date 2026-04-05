using System;
using ALPackage;
using Common.NpChatObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣分享详情弹窗
    /// </summary>
    public class GGUIWndShareChildDetail  : _ANPGGUIBasicWnd<GGUIMonoShareChildDetail>
    {
        private static GGUIWndShareChildDetail _g_instance;

        public static GGUIWndShareChildDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareChildDetail();
                return _g_instance;
            }
        }

        //子嗣分享信息
        private NPCommon_ChatContent_ChildShare _m_childShareInfo;
        //子嗣形象
        private NPGGUIWndCommonShowCase _m_commonShowcaseWnd;
        //相性图标
        private NPGGuiWndTexture _m_wSpecAttrIcon;
        //半身像
        private NPGGuiWndTexture _m_cardIcon;
        //是否已检查组队状态
        private bool _m_bIsCheckMarried;
        //检查组队状态完成回调
        private Action _m_aCheckDone;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndShareChildDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareChildDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareChildDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsCheckMarried = false;
            _m_aCheckDone = null;
            _m_commonShowcaseWnd?.hideWnd();
            _m_wSpecAttrIcon?.hideWnd();
            _m_cardIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_commonShowcaseWnd?.resetWnd();
            _m_wSpecAttrIcon?.discardTexture();
            _m_cardIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_commonShowcaseWnd?.discard();
            _m_commonShowcaseWnd = null;

            _m_wSpecAttrIcon?.discard();
            _m_wSpecAttrIcon = null;

            _m_cardIcon?.discard();
            _m_cardIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnTeamUp, _onClickTeamUp);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShowCase != null)
                _m_commonShowcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowCase);

            if (wnd.imgSpecAttrIcon != null)
                _m_wSpecAttrIcon = new NPGGuiWndTexture(wnd.imgSpecAttrIcon);

            if (wnd.imgCardIcon != null)
                _m_cardIcon = new NPGGuiWndTexture(wnd.imgCardIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnTeamUp, _onClickTeamUp);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroShow"></param>
        public void setInfo(NPCommon_ChatContent_ChildShare _heroShow)
        {
            _m_childShareInfo = _heroShow;
            _refreshWnd();
            _checkTeamUpState();
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (wnd == null || _m_childShareInfo == null)
                return;

            GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_childShareInfo.getConsortId());
            ChildResRefObj childResRef = GRefdataCoreMgr.instance.childResCore.getRef(_m_childShareInfo.getChildResId());
            ChildQualityRefObj childQualityRef = GRefdataCoreMgr.instance.childQualityCore.getRef(_m_childShareInfo.getChildQualityId());
            ChildAttrRefObj childAttrRef = GRefdataCoreMgr.instance.childAttrCore.getRef((long)_m_childShareInfo.getAttrType());
            ChildCareerRefObj childCareerRef = GRefdataCoreMgr.instance.childCareerCore.getRef(_m_childShareInfo.getChildCareerId());
            int childLevel = _m_childShareInfo.getChildLevel();
            int step = _m_childShareInfo.getChildStep();
            bool isGraduated = _m_childShareInfo.getIsGraduated();

            //未毕业展示形象
            if (!isGraduated)
            {
                if (_m_commonShowcaseWnd != null)
                {
                    _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[2];
                    showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(childResRef?.td_show), 0);
                    if (childQualityRef != null)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(GRefdataCoreMgr.instance.npGeneral.child_classroom_res_index), 1);
                    _m_commonShowcaseWnd.showWnd(showCaseUnitInfoObjList);
                    _m_commonShowcaseWnd.regInitDoneDelegate(() =>
                    {
                        if (childQualityRef != null)
                            _m_commonShowcaseWnd.playAnim(1, childAttrRef?.getRoomVideoNameByStep(childQualityRef.getStepByLvl(childLevel)));
                    });
                }
            }
            else
                _m_commonShowcaseWnd?.hideWnd();

            //相性图标
            if (childAttrRef != null)
            {
                BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)childAttrRef.type);
                if (_m_wSpecAttrIcon != null)
                {
                    _m_wSpecAttrIcon.showWnd();
                    _m_wSpecAttrIcon.setTexture(basicAttrRef?.icon);
                }
            }
            //半身像
            _m_cardIcon?.showWnd();
            _m_cardIcon?.setTexture(childResRef?.card_icon);
            //天资
            ALUGUICommon.setLabelTxt(wnd.txtQuality, childQualityRef?.nameTranslated);
            //名称
            ALUGUICommon.setLabelTxt(wnd.txtChildName, _m_childShareInfo.getChildName());
            //收益
            ALUGUICommon.setLabelTxt(wnd.txtEarnings, string.IsNullOrEmpty(wnd.earningsKey) ? _m_childShareInfo.getEarnings().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD) : TextTranslate.instance.getLanguage(wnd.earningsKey, _m_childShareInfo.getEarnings().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            //单次教学奖励
            ALUGUICommon.setLabelTxt(wnd.txtEducationExpValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_childShareInfo.getEducationExpValue().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //基础奖励
            ALUGUICommon.setLabelTxt(wnd.txtEducatingBaseAwards, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_childShareInfo.getEducatingBaseAwards().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            //天资加成
            ALUGUICommon.setLabelTxt(wnd.txtChildQualityBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_childShareInfo.getChildQualityBonus()/100f)));
            //情人羁绊加成
            ALUGUICommon.setLabelTxt(wnd.txtConsortBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_childShareInfo.getConsortBonus()/100f)));
            //顾问技能加成
            ALUGUICommon.setLabelTxt(wnd.txtHeroBonus, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_childShareInfo.getHeroBonus()/100f)));
            //监护人
            ALUGUICommon.setLabelTxt(wnd.txtGuardianName, consortRef?.transName);
            //职业
            ALUGUICommon.setLabelTxt(wnd.txtCareer, childCareerRef?.transName);
            //卷王展示
            ALUGUICommon.setGameObjEnable(wnd.goSuperShowList, _m_childShareInfo.getIsSuper());
            //阶段展示
            wnd.setPhase(step);
            //设置显隐组队按钮
            ALUGUICommon.setGameObjEnable(wnd.btnTeamUp, _m_childShareInfo.getCid() != NPPlayer.instance.playerInfo.CID &&
                                                         _m_childShareInfo.getCid() > 0 &&
                                                         _m_childShareInfo.getAdultId() >0 &&
                                                         _m_childShareInfo.getIsGraduated());
            ALUGUICommon.setGameObjEnable(wnd.goGraduatedShowList, isGraduated);
            ALUGUICommon.setGameObjEnable(wnd.goGraduatedHideList, !isGraduated);
        }

        //检查组队状态
        private void _checkTeamUpState()
        {
            if (_m_bIsCheckMarried || 
                _m_childShareInfo == null ||
                !_m_childShareInfo.getIsGraduated() ||
                _m_childShareInfo.getCid() <= 0 ||
                _m_childShareInfo.getAdultId() <= 0 ||
                _m_childShareInfo.getCid() == NPPlayer.instance.playerInfo.CID)
                return;

            //本地记录是否已组队过
            if (AccountSettingMgr.instance.childSaver.getChatShareAdultIsMarried(_m_childShareInfo.getAdultId()))
            {
                _m_bIsCheckMarried = true;
                return;
            }

            //请求服务器检查是否已组队
            long curSerialize = _m_lShowSerialize;
            NPPlayer.instance.childComp.reqCidAdultIsMarried(_m_childShareInfo.getCid(), _m_childShareInfo.getAdultId(), (_isSuc, _msg) =>
            {
                if (_msg == null || curSerialize != _m_lShowSerialize)
                    return;

                if (_isSuc)
                {
                    _m_bIsCheckMarried = true;
                    //处理检查完成回调
                    _m_aCheckDone?.Invoke();
                    _m_aCheckDone = null;
                }
            });
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_Chat_SHARE_CHILD_DETAIL);
        }

        //点击组队
        private void _onClickTeamUp(GameObject _go)
        {
            if (_m_childShareInfo == null ||
                !_m_childShareInfo.getIsGraduated() ||
                _m_childShareInfo.getCid() <= 0 ||
                _m_childShareInfo.getAdultId() <= 0 ||
                _m_childShareInfo.getCid() == NPPlayer.instance.playerInfo.CID)
                return;

            //系统是否解锁
            if (!GCommon.isFuncUnlock(ENPFunctionType.CHILD_MARRY, true))
                return;

            //本地记录是否已组队过
            if (AccountSettingMgr.instance.childSaver.getChatShareAdultIsMarried(_m_childShareInfo.getAdultId()))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_shareChildAlreadyMarried_none);
                return;
            }

            //处理组队点击事件
            Action dealClickTeamUp = () =>
            {
                bool isMarried = AccountSettingMgr.instance.childSaver.getChatShareAdultIsMarried(_m_childShareInfo.getAdultId());
                if (isMarried)
                {
                    //已组队，弹tip
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_shareChildAlreadyMarried_none);
                }
                else
                {
                    //未组队，打开组队界面
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareChildTeamUp.instance, () =>
                    {
                        GGUIWndShareChildTeamUp.instance.showWnd();
                        GGUIWndShareChildTeamUp.instance.setInfo(_m_childShareInfo);
                    }, UINodeTagConst.C_CHAT_SHARE_CHILD_TEAM_UP);
                }
            };

            //未检查过组队状态，监听检查请求再处理点击事件，否则直接处理点击事件
            if (!_m_bIsCheckMarried)
            {
                _m_aCheckDone = dealClickTeamUp;
            }
            else
            {
                dealClickTeamUp();
            }
        }
    }
}