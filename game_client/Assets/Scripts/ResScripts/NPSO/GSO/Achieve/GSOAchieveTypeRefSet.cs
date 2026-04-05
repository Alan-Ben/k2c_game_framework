using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 成就类型数据
    /// </summary>
    [System.Serializable]
    public class AchieveTypeRefObj:_IALBasicRefObj
    {
        public long _refId { get { return (long)type; } }
        public EAchieveType type;//成就类型
        public NPGTextureIndex icon;//图标
        public string title;//标题
        public bool need_reorder;//步骤列表是否需要重新排序，TRUE:已完成的步骤排在后面 FALSE:只按步骤id排序
        public long center_tip_id;//完成成就时提示id（center_tips表id）
        public long red_tip_id;//红点id
        public bool is_show_in_achieve_wnd;// 是否在成就窗口显示
    }

    public class GSOAchieveTypeRefSet : _TALSOBasicRefSet<AchieveTypeRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/achieve_refdata.unity3d"; } }
        public static string objName { get { return "achieve_type"; } }
    }
}

