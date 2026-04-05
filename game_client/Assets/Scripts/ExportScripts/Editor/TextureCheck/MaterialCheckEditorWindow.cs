using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D;
using UnityEngine.U2D;

public class MaterialCheckEditorWindow : EditorWindow
{
    //查询范围
    public static List<Object> selectionObjectList = new List<Object>();
    public Vector2 scrollPosition1 = new Vector2(0, 0);
    public Vector2 scrollPosition2 = new Vector2(0, 0);

    //初始化选项框
    public static bool miss = true;    //材质丢失
    public static bool system_Lit = true;   //系统lit材质
    public static bool system_Unlit = true; //系统Unlit材质
    public static bool urp = true;  //urp材质
    public static bool error = true;    //错误材质
    public static bool other = true;    //其他错误材质
    
    [MenuItem("NPAssets/资源规范/材质规范窗口",false,50)]
    public static void DefaultShowWindow()
    {
        selectionObjectList = new List<Object>();
        selectionObjectList.Add(AssetDatabase.LoadAssetAtPath<Object>(new string("Assets/Resources")));
        MaterialCheckEditorWindow window = GetWindow<MaterialCheckEditorWindow>("材质规范检查");
        window.minSize = new Vector2(800, 300);  // 最小尺寸
    }
    
    public static void ShowWindow()
    {
        MaterialCheckEditorWindow window = GetWindow<MaterialCheckEditorWindow>("材质规范检查");
        window.minSize = new Vector2(800, 300);  // 最小尺寸
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1,  true,true,GUILayout.Width(450), GUILayout.Height(300));
        GUILayout.Label("------------------当前选择的检查文件夹对象------------");
        if (selectionObjectList.Count == 0)
        {
            GUILayout.Label("当前选择的文件夹为空，默认搜索Resources路径，请添加文件选择");
        }
        else
        {
            for(int i=0;i<selectionObjectList.Count;i++)
            {
                GUILayout.BeginHorizontal();
                string path = AssetDatabase.GetAssetPath(selectionObjectList[i]);
                //取消文件按钮
                if(GUILayout.Button("取消",GUILayout.Width(40)))
                {
                    selectionObjectList.RemoveAt(i);
                }
                GUILayout.Label(path);
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        
        
        GUILayout.Space(15);
        GUILayout.BeginVertical();
        GUILayout.Label("------------------需要检查的错误类型------------");
        GUILayout.Label("---材质检查---");
        if(miss != GUILayout.Toggle(miss,"材质丢失",GUILayout.Width(200)))
        {
            miss = !miss;
        }
        if (system_Lit != GUILayout.Toggle(system_Lit, "系统Lit材质（会重复打包）", GUILayout.Width(150)))
        {
            system_Lit = !system_Lit;
        }
        if (system_Unlit != GUILayout.Toggle(system_Unlit, "系统Unlit材质（会重复打包）", GUILayout.Width(200)))
        {
            system_Unlit = !system_Unlit;
        }
        if (urp != GUILayout.Toggle(urp, "系统其他URP材质（会重复打包）", GUILayout.Width(200)))
        {
            urp = !urp;
        }
        if (error != GUILayout.Toggle(error, "不存在的材质", GUILayout.Width(200)))
        {
            error = !error;
        }
        if (other != GUILayout.Toggle(other, "非项目规范的材质", GUILayout.Width(200)))
        {
            other = !other;
        }
        
        
        GUILayout.Space(15);
        GUILayout.Label("---快捷操作---");
        if(GUILayout.Button("将选择文件夹添加至列表",GUILayout.Width(200)))
        {
            Object[] selectionObjects = Selection.objects;
            foreach (var selectionObject in selectionObjects)
            {
                string path = AssetDatabase.GetAssetPath(selectionObject);
                if(Directory.Exists(path))
                    selectionObjectList.Add(selectionObject);
            }
        }
        if(GUILayout.Button("清空列表",GUILayout.Width(200)))
        {
            selectionObjectList = new List<Object>();
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(15);
        // 设置按钮为红色
        Color originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        if(GUILayout.Button("输出错误列表",GUILayout.Height(30)))
        {
            string[] selectionPath = new string[selectionObjectList.Count];
            for (int i = 0; i < selectionObjectList.Count; i++)
            {
                string path = AssetDatabase.GetAssetPath(selectionObjectList[i]);
                selectionPath[i] = path;
            }
            TextureCheck.CheckErrorMaterialAtPath(selectionPath,miss,system_Lit,system_Unlit,urp,error,other);
        }
        GUI.backgroundColor = originalBackgroundColor;
    }
    
}
