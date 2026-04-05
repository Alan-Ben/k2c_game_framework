using NPEnum;

namespace GOE
{
    /// <summary>
    /// 数据集定义的固定强制红点数据节点
    /// </summary>
    public class RefdataRedTipNode : CommonForceRedTipNode
    {
        //对应红点数据集Id
        private NPRedTipRefObj _m_redTipRefObj;

        public RefdataRedTipNode(long _id) : base(_id.ToString())
        {
            _m_redTipRefObj = GRefdataCoreMgr.instance.redTipRefCore.getRef(_id);
        }

        public NPRedTipRefObj redTipRefObj { get { return _m_redTipRefObj; } }

        // 计数重载为未解锁功能不算计数
        protected override bool _needShow()
        {
            if (null == _m_redTipRefObj)
                return base._needShow();

            RedMonitorRefObj monitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(_m_redTipRefObj.id);
            if ((_m_redTipRefObj.function_type != ENPFunctionType.NONE && !NPPlayer.instance.funcUnlockComp.isFuncUnlock(_m_redTipRefObj.function_type)) 
                || (monitorRefObj != null && monitorRefObj.simple_unlock_id > 0 && !GCommon.isSimpleUnlock(monitorRefObj.simple_unlock_id)))
            {
                return false;
            }
            
            return base._needShow();
        }

        public override long getCount()
        {
            if (null == _m_redTipRefObj)
                return 0;

            RedMonitorRefObj monitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(_m_redTipRefObj.id);
            //未解锁按0返回，解锁了要重新计算子父节点
            if ((_m_redTipRefObj.function_type != ENPFunctionType.NONE && !NPPlayer.instance.funcUnlockComp.isFuncUnlock(_m_redTipRefObj.function_type))
                || (monitorRefObj != null && monitorRefObj.simple_unlock_id > 0 && !GCommon.isSimpleUnlock(monitorRefObj.simple_unlock_id)))
            {
                return 0;
            }
            
            return base.getCount();
        }
    }
}