package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店特殊客人变更
 **/
public class GS2GC_034_061_OnInnSpecialGuestChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 特殊客人信息 */
private Common.InnObj.Inn_SpecialGuestInfo specialGuestInfo;


public GS2GC_034_061_OnInnSpecialGuestChg() {
	specialGuestInfo = new Common.InnObj.Inn_SpecialGuestInfo();
}

public GS2GC_034_061_OnInnSpecialGuestChg(
	 Common.InnObj.Inn_SpecialGuestInfo _specialGuestInfo
) {	specialGuestInfo = _specialGuestInfo;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)61; }

/** 特殊客人信息 */
public Common.InnObj.Inn_SpecialGuestInfo getSpecialGuestInfo() { return specialGuestInfo; }
/** 特殊客人信息 */
public void setSpecialGuestInfo(Common.InnObj.Inn_SpecialGuestInfo _specialGuestInfo) { specialGuestInfo = _specialGuestInfo; }


public final int GetBufSize() {
	int _size = 14;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _specialGuestInfoCustLen = _buf.getInt();
	int _specialGuestInfoCurPos = _buf.position();
	specialGuestInfo.ReadUnzipBuf(_buf, _specialGuestInfoCurPos + _specialGuestInfoCustLen);
	_buf.position(_specialGuestInfoCurPos + _specialGuestInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(specialGuestInfo.GetBufSize());
	specialGuestInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)61);
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

