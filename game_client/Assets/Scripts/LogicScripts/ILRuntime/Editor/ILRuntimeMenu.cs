using System;
using ALPackage;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    public class ILRuntimeMenu
    {
        [MenuItem("Hotfix/ILRuntime/生成Binding", false, 1)]
        static void GenerateCLRBindingByAnalysis()
        {
            //用新的分析热更dll调用引用来生成绑定代码
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
            System.IO.FileStream fs = null;
            fs = new System.IO.FileStream("Assets/hotfix~/Hotfix/bin/Editor/Hotfix.dll", System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            
            domain.LoadAssembly(fs);
            //Crossbind Adapter is needed to generate the correct binding code

            //注册所有的Adapter，分为alpackage跟主工程两部分
            ALHotfixMgr_ILRuntime_Global.bindILRuntimeAdapterFromAlpackage(domain);
            ILRuntimeBind.bindILRuntimeAdapter(domain);
        
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Scripts/LogicScripts/ILRuntime/Generated");
            AssetDatabase.Refresh();
            
            if(null != fs)
                fs.Close();
            fs = null;
        }
        
        [MenuItem("Hotfix/ILRuntime/生成跨域继承适配器")]
        static void GenerateCrossbindAdapter()
        {
            // //TODO 注意注意，跨域继承的基类的构造方法只能public
            //
            // //由于跨域继承特殊性太多，自动生成无法实现完全无副作用生成，所以这里提供的代码自动生成主要是给大家生成个初始模版，简化大家的工作
            // //大多数情况直接使用自动生成的模版即可，如果遇到问题可以手动去修改生成后的文件，因此这里需要大家自行处理是否覆盖的问题
            //
            // string aaa = "";
            // using(System.IO.StreamWriter sw = new System.IO.StreamWriter("Assets/Scripts/LogicScripts/ILRuntime/Adapters/NPGGUIMonoCommonTabAdapter.cs"))
            // {
            //     sw.WriteLine(ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(Type.GetType(aaa), "GOE"));
            // }
            //
            // AssetDatabase.Refresh();

            Rect _s_rect = new Rect(0, 0, 500, 250);
            ILRuntimeAdapterEditorWnd window = (ILRuntimeAdapterEditorWnd)EditorWindow.GetWindowWithRect(typeof(ILRuntimeAdapterEditorWnd), _s_rect, true, "生成跨域继承适配器");
            window.Show();
        }
        
        [MenuItem("Hotfix/ILRuntime/测试方法")]
        static void testHotfix()
        {
            //用新的分析热更dll调用引用来生成绑定代码
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
            System.IO.FileStream fs = null;
            fs = new System.IO.FileStream("Assets/hotfix~/Hotfix/bin/Editor/Hotfix.dll", System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            
            domain.LoadAssembly(fs);
            //Crossbind Adapter is needed to generate the correct binding code
            domain.Invoke("Hotfix.HotfixMain", "test", null, null);
            
            if(null != fs)
                fs.Close();
            fs = null;
            
            // ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixMain", "test");
        }
    }
}