package NPGameRes.Refs.Anecdote;

public enum EAnecdoteEventType
{
    NONE,
    REWARD,
    EARNINGS,
    CHOICE;

    public static final EAnecdoteEventType[]  EAnecdoteEventType_Values = EAnecdoteEventType.values();
    public static final int EAnecdoteEventType_Length = EAnecdoteEventType_Values.length;
    public static EAnecdoteEventType EAnecdoteEventType_FromInt(int _ivalue) {
        if(_ivalue < 0 || _ivalue >= EAnecdoteEventType_Length){ return null; }
        return EAnecdoteEventType_Values[_ivalue];
    }
}
