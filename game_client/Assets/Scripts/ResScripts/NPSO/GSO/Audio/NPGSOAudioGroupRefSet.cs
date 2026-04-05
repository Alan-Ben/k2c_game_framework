using ALPackage;

namespace GOE
{
    /// <summary>
    /// 音效分组数据
    /// </summary>
    [System.Serializable]
    public class NPAudioGroupRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        //音效组id
        public long id;
        //音效组名字，跟资源audioMixer配置走
        public string audio_group_name;
    }

    public class NPGSOAudioGroupRefSet : _TALSOBasicRefSet<NPAudioGroupRefObj>
    {
        /************
     * 资源加载路径
     **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "audio_group"; } }
    }
}