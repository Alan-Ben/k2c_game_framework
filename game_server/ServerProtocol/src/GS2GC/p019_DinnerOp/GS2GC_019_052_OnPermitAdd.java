package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 宴会凭证推送
 **/
public class GS2GC_019_052_OnPermitAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会凭证 */
private Common.DinnerObj.Dinner_Permit permit;


public GS2GC_019_052_OnPermitAdd() {
	permit = new Common.DinnerObj.Dinner_Permit();
}

public GS2GC_019_052_OnPermitAdd(
	 Common.DinnerObj.Dinner_Permit _permit
) {	permit = _permit;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)52; }

/** 宴会凭证 */
public Common.DinnerObj.Dinner_Permit getPermit() { return permit; }
/** 宴会凭证 */
public void setPermit(Common.DinnerObj.Dinner_Permit _permit) { permit = _permit; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _permitCustLen = _buf.getInt();
	int _permitCurPos = _buf.position();
	permit.ReadUnzipBuf(_buf, _permitCurPos + _permitCustLen);
	_buf.position(_permitCurPos + _permitCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(permit.GetBufSize());
	permit.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)52);
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

