using ALPackage;
using GOE;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 范例Wnd
    /// </summary>
    public class GGuiDemoWnd : _AHotfixBaseWnd<GGUIDemoMono>
    {
        private static GGuiDemoWnd _g_instance;
        public static GGuiDemoWnd instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGuiDemoWnd();
                return _g_instance;
            }
        }
        
        protected override string _monoAssetPath { get { return "gui/game_gui.unity3d";} }
        protected override string _monoObjName { get { return "win_hotfix_demo_wnd";} }

        private GGUIDemoSubWnd _m_demoSubWnd;
        private GGUIDemoSubPrefabWnd _m_subPrefabWnd;
        private NPGGUIWndCommonItem _m_wItemWnd;//物品信息
        private GGuiWndSprite _m_wQualityWnd;//品质图片

        public GGuiDemoWnd() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGuiDemoWnd===_onShowWnd");

            if (_m_demoSubWnd != null)
            {
                _m_demoSubWnd.showWnd();
                _m_demoSubWnd.showText("测试SubWnd子窗口成功");
            }

            if (null != _m_subPrefabWnd)
            {
                _m_subPrefabWnd.showWnd();
                _m_subPrefabWnd.showText("测试subPrefabWnd子窗口成功");
            }
            else
            {
                _m_subPrefabWnd = new GGUIDemoSubPrefabWnd("gui/game_gui.unity3d", "prefab_hotfix_demo_sub_wnd",  hotfixWnd.subPrefabRoot);
                _m_subPrefabWnd.load();
                _m_subPrefabWnd.regLoadDoneDelegate(() =>
                {
                    _m_subPrefabWnd.showWnd();
                    _m_subPrefabWnd.showText("测试subPrefabWnd子窗口成功");
                });
            }

            ALUGUICommon.setLabelTxt(hotfixWnd.textTest, "测试窗口文本成功");

            if (_m_wItemWnd != null) 
                _m_wItemWnd.setItem(new NPCommonCostItem(ENPItemType.BAG_ITEM, 1011, 983));

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.setTexture(GRefdataCoreMgr.instance.npGeneral.avatar_snapshot_quality_icon);
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGuiDemoWnd===_onHideWnd");

            if (_m_demoSubWnd != null) 
                _m_demoSubWnd.hideWnd();

            if (_m_subPrefabWnd != null) 
                _m_subPrefabWnd.hideWnd();

            if (_m_wItemWnd != null) 
                _m_wItemWnd.hideWnd();

            if (_m_wQualityWnd != null) 
                _m_wQualityWnd.hideWnd();
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGuiDemoWnd===_onReset");

            if (_m_demoSubWnd != null) 
                _m_demoSubWnd.resetWnd();

            if (_m_subPrefabWnd != null) 
                _m_subPrefabWnd.resetWnd();

            if (_m_wItemWnd != null) 
                _m_wItemWnd.resetWnd();

            if (_m_wQualityWnd != null) 
                _m_wQualityWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGuiDemoWnd===_onDiscard");

            if (_m_demoSubWnd != null) 
                _m_demoSubWnd.discard();

            if (_m_subPrefabWnd != null) 
                _m_subPrefabWnd.discard();

            if (_m_wItemWnd != null) 
                _m_wItemWnd.discard();

            if (_m_wQualityWnd != null) 
                _m_wQualityWnd.discard();
        }

        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGuiDemoWnd===_onWndInitDoneHotfix");

            if(null == hotfixWnd)
                return;
            Debug.LogError(hotfixWnd.boolTest);
            Debug.LogError(hotfixWnd.floatTest);
            Debug.LogError(hotfixWnd.strTest);
            Debug.LogError(hotfixWnd.vector3Test);
            Debug.LogError(hotfixWnd.colorTest);
            Debug.LogError(hotfixWnd.intListTest.ToStringList());
            Debug.LogError(hotfixWnd.gameObjectListTest.ToStringList());

            if (hotfixWnd.monoCostItem != null) 
                _m_wItemWnd = new NPGGUIWndCommonItem(hotfixWnd.monoCostItem);
            
            if (hotfixWnd.subWndMono != null) 
                _m_demoSubWnd = new GGUIDemoSubWnd(hotfixWnd.subWndMono);

            if (hotfixWnd.image != null) 
                _m_wQualityWnd = new GGuiWndSprite(hotfixWnd.image);
        }
    }
}