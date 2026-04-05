namespace GOE
{
    public class GNodeMarsBuildQueueBuy : GAddNode_SingleWnd
    {
        public GNodeMarsBuildQueueBuy()
            : base(GGUIWndMarsBuildQueueBuy.instance, GGUIWndMarsBuildQueueBuy.instance.showWnd, UINodeTagConst.C_MARS_BUILD_QUEUE_BUY)
        {
        }


        public override void onEnterQueue()
        {
            base.onEnterQueue();
            
            NPPlayer.instance.foreverAddComponent.onForeverAddChanged += _onForeverChange;
        }
        public override void onClose()
        {
            NPPlayer.instance.foreverAddComponent.onForeverAddChanged -= _onForeverChange;
            
            base.onClose();
        }
        
        
        private void _onForeverChange(long _id, int _count)
        {
            if (_id == GRefdataCoreMgr.instance.npGeneral.mars_building_queue_forever_add_id)
                QueueMgr.instance.forceCloseNode(this);
        }
    }
}