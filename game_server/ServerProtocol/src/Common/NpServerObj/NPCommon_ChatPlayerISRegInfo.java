package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 玩家注册到Interface上携带的身份信息
 **/
public class NPCommon_ChatPlayerISRegInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家id */
private long cid;
/** 所属us服务器id */
private int usTypeId;
/** 所属联盟id */
private long allianceId;


public NPCommon_ChatPlayerISRegInfo() {
	cid = (long)0;
	usTypeId = 0;
	allianceId = (long)0;
}

public NPCommon_ChatPlayerISRegInfo(
	 long _cid
	, int _usTypeId
	, long _allianceId
) {	cid = _cid;
	usTypeId = _usTypeId;
	allianceId = _allianceId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家id */
public long getCid() { return cid; }
/** 玩家id */
public void setCid(long _cid) { cid = _cid; }
/** 所属us服务器id */
public int getUsTypeId() { return usTypeId; }
/** 所属us服务器id */
public void setUsTypeId(int _usTypeId) { usTypeId = _usTypeId; }
/** 所属联盟id */
public long getAllianceId() { return allianceId; }
/** 所属联盟id */
public void setAllianceId(long _allianceId) { allianceId = _allianceId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) allianceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(usTypeId);
	_buf.putLong(allianceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

