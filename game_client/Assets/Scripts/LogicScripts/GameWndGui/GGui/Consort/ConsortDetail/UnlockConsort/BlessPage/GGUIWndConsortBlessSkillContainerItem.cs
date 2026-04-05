using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 加护技能列表item
    /// </summary>
    public class GGUIWndConsortBlessSkillContainerItem : _ATALBasicUISubWnd<GGUIMonoConsortBlessSkillContainerItem>
    {
        private GGottenConsortInfo _m_iConsortInfo;//妃子信息
        /// <summary>
        /// 妃子加护技能信息
        /// </summary>
        private ConsortBlessSkillInfo _m_blessSkillInfo;
        
        private NPCommonCostItem _m_LevelUpCostItem;//升级消耗物品
        
        /// <summary>
        /// 升级消耗物品窗口
        /// </summary>
        private NPGGUIWndCommonItem _m_costItemWnd;
        
        public GGUIWndConsortBlessSkillContainerItem(GGUIMonoConsortBlessSkillContainerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.cost_item != null)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.cost_item);
         
            ALUGUICommon.combineBtnClick(wnd.btnLvlUp, _clickLevelUpBtn);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnLvlUp, _clickLevelUpBtn);
            }
            
            _m_costItemWnd?.discard();
            _m_costItemWnd = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _onConsortBlessSkillInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHARM_POINT_CHG, _onConsortCharmPointChg);

        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _onConsortBlessSkillInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CHARM_POINT_CHG, _onConsortCharmPointChg);

            _m_costItemWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_costItemWnd?.resetWnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_consortInfo">妃子信息</param>
        /// <param name="_blessSKillInfo">加护技能信息</param>
        public void setData(GGottenConsortInfo _consortInfo, ConsortBlessSkillInfo _blessSKillInfo)
        {
            _m_iConsortInfo = _consortInfo;
            _m_blessSkillInfo = _blessSKillInfo;

            _m_LevelUpCostItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_bless_point_item,
                _m_blessSkillInfo?.consortBlessSkillLvlRefObj?.cost_skill_point ?? 0);

            _refreshWnd();
        }

        //设置点击升级
        public void setClickUpgrade()
        {
            _clickLevelUpBtn(null);
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_blessSkillInfo == null || _m_blessSkillInfo.consortBlessSkillRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_blessSkillInfo.consortBlessSkillRef.name));

            ALUGUICommon.setLabelTxt(wnd.txtLvl, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_blessSkillInfo.level));

            ALUGUICommon.setLabelTxt(wnd.txtNowLvlAddEffectDesc, TextTranslate.instance.getLanguage(_m_blessSkillInfo.consortBlessSkillRef.desc, _m_blessSkillInfo.consortBlessSkillLvlRefObj?.desc_args_list));

            ConsortBlessSkillLvlRefObj maxLevelSkillLvlRefObj = _m_blessSkillInfo.consortBlessSkillRef.skillLvlRefList?.GetLast();//最高级配表数据
            if (maxLevelSkillLvlRefObj == null || maxLevelSkillLvlRefObj.lvl <= _m_blessSkillInfo.level)
            {
                ALUGUICommon.setGameObjEnable(wnd.maxLvlShow, true);
                ALUGUICommon.setGameObjEnable(wnd.maxLvlHide, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.maxLvlShow, false);
                ALUGUICommon.setGameObjEnable(wnd.maxLvlHide, true);
                
                ConsortBlessSkillLvlRefObj nextLvlRef = GRefdataCoreMgr.instance.getConsortBlessSkillLvlRefObj(_m_blessSkillInfo.consortBlessSkillRef, _m_blessSkillInfo.level + 1);
                if(nextLvlRef != null)
                    ALUGUICommon.setLabelTxt(wnd.txtNextLvlAddEffectDesc, TextTranslate.instance.getLanguage(TransKeyConst.consort_relate_next_level_desc, nextLvlRef.desc_args_list));
            }

            _refreshCostItem();
        }
        
        /// <summary>
        /// 刷新消耗道具
        /// </summary>
        private void _refreshCostItem()
        {
            _m_LevelUpCostItem.setCount(_m_blessSkillInfo?.consortBlessSkillLvlRefObj?.cost_skill_point ?? 0);
            if (_m_costItemWnd != null)
            {
                _m_costItemWnd.showWnd();
                _m_costItemWnd.setItem(_m_LevelUpCostItem, _m_iConsortInfo?.charmPoint ?? 0);
            }
        }

        /// <summary>
        /// 点击升级按钮
        /// </summary>
        private void _clickLevelUpBtn(GameObject _go)
        {
            if(_m_blessSkillInfo == null || _m_blessSkillInfo.consortBlessSkillLvlRefObj == null || _m_iConsortInfo == null)
                return;
            
            long costSkillPoint = _m_blessSkillInfo.consortBlessSkillLvlRefObj.cost_skill_point;//升级需要加护点
            if (costSkillPoint > _m_iConsortInfo?.charmPoint)//若升级需要加护点数大于当前妃子拥有的加护点数
            {
                GCommon.dealItemNotEnough(GRefdataCoreMgr.instance.npGeneral.consort_bless_point_item);
                return;
            }
            
            //请求升级加护技能
            NPPlayer.instance.consortComp.reqUpgradeBlessSkill(_m_iConsortInfo.consortId, _m_blessSkillInfo.skillId,
                (_msg) =>
                {
                    
                }, null);
        }

        /// <summary>
        /// 当妃子加护技能信息变化时
        /// </summary>
        private void _onConsortBlessSkillInfoChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 2 || !(_objs[0] is long) || !(_objs[1] is ConsortBlessSkillInfo) 
               || _m_iConsortInfo == null || _m_blessSkillInfo == null)
                return;
            
            long consortId = (long)_objs[0];
            ConsortBlessSkillInfo blessSkillInfo = (ConsortBlessSkillInfo)_objs[1];

            if (_m_iConsortInfo.consortId == consortId && _m_blessSkillInfo.skillId == blessSkillInfo.skillId)
            {
                _refreshWnd();
            }
        }

        /// <summary>
        /// 当妃子加护点数变化时
        /// </summary>
        /// <param name="_objs"></param>
        private void _onConsortCharmPointChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is long) || _m_iConsortInfo == null || _m_blessSkillInfo == null)
                return;

            long consortId = (long) _objs[0];
            if(consortId != _m_iConsortInfo.consortId)
                return;
            
            _refreshCostItem();
        }
    }
}