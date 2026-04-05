
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeHandbookContainerItem : _AHotfixBaseMono
    {
        [HotfixMono("棋子图标")]
        public RawImage icon;
        [HotfixMono("棋子名称")]
        public Text name;
        [HotfixMono("详情按钮")]
        public GameObject btnDetail;
    }
}