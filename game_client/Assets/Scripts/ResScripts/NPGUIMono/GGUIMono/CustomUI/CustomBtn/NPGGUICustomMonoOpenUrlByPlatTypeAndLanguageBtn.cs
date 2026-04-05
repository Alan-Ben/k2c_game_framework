using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 根据平台类型和语言打开指定URL
    /// </summary>
    public class NPGGUICustomMonoOpenUrlByPlatTypeAndLanguageBtn : MonoBehaviour
    {
        [ALHeader("点击跳转按钮")]
        public GameObject Btn; // 按钮
        [ALInfo("不同平台跳转的url地址列表\n" +
              "如果plat和language两者都匹配，则返回都匹配的url\n" +
              "否则，返回plat匹配，language为NONE的url\n" +
              "再找不到，返回plat为NONE，language匹配的url\n" +
              "再找不到，返回两个都是NONE的url")]
        public List<PlatLanguageUrlInfo> platUrlInfoList;//不同平台跳转的url地址

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
            GCommon.openURL(_getUrlByEMGPlatType(MainCameraMono.selfInstance.platType, GameSetting.instance.getCurrentLanguage()));
#endif
        }

        /// <summary>
        /// 如果plat和language两者都匹配，则返回都匹配的url
        /// 否则，返回plat匹配，language为NONE的url
        /// 再找不到，返回plat为NONE，language匹配的url
        /// 再找不到，返回两个都是NONE的url
        /// 还找不到，报个错
        /// </summary>
        /// <param name="_platType"></param>
        /// <param name="_language"></param>
        /// <returns></returns>
        private string _getUrlByEMGPlatType(EWCGPlatType _platType, ENPLanguage _language)
        {
            if (platUrlInfoList == null || platUrlInfoList.Count <= 0)
                return string.Empty;

            string platDefaultUrl = string.Empty;
            string languageDefaultUrl = string.Empty;
            string defaultUrl = string.Empty;//最终的默认Url
            foreach (PlatLanguageUrlInfo platUrlInfo in platUrlInfoList)
            {
                if (platUrlInfo != null)
                {
                    if (platUrlInfo.platType == _platType && platUrlInfo.language == _language)
                    {
                        return platUrlInfo.url;
                    }
                    if (platUrlInfo.platType == _platType && platUrlInfo.language == ENPLanguage.NONE)
                    {
                        platDefaultUrl = platUrlInfo.url;
                    }
                    if (platUrlInfo.platType == EWCGPlatType.NONE && platUrlInfo.language == _language)
                    {
                        languageDefaultUrl = platUrlInfo.url;
                    }
                    if (platUrlInfo.platType == EWCGPlatType.NONE && platUrlInfo.language == ENPLanguage.NONE)
                    {
                        defaultUrl = platUrlInfo.url;
                    }

                }
            }
            if (!string.IsNullOrEmpty(platDefaultUrl))
            {
                return platDefaultUrl;
            }
            if (!string.IsNullOrEmpty(languageDefaultUrl))
            {
                return languageDefaultUrl;
            }
            if (!string.IsNullOrEmpty(defaultUrl))
            {
                return defaultUrl;
            }

            Debug.LogError($"找不到匹配的Url：{_platType}, {_language}");
            return string.Empty;
        }
    }

    /// <summary>
    /// 平台跳转到的Url
    /// </summary>
    [System.Serializable]
    public class PlatLanguageUrlInfo
    {
        public EWCGPlatType platType;
        public ENPLanguage language;
        public string url;
    }
}