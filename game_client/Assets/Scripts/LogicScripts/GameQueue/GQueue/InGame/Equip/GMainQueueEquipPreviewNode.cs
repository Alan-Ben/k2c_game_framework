using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 藏品图鉴预览界面节点
    /// </summary>
    public class GMainQueueEquipPreviewNode : UIQueueBaseNode
    {
        private List<EquipRefObj> _m_lEquipRefList;
        private EquipRefObj _m_equipRef;

        public GMainQueueEquipPreviewNode(List<EquipRefObj> _equipRefList, EquipRefObj _curEquipRef) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_EQUIP_PREVIEW)
        {
            _m_lEquipRefList = new List<EquipRefObj>();
            if(_equipRefList != null)
                _m_lEquipRefList.AddRange(_equipRefList);
            _m_equipRef = _curEquipRef;
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
            if (_m_lEquipRefList == null || _m_equipRef == null)
                return;

            GMainGUIMainSceneEquipPreview.instance.regInitDelegate(() =>
            {
                int showIndex = 0;
                for (int i = 0; i < _m_lEquipRefList.Count; i++)
                {
                    if (_m_lEquipRefList[i].id == _m_equipRef.id)
                    {
                        showIndex = i;
                        break;
                    }
                }
                GMainGUIMainSceneEquipPreview.instance.initShowData(_m_lEquipRefList, showIndex);
                GUISceneMain.instance.showMainScene(GMainGUIMainSceneEquipPreview.instance);
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GMainGUIMainSceneEquipPreview.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();

            //先销毁旧的
            if (GMainGUIMainSceneEquipDetail.instance.isEntered)
                GMainGUIMainSceneEquipDetail.instance.quitScene();

            //设置窗口的资源id
            GGUIWndEquipDetail.instance.uiResId = 1803;
            GGUIWndEquipDetail.instance.nodeTag = UINodeTagConst.C_EQUIP_PREVIEW;
            GMainGUIMainSceneEquipPreview.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GMainGUIMainSceneEquipPreview.instance.quitScene();
        }
    }
}
