using Common.ChildEnum;
using GC2GS.p014_ChildOp;
using System.Collections.Generic;

namespace GOE
{
    public static class GSWriter_014_ChildOp
    {
        public static GC2GS_014_001_ReqSetChildName make_001_ReqSetChildName(long _childId, string _name)
        {
            GC2GS_014_001_ReqSetChildName protocol = new GC2GS_014_001_ReqSetChildName();
            protocol.setId(_childId);
            protocol.setName(_name);
            return protocol;
        }
        public static GC2GS_014_002_ReqTrainChild make_002_ReqTrainChild(long _childId)
        {
            GC2GS_014_002_ReqTrainChild protocol = new GC2GS_014_002_ReqTrainChild();
            protocol.setId(_childId);
            return protocol;
        }
        public static GC2GS_014_003_ReqAddSeatEnergy make_003_ReqAddSeatEnergy(long _seatId, int _addCount)
        {
            GC2GS_014_003_ReqAddSeatEnergy protocol = new GC2GS_014_003_ReqAddSeatEnergy();
            protocol.setSeatId(_seatId);
            protocol.setAddCount(_addCount);
            return protocol;
        }
        public static GC2GS_014_004_ReqSetChildGraduate make_004_ReqSetChildGraduate(long _childId)
        {
            GC2GS_014_004_ReqSetChildGraduate protocol = new GC2GS_014_004_ReqSetChildGraduate();
            protocol.setId(_childId);
            return protocol;
        }
        public static GC2GS_014_006_ReqGetUnMarriedAdult make_006_ReqGetUnMarriedAdult(long _adultId)
        {
            GC2GS_014_006_ReqGetUnMarriedAdult protocol = new GC2GS_014_006_ReqGetUnMarriedAdult();
            protocol.setAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_007_ReqGetMarriedAdult make_007_ReqGetMarriedAdult(long _adultId)
        {
            GC2GS_014_007_ReqGetMarriedAdult protocol = new GC2GS_014_007_ReqGetMarriedAdult();
            protocol.setAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_008_ReqGetToMeApply make_008_ReqGetToMeApply(long _adultId)
        {
            GC2GS_014_008_ReqGetToMeApply protocol = new GC2GS_014_008_ReqGetToMeApply();
            protocol.setApplyAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_009_ReqRefuseToMeApply make_009_ReqRefuseToMeApply(long _adultId)
        {
            GC2GS_014_009_ReqRefuseToMeApply protocol = new GC2GS_014_009_ReqRefuseToMeApply();
            protocol.setApplyAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_010_ReqAkeyRefuseToMeApply make_010_ReqAkeyRefuseToMeApply()
        {
            GC2GS_014_010_ReqAkeyRefuseToMeApply protocol = new GC2GS_014_010_ReqAkeyRefuseToMeApply();
            return protocol;
        }
        public static GC2GS_014_011_ReqAgreeToMeApply make_011_ReqAgreeToMeApply(long _adultId, long _engageAdultId)
        {
            GC2GS_014_011_ReqAgreeToMeApply protocol = new GC2GS_014_011_ReqAgreeToMeApply();
            protocol.setAdultId(_adultId);
            protocol.setApplyAdultId(_engageAdultId);
            return protocol;
        }
        public static GC2GS_014_012_ReqGetRecommendPlayerList make_012_ReqGetRecommendPlayerList(long _adultId)
        {
            GC2GS_014_012_ReqGetRecommendPlayerList protocol = new GC2GS_014_012_ReqGetRecommendPlayerList();
            protocol.setAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_013_ReqApplyToPlayer make_013_ReqApplyToPlayer(long _targetCid, long _adultId)
        {
            GC2GS_014_013_ReqApplyToPlayer protocol = new GC2GS_014_013_ReqApplyToPlayer();
            protocol.setTargetCid(_targetCid);
            protocol.setAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_014_ReqApplyToGroup make_014_ReqApplyToGroup(long _adultId, long _limitEarning)
        {
            GC2GS_014_014_ReqApplyToGroup protocol = new GC2GS_014_014_ReqApplyToGroup();
            protocol.setAdultId(_adultId);
            protocol.setMinValue(_limitEarning);
            return protocol;
        }
        public static GC2GS_014_015_ReqAgreeApplyGroup make_015_ReqAgreeApplyGroup(long _adultId, long _engageAdultId, long _engageCid)
        {
            GC2GS_014_015_ReqAgreeApplyGroup protocol = new GC2GS_014_015_ReqAgreeApplyGroup();
            protocol.setAdultId(_adultId);
            protocol.setApplyCid(_engageCid);
            protocol.setApplyAdultId(_engageAdultId);
            return protocol;
        }
        public static GC2GS_014_016_ReqCancelApplyToPlayer make_016_ReqCancelApplyToPlayer(long _adultId)
        {
            GC2GS_014_016_ReqCancelApplyToPlayer protocol = new GC2GS_014_016_ReqCancelApplyToPlayer();
            protocol.setAdultId(_adultId);
            return protocol;
        } 
        public static GC2GS_014_017_ReqCancelApplyToGroup make_017_ReqCancelApplyToGroup(long _adultId)
        {
            GC2GS_014_017_ReqCancelApplyToGroup protocol = new GC2GS_014_017_ReqCancelApplyToGroup();
            protocol.setAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_018_ReqGetPoolAdult make_018_ReqGetPoolAdult(long _cid, long _adultId)
        {
            GC2GS_014_018_ReqGetPoolAdult protocol = new GC2GS_014_018_ReqGetPoolAdult();
            protocol.setCid(_cid);
            protocol.setAdultId(_adultId);
            return protocol;
        }
        public static GC2GS_014_030_ReqCidAdultIsMarried make_030_ReqCidAdultIsMarried(long _cid, long _adultId)
        {
            GC2GS_014_030_ReqCidAdultIsMarried protocol = new GC2GS_014_030_ReqCidAdultIsMarried();
            protocol.setCid(_cid);
            protocol.setAdultId(_adultId);
            return protocol;
        }
    }
}
