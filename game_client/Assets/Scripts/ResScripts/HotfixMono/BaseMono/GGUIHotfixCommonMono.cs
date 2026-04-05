using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 热更窗体wnd专用mono
    /// </summary>
    [RequireComponent(typeof(MonoSkin))]
    public class GGUIHotfixCommonMono : _AALBasicUIWndMono
    {
        [HideInInspector]
        public MonoSkin monoSkin;
        
        private void Awake () {
            monoSkin = this.GetComponent<MonoSkin>();
        }
    }
}