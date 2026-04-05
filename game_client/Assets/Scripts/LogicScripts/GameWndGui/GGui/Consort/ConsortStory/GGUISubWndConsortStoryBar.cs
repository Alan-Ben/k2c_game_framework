using ALPackage;
using Common.ConsortEnum;

namespace GOE
{
    public class GGUISubWndConsortStoryBar : _ANPGGUIBasicSubWnd<GGUIMonoConsortStoryBar>
    {
        private EConsortStoryType _m_eStoryType;
        
        public GGUISubWndConsortStoryBar(GGUIMonoConsortStoryBar _wnd) : base(_wnd)
        {
            initWnd();
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
        
        public void setData(EConsortStoryType _storyType)
        {
            _m_eStoryType = _storyType;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (wnd.barSettingList != null)
            {
                EConsortStoryTypeBarSetting barSetting = null;
                foreach (var item in wnd.barSettingList)
                {
                    if(item == null)
                        continue;
                    
                    if(item.storyType == _m_eStoryType)
                    {
                        barSetting = item;
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(item.goShowList, false);
                    }
                }

                if (barSetting != null)
                {
                    ALUGUICommon.setGameObjEnable(barSetting.goShowList, true);
                    ALUGUICommon.setLabelTxt(wnd.txtBarName, TextTranslate.instance.getLanguage(barSetting.barName));
                }
            }
        }
    }
}