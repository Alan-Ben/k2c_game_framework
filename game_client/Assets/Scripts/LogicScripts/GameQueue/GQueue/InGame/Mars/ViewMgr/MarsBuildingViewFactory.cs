using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsBuildingViewFactory
    {
        [NotNull] public static MarsBuildingViewFactory instance { get { return _g_instance ??= new MarsBuildingViewFactory(); } }
        private static MarsBuildingViewFactory _g_instance;
        
        
        [NotNull] private readonly Dictionary<(EMarsBuildingType, MarsBuildingInfo.StateType), Func<MarsBuildingInfo, _AMarsBuildingView>> _m_viewCreators;
        
        
        private MarsBuildingViewFactory()
        {
            _m_viewCreators = new Dictionary<(EMarsBuildingType, MarsBuildingInfo.StateType), Func<MarsBuildingInfo, _AMarsBuildingView>>();
            
            registerView(EMarsBuildingType.HOME, MarsBuildingInfo.StateType.Normal, _info => new HomeNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.HOME, MarsBuildingInfo.StateType.Upgrading, _info => new HomeUpgradingMarsBuildingView(_info));
            registerView(EMarsBuildingType.ENERGY, MarsBuildingInfo.StateType.Normal, _info => new EnergyNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.ENERGY, MarsBuildingInfo.StateType.Upgrading, _info => new EnergyUpgradingMarsBuildingView(_info));
            registerView(EMarsBuildingType.REPAIR, MarsBuildingInfo.StateType.Normal, _info => new ExploreTeamNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.REPAIR, MarsBuildingInfo.StateType.Upgrading, _info => new ExploreTeamUpgradingMarsBuildingView(_info));
            registerView(EMarsBuildingType.SOLDIER, MarsBuildingInfo.StateType.Normal, _info => new ArmoryNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.SOLDIER, MarsBuildingInfo.StateType.Upgrading, _info => new ArmoryUpgradingMarsBuildingView(_info));
            registerView(EMarsBuildingType.TECHNOLOGY, MarsBuildingInfo.StateType.Normal, _info => new TechnologyNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.TECHNOLOGY, MarsBuildingInfo.StateType.Upgrading, _info => new TechnologyUpgradingMarsBuildingView(_info));
            registerView(EMarsBuildingType.LAW, MarsBuildingInfo.StateType.Normal, _info => new LawNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.EXPLORE, MarsBuildingInfo.StateType.Normal, _info => new ExploreNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.HELP, MarsBuildingInfo.StateType.Normal, _info => new AssistNormalMarsBuildingView(_info));
            registerView(EMarsBuildingType.HELP, MarsBuildingInfo.StateType.Upgrading, _info => new AssistUpgradingMarsBuildingView(_info));
        }
        
        
        public void registerView(EMarsBuildingType _buildingType, MarsBuildingInfo.StateType _state, Func<MarsBuildingInfo, _AMarsBuildingView> _creator)
        {
            if (_creator == null)
            {
                ALLog.Error("Cannot register null view creator");
                return;
            }
            
            (EMarsBuildingType _buildingType, MarsBuildingInfo.StateType _state) key = (_buildingType, _state);
            if (_m_viewCreators.ContainsKey(key))
            {
                ALLog.Warning($"Overriding existing view creator for {_buildingType} - {_state}");
            }
            
            _m_viewCreators[key] = _creator;
        }
        public _AMarsBuildingView createBuildingView(MarsBuildingInfo _buildingInfo)
        {
            if (_buildingInfo == null)
                return null;

            (EMarsBuildingType type, MarsBuildingInfo.StateType state) key = (_buildingInfo.type, _buildingInfo.state);
            if (_m_viewCreators.TryGetValue(key, out Func<MarsBuildingInfo, _AMarsBuildingView> creator))
            {
                try
                {
                    return creator(_buildingInfo);
                }
                catch (Exception e)
                {
                    ALLog.Error($"Failed to create view for {_buildingInfo.type} - {_buildingInfo.state}: {e.Message}");
                    return null;
                }
            }
            
            // Try fallback to common view for this state
            return _createCommonView(_buildingInfo);
        }


        private _AMarsBuildingView _createCommonView([NotNull] MarsBuildingInfo _buildingInfo)
        {
            switch (_buildingInfo.state)
            {
                case MarsBuildingInfo.StateType.Unbuilt:
                    return new CommonUnbuiltMarsBuildingView(_buildingInfo);
                case MarsBuildingInfo.StateType.Constructing:
                    return new CommonConstructingMarsBuildingView(_buildingInfo);
                case MarsBuildingInfo.StateType.Upgrading:
                    return new CommonUpgradingMarsBuildingView(_buildingInfo);
                case MarsBuildingInfo.StateType.Normal:
                    return new CommonNormalMarsBuildingView(_buildingInfo);
                default:
                    ALLog.Warning($"Unknown building state: {_buildingInfo.state}");
                    return null;
            }
        }
    }
}