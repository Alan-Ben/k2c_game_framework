package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店客人变更
 **/
public class GS2GC_034_060_OnInnGuestChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 客人信息 */
private Common.InnObj.Inn_GuestInfo guestInfo;


public GS2GC_034_060_OnInnGuestChg() {
	guestInfo = new Common.InnObj.Inn_GuestInfo();
}

public GS2GC_034_060_OnInnGuestChg(
	 Common.InnObj.Inn_GuestInfo _guestInfo
) {	guestInfo = _guestInfo;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)60; }

/** 客人信息 */
public Common.InnObj.Inn_GuestInfo getGuestInfo() { return guestInfo; }
/** 客人信息 */
public void setGuestInfo(Common.InnObj.Inn_GuestInfo _guestInfo) { guestInfo = _guestInfo; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _guestInfoCustLen = _buf.getInt();
	int _guestInfoCurPos = _buf.position();
	guestInfo.ReadUnzipBuf(_buf, _guestInfoCurPos + _guestInfoCustLen);
	_buf.position(_guestInfoCurPos + _guestInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(guestInfo.GetBufSize());
	guestInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)60);
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

