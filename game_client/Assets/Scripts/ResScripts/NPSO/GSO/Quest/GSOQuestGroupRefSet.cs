using ALPackage;
using NPEnum;
using System.Collections.Generic;
using GOE;



/**************
 * 任务章节配表
 **/
 
[System.Serializable]
public class QuestGroupRefObj : _IALBasicRefObj
{
    public long _refId { get { return group_id; } }

    public long group_id;

    public string area_name;//区域名称

    public string group_sort_txt;//章节顺序

    public string group_name;//章节名称

    public bool open_need_show;//章节开启时是否需要弹窗

    public bool end_need_show;//章节结束时是否需要弹窗

    public long first_quest_id;//第一个任务id

    public long last_quest_id;//最后一个任务id
}

public class GSOQuestGroupRefSet : _TALSOBasicRefSet<QuestGroupRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/quest_refdata.unity3d"; } }
    public static string objName { get { return "quest_group"; } }
}
