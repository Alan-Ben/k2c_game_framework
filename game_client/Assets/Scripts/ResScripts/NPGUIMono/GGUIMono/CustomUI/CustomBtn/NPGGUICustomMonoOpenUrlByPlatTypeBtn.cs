using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 根据平台类型打开指定URL
    /// </summary>
    public class NPGGUICustomMonoOpenUrlByPlatTypeBtn : MonoBehaviour
    {
        [ALHeader("点击跳转按钮")]
        public GameObject Btn; // 按钮
        [ALHeader("不同平台跳转的url地址列表")]
        public List<PlatUrlInfo> platUrlInfoList;//不同平台跳转的url地址
        
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
#if NP_GAME
            //调用点击处理
            GCommon.openURL(_getUrlByEMGPlatType(MainCameraMono.selfInstance.platType));
#endif
        }

        private string _getUrlByEMGPlatType(EWCGPlatType _platType)
        {
            if (platUrlInfoList == null || platUrlInfoList.Count <= 0)
                return string.Empty;

            foreach (PlatUrlInfo platUrlInfo in platUrlInfoList)
            {
                if (platUrlInfo != null && platUrlInfo.platType == _platType)
                {
                    return platUrlInfo.url;
                }
            }

            return string.Empty;
        }
    }
    
    /// <summary>
    /// 平台跳转到的Url
    /// </summary>
    [System.Serializable]
    public class PlatUrlInfo
    {
        public EWCGPlatType platType;
        public string url;
    }
}