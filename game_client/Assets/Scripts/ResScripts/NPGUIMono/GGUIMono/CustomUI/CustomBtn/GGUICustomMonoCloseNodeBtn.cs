using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUICustomMonoCloseNodeBtn : MonoBehaviour
    {
        public string nodeTag;
        
        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(gameObject, _onClickBtn);
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
#if NP_GAME
            QueueMgr.instance.forceCloseNodeByTag(nodeTag);
#endif
        }
    }
}