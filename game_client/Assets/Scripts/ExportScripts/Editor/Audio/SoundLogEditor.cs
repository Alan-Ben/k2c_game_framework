using UnityEditor;
using UnityEngine;
// using H2Framework.ForXLua;
using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using GOE;

public class SoundLogEditor : EditorWindow
{
    [MenuItem("Tools/音频/音频Log面板")]
    private static void Display()
    {
        var _win = GetWindow<SoundLogEditor>("Sound Logger", true);
        _win.Show();
    }

    private static readonly string g_skinPath = "Assets/Scripts/ExportScripts/Editor/Audio/SoundLogSkin.guiskin";
    private static readonly ENPExportSettingEnum g_exportEnum = ENPExportSettingEnum.AUDIO; //指定类型，用于获取Excel路径
    private static readonly int g_excel_idIndex =0;   //excel表内id列索引
    private static readonly int g_excel_descIndex =1; //excel表内desc列索引
    private readonly GUIContent HEADER_ID = new GUIContent("Id");
    private readonly GUIContent HEADER_INSTRUCTION = new GUIContent("Description");
    private readonly GUIContent HEADER_CREATE_TIME = new GUIContent("CreateTime");
    private readonly GUIContent HEADER_DURING_TIME = new GUIContent("During");
    private readonly GUIContent HEADER_SOURCE = new GUIContent("Source");
    private readonly GUIContent HEADER_PREFAB = new GUIContent("prefab");
    
    private Vector2 m_ScrollVec2;
    private GUISkin skin;
    private string draggingString = "";
    private int cid;

    private const int ROW_HEIGHT = 20;      //Log行高
    private int maxShowCount = 200;        //最大显示行数
    private string maxShowCountCache = "200";
    private string filterString = "";
    private bool showVideoLog = true;
    private bool showAudioLog = true;

    private Event curEvent;

    private string[,] excelData=new string[0,0];
    /// <summary>
    /// 窗口开启处理
    /// </summary>
    private void OnEnable()
    {
        skin = AssetDatabase.LoadAssetAtPath<GUISkin>(g_skinPath);
        
        //获取Excel表数据
        string excelPath=ALExportDataCore.instance.getValue(g_exportEnum.ToString());
        string excelWorksheetName=NPInputTabData.instance.getValue(g_exportEnum.ToString());
        excelData = ExcelCommon.LoadExcel(excelPath, excelWorksheetName);
        
        EditorApplication.update += OnUpdate;
    }

    /// <summary>
    /// 绘制根循环
    /// </summary>
    private void OnUpdate()
    {
        Repaint();
    }

    /// <summary>
    /// 窗口关闭处理
    /// </summary>
    private void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }
    
    private List<AudioDebugMgr.AudioDebugInfo> _m_tempList = new List<AudioDebugMgr.AudioDebugInfo>();

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                AudioDebugMgr.Instance.ClearSoundLog();
            }

            GUILayout.Space(10);
            EditorGUILayout.LabelField("MaxLog", EditorStyles.label, GUILayout.Width(50));
            maxShowCountCache = EditorGUILayout.TextField(maxShowCountCache, EditorStyles.miniTextField);
            if (GUILayout.Button("Confirm", EditorStyles.toolbarButton))
            {
                if (!string.IsNullOrEmpty(maxShowCountCache))
                {
                    if(!int.TryParse(maxShowCountCache, out maxShowCount))
                    {
                        maxShowCount = 1000;
                    }
                }
            }

            GUILayout.Space(10);
            EditorGUILayout.LabelField("filter", EditorStyles.label, GUILayout.Width(30));
            filterString = EditorGUILayout.TextField(filterString, EditorStyles.miniTextField);
            showAudioLog = GUILayout.Toggle(showAudioLog, "音频Log");
            showVideoLog = GUILayout.Toggle(showVideoLog, "视频Log");
        }

        EditorGUILayout.EndHorizontal();

        DrawHeader();


        UpdateInstruction(); //根据Excel表内容刷新AudioDebugMgr.audioDebugInfoList内的instruction
        _m_tempList.Clear();
        if (!string.IsNullOrEmpty(filterString))
        {
            AudioDebugMgr.audioDebugInfoList.ForEach(
                (_info) =>
                {
                    if (_info.source.ToLowerInvariant().Contains(filterString.ToLowerInvariant()))
                    {
                        _m_tempList.Add(_info);
                    }
                });
        }
        else
        {
            _m_tempList.AddRange(AudioDebugMgr.audioDebugInfoList);
        }

        for (var i = _m_tempList.Count - 1; i >= 0; i--)
        {
            var debugInfo = _m_tempList[i];
            if (debugInfo == null)
            {
                _m_tempList.RemoveAt(i);
                continue;
            }
            if(!showAudioLog && debugInfo.instruction != AudioDebugMgr.Instance.videoRefId)
                _m_tempList.RemoveAt(i);
            if(!showVideoLog && debugInfo.instruction == AudioDebugMgr.Instance.videoRefId)
                _m_tempList.RemoveAt(i);
        }

        var _count = _m_tempList.Count;
        _m_tempList = _m_tempList
            .OrderBy(x => x == null)             // false (非空) 排在前面，true (空) 排在后面
            .ThenBy(x => x?.IsDone() ?? false)   // 接着按 IsDone 状态排序
            .ThenByDescending(x => x?.createTs ?? 0f) // 最后按时间戳倒序
            .ToList();

        if(_count <= 0)
        {
            return;
        }
        if(_m_tempList.Count > maxShowCount)
        {
            int needDelete = _m_tempList.Count - maxShowCount;
            for(int i = 0; i<needDelete; ++i)
            {
                _m_tempList.RemoveAt(_m_tempList.Count - 1);
            }
        }
        m_ScrollVec2 = EditorGUILayout.BeginScrollView(m_ScrollVec2);
        _m_tempList.ForEach((_info) =>
        {
            DrawRow(_info);
        });
        EditorGUILayout.EndScrollView();

        //快捷复制
        // Vector2 gridIndex;
        // curEvent = Event.current;
        // if(GetGridUnderMouse(_list, out gridIndex))
        // {
        //     if(curEvent.GetTypeForControl(cid) == EventType.MouseDown)
        //     {
        //         draggingString = GetGridString(_list, gridIndex);
        //         GUIUtility.systemCopyBuffer = draggingString;
        //     }
        // }
    }

    /// <summary>
    /// 绘制表头
    /// </summary>
    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            var _alignment = EditorStyles.toolbarButton.alignment;
            EditorStyles.toolbarButton.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(HEADER_ID, EditorStyles.toolbarButton, GUILayout.Width(100));
            GUILayout.Label(HEADER_CREATE_TIME, EditorStyles.toolbarButton, GUILayout.Width(100));
            GUILayout.Label(HEADER_DURING_TIME, EditorStyles.toolbarButton, GUILayout.Width(100));
            GUILayout.Label(HEADER_PREFAB,EditorStyles.toolbarButton, GUILayout.Width(200));
            GUILayout.Label(HEADER_SOURCE, EditorStyles.toolbarButton, GUILayout.Width(200));
            GUILayout.Label(HEADER_INSTRUCTION, EditorStyles.toolbarButton, GUILayout.Width(350));
            EditorStyles.toolbarButton.alignment = _alignment;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawRow(AudioDebugMgr.AudioDebugInfo info)
    {
        bool done = info.isDone;
        float playedTime = 0f;
        float playedTimeSinceHead = 0f;
      
        playedTime = info.quit ? info.quitTime - info.createTs : Time.time - info.createTs;
        playedTimeSinceHead = info.quit ? info.beginTime + (info.quitTime - info.createTs) : info.beginTime + (Time.time - info.createTs);

        playedTimeSinceHead = playedTimeSinceHead % info.duringTime;
        var style = done ? skin.customStyles[1] : skin.customStyles[0];
        EditorGUILayout.BeginVertical(style);
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(10));
            {
                if (info.isBgm)
                {
                    GUILayout.HorizontalSlider(playedTimeSinceHead < 10e-2 ? info.duringTime : playedTimeSinceHead, 0, info.duringTime, GUILayout.Width(700));
                }
                else
                {
                    if (done)
                    {
                        GUILayout.HorizontalSlider(1, 0, 1, GUILayout.Width(700));
                    }
                    else
                    {
                        GUILayout.HorizontalSlider(playedTimeSinceHead < 10e-2 ? info.duringTime : playedTimeSinceHead, 0, info.duringTime, GUILayout.Width(700));
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal(GUILayout.Height(ROW_HEIGHT));
            {
                if (GUILayout.Button(info.instruction, skin.label, GUILayout.Width(100), GUILayout.Height(ROW_HEIGHT)))
                    GUIUtility.systemCopyBuffer = info.instruction;
                if(GUILayout.Button(info.createTs.ToString(), skin.label, GUILayout.Width(100), GUILayout.Height(ROW_HEIGHT)))
                    GUIUtility.systemCopyBuffer = info.createTs.ToString();
                if (GUILayout.Button(playedTime.ToString(), skin.label, GUILayout.Width(100), GUILayout.Height(ROW_HEIGHT)))
                    GUIUtility.systemCopyBuffer = playedTime.ToString();
                if (GUILayout.Button(info.prefab, skin.label, GUILayout.Width(200), GUILayout.Height(ROW_HEIGHT)))
                    GUIUtility.systemCopyBuffer = info.prefab;
                if (GUILayout.Button(info.source, skin.label, GUILayout.Width(200), GUILayout.Height(ROW_HEIGHT)))
                    GUIUtility.systemCopyBuffer = info.source;
                if (GUILayout.Button(info.excelInstruction, skin.label, GUILayout.Width(350), GUILayout.Height(ROW_HEIGHT)))
                    GUIUtility.systemCopyBuffer = info.excelInstruction;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 根据Excel表内容刷新AudioDebugMgr.audioDebugInfoList内的instruction
    /// </summary>
    private void UpdateInstruction()
    {
        //判断Info中的instruction是否有经过Excel转化
        for (int i = AudioDebugMgr.audioDebugInfoList.Count-1; i >=0; i--)
        {
            if (AudioDebugMgr.audioDebugInfoList[i].excel == false)
            {
                for (int j = 0; j < excelData.GetLength(0); j++)
                {
                    if (excelData[j, g_excel_idIndex].Equals(AudioDebugMgr.audioDebugInfoList[i].instruction))
                    {
                        AudioDebugMgr.audioDebugInfoList[i].excelInstruction = excelData[j, g_excel_descIndex];
                        break;
                    }
                }
                AudioDebugMgr.audioDebugInfoList[i].excel = true;
            }
            else
            {
                break;
            }
        }
    }
    private bool GetGridUnderMouse(List<AudioDebugMgr.AudioDebugInfo> _list, out Vector2 _result)
    {
        Vector2 pos = Event.current.mousePosition + m_ScrollVec2;
        float paddingY = 42;
        float lineHeight = ROW_HEIGHT + 23f;
        if (pos.y < paddingY || pos.x < 0)
        {
            _result = Vector2.zero;
            return false;
        }
        
        Rect rect = new Rect(0, 0, 50, ROW_HEIGHT);
        for(int i = 0; i<_list.Count; ++i)
        {
            //ID格
            rect = new Rect(0, i * lineHeight + paddingY, 100, lineHeight);
            if (rect.Contains(pos))
            {
                _result = new Vector2(0, i);
                return true;
            }
            //创建时间格
            rect = new Rect(100, i * lineHeight + paddingY, 100, lineHeight);
            if (rect.Contains(pos))
            {
                _result = new Vector2(1, i);
                return true;
            }
            //播放时间格子
            rect = new Rect(200, i * lineHeight + paddingY, 100, lineHeight);
            if (rect.Contains(pos))
            {
                _result = new Vector2(2, i);
                return true;
            }
            //文件名格
            rect = new Rect(300, i * lineHeight + paddingY, 200, lineHeight);
            if (rect.Contains(pos))
            {
                _result = new Vector2(3, i);
                return true;
            }
            //描述格
            rect = new Rect(500, i * lineHeight + paddingY, 250, lineHeight);
            if (rect.Contains(pos))
            {
                _result = new Vector2(4, i);
                return true;
            }
        }

        _result = Vector2.zero;
        return false;
    }

    private string GetGridString(List<AudioDebugMgr.AudioDebugInfo> _list, Vector2 _index)
    {
        var _info = _list[(int)_index.y];
        if((int) _index.x == 0)
        {
            return _info.instruction.ToString();
        }
        else if((int)_index.x == 1)
        {
            return _info.createTs.ToString();
        }
        else if ((int)_index.x == 2)
        {
            float playedTime = 0f;
            if (_info.loop)
            {
                playedTime = _info.quit ? _info.quitTime - _info.createTs : Time.time - _info.createTs;
            }
            else
            {
                if (_info.stopTime > (_info.quit ? _info.quitTime : Time.time))
                {
                    playedTime = _info.quit ? _info.quitTime - _info.createTs : Time.time - _info.createTs;
                }
                else
                {
                    playedTime = _info.stopTime - _info.createTs;
                }
            }
            return playedTime.ToString();
        }
        else if ((int)_index.x == 3)
        {
            return _info.source;
        }
        else
        {
            return _info.instruction;
        }
    }
}

