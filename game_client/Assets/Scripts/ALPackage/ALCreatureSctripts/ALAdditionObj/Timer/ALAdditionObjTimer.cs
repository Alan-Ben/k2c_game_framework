using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
/**********************
 * 附加物件的定时控制对象
 **/

namespace ALPackage
{
    public class ALAdditionObjTimer : MonoBehaviour
    {
        /** 存活时间 */
        public float lifeTime;
        /** 添加的物件控制对象 */
        public ALCreatureAdditionObj additionObj;
        /** 已经存在的时间 */
        private float aliveTime;

        void Start()
        {
            aliveTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            aliveTime += Time.deltaTime;

            //当到达生命周期上限时
            if (lifeTime <= aliveTime)
            {
                //从父亲节点中删除此添加对象关联
                additionObj.discard();
            }
        }
    }
}
#endif
