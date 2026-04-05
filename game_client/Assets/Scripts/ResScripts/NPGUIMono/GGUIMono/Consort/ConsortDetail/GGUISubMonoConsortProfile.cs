using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子简介子窗口
    /// </summary>
    public class GGUISubMonoConsortProfile : _AALBasicUIWndMono
    {
        [ALHeader("妃子名")]
        public TextEx consortName;

        [ALHeader("妃子称号")]
        public TextEx consortTitle;

        [ALHeader("出生地")]
        public TextEx birthplace;

        [ALHeader("妃子描述")]
        public TextEx desc;
    }
}