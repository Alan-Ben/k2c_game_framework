using System;
using System.Collections.Generic;

using UnityEngine;

/*********************
 * 加载对象的管理对象，通过向本对象注册需要加载的对象，可以直接通过本管理对象对所有子对象进行加载和释放操作
 **/
namespace ALPackage
{
    public abstract class _AALLoadObjMgr
    {
        /** 加载的对象列表 */
        protected List<_AALBasicLoadObj> _m_lLoadObjList;

        protected _AALLoadObjMgr()
        {
            //获取需要加载的队列
            _m_lLoadObjList = new List<_AALBasicLoadObj>();
            _getLoadObjList(_m_lLoadObjList);
        }

        /*******************
         * 加载所有对象
         **/
        public void loadAllObj(Action _loadedDelegate)
        {
            if (null == _m_lLoadObjList || _m_lLoadObjList.Count <= 0)
            {
                //直接调用回调
                if (null != _loadedDelegate)
                    _loadedDelegate();
                return;
            }

            //开启加载操作
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_lLoadObjList.Count);
            //注册加载完成操作
            stepCounter.regAllDoneDelegate(_loadedDelegate);

            //逐个开启加载
            for (int i = 0; i < _m_lLoadObjList.Count; i++)
            {
                _AALBasicLoadObj loadObj = _m_lLoadObjList[i];
                if (null == loadObj)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }

                //开启加载
                loadObj.load(stepCounter.addDoneStepCount);
            }
        }

        /******************
         * 释放所有对象资源
         **/
        public virtual void discardAllObj()
        {
            if (null == _m_lLoadObjList || _m_lLoadObjList.Count <= 0)
                return;

            //逐个开启加载
            for (int i = 0; i < _m_lLoadObjList.Count; i++)
            {
                _AALBasicLoadObj loadObj = _m_lLoadObjList[i];
                if (null == loadObj)
                    continue;

                loadObj.discard();
            }
        }

        /******************
         * 获取加载对象的队列
         **/
        protected abstract void _getLoadObjList(List<_AALBasicLoadObj> _recList);
    }
}
