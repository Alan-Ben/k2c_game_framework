using ALPackage;
using UnityEngine;

/// <summary>
/// 3D场景中的Go对象
/// </summary>
namespace GOE
{
    public class NPUIObjPos : _INPShowPos
    {
        //UI对象
        private RectTransform _m_uiObj;

        public NPUIObjPos(RectTransform _uiObj)
        {
            _m_uiObj = _uiObj;
        }

        /// <summary>
        /// 获取对应显示的UI坐标
        /// </summary>
        /// <returns></returns>
        public Vector2 getUIPos()
        {
            if (null == _m_uiObj)
                return Vector2.zero;

#if NP_GAME
            //通过主摄像头查询显示对象位置
            return GCommon.getUIRootPos(_m_uiObj);
#else
            return Vector2.zero;
#endif
        }
    }
}

