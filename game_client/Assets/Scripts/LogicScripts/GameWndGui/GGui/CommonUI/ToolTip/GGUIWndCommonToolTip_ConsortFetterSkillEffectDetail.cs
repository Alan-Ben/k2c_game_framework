using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndCommonToolTip_ConsortFetterSkillEffectDetail : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_ConsortFetterSkillEffectDetail>
    {
        private int _m_iFettersLvl;//当前所处羁绊等级
        private ConsortFettersSkillRefObj _m_rSkillRefObj;//羁绊技能配表
        
        private GGUIWndConsortFetterSkillEffectDescItemContainer _m_wEffectDescItemContainer;
        
        public GGUIWndCommonToolTip_ConsortFetterSkillEffectDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
            
            if(wnd.fettersSkillEffectDescContainer != null)
            {
                _m_wEffectDescItemContainer = new GGUIWndConsortFetterSkillEffectDescItemContainer(wnd.fettersSkillEffectDescContainer);
            }
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_wEffectDescItemContainer?.discard();
            _m_wEffectDescItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wEffectDescItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wEffectDescItemContainer?.resetWnd();
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_ConsortFetterSkillEffect));
        }

        public void setData(int _fettersLvl, ConsortFettersSkillRefObj _skillRefObj, RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _m_iFettersLvl = _fettersLvl;
            _m_rSkillRefObj = _skillRefObj;
            
            _refreshWnd();
            
            setPos(_targetTransRoot, _intervalX, _intervalY);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || _m_rSkillRefObj == null)
                return;

            // 获取当前所处羁绊等级配表数据
            ConsortFettersLvlRefObj fettersLvlRefObj = GRefdataCoreMgr.instance.consortFettersLvlRefCore.getRef(_m_iFettersLvl);
            if (fettersLvlRefObj != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtNowFetterSkillLvl, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, fettersLvlRefObj?.consort_fetters_skill_lvl));
                ALUGUICommon.setLabelTxt(wnd.txtNowFetterSkillName, TextTranslate.instance.getLanguage(_m_rSkillRefObj.name));

                // 获取当前所处羁绊技能等级配表数据
                ConsortFettersSkillLvlRefObj skillLvlRefObj = _m_rSkillRefObj.getSkillLvlRefByLvl(fettersLvlRefObj?.consort_fetters_skill_lvl ?? 0);

                ALUGUICommon.setLabelTxt(wnd.txtNowFetterSkillEffectDesc, TextTranslate.instance.getLanguage(_m_rSkillRefObj.desc, skillLvlRefObj?.desc_args_list));                
            }
            
            if (_m_wEffectDescItemContainer != null)
            {
                _m_wEffectDescItemContainer.showWnd();
                _m_wEffectDescItemContainer.setData(fettersLvlRefObj, _m_rSkillRefObj);
            }
        }
    }
}