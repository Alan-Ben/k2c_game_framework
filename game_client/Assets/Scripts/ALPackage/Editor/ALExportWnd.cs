using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace ALPackage
{
    public class ALExportWnd : EditorWindow
    {
        public static void showExportWnd<T>(Rect _rect, string _title) where T : ALExportWnd
        {
            //创建窗口
            T window = (T)EditorWindow.GetWindowWithRect(typeof(T), _rect, true, _title);
            window.Show();
        }

        private static ALExportWnd _g_instance = null;
        public static ALExportWnd instance { get { return _g_instance; } }

        /***********
         * 注册导出函数
         **/
        public static void regExportFunc(Action _action)
        {
            if (null == _action || null == _g_instance)
                return;

            _g_instance._m_eaiExportAllItem.regExportFunc(_action);
        }

        private List<_IALExportMenuInterface> _m_lMenuItemList;
        //滚动区域位置
        private Vector2 _m_vScrollViewPos;

        //导出所有的对象
        protected ALExportAllItem _m_eaiExportAllItem;

        //选择所有对象
        protected ALSelectItem _m_saiSelectAllItem;

        //忽略所有对象的差异校验
        protected ALSelectItem _m_saiForceAllItem;

        private ALFolderPathItem _m_fpiFolderPathItem;

        public ALExportWnd()
        {
            _g_instance = this;

            _m_lMenuItemList = new List<_IALExportMenuInterface>();

            _m_fpiFolderPathItem = new ALFolderPathItem("xml 文 件 导 出 路 径 ：", ALExportDataCore.xmlRootPathKey);

            _m_eaiExportAllItem = new ALExportAllItem();

            _m_saiSelectAllItem = new ALSelectItem("select all");

            _m_saiForceAllItem = new ALSelectItem("force all");

            _regMenuItem(new ALSplitLine());
        }

       


        /*********
         * gui处理函数
         **/
        void OnGUI()
        {
            _m_vScrollViewPos = GUILayout.BeginScrollView(_m_vScrollViewPos);

            //开始纵向布局
            EditorGUILayout.BeginVertical();

            //显示文本
            EditorGUILayout.LabelField("DL 资源导出窗口");
            _m_fpiFolderPathItem.onGUI();

            EditorGUILayout.BeginHorizontal();
            _m_eaiExportAllItem.onGUI();
            _m_saiSelectAllItem.onGUI();
            _m_saiForceAllItem.onGUI();
            EditorGUILayout.EndHorizontal();

            //开始逐项进行显示
            for (int i = 0; i < _m_lMenuItemList.Count; i++)
            {
                if (!_m_lMenuItemList[i].needShow)
                    continue;

                _m_lMenuItemList[i].onGUI();
            }

            //结束最外围纵向布局
            EditorGUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        /***********************
         * 注册资源导出菜单项
         **/
        protected void _regMenuItem(_IALExportMenuInterface _item)
        {
            if (null == _item)
                return;

            if (_m_lMenuItemList.Contains(_item))
            {
                UnityEngine.Debug.LogError("reg menu item multi times!");
                return;
            }

            _m_lMenuItemList.Add(_item);
        }
    }
}

