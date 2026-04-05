package NPGameRes.GameObjs.PlayerEffect;

import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.*;

public abstract class _ANPPlayerEffectInfo
{
    /************
     * 效果类型
     **/
    public abstract ENPPlayerEffectType effectType();

    /***********************
     * 获取本对象的实际条件数据对象
     */
    public static _ANPPlayerEffectInfo readEffectInfo(ENPPlayerEffectType _effectType, String _infoStr)
    {
        switch (_effectType)
        {
            ///////////////////// 客户端效果，不做检测
            case NONE:
            case C_ANI_G_PLAY:
            case C_ANI_G_SET_BOOL:
            case C_ANI_G_SET_FLOAT:
            case C_ANI_G_SET_INT:
            case C_ANI_PLAY:
            case C_ANI_SET_BOOL:
            case C_ANI_SET_FLOAT:
            case C_ANI_SET_INT:
            case C_DEAL_ID:
            case C_ENTER_ACTOR_TEST:
            case C_SHOW_ERR:
            case C_SHOWCASE_GRAY:
            case C_SHOWCASE_LOAD:
            case C_SPECIAL:
            case C_SPECIAL_DIALOGUE:
            case C_T_ENTER_DUNGEON:
            case C_UI_SHOW_CUST_WND:
            case C_UI_TO_SYS_SCE:
            case C_USE_BAG_ITEM:
                return new NPPlayerEffect_NONE();

            ///////////////////////////////////////////////////////

            case S_R_EFFECT:
                return NPPlayerEffect_S_R_EFFECT.readStr(_infoStr);
            case S_GAIN_ITEM:
                return NPPlayerEffect_S_GAIN_ITEM.readStr(new NPStringReader(_infoStr));
            case S_SPEND_ITEM:
                return NPPlayerEffect_S_SPEND_ITEM.readStr(new NPStringReader(_infoStr));
            case S_GAIN_ITEM_FORM:
            	return NPPlayerEffect_S_GAIN_ITEM_FORM.readStr(new NPStringReader(_infoStr));
            case S_SET_BUFF:
                return NPPlayerEffect_S_SET_BUFF.readStr(new NPStringReader(_infoStr));
            case S_CHG_BUFF:
                return NPPlayerEffect_S_CHG_BUFF.readStr(new NPStringReader(_infoStr));
            case S_DEL_BUFF:
                return NPPlayerEffect_S_DEL_BUFF.readStr(_infoStr);
            case S_GAIN_REWARD:
                return NPPlayerEffect_S_GAIN_REWARD.readStr(new NPStringReader(_infoStr));
            case S_START_QUEST:
                return NPPlayerEffect_S_START_QUEST.readStr(new NPStringReader(_infoStr));
            case S_QUEST_COUNTER_CHG:
                return NPPlayerEffect_S_QUEST_COUNTER_CHG.readStr(new NPStringReader(_infoStr));
            case S_RECORD_CHG:
                return NPPlayerEffect_S_RECORD_CHG.readStr(new NPStringReader(_infoStr));
            case S_EVENT_RECORD_CHG:
                return NPPlayerEffect_S_EVENT_RECORD_CHG.readStr(new NPStringReader(_infoStr));
            case S_DEAL_ID:
                return NPPlayerEffect_S_DEAL_ID.readStr(new NPStringReader(_infoStr));
            case S_RND_GAIN_X_BUSINESS_WORKERS:
                return NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS.readStr(new NPStringReader(_infoStr));
            case S_ANECDOTE_REFRESH:
                return NPPlayerEffect_S_ANECDOTE_REFRESH.readStr(new NPStringReader(_infoStr));
            case S_GAIN_X_CONSORT_CHARM_FROM:
                return NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_FROM.readStr(new NPStringReader(_infoStr));
            case S_GAIN_X_CONSORT_INTIMACY_FROM:
                return NPPlayerEffect_S_GAIN_X_CONSORT_INTIMACY_FROM.readStr(new NPStringReader(_infoStr));
            case S_GAIN_X_CONSORT_LIKE_FROM:
            	return NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM.readStr(new NPStringReader(_infoStr));
            case S_GAIN_LAZY_CD:
                return NPPlayerEffect_S_GAIN_LAZY_CD.readStr(new NPStringReader(_infoStr));
            case S_ADD_P_V:
            	return NPPlayerEffect_S_ADD_P_V.readStr(new NPStringReader(_infoStr));
            case S_ADD_P_V_FORM:
            	return NPPlayerEffect_S_ADD_P_V_FORM.readStr(new NPStringReader(_infoStr));
            case S_LEVY_ADD_COUNT:
            	return NPPlayerEffect_S_LEVY_ADD_COUNT.readStr(new NPStringReader(_infoStr));
            case S_GAIN_AND_SET_CUTE_ACTOR:
            	return NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR.readStr(new NPStringReader(_infoStr));
            case S_CHG_LEVY_SILVER_TIME:
            	return NPPlayerEffect_S_CHG_LEVY_SILVER_TIME.readStr(new NPStringReader(_infoStr));
            case S_UNLOCK_SEVEN_DAYS_LOGIN:
                return NPPlayerEffect_S_UNLOCK_SEVEN_DAYS_LOGIN.readStr(_infoStr);
            case S_GAIN_X_CONSORT_CHARM_POINT_FROM:
            	return NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM.readStr(new NPStringReader(_infoStr));
            case S_GAIN_X_CHILD_EXP_POINT_FROM:
            	return NPPlayerEffect_S_GAIN_X_CHILD_EXP_POINT_FROM.readStr(new NPStringReader(_infoStr));
            case S_MARS_CHG_MOOD:
            	return NPPlayerEffect_S_MARS_CHG_MOOD.readStr(new NPStringReader(_infoStr));
            case S_MARS_CURE:
            	return NPPlayerEffect_S_MARS_CURE.readStr(new NPStringReader(_infoStr));
            case S_MARS_RND_B_RND_SICK:
            	return NPPlayerEffect_S_MARS_RND_B_RND_SICK.readStr(_infoStr);
            case S_MARS_RND_B_RND_LOST:
            	return NPPlayerEffect_S_MARS_RND_B_RND_LOST.readStr(_infoStr);
            case S_MARS_GAIN_ENERGY:
            	return NPPlayerEffect_S_MARS_GAIN_ENERGY.readStr(new NPStringReader(_infoStr));
            case S_GAIN_REWARD_FORM:
            	return NPPlayerEffect_S_GAIN_REWARD_FORM.readStr(new NPStringReader(_infoStr));
            case S_INN_ADD_GUEST:
                return NPPlayerEffect_S_INN_ADD_GUEST.readStr(new NPStringReader(_infoStr));
            case S_CHG_BIRTH_GIFTDE_COUM:
                return NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM.readStr(new NPStringReader(_infoStr));
            default:
                return null;
        }
    }
}
