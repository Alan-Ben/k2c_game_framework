package GS2GC.p022_ChatOp;

import java.nio.ByteBuffer;
public class GS2GC_022_001_RetPlayerChatLogin implements ALBasicProtocolPack._IALProtocolStructure {
/** 聊天系统ID */
private long systemId;
/** 聊天系统标识 */
private String systemTag;
/** 聊天服ip */
private String ip;
/** 聊天服端口 */
private int port;
/** 登录密码 */
private String checkCode;


public GS2GC_022_001_RetPlayerChatLogin() {
	systemId = (long)0;
	systemTag = "";
	ip = "";
	port = 0;
	checkCode = "";
}

public GS2GC_022_001_RetPlayerChatLogin(
	 long _systemId
	, String _systemTag
	, String _ip
	, int _port
	, String _checkCode
) {	systemId = _systemId;
	systemTag = _systemTag;
	ip = _ip;
	port = _port;
	checkCode = _checkCode;
}

public final byte getMainOrder() { return (byte)22; }

public final byte getSubOrder() { return (byte)1; }

/** 聊天系统ID */
public long getSystemId() { return systemId; }
/** 聊天系统ID */
public void setSystemId(long _systemId) { systemId = _systemId; }
/** 聊天系统标识 */
public String getSystemTag() { return systemTag; }
/** 聊天系统标识 */
public void setSystemTag(String _systemTag) { systemTag = _systemTag; }
/** 聊天服ip */
public String getIp() { return ip; }
/** 聊天服ip */
public void setIp(String _ip) { ip = _ip; }
/** 聊天服端口 */
public int getPort() { return port; }
/** 聊天服端口 */
public void setPort(int _port) { port = _port; }
/** 登录密码 */
public String getCheckCode() { return checkCode; }
/** 登录密码 */
public void setCheckCode(String _checkCode) { checkCode = _checkCode; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(systemTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ip);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(systemTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ip);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) systemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) systemTag = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ip = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) port = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) checkCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(systemId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, systemTag);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, ip);
	_buf.putInt(port);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, checkCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)1);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

