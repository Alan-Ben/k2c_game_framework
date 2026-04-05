using ALPackage;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        private void _initAddPackRefCore()
        {
            addPackPathsRefCore.dealAllRef(_ref =>
            {
                if (_ref == null)
                    return;

                AddPackRefObj addPackRef = addPackRefCore.getRef(_ref.pack_id);
                if (addPackRef == null)
                {
                    ALLog.Warning("增量包路径表配置了不存在的增量包id: " + _ref.pack_id);
                    return;                    
                }
                
                addPackRef.pack_paths.Add(_ref.pack_path);
            });
        }
    }
}