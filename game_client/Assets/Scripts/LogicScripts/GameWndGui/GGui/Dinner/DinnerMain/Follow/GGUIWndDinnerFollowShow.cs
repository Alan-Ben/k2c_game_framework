using ALPackage;
using GOE;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 宴会跟随的wnd
    /// </summary>
    public class GGUIWndDinnerFollowShow : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoBuildingFollow>
    {
        private static GGUIWndDinnerFollowShow _g_instance = new GGUIWndDinnerFollowShow();
        [NotNull]public static GGUIWndDinnerFollowShow instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerFollowShow();
                return _g_instance;
            }
        }

        protected GGUIWndDinnerFollowShow()
            : base()
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return GGUIMonoBuildingFollow.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBuildingFollow.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
    }
}