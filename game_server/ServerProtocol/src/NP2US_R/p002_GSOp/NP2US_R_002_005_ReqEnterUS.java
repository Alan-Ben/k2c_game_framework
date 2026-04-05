package NP2US_R.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_R_002_005_ReqEnterUS implements ALBasicProtocolPack._IALProtocolStructure {
private String uid;
/** gs连接对象的sessionId，用于处理结果获取对象用 */
private long gsSessionId;
/** GS对应的信息序列号 */
private long gsInfoSerialize;
private String customData;


public NP2US_R_002_005_ReqEnterUS() {
	uid = "";
	gsSessionId = (long)0;
	gsInfoSerialize = (long)0;
	customData = "";
}

public NP2US_R_002_005_ReqEnterUS(
	 String _uid
	, long _gsSessionId
	, long _gsInfoSerialize
	, String _customData
) {	uid = _uid;
	gsSessionId = _gsSessionId;
	gsInfoSerialize = _gsInfoSerialize;
	customData = _customData;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)5; }

public String getUid() { return uid; }
public void setUid(String _uid) { uid = _uid; }
/** gs连接对象的sessionId，用于处理结果获取对象用 */
public long getGsSessionId() { return gsSessionId; }
/** gs连接对象的sessionId，用于处理结果获取对象用 */
public void setGsSessionId(long _gsSessionId) { gsSessionId = _gsSessionId; }
/** GS对应的信息序列号 */
public long getGsInfoSerialize() { return gsInfoSerialize; }
/** GS对应的信息序列号 */
public void setGsInfoSerialize(long _gsInfoSerialize) { gsInfoSerialize = _gsInfoSerialize; }
public String getCustomData() { return customData; }
public void setCustomData(String _customData) { customData = _customData; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customData);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(customData);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gsSessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gsInfoSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) customData = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	_buf.putLong(gsSessionId);
	_buf.putLong(gsInfoSerialize);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, customData);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)5);
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

