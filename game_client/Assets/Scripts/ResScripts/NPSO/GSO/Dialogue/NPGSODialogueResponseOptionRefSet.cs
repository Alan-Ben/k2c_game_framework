using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 对话选项表
    /// </summary>
    [Serializable]
    public class NPDialogueResponseOptionRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public string option_desc;//选项文本
        public _NPPlayerEffectSerializeInfo client_effect;//客户端效果
        public long ui_path_id;//样式pathid
        public long plot_dialog_ui_path_id;//剧情对话展示样式id
    }

    public class NPGSODialogueResponseOptionRefSet : _TALSOBasicRefSet<NPDialogueResponseOptionRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "response_option"; } }
    }
}