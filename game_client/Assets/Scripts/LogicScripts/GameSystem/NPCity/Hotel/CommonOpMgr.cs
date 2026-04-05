namespace GOE
{
    /// <summary>
    /// 通用的点击操作管理
    /// </summary>
    public class CommonOpMgr
    {
        private static CommonOpMgr _g_instance = new CommonOpMgr();
        public static CommonOpMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CommonOpMgr();
                return _g_instance;
            }
        }

        private DevMonoCommonClickGo _m_lastGo;

        public CommonOpMgr()
        {
            
        }
        
        /// <summary>
        /// 选中对象
        /// </summary>
        /// <param name="_go"></param>
        public void setSelectedGo(DevMonoCommonClickGo _go)
        {
            //条件不满足或者当两个对象一致则不处理
            if(null != _go && _m_lastGo == _go)
                return;

            DevMonoCommonClickGo lastGo = _m_lastGo;

            //设置对象
            _m_lastGo = _go;

            if(!ReferenceEquals(null, lastGo))
                lastGo.disSelected();

            if (!ReferenceEquals(null, _m_lastGo))
            {
                _m_lastGo.onSelected();
                if (null != _m_lastGo && _m_lastGo.isAutoDisSelected)
                {
                    _m_lastGo.disSelected();
                    _m_lastGo = null;
                }
            }
        }
        
        /// <summary>
        /// 取消选中
        /// </summary>
        public void disSelectedGo(DevMonoCommonClickGo _go)
        {
            //条件不满足或者当两个对象不一致则不处理
            if(null == _go || _m_lastGo != _go)
                return;
            setSelectedGo(null);
        }
    }
}