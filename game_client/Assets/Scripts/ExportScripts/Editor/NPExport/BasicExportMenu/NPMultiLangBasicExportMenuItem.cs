using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using ALPackage;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 用于多个表内保存多语言数据的方式，表格命名规则：*****_zh_cn，第一个类型是模板类,  第二个类型是实际游戏中运用的类,  第三个类型是表的集合类
    /// </summary>
    /// <typeparam name="TTemp"></typeparam>
    /// <typeparam name="Tobj"></typeparam>
    /// <typeparam name="TMap"></typeparam>
    public abstract class NPMultiLangBasicExportMenuItem<TTemp, Tobj, TMap> : NPBasicExportMenuItemEX where TTemp : new() where Tobj : _IALBasicRefObj, new() where TMap : _TALSOBasicRefSet<Tobj>, new()
    {
        private List<ENPLanguage> _m_languages;
        public NPMultiLangBasicExportMenuItem(string _exportName
            , List<ENPLanguage> _languages
            , ENPExportSettingEnum _exprotEnum
            , Func<string, string, bool> _judgeCanShowFunc)
            : base(_exportName/*之类使用_exportName当作tag来搜索*/, _exprotEnum, _judgeCanShowFunc)
        {
            _m_languages = _languages;
            _regSubItem(new NPTextButtonItem(_exportName, 15, "copy", () => { GUIUtility.systemCopyBuffer = _exportName; }));
            _regSubItem(new ALSplitLine());
            _regSubItem(new ALExcelPathItem("数 据 信 息 Excel 文 件：", exportEnum.ToString()));
            _regSubItem(new ALTextItem("选择的页签名字。", 15));
            _regSubItem(new NPInputItem(exportEnum.ToString(), 25));

            _additionEditorOption();

            _regSubItem(new NPComfirmItem("导 出", _exExport));

            NPExportSettingMgr.instance.regSubExportSetting(exportEnum, exportGeneralRefSet, isSelect, _exportName, _judgeCanShowFunc);
        }

        protected override void _exExport()
        {
            exportGeneralRefSet();
        }

        /******************
     * 具体的导出操作
     **/
        protected void exportGeneralRefSet()
        {
            Stopwatch timeWatch = new Stopwatch();
            timeWatch.Start();

            //读取对应的excel文件
            string excelPath = ALExportDataCore.instance.getValue(exportEnum.ToString());
            if (string.IsNullOrEmpty(excelPath))
            {
                Debug.LogError(string.Format("【{0}】的ExcelPath是空", exportEnum));
                return;
            }
            if (!File.Exists(excelPath))
            {
                Debug.LogError(string.Format("【{0}】的ExcelPath填写错误: {1}", exportEnum, excelPath));
                return;
            }

            // 查找每种语言的表进行导出
            foreach (var langEnum in _m_languages)
            {
                var lang = langEnum.ToString().ToLowerInvariant();
                string tabName = getTableName(langEnum, NPInputTabData.instance.getValue(exportEnum.ToString()));
                //开始读取excel文件
                List<TTemp> tempList = NPExportWnd.readXls<TTemp>(
                    excelPath
                    , tabName
                    , exportEnum
                    , _readRefInfo, false);

                //输出txt文件
                NPExportWnd.WriteToTxtFile(excelPath, tabName, exportEnum);

                if (tempList.Count == 0)
                {
                    Debug.LogError($"{exportEnum}数据为空,请注意.{tabName}");
                    continue;
                }

                var assetPath = getAssetPath(langEnum);
                var nobjName = getAssetObjName(langEnum);

                //空表只是为了导出txt文件，不生成asset
                if (assetPath.Equals(NPSOEmptyRefSet.assetPath) && nobjName.Equals(NPSOEmptyRefSet.objName))
                {

                }
                else
                {
                    TMap refSet = ScriptableObject.CreateInstance<TMap>();
                    refSet.refList = new List<Tobj>(_exchangeTemplate(tempList));
                    if (Application.isPlaying) // 如果在运行中尝试直接覆盖
                    {
                        tryCoverOldData(refSet, nobjName);
                        ALBasicExportFunction.exportAsset(refSet, nobjName, assetPath, "unity3d");
                    }
                    else
                    {

                        ALBasicExportFunction.exportAsset(refSet, nobjName, assetPath, "unity3d");
                    }
                }

                tempList.Clear();
                Debug.LogWarning(string.Format("{0} : Ref Set 导出完成!! {1} : {2}， 用时：{3}ms", tabName, assetPath, nobjName, timeWatch.ElapsedMilliseconds));
            }
            timeWatch.Stop();
            //输出导出完成
            Debug.LogWarning(string.Format("{0} : Ref Set 导出完成!!  用时：{1}ms", excelPath, timeWatch.ElapsedMilliseconds));
        }

        protected TTemp obj;

        public void _readRefInfo(TTemp _tmpInfo, int _line, Dictionary<string, string> _lineData)
        {
            lineValue = _lineData;
            line = _line;
            obj = _tmpInfo;
            _readRefInfo();
            lineValue = null;
        }
        public abstract void _readRefInfo();

        public abstract List<Tobj> _exchangeTemplate(List<TTemp> _tempList);

        protected virtual void _additionEditorOption() { }

        // 尝试直接覆盖现有的SO
        private bool tryCoverOldData(TMap _refSet, string _objName)
        {
            // 判断文件夹存不存在
            string folderPath = _objName.Substring(0, _objName.LastIndexOf('/') + 1);
            string assetPath = "/Resources/Refdata/__DLExport/" + folderPath;
            if (!Directory.Exists(Application.dataPath + assetPath))
                return false;

            TMap refdataAsset = AssetDatabase.LoadAssetAtPath<TMap>("Assets/Resources/Refdata/__DLExport/" + _objName + ".asset");
            if (refdataAsset == null)
                return false;

            // 遍历新数据，如果旧数据里有这个id则覆盖，没有则添加
            for (int i = 0; i < _refSet.refList.Count; i++)
            {
                Tobj refdataObj = refdataAsset.refList.Find((obj) => obj._refId == _refSet.refList[i]._refId); // 先尝试查找这个数据
                if (refdataObj == null)
                    refdataAsset.refList.Add(refdataObj); // 如果没有，则添加
                else
                    _copyValueToOldObj(refdataObj, _refSet.refList[i]); // 如果有，把值全部赋过去
            }

            // 遍历旧数据，去新数据里查找，如果没有找到，则移除
            for (int i = 0; i < refdataAsset.refList.Count; i++)
            {
                Tobj refdataObj = _refSet.refList.Find(obj => obj._refId == refdataAsset.refList[i]._refId);
                if (refdataObj == null)
                {
                    refdataAsset.refList.RemoveAt(i);
                    i--;
                }
            }
            EditorUtility.SetDirty(refdataAsset);
            AssetDatabase.SaveAssets();
            return true;
        }

        private void _copyValueToOldObj(Tobj _oldValue, Tobj _newObj)
        {
            Type objType = typeof(Tobj);
            FieldInfo[] fieldList = objType.GetFields(); // 赋值多有的Field
            for (int i = 0; i < fieldList.Length; i++)
            {
                fieldList[i].SetValue(_oldValue, fieldList[i].GetValue(_newObj));
            }
        }

        protected abstract string getAssetPath(ENPLanguage _language);
        protected abstract string getAssetObjName(ENPLanguage _language);
        protected abstract string getTableName(ENPLanguage _language, string _tableName);

    }
}