package NPCommon.Util;

import NPCommon.Log.CommLog;
import WCGCommon.Exception.RequestException;
import WCGCommon.Security.MD5;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import org.apache.http.NameValuePair;

import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.*;

public class HttpUtils
{
    static private SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    static public final String SIGN_KEY = "7b20b1b0fa89db3886e455a4c4ab8251";
    static public final String SIGN_KEY_RECHARGE = "nhUeW6FY29MOXrHjx7nkUAPMdfJ8Sosq";

    public static Map<String, String> abstractHttpParams(String query)
    {
        Map<String, String> map = new TreeMap<String, String>();
        if (query == null || query.trim().isEmpty())
        {
            return map;
        }
        String[] arrayStr = query.split("&");
        for (String str : arrayStr)
        {
            if (str == null || str.isEmpty())
            {
                continue;
            }
            int sign = str.indexOf('=');
            if (sign >= 0)
            {
                map.put(str.substring(0, sign), str.substring(sign + 1));
            } else
            {
                map.put(str, "");
            }
        }
        return map;
    }

    public static JsonObject abstractGMParams(String query) throws Exception
    {
        return abstractGMParams(query, SIGN_KEY, null);
    }

    public static JsonObject abstractGMParams(String query, String signkey, List<String> _ignoreList) throws Exception
    {
        if (query == null)
        {
            throw new RequestException(30001, "消息为空");
        }
        query = query.trim();
        if (query.isEmpty())
        {
            throw new RequestException(30001, "消息为空");
        }

        JsonObject jsonObject = null;
        try
        {
            jsonObject = new JsonParser().parse(query).getAsJsonObject();
        } catch (Exception e)
        {
            throw new RequestException(30002, "发送的post数据无法解析出相关活动参数");
        }

        TreeMap<String, JsonElement> params = new TreeMap<>();
        for (java.util.Map.Entry<String, JsonElement> pair : jsonObject.entrySet())
        {
            params.put(pair.getKey(), pair.getValue());
        }

        String sign = params.get("sign").getAsString();
        if (sign == null)
        {
            throw new RequestException(30002, "缺少参数[sign]");
        }
        StringBuilder signsrc = new StringBuilder();

        for (java.util.Map.Entry<String, JsonElement> sendparam : params.entrySet())
        {
            if (sendparam.getKey().equalsIgnoreCase("sign"))
            {
                continue;
            }
            boolean isIgnore = false;
            if (_ignoreList != null)
            {
                for (String ignore : _ignoreList)
                {
                    if (sendparam.getKey().equalsIgnoreCase(ignore))
                    {
                        isIgnore = true;
                        break;
                    }
                }
            }
            if (isIgnore) continue;

            if (sendparam.getValue() == null || sendparam.getValue().isJsonNull())
            {
                throw new RequestException(30002, String.format("key=%s的值为null", sendparam.getKey()));
            }
            String value = null;
            if (sendparam.getValue().isJsonObject() || sendparam.getValue().isJsonArray())
            {
                value = sendparam.getValue().toString();
                value = toUnicode(value);
            } else
            {
                value = sendparam.getValue().getAsString();

            }
            value = URLEncoder.encode(value, "utf-8");
            signsrc.append(sendparam.getKey()).append("=").append(value).append("&");
        }
        signsrc = signsrc.append(signkey);
        String s = MD5.md5(signsrc.toString());
        if (!sign.equalsIgnoreCase(s))
        {
            CommLog.info("原串:{},预期签名:{},传参签名:{}", signsrc.toString(), s, sign);
            throw new RequestException(30002, "签名错误");
        }
        return jsonObject;
    }

    private static String toUnicode(String asciicode)
    {
        char[] utfBytes = asciicode.toCharArray();
        StringBuilder unicodeBytes = new StringBuilder();
        for (int byteIndex = 0; byteIndex < utfBytes.length; byteIndex++)
        {
            String hexB = Integer.toHexString(utfBytes[byteIndex]);
            if (hexB.length() <= 2)
            {
                unicodeBytes.append(utfBytes[byteIndex]);
            } else
            {
                unicodeBytes.append("\\u" + hexB);
            }
        }
        return unicodeBytes.toString();
    }

    public static int getTime(JsonObject json, String name) throws RequestException
    {
        try
        {
            return (int) (sdf.parse(json.get(name).getAsString()).getTime() / 1000);
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少int格式参数[%s]", name);
        }
    }

    public static int getTime(JsonObject json, String name, int def) throws RequestException
    {
        JsonElement value = json.get(name);
        if (value == null)
        {
            return def;
        }
        try
        {
            return (int) (sdf.parse(value.getAsString()).getTime() / 1000);
        } catch (Exception e)
        {
            throw new RequestException(30002, "参数[%s]无法解析出int值", name);
        }
    }

    public static <T extends Enum<T>> T getEnum(JsonObject json, String name, Class<T> class1) throws RequestException
    {
        try
        {
            return Enum.valueOf((Class<T>) class1, json.get(name).getAsString());
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少[%s]格式参数[%s]", class1.getSimpleName(), name);
        }
    }

    public static boolean getBool(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsBoolean();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少bool格式参数[%s]", name);
        }
    }

    public static boolean getBool(JsonObject json, String name, boolean def)
    {
        try
        {
            return json.get(name).getAsBoolean();
        } catch (Exception e)
        {
            return def;
        }
    }

    public static JsonObject getJsonObject(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsJsonObject();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少JsonObject格式参数[%s]", name);
        }
    }

    public static JsonArray getJsonArray(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsJsonArray();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少JsonArray格式参数[%s]", name);
        }
    }

    public static int getInt(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsInt();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少Int格式参数[%s]", name);
        }
    }

    public static int getInt(JsonObject json, String name, int defaultInt) throws RequestException
    {
        try
        {
            return json.get(name).getAsInt();
        } catch (Exception e)
        {
            return defaultInt;
        }
    }

    public static long getLong(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsLong();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少Long格式参数[%s]", name);
        }
    }

    public static double getDouble(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsDouble();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少Double格式参数[%s]", name);
        }
    }

    public static String getString(JsonObject json, String name) throws RequestException
    {
        try
        {
            return json.get(name).getAsString();
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少string格式参数[%s]", name);
        }
    }

    public static String getString(JsonObject json, String name, String def) throws RequestException
    {
        JsonElement value = json.get(name);
        if (value == null)
        {
            return def;
        }
        try
        {
            return value.getAsString();
        } catch (Exception e)
        {
            throw new RequestException(30002, "参数[%s]无法解析出string值", name);
        }
    }

    public static String getString(JsonObject json, String name, boolean isDecodeUnicode) throws RequestException
    {
        try
        {
            String string = json.get(name).getAsString();
            if (isDecodeUnicode)
            {
                return decodeUnicode(string);
            }
            return string;
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少string格式参数[%s]", name);
        }
    }

    public static List<Integer> getIntList(JsonObject json, String name) throws RequestException
    {
        List<Integer> list = new ArrayList<>();
        try
        {
            JsonArray jsonArray = json.get(name).getAsJsonArray();
            for (JsonElement itemElement : jsonArray)
            {
                list.add(itemElement.getAsInt());
            }
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少Integer格式参数[%s]", name);
        }
        return list;
    }

    public static ArrayList<Long> getLongList(JsonObject json, String name) throws RequestException
    {
        ArrayList<Long> list = new ArrayList<>();
        try
        {
            JsonArray jsonArray = json.get(name).getAsJsonArray();
            for (JsonElement itemElement : jsonArray)
            {
                list.add(itemElement.getAsLong());
            }
        } catch (Exception e)
        {
            throw new RequestException(30002, "缺少Long格式参数[%s]", name);
        }
        return list;
    }

    /****************************
     * 把中文转成Unicode码
     *
     * @param str
     * @return
     */
    public static String chinaToUnicode(String str)
    {
        String result = "";
        for (int i = 0; i < str.length(); i++)
        {
            int chr1 = (char) str.charAt(i);
            if (chr1 >= 19968 && chr1 <= 171941)
            {
                result += "\\u" + Integer.toHexString(chr1);
            } else
            {
                result += str.charAt(i);
            }
        }
        return result;
    }

    /****************************
     * Unicode转中文
     *
     * @param theString
     * @return
     */
    public static String decodeUnicode(String theString)
    {
        char aChar;
        int len = theString.length();
        StringBuffer outBuffer = new StringBuffer(len);
        for (int x = 0; x < len; )
        {
            aChar = theString.charAt(x++);
            if (aChar == '\\')
            {
                aChar = theString.charAt(x++);
                if (aChar == 'u')
                {
                    int value = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        aChar = theString.charAt(x++);
                        switch (aChar)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                value = (value << 4) + aChar - '0';
                                break;
                            case 'a':
                            case 'b':
                            case 'c':
                            case 'd':
                            case 'e':
                            case 'f':
                                value = (value << 4) + 10 + aChar - 'a';
                                break;
                            case 'A':
                            case 'B':
                            case 'C':
                            case 'D':
                            case 'E':
                            case 'F':
                                value = (value << 4) + 10 + aChar - 'A';
                                break;
                            default:
                                throw new IllegalArgumentException("Malformed \\uxxxx encoding.");
                        }
                    }
                    outBuffer.append((char) value);
                } else
                {
                    if (aChar == 't')
                        aChar = '\t';
                    else if (aChar == 'r')
                        aChar = '\r';
                    else if (aChar == 'n')
                        aChar = '\n';
                    else if (aChar == 'f')
                        aChar = '\f';
                    outBuffer.append(aChar);
                }
            } else
                outBuffer.append(aChar);
        }
        return outBuffer.toString();
    }

    /**
     * WebPay接口签名校验（表单方式）
     *
     * 验证流程：
     * 1. 参数按名称排序
     * 2. 拼接格式：name=value（跳过sign字段）
     * 3. 追加密钥
     * 4. MD5加密并转小写
     * 5. 对比客户端签名
     *
     * @param _parseList 已解析的表单参数列表
     * @param _signKey 签名密钥
     * @return 校验结果，true表示签名正确
     */
    public static boolean checkWebPaySign(List<NameValuePair> _parseList, String _signKey)
    {
        // 参数按名称排序
        _parseList.sort(Comparator.comparing(NameValuePair::getName));

        String clientSign = "";
        StringBuilder sb = new StringBuilder();

        for (NameValuePair nameValuePair : _parseList)
        {
            // 跳过签名字段
            if ("sign".equals(nameValuePair.getName()))
            {
                clientSign = nameValuePair.getValue();
                continue;
            }
            sb.append(nameValuePair.getName()).append("=").append(nameValuePair.getValue());
        }

        // 追加密钥
        sb.append(_signKey);

        // MD5加密并转小写
        String sign = MD5.md5(sb.toString()).toLowerCase(Locale.ROOT);

        return sign.equals(clientSign);
    }
}
