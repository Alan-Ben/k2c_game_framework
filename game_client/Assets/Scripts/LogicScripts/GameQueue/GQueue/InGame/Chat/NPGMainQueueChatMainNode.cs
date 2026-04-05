using ChatPackage;

namespace GOE
{
    /// <summary>
    /// 聊天node
    /// </summary>
    public class NPGMainQueueChatMainNode : UIQueueBaseNode
    {
        private bool _m_isMainNode;
        private _AChatInfo _m_chatInfo;
        private readonly EChatPageType _m_showType;
        private bool _m_isFirstEnter = true;

        public NPGMainQueueChatMainNode(_AChatInfo _chatInfo, bool _isMainNode, EChatPageType _showType = EChatPageType.CHAT_INFO_LIST)
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_Main_ChatNode)
        {
            _m_isFirstEnter = true;
            _m_isMainNode = _isMainNode;
            _m_chatInfo = _chatInfo;
            _m_showType = _showType;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return _m_isMainNode; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return false; } }
        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            //当这个节点在队列的时候，多进入一次，保证在缓存，在close的时候需要多释放一次保证scene能正常释放
            GGUIWndChat.instance.load();
        }
        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            if (_m_isFirstEnter)
            {
                GGUIWndChat.instance.setShowData(_m_chatInfo);
                GGUIWndChat.instance.setCurPageType(_m_showType);
                _m_isFirstEnter = false;
            }
            
            if (_m_isMainNode)
            {
                GUISceneMain.instance.showMainWnd(GGUIWndChat.instance, () =>
                {
                    GGUIWndChat.instance.showWnd();
                    GGUIWndChat.instance.refreshWnd();
                });
            }
            else
            {
                GGUIWndChat.instance.regLoadDoneDelegate(() =>
                    {
                        GGUIWndChat.instance.showWnd();
                        GGUIWndChat.instance.refreshWnd();
                        GGUIWndChat.instance.rectTransform.SetAsLastSibling();
                    });
            }
        }
        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            //这里多进行一次释放，由于在enterqueue的时候多一次加载保证在缓存队列的时候scene不会被释放，因此这里多一个释放保证能正常释放
            GGUIWndChat.instance.discard();
        }
    }
}
