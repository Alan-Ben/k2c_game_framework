
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum NPGGUIMonoChatInputterShowType
    {
        NONE,
        INPUT_WAITING, // 等待输入
        SEND_WAITING, // 等待发送
        CD, // 在发送cd中
        BANNED, // 被禁言了
        LOCK, // 未解锁
    }
    [Serializable]
    public class NPGGUIMonoChatInputterShowState
    {
        public NPGGUIMonoChatInputterShowType type;
        public List<GameObject> showList;
    }
    public class NPGGUIMonoChatInputter : _AALBasicUIWndMono
    {
        [ALHeader("输入框")] 
        public InputFieldEmoji inputField;
        [ALHeader("发送消息按钮")] 
        public GameObject btnSend;
        [ALHeader("发送表情按钮")] 
        public GameObject btnEmoji;
        [ALHeader("自定义添加列表按钮")] 
        public GameObject btnAddtion;
        
        [ALHeader("剩余冷却的倒计时文字")]
        public Text txtCD;

        [ALHeader("各种发送状态下显示的对象")] 
        public List<NPGGUIMonoChatInputterShowState> stateShowList;

        [ALHeader("聊天表情列表")] 
        public GGUIMonoChatEmotePage chatEmotePage;
        [ALHeader("自定义添加列表")] 
        public GGUIMonoCustomAddList customAddList;

        [ALHeader("发送消息的音效id")]
        public long sendMsgAudioId;

        /// <summary>
        /// 设置页面的显示参数
        /// </summary>
        public void setShowData(NPGGUIMonoChatInputterShowType _type)
        {
            int checkIndex = -1;
            for (int i = 0; i < stateShowList.Count; i++)
            {
                NPGGUIMonoChatInputterShowState state = stateShowList[i];
                if (state.type == _type)
                    checkIndex = i;
                else
                    ALUGUICommon.setGameObjEnable(state.showList, false);
            }
            
            if (checkIndex >= 0)
                ALUGUICommon.setGameObjEnable(stateShowList[checkIndex].showList, true);
        }
    }
}