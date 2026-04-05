using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;

/**************
 * boss样式表
 **/
[System.Serializable]
public class ChapterNodeStyleRefObj : _IALBasicRefObj, _IChapterNodeStyle
{
    public long _refId { get { return node_style_id; } }
    public long node_style_id; // 节点样式id
    public NPGTextureIndex node_mini_img;//小贴图
    public NPGGoIndex video_go_path;//视频资源
    public NPGSpriteIndex mask_ui_image;//遮罩资源贴图
    public int video_animator_controller_id;//视频动画控制器id
    public long forward_screen_sfx_id;//前屏特效id
    public long forward_td_sfx_id;//前进场景特效id

    public NPGTextureIndex nodeMiniImg
    {
        get { return node_mini_img; }
    }
    public NPGGoIndex videoGoPath
    {
        get { return video_go_path; }
    }

    public NPGSpriteIndex maskUiImage
    {
        get { return mask_ui_image; }
    }

    public int videoAnimatorControllerId
    {
        get { return video_animator_controller_id; }
    }
}

/**************
 * 关卡弹幕表
 **/
public class ChapterNodeStyleRefSet : _TALSOBasicRefSet<ChapterNodeStyleRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "chapter_node_style"; } }
}
