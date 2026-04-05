using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分组显示的groupitem
    /// </summary>
    public class GGUICustomMonoShowGroupItem : MonoBehaviour
    {
        [ALHeader("分组id")]
        public int groupId;
        // 优先级
        [ALHeader("优先级，小的优先显示")]
        public int priority;
        // 需要显示的go
        [ALHeader("控制显示的go，不要和本脚本挂在同一个go")]
        public List<GameObject> ctrGoList;
        
        #if NP_GAME


        private void OnEnable()
        {
            //注册到管理器中
            GGUICustomShowGroupItemMgr.instance.addItem(this);
        }

        private void OnDisable()
        {
            //从管理器移除
            GGUICustomShowGroupItemMgr.instance.removeItem(this);
        }

        /// <summary>
        /// 设置是否显示
        /// </summary>
        /// <param name="_isShow"></param>
        public void setItemShow(bool _isShow)
        {
            if(ctrGoList == null || ctrGoList.Count == 0)
                return;
            
            ALUGUICommon.setGameObjEnable(ctrGoList, _isShow);
        }
        #endif
    }
}