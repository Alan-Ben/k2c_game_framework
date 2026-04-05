using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class ConsortSendGiftDrawShowTypeShowSfxConfig
    {
        [ALHeader("类型")]
        public Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType showType;

        [ALHeader("特效父节点")]
        public Transform sfxParent;
        [ALHeader("显示特效id")]
        public long sfxId;
        [ALHeader("特效数上限")]
        public int sfxLimit;
    }
    
    /// <summary>
    /// 赠送礼物页面
    /// </summary>
    public class GGUIMonoUnlockConsortDetailInteractionSendGiftPage : _AGGUIMonoUnLockConsortDetailInteractionPageTabPageMono
    {
        [ALHeader("拥有的赠送item列表")]
        public GGUIMonoConsortBagItemContainer itemContainer;
        [ALHeader("item列表空的时候显示")] 
        public List<GameObject> listEmptyShow;
        [ALHeader("item列表空的时候隐藏")] 
        public List<GameObject> listEmptyHide;
        [ALHeader("列表为空的时候用于弹出获取途径的item")] 
        public NPCommonItem emptyListItem;

        [ALHeader("选中item的Mono")]
        public GGUIMonoConsortBagItem monoSelectItem;
        
        [ALHeader("赠送按钮")] 
        public GameObject btnGive;
        [ALHeader("批量赠送")]
        public GameObject btnBatchGive;
        
        [ALHeader("特效配置")]
        public List<ConsortSendGiftDrawShowTypeShowSfxConfig> drawShowTypeShowConfigList;

        public ConsortSendGiftDrawShowTypeShowSfxConfig getConsortSendGiftDrawShowTypeShowSfxConfig(Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType _type)
        {
            if(drawShowTypeShowConfigList == null)
                return null;

            return drawShowTypeShowConfigList.Find(_config => _config != null && _config.showType == _type);
        }
    }
}