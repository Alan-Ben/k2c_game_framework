using ALPackage;
using GOE;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 杰出者大厅主界面的容器wnd
    /// </summary>
    public class GGUIWndGraveMainFollowRoot : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoBuildingFollow>
    {
        private static GGUIWndGraveMainFollowRoot _g_instance = new GGUIWndGraveMainFollowRoot();
        [NotNull]public static GGUIWndGraveMainFollowRoot instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveMainFollowRoot();
                return _g_instance;
            }
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return GGUIMonoBuildingFollow.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingFollow.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}