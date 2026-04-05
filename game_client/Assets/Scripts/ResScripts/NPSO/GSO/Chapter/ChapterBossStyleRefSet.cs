using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;

/**************
 * boss样式表
 **/
[System.Serializable]
public class ChapterBossStyleRefObj : _IALBasicRefObj, _IChapterNodeStyle
{
    public long _refId { get { return id; } }
    public long id; // 唯一id
    public string boss_name;//boss名字
    public NPGTextureIndex boss_icon;//boss图标
    public NPGGoIndex video_boss_go_path;//视频资源
    public List<string> boss_talk_list;//boss对话
    public NPGGoIndex first_hit_video_boss_go_path;//1阶段受击视频资源
    public NPGGoIndex second_hit_video_boss_go_path;//2阶段受击视频资源
    public int video_animator_controller_id;//视频动画控制器id

    
    public NPGTextureIndex nodeMiniImg
    {
        get { return boss_icon; }
    }
    
    public NPGGoIndex videoGoPath
    {
        get { return video_boss_go_path; }
    }

    public NPGSpriteIndex maskUiImage
    {
        get { return null; }
    }
    
    public int videoAnimatorControllerId
    {
        get { return video_animator_controller_id; }
    }
    
    public string getRandomTalk()
    {
        return boss_talk_list.GetRandomItem();
    }
}

/**************
 * 关卡弹幕表
 **/
public class ChapterBossStyleRefSet : _TALSOBasicRefSet<ChapterBossStyleRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "chapter_boss_style"; } }
}
