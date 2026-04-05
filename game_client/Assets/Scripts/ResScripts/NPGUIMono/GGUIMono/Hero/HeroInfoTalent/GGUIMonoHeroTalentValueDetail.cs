using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴资质详情界面
    /// </summary>
    public class GGUIMonoHeroTalentValueDetail : _AALBasicUIWndMono
    {
        [ALHeader("总资质")]
        public Text txtTotalValue;
        [ALHeader("技能加成")]
        public Text txtSkillAdd;
        [ALHeader("藏品加成")]
        public Text txtCollectionAdd;
        [ALHeader("家人加成")]
        public Text txtFamilyAdd;
        [ALHeader("服装加成")]
        public Text txtSkinAdd;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1009); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1009); } }
    }
}