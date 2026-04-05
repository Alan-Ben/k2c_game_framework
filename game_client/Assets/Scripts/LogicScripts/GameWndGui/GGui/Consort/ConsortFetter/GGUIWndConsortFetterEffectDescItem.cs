using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 妃子羁绊效果描述item
    /// </summary>
    public class GGUIWndConsortFetterEffectDescItem : _ATALBasicUISubWnd<GGUIMonoConsortFetterEffectDescItem>
    {
        private ConsortFettersLvlRefObj _m_rFettersLvlRefObj;//羁绊等级配表
        private bool _m_bIsNowLvl;//是否当前等级

        private NPGGUIWndCommonItem _m_wGraduateReward;//毕业奖励列表
        
        public GGUIWndConsortFetterEffectDescItem(GGUIMonoConsortFetterEffectDescItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoGraduateReward != null)
            {
                _m_wGraduateReward = new NPGGUIWndCommonItem(wnd.monoGraduateReward);
                _m_wGraduateReward.getShowItemNumStrFunc += _getShowRewardItemNum;
            }
        }
        
        protected override void _onDiscard()
        {
            if (_m_wGraduateReward != null)
            {
                _m_wGraduateReward.getShowItemNumStrFunc -= _getShowRewardItemNum;
                _m_wGraduateReward.discard();
                _m_wGraduateReward = null;
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wGraduateReward?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGraduateReward?.resetWnd();
        }
        
        public void setData(ConsortFettersLvlRefObj _fettersLvlRefObj, bool _isNowLvl)
        {
            _m_rFettersLvlRefObj = _fettersLvlRefObj;
            _m_bIsNowLvl = _isNowLvl;

            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_rFettersLvlRefObj == null)
                return;

            if (_m_rFettersLvlRefObj.adoptChildQualityRefObj != null)
                ALUGUICommon.setLabelTxt(wnd.txtChildQuality, TextTranslate.instance.getLanguage(_m_rFettersLvlRefObj.adoptChildQualityRefObj.name, _m_rFettersLvlRefObj.adoptChildQualityRefObj.name_args));

            // 学习天资加成
            string childStudyBonusStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_rFettersLvlRefObj.study_bonus / 100);
            childStudyBonusStr = GCommon.addColorForRichText(childStudyBonusStr, _m_bIsNowLvl ? wnd.nowLvlTxtColor : wnd.othersLvlTxtColor);
            ALUGUICommon.setLabelTxt(wnd.txtChildStudyBonus, childStudyBonusStr);
            
            // 收益天资加成
            string childIncomeBonusStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_rFettersLvlRefObj.income_bonus / 100);
            childIncomeBonusStr = GCommon.addColorForRichText(childIncomeBonusStr, _m_bIsNowLvl ? wnd.nowLvlTxtColor : wnd.othersLvlTxtColor);
            ALUGUICommon.setLabelTxt(wnd.txtChildIncomeBonus, childIncomeBonusStr);
            
            if (_m_wGraduateReward != null)
            {
                BasicAttrRefObj defaultAttr = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) ESpecAttrType.TYPE_A);
                _m_wGraduateReward.showWnd();
                _m_wGraduateReward.setItem(new NPCommonCostItem(defaultAttr?.graduate_item, _m_rFettersLvlRefObj.graduate_get_item_num));
            }    
            
            
            ALUGUICommon.setGameObjEnable(wnd.nowLvlShowGoList, _m_bIsNowLvl);
            ALUGUICommon.setGameObjEnable(wnd.othersLvlShowGoList, !_m_bIsNowLvl);
        }

        private string _getShowRewardItemNum(string _itemNum)
        {
            if (wnd == null)
                return _itemNum;

            return GCommon.addColorForRichText(_itemNum, _m_bIsNowLvl ? wnd.nowLvlTxtColor : wnd.othersLvlTxtColor);
        }
    }
}