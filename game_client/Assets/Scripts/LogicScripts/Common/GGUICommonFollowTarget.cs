using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUICommonFollowTarget : _AALCommonFollowInstance
    {
        private Transform _m_followView;
        private Vector3 _m_offset;
        
        private Vector2 _m_vLastUIPos;

        public GGUICommonFollowTarget(Transform _followTarget, Vector3 _offset)
        {
            _m_followView = _followTarget;
            _m_offset = _offset;
            
            _m_vLastUIPos = Vector2.zero;
        }

        /// <summary>
        /// 最后一次刷新的UI坐标
        /// </summary>
        public Vector2 LastUIPos { get { return _m_vLastUIPos; } }

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
                
                _m_vLastUIPos = GCommon.worldPos2UIPos(_m_followView.position + _m_offset);

                return _m_vLastUIPos;
            }
        }
        /// <summary>
        /// 是否在 controller 队列为 0 时一直刷新坐标信息
        /// </summary>
        public override bool isAlwaysRefreshPos { get { return true; } }
    }
}