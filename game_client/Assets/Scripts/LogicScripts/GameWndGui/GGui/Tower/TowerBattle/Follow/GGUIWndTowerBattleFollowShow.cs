using ALPackage;
using GOE;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 放置跟随item的容器wnd
    /// </summary>
    public class GGUIWndTowerBattleFollowShow : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoBuildingFollow>
    {
        private static GGUIWndTowerBattleFollowShow _g_instance = new GGUIWndTowerBattleFollowShow();
        [NotNull]public static GGUIWndTowerBattleFollowShow instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattleFollowShow();
                return _g_instance;
            }
        }

        protected GGUIWndTowerBattleFollowShow()
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