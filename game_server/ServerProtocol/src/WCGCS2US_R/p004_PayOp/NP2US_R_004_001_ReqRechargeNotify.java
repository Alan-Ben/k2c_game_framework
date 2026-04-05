package WCGCS2US_R.p004_PayOp;

import java.nio.ByteBuffer;
/*********
 * 充值通知
 **/
public class NP2US_R_004_001_ReqRechargeNotify implements ALBasicProtocolPack._IALProtocolStructure {
private Common.ServerObj.ServerObj_PayCallbackInfo payCallbackInfo;


public NP2US_R_004_001_ReqRechargeNotify() {
	payCallbackInfo = new Common.ServerObj.ServerObj_PayCallbackInfo();
}

public NP2US_R_004_001_ReqRechargeNotify(
	 Common.ServerObj.ServerObj_PayCallbackInfo _payCallbackInfo
) {	payCallbackInfo = _payCallbackInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)1; }

public Common.ServerObj.ServerObj_PayCallbackInfo getPayCallbackInfo() { return payCallbackInfo; }
public void setPayCallbackInfo(Common.ServerObj.ServerObj_PayCallbackInfo _payCallbackInfo) { payCallbackInfo = _payCallbackInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + payCallbackInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + payCallbackInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _payCallbackInfoCustLen = _buf.getInt();
	int _payCallbackInfoCurPos = _buf.position();
	payCallbackInfo.ReadUnzipBuf(_buf, _payCallbackInfoCurPos + _payCallbackInfoCustLen);
	_buf.position(_payCallbackInfoCurPos + _payCallbackInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(payCallbackInfo.GetBufSize());
	payCallbackInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)1);
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

