package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 金币初始化
 **/
public class GS2GC_002_011_RetGoldInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 金币信息 */
private Common.PlayerObj.Player_GoldInfo goldInfo;
/** 离线产出数量 */
private long offlineProduceCount;
/** 离线产出时间 */
private long offlineProduceTime;


public GS2GC_002_011_RetGoldInit() {
	goldInfo = new Common.PlayerObj.Player_GoldInfo();
	offlineProduceCount = (long)0;
	offlineProduceTime = (long)0;
}

public GS2GC_002_011_RetGoldInit(
	 Common.PlayerObj.Player_GoldInfo _goldInfo
	, long _offlineProduceCount
	, long _offlineProduceTime
) {	goldInfo = _goldInfo;
	offlineProduceCount = _offlineProduceCount;
	offlineProduceTime = _offlineProduceTime;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)11; }

/** 金币信息 */
public Common.PlayerObj.Player_GoldInfo getGoldInfo() { return goldInfo; }
/** 金币信息 */
public void setGoldInfo(Common.PlayerObj.Player_GoldInfo _goldInfo) { goldInfo = _goldInfo; }
/** 离线产出数量 */
public long getOfflineProduceCount() { return offlineProduceCount; }
/** 离线产出数量 */
public void setOfflineProduceCount(long _offlineProduceCount) { offlineProduceCount = _offlineProduceCount; }
/** 离线产出时间 */
public long getOfflineProduceTime() { return offlineProduceTime; }
/** 离线产出时间 */
public void setOfflineProduceTime(long _offlineProduceTime) { offlineProduceTime = _offlineProduceTime; }


public final int GetBufSize() {
	int _size = 52;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _goldInfoCustLen = _buf.getInt();
	int _goldInfoCurPos = _buf.position();
	goldInfo.ReadUnzipBuf(_buf, _goldInfoCurPos + _goldInfoCustLen);
	_buf.position(_goldInfoCurPos + _goldInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) offlineProduceCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) offlineProduceTime = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(goldInfo.GetBufSize());
	goldInfo.PutUnzipBuf(_buf);
	_buf.putLong(offlineProduceCount);
	_buf.putLong(offlineProduceTime);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)11);
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

