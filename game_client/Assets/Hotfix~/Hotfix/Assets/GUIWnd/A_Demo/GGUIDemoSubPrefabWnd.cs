using ALPackage;
using UnityEngine;

namespace Hotfix
{
    public class GGUIDemoSubPrefabWnd : _AHotfixBaseSubPrefabWnd<GGUIDemoSubPrefabWndMono>
    {
        private string _m_assetPath;//_assetPath
        private string _m_objName;//_objName

        public GGUIDemoSubPrefabWnd(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }
        
        protected override string _monoAssetPath { get { return _m_assetPath;} }
        protected override string _monoObjName { get { return _m_objName;} }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onShowWnd");
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onHideWnd");
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onReset");
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onDiscard");
        }
        
        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onWndInitDoneHotfix");
        }
        
        /// <summary>
        /// 外部调用测试显示方法
        /// </summary>
        /// <param name="_value"></param>
        public void showText(string _value)
        {
            if(null == hotfixWnd)
                return;
            
            ALUGUICommon.setLabelTxt(hotfixWnd.goText, _value);
        }
    }
}