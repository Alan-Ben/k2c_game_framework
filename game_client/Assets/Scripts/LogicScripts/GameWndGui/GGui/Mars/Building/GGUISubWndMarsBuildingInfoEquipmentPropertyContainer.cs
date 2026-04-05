using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public struct MarsBuildingEquipmentPropertyShowData
    {
        public string propertyName;
        public string propertyDesc;
        public NPGTextureIndex icon;
        public string currentValueStr;
        public string nextValueStr;
        public bool isLevelMax;
    }
    public class GGUISubWndMarsBuildingInfoEquipmentPropertyContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsBuildingInfoEquipmentPropertyContainerItem, GGUIMonoMarsBuildingInfoEquipmentPropertyContainer, GGUISubWndMarsBuildingInfoEquipmentPropertyContainerItem>
    {
        [NotNull] private readonly List<MarsBuildingEquipmentPropertyShowData> _m_propertyDataList;
        
        
        public GGUISubWndMarsBuildingInfoEquipmentPropertyContainer([NotNull] GGUIMonoMarsBuildingInfoEquipmentPropertyContainer _containerMono)
            : base(_containerMono)
        {
            _m_propertyDataList = new List<MarsBuildingEquipmentPropertyShowData>();
            initWnd();
        }


        protected override GGUISubWndMarsBuildingInfoEquipmentPropertyContainerItem _createItemWnd(GGUIMonoMarsBuildingInfoEquipmentPropertyContainerItem _itemMono)
        {
            return new GGUISubWndMarsBuildingInfoEquipmentPropertyContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsBuildingInfoEquipmentPropertyContainerItem _itemWnd, int _index)
        {
            if (_index >= 0 && _index < _m_propertyDataList.Count)
            {
                MarsBuildingEquipmentPropertyShowData propertyShowData = _m_propertyDataList[_index];
                _itemWnd.refreshWnd(propertyShowData);
            }
        }


        public void refreshWnd(MarsBuildingEquipmentInfo _equipmentInfo)
        {
            _m_propertyDataList.Clear();
            
            if (_equipmentInfo != null)
            {
                // Add all property data types that have values
                if (_equipmentInfo.energyData.isValid())
                    _m_propertyDataList.AddRange(_equipmentInfo.energyData.getPropertyShowData());
                if (_equipmentInfo.foodData.isValid())
                    _m_propertyDataList.AddRange(_equipmentInfo.foodData.getPropertyShowData());
                if (_equipmentInfo.hospitalData.isValid())
                    _m_propertyDataList.AddRange(_equipmentInfo.hospitalData.getPropertyShowData());
                if (_equipmentInfo.livingData.isValid())
                    _m_propertyDataList.AddRange(_equipmentInfo.livingData.getPropertyShowData());
            }
            
            refreshWnd(_m_propertyDataList.Count);
        }
    }
}