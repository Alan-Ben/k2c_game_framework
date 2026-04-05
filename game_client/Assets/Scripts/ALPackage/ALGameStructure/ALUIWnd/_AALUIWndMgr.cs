using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * ui窗口的管理对象，由于窗口有特别的reset操作，因此在本管理对象中重写了释放操作
 **/
namespace ALPackage
{
    public abstract class _AALUIWndMgr : _AALLoadObjMgr
    {
        /** 存放全局单例窗口存储对象 */
        private static _AALUIWndMgr _g_wmSingleWndMgr = null;

        /** 是否单例的窗口集合 */
        private bool _m_bIsSingleGroup;

        public _AALUIWndMgr(bool _isSingleGroup)
        {
            _m_bIsSingleGroup = _isSingleGroup;
        }

        /***************
         * 初始化相关窗口
         **/
        public void load(Action _loadedDelegate)
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] load.");
            }
            
            if(_m_bIsSingleGroup)
            {
                //是单例对象则需要判断全局对象是否需要卸载
                _AALUIWndMgr preSingleWndMgr = _g_wmSingleWndMgr;

                //开始本对象加载，并设置为全局单例对象
                _g_wmSingleWndMgr = this;
                base.loadAllObj(_loadedDelegate);

                //判断原先对象是否与本对象不同，如不同则需要卸载原对象
                //此处后卸载时避免部分窗口在卸载后马上加载的无谓损耗
                if (preSingleWndMgr != this && null != preSingleWndMgr)
                    preSingleWndMgr.discardAllObj();
            }
            else
            {
                //直接进行额外加载操作
                base.loadAllObj(_loadedDelegate);
            }
        }

        /**************
         * 重载基类函数
         **/
        public new void loadAllObj(Action _loadedDelegate)
        {
            load(_loadedDelegate);
        }

        /******************
         * 释放所有对象资源
         **/
        public override void discardAllObj()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UI][{this.GetType().Name}] discardAllObj.");
            }
            
            if (null == _m_lLoadObjList || _m_lLoadObjList.Count <= 0)
                return;

            //逐个开启加载
            for (int i = 0; i < _m_lLoadObjList.Count; i++)
            {
                _AALBasicLoadObj loadObj = _m_lLoadObjList[i];
                if (null == loadObj)
                    continue;

                //进行资源释放操作
                loadObj.discard();
            }
        }
    }
}
