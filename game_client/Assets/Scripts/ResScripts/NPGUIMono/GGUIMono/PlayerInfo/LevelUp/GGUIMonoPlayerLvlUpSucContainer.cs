using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家升级成功弹窗属性变化Container
    /// </summary>
    public class GGUIMonoPlayerLvlUpSucContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoPlayerLvlUpSucContainerItem>
    {
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("列表为空时隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}