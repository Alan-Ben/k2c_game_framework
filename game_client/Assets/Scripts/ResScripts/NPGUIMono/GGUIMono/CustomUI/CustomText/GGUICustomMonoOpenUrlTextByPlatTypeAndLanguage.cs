using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 根据平台类型和语言设置翻译key
    /// </summary>
    public class GGUICustomMonoOpenUrlTextByPlatTypeAndLanguage : MonoBehaviour
    {
        [ALHeader("支持超链接的文本")]
        public TextExClickBaseOpenUrl textOpenUrl;
        [ALInfo("不同平台文本翻译KEY设置列表\n" +
              "如果plat和language两者都匹配，则返回都匹配的KEY\n" +
              "否则，返回plat匹配，language为NONE的KEY\n" +
              "再找不到，返回plat为NONE，language匹配的KEY\n" +
              "再找不到，返回两个都是NONE的KEY")]
        public List<PlatLanguageTextInfo> platTransKeyList;//不同平台对应的翻译key

        private void OnEnable()
        {
#if NP_GAME
            ALUGUICommon.setLabelTxt(textOpenUrl, TextTranslate.instance.getLanguage(
                _getKeyByPlatTypeAndLanguage(MainCameraMono.selfInstance.platType, GameSetting.instance.getCurrentLanguage())));            
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
        private string _getKeyByPlatTypeAndLanguage(EWCGPlatType _platType, ENPLanguage _language)
        {
            if (platTransKeyList == null || platTransKeyList.Count <= 0)
                return string.Empty;

            string platDefaultKey = string.Empty;
            string languageDefaultKey = string.Empty;
            string defaultKey = string.Empty;//最终的默认Key
            foreach (PlatLanguageTextInfo platKeyInfo in platTransKeyList)
            {
                if (platKeyInfo != null)
                {
                    if (platKeyInfo.platType == _platType && platKeyInfo.language == _language)
                    {
                        return platKeyInfo.transKey;
                    }
                    if (platKeyInfo.platType == _platType && platKeyInfo.language == ENPLanguage.NONE)
                    {
                        platDefaultKey = platKeyInfo.transKey;
                    }
                    if (platKeyInfo.platType == EWCGPlatType.NONE && platKeyInfo.language == _language)
                    {
                        languageDefaultKey = platKeyInfo.transKey;
                    }
                    if (platKeyInfo.platType == EWCGPlatType.NONE && platKeyInfo.language == ENPLanguage.NONE)
                    {
                        defaultKey = platKeyInfo.transKey;
                    }

                }
            }
            if (!string.IsNullOrEmpty(platDefaultKey))
            {
                return platDefaultKey;
            }
            if (!string.IsNullOrEmpty(languageDefaultKey))
            {
                return languageDefaultKey;
            }
            if (!string.IsNullOrEmpty(defaultKey))
            {
                return defaultKey;
            }

            Debug.LogError($"找不到匹配的Key：{_platType}, {_language}");
            return string.Empty;
        }
    }

    /// <summary>
    /// 平台语言对应的翻译key
    /// </summary>
    [System.Serializable]
    public class PlatLanguageTextInfo
    {
        public EWCGPlatType platType;
        public ENPLanguage language;
        public string transKey;
    }
}