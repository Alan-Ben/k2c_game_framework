using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 羁绊信息子窗口
    /// </summary>
    public class GGUISubWndConsortFetterInfo : _ANPGGUIBasicSubWnd<GGUISubMonoConsortFetterInfo>
    {
        private ConsortFetterInfo _m_iConsortFetterInfo;//妃子羁绊信息

        private NPGGuiWndTexture _m_wFetterLvlSignIcon;//羁绊等级称号图标
        private NPGGuiWndTexture _m_wFetterSkillIcon;//羁绊技能图标
        
        public GGUISubWndConsortFetterInfo(GGUISubMonoConsortFetterInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.fetterSkillIcon)
                _m_wFetterSkillIcon = new NPGGuiWndTexture(wnd.fetterSkillIcon);

            if (wnd.imgFetterLvlSignIcon != null)
                _m_wFetterLvlSignIcon = new NPGGuiWndTexture(wnd.imgFetterLvlSignIcon);
        }
        
        protected override void _onDiscard()
        {
            if(_m_wFetterSkillIcon != null)
                _m_wFetterSkillIcon.discard();
            _m_wFetterSkillIcon = null;
            
            _m_wFetterLvlSignIcon?.discard();
            _m_wFetterLvlSignIcon = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wFetterSkillIcon?.hideWnd();
            _m_wFetterLvlSignIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wFetterSkillIcon?.discardTexture();
            _m_wFetterLvlSignIcon?.discardTexture();
        }

        public void setData(ConsortFetterInfo _consortFetterInfo)
        {
            _m_iConsortFetterInfo = _consortFetterInfo;
            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_iConsortFetterInfo == null)
                return;

            ConsortFettersLvlRefObj consortFettersLvlRefObj = _m_iConsortFetterInfo.consortFettersLvlRef;
            if (consortFettersLvlRefObj != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtFetterLvlName, TextTranslate.instance.getLanguage(consortFettersLvlRefObj.name));
                ALUGUICommon.setLabelTxt(wnd.txtFetterLvlEffectDesc, TextTranslate.instance.getLanguage(wnd.fetterLvlEffectDesc, consortFettersLvlRefObj.effect_desc_args_list));

                if (_m_wFetterLvlSignIcon != null)
                {
                    _m_wFetterLvlSignIcon.showWnd();
                    _m_wFetterLvlSignIcon.setTexture(consortFettersLvlRefObj.fetters_sign_icon);
                }
                
                bool isMaxLvl = consortFettersLvlRefObj.isMaxLvl;
                ALUGUICommon.setGameObjEnable(wnd.maxFetterLvlShow, isMaxLvl);
                ALUGUICommon.setGameObjEnable(wnd.maxFetterLvlHide, !isMaxLvl);
            }
            
            ConsortFetterSkillInfo fetterSkillInfo = _m_iConsortFetterInfo.consortFetterSkillInfo;
            if (fetterSkillInfo != null)
            {
                if (_m_wFetterSkillIcon != null)
                {
                    _m_wFetterSkillIcon.showWnd();
                    _m_wFetterSkillIcon.setTexture(fetterSkillInfo.consortFettersSkillRefObj?.icon);
                }
                
                ALUGUICommon.setLabelTxt(wnd.txtFetterSkillLvl, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, fetterSkillInfo.skillLevel));
                ALUGUICommon.setLabelTxt(wnd.txtFetterSkillName, TextTranslate.instance.getLanguage(fetterSkillInfo.consortFettersSkillRefObj?.name));

                // 若不存在羁绊技能等级数据
                if (fetterSkillInfo.consortFettersSkillLvlRefObj == null || fetterSkillInfo.consortFettersSkillRefObj == null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtSKillEffectDesc, string.Empty);
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtSKillEffectDesc, TextTranslate.instance.getLanguage(fetterSkillInfo.consortFettersSkillRefObj.desc, fetterSkillInfo.consortFettersSkillLvlRefObj.desc_args_list));
                }
                
                bool isSkillEnable = fetterSkillInfo.consortFettersSkillRefObj != null && 
                                     (fetterSkillInfo.consortFettersSkillRefObj.enable_condition == null || fetterSkillInfo.consortFettersSkillRefObj.enable_condition.isNoConditionOrEnable(null));
                ALUGUICommon.setGameObjEnable(wnd.fetterSkillEnableShow, isSkillEnable);
                ALUGUICommon.setGameObjEnable(wnd.fetterSkillDisableShow, !isSkillEnable);
            }
        }
    }
}