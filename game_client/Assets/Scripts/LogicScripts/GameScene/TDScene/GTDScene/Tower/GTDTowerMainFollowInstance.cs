using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// FollowInstance
    /// </summary>
    public class GTDTowerMainFollowInstance : _AALCommonFollowInstance
    {
        //跟随对象
        private Transform _m_followView;

        public GTDTowerMainFollowInstance(Transform _followTarget)
        {
            _m_followView = _followTarget;
        }
        
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
        /// 是否在controller队列为0时一直刷新坐标信息
        /// </summary>
        public override bool isAlwaysRefreshPos {get { return false; } }
    }
}