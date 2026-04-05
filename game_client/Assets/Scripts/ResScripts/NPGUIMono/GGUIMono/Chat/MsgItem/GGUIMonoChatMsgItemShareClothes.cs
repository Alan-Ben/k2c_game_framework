
using ChatPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChatMsgItemShareClothes : GGUIMonoChatMsgItemShareCommon
    {
        [ALHeader("分享图片")]
        public RawImage texImage;
        
        
        public static string myAssetPath { get { return UIResPathAssistant.getAssetPath(1354); } }
        public static string myObjName { get { return UIResPathAssistant.getObjName(1354);} }
        
        public static string othersAssetPath { get { return UIResPathAssistant.getAssetPath(1355); } }
        public static string othersObjName { get { return UIResPathAssistant.getObjName(1355);} }


        #region 分享穿搭相关

        public static string myShareClothesAssetPath { get { return UIResPathAssistant.getAssetPath(1359); } }
        public static string myShareClothesObjName { get { return UIResPathAssistant.getObjName(1359);} }
        
        public static string othersShareClothesAssetPath { get { return UIResPathAssistant.getAssetPath(1360); } }
        public static string othersShareClothesObjName { get { return UIResPathAssistant.getObjName(1360);} }

        #endregion
    }
}