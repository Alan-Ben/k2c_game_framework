package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_017_002_RetActivityRankSettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 结算信息 */
private Common.ActivityObj.Activity_RankSettleInfo settleInfo;


public GS2GC_017_002_RetActivityRankSettleInfo() {
	settleInfo = new Common.ActivityObj.Activity_RankSettleInfo();
}

public GS2GC_017_002_RetActivityRankSettleInfo(
	 Common.ActivityObj.Activity_RankSettleInfo _settleInfo
) {	settleInfo = _settleInfo;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)2; }

/** 结算信息 */
public Common.ActivityObj.Activity_RankSettleInfo getSettleInfo() { return settleInfo; }
/** 结算信息 */
public void setSettleInfo(Common.ActivityObj.Activity_RankSettleInfo _settleInfo) { settleInfo = _settleInfo; }


public final int GetBufSize() {
	int _size = 29;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _settleInfoCustLen = _buf.getInt();
	int _settleInfoCurPos = _buf.position();
	settleInfo.ReadUnzipBuf(_buf, _settleInfoCurPos + _settleInfoCustLen);
	_buf.position(_settleInfoCurPos + _settleInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(settleInfo.GetBufSize());
	settleInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)2);
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

