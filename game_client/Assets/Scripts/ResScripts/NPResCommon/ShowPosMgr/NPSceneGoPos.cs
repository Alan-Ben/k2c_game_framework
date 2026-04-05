using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D场景中的Go对象
/// </summary>
namespace GOE
{
    public class NPSceneGoPos : _INPShowPos
    {
        //场景中的对象
        private Transform _m_trans;

        public NPSceneGoPos(Transform _trans)
        {
            _m_trans = _trans;
        }

        /// <summary>
        /// 获取对应显示的UI坐标
        /// </summary>
        /// <returns></returns>
        public Vector2 getUIPos()
        {
            if (null == _m_trans)
                return Vector2.zero;

#if NP_GAME
            //通过主摄像头查询显示对象位置
            return GCommon.worldPos2UIPos(_m_trans.position);
#else
            return Vector2.zero;
#endif
        }
    }
}

