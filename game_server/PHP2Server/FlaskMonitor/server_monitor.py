# monitor_server.py

# 标准库
import binascii
import json
import socket
import struct
import threading
import time
from time import time
from functools import wraps

# 第三方库
from flask import Flask, render_template, request
from server_configs import SERVER_CONFIGS  # 独立的服务器配置

class MonitorClientPool:
    def __init__(self):
        self.clients = {}
        self.lock = threading.Lock()

    def get_client(self, server_id):
        with self.lock:
            if server_id not in self.clients:
                config = SERVER_CONFIGS[server_id].copy()
                self.clients[server_id] = MonitorClient(server_id,config)
            return self.clients[server_id]

# 全局客户端池
client_pool = MonitorClientPool()

# 添加缓存管理类
class CacheManager:
    def __init__(self):
        self.cache = {}
        self.lock = threading.Lock()
    
    def cache_with_timeout(self, timeout=5):
        def decorator(f):
            @wraps(f)
            def wrapper(*args, **kwargs):
                # 获取 MonitorClient 实例的 serverId
                instance = args[0]
                server_id = instance.serverId
                
                # 从参数中提取 t 和 tid
                t = kwargs.get('t') if 't' in kwargs else args[1] if len(args) > 1 else None
                tid = kwargs.get('tid') if 'tid' in kwargs else args[2] if len(args) > 2 else None
                
                # 将 serverId 加入缓存键
                cache_key = f"{server_id}:{t}:{tid}"
                
                with self.lock:
                    if cache_key in self.cache:
                        result, timestamp = self.cache[cache_key]
                        if time() - timestamp < timeout:
                            return result
                    
                    result = f(*args, **kwargs)
                    self.cache[cache_key] = (result, time())
                    return result
            return wrapper
        return decorator

# 初始化缓存管理器
cache_manager = CacheManager()

class MonitorClient:
    def __init__(self,server_id, config):
        self.serverId = server_id
        self.config = config
        self.socket = None
        self.cache_manager = cache_manager

    @cache_manager.cache_with_timeout(timeout=10)  # 10秒缓存
    def get_server_status(self):
        try:
            # 直接发送消息，不预先检查连接
            result = self._send_message(0, 1, None)
            if result:
                return result
            return {"error": "获取数据失败"}
        except Exception as e:
            return {"error": str(e)} 

    @cache_manager.cache_with_timeout(timeout=5)  # 5秒缓存
    def get_detail(self, t, tid):
        try:
            # 直接发送消息，不预先检查连接
            data = struct.pack('>II', int(t), int(tid))
            
            result = self._send_message(0, 2, data)
            if result:
                return result
            return {"error": "获取数据失败"}
            
        except Exception as e:
            return {"error": str(e)}

    def _connect(self):
        try:
            print(self.serverId, "连接中...")
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.settimeout(5)  # 设置超时时间为 5 秒
            self.socket.connect((self.config['host'], self.config['port']))
            if self._verify_identity():
                print(self.serverId, "连接成功")
                return True
            else:
                print(self.serverId, "身份验证失败")
                self._close()
                return False
        except socket.timeout:
            print(f"{self.serverId} 连接超时")
            self._close()
            return False
        except Exception as e:
            print(f"{self.serverId} 连接失败: {e}")
            self._close()
            return False

    def _verify_identity(self):
        try:
            content = b''
            content += struct.pack('>I', self.config['client'])
            
            skey = str(int(time())).encode()
            
            username = self._encrypt(skey, self.config['user'])
            content += struct.pack('>H', len(username))
            content += username
            
            password = self._encrypt(skey, self.config['password'])
            content += struct.pack('>H', len(password))
            content += password
            
            custom_msg = self._encrypt(self.config['key'].encode(), skey)
            content += struct.pack('>H', len(custom_msg))
            content += custom_msg
            
            packet = struct.pack('>I', len(content)) + content
            self.socket.send(packet)
            return self._read_packet() is not None
        except:
            return False

    def _encrypt(self, key, msg):
        if isinstance(msg, str):
            msg = msg.encode()
            
        key_len = len(key)
        msg_len = len(msg)
        
        result = bytearray()
        for i in range(msg_len):
            result.append(msg[i] ^ key[i % key_len])
            
        return binascii.hexlify(result)

    def _send_message(self, main_order, sub_order, data=None):
        # 尝试发送消息，如果失败则重新连接再试一次
        try:
            # 如果没有连接，直接建立连接
            if not self.socket and not self._connect():
                return None

            content = struct.pack('BB', main_order, sub_order)
            if data:
                content += data
                
            packet = struct.pack('>I', len(content)) + content
            
            # 尝试发送消息
            try:
                self.socket.send(packet)
            except Exception as e:
                print(f"{self.serverId} 发送失败，尝试重新连接: {e}")
                self._close()
                # 重新连接后再次尝试
                if not self._connect():
                    return None
                
                # 重新发送消息
                self.socket.send(packet)
            
            # 接收并处理响应
            response = self._read_packet()
            if not response:
                # 如果接收失败，尝试重新连接
                print(f"{self.serverId} 接收响应失败，尝试重新连接")
                self._close()
                if self._connect():
                    # 重连成功后再次发送
                    return self._send_message(main_order, sub_order, data)
                return None
                
            main = response[0]
            sub = response[1]
            msg_len = struct.unpack('>H', response[2:4])[0]
            
            if msg_len > 0:
                msg_content = response[4:4+msg_len]
                return json.loads(msg_content)
            return None
            
        except Exception as e:
            print(f"{self.serverId} 发送消息时发生错误: {e}")
            return None

    def _read_packet(self):
        try:
            length_bytes = self.socket.recv(4)
            if not length_bytes:
                return None
                
            body_length = struct.unpack('>I', length_bytes)[0]
            
            body = b''
            while len(body) < body_length:
                chunk = self.socket.recv(min(1024, body_length - len(body)))
                if not chunk:
                    break
                body += chunk
                
            return body
            
        except:
            return None

    def _close(self):
        if self.socket:
            try:
                self.socket.close()
            except:
                pass
            self.socket = None

# Flask应用
app = Flask(__name__)

# SERVER_CONFIGS 已移动到 server_configs.py

@app.route('/')
def index():
    return render_template('index.html', status={})

@app.route('/api/status')
def get_status():
    server_id = request.args.get('server')
    if not server_id:
        return {'list': []}
    
    client = client_pool.get_client(server_id)
    return client.get_server_status()

@app.route('/api/detail')
def get_detail():
    server_id = request.args.get('server')
    client = client_pool.get_client(server_id)
    t = request.args.get('t')
    tid = request.args.get('tid')
    result = client.get_detail(t, tid)
    
    # 解析 info 字段中的 JSON 字符串
    info_str = result.get('info', '{}')
    try:
        info = json.loads(info_str)
        
        # 提取 refVersion 信息
        ref_version = info.get('__extMsg__', {}).get('__ref__', {}).get('refVersion')
        if ref_version:
            info['refVersion'] = ref_version
        
        # 更新 result 中的 info 字段
        result['info'] = json.dumps(info)
    except json.JSONDecodeError:
        # 如果解析失败，返回原始的 result
        pass
    
    return json.dumps(result, indent=2, ensure_ascii=False)

@app.route('/api/servers')
def get_servers():
    # 只返回服务器标识符和名称
    return {
        server_id: {'name': conf['name']} 
        for server_id, conf in SERVER_CONFIGS.items()
    }

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)