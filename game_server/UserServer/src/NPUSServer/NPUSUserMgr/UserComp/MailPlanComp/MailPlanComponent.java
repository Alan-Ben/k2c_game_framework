package NPUSServer.NPUSUserMgr.UserComp.MailPlanComp;

import Common.MailObj.Mail_Data;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mail.RefMailPlan;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;

/**
 * MailPlanComponent - 邮件计划组件
 *
 * 主要功能：
 * 1. 根据服务器开启天数自动发送邮件计划
 * 2. 基于RefMailPlan配表配置邮件内容和奖励
 * 3. 通过MAIL_PLAN_HAD_SEND_DAYS记录防止重复发送
 *
 * 触发时机：
 * 1. 玩家初始化登录时检查
 * 2. 每日跨天事件触发检查
 *
 * 业务逻辑：
 * - 发送范围：从上次发送天数+1到当前服务器开启天数
 * - 邮件模板：使用RefGeneral.mail_plan_mail_id配置的邮件模板
 * - 奖励转换：自动将配表中的NPCommonCostItem转换为邮件奖励
 *
 * 设计特点：
 * - 无需数据库表，基于记录组件存储已发送天数状态
 * - 轻量级组件，纯逻辑处理
 * - 与MailComponent协作完成邮件实际发送
 *
 * 线程安全：通过玩家级别锁保证数据一致性
 */
public class MailPlanComponent extends _ANPUserComponent implements _IHandlerHolder
{
    public MailPlanComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MAIL_PLAN_COMP);
    }

    @Override
    protected void _init()
    {
        // 邮件计划组件无需异步初始化，直接标记完成
        setInited();
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        // 邮件计划组件依赖记录组件存储状态，依赖邮件组件发送邮件
        // 但由于组件加载顺序已确保，此处返回null即可
        return null;
    }

    @Override
    public void onInited()
    {
        // 注册跨天事件监听器，每日0点检查是否有新的邮件计划需要发送
        getUserData().OnCrossDay.addHandler(this, new HandlerOne<Integer>()
        {
            @Override
            public void handle(Integer _nowTag)
            {
                checkAndSendMailPlans(getUserData().getPlayerInitContext());
            }
        });

        // 玩家登录时立即检查一次，补发遗漏的邮件计划
        checkAndSendMailPlans(getUserData().getPlayerInitContext());
    }

    @Override
    public void dispose()
    {
        getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
    }

    /**
     * 检查并发送邮件计划 - 核心业务方法
     *
     * 执行逻辑：
     * 1. 获取服务器当前开启天数作为发送上限
     * 2. 获取玩家已发送的最大天数作为起点
     * 3. 遍历RefMailPlan配表，发送在范围内的邮件计划
     * 4. 更新玩家的已发送天数记录
     *
     * 业务规则：
     * - 只发送day在(已发送天数, 服务器开启天数]范围内的邮件
     * - 使用RefGeneral.mail_plan_mail_id作为邮件模板ID
     * - 自动转换配表奖励格式为邮件奖励格式
     *
     * @param _context 操作上下文，用于事件跟踪和审计
     *
     * 线程安全：通过玩家级别锁保证数据一致性
     */
    public void checkAndSendMailPlans(NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            // 获取服务器开启天数，作为邮件发送的上限
            int serverStartDay = getUSServer().getServerStartDay();

            // 获取玩家已发送的最大天数，作为发送的起点
            long currentSentDays = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.MAIL_PLAN_HAD_SEND_DAYS);

            // 如果已经发送到当前服务器天数，无需处理
            if (currentSentDays >= serverStartDay)
                return;

            // 遍历邮件计划配表，发送符合条件的邮件
            boolean hasSentMail = false;
            long maxSentDay = currentSentDays;

            for (RefMailPlan ref : RefMailPlan.getMgr().getList())
            {
                if (ref == null)
                    continue;

                // 跳过已发送的邮件计划
                if (ref.day <= currentSentDays)
                    continue;

                // 超过服务器开启天数的邮件计划暂不发送
                if (ref.day > serverStartDay)
                    break;

                // 构造并发送邮件
                Mail_Data mailData = new Mail_Data();
                // 使用配置的邮件模板ID
                mailData.setMailRefId(RefGeneral.Ref().mail_plan_mail_id);
                // 将配表中的奖励物品转换为邮件奖励格式
                mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(ref.item_list));
                // 通过邮件组件发送邮件
                getUserData().getMailComponent().addMail(mailData, _context);

                hasSentMail = true;
                maxSentDay = ref.day;
            }

            // 更新玩家的已发送天数记录，防止重复发送
            if (hasSentMail)
            {
                getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.MAIL_PLAN_HAD_SEND_DAYS, maxSentDay, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }
}