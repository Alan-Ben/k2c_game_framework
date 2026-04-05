using ALPackage;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 实验室跟随物体根节点
    /// </summary>
    public class GGUIWndTreasureHuntLabFollowItemRoot : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoBuildingFollow>
    {
        private static GGUIWndTreasureHuntLabFollowItemRoot _g_instance = new GGUIWndTreasureHuntLabFollowItemRoot();
        [NotNull]public static GGUIWndTreasureHuntLabFollowItemRoot instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTreasureHuntLabFollowItemRoot();
                return _g_instance;
            }
        }

        protected GGUIWndTreasureHuntLabFollowItemRoot() : base()
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