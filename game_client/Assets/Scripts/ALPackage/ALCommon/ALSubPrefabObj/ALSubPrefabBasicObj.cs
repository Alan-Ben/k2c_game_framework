using System;

using UnityEngine;

/**********************
 * 基本的窗口加载与处理对象
 **/
namespace ALPackage
{
    public class ALSubPrefabBasicObj : _ATALSubPrefabObj<ALBasicSubPrefabMono>
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        private _AALResourceCore _m_rcResCore;

        public ALSubPrefabBasicObj(string _assetPath, string _objName, Transform _parent, _AALResourceCore _resCore)
            : base(_parent)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;

            _m_rcResCore = _resCore;
        }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShow()
        {

        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHide()
        {

        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {

        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onInitDone()
        {

        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        /**************
         * 获取资源中本对象的名称
         **/
        protected override string _monoObjName { get { return _m_sObjName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return _m_rcResCore; } }
    }
}
