using System.Collections.Generic;
using ALPackage;
using GOE;
using GOE.FollowItem;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 主城入口跟随的wnd
    /// </summary>
    public class GGUIWndHomeEntryFollow : _ATNPGGUIWndCommonFollowRootWnd<GGUIMonoHomeEnrtyFollow>
    {
        private static GGUIWndHomeEntryFollow _g_instance = new GGUIWndHomeEntryFollow();
        [NotNull] public static GGUIWndHomeEntryFollow instance { get { return _g_instance ??= new GGUIWndHomeEntryFollow(); } }


        [NotNull] private readonly List<HomeEntryPointFollowInstance> _m_homeEntryFollowInstanceList;


        private GGUIWndHomeEntryFollow()
        {
            _m_homeEntryFollowInstanceList = new List<HomeEntryPointFollowInstance>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoHomeEnrtyFollow.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHomeEnrtyFollow.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        /// <summary>
        /// 注册 entryPoint 的跟随实例。
        /// </summary>
        /// <remarks>
        /// <para>特殊收集 entryPoint 的实例，以供查询。</para>
        /// </remarks>
        public void regInstance(HomeEntryPointFollowInstance _instance)
        {
            _m_homeEntryFollowInstanceList.Add(_instance);
            base.regInstance(_instance);
        }
        /// <summary>
        /// 移除 entryPoint 的跟随实例。
        /// </summary>
        /// <remarks>
        /// <para>从特殊收集的 entryPoint 中</para>
        /// </remarks>
        public void removeInstance(HomeEntryPointFollowInstance _instance)
        {
            _m_homeEntryFollowInstanceList.Remove(_instance);
            base.removeInstance(_instance);
        }
        /// <summary>
        /// 获取 entryPoint 的跟随实例。
        /// </summary>
        public HomeEntryPointFollowInstance getInstance(long _entryPointId)
        {
            foreach (HomeEntryPointFollowInstance followInstance in _m_homeEntryFollowInstanceList)
            {
                if (followInstance != null && followInstance.entryPointId == _entryPointId)                
                    return followInstance;
            }

            return null;
        }
    }
}