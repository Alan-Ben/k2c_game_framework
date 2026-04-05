package NP2IS_RB.p001_ISOp;

import java.nio.ByteBuffer;
public class NP2IS_RB_001_003_RetRegChatUser implements ALBasicProtocolPack._IALProtocolStructure {
/** 聊天系统用户唯一标识 */
private String chatUid;
/** 聊天系统ID，用于区分不同大区的唯一标识，聊天用户UID的组成部分 */
private long systemId;
/** 聊天系统ID，用于区分不同大区的标签，聊天用户UID的组成部分 */
private String systemTag;
/** 聊天服IP */
private String ip;
/** 聊天服端口 */
private int port;
/** 登录密码 */
private String checkCode;
/** 用户线路标识，即登录GS服务器标签 */
private String areaTag;


public NP2IS_RB_001_003_RetRegChatUser() {
	chatUid = "";
	systemId = (long)0;
	systemTag = "";
	ip = "";
	port = 0;
	checkCode = "";
	areaTag = "";
}

public NP2IS_RB_001_003_RetRegChatUser(
	 String _chatUid
	, long _systemId
	, String _systemTag
	, String _ip
	, int _port
	, String _checkCode
	, String _areaTag
) {	chatUid = _chatUid;
	systemId = _systemId;
	systemTag = _systemTag;
	ip = _ip;
	port = _port;
	checkCode = _checkCode;
	areaTag = _areaTag;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

/** 聊天系统用户唯一标识 */
public String getChatUid() { return chatUid; }
/** 聊天系统用户唯一标识 */
public void setChatUid(String _chatUid) { chatUid = _chatUid; }
/** 聊天系统ID，用于区分不同大区的唯一标识，聊天用户UID的组成部分 */
public long getSystemId() { return systemId; }
/** 聊天系统ID，用于区分不同大区的唯一标识，聊天用户UID的组成部分 */
public void setSystemId(long _systemId) { systemId = _systemId; }
/** 聊天系统ID，用于区分不同大区的标签，聊天用户UID的组成部分 */
public String getSystemTag() { return systemTag; }
/** 聊天系统ID，用于区分不同大区的标签，聊天用户UID的组成部分 */
public void setSystemTag(String _systemTag) { systemTag = _systemTag; }
/** 聊天服IP */
public String getIp() { return ip; }
/** 聊天服IP */
public void setIp(String _ip) { ip = _ip; }
/** 聊天服端口 */
public int getPort() { return port; }
/** 聊天服端口 */
public void setPort(int _port) { port = _port; }
/** 登录密码 */
public String getCheckCode() { return checkCode; }
/** 登录密码 */
public void setCheckCode(String _checkCode) { checkCode = _checkCode; }
/** 用户线路标识，即登录GS服务器标签 */
public String getAreaTag() { return areaTag; }
/** 用户线路标识，即登录GS服务器标签 */
public void setAreaTag(String _areaTag) { areaTag = _areaTag; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(systemTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ip);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(systemTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ip);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatUid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
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
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) areaTag = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, chatUid);
	_buf.putLong(systemId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, systemTag);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, ip);
	_buf.putInt(port);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, checkCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, areaTag);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

