using GOE;
namespace Common
{
/****
 玩家相关错误
 ****/
    class PlayerErr
    {
        static PlayerErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190002, "玩家已经解锁功能"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190003, "没有可以领取的奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190004, "已经领取拜访奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190005, "玩家用户名重复"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190006, "玩家用户名包含非法字符"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190007, "玩家Q版形象不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190008, "玩家头衔不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190009, "玩家头像不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190010, "玩家头像框不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190011, "玩家聊天气泡框不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190012, "玩家账号被冻结"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190013, "领取离线奖励失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190014, "屏蔽玩家数量超过上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190015, "屏蔽玩家禁止"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190016, "对方玩家屏蔽了当前玩家"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190017, "周卡免费试用已使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190018, "今日已点赞"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190019, "今日点赞次数达到上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190020, "不能给自己点赞"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190021, "已经兑换过"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190022, "抽奖累计奖励兑换积分不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190023, "竞技场贸易站没有产出"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190024, "竞技场贸易站已达最高等级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190025, "目标奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190026, "目标奖励还没达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190027, "刷新时间未到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190028, "已经领取过每日奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190029, "没有每日奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190031, "七日登录奖励未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190032, "七日登录奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190033, "七日登录奖励不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190034, "赚速目标奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190035, "赚速目标奖励未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190036, "VIP等级奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190037, "VIP等级奖励未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190038, "首充礼包未购买"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190039, "首充奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190040, "首充奖励天数未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190041, "首充奖励领取失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190042, "充值返利档位未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190043, "充值返利奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190044, "充值返利档位不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190045, "权益卡不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190046, "权益卡未激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190047, "权益卡当日奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190048, "商店评价已设置"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190050, "玩家房间皮肤不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190051, "玩家不能举报自己"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(190052, "玩家举报内容"));
        }
    }
}
