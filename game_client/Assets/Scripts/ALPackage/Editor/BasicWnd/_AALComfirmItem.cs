using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace ALPackage
{
    public abstract class _AALComfirmItem : _IALExportMenuInterface
    {
        public _AALComfirmItem()
        {
        }

        public virtual bool needShow { get { return true; } }

        //具体的gui绘制函数
        public void onGUI()
        {
            //按钮对象
            if (GUILayout.Button(_text, GUILayout.Height(30), GUILayout.Width(600)))
            {
                _dealComfirm();
            }
        }

        /****************
         * 显示的说明
         **/
        protected abstract string _text { get; }
        /**************
         * 具体的处理函数
         */
        protected abstract void _dealComfirm();

    }

    public abstract class _AALToggleItem : _IALExportMenuInterface {
        protected bool _m_bIsOn;
        private bool tempResult;

        public bool isOn { get { return _m_bIsOn; } }

        public _AALToggleItem () {
            _m_bIsOn = false;
        }

        public virtual bool needShow { get { return true; } }

        //具体的gui绘制函数
        public void onGUI()
        {
            tempResult = GUILayout.Toggle(_m_bIsOn, _text, GUILayout.Height(30));
            if (tempResult != _m_bIsOn)
            {
                _m_bIsOn = tempResult;
                _dealTgeChg(_m_bIsOn);
            }
        }

        /****************
         * 显示的说明
         **/
        protected abstract string _text { get; }
        /**************
         * 具体的处理函数
         */
        protected abstract void _dealTgeChg(bool _isOn);

        public void setToggleValue (bool _isOn) {
            if(_m_bIsOn == _isOn)
                return;

            _m_bIsOn = _isOn;
        }

    }
}
