using ALPackage;

[System.Serializable]
public class NPAudioRefObj : _IALBasicRefObj
{
    public const long invaildAudioRefId = -1;//无效的音效配表id
    
    public long _refId { get { return id; } }

    public long id;

    public NPGAudioIndex audio_index;//资源索引
    public string audio_path; //资源相对路径

    public long duration;  //持续时间  毫秒

    public int min_cache_count;
    public int max_cache_count;

    public float fade_in_time; //淡入时间(音量由0至1的过渡时间)
    public float fade_out_time; //淡出时间(音量由1至0的过渡时间)

    public float fade_in_tar_value;//淡入峰值
    public long audio_group_id;//音效组id

    //是否不在离开屏幕时关闭，当此值为true时，离开屏幕也不会关闭声音
    public bool is_ignore_screen_distance;
    
    public bool is_language_audio;  // 是否是语言audio
    public bool is_async_audio;     // 是否是按需下载audio
}

public class NPGSOAudioRefSet : _TALSOBasicRefSet<NPAudioRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "audio"; } }
}
