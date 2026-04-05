using System;
using System.Collections;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 下一步触发消息类型
    /// </summary>
    public enum ENextStepTriggerMsgType
    {
        NONE,
        DIALOG_END,//对话结束
    }
    
    /// <summary>
    /// 下一步触发器类型
    /// </summary>
    public enum ENextStepTriggerType
    {
        CLICK,//点击触发
        MSG,//消息触发
        AUTO,//自动触发
    }
    
    /// <summary>
    /// 下一步触发器
    /// </summary>
    [Serializable]
    public class NextStepTrigger
    {
        [ALHeader("触发类型")]
        public ENextStepTriggerType triggerType;

        //[ALInfo("触发类型为ENextStepTriggerType.CLICK使用")]
        [ALHeader("触发按钮")]
        public GameObject btnTrigger;

        //[ALInfo("触发类型为ENextStepTriggerType.MSG使用")]
        [ALHeader("触发的消息类型")]
        public ENextStepTriggerMsgType triggerMsgType;

        //[ALInfo("触发类型为ENextStepTriggerType.AUTO使用")]
        [ALHeader("自动触发的延迟时间")]
        public float autoTriggerDelayTime;
        
        [ALHeader("触发后执行效果")]
        public string triggerFunc;
    }

    [Serializable]
    public class NextStepTriggerList
    {
        [ALHeader("是否所有触发器都触发才进入下一步(若为true, 所有触发器触发后才会进入下一步, 否则只要有一个触发器触发就会进入下一步)")]
        public bool allTriggerToNextStep;

        [ALHeader("触发器列表")]
        public List<NextStepTrigger> triggerList;
    }

    [Serializable]
    public class MiniQTEGameStepObj
    {
        [ALHeader("第一个变量是name，在编辑步骤的时候，在列表会默认显示这个名字方便编辑")]
        public string stepName;
        [ALHeader("本步骤的父节点go,可以用来判断是否拖错了")]
        public GameObject selfGo;
        
        [ALHeader("进入本阶段时需要有效的Go对象")]
        public List<GameObject> enableGo;
        [ALHeader("进入本阶段时需要无效的Go对象")]
        public List<GameObject> disableGo;
        
        [ALHeader("本阶段音源对象索引")]
        public NPGAudioIndex stepVoiceIndex;
        
        [ALHeader("在开始本步骤之前执行的效果")]
        public string beforeStartStepEffect;
        
        [ALHeader("下一步触发器列表")]
        public NextStepTriggerList nextStepTriggerList;

        [ALHeader("本步骤完成后执行的效果")]
        public string stepDoneEffect;
        [ALHeader("本步骤执行完成后, 到下一步的延迟时间（秒）")]
        public float toNextStepDelayTime;
        
        [ALHeader("退出本阶段时需要有效的Go对象")]
        public List<GameObject> exitEnableGo;
        [ALHeader("退出本阶段时需要无效的Go对象")]
        public List<GameObject> exitDisableGo;
        
        //CustomDrawer用的，用于存储是否展示所有字段，还是只展示有数值的字段
        public bool _showAllFields = false;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class GGUIMonoMiniQTEGame : _AALBasicUIWndMono
    {
        [ALHeader("步骤列表")]
        public List<MiniQTEGameStepObj> stepObjList;
        
        [TextArea(2,6)]
        [ALHeader("结束引导时执行脚本。使用PlayerEffect字符串方式解析")]
        public string gameEndFunc;

        [ALHeader("延迟结束引导时间")]
        public float endDelayTime;
    }
}