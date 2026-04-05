using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public abstract class _AALBaseResourceMgr<T> where T : UnityEngine.Object
    {
        /** get resource */
        private Dictionary<string, ALBaseRsourceObj<T>> _m_dResourceTable;

        private HashSet<ALBaseRsourceObj<T>> _m_lEmptyRelateResourceSet;

        protected _AALBaseResourceMgr()
        {
            _m_dResourceTable = new Dictionary<string, ALBaseRsourceObj<T>>();

            _m_lEmptyRelateResourceSet = new HashSet<ALBaseRsourceObj<T>>();
        }

        /*****************
         * 尝试在缓存中获取对象，无则从资源重新加载
         **/
        public virtual ALBaseRsourceObj<T> requestResource(string _path)
        {
            string lowPath = _path.ToLowerInvariant();

            if (_m_dResourceTable.ContainsKey(lowPath))
            {
                return _m_dResourceTable[lowPath].request();
            }
            else
            {
                //try to load the animation
                T obj = null;
                try
                {
                    obj = _loadResource(_path);

                    if (null == obj)
                        Debug.LogError("Can not find Resource: " + _path + " !\n");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Load Resource error!\n Path: " + _path + "\n" + ex.Message);
                    obj = null;
                }

                ALBaseRsourceObj<T> resouceObj = new ALBaseRsourceObj<T>(this, obj);
                _m_dResourceTable.Add(lowPath, resouceObj);

                //调用增加资源的事件函数
                _onAddResource(resouceObj);

                return resouceObj.request();
            }
        }

        /***************
         * 释放资源联系
         **/
        public virtual void releaseResource(ALBaseRsourceObj<T> _obj)
        {
            if (null == _obj)
                return;

            _obj.release();
        }

        /**************
         * 根据子类中具体函数判断是否需要释放资源，并按照断开联系的先后进行释放操作
         * 
         * 此操作由定时任务进行处理
         **/
        public void checkResourceReleaseFunction()
        {
            //判断是否需要释放内存
            if (_judgeNeedManageMem())
            {
                //空队列表示所有资源为有效资源，此处无法进行处理
                if (_m_lEmptyRelateResourceSet.Count <= 0)
                    return;

                List<ALBaseRsourceObj<T>> removedObjList = new List<ALBaseRsourceObj<T>>();
                foreach (ALBaseRsourceObj<T> removeObj in _m_lEmptyRelateResourceSet)
                {
                    //空队列表示所有资源为有效资源，此处无法进行处理
                    if (_m_lEmptyRelateResourceSet.Count <= 0)
                        break;

                    if (!_judgeNeedManageMem())
                        break;

                    //调用事件函数
                    _onFreeResource(removeObj);

                    //删除资源对象
                    _removeResourceObj(removeObj);

                    //将对象从队列中删除
                    removedObjList.Add(removeObj);
                }

                for (int i = 0; i < removedObjList.Count; i++)
                {
                    _m_lEmptyRelateResourceSet.Remove(removedObjList[i]);
                }
            }
        }

        /*****************
         * 删除对象操作
         * 需要判断资源等属性，并清理相关对象
         **/
        protected void _removeResourceObj(ALBaseRsourceObj<T> _obj)
        {
            if (_obj.getRelateCount() < 0)
                return;

            Debug.Log("Remove Resource Object: " + _obj.path);

            if (_obj.getRelateCount() > 0)
            {
                Debug.LogError("Try to remove useful resource object: " + _obj.path);
                return;
            }

            //remove table object
            _m_dResourceTable.Remove(_obj.path);

            //free object
            _obj.free();
        }

        /*****************
         * 添加对应对象到空引用列表中
         **/
        protected internal void _addObjToEmptyList(ALBaseRsourceObj<T> _obj)
        {
            _m_lEmptyRelateResourceSet.Add(_obj);
        }

        /*****************
         * 将对应对象从空引用列表中删除
         **/
        protected internal void _removeObjFromEmptyList(ALBaseRsourceObj<T> _obj)
        {
            _m_lEmptyRelateResourceSet.Remove(_obj);
        }

        /*****************
         * 加载资源函数
         **/
        protected abstract T _loadResource(string _path);

        /************
         * 释放资源的处理函数
         **/
        protected abstract void _dealFreeResource(ALBaseRsourceObj<T> _obj);

        /************
         * 判断是否需要整理内存对象（删除无引用对象）
         **/
        protected abstract bool _judgeNeedManageMem();

        /************
         * 当增加资源时调用的事件函数，在子类中必须要继承
         * 
         * 此时需要进行资源相关负载参考系数的计算
         **/
        protected abstract void _onAddResource(ALBaseRsourceObj<T> _obj);

        /************
         * 当删除资源时调用的事件函数，在子类中必须要继承
         * 
         * 此时需要进行资源相关负载参考系数的计算
         **/
        protected abstract void _onFreeResource(ALBaseRsourceObj<T> _obj);
    }
}
