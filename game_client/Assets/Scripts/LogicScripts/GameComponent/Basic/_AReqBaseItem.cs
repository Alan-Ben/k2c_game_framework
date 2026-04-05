using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;
using CommonEnum;
using System.Linq;
using Common.ChildObj;
using ALPackage;

namespace GOE
{
    // 需要请求数据的基类
    public abstract class _AReqBaseItem<T>
    {
        //当前是否请求了详细信息
        private bool _m_isReqDetail;
        //是否请求详情完成
        private bool _m_bIsDetailDone;

        //请求回调对象
        private Action<T> _m_dRequestDelegate;
        private T _m_data;
        
        //具体数据类
        public T data { get { return _m_data; } }
        
        //调用这个方法获取数据
        public void getValue(Action<T> _callBack)
        {
            if (_checkIsDone(_callBack))
                return;
            else
            {
                //已经请求数据了
                if(_m_isReqDetail)
                    return;
                
                _reqData();
            }
        }

        //强制更新数据
        public void forceGetValue(Action<T> _callBack)
        {
            _m_bIsDetailDone = false;
            _m_isReqDetail = false;
            getValue(_callBack);
        }

        //直接设置数据
        protected void _setValue(T _data)
        {
            _retData(_data);
        }
        
        //重置数据
        public void reset()
        {
            _m_data = default;
            _m_dRequestDelegate = null;

            _m_isReqDetail = false;
            _m_bIsDetailDone = false;

            _onResetData();
        }

        //请求数据
        private void _reqData()
        {
            _m_isReqDetail = true;
            _onReqData(_retData);
        }

        //数据回包后处理
        private void _retData(T _data)
        {
            _m_data = _data;
            
            _setDetailDone();
        }

        /// <summary>
        /// 设置加载完成
        /// </summary>
        private void _setDetailDone()
        {
            //设置数据加载完成
            _m_bIsDetailDone = true;

            if (null != _m_dRequestDelegate)
                _m_dRequestDelegate(_m_data);
            _m_dRequestDelegate = null;
        }

        private bool _checkIsDone(Action<T> _callBack)
        {
            //已经完成直接调用
            if (_m_bIsDetailDone)
            {
                if (null != _callBack)
                    _callBack(_m_data);
                return true;
            }

            //注册回调返回
            if (null == _m_dRequestDelegate)
                _m_dRequestDelegate = _callBack;
            else
                _m_dRequestDelegate += _callBack;

            return false;
        }
        
                
        //当请求数据时候
        protected abstract void _onReqData(Action<T> _doneAction);
        
        protected virtual void _onResetData() { }
    }
}
