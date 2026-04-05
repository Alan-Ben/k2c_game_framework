package NP2HS_RB.p001_HSOp;

import java.nio.ByteBuffer;
public class NP2HS_RB_001_005_RetPlatformInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 平台提供的平台id */
private int platformId;
/** 平台提供的大区id */
private int platAreaId;


public NP2HS_RB_001_005_RetPlatformInfo() {
	platformId = 0;
	platAreaId = 0;
}

public NP2HS_RB_001_005_RetPlatformInfo(
	 int _platformId
	, int _platAreaId
) {	platformId = _platformId;
	platAreaId = _platAreaId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)5; }

/** 平台提供的平台id */
public int getPlatformId() { return platformId; }
/** 平台提供的平台id */
public void setPlatformId(int _platformId) { platformId = _platformId; }
/** 平台提供的大区id */
public int getPlatAreaId() { return platAreaId; }
/** 平台提供的大区id */
public void setPlatAreaId(int _platAreaId) { platAreaId = _platAreaId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) platformId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) platAreaId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(platformId);
	_buf.putInt(platAreaId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

