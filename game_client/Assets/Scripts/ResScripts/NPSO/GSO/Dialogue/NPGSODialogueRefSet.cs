using ALPackage;
using System;
using UnityEngine.Serialization;

namespace GOE
{
    /// <summary>
    /// 对话主表
    /// </summary>
    [Serializable]
    public class NPDialogueRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public long start_sentence_id;//第一句id
        public NPGShowcaseIndex showcase_index;//showcase舞台下标
        public bool can_skip;//是否可以跳过
        public bool is_main_node;//是否全屏界面，是的话其他ui会关掉，并且用mainCamera渲染
        public bool is_need_bk;//是否需要模糊背景，只有在非全屏界面情况下生效
        public bool can_review;//是否可回顾
        public bool can_auto_play;//是否可自动播放
        public long bgm_audio_id;//背景音乐id
    }

    public class NPGSODialogueRefSet : _TALSOBasicRefSet<NPDialogueRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "dialogue"; } }
    }
}