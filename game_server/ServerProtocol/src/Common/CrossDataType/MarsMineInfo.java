package Common.CrossDataType;

import java.nio.ByteBuffer;
public class MarsMineInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿实例Id，注意要区分服务器 */
private long mineInstanceId;
/** 静态数据Id */
private long mineRefId;
/** 矿超时时间戳 */
private long mineExpireTimeMS;


public MarsMineInfo() {
	mineInstanceId = (long)0;
	mineRefId = (long)0;
	mineExpireTimeMS = (long)0;
}

public MarsMineInfo(
	 long _mineInstanceId
	, long _mineRefId
	, long _mineExpireTimeMS
) {	mineInstanceId = _mineInstanceId;
	mineRefId = _mineRefId;
	mineExpireTimeMS = _mineExpireTimeMS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 矿实例Id，注意要区分服务器 */
public long getMineInstanceId() { return mineInstanceId; }
/** 矿实例Id，注意要区分服务器 */
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
/** 静态数据Id */
public long getMineRefId() { return mineRefId; }
/** 静态数据Id */
public void setMineRefId(long _mineRefId) { mineRefId = _mineRefId; }
/** 矿超时时间戳 */
public long getMineExpireTimeMS() { return mineExpireTimeMS; }
/** 矿超时时间戳 */
public void setMineExpireTimeMS(long _mineExpireTimeMS) { mineExpireTimeMS = _mineExpireTimeMS; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineExpireTimeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mineInstanceId);
	_buf.putLong(mineRefId);
	_buf.putLong(mineExpireTimeMS);
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

