<?php
/**
 * Created by PhpStorm.
 * User: mj
 * Date: 2019/7/9
 * Time: 14:03
 */

error_reporting(0);

// 大端序or小端序
define('BIG_ENDIAN', pack('L', 1) === pack('N', 1));

/**
 * 通用方法（PHP5实现支持对Long类型解析）
 * @param $pack
 * @param $value
 * @return array|bool|int
 */
function unpack64($pack, $value)
{
    switch($pack)
    {
        case 'J':
            list($h, $l) = array_values(unpack('N*N*', $value));
            if($h<0) $h += 1<<32;
            if($l<0) $l += 1<<32;
            return (($h<<32) + $l);
        case 'd':
            return BIG_ENDIAN ? unpack('d', $value) : unpack('d', strrev($value));
        default:
            return false;
    }
}

require 'MGHS_ERROR.php';

class MGHS_SOCKET
{
	//请勿使用单例模式，避免config互相覆盖
    public static function getInstance($config = [])
    {
        return new MGHS_SOCKET($config);
    }

    ///////////////////// 实例方法 /////////////////////

    private $_m_arrConfigArr;

    private $_m_stSocket;

    private $_m_iRetCode = MGHS_ERROR::NONE;
    private $_m_sRetMsg = 'success';
    private $_m_arrRetData = [];

    public function __construct($config)
    {
        $this->_m_arrConfigArr = $config;
    }

    /**
     * 重置配置文件
     * @param $config
     * @return $this
     */
    public function setConfig($config)
    {
        $this->_m_arrConfigArr = $config;

        return $this;
    }

    /**
     * HS通信处理
     * @param $mainOrder
     * @param $subOrder
     * @param array $packData
     * @return array
     */
    public function call($mainOrder, $subOrder, $packData = null)
    {
		do
		{
			//检查配置
			if(!$this->_checkConfig())
				break;

			//开启连接
			if(!$this->_openSocket())
				break;

			//检查身份
			if(!$this->_verifyIdentity())
				break;

			//发送消息
			if(!$this->_sendMsg($mainOrder, $subOrder, $packData))
				break;

		} while(false);

		//释放连接
		if(null != $this->_m_stSocket)
		{
			socket_close($this->_m_stSocket);
			$this->_m_stSocket = null;
		}

        return 
		[
            'code' => $this->_m_iRetCode,
            'msg'  => $this->_m_sRetMsg,
            'data' => $this->_m_arrRetData,
        ];
    }
	
    ///////////////////// 私有方法 /////////////////////

    /**
     * 检查配置
     * @return bool
     */
    private function _checkConfig()
    {
        if(!is_array($this->_m_arrConfigArr))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config illegal!';
            return false;
        }

        if(empty($this->_m_arrConfigArr['host']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config error, require host!';
            return false;
        }

        if(empty($this->_m_arrConfigArr['port']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config error, require host!';
            return false;
        }

        if(empty($this->_m_arrConfigArr['client']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config error, require client!';
            return false;
        }

        if(empty($this->_m_arrConfigArr['user']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config error, require user!';
            return false;
        }

        if(empty($this->_m_arrConfigArr['password']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config error, require password!';
            return false;
        }

        if(empty($this->_m_arrConfigArr['key']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_CONFIG;
            $this->_m_sRetMsg = 'config error, require key!';
            return false;
        }

        return true;
    }

    /**
     * 开启连接
     * @return bool
     */
    private function _openSocket()
    {
        $this->_m_stSocket = socket_create(AF_INET, SOCK_STREAM, SOL_TCP);
        // socket_set_option( $this->_m_stSocket,SOL_SOCKET,SO_RCVTIMEO,array("sec"=>10, "usec"=>0 ) );
        // socket_set_option( $this->_m_stSocket,SOL_SOCKET,SO_SNDTIMEO,array("sec"=>10, "usec"=>0 ) );
        if(!socket_connect($this->_m_stSocket, $this->_m_arrConfigArr['host'], $this->_m_arrConfigArr['port']))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET;
            $this->_m_sRetMsg = 'connect fail';
            return false;
        }

        return true;
    }

    /**
     * 检查身份
     * @return bool
     */
    private function _verifyIdentity()
    {
        //4b - int - 客户端对象
        $content = pack('N', $this->_m_arrConfigArr['client']);

        //私钥（取当前时间）
        $skey = (string) time();

        //2b - short - 用户名字符串长度 . bytes - 用户名字符串内容
        $user = $this->__encry($skey, $this->_m_arrConfigArr['user']);
        $userLength = strlen($user);
        $content .= pack('n', $userLength).pack('a'.$userLength, $user);

        //2b - short - 用户密码字符串长度 . bytes - 用户密码字符串内容
        $password = $this->__encry($skey, $this->_m_arrConfigArr['password']);
        $passwordLength = strlen($password);
        $content .= pack('n', $passwordLength).pack('a'.$passwordLength, $password);

        //2b - short - 密钥信息字符串长度 . bytes - 密钥信息字符串内容
        $customMsg = $this->__encry($this->_m_arrConfigArr['key'], $skey);
        $customMsgLength = strlen($customMsg);
        $content .= pack('n', $customMsgLength).pack('a'.$customMsgLength, $customMsg);

        //4b - int - 整体包长度
        $length = pack('N', strlen($content));
        $body = $length.$content;

        //发送
        if(!socket_write($this->_m_stSocket, $body))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_WRITE;
            $this->_m_sRetMsg = 'write socket error, [length:'.$length.']!';
            return false;
        }

        //读取并解析
        $read = '';
        $bodyLength = 0;
        $hasHead = false;
        while($buffer = socket_read($this->_m_stSocket, 32))
        {
            $read .= $buffer;

            //读取包头部分数据（包体长度）
            if(!$hasHead && strlen($read) > 3)
            {
                $hasHead = true;

                $result = unpack('Nlength', substr($read, 0, 4));
                if(empty($read) || empty($result['length']))
                {
                    $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body length error!';
                    return false;
                }

                $bodyLength = $result['length'];
            }

            //读取包内容数据
            if($hasHead)
            {
                $body = substr($read, 4);

                //读取包体长度
                if(strlen($body) < $bodyLength)
                    continue;

                //读取SocketID
                $result = unpack64('J', substr($body, 0, 8));
                if(empty($result))
                {
                    $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body [SocketId] error!';
                    return false;
                }

                //读取内容长度
                $result = unpack('nlength', substr($body, 8, 2));
                if(empty($result) || empty($result['length']))
                {
                    $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body [ContentLength] error!';
                    return false;
                }
                $contentLength = $result['length'];

                //读取内容
                $result = unpack('a'.$contentLength.'content', substr($body, 10));
                if(empty($result) || empty($result['content'])) {
                    $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body [content] error!';
                    return false;
                }
                $content = $result['content'];

                //检查返回内容
                if($content != 'success')
                {
                    $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body content ['.$content.'] error!';
                    return false;
                }

                return true;
            }
        }

        $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
        $this->_m_sRetMsg = 'read socket[verifyIdentity] error!';
        return false;
    }
	
    /**
     * 发送消息
     * @param $mainOrder
     * @param $subOrder
     * @param array $reqData
     * @return bool
     */
    private function _sendMsg($mainOrder, $subOrder, $packData)
    {
        //1b - byte - mainOrder & 1b - byte - subOrder
        $content = pack('C', $mainOrder).pack('C', $subOrder).$packData;
		
        //包内容
        if(isset($packData))
		{
			$content .= $packData;
		}
		
		//构成包数据
		$body = pack('N', strlen($content)).$content;

		//根据需求处理不同的返回结果
		return $this->__deal($mainOrder, $subOrder, $body);
    }
	
	/**
     * 处理消息
     * @param $mainOrder
     * @param $subOrder
     * @param array $reqData
     * @return bool
     */
	private function __deal($mainOrder, $subOrder, $body)
	{
        //发送消息
        if(!socket_write($this->_m_stSocket, $body))
        {
            $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_WRITE;
            $this->_m_sRetMsg = 'write socket msg error!';
            return false;
        }

        //读取并解析
        $read = '';
		$readBody = '';
        $bodyLength = 0;
		do
		{
			$buffer = socket_read($this->_m_stSocket, $this->_m_arrConfigArr['socket_read']);
			if('' == $buffer)
				break;
			
            $read .= $buffer;
			
			//还未达到包头长度，需要重复请求
			if(strlen($read) < 4)
				continue;

            //读取包头部分数据（包体长度）
            if($bodyLength == 0)
            {
                $result = unpack('Nlength', substr($read, 0, 4));
                if(empty($result['length']))
                {
                    $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body length error!';
                    return false;
                }
				
				//标记包头信息
                $bodyLength = $result['length'];
				if($bodyLength == 0)
				{
					$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
                    $this->_m_sRetMsg = 'read socket body length empty error!';
					return false;
				}
            }
			
			if(strlen($read) < ($bodyLength + 4))
				continue;
			
			//数据包体
			$readBody = substr($read, 4);
			
            //读取包内容数据
			//读取mainOrder
			$result = unpack('Corder', substr($readBody, 0, 128));
			if(empty($result))
			{
				$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
				$this->_m_sRetMsg = 'read socket body [mainOrder] error!';
				return false;
			}
			/* if($result['order'] != $mainOrder)
			{
				$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
				$this->_m_sRetMsg = 'read socket body [mainOrder:'.$result['order'].'] error!';
				return false;
			} */

			//读取subOrder
			$result = unpack('Corder', substr($readBody, 1, 1));
			if(empty($result))
			{
				$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
				$this->_m_sRetMsg = 'read socket body [subOrder] error!';
				return false;
			}
			/* if($result['order'] != $subOrder)
			{
				$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
				$this->_m_sRetMsg = 'read socket body [subOrder:'.$result['order'].'] error!';
				return false;
			} */

			//读取内容长度
			$result = unpack('nlength', substr($readBody, 2, 2));
			if(empty($result))
			{
				$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
				$this->_m_sRetMsg = 'read socket body [ContentLength] error!';
				return false;
			}
			$contentLength = $result['length'];

			//读取内容
			if($contentLength > 0)
			{
				$result = unpack('a' . $contentLength . 'content', substr($readBody, 4));
				if(empty($result) || empty($result['content']))
				{
					$this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
					$this->_m_sRetMsg = 'read socket body [content] error!';
					return false;
				}
				$this->_m_arrRetData = json_decode($result['content'], true);
			}

			return true;
			
		}while(true);
		
        $this->_m_iRetCode = MGHS_ERROR::ERR_SOCKET_RET;
        $this->_m_sRetMsg = 'read socket[customMsg] error!';
        return false;
	}
	
    /**
     * 加密数据
     * @param $key
     * @param $msg
     * @return string
     */
    private function __encry($key, $msg)
    {
        $key_len = strlen($key);
        $msg_len = strlen($msg);

        $ent_msg = '';
        for($i=0,$j=0; $i<$msg_len; $i++)
        {
            $ent_msg .= $msg[$i] ^ $key[$j];

            $j++;
            if($j>=$key_len)
                $j = 0;
        }

        $msg = bin2hex($ent_msg);
        return $msg;
    }
}