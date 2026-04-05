using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    //登入相关
    public partial class GRefdataCoreMgr
    {
        public NPLoginAreaRefObj getAreaByTag(string _areaTag)
        {
            foreach (NPLoginAreaRefObj loginAreaRefObj in loginAreaRefCore.refList)
            {
                if(null == loginAreaRefObj)
                    continue;

                if (loginAreaRefObj.tag == _areaTag)
                    return loginAreaRefObj;
            }

            return null;
        }
    }
}