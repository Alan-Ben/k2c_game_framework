using GOE;
using UnityEngine;


namespace Hotfix
{
    /// <summary>
    /// 万能活动消耗商店仓库container item
    /// </summary>
    public class GGUIMonoRegularEventWarehouseContainerItem : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("道具")]
        public NPGGUIMonoCommonItem monoItem;
        [HotfixMonoAttribute("增加积分描述")]
        public TextEx txtUseAddPoint;
        [HotfixMonoAttribute("使用按钮")]
        public GameObject btnUse;
    }
}