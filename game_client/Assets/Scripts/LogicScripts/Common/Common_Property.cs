using System;

namespace GOE
{
    public static partial class Common_Property
    {
        public static _IPropertyShow getPropertyShow(this Enum _enum)
        {
            if (_enum == null)
                return null;

            if (_enum is Common.MarsEnum.EMarsPropertyType marsPropertyType)
            {
                return GRefdataCoreMgr.instance.marsPropertyShowRefCore.getRef((long)marsPropertyType);
            }
            else if (_enum is NPEnum.ENPPlayerPropertyType playerPropertyType)
            {
                return GRefdataCoreMgr.instance.playerPropertyShowRefCore.getRef((long)playerPropertyType);
            }
            else
            {
                Debug.LogError_EditorOnly($"{_enum.GetType()} is not property enum");
            }
            
            return null;
        }  
    }
}