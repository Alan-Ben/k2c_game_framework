
using System;
using CommonEnum;

namespace GOE
{
    public interface _IChildInfo
    {
        public long id { get; }
        public GConsortRefObj guardianRef { get; }
        public ChildInitResRefObj initResRef { get; }
        public ChildQualityRefObj qualityRef { get; }
        public ChildCareerRefObj careerRef { get; }
        public ChildAttrRefObj attrRef { get; }
        public BasicAttrRefObj basicAttrRef { get; }
        public long initIntimacy { get; }
        public bool isSuper { get; }
        public ChildResRefObj resRef { get; }
        public string name { get; }
        public EChildSexType sex { get; }
        public JudgeUnionBonusPart[] bonusJudgeParts { get; }
        public long earnings { get; }

        
        public void getPlayerName(Action<string> _complete);
    }
}