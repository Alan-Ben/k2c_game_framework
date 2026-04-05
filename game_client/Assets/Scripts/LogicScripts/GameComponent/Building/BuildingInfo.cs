
using JetBrains.Annotations;

namespace GOE
{
    public class BuildingInfo
    {
        [NotNull] private readonly BuildingRefObj _m_buildingRef;
        private bool _m_isBuilt;
        
        
        public BuildingInfo([NotNull] BuildingRefObj _buildingRef)
        {
            _m_buildingRef = _buildingRef;
        }
        
        
        [NotNull] public BuildingRefObj baseRef { get { return _m_buildingRef; } }
        public long id { get { return _m_buildingRef.id; } }
        public bool isBuilt { get { return _m_isBuilt; } }


        public void setBuildingBuilt()
        {
            _m_isBuilt = true;
        }

        [Pure]
        public bool isHasBusinessFunction()
        {
            return GRefdataCoreMgr.instance.businessBuildingRefCore.getRef(_m_buildingRef.id) != null;
        }
        [Pure]
        public long getBusinessFunctionEmployeeEarnings()
        {
            BusinessBuildingRefObj businessRef = GRefdataCoreMgr.instance.businessBuildingRefCore.getRef(_m_buildingRef.id);
            if (businessRef == null)
                return 0;

            return businessRef.employee_earnings;
        }
    }
}