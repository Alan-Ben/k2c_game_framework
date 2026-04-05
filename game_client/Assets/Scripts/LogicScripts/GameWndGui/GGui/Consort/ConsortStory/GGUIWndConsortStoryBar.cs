using ALPackage;
using Common.ConsortEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortStoryBar : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoConsortStoryBar>
    {
        private NPCommonAssetPathInfo _m_assetPathInfo;
        
        private EConsortStoryType _m_eStoryType;
        
        public GGUIWndConsortStoryBar(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_assetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_assetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_assetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
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