using System;
using System.Collections.Generic;
using System.Linq;
using GS2GC.p011_ClientDataOp;

namespace GOE
{
    /************
     * 数据存储接口的基类
     **/
    public abstract class _ANPRemarkInfo
    {
        private ENPClientDataType _m_eDataType;

        /** 是否初始化 */
        private bool _m_bIsInit;

        /** 初始化完成回调 */
        private Action _m_dInitDoneDelegate;

        public _ANPRemarkInfo(ENPClientDataType _dataType)
        {
            _m_eDataType = _dataType;

            _m_bIsInit = false;

            _m_dInitDoneDelegate = default(Action);
        }

        public ENPClientDataType dataType { get { return _m_eDataType; } }

        public bool isInit { get { return _m_bIsInit; } }

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
            if(_m_bIsInit)
                return;

            _readData(_data);

            _m_bIsInit = true;
            //调用回调
            if(null != _m_dInitDoneDelegate)
                _m_dInitDoneDelegate();
            _m_dInitDoneDelegate = null;
        }

        /****************
         * 发送请求数据
         **/
        public void sendRequest(Action _doneDelegate)
        {
            //注册回调
            regDelegate(_doneDelegate);

            //发送请求
            sendRequest();
        }

        /****************
         * 发送请求数据
         **/
        public void sendRequest()
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
        public void saveData()
        {
            NPGSClientListener.sendMsgByLog(NPGSWriter_011_ClientDataOp.make_002_ReqSaveClientData((int)dataType, _makeData()));
        }

        /** 构造数据 */
        protected abstract byte[] _makeData();

        /** 读取数据 */
        protected abstract void _readData(byte[] _data);
        
        protected abstract void _resetRemarkInfo();
    }
}
