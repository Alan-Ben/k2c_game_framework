using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子列表
    /// </summary>
    public class GGUIMonoConsortListGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoConsortListGridItem>
    {
        [ALHeader("被邀约的妃子item需要移动到的对于可视区域上边界的位置")]
        public float byInviteConsortItemMoveToPosY;
        [ALHeader("移动时间")]
        public float smoothMoveTime;

        [ALHeader("偶数索引item位置偏移")]
        public Vector2 evenIdxItemOffset;
        [ALHeader("奇数索引item位置偏移")]
        public Vector2 oddIdxItemOffset;

        [ALHeader("已解锁妃子bar路径信息")]
        public NPCommonAssetPathInfo unlockConsortBarAssetPathInfo;
        [ALHeader("未解锁妃子bar路径信息")]
        public NPCommonAssetPathInfo lockConsortBarAssetPathInfo;
        
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}