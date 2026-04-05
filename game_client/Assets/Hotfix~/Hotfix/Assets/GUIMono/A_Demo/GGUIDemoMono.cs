using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;


namespace Hotfix
{
    /// <summary>
    /// 范例mono
    /// </summary>
    public class GGUIDemoMono : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("测试subWnd用")]
        public GGUIHotfixCommonMono subWndMono;
        
        [HotfixMonoAttribute("子窗口父节点")]
        public RectTransform subPrefabRoot;       
        
        [HotfixMonoAttribute("monoCostItem测试")]
        public NPGGUIMonoCommonItem monoCostItem;
        
        [HotfixMonoAttribute("Image测试")]
        public Image image;     
        
        [HotfixMonoAttribute("文本测试")]
        public Text textTest;
        
        [HotfixMonoAttribute("boolTest注释")]
        public bool boolTest;
        
        [HotfixMonoAttribute("floatTest注释")]
        public float floatTest;
        
        [HotfixMonoAttribute("strTest注释222")]
        public string strTest;
        
        [HotfixMonoAttribute("Vector3注释")]
        public Vector3 vector3Test;
        
        [HotfixMonoAttribute("color注释")]
        public Color colorTest;
        
        [HotfixMonoAttribute("int List Test注释")]
        public List<int> intListTest;
        
        [HotfixMonoAttribute("GameObject List Test注释")]
        public List<GameObject> gameObjectListTest;
    }
}