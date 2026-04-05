using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 测试场景类的Mono
    /// </summary>
    public class GTDHotfixScene_DemoMono : _AHotfixBaseMono
    {
        [HotfixMono("go测试")]
        public GameObject go;     
        
        [HotfixMono("float测试")]
        public float floatTest = 2f;
    }
}