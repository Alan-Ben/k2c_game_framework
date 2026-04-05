using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(user-del：用户注销)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_user_del : MJSDK_2Engine_Base
    {
        public int status;//账号状态,1正常,2软删冷静期,3已软删，0封禁
        public long del_time;
    }
}
