using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤解锁弹窗
    /// </summary>
    public class GGUIMonoConsortSkinGet : _AALBasicUIWndMono
    {
        [ALHeader("获取皮肤提示")]
        public TextEx txtGainSkinTip;
        [ALHeader("获取皮肤提示key(需要一个参数, 皮肤名)")]
        public string txtGainSkinTipKey;

        [ALHeader("皮肤item")]
        public GGUIMonoConsortSkinItem monoSkinItem;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1413); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1413);} }
    }
}