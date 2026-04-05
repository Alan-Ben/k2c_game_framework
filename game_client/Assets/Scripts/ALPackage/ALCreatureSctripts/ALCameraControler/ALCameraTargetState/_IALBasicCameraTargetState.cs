using UnityEngine;
using System.Collections;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /******************
     * 摄像头的目标状态
     * 在每帧中操作摄像头执行需要的操作
     **/
    public interface _IALBasicCameraTargetState
    {
        void updateCameraState(Camera _camera);
    }
}
#endif
