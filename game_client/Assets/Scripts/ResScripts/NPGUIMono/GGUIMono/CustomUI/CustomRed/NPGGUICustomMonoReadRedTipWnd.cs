using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    ///  玩家自定义的设置红点已读窗口
    /// </summary>
    public class NPGGUICustomMonoReadRedTipWnd : MonoBehaviour
    {
        [ALHeader("非强制型红点Id")]
        [ALInfo("该mono用于设置非强制型红点为已读(数量置为0)状态")]
        public List<long> redTipIdList;
        
        //有效的时候处理已读
        private void OnEnable()
        {
            //到管理对象中进行处理
            _check();
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
        }

        protected void _check()
        {
            if(null == this || null == gameObject)
                return;

            if(null == redTipIdList || redTipIdList.Count == 0)
                return;
            
            if(gameObject.activeInHierarchy)
            {
#if NP_GAME
                foreach (var redRefID in redTipIdList)
                {
                    _ARedTipNode node = RedTipMgr.instance.getNodeByRefRedTipId(redRefID);
                    // 若这个节点是红点表中的非强制红点
                    if (node != null)
                    {
                        node.setCount(0);
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogError($"本窗口的{this.gameObject.name}：redTipList设置id错误", this.gameObject);
#endif
                    }
                }
#endif
            }
        }
    }
}