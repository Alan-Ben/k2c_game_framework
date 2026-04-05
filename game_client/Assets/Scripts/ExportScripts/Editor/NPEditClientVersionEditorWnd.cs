using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using ALPackage;
using GOE;
using Object = UnityEngine.Object;

public class NPEditClientVersionEditorWnd : EditorWindow {
    
        private List<string> _m_replacelist = new List<string>();
        private List<string> _m_replacelistHotfix = new List<string>();
        public static void ShowWindow() {
            //是否存在热更版本号文件
            EditorWindow.GetWindow(typeof(NPEditClientVersionEditorWnd));
        }
    
#if NP_GAME

        private WCGClientInfo _m_hotfixVersionInfo;

        private void Awake()
        {
            startHotfixScVersion();
        }

        void OnGUI() {
            GUILayout.Space(10);
            ClientVersionSetting.instance.ClientVersionInfo.majorVersion =
                EditorGUILayout.IntField("Major Version", ClientVersionSetting.instance.ClientVersionInfo.majorVersion);
            ClientVersionSetting.instance.ClientVersionInfo.minorVersion =
                EditorGUILayout.IntField("Minor Version", ClientVersionSetting.instance.ClientVersionInfo.minorVersion);
            ClientVersionSetting.instance.ClientVersionInfo.revisionVersion =
                EditorGUILayout.IntField("Revision Version", ClientVersionSetting.instance.ClientVersionInfo.revisionVersion);
            ClientVersionSetting.instance.ClientVersionInfo._buildVersion =
                EditorGUILayout.IntField("Build Version", ClientVersionSetting.instance.ClientVersionInfo._buildVersion);
            ClientVersionSetting.instance.ClientVersionInfo._dateVersion =
                EditorGUILayout.IntField("Date Version", ClientVersionSetting.instance.ClientVersionInfo._dateVersion);

            //存在热更版本号文件则可以修改
            if(null != _m_hotfixVersionInfo)
            {
                _m_hotfixVersionInfo.majorVersion = EditorGUILayout.IntField("HotfixSCVersion.main", _m_hotfixVersionInfo.majorVersion);
                _m_hotfixVersionInfo.minorVersion = EditorGUILayout.IntField("HotfixSCVersion.sub", _m_hotfixVersionInfo.minorVersion);
                _m_hotfixVersionInfo.revisionVersion = EditorGUILayout.IntField("HotfixSCVersion.patch", _m_hotfixVersionInfo.revisionVersion);
                _m_hotfixVersionInfo._buildVersion = EditorGUILayout.IntField("HotfixSCVersion.build", _m_hotfixVersionInfo._buildVersion);
                _m_hotfixVersionInfo._dateVersion = EditorGUILayout.IntField("HotfixSCVersion.date", _m_hotfixVersionInfo._dateVersion);
            }
            
            if (GUILayout.Button("ClientVersionSetting")) {
                _m_replacelist.Add(ClientVersionSetting.instance.ClientVersionInfo.majorVersion.ToString());
                _m_replacelist.Add(ClientVersionSetting.instance.ClientVersionInfo.minorVersion.ToString());
                _m_replacelist.Add(ClientVersionSetting.instance.ClientVersionInfo.revisionVersion.ToString());
                _m_replacelist.Add(ClientVersionSetting.instance.ClientVersionInfo._buildVersion.ToString());
                _m_replacelist.Add(ClientVersionSetting.instance.ClientVersionInfo._dateVersion.ToString());

                refreshclientSetting(Application.dataPath + "/Scripts/LogicScripts/ClientSCVersion.cs", _m_replacelist);
                refreshclientSetting(Application.dataPath + "/Scripts/ResScripts/ResSCVersion.cs", _m_replacelist);

                Object asset = AssetDatabase.LoadAssetAtPath(ClientVersionSetting.assetPath, typeof(ScriptableObject));

                if (null != _m_hotfixVersionInfo)
                {
                    _m_replacelistHotfix.Add(_m_hotfixVersionInfo.majorVersion.ToString());
                    _m_replacelistHotfix.Add(_m_hotfixVersionInfo.minorVersion.ToString());
                    _m_replacelistHotfix.Add(_m_hotfixVersionInfo.revisionVersion.ToString());
                    _m_replacelistHotfix.Add(_m_hotfixVersionInfo._buildVersion.ToString());
                    _m_replacelistHotfix.Add(_m_hotfixVersionInfo._dateVersion.ToString());
                    refreshclientSetting(Application.dataPath + "/Hotfix~/Hotfix/Assets/HotfixSCVersion.cs", _m_replacelistHotfix);

                }
            
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                _m_replacelist.Clear();
                _m_replacelistHotfix.Clear();
            }
        }
    
        static void refreshclientSetting(string _address , List<string> _replacelist)
        {
            if(_replacelist == null || _address == null)
                return;
            List<string> linelist = new List<string>();
            StreamReader rclientSetting = new StreamReader(_address);
            try
            {
                string line;
                string aheadline;
                int num = 0;
                while ((line = rclientSetting.ReadLine()) != null)
                {
                    string[] last = Regex.Split(line,"=",RegexOptions.IgnoreCase);
                    if(last.Length > 1)
                    {
                        aheadline = last[0];
                        if(last[1] != null && Regex.Matches(last[1], @"\d").Count != 0)
                        {
                            if(_replacelist[num] != null)
                                line = aheadline + "= " + _replacelist[num] + ";";
                        }
                        num++;
                    }
                    linelist.Add(line);
                }
            }
            finally
            {
                rclientSetting.Close();
            }
            StreamWriter wclientSetting = new StreamWriter(_address,false);
            try
            {
                linelist.ForEach((_line) =>
                {
                    wclientSetting.WriteLine(_line);
                });
            }
            finally
            {
                wclientSetting.Close();    
            }
        }
        
        //是否存在热更版本号文件
        private bool startHotfixScVersion()
        {
            //存在该文件则初始化当前版本号值
            if(File.Exists(Application.dataPath + "/Hotfix~/Hotfix/Assets/HotfixSCVersion.cs"))
            {
                StreamReader rclientSetting = new StreamReader(Application.dataPath + "/Hotfix~/Hotfix/Assets/HotfixSCVersion.cs");
                List<int> idList = new List<int>();
                try
                {
                    string line;
                    while ((line = rclientSetting.ReadLine()) != null)
                    {
                        string[] last = Regex.Split(line,"=",RegexOptions.IgnoreCase);
                        if(last.Length > 1)
                        {
                            string behindline = last[1];
                            if(behindline != null)
                            {
                                string id = Regex.Replace(behindline, @"[^0-9]+", "");
                                int tempValue = 0;
                                if(int.TryParse(id, out tempValue)) 
                                    idList.Add(tempValue);
                            }
                        }
                    }
                }
                finally
                {
                    rclientSetting.Close();
                }
                if(idList.Count > 4)
                {
                    if(null == _m_hotfixVersionInfo)
                        _m_hotfixVersionInfo = new WCGClientInfo();
                    
                    _m_hotfixVersionInfo.majorVersion = idList[0];
                    _m_hotfixVersionInfo.minorVersion = idList[1];
                    _m_hotfixVersionInfo.revisionVersion = idList[2];
                    _m_hotfixVersionInfo._buildVersion = idList[3];
                    _m_hotfixVersionInfo._dateVersion = idList[4];
                }
                return true;
            }
            return false;
        }

#endif
}