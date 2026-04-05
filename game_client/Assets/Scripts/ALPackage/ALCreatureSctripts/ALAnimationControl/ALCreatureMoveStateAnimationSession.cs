using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
/**************************
 * 保存动作片段相关信息，并与Action部分关联
 **/

namespace ALPackage
{
    public class ALCreatureMoveStateAnimationSession : ALCreatureAnimationSession
    {
        /** 归属的移动状态对象信息 */
        private _AALBaseCharacterMoveState _m_aoParentMoveStateObj;

        public ALCreatureMoveStateAnimationSession(_AALBaseCharacterMoveState _moveState, int _serialize, float _parentTimeScale, ALSOBaseAnimationInfo _animationInfo)
            : base(_moveState.getParent().creatureControl, _serialize, _parentTimeScale, _animationInfo)
        {
            _m_aoParentMoveStateObj = _moveState;
        }

        public _AALBaseCharacterMoveState moveState { get { return _m_aoParentMoveStateObj; } }

        /***************
         * 删除本对象时处理
         **/
        public override void onDisable()
        {
            base.onDisable();

            _m_aoParentMoveStateObj = null;
        }
    }
}
#endif
