using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 羁绊升级弹窗
    /// </summary>
    public class GGUIWndConsortLevelUpFetter : _ATALBasicUIWnd<GGUIMonoConsortLevelUpFetter>
    {
        private static GGUIWndConsortLevelUpFetter _g_instance;
        public static GGUIWndConsortLevelUpFetter instance { get { return _g_instance ??= new GGUIWndConsortLevelUpFetter(); } }
     
        private GGottenConsortInfo _m_iConsortInfo;//妃子信息

        private NPGGuiWndTexture _m_wNowFetterlvlSignIcon;//当前羁绊等级标志icon
        private NPGGuiWndTexture _m_wNextFetterlvlSignIcon;//下一羁绊等级标志icon
        
        private NPGGuiWndTexture _m_wFetterSkillIcon;//当前羁绊技能icon

        private CommonUISfxObj _m_oLevelUpSfxObj;//升级成功特效
        
        public GGUIWndConsortLevelUpFetter() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortLevelUpFetter.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortLevelUpFetter.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.nowFetterLvlSign != null)
                _m_wNowFetterlvlSignIcon = new NPGGuiWndTexture(wnd.nowFetterLvlSign);
            
            if (wnd.nextFetterLvlSign != null)
                _m_wNextFetterlvlSignIcon = new NPGGuiWndTexture(wnd.nextFetterLvlSign);
            
            if(wnd.imgFetterSkillIcon != null)
                _m_wFetterSkillIcon = new NPGGuiWndTexture(wnd.imgFetterSkillIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnFetterLvlEffectDetail, _onClickFetterLvlEffectDetailBtn);
            ALUGUICommon.combineBtnClick(wnd.btnLevelUp, _onClickLevelUpBtn);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnFetterLvlEffectDetail, _onClickFetterLvlEffectDetailBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnLevelUp, _onClickLevelUpBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wNowFetterlvlSignIcon?.discard();
            _m_wNowFetterlvlSignIcon = null;
            
            _m_wNextFetterlvlSignIcon?.discard();
            _m_wNextFetterlvlSignIcon = null;
            
            _m_wFetterSkillIcon?.discard();
            _m_wFetterSkillIcon = null;
            
            _discardLevelUpSfxObj();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wNowFetterlvlSignIcon?.hideWnd();
            _m_wNextFetterlvlSignIcon?.hideWnd();
            _m_wFetterSkillIcon?.hideWnd();
            
            _discardLevelUpSfxObj();
        }

        protected override void _onReset()
        {
            _m_wNowFetterlvlSignIcon?.discardTexture();
            _m_wNextFetterlvlSignIcon?.discardTexture();
            _m_wFetterSkillIcon?.discardTexture();
            
            _discardLevelUpSfxObj();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_iConsortInfo = _consortInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iConsortInfo == null || _m_iConsortInfo.consortRefObj == null)
                return;

            ConsortFetterInfo consortFetterInfo = _m_iConsortInfo.fetterInfo;
            if(consortFetterInfo == null || consortFetterInfo.consortFettersLvlRef == null)
                return;
            
            ConsortFettersLvlRefObj nextFetterLvlRef = GRefdataCoreMgr.instance.consortFettersLvlRefCore.getRef(consortFetterInfo.consortFettersLvlRef.lvl + 1);
            
            ALUGUICommon.setGameObjEnable(wnd.maxLevelShow, consortFetterInfo.consortFettersLvlRef.isMaxLvl);
            ALUGUICommon.setGameObjEnable(wnd.maxLevelHide, !consortFetterInfo.consortFettersLvlRef.isMaxLvl);

            ALUGUICommon.setLabelTxt(wnd.txtNowFetterLvlName, TextTranslate.instance.getLanguage(consortFetterInfo.consortFettersLvlRef.name));
            if (_m_wNowFetterlvlSignIcon != null)
            {
                _m_wNowFetterlvlSignIcon.showWnd();
                _m_wNowFetterlvlSignIcon.setTexture(consortFetterInfo.consortFettersLvlRef.fetters_sign_icon);
            }

            ALUGUICommon.setLabelTxt(wnd.txtNextFetterLvlName, TextTranslate.instance.getLanguage(nextFetterLvlRef?.name));
            if (_m_wNextFetterlvlSignIcon != null)
            {
                _m_wNextFetterlvlSignIcon.showWnd();
                _m_wNextFetterlvlSignIcon.setTexture(nextFetterLvlRef?.fetters_sign_icon);
            }

            // 刷新子嗣品质信息
            ALUGUICommon.setGameObjEnable(wnd.childQualityHasImprovedShow, nextFetterLvlRef != null && consortFetterInfo.consortFettersLvlRef.adopt_child_quality_id != nextFetterLvlRef.adopt_child_quality_id);

            ALUGUICommon.setLabelTxt(wnd.txtNowFetterLvlChildQuality,
                TextTranslate.instance.getLanguage(consortFetterInfo.consortFettersLvlRef.adoptChildQualityRefObj?.name ?? string.Empty,
                    consortFetterInfo.consortFettersLvlRef.adoptChildQualityRefObj?.name_args));
            if (nextFetterLvlRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtNextFetterLvlChildQuality, 
                    TextTranslate.instance.getLanguage(nextFetterLvlRef.adoptChildQualityRefObj?.name ?? string.Empty,
                        nextFetterLvlRef.adoptChildQualityRefObj?.name_args));
            }
            
            // 当前羁绊技能信息
            ConsortFetterSkillInfo nowSkillLvlInfo = consortFetterInfo.consortFetterSkillInfo;
            // 获取下一级羁绊技能信息
            ConsortFettersSkillLvlRefObj nextSkillLvlRefObj =
                GRefdataCoreMgr.instance.getConsortFettersSkillLvlRefObj(_m_iConsortInfo.consortRefObj.consort_fetters_skill_id, nextFetterLvlRef?.consort_fetters_skill_lvl ?? 0);
            
            // 刷新羁绊技能信息
            bool fetterSkillLvlHasChg = nextFetterLvlRef != null && consortFetterInfo.consortFettersLvlRef.consort_fetters_skill_lvl != nextFetterLvlRef.consort_fetters_skill_lvl 
                                        && nowSkillLvlInfo != null && nowSkillLvlInfo.consortFettersSkillRefObj != null 
                                        && nowSkillLvlInfo.consortFettersSkillLvlRefObj != null && nextSkillLvlRefObj != null;
            ALUGUICommon.setGameObjEnable(wnd.fetterSkillLvlHasImprovedShow, fetterSkillLvlHasChg);
            if (fetterSkillLvlHasChg)
            {
                ALUGUICommon.setLabelTxt(wnd.txtFetterSkillName,
                    TextTranslate.instance.getLanguage(consortFetterInfo.consortFetterSkillInfo.consortFettersSkillRefObj.name));

                if (_m_wFetterSkillIcon != null)
                {
                    _m_wFetterSkillIcon.showWnd();
                    _m_wFetterSkillIcon.setTexture(consortFetterInfo.consortFetterSkillInfo.consortFettersSkillRefObj.icon);
                }

                ALUGUICommon.setLabelTxt(wnd.txtNowFetterSkillLvl,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, nowSkillLvlInfo.consortFettersSkillLvlRefObj.lvl));
                ALUGUICommon.setLabelTxt(wnd.txtNextFetterSkillLvl, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, nextSkillLvlRefObj.lvl));

                ALUGUICommon.setLabelTxt(wnd.txtNowFetterSkillLvlEffectDesc,
                    TextTranslate.instance.getLanguage(nowSkillLvlInfo.consortFettersSkillRefObj.desc,
                        nowSkillLvlInfo.consortFettersSkillLvlRefObj.desc_args_list));

                string nextFetterSkillLvlEffectDescKey = string.IsNullOrEmpty(wnd.nextFetterSkillLvlEffectDescKey) ? nowSkillLvlInfo.consortFettersSkillRefObj.desc : wnd.nextFetterSkillLvlEffectDescKey;
                ALUGUICommon.setLabelTxt(wnd.txtNextFetterSkillLvlEffectDesc,
                    TextTranslate.instance.getLanguage(nextFetterSkillLvlEffectDescKey, nextSkillLvlRefObj.desc_args_list));
            }

            // 刷新升级条件
            // https://www.teambition.com/task/67b6f358e42d3fd72e673aa8 【优化-0】家人羁绊技能升级需求修改
            int nowConsortNum = NPPlayer.instance.consortComp.getConsortCount();//
            string nowConsortNumStr = nowConsortNum >= consortFetterInfo.consortFettersLvlRef.need_consort_num
                ? GCommon.addColorForRichText(nowConsortNum.ToString(), wnd.needEnoughColor)
                : GCommon.addColorForRichText(nowConsortNum.ToString(), wnd.needNotEnoughColor);
            string txtNeedPlayerLvlKey = string.IsNullOrEmpty(wnd.txtNeedPlayerLvlKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtNeedPlayerLvlKey;
            ALUGUICommon.setLabelTxt(wnd.txtNeedPlayerLvl, TextTranslate.instance.getLanguage(txtNeedPlayerLvlKey, nowConsortNumStr, consortFetterInfo.consortFettersLvlRef.need_consort_num));
            
            string nowIntimacyStr = _m_iConsortInfo.intimacy >= consortFetterInfo.consortFettersLvlRef.need_consort_intimacy
                ? GCommon.addColorForRichText(_m_iConsortInfo.intimacy.ToString(), wnd.needEnoughColor) 
                : GCommon.addColorForRichText(_m_iConsortInfo.intimacy.ToString(), wnd.needNotEnoughColor);
            ALUGUICommon.setLabelTxt(wnd.txtNeedIntimacy, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, nowIntimacyStr, consortFetterInfo.consortFettersLvlRef.need_consort_intimacy));
            
            string nowCharmStr = _m_iConsortInfo.charm >= consortFetterInfo.consortFettersLvlRef.need_consort_charm
                ? GCommon.addColorForRichText(_m_iConsortInfo.charm.ToString(), wnd.needEnoughColor) 
                : GCommon.addColorForRichText(_m_iConsortInfo.charm.ToString(), wnd.needNotEnoughColor);
            ALUGUICommon.setLabelTxt(wnd.txtNeedCharm, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, nowCharmStr, consortFetterInfo.consortFettersLvlRef.need_consort_charm));
        }

        #region 点击事件

        /// <summary>
        /// 当点击查看羁绊等级效果详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickFetterLvlEffectDetailBtn(GameObject _go)
        {
            if (_m_iConsortInfo == null || _go == null || wnd == null || _m_iConsortInfo.fetterInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_ConsortFetterEffect(
                wnd.fetterLvlDetailToolTipUIResId, _m_iConsortInfo.fetterInfo.fetterLevel, (RectTransform)_go.transform, 0, 0));
        }

        /// <summary>
        /// 点击升级按钮
        /// </summary>
        private void _onClickLevelUpBtn(GameObject _go)
        {
            NPPlayer.instance.consortComp.reqUpgradeFettersLvl(_m_iConsortInfo, (_msg) =>
            {
                if (_m_iConsortInfo != null && _m_iConsortInfo.fetterInfo != null && _m_iConsortInfo.fetterInfo.consortFettersLvlRef != null)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.consort_fetter_levelUpSuccTip_str1, _m_iConsortInfo.fetterInfo.consortFettersLvlRef.name));
                    // 播放羁绊等级升级音效
                    ConsortVoiceMgr.instance.playVoice(_m_iConsortInfo.consortId, EConsortVoiceType.RankUp);
                }
                
                _refreshWnd();
                _playLevelUpSfxObj();
            }, null);
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_LEVELUP_FETTER);
        }

        #endregion

        #region 升级成功特效

        /// <summary>
        /// 销毁升级特效
        /// </summary>
        private void _discardLevelUpSfxObj()
        {
            _m_oLevelUpSfxObj?.forceDiscard();
            _m_oLevelUpSfxObj = null;
        }
        
        /// <summary>
        /// 播放升级成功特效
        /// </summary>
        private void _playLevelUpSfxObj()
        {
            _discardLevelUpSfxObj();
            if(wnd == null || wnd.levelUpSfxParent == null || wnd.levelUpSfxId <= 0)
                return;

            _m_oLevelUpSfxObj = PlaySfxMgr.instance.playUISfx(wnd.levelUpSfxId, wnd.levelUpSfxParent);
        }

        #endregion
    }
}