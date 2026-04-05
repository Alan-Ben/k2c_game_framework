using ALPackage;
using NPEnum;
using System.Collections.Generic;
using GOE;

/**************
 * 任务目标配表
 **/

[System.Serializable]
public class QuestTargetRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;

    public long step_id;//关联的步骤id

    public string target_txt;//目标文本

    public List<string> target_txt_args;//目标文本参数

    public int process_count;//进度目标值

    public _NPPlayerVariableSerializeInfo process_cur_count;//进度当前值 (高级公式)

    public List<string> add_msg_type_list;//客户端需要监听的枚举

    public long go_to_simple_tutorial_id;//跳转简单引导id
    public _NPPlayerEffectSerializeInfo go_to;//跳转效果

    public bool is_client_target;//是否客户端判断的目标值，true的情况下服务器会通过客户端提交的消息增加进度
    public List<ClientTriggerMsgInfo> client_trigger_msg;//在is_client_target有效的情况下，客户端监听增加进度的消息
    public long done_center_tip_id;//任务完成提示centerTipId（需要展示的才配置）

    /// <summary>
    /// 获取目标文本
    /// </summary>
    /// <returns></returns>
    public string getTargetStr()
    {
		return TextTranslate.instance.getLanguage(target_txt, target_txt_args);
    }
}

public class GSOQuestTargetRefSet : _TALSOBasicRefSet<QuestTargetRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/quest_refdata.unity3d"; } }
    public static string objName { get { return "quest_target"; } }
}
