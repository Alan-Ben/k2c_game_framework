package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_074_OnGachaPoolChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 卡池信息 */
private Common.GachaObj.Gacha_PoolInfo poolInfo;


public GS2GC_007_074_OnGachaPoolChg() {
	poolInfo = new Common.GachaObj.Gacha_PoolInfo();
}

public GS2GC_007_074_OnGachaPoolChg(
	 Common.GachaObj.Gacha_PoolInfo _poolInfo
) {	poolInfo = _poolInfo;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)74; }

/** 卡池信息 */
public Common.GachaObj.Gacha_PoolInfo getPoolInfo() { return poolInfo; }
/** 卡池信息 */
public void setPoolInfo(Common.GachaObj.Gacha_PoolInfo _poolInfo) { poolInfo = _poolInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + poolInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + poolInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _poolInfoCustLen = _buf.getInt();
	int _poolInfoCurPos = _buf.position();
	poolInfo.ReadUnzipBuf(_buf, _poolInfoCurPos + _poolInfoCustLen);
	_buf.position(_poolInfoCurPos + _poolInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(poolInfo.GetBufSize());
	poolInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)74);
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

