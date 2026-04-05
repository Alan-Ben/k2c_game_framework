using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{

    /// <summary>
    /// SDK服务端（登录/支付）错误码
    /// </summary>
    public class MJSDK_PhpApi_LoginOrPay_ErrorCode
    {
        public const int C_Unity_Php_SUCC = 200;//	成功  ok
        public const int C_Unity_Php_SERVER_ERR = 500;//	服务器发生错误 error msg
        public const int C_Unity_Php_ParameterIncomplete = 10000;//   参数缺失    Parameter incomplete
        public const int C_Unity_Php_SignError = 10001;// 签名错误    sign error
        public const int C_Unity_Php_TokenErrorOrTimeout = 10002;// token错误或者超时 token error or timeout
        public const int C_Unity_Php_AccountAlreadyExists = 10003;// 帐号已存在   Account already exists
        public const int C_Unity_Php_TheUserDoesNotExist = 10004;//    用户不存在   The user does not exist
        public const int C_Unity_Php_UserStatusException = 10005;//  用户状态异常  User status exception
        public const int C_Unity_Php_App_idIsNotExists = 10006;//   应用不存在   app_id is not exists
        public const int C_Unity_Php_App_idHasNoPermissionToLogin = 10007;//  应用没有开放登录    app_id has no permission to login
        public const int C_Unity_Php_App_idHasNoPermissionToRecharge = 10008;//   应用没有开放充值    app_id has no permission to recharge
        public const int C_Unity_Php_TheProductIdIsNotConfigured = 10009;//   没有配置对应的产品ID The product id is not configured
        public const int C_Unity_Php_OrderMarkPaymentFailed = 10010;//  订单标记支付失败    Order mark payment failed
        public const int C_Unity_Php_AbnormalProductId = 10011;// productId 异常    Abnormal productId
        public const int C_Unity_Php_UidDoesNotMatch = 10012;// Uid 不匹配 Uid does not match
        public const int C_Unity_Php_AccountTokenErrorOrTimeout = 10013;// account token 错误或者超时    account token error or timeout
        public const int C_Unity_Php_AccountDoesNotExist = 10014;// 帐号不存在   Account does not exist
        public const int C_Unity_Php_TheAccountDoesNotExistOrHasBeenBoundByAnotherUser = 10015;//   帐号不存在或已被其他用户绑定  The account does not exist or has been bound by another user
        public const int C_Unity_Php_TheAccountHasBeenBound = 10016;// 帐号已经被绑定 The account has been bound
        public const int C_Unity_Php_TheUserHasAlreadyBoundOtherAccountsOfTheSameType = 10017;// 用户已经绑定其他同类型帐号   The user has already bound other accounts of the same type
        public const int C_Unity_Php_TheUserHasAlreadyBoundOtherAccountsOfTheAameType = 10018;// 该帐号不是绑定该用户  The user has already bound other accounts of the same type
        public const int C_Unity_Php_TheTradeIdRepeatability = 10019;//  第三方订单号重复    The tradeId repeatability
        public const int C_Unity_Php_PaymentNoteTimeout = 10020;//   支付票据超时  Payment note timeout
        public const int C_Unity_Php_TheCallbackProtocolIsNotValid = 10021;// 回调协议不合法 The callback protocol is not valid
        public const int C_Unity_Php_SignTimeout = 10022;//   签名过期    sign timeout
        public const int C_Unity_Php_OrderHasBeenPaid = 10023;//   订单已支付   Order has been paid
        public const int C_Unity_Php_UserDelStatusException = 10040;//   用户状态异常[注销]   User del status exception




        /// <summary>
        /// 解析php服务端错误信息
        /// </summary>
        /// <param name="_msg"></param>
        /// <returns></returns>
        public static MJSDK_PhpErrInfo parsePhpErrorMsg(string _msg)
        {
            MJSDK_PhpErrInfo phpErrInfo = new MJSDK_PhpErrInfo();
            //添加默认值
            phpErrInfo.code = 0;
            phpErrInfo.msg = "解析SDKPhp服务端错误信息失败";

            if (string.IsNullOrEmpty(_msg))
            {
                return phpErrInfo;
            }

            try
            {
                phpErrInfo = JsonUtility.FromJson<MJSDK_PhpErrInfo>(_msg);
                return phpErrInfo;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("【MJSDK-Unity】php服务端错误信息解析失败,reason:" + ex);
                return phpErrInfo;
            }
        }
    }

    /// <summary>
    /// SDK服务端错误信息
    /// </summary>
    public class MJSDK_PhpErrInfo
    {
        //回执消息码
        public int code;
        //回执消息
        public string msg;
    }
}