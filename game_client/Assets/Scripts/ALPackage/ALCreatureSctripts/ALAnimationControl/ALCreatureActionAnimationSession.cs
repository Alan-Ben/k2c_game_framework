using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
/**************************
 * 保存动作片段相关信息，并与Action部分关联
 **/

namespace ALPackage
{
    public class ALCreatureActionAnimationSession : ALCreatureAnimationSession
    {
        /** 归属的操作对象信息 */
        private ALBaseCreatureActionObj _m_aoParentActionObj;

        public ALCreatureActionAnimationSession(ALBaseCreatureActionObj _parentObj, int _serialize, ALSOBaseAnimationInfo _animationInfo)
            : base(_parentObj.creatureControl, _serialize, _parentObj.getRealTimeScale(), _animationInfo)
        {
            _m_aoParentActionObj = _parentObj;
        }

        public ALBaseCreatureActionObj parentActionObj { get { return _m_aoParentActionObj; } }

        /***************
         * 删除本对象时处理
         **/
        public override void onDisable()
        {
            base.onDisable();

            _m_aoParentActionObj = null;
        }
    }
}
#endif
