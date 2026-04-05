using System;
using System.Collections.Generic;

using UnityEngine;

/****************
 * 补丁对象的自动保存任务对象
 **/
namespace ALPackage
{
    public class ALLocalPatchAutoSaveMonoTask : _IALBaseMonoTask
    {
        private ALLocalPatchInfo _m_piPatchInfo;

        public ALLocalPatchAutoSaveMonoTask(ALLocalPatchInfo _patchInfo)
        {
            _m_piPatchInfo = _patchInfo;
        }

        /**************
         * 函数主体
         **/
        public void deal()
        {
            if (null == _m_piPatchInfo)
                return;

            _m_piPatchInfo.savePatchInfo();
        }
    }
}
