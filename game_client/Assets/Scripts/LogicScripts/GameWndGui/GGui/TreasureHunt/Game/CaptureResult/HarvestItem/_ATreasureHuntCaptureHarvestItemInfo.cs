using System.Collections.Generic;
using Common.TreasureHuntEnum;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝-打捞收获物item信息
    /// </summary>
    public abstract class _ATreasureHuntCaptureHarvestItemInfo
    {
        /// <summary>
        /// 收获物类型
        /// </summary>
        public abstract ETreasureHuntGainType havestType { get; }

        /// <summary>
        /// 不同类型的子id
        /// </summary>
        public abstract long subId { get; }

        /// <summary>
        /// 收获物品质
        /// </summary>
        public abstract EQuality quality { get; }

        public abstract int compareTo(_ATreasureHuntCaptureHarvestItemInfo _target);
    }
    
    public class TreasureHuntCaptureHarvestTypeItemShowComparer
    {
        public static int Compare(ETreasureHuntGainType x, ETreasureHuntGainType y)
        {
            int xSortOrder = getSortOrder(x);
            int ySortOrder = getSortOrder(y);
            return xSortOrder.CompareTo(ySortOrder);
        }

        public static int getSortOrder(ETreasureHuntGainType _type)
        {
            switch (_type)
            {
                case ETreasureHuntGainType.TREASURE: return 1;
                case ETreasureHuntGainType.ORE: return 2;
                case ETreasureHuntGainType.REWARD : return 3;
                
                default: return int.MaxValue;
            }
        }
    }
    
    /// <summary>
    /// 太空寻宝-打捞收获物-矿石信息
    /// </summary>
    public class TreasureHuntCaptureHarvestItemInfo_Ore : _ATreasureHuntCaptureHarvestItemInfo
    {
        [NotNull] private TreasureHuntCommonOreInfo _m_oreInfo;
        
        public TreasureHuntCaptureHarvestItemInfo_Ore([NotNull] TreasureHuntCommonOreInfo oreInfo)
        {
            _m_oreInfo = oreInfo;
        }

        public override ETreasureHuntGainType havestType { get { return ETreasureHuntGainType.ORE; } }
        public override long subId { get { return _m_oreInfo.oreId; } }
        public override EQuality quality { get { return _m_oreInfo.oreRefObj?.quality ?? EQuality.NONE; } }
        
        [NotNull] public TreasureHuntCommonOreInfo oreInfo { get { return _m_oreInfo; } }

        public override int compareTo(_ATreasureHuntCaptureHarvestItemInfo _target)
        {
            if (_target == null)
                return -1;
            
            // 先进行类型排序
            int typeCompare = TreasureHuntCaptureHarvestTypeItemShowComparer.Compare(havestType, _target.havestType);
            if(typeCompare != 0)
                return typeCompare;
            
            // 同类型再进行排序
            TreasureHuntCaptureHarvestItemInfo_Ore targetOre = _target as TreasureHuntCaptureHarvestItemInfo_Ore;
            if (targetOre == null)
                return -1;
            
            // 先按照品质排序
            int qualityCompare = targetOre.quality.CompareTo(quality);
            if (qualityCompare != 0)
                return qualityCompare;
            
            // 再按照是否高级矿石排序
            bool selfIsAdvance = _m_oreInfo.oreRefObj?.isReachAdvanceOreMass(_m_oreInfo.mass) ?? false;
            bool targetIsAdvance = targetOre.oreInfo.oreRefObj?.isReachAdvanceOreMass(targetOre.oreInfo.mass) ?? false;
            if (selfIsAdvance != targetIsAdvance)
                return targetIsAdvance.CompareTo(selfIsAdvance);

            // 最后按照id排序
            return _m_oreInfo.oreId.CompareTo(targetOre.oreInfo.oreId);
        }
    }
    
    /// <summary>
    /// 太空寻宝-打捞收获物-奖励信息
    /// </summary>
    public class TreasureHuntCaptureHarvestItemInfo_Reward : _ATreasureHuntCaptureHarvestItemInfo
    {
        [NotNull] private _IItem _m_rewardInfo;
        
        public TreasureHuntCaptureHarvestItemInfo_Reward([NotNull] _IItem rewardInfo)
        {
            _m_rewardInfo = rewardInfo;
        }

        public override ETreasureHuntGainType havestType { get { return ETreasureHuntGainType.REWARD; } }
        public override long subId { get { return _m_rewardInfo.subId; } }
        public override EQuality quality { get { return _m_rewardInfo.getQuality(); } }
        
        [NotNull] public _IItem rewardInfo { get { return _m_rewardInfo; } }

        public override int compareTo(_ATreasureHuntCaptureHarvestItemInfo _target)
        {
            if (_target == null)
                return -1;
            
            // 先进行类型排序
            int typeCompare = TreasureHuntCaptureHarvestTypeItemShowComparer.Compare(havestType, _target.havestType);
            if(typeCompare != 0)
                return typeCompare;
            
            // 同类型再进行排序
            TreasureHuntCaptureHarvestItemInfo_Reward targetReward = _target as TreasureHuntCaptureHarvestItemInfo_Reward;
            if (targetReward == null)
                return -1;
            
            // 先按照品质排序
            int qualityCompare = targetReward.quality.CompareTo(quality);
            if (qualityCompare != 0)
                return qualityCompare;

            // 最后按照id排序
            return _m_rewardInfo.subId.CompareTo(targetReward.rewardInfo.subId);
        }
    }
    
    public class TreasureHuntCaptureHarvestItemInfo_Treasure : _ATreasureHuntCaptureHarvestItemInfo
    {
        [NotNull] private TreasureHuntGotTreasureInfo _m_treasureInfo;
        
        public TreasureHuntCaptureHarvestItemInfo_Treasure([NotNull] TreasureHuntGotTreasureInfo treasureInfo)
        {
            _m_treasureInfo = treasureInfo;
        }

        public override ETreasureHuntGainType havestType { get { return ETreasureHuntGainType.TREASURE; } }
        public override long subId { get { return _m_treasureInfo.treasureId; } }
        public override EQuality quality { get { return _m_treasureInfo.treasureRefObj?.quality ?? EQuality.NONE; } }
        
        [NotNull] public TreasureHuntGotTreasureInfo treasureInfo { get { return _m_treasureInfo; } }

        public override int compareTo(_ATreasureHuntCaptureHarvestItemInfo _target)
        {
            if (_target == null)
                return -1;
            
            // 先进行类型排序
            int typeCompare = TreasureHuntCaptureHarvestTypeItemShowComparer.Compare(havestType, _target.havestType);
            if(typeCompare != 0)
                return typeCompare;
            
            // 同类型再进行排序
            TreasureHuntCaptureHarvestItemInfo_Treasure targetTreasure = _target as TreasureHuntCaptureHarvestItemInfo_Treasure;
            if (targetTreasure == null)
                return -1;
            
            // 先按照品质排序
            int qualityCompare = targetTreasure.quality.CompareTo(quality);
            if (qualityCompare != 0)
                return qualityCompare;

            // 最后按照id排序
            return _m_treasureInfo.treasureId.CompareTo(targetTreasure.treasureInfo.treasureId);
        }
    }
}