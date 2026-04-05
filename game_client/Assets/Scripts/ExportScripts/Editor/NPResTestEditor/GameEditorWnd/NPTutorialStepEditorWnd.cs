using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using ALPackage;

using System.Text;
using GOE;

/******************
 * WCG战斗数据调整面板统一操作窗口
 **/
public class NPTutorialStepEditorWnd : _ANPBasicEditorWnd
{
    static Rect _s_rect = new Rect(0, 0, 600, 200);
    public static void showExportWnd(string _title)
    {
        NPTutorialStepEditorWnd window = (NPTutorialStepEditorWnd)EditorWindow.GetWindowWithRect(typeof(NPTutorialStepEditorWnd),
           _s_rect, true, _title);
        window.Show();
    }

    public NPTutorialStepEditorWnd()
    {
        regChildItem(new EditorWndInt("删除步骤：", -1, null, 300
            , (int _value) =>
            {
                UnityEngine.Object selectObj = Selection.objects[0];
                if (null == selectObj || !(selectObj is GameObject))
                {
                    UnityEngine.Debug.LogError("选中的：" + selectObj + " 不是GameObject!");
                    return;
                }

                NPGGUIMonoTutorialMainWnd selectTutorialWnd = ((GameObject)selectObj).GetComponent<NPGGUIMonoTutorialMainWnd>();
                if (null == selectTutorialWnd)
                {
                    UnityEngine.Debug.LogError("选中的：" + selectObj + " 不是WCGGGUIMonoTutorialWnd!");
                    return;
                }

                AssetDatabase.StartAssetEditing();

                selectTutorialWnd.stepList.RemoveAt(_value);

                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }));

        regChildItem(new EditorWndString("替换步骤（格式 srcIdx:tarIdx：", "", null, 300
            , (string _value) =>
            {
                //解析步骤
                if (null == _value || _value.Length <= 0)
                    return;

                //解析
                string[] strs = _value.Split(':');
                int srcIdx, tarIdx = 0;
                if(!int.TryParse(strs[0], out srcIdx)
                     || !int.TryParse(strs[1], out tarIdx))
                {
                    UnityEngine.Debug.LogError("格式不对! srcIdx:tarIdx!");
                    return;
                }

                UnityEngine.Object selectObj = Selection.objects[0];
                if (null == selectObj || !(selectObj is GameObject))
                {
                    UnityEngine.Debug.LogError("选中的：" + selectObj + " 不是GameObject!");
                    return;
                }

                NPGGUIMonoTutorialMainWnd selectTutorialWnd = ((GameObject)selectObj).GetComponent<NPGGUIMonoTutorialMainWnd>();
                if (null == selectTutorialWnd)
                {
                    UnityEngine.Debug.LogError("选中的：" + selectObj + " 不是WCGGGUIMonoTutorialWnd!");
                    return;
                }

                //判断长度
                if (selectTutorialWnd.stepList.Count <= srcIdx)
                {
                    UnityEngine.Debug.LogError("步骤数小于srcIdx： " + srcIdx + " !!");
                    return;
                }
                if (selectTutorialWnd.stepList.Count <= tarIdx)
                {
                    UnityEngine.Debug.LogError("步骤数小于tarIdx： " + tarIdx + " !!");
                    return;
                }

                AssetDatabase.StartAssetEditing();

                NPGGUIMonoTutorialWndStepObj srcObj = selectTutorialWnd.stepList[srcIdx];
                selectTutorialWnd.stepList[srcIdx] = selectTutorialWnd.stepList[tarIdx];
                selectTutorialWnd.stepList[tarIdx] = srcObj;

                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }));
    }

    //初始栏的GUI处理
    protected override void _initBarGUI()
    {
        //显示当前版本
        GUILayout.Label("引导界面UI便捷编辑面板");
        //空白
        GUILayout.Label("——————————————————");
        //空白
        GUILayout.Label("");
    }

    //获取本窗口大小
    protected override Rect _getWinRect()
    {
        return new Rect(10, 10, 600, 200);
    }
    //最小宽度
    protected override int _minWidth { get { return 450; } }

    protected string _makeItemHeader() { return ""; }
}
