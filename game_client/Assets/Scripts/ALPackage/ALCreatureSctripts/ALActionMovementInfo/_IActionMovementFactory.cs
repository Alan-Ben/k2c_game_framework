
using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /************************
     * 主动行为过程中的位移信息创建者
     **/
    public interface _IActionMovementFactory
    {
        /**************
         * 获取是否有位移操作对象，有则返回位移操作目标位置
         **/
        ActionMovementInfo popMovementInfo();
    }
}
#endif
