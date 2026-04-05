
using GOE;
using UnityEngine;

namespace Hotfix
{
    public class GGUIMonoNumMergeHandbook : _AHotfixBaseMono
    {
        [HotfixMono("关闭按钮")]
        public GameObject btnClose;
        [HotfixMono("图鉴棋子容器")]
        public GGUIHotfixCommonMono monoItemContainer;
    }
}