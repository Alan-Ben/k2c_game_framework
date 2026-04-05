using GOE;

namespace Hotfix
{
    public class _AHotfixBasicUIResBarMono : _AHotfixBaseMono
    {
        [HotfixMono("头像的资源id，对应ui_res_path表")]
        public long playerIconResId = 0;
        [HotfixMono("bar种类的资源id，对应ui_res_path表")]
        public long barResId = UIResPathConst.C_DEFAULT_RESBAR_RES_ID;
    }
}