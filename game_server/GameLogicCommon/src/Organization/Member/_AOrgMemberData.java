package Organization.Member;

/**
 * 成员信息类
 * 作为操作成员信息的基类
 */
public abstract class _AOrgMemberData
{
    /**
     * 获取成员id
     * @return
     */
    public abstract Long getMemberId();

    /**
     * 销毁处理
     */
    public abstract void discard();
}
