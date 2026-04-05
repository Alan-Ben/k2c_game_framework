package NPUSServer.CommonActivityMgr.Activities.EarningsGoal;

public enum EEarningsGoalType
{
    REWARD,
    HONOR_REWARD;

    public static final EEarningsGoalType[] E_EARNING_GOAL_TYPE___VALUES = EEarningsGoalType.values();
    public static final int EEarningsGoalType_Length = E_EARNING_GOAL_TYPE___VALUES.length;

    public static EEarningsGoalType EEarningsGoalType_FromInt(int _ivalue)
    {
        if (_ivalue < 0 || _ivalue >= EEarningsGoalType_Length)
        {
            return null;
        }
        return E_EARNING_GOAL_TYPE___VALUES[_ivalue];
    }
}
