using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 藏品详情界面节点
    /// </summary>
    public class GMainQueueEquipDetailNode : UIQueueBaseNode
    {
        private List<EquipInfo> _m_lEquipInfoList;
        private EquipInfo _m_equipInfo;

        public GMainQueueEquipDetailNode(List<EquipInfo> _equipInfoList, EquipInfo _curEquipInfo) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_EQUIP_DETAIL)
        {
            _m_lEquipInfoList = new List<EquipInfo>();
            if(_equipInfoList != null)
                _m_lEquipInfoList.AddRange(_equipInfoList);
            _m_equipInfo = _curEquipInfo;
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
        public override bool IsMainViewNode { get { return true; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            if (_m_lEquipInfoList == null || _m_equipInfo == null)
                return;

            GMainGUIMainSceneEquipDetail.instance.regInitDelegate(() =>
            {
                int showIndex = 0;
                for (int i = 0; i < _m_lEquipInfoList.Count; i++)
                {
                    if (_m_lEquipInfoList[i].dbId == _m_equipInfo.dbId)
                    {
                        showIndex = i;
                        break;
                    }
                }
                GMainGUIMainSceneEquipDetail.instance.initShowData(_m_lEquipInfoList, showIndex);
                GUISceneMain.instance.showMainScene(GMainGUIMainSceneEquipDetail.instance);
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GMainGUIMainSceneEquipDetail.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();

            //先销毁旧的
            if(GMainGUIMainSceneEquipPreview.instance.isEntered)
                GMainGUIMainSceneEquipPreview.instance.quitScene();

            //设置窗口的资源id
            GGUIWndEquipDetail.instance.uiResId = 1801;
            GGUIWndEquipDetail.instance.nodeTag = UINodeTagConst.C_EQUIP_DETAIL;
            GMainGUIMainSceneEquipDetail.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GMainGUIMainSceneEquipDetail.instance.quitScene();
        }
    }
}
