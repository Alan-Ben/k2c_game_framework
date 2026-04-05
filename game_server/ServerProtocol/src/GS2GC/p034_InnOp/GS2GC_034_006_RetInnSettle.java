package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
public class GS2GC_034_006_RetInnSettle implements ALBasicProtocolPack._IALProtocolStructure {
/** 结算信息 */
private Common.InnObj.Inn_SettleInfo settleInfo;


public GS2GC_034_006_RetInnSettle() {
	settleInfo = new Common.InnObj.Inn_SettleInfo();
}

public GS2GC_034_006_RetInnSettle(
	 Common.InnObj.Inn_SettleInfo _settleInfo
) {	settleInfo = _settleInfo;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)6; }

/** 结算信息 */
public Common.InnObj.Inn_SettleInfo getSettleInfo() { return settleInfo; }
/** 结算信息 */
public void setSettleInfo(Common.InnObj.Inn_SettleInfo _settleInfo) { settleInfo = _settleInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + settleInfo.GetBufSize();

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
	_buf.put((byte)34);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)6);
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

