using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 星辉等级变更子窗口
    /// </summary>
    public class GGUISubWndConsortHaloLvlChg : _ATALBasicUISubWnd<GGUISubMonoConsortHaloLvlChg>
    {
        private ConsortHaloLvlRefObj _m_rPreHaloLvlRefObj;//上一等级星辉等级配表数据
        private ConsortHaloLvlRefObj _m_rNextHaloLvlRefObj;//下一等级星辉等级配表数据
        
        // 获取奖励列表
        private GGUIWndCommonRewardContainer _m_wCommonRewardContainer;

        // 星辉技能等级变化Container
        private GGUIWndConsortHaloSkillLvlChgContainer _m_wSkillLvlChgContainer;//技能等级变更容器
        
        public GGUISubWndConsortHaloLvlChg(GGUISubMonoConsortHaloLvlChg _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if (wnd.monoCommonRewardContainer != null)
                _m_wCommonRewardContainer = new GGUIWndCommonRewardContainer(wnd.monoCommonRewardContainer);

            if (wnd.monoHaloSkillLvlChgContainer != null)
                _m_wSkillLvlChgContainer = new GGUIWndConsortHaloSkillLvlChgContainer(wnd.monoHaloSkillLvlChgContainer);
        }
        
        protected override void _onDiscard()
        {
            _m_wCommonRewardContainer?.discard();
            _m_wCommonRewardContainer = null;
            
            _m_wSkillLvlChgContainer?.discard();
            _m_wSkillLvlChgContainer = null;   
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCommonRewardContainer?.hideWnd();
            _m_wSkillLvlChgContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonRewardContainer?.resetWnd();
            _m_wSkillLvlChgContainer?.resetWnd();
        }

        public void setData(ConsortHaloLvlRefObj _preHaloLvlRefObj, ConsortHaloLvlRefObj _nextHaloLvlRefObj)
        {
            _m_rPreHaloLvlRefObj = _preHaloLvlRefObj;
            _m_rNextHaloLvlRefObj = _nextHaloLvlRefObj;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            #region 星辉等级变化文本

            if (string.IsNullOrEmpty(wnd.txtHaloLevelChgKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtHaloLevelChg,
                    $"{TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, _m_rPreHaloLvlRefObj?.level ?? 0)} -> " +
                    $"{TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, _m_rNextHaloLvlRefObj?.level ?? 0)}");
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtHaloLevelChg, TextTranslate.instance.getLanguage(wnd.txtHaloLevelChgKey, _m_rPreHaloLvlRefObj?.level ?? 0, _m_rNextHaloLvlRefObj?.level ?? 0));
            }

            string haloLvlKey = string.IsNullOrEmpty(wnd.txtHaloLvlKey) ? TransKeyConst.common_value : wnd.txtHaloLvlKey;
            ALUGUICommon.setLabelTxt(wnd.txtPreHaloLvl, TextTranslate.instance.getLanguage(haloLvlKey, _m_rPreHaloLvlRefObj?.level ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtNextHaloLvl, TextTranslate.instance.getLanguage(haloLvlKey, _m_rNextHaloLvlRefObj?.level ?? 0));
            
            #endregion

            #region 星辉等级奖励

            if (_m_rNextHaloLvlRefObj != null && _m_rNextHaloLvlRefObj.reward_item_list != null && _m_rNextHaloLvlRefObj.reward_item_list.Count > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasHaloRewardShow, true);
                //存在奖励
                if (_m_wCommonRewardContainer != null)
                {
                    _m_wCommonRewardContainer.showWnd();
                    _m_wCommonRewardContainer.setRewardList(_m_rNextHaloLvlRefObj.reward_item_list);
                }
            }
            else
            {
                //不存在奖励
                ALUGUICommon.setGameObjEnable(wnd.hasHaloRewardShow, false);
                _m_wCommonRewardContainer?.hideWnd();
            }

            #endregion

            #region 妃子属性加成变化

            long preAddIntimacy = _m_rPreHaloLvlRefObj?.add_intimacy ?? 0;//上一等级亲密度加成
            long nextAddIntimacy = _m_rNextHaloLvlRefObj?.add_intimacy ?? 0;//下一等级亲密度加成
            bool hasIntimacyAddChange = preAddIntimacy != nextAddIntimacy;//是否有亲密度加成变化
            
            long preAddCharm = _m_rPreHaloLvlRefObj?.add_charm ?? 0;//上一等级加护力加成
            long nextAddCharm = _m_rNextHaloLvlRefObj?.add_charm ?? 0;//下一等级加护力加成
            // 是否有加护力加成变化
            bool hasCharmAddChange = preAddCharm != nextAddCharm;
            
            ALUGUICommon.setGameObjEnable(wnd.hasConsortAttrChgShow, hasIntimacyAddChange || hasCharmAddChange);
            ALUGUICommon.setGameObjEnable(wnd.hasIntimacyChgShow, hasIntimacyAddChange);
            ALUGUICommon.setGameObjEnable(wnd.hasCharmChgShow, hasCharmAddChange);

            if (hasIntimacyAddChange)
            {
                string intimacyChgStr = string.IsNullOrEmpty(wnd.txtIntimacyChgKey) ? $"{{0}} -> {{1}}" : wnd.txtIntimacyChgKey;
                ALUGUICommon.setLabelTxt(wnd.txtIntimacyChg, TextTranslate.instance.getLanguage(intimacyChgStr, preAddIntimacy, nextAddIntimacy));
            }

            if (hasCharmAddChange)
            {
                string charmChgStr = string.IsNullOrEmpty(wnd.txtCharmChgKey) ? $"{{0}} -> {{1}}" : wnd.txtCharmChgKey;
                ALUGUICommon.setLabelTxt(wnd.txtCharmChg, TextTranslate.instance.getLanguage(charmChgStr, preAddCharm, nextAddCharm));
            }
            
            #endregion

            #region 星辉技能等级变化

            List<ConsortHaloSkillLvlChgInfo> skillLvlChgList =
                ConsortHaloSkillInfo.getHaloSkillLvlChgInfoList(_m_rPreHaloLvlRefObj?.halo_skill_list, _m_rNextHaloLvlRefObj?.halo_skill_list);

            if (skillLvlChgList != null && skillLvlChgList.Count > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasSkillLvlChgShow, true);

                if (_m_wSkillLvlChgContainer != null)
                {
                    _m_wSkillLvlChgContainer.showWnd();
                    _m_wSkillLvlChgContainer.setData(skillLvlChgList);
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasSkillLvlChgShow, false);
                
                _m_wSkillLvlChgContainer?.hideWnd();
            }
            
            #endregion
        }
    }
}