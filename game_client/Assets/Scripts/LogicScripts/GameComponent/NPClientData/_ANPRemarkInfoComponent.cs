using System;
using System.Collections.Generic;
using System.Linq;
using GS2GC.p011_ClientDataOp;

namespace GOE
{
    /************
     * 数据存储接口的Component
     **/
    public abstract class _ANPRemarkInfoComponent : _ANPBasicPlayerComponent
    {
        private ENPClientDataType _m_eDataType;

        /** 是否初始化 */
        private bool _m_bIsInit;

        /** 初始化完成回调 */
        private Action _m_dInitDoneDelegate;

        public _ANPRemarkInfoComponent(NPPlayerComponentMgr _compMgr, ENPClientDataType _dataType)
            : base(_compMgr)
        {
            _m_eDataType = _dataType;

            _m_bIsInit = false;

            _m_dInitDoneDelegate = default(Action);
        }

        public ENPClientDataType dataType { get { return _m_eDataType; } }

        public bool isInit { get { return _m_bIsInit; } }

        #region component内部处理

        protected override void _dealInit()
        {
            //发送消息
            _sendRequest();
        }

        #endregion

        /********
         * 注册回调
         **/
        public void regDelegate(Action _initDone)
        {
            if(null == _initDone)
                return;

            if(_m_bIsInit)
                _initDone();
            else
                _m_dInitDoneDelegate += _initDone;
        }

        /************
         * 初始化数据
         **/
        private void _initData(byte[] _data)
        {
            try
            {
                if(_m_bIsInit)
                    return;

                _readData(_data);

                _m_bIsInit = true;
                //调用回调
                if(null != _m_dInitDoneDelegate)
                    _m_dInitDoneDelegate();
                _m_dInitDoneDelegate = null;
            }
            finally
            {
                //设置完成
                setInitDone();
            }
        }

        /****************
         * 发送请求数据
         **/
        protected void _sendRequest()
        {
            NPGSClientListener.sendRequestByLog(new GC2GS.p011_ClientDataOp.GC2GS_011_001_ReqQueryClientData((int)dataType),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_011_001_RetQueryClientData>((_info) =>
                {
                    if (null == _info || _info.getIndex() != (int)dataType)
                    {
                        Debug.LogError($"011_001 下发的index错误对不上{_info?.getIndex()}==={(int)dataType}");
                        return;
                    }
                    
                    //回包初始化数据
                    _initData(_info.getData());
                }));
        }

        /****************
         * 保存数据
         **/
        protected void _saveData()
        {
            NPGSClientListener.sendMsgByLog(NPGSWriter_011_ClientDataOp.make_002_ReqSaveClientData((int)dataType, _makeData()));
        }

        /** 构造数据 */
        protected abstract byte[] _makeData();

        /** 读取数据 */
        protected abstract void _readData(byte[] _data);

        /// <summary>
        /// 重置数据方法
        /// </summary>
        protected abstract void _resetRemarkInfo();
    }
}
