using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 打开指定URL
    /// </summary>
    public class NPGGUICustomMonoOpenUrlBtn : MonoBehaviour
    {
        [ALHeader("点击按钮")]
        public GameObject Btn; // 按钮
        [ALHeader("要打开的URL地址，手机上会弹出弹窗，让用户选择使用什么应用打开")]
        public string url;//url地址

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        /// <summary>
        ///  点击响应事件 
        /// </summary>
        protected void _onClickBtn(GameObject _go)
        {
            //调用点击处理
#if NP_GAME
            GCommon.openURL(url);
#endif
        }

    }
}