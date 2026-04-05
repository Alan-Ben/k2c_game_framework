using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 展示历史用户名的展示节点
    /// </summary>
    public class LAddNodeLoginUserNameList : UIQueueBaseNode
    {
        //选择后的处理
        private Action<string> _m_dOnSelectName;
        //退出节点时调用的函数
        private Action _m_dOnQuitNode;

        public LAddNodeLoginUserNameList(Action<string> _onSelectname, Action _onQuitNode = null)
            : base(EUIQueueStageType.NONE)
        {
            _m_dOnSelectName = _onSelectname;
            _m_dOnQuitNode = _onQuitNode;
        }

        public override bool IsCanRollBackQuit { get { return true; } }
        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return false; } }
        public override bool isEnable { get { return true; } }


        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            _m_dOnQuitNode = null;
            _m_dOnSelectName = null;
        }

        public override void EnterNode()
        {
            //加载窗口再显示
            NPGGUIWndAcountGridWnd.instance.load();
            NPGGUIWndAcountGridWnd.instance.regLoadDoneDelegate(
                () =>
                {
                    NPGGUIWndAcountGridWnd.instance.showWnd();

                //获取用户信息列表
                List<InternalAccountInfo> cloneList = InternalAccountMgr.instance.cloneList();
                //倒序
                cloneList.Reverse();

                //设置数据
                NPGGUIWndAcountGridWnd.instance.setAccountList(cloneList
                        , (string _name) =>
                        {
                            if(null != _m_dOnSelectName)
                                _m_dOnSelectName(_name);
                            InternalAccountMgr.instance.setLastLoginTag(NPEnum.ENPLoginWayType.NONE, _name);

                        //退出本节点
                        QueueMgr.instance.forceCloseNode(this);
                        });
                });
        }

        public override void QuitNode()
        {
            //释放窗口
            NPGGUIWndAcountGridWnd.instance.discard();

            if(null != _m_dOnQuitNode)
                _m_dOnQuitNode();
        }
    }
}
