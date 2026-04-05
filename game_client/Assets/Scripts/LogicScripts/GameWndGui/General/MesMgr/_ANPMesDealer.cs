using ALPackage;

namespace GOE
{
    public abstract class _ANPMesDealer
    {
        private NPGAddQueueMesDealerNode _m_nDealNode;
        private bool _m_bIsDone = false;
        private bool _m_bIsShow = false;

        public bool isDone { get { return _m_bIsDone; } }
        public virtual bool needTransBk { get { return true; } }
        public virtual string uiNodeTag { get { return UINodeTagConst.C_MES_DEALER; } }

        /// <summary>
        /// 设置处理完成
        /// </summary>
        public void setDealerDone()
        {
            if (_m_bIsDone)
                return;

            //设置已完成
            _m_bIsDone = true;

            //触发函数
            _onDealerDone();

            //关闭节点
            if (null != _m_nDealNode)
            {
                QueueMgr.instance.forceCloseNode(_m_nDealNode);
                _m_nDealNode = null;
            }

            //处理下一个
            NPMesMgr.instance.setMesShowDone();
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        public void showMes()
        {
            //关闭原先的node
            if (null != _m_nDealNode)
            {
                QueueMgr.instance.forceCloseNode(_m_nDealNode);
                _m_nDealNode = null;
            }
            //开启一个新node
            _m_nDealNode = new NPGAddQueueMesDealerNode(this);

            //进入处理Node
            QueueMgr.instance.AddNode(_m_nDealNode);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            //关闭节点
            if (null != _m_nDealNode)
            {
                QueueMgr.instance.forceCloseNode(_m_nDealNode);
                _m_nDealNode = null;
            }

            //触发函数
            _onDiscard();

            //设置已完成
            _m_bIsDone = true;
        }

        /// <summary>
        /// 显示信息的处理
        /// </summary>
        public void dealShowMes()
        {
            if (_m_bIsShow)
                return;

            _m_bIsShow = true;
            _onShowMes();
        }

        /// <summary>
        /// 隐藏信息的处理
        /// </summary>
        public void dealHideMes()
        {
            if (!_m_bIsShow)
                return;

            _m_bIsShow = false;
            _onHideMes();
        }

        /// <summary>
        /// 关闭消息的处理，回退时，在dealHideMes前调用
        /// </summary>
        public virtual void dealCloseMes() { }
        /// <summary>
        /// 点击模糊背景的处理
        /// </summary>
        public virtual void dealClickTransBk() { }

        /// <summary>
        /// 显示消息的处理
        /// </summary>
        protected abstract void _onShowMes();

        /// <summary>
        /// 隐藏消息的处理
        /// </summary>
        protected abstract void _onHideMes();

        protected abstract void _onDealerDone();
        protected abstract void _onDiscard();
    }
}
