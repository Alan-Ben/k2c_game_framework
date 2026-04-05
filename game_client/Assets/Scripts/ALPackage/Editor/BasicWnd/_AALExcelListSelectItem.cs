using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace ALPackage
{
    public abstract class _AALExcelListSelectItem : _IALExportMenuInterface
    {
        private List<string> _m_lExcelPathList;
        private bool _m_bInit;

        /** 存储路径的队列 */
        private int _m_iPathCount;

        public _AALExcelListSelectItem()
        {
            _m_bInit = false;
        }

        public virtual bool needShow { get { return true; } }

        //具体的gui绘制函数
        public void onGUI()
        {
            if (!_m_bInit)
            {
                _m_bInit = true;
                _m_lExcelPathList = ALExportDataCore.instance.getValueList(_dataKey);

                _m_iPathCount = _m_lExcelPathList.Count;
            }

            //输出文本信息
            GUILayout.Label(_text, GUILayout.Height(20));

            //输出队列长度
            GUILayout.BeginHorizontal();
            GUILayout.Label("文 件 数 量 ：", GUILayout.Width(80), GUILayout.Height(20));
            if (GUILayout.Button("加 1", GUILayout.Width(40), GUILayout.Height(20)))
            {
                _m_iPathCount++;

                //检测存储队列的长度
                while (_m_lExcelPathList.Count < _m_iPathCount)
                    _m_lExcelPathList.Add("");
            }
            string newPathCount = GUILayout.TextField(_m_iPathCount.ToString(), GUILayout.Height(20));
            if (!newPathCount.Equals(_m_iPathCount.ToString()))
            {
                //数量被更改则重新处理
                int.TryParse(newPathCount, out _m_iPathCount);

                //检测存储队列的长度
                while (_m_lExcelPathList.Count < _m_iPathCount)
                    _m_lExcelPathList.Add("");
            }
            GUILayout.EndHorizontal();

            //根据数量进行处理
            for (int i = 0; i < _m_iPathCount; i++)
            {
                string curPath = _m_lExcelPathList[i];

                //开始横向排布
                GUILayout.BeginHorizontal();

                //输出浏览按钮
                if (GUILayout.Button("Browser", GUILayout.Width(60), GUILayout.Height(25)))
                {
                    //获取原文件路径
                    string prePath = _defaultFolderPath;
                    if (null != curPath && File.Exists(curPath))
                    {
                        FileInfo fileInfo = new FileInfo(curPath);
                        prePath = fileInfo.DirectoryName;
                    }
                    //弹出窗口选择excel文件
                    string selectExcel = EditorUtility.OpenFilePanel("Select Excel Files", prePath, "xlsx");
                    if (null != selectExcel && selectExcel.Length > 0)
                    {
                        _m_lExcelPathList[i] = selectExcel;
                        //保存数据
                        ALExportDataCore.instance.setValue(_dataKey, _getCurWholeStr());
                    }
                }
                //输出文件夹路径
                string newValue = GUILayout.TextField(curPath, GUILayout.Height(25));
                if (!newValue.Equals(curPath))
                {
                    _m_lExcelPathList[i] = newValue;
                    //保存数据
                    ALExportDataCore.instance.setValue(_dataKey, _getCurWholeStr());
                }

                GUILayout.EndHorizontal();
            }
        }

        /*****************
         * 根据当前的路径队列，获取所有路径信息
         **/
        protected string _getCurWholeStr()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _m_iPathCount; i++)
            {
                if (i >= _m_lExcelPathList.Count)
                    break;

                if (0 == i)
                    builder.Append(_m_lExcelPathList[i]);
                else
                    builder.Append("#").Append(_m_lExcelPathList[i]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 返回默认选择位置
        /// </summary>
        /// <returns></returns>
        protected virtual string _defaultFolderPath
        {
            get
            {
                return string.Empty;
            }
        }

        /****************
         * 显示的说明
         **/
        protected abstract string _text { get; }
        protected abstract string _dataKey { get; }
    }
}
