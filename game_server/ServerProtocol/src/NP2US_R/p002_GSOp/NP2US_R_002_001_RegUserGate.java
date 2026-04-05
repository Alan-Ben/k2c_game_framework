package NP2US_R.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_R_002_001_RegUserGate implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long sessionId;
/** 消息处理序列号 */
private int dealerSerialize;
private String clientIp;
private String customData;
/** GS服务器的areaTag，用于聊天服务器选服 */
private String areaTag;


public NP2US_R_002_001_RegUserGate() {
	cid = (long)0;
	sessionId = (long)0;
	dealerSerialize = 0;
	clientIp = "";
	customData = "";
	areaTag = "";
}

public NP2US_R_002_001_RegUserGate(
	 long _cid
	, long _sessionId
	, int _dealerSerialize
	, String _clientIp
	, String _customData
	, String _areaTag
) {	cid = _cid;
	sessionId = _sessionId;
	dealerSerialize = _dealerSerialize;
	clientIp = _clientIp;
	customData = _customData;
	areaTag = _areaTag;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getSessionId() { return sessionId; }
public void setSessionId(long _sessionId) { sessionId = _sessionId; }
/** 消息处理序列号 */
public int getDealerSerialize() { return dealerSerialize; }
/** 消息处理序列号 */
public void setDealerSerialize(int _dealerSerialize) { dealerSerialize = _dealerSerialize; }
public String getClientIp() { return clientIp; }
public void setClientIp(String _clientIp) { clientIp = _clientIp; }
public String getCustomData() { return customData; }
public void setCustomData(String _customData) { customData = _customData; }
/** GS服务器的areaTag，用于聊天服务器选服 */
public String getAreaTag() { return areaTag; }
/** GS服务器的areaTag，用于聊天服务器选服 */
public void setAreaTag(String _areaTag) { areaTag = _areaTag; }


public final int GetBufSize() {
	int _size = 20;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientIp);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customData);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientIp);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customData);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealerSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientIp = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) customData = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) areaTag = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(sessionId);
	_buf.putInt(dealerSerialize);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, clientIp);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, customData);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, areaTag);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

