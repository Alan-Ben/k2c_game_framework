using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    
    /// <summary>
    ///  自定义的物品item显示窗口
    /// </summary>
    public class GGUICustomMonoAchieveCommonItem : MonoBehaviour
    {
        [ALHeader("物品类型")]
        public ENPItemType itemType;
        [ALHeader("物品id")]
        public long itemId;
        [ALHeader("物品mono")]
        public NPGGUIMonoCommonItem itemMono;
        
        
        #if NP_GAME

        private NPCommonCostItem itemData;
        private NPGGUIWndCommonItem _m_itemWnd;
        
        private void OnEnable()
        {
            itemData = new NPCommonCostItem(itemType,itemId,1);
            if (null != itemMono)
            {
                _m_itemWnd = new NPGGUIWndCommonItem(itemMono);
            }
            
            _m_itemWnd?.showWnd();
            _m_itemWnd?.setItem(itemData);
        }


        private void OnDisable()
        {
            _m_itemWnd?.discard();
            _m_itemWnd = null;
        }
#endif
        
    }
}