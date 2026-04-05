
using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /************************
     * 主动行为过程中位移操作的监听对象
     **/
    public interface _IActionMovementListener
    {
        /**************
         * 在进入位移行为操作时的事件函数
         **/
        void onEnterMovement(ALBaseCreatureActionObj _actionObj);
        /**************
         * 在完成位移行为操作时的事件函数
         **/
        void onMovementDone(ALBaseCreatureActionObj _actionObj);
    }
}
#endif
