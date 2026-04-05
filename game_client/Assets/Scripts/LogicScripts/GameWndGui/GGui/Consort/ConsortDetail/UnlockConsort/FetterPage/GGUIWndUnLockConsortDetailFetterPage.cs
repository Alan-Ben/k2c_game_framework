using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面羁绊page
    /// </summary>
    public class GGUIWndUnLockConsortDetailFetterPage : _AGGUIWndUnLockConsortDetailTabPage<GGUIMonoUnLockConsortDetailFetterPage>
    {
        public GGUISubWndConsortFetterInfo _m_wndConsortFetterInfo;//羁绊信息子窗口
        
        public GGUIWndUnLockConsortDetailFetterPage(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_commonAssetPathInfo, _parent)
        {
        }

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public override EUnLockConsortDetailWndTabType tabPageType { get { return EUnLockConsortDetailWndTabType.FETTER; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoFetterInfo != null)
                _m_wndConsortFetterInfo = new GGUISubWndConsortFetterInfo(wnd.monoFetterInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnLevelUp, _onLevelUpBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnFetterSkillDetail, _clickFettersSkillDetailBtn);
            ALUGUICommon.combineBtnClick(wnd.btnFetterLvlDetail, _clickFettersLvlDetailBtn);
        }

        protected override void _onDiscardSub()
        {
            _m_wndConsortFetterInfo?.discard();
            _m_wndConsortFetterInfo = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnLevelUp, _onLevelUpBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnFetterSkillDetail, _clickFettersSkillDetailBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnFetterLvlDetail, _clickFettersLvlDetailBtn);
        }

        protected override void _onShowWndSub()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, _onConsortFetterLevelUp);
        }

        protected override void _onHideWndSub()
        {
            _m_wndConsortFetterInfo?.hideWnd();
            
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, _onConsortFetterLevelUp);
        }

        protected override void _onResetSub()
        {
            _m_wndConsortFetterInfo?.resetWnd();
        }
        
        protected override void _setDataSub()
        {
        }
        
        protected override void _refreshWndSub()
        {
            if(_m_iConsortShowInfo == null || wnd == null)
                return;

            ConsortFetterInfo consortFetterInfo = _m_iConsortShowInfo.fetterInfo;
            if(consortFetterInfo == null)
                return;

            if (_m_wndConsortFetterInfo != null)
            {
                _m_wndConsortFetterInfo.showWnd();
                _m_wndConsortFetterInfo.setData(consortFetterInfo);
            }

            bool isMaxFetterLvl = consortFetterInfo.consortFettersLvlRef?.isMaxLvl ?? true;//是否达到最高等级
            
            // https://www.teambition.com/task/67b6f358e42d3fd72e673aa8 【优化-0】家人羁绊技能升级需求修改
            // long levelUpNeedConsortCount = consortFetterInfo.consortFettersLvlRef?.need_player_lvl ?? 0;//升级需要玩家等级
            // long playerNowLvl = NPPlayer.instance.getValue(ENPPlayerValueType.LVL);//玩家当前等级
            int levelUpNeedConsortCount = consortFetterInfo.consortFettersLvlRef?.need_consort_num ?? 0;//升级需要妃子数量
            int playerNowConsortCount = NPPlayer.instance.consortComp.getConsortCount();//玩家当前妃子数量
            if (wnd.playerLvlSlider != null)
            {
                wnd.playerLvlSlider.wholeNumbers = true;
                wnd.playerLvlSlider.minValue = 0;
                wnd.playerLvlSlider.maxValue = isMaxFetterLvl ? playerNowConsortCount : levelUpNeedConsortCount;

                wnd.playerLvlSlider.value = playerNowConsortCount;
            }

            ALUGUICommon.setLabelTxt(wnd.txtPlayerLvlSliderValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, playerNowConsortCount,
                    levelUpNeedConsortCount));

            long levelUpNeedIntimacy = consortFetterInfo.consortFettersLvlRef?.need_consort_intimacy ?? 0;//升级需要亲密度
            if (wnd.consortIntimacySlider != null)
            {
                wnd.consortIntimacySlider.wholeNumbers = true;
                wnd.consortIntimacySlider.minValue = 0;
                wnd.consortIntimacySlider.maxValue = isMaxFetterLvl ? _m_iConsortShowInfo.intimacy : levelUpNeedIntimacy;

                wnd.consortIntimacySlider.value = _m_iConsortShowInfo.intimacy;
            }
            ALUGUICommon.setLabelTxt(wnd.txtConsortIntimacySliderValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _m_iConsortShowInfo.intimacy,
                    levelUpNeedIntimacy));
            
            long levelUpNeedCharm = consortFetterInfo.consortFettersLvlRef?.need_consort_charm ?? 0;//升级需要加护力
            if (wnd.consortCharmSlider != null)
            {
                wnd.consortCharmSlider.wholeNumbers = true;
                wnd.consortCharmSlider.minValue = 0;
                wnd.consortCharmSlider.maxValue = isMaxFetterLvl ? _m_iConsortShowInfo.charm : levelUpNeedCharm;

                wnd.consortCharmSlider.value = _m_iConsortShowInfo.charm;
            }
            ALUGUICommon.setLabelTxt(wnd.txtConsortCharmSliderValue,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _m_iConsortShowInfo.charm,
                    levelUpNeedCharm));
            
            ALUGUICommon.setGameObjEnable(wnd.maxLevelShow, isMaxFetterLvl);
            ALUGUICommon.setGameObjEnable(wnd.maxLevelHide, !isMaxFetterLvl);
        }

        /// <summary>
        /// 点击羁绊技能详情按钮
        /// </summary>
        private void _clickFettersSkillDetailBtn(GameObject _gameObject)
        {
            if (_m_iConsortShowInfo == null || _m_iConsortShowInfo.consortRefObj == null || _gameObject == null || wnd == null || _m_iConsortShowInfo.fetterInfo == null)
                return;

            ConsortFettersSkillRefObj skillRefObj = GRefdataCoreMgr.instance.consortFettersSkillRefCore.getRef(_m_iConsortShowInfo.consortRefObj.consort_fetters_skill_id);
            if (skillRefObj == null)
            {
                Debug.LogError($"[GGUIWndUnLockConsortDetailFetterPage _clickFettersSkillDetailBtn] error, 找不到妃子:{_m_iConsortShowInfo.consortId} 的羁绊技能:{_m_iConsortShowInfo.consortRefObj.consort_fetters_skill_id} 配表数据");
                return;
            }
            
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_ConsortFetterSkillEffect(
                wnd.fetterSkillDetailToolTipUIResId, _m_iConsortShowInfo.fetterInfo.fetterLevel, skillRefObj, (RectTransform)_gameObject.transform, 0, 0));
        }

        /// <summary>
        /// 点击羁绊等级详情按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _clickFettersLvlDetailBtn(GameObject _go)
        {
            if (_m_iConsortShowInfo == null || _m_iConsortShowInfo.consortRefObj == null || _go == null || wnd == null || _m_iConsortShowInfo.fetterInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_ConsortFetterEffect(
                wnd.fetterLvlDetailToolTipUIResId, _m_iConsortShowInfo.fetterInfo.fetterLevel, (RectTransform)_go.transform, 0, 0));
        }
        
        /// <summary>
        /// 当升级按钮被点击时
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onLevelUpBtnClick(GameObject _gameObject)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndConsortLevelUpFetter.instance, () =>
            {
                GGUIWndConsortLevelUpFetter.instance.showWnd();
                GGUIWndConsortLevelUpFetter.instance.setData(_m_iConsortShowInfo);
            }, UINodeTagConst.C_CONSORT_LEVELUP_FETTER);
        }
        
        /// <summary>
        /// 当前妃子羁绊等级提升
        /// </summary>
        private void _onConsortFetterLevelUp(params object[] _objs)
        {
            if(_m_iConsortShowInfo == null || _objs == null || 
               _objs.Length <= 0 || _objs[0] == null || !(_objs[0] is long))
                return;

            long consortId = (long) _objs[0];
            if (_m_iConsortShowInfo.consortId == consortId)
            {
                _refreshWnd();
            }
        }
    }
}