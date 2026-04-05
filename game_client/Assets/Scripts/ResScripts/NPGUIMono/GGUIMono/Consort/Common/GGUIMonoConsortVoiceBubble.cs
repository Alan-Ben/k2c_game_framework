using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子配音气泡附加窗口
    /// </summary>
    public class GGUIMonoConsortVoiceBubble : _AALBasicUIWndMono
    {
        [ALHeader("是否随机播放(若不随机播放就是按照音效配置顺序播放)")]
        public bool playRandom = true;
        
        [ALInfo("播放时是否依赖上一次的播放结果" +
                  "\ntrue: 随机播放时下一次播放的不会与上一次播放的重复, 顺序播放时下一次播放的接着上一次播放的下一个; " +
                  "\nfalse: 随机播放时下一次播放的可能与上一次播放的重复, 顺序播放每次都会播放第一个")]
        public bool playBaseOnPre = true;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("打字机附加窗口")]
        public GGUIMonoTextTypewriter monoTypewriter;
        [ALHeader("文本显示完气泡再隐藏的延迟时间")]
        public float playDoneDelayHideBubble = 3f;
        [ALHeader("是否播放配音和气泡，false:只播放气泡，true:播放配音和气泡")]
        public bool isPlayVoiceAndBubble = false;
    }
}