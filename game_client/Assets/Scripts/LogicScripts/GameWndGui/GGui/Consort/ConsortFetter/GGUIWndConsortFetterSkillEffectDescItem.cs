using ALPackage;

namespace GOE
{
    /// <summary>
    /// 羁绊技能效果描述item
    /// </summary>
    public class GGUIWndConsortFetterSkillEffectDescItem : _ATALBasicUISubWnd<GGUIMonoConsortFetterSkillEffectDescItem>
    {
        private ConsortFettersLvlRefObj _m_rFetterLvlRefObj;//羁绊等级配表
        private ConsortFettersSkillRefObj _m_rSkillRefObj;//羁绊技能配表
        private EGameCommonUnlockType _m_unlockType;//解锁类型
        
        public GGUIWndConsortFetterSkillEffectDescItem(GGUIMonoConsortFetterSkillEffectDescItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
        }
        
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }
        
        public void setData(ConsortFettersLvlRefObj _fettersLvlRefObj, ConsortFettersSkillRefObj _skillRefObj, EGameCommonUnlockType _unlockType)
        {
            _m_rFetterLvlRefObj = _fettersLvlRefObj;
            _m_rSkillRefObj = _skillRefObj;
            _m_unlockType = _unlockType;
            
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_rFetterLvlRefObj == null || _m_rSkillRefObj == null)
                return;

            ConsortFettersSkillLvlRefObj skillLvlRefObj = _m_rSkillRefObj.getSkillLvlRefByLvl(_m_rFetterLvlRefObj.consort_fetters_skill_lvl);
            if(skillLvlRefObj == null)
                return;
            
            ConsortFetterSkillUnlockStateConf unlockStateConf = null;
            foreach (var item in wnd.unlockStateConfList)
            {
                if(item == null)
                    continue;

                if (item.unlockType == _m_unlockType)
                {
                    unlockStateConf = item;
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(item.showList, false);
                }
            }

            string effectDescKey = TransKeyConst.common_str_colon_str;
            if (unlockStateConf != null)
            {
                ALUGUICommon.setGameObjEnable(unlockStateConf.showList, true);
                
                if(!string.IsNullOrEmpty(unlockStateConf.effectDescKey))
                    effectDescKey = unlockStateConf.effectDescKey;
            }

            ALUGUICommon.setLabelTxt(wnd.txtEffectDesc,
                TextTranslate.instance.getLanguage(effectDescKey, _m_rFetterLvlRefObj.name,
                    TextTranslate.instance.getLanguage(_m_rSkillRefObj.desc, skillLvlRefObj.desc_args_list)));
        }
    }
}