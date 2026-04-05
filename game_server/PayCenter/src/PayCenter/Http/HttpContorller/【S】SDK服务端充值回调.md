# 【S】SDK服务端充值回调

**简要描述：**

* 普通订单付款成功后推送说明

**推送方式：**

* POST(application/json)

**参数：**

| 参数名            | 类型     | mysql类型参考     | 说明                          |
|----------------|--------|---------------|-----------------------------|
| type           | int    | tinyint(4)    | 1:普通订单，3:普通订单退款             |
| order\_id      | string | varchar(32)   | 订单id(充值平台订单)                |
| app\_order\_id | string | varchar(64)   | 应用订单id（项目组订单）               |
| uid            | string | varchar(32)   | 用户id                        |
| server\_id     | string | varchar(32)   | 服务器标识                       |
| role\_id       | string | varchar(32)   | 角色id                        |
| app\_id        | string | varchar(16)   | 应用id                        |
| product\_id    | string | varchar(32)   | 游戏内产品id                     |
| product\_name  | string | varchar(32)   | 游戏内产品名称                     |
| amount         | float  | decimal(16,2) | 商品定价价值                      |
| amount\_type   | string | varchar(8)    | 商品定价货币类型                    |
| pay\_type      | string | varchar(16)   | 支付方式（钱包聚合平台）                |
| create\_time   | int    | int(11)       | 创建时间（10位时间戳）                |
| pay\_time      | int    | int(11)       | 支付时间（10位时间戳）                |
| extension      | string | varchar(255)  | 扩展参数                        |
| sdk\_type      | int    | tinyint(4)    | 订单来源:1正常，2补单，3虚拟充值,4测试订单    |
| trade\_id      | string | varchar(64)   | 第三方订单号                      |
| sku\_id        | string | varchar(64)   | 第三方内购产品id                   |
| sdk\_pay\_id   | string | varchar(32)   | sdk档位ID                     |
| purchase\_type | int    | tinyint(4)    | 购买类型：0普通，1测试，2促销，3奖励        |
| payment        | float  | decimal(16,2) | 实际支付金额                      |
| payment\_code  | string | varchar(8)    | 实际支付货币                      |
| channel\_code  | string | varchar(64)   | 渠道标识(网页支付时玩家选择的站点标识)        |
| pay\_id        | string | varchar(64)   | 支付ID（钱包ID）                  |
| order\_type    | int    | tinyint(4)    | 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） |
| refund\_source | int    | tinyint(4)    | 退款来源：-1未知,0用户,1开发者(商户),2第三方 |
| refund\_time   | int    | int(11)       | 收到退款通知时间（10位时间戳）            |
| sign           | string | varchar(32)   | 签名（生成方式见备注，32位MD5）          |

**推送示例**

```plaintext
{
 "type": 1,
    "order_id": "MJ1099820220622153544476FMHE",
    "app_order_id": "1655883343f5d7c0b61b7017ab2982",
    "uid": "601418702260297728",
    "server_id": "2",
    "role_id": "10030020002",
    "app_id": "10998",
    "product_id": "11002",
    "product_name": "\u91d1\u5e01\u8865\u7ed9",
    "amount": "33.00",
    "amount_type": "TWD",
    "pay_type": "webmycard",
    "create_time": 1655883344,
    "pay_time": 1655883485,
    "extension": "",
    "sdk_type": 1,
    "trade_id": "GST2206220000346",
    "sdk_pay_id": "1000099",
    "sku_id": "1000099",
    "purchase_type": 0,
    "payment": "33.000000",
    "payment_code": "TWD",
    "channel_code": "poi_mycard",
    "pay_id": "",
    "refund_source": -1,
    "refund_time": 1683949808,
    "sign": "c7f840ab5d5a1c561c30496e20db1045"
}

```

**返回参数说明**

* 返回规格规定为json格式，必须包含code参数，其中code值为1表示成功，其他值或者不包含code即认为是失败，失败不重推。

* 如果http返回码不是200的话，系统会在间隔 推送次数 \* 1分钟（推送时间 0 1min 3min 6min 10min）
  后再推送，最多推送5次没有得到返回，则不会再进行推送，并且标记该订单异常。

**返回示例**

```plaintext
{
    "code": 1,
    //"msg":"ok",
    //....
}

```

**备注**

* 可能会出现网络波动或者响应超时，如遇到重复推送的订单code直接响应1告知成功，msg说明即可

* 项目组的已知异常逻辑请不要响应异常,SDK的职责主要告知已付款，如果遇到项目组的业务逻辑导致重推都无法到账，code响应1告知成功即可，请项目组自行记录处理。

* 建议校验uid以及cid字段真实性以及对应关系


* sign签名说明  
  **所有参数**排除sign，按参数名进行升序排序，对应的参数值（如果参数值为数组对象，请使用json格式）连接成字符串，paramsString  
  md5(‘MJ’ + md5(paramsString + 密钥));

* **接入时严格按说明实现，切勿写死字段，平台这边可能会随时加入新的字段。否则可能会导致签名过不了。**

_具体拼凑示例_**（只是示例，具体字段按上述文档）**

```plaintext
md5( 'MJ' + md5( amount + amount_type + app_id + create_time + extension + order_id + pay_time + pay_type + product_id + product_name + purchase_type + role_id + sdk_pay_id + sdk_type + server_id + sku_id + trade_id + type + uid + PAYSECRET ) )

```

_PHP示例：_

```plaintext
$paySecret = 'xxxxxxxxxx';//具体找服务端获取
$post = [
    'type' => 3,//退款类型
    'order_id' => $orderInfo['order_id'],//订单id
    'app_order_id' => $orderInfo['app_order_id'],//应用订单id
    'uid' => $orderInfo['uid'],//用户id
    'server_id' => $orderInfo['server_id'],//服务器标识
    'role_id' => $orderInfo['role_id'],//角色id
    'app_id' => $orderInfo['app_id'],//应用id
    'product_id' => $orderInfo['product_id'],//产品id
    'product_name' => $orderInfo['product_name'],//产品名称
    'amount' => $orderInfo['amount'],//价格
    'amount_type' => $orderInfo['amount_type'],//货币类型
    'pay_type' => $orderInfo['pay_type'],//支付渠道
    'create_time' => $orderInfo['create_time'],//创建时间
    'pay_time' => $orderInfo['pay_time'],//支付时间
    'extension' => $orderInfo['extension'],//扩展参数
    'sdk_type' => $orderInfo['order_from'],//订单来源
    'trade_id' => $orderInfo['trade_id'],//第三方支付id
    'sdk_pay_id' => $orderInfo['sdk_pay_id'],//sdk档位ID
    'sku_id' => $orderInfo['sku_id'],//第三方支付id
    'purchase_type' => $orderInfo['purchase_type'],//购买类型
    'payment' => $orderInfo['payment'],//支付金额
    'payment_code' => $orderInfo['payment_code'],//支付货币
    'channel_code' => $orderInfo['channel_code'],//渠道标识
    'pay_id' => $orderInfo['pay_id'],//支付方式
    'order_type' => $orderInfo['order_type'],//订单类型
    'refund_source' => $orderInfo['refund_source'],//退款来源
    'refund_time' => $orderInfo['refund_time'],//收到退款通知时间
];
ksort($post);
unset($post['sign']);
$paramsStr = implode('', array_values($post));
$sign = md5('MJ' . md5($paramsStr . $paySecret));//签名规规则
$post['sign'] = $sign;
```