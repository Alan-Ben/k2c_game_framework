package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_059_PushActivityCurrencyInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_ActivityCurrencyInfo currencyInfo;


public GS2GC_004_059_PushActivityCurrencyInfo() {
	currencyInfo = new Common.Common_ActivityCurrencyInfo();
}

public GS2GC_004_059_PushActivityCurrencyInfo(
	 Common.Common_ActivityCurrencyInfo _currencyInfo
) {	currencyInfo = _currencyInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)59; }

public Common.Common_ActivityCurrencyInfo getCurrencyInfo() { return currencyInfo; }
public void setCurrencyInfo(Common.Common_ActivityCurrencyInfo _currencyInfo) { currencyInfo = _currencyInfo; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _currencyInfoCustLen = _buf.getInt();
	int _currencyInfoCurPos = _buf.position();
	currencyInfo.ReadUnzipBuf(_buf, _currencyInfoCurPos + _currencyInfoCustLen);
	_buf.position(_currencyInfoCurPos + _currencyInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(currencyInfo.GetBufSize());
	currencyInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)59);
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

