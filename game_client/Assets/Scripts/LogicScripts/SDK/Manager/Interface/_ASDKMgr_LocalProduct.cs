using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using MJSDK_Package;

namespace GOESDK
{
    /// <summary>
    /// 充值商品档位本地化相关方法
    /// </summary>
    public abstract partial class _ASDKMgr
    {
        //处理流程对象
        private ALProcess _m_pLocalProductProcessObj;
        //是否已经初始化过商品档位本地化信息
        private bool _m_bIsInitLoaclProductSuc;
        //商品档位本地化信息字典，<app_product_id,商品本地化信息>
        [NotNull] private Dictionary<string, MJSDK_ThirdParty_local_product> _m_dLocalProductDic = new Dictionary<string, MJSDK_ThirdParty_local_product>();

        /// <summary>
        /// 初始化商品档位本地化信息
        /// </summary>
        public void initLocalProduct(Action _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("initLocalProduct", _failDelegate))
                return;

            //如果已经初始化成功过，则不再重复初始化
            if (_m_bIsInitLoaclProductSuc)
            {
                _sucDelegate?.Invoke();
                return;
            }

            //创建处理流程对象
            _m_pLocalProductProcessObj = ALProcess.CreateProcess("initLocalProduct");
            _m_pLocalProductProcessObj
            //1、先检查token是否过期
            .addDelegateProcess(_onDone => { _checkTokenExpire(_onDone, _failDelegate); })
            //2、处理本地化商品档位信息
            .addDelegateProcess(_onDone => { _initLocalProductProcess(_onDone, _failDelegate); })
            //3、成功回调
            .addProcess(_sucDelegate)
            .deal();
        }

        /// <summary>
        /// 获取本地化商品信息
        /// </summary>
        /// <param name="_payRefId">支付档位表id</param>
        public MJSDK_ThirdParty_local_product getLocalProduct(long _payRefId)
        {
            PayRefObj payRefObj = GRefdataCoreMgr.instance.payRefCore.getRef(_payRefId);
            if (payRefObj == null)
            {
                SDKUtil.showSDKLogError($"[getLocalProduct] 未获取到对应的支付档位信息，payId：{_payRefId}");
                return null;
            }

            return getLocalProduct(payRefObj.sdk_pay_id);
        }

        /// <summary>
        /// 获取本地化商品信息
        /// </summary>
        /// <param name="_appProductId">后台商品支付id</param>
        public MJSDK_ThirdParty_local_product getLocalProduct(string _appProductId)
        {
            MJSDK_ThirdParty_local_product localProduct = null;
            _m_dLocalProductDic.TryGetValue(_appProductId, out localProduct);

            if(localProduct == null)
                SDKUtil.showSDKLogError($"[getLocalProduct] 未获取到对应本地化商品信息，appProductId：{_appProductId}");

            return localProduct;
        }

        /// <summary>
        /// 获取本地价格展示
        /// </summary>
        /// <param name="_payRefId">支付档位表id</param>
        /// <returns></returns>
        public string getLocalShowPrice(long _payRefId)
        {
            PayRefObj payRefObj = GRefdataCoreMgr.instance.payRefCore.getRef(_payRefId);
            if(payRefObj == null)
            {
                SDKUtil.showSDKLogError($"[getLocalShowPrice] 未获取到对应的支付档位信息，payId：{_payRefId}");
                return string.Empty;
            }

            //如果未使用SDK，则直接返回档位展示价格
            string showPrice = string.Format($"${payRefObj.show_price}");
            if (!isUseSDK)
                return showPrice;

            MJSDK_ThirdParty_local_product localProduct = getLocalProduct(payRefObj.sdk_pay_id);
            if (localProduct == null)
            {
                SDKUtil.showSDKLogError($"[getLocalShowPrice] 未获取到对应的本地化商品信息，sdk_pay_id：{payRefObj.sdk_pay_id}");
                return showPrice;
            }

            //拼接货币类型和价格，如USD 0.99
            return TextTranslate.instance.getLanguage(TransKeyConst.common_twoParamWithSpace_str_str, localProduct.currency, localProduct.price);
        }

        /// <summary>
        /// 处理本地化商品档位信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _initLocalProductProcess(Action _sucDelegate, Action<int, string> _failDelegate)
        {
            //获取token
            SDKTokenData tokenData = SDKLoginSetting.instance.getTokenData();
            //设置信息
            MJSDK_PhpApiCommon_2SDK_get_product getProduct = new MJSDK_PhpApiCommon_2SDK_get_product();
            getProduct.token = tokenData?.token;//用户登录令牌
            getProduct.pay_type = _getPayTypeStr();//支付类型, 微信：wechat , 支付宝：alipay , 苹果：apple , 谷歌：google

            //获取档位本地化
            MJSDK_Mgr_Pay.req_localProduct(_getPayLibType(), getProduct, (_infoList) =>
            {
                //有数据，记录到本地化商品字典中
                if (_infoList != null && _infoList.product != null)
                {
                    for (int i = 0; i < _infoList.product.Count; i++)
                    {
                        if (_infoList.product[i] == null)
                            continue;

                        if (!string.IsNullOrEmpty(_infoList.product[i].app_product_id))
                            _m_dLocalProductDic[_infoList.product[i].app_product_id] = _infoList.product[i];
                    }
                }
                _m_bIsInitLoaclProductSuc = true;
                _sucDelegate?.Invoke();
            }, _failDelegate);
        }
    }
}
