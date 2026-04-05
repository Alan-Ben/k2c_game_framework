using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 功能点的FollowInstance
    /// </summary>
    public class HomeEntryPointFollowInstance : _AALCommonFollowInstance
    {
        private readonly long _m_entryPointId;
        //建筑信息对象
        private readonly Transform _m_followView;

        public HomeEntryPointFollowInstance(long _entryPointId, Transform _followTarget)
        {
            _m_entryPointId = _entryPointId;
            _m_followView = _followTarget;
        }
        
        /// <summary>
        /// EntryPoint 表的 id 
        /// </summary>
        public long entryPointId { get { return _m_entryPointId; } }
        /// <summary>
        /// 获取对象在UI中显示的中心位置
        /// 每个子UI对象根据这个位置来刷新各自的位置
        /// </summary>
        public override Vector2 itemUICenterPos
        {
            get
            {
                if (null == _m_followView)
                {
                    return Vector2.zero;
                }
                
                return GCommon.worldPos2UIPos(_m_followView);
            }
        }
        /// <summary>
        /// 获取对象在场景中的位置
        /// </summary>
        public Vector3 itemWorldPos
        {
            get
            {
                if (null == _m_followView)
                {
                    return Vector3.zero;
                }
                
                return _m_followView.position;
            }
        }

        /// <summary>
        /// 是否在controller队列为0时一直刷新坐标信息
        /// </summary>
        public override bool isAlwaysRefreshPos {get { return false; } }
    }
}