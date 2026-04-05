using System.Collections.Generic;

namespace GOE
{
    //皮肤相关
    public partial class GRefdataCoreMgr
    {
        private void _initTravel()
        {
            GRefdataCoreMgr.instance.travelConsortRefCore.dealAllRef((_refObj) =>
            {
                if(_refObj == null)
                    return;

                // 对like_step_dialog_list列表进行排序, 按照first字段(所需好感度)升序排序
                if (_refObj.like_step_dialog_list != null)
                {
                    _refObj.like_step_dialog_list.Sort((_a, _b) =>
                    {
                        if (_b == null)
                            return -1;
                        if (_a == null)
                            return 1;
                        if (object.ReferenceEquals(_a, _b))
                            return 0;

                        return _a.first().CompareTo(_b.first());
                    });
                }
            });
        }
    }
}