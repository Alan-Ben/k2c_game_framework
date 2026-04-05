using UnityEditor;
using UnityEngine;
// using H2Framework.ForXLua;
using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using GOE;

namespace MJSoundEditor
{
    /// <summary>
    /// Editor调音台
    /// </summary>
    public class SoundTunerEditor : EditorWindow
    {
        [MenuItem("Tools/音频/音频调整面板")]
        private static void Display()
        {
            var _win = GetWindow<SoundTunerEditor>("Sound Tuner", true);
            _win.Show();
        }

        [MenuItem("Assets/音频相关/统一质量参数", priority = 1)]
        public static void RefreshAudioQuality()
        {
            string[] guids = Selection.assetGUIDs;
            string[] pathes = new string[guids.Length];
            for (int i = 0; i < pathes.Length; ++i)
            {
                pathes[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }
            NormalizeAudioQuality(pathes);
        }

        private static readonly string g_skinPath = "Assets/Scripts/ExportScripts/Editor/Audio/SoundTunerSkin.guiskin";

        /// <summary>
        /// 音频文件质量相关参数批处理
        /// </summary>
        /// <param name="_pathes"></param>
        static void NormalizeAudioQuality(string[] _pathes)
        {
            foreach (string path in _pathes)
            {
                string audioFilePath = path;
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioFilePath);

                if (clip)
                {
                    AudioImporter auImporter = (AudioImporter)AssetImporter.GetAtPath(path);
                    var setting = auImporter.defaultSampleSettings;
                    setting.quality = 0.4f;
                    setting.sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate;
                    setting.sampleRateOverride = 32000;
                    auImporter.defaultSampleSettings = setting;
                }
            }
        }

        private readonly GUIContent HEADER_VOLUME = new GUIContent("Volume");

        private enum AudioType { audio, bgm };

        private GUISkin skin;
        public GUIStyle backgroundOdd;
        public GUIStyle backgroundEven;
        
        private Dictionary<string, _IAudioVolumeShow> m_audioSource_peek;
        private Dictionary<string, _IAudioVolumeShow> m_video_peek;
    
        private string m_filter_name;

        private AudioType m_filter_type = AudioType.audio;
    
        private Vector2 m_ScrollVec2;
        private int m_pageIndex = 0; // 当前页码
        private int m_pageSize = 50; // 每页显示数量
        private const string PREFS_PAGE_SIZE_KEY = "SoundTunerEditor_PageSize";
        private const string PREFS_AUTO_SELECT_PLAY_GO = "SoundTunerEditor_AutoSelectPlayGo";
        private HashSet<_IAudioVolumeShow> m_selectedItems = new HashSet<_IAudioVolumeShow>(); // 勾选状态
        private bool m_isOffsetMode = true; // true:相对偏移模式  false:设置相同值模式
        private bool m_showVideo = true; 
        private bool m_showAudio = true; 
        private bool m_autoSelectPlayGo  = false; 

        private const int ROW_HEIGHT = 18;      //行高

        private SoundTunerConfig m_soundConfig;
    
        private static readonly ENPExportSettingEnum g_exportEnum = ENPExportSettingEnum.AUDIO; //指定类型，用于获取Excel路径
        private static readonly int g_excel_idIndex =0;   //excel表内id列索引
        private static readonly int g_excel_descIndex =1; //excel表内desc列索引

      
        
        static List<SoundTunerConfig> GetAvailableConfigs()
        {
            List<SoundTunerConfig> brushes = MJEditorUtility.GetAll<SoundTunerConfig>();

            if (brushes.Count < 1)
                brushes.Add(MJEditorUtility.GetFirstOrNew<SoundTunerConfig>());

            return brushes;
        }
        /// <summary>
        /// 窗口开启处理
        /// </summary>
        private void OnEnable()
        {
          
            m_pageSize = EditorPrefs.GetInt(PREFS_PAGE_SIZE_KEY, 30);
            m_autoSelectPlayGo = EditorPrefs.GetBool(PREFS_AUTO_SELECT_PLAY_GO, false);
            m_filter_name = "";
            m_audioSource_peek = new Dictionary<string, _IAudioVolumeShow>();
            m_video_peek = new Dictionary<string, _IAudioVolumeShow>();
            
            //加载EditorGUI皮肤
            skin = AssetDatabase.LoadAssetAtPath<GUISkin>(g_skinPath);
            
            initAudioSourceList();
            initVideoList();
            m_selectedItems.Clear();
            EditorApplication.update += OnUpdate;
        }

        private void initAudioSourceList()
        {
            //获取Excel表数据
            string excelPath=ALExportDataCore.instance.getValue(g_exportEnum.ToString());
            string excelWorksheetName=NPInputTabData.instance.getValue(g_exportEnum.ToString());
            string[,] excelData = ExcelCommon.LoadExcel(excelPath, excelWorksheetName);
            
            var configs = GetAvailableConfigs();
            m_soundConfig = configs[0];
            foreach (AudioPaths audioPath in m_soundConfig.audioPaths)
            {
                foreach (var path in audioPath.paths)
                {
                    var absolutePaths = System.IO.Directory.GetFiles(path, "*.prefab", System.IO.SearchOption.AllDirectories);
                    for(int i=0; i<absolutePaths.Length; ++i)
                    {
                        absolutePaths[i] = absolutePaths[i].Replace('\\', '/');
                        
                        int firstIndex = absolutePaths[i].LastIndexOf('/');
                        int lastIndex = absolutePaths[i].IndexOf('.');
                        string audioName = absolutePaths[i].Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                        GameObject audioObj = AssetDatabase.LoadAssetAtPath<GameObject>(absolutePaths[i]);
                        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
                        if (audioSource)
                        {
                            string idName = audioName.Substring(audioName.LastIndexOf("_")+1);
                            AudioSourceVolumeInfo info = new AudioSourceVolumeInfo(audioSource, audioPath.mixerGroup, GetInstruction(excelData, idName));
                          
                            m_audioSource_peek[audioName] = info;
                        }
                    }
                }
            }
        }
     
        protected  void initVideoList()
        {
            string videoVolumeConfigPath = "Assets/Resources/GameRes/audio/video_volume_info.asset";
            GSOVideoVolumeRefSet videoVolumeRef = AssetDatabase.LoadAssetAtPath<GSOVideoVolumeRefSet>(videoVolumeConfigPath);
            string _c_resRootPath = "video";
            string _c_remoteVersionFileName = "remote_video_version";

            string videoRemotePath = Application.persistentDataPath + "/" + _c_resRootPath;
            var absolutePaths = System.IO.Directory.GetFiles(videoRemotePath, "vc_*", System.IO.SearchOption.AllDirectories);
            for(int i=0; i<absolutePaths.Length; ++i)
            {
                absolutePaths[i] = absolutePaths[i].Replace('\\', '/');
                        
                int firstIndex = absolutePaths[i].LastIndexOf('/');
                int lastIndex = absolutePaths[i].IndexOf('.');
                string audioName = absolutePaths[i].Substring(firstIndex + 1, lastIndex - firstIndex - 1);
                string fileName = Path.GetFileNameWithoutExtension(absolutePaths[i]);
             
                var splits = fileName.Split("_");
            
                GVideoClipIndex videoClipIndex= null;
                if (splits != null && splits.Length >= 3 && int.TryParse(splits[1], out int mainId) && int.TryParse(splits[2], out int subId)) 
                    videoClipIndex = new GVideoClipIndex(mainId, subId);
                if (videoClipIndex != null)
                {
                    VideoVolumeInfo info = new VideoVolumeInfo(videoClipIndex, videoVolumeRef);
                    m_video_peek[audioName] = info;
                }
            }
        }
        /// <summary>
        /// 窗口关闭处理
        /// </summary>
        private void OnDisable()
        {
            EditorApplication.update -= OnUpdate;
            ClearCache();
        }

        /// <summary>
        /// 绘制根循环
        /// </summary>
        private void OnUpdate()
        {
            Repaint();
        }

        private List<_IAudioVolumeShow> tempAudioList = new List<_IAudioVolumeShow>();
        private void OnGUI()
        {
            if (backgroundOdd == null)
            {
                backgroundOdd = "CN EntryBackOdd";
                backgroundOdd = new GUIStyle(backgroundOdd);
                backgroundOdd.border = new RectOffset(0, 0, 0, 0);
                backgroundOdd.margin = new RectOffset(0, 0, 0, 0);
                backgroundOdd.padding = new RectOffset(0, 0, 0, 0);
            }

            if (backgroundEven == null)
            {
                backgroundEven = "CN EntryBackEven";
                backgroundEven = new GUIStyle(backgroundEven);
                backgroundEven.border = new RectOffset(0, 0, 0, 0);
                backgroundEven.margin = new RectOffset(0, 0, 0, 0);
                backgroundEven.padding = new RectOffset(0, 0, 0, 0);
            }
            
            tempAudioList.Clear();
            if (string.IsNullOrEmpty(m_filter_name))
            {
                if(m_showAudio)
                    tempAudioList.AddRange(m_audioSource_peek.Values);
                if(m_showVideo)
                    tempAudioList.AddRange(m_video_peek.Values);
            }
            else
            {
                if(m_showAudio)
                {
                    var enumator = m_audioSource_peek.GetEnumerator();
                    while (enumator.MoveNext())
                    {
                        if (enumator.Current.Key.ToLowerInvariant().IndexOf(m_filter_name.ToLowerInvariant()) >= 0)
                        {
                            tempAudioList.Add(enumator.Current.Value);
                        }
                    }
                }
                if(m_showVideo)
                {
                    var enumator = m_video_peek.GetEnumerator();
                    while (enumator.MoveNext())
                    {
                        if(enumator.Current.Key.ToLowerInvariant().IndexOf(m_filter_name.ToLowerInvariant()) >= 0)
                        {
                            tempAudioList.Add(enumator.Current.Value);
                        }
                    }
                }
            }
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                EditorGUILayout.LabelField("筛选", EditorStyles.label, GUILayout.Width(40));
                m_filter_name = EditorGUILayout.TextField(m_filter_name, EditorStyles.miniTextField);
                EditorGUILayout.LabelField("视频", EditorStyles.label, GUILayout.Width(40));
                m_showVideo = EditorGUILayout.Toggle(m_showVideo, GUILayout.Width(20));
                EditorGUILayout.LabelField("音频", EditorStyles.label, GUILayout.Width(40));
                m_showAudio = EditorGUILayout.Toggle(m_showAudio, GUILayout.Width(20));
                EditorGUILayout.LabelField("选中播放音视频", EditorStyles.label, GUILayout.Width(80));
                bool autoSelectPlayGo = EditorGUILayout.Toggle(m_autoSelectPlayGo, GUILayout.Width(20));
                if (autoSelectPlayGo != m_autoSelectPlayGo)
                {
                    m_autoSelectPlayGo = autoSelectPlayGo;
                    EditorPrefs.SetBool(PREFS_AUTO_SELECT_PLAY_GO, m_autoSelectPlayGo);
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("清除所有播放", EditorStyles.toolbarButton, GUILayout.Width(150)))
                {
                    foreach (var audio in tempAudioList)
                        audio?.stop();
                }
                if (GUILayout.Button("保存", EditorStyles.toolbarButton, GUILayout.Width(150)))
                {
                    AssetDatabase.SaveAssets();
                }
            }
            EditorGUILayout.EndHorizontal();


            int totalCount = tempAudioList.Count;
            int totalPages = Mathf.CeilToInt((float)totalCount / m_pageSize);
            m_pageIndex = Mathf.Clamp(m_pageIndex, 0, Math.Max(0, totalPages - 1));
            int startIdx = m_pageIndex * m_pageSize;
            int endIdx = Math.Min(startIdx + m_pageSize, totalCount);

      

            m_ScrollVec2 = EditorGUILayout.BeginScrollView(m_ScrollVec2);
            GUILayout.Space(5);

            drawSelectAll(startIdx, endIdx, tempAudioList);

            for (int i = startIdx; i < endIdx; ++i)
            {
                DrawRow(tempAudioList[i], i - startIdx);
            }
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("每页数量:", GUILayout.Width(60));
            int newPageSize = EditorGUILayout.IntField(m_pageSize, EditorStyles.miniTextField, GUILayout.Width(60));
            if (newPageSize != m_pageSize && newPageSize > 0)
            {
                m_pageSize = newPageSize;
                EditorPrefs.SetInt(PREFS_PAGE_SIZE_KEY, m_pageSize);
                m_pageIndex = 0;
            }
            if (GUILayout.Button("上一页", GUILayout.Width(80)))
            {
                m_pageIndex = Math.Max(0, m_pageIndex - 1);
                m_selectedItems.Clear();
            }
            GUILayout.Label($"第 {m_pageIndex + 1} 页 / 共 {totalPages} 页", GUILayout.Width(150));
            if (GUILayout.Button("下一页", GUILayout.Width(80)))
            {
                m_pageIndex = Math.Min(totalPages - 1, m_pageIndex + 1);
                m_selectedItems.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void drawSelectAll(int startIdx, int endIdx, List<_IAudioVolumeShow> audioList)
        {
            int showCount = endIdx - startIdx;

            EditorGUILayout.BeginHorizontal();
            bool oldSelectAll= m_selectedItems.Count == showCount;
            bool selectAll = EditorGUILayout.ToggleLeft("全选", oldSelectAll, GUILayout.Width(100));
            if (selectAll != oldSelectAll)
            {
                if (selectAll)
                {
                    m_selectedItems.Clear();
                    for (int i = startIdx; i < endIdx; ++i)
                    {
                        m_selectedItems.Add(audioList[i]);
                    }
                }
                else
                {
                    m_selectedItems.Clear();
                }
            }
            m_isOffsetMode = EditorGUILayout.Toggle(m_isOffsetMode, GUILayout.Width(20));
            EditorGUILayout.LabelField(m_isOffsetMode ? "勾选项：相对偏移" : "勾选项：设为相同值", GUILayout.Width(150));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRow(_IAudioVolumeShow _audio_source, int rowIndex)
        {
            // 使用 Unity 内置的隔行背景样式（与 Console 窗口一致）
            GUIStyle rowBgStyle = rowIndex % 2 == 0 ? backgroundEven : backgroundOdd;
            {
                EditorGUILayout.BeginHorizontal(rowBgStyle);
                {
                    // 勾选框
                    bool isSelected = m_selectedItems.Contains(_audio_source);

                    bool newSelected = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));
                    if (newSelected != isSelected)
                    {
                        if(newSelected)
                            m_selectedItems.Add(_audio_source);
                        else
                            m_selectedItems.Remove(_audio_source);
                    }
                  
                    
                    //名字
                    if (GUILayout.Button(_audio_source.name, skin.label, GUILayout.Width(150)))
                        GUIUtility.systemCopyBuffer = _audio_source.name;
                  
                    if (GUILayout.Button(_audio_source.instruction, skin.label, GUILayout.Width(150)))
                        GUIUtility.systemCopyBuffer = _audio_source.instruction;
                    //音量条
                    float oldVolume = _audio_source.volume;
                    float newVolume = oldVolume;
                    newVolume = GUILayout.HorizontalSlider(newVolume, 0, 1, GUILayout.Width(270));
                    newVolume = EditorGUILayout.FloatField(newVolume, EditorStyles.miniTextField);
               
                    if (GUILayout.Button(_audio_source.isPlaying ?"Playing":"Play", EditorStyles.toolbarButton, GUILayout.Width(100)))
                    {
                        if (_audio_source.isPlaying)
                            _audio_source.stop();
                        foreach (var audio in tempAudioList)
                            audio?.stop();
                        _audio_source.play(m_autoSelectPlayGo);
                    }
                    if (GUILayout.Button("Stop", EditorStyles.toolbarButton, GUILayout.Width(100)))
                    {
                        _audio_source.stop();
                    }
                    // 如果当前项被勾选且音量发生变化，同步修改所有勾选项的音量
                    if (m_selectedItems.Contains(_audio_source) && !Mathf.Approximately(newVolume, oldVolume))
                    {
                        float volumeDelta = newVolume - oldVolume;
                        foreach (var kvp in m_selectedItems)
                        {
                            if (kvp == null) continue;
                            kvp.volume = m_isOffsetMode ? 
                                // 相对偏移模式：所有勾选项的音量按相同偏移量调整
                                Mathf.Clamp01(kvp.volume + volumeDelta) 
                                :
                                // 绝对值模式：所有勾选项设为相同值
                                newVolume;
                        }
                    }
                    else
                    {
                        _audio_source.volume = newVolume;
                    }
                    
                    // TODO
                    // if (Application.isPlaying)
                    // {
                    //     SoundAgent.Instance.SetMaxVolume(_audio_source.name, _audio_source.volume);
                    //     SoundAgent.Instance.RegistApplicationQuitProc(ClearCache);
                    // }
                }
                EditorGUILayout.EndHorizontal();
            }
            // EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white; // 重置背景色
        }


        private string GetInstruction(string[,] _excelData,string _id)
        {
            for (int i = 0; i < _excelData.GetLength(0); i++)
            {
                if (_excelData[i, g_excel_idIndex].Equals(_id))
                {
                    return _excelData[i, g_excel_descIndex];
                }
            }

            return null;
        }
        
        /// <summary>
        /// 清理音频物体
        /// </summary>
        public void ClearCache()
        {
            var iterator = m_video_peek.GetEnumerator();
            while (iterator.MoveNext())
            {
                iterator.Current.Value.stop();
            }
            m_video_peek.Clear();
            iterator = m_audioSource_peek.GetEnumerator();
            while (iterator.MoveNext())
                iterator.Current.Value.stop();
            m_audioSource_peek.Clear();
        }
    }


}