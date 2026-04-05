using ALPackage;

namespace GOE
{
    /// <summary>
    /// 带bar的mono基类，会自动加载bar
    /// </summary>
    public abstract class _ANPBasicUIWndResBarMono : _AALBasicUIWndMono
    {
        [ALHeader("头像的资源id，对应ui_res_path表")]
        public long playerIconResId = 0;
        [ALHeader("bar种类的资源id，对应ui_res_path表")]
        public long barResId = UIResPathConst.C_DEFAULT_RESBAR_RES_ID;
    }   
}