using System.Collections.Generic;
using ALPackage;

namespace GOE
{

    public class GGUIMonoSubScrollNum : _AALBasicUIWndMono
    {
        [ALHeader("位数列表（第0位代表个位数第1位代表十位数，按顺序以此类推）")]
        public List<GGUIMonoSubScrollNumContainer> monoDigitList;
        [ALHeader("是否默认展示全部配置的位数")]
        public bool needShowAll;
    }
}

