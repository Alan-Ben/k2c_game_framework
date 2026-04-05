using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoAccessBagItem : _AALBasicUIWndMono
    {
        [ALHeader("物品item")]
        public NPGGUIMonoCommonItem monoItem;
        [ALHeader("物品描述")]
        public Text txtDesc;
        [ALHeader("使用按钮")]
        public GameObject btnUse;
        [ALHeader("全部使用按钮")]
        public GameObject btnUseAll;
        [ALHeader("全部使用按钮文本")]
        public Text txtUseAll;
        [ALHeader("全部使用的数量限制")]
        public long useAllLimit = 100;
        [ALHeader("物品数量为0时需要置灰的列表")]
        public List<MaskableGraphic> emptyGrayList;
        [ALHeader("使用一次后需要显示的GO列表")]
        public List<GameObject> goUseOneShowList;
        [ALHeader("使用一次后需要隐藏的GO列表")]
        public List<GameObject> goUseOneHideList;
        [ALHeader("全部使用完后需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("全部使用完后需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;

    }
}
