package ToPay_RB.p001_PayOp;

import java.nio.ByteBuffer;
public class ToPay_RB_001_001_GetPayCallbackList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ServerObj.ServerObj_PayCallbackInfo> payCallbackList;


public ToPay_RB_001_001_GetPayCallbackList() {
	payCallbackList = new java.util.ArrayList<Common.ServerObj.ServerObj_PayCallbackInfo>();
}

public ToPay_RB_001_001_GetPayCallbackList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_PayCallbackInfo> _payCallbackList
) {	payCallbackList = _payCallbackList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public java.util.ArrayList<Common.ServerObj.ServerObj_PayCallbackInfo> getPayCallbackList() { return payCallbackList; }
public void addPayCallbackList(Common.ServerObj.ServerObj_PayCallbackInfo _payCallbackList) { payCallbackList.add(_payCallbackList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < payCallbackList.size(); _i++) {
	_size += 4 + payCallbackList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < payCallbackList.size(); _i++) {
	_size += 4 + payCallbackList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _payCallbackListCount = _buf.getShort();
	for(int _i = 0; _i < _payCallbackListCount; _i++) { 
		Common.ServerObj.ServerObj_PayCallbackInfo _payCallbackList = new Common.ServerObj.ServerObj_PayCallbackInfo();
		if(_buf.remaining() <= 0) return;
	int __payCallbackListCustLen = _buf.getInt();
	int __payCallbackListCurPos = _buf.position();
	_payCallbackList.ReadUnzipBuf(_buf, __payCallbackListCurPos + __payCallbackListCustLen);
	_buf.position(__payCallbackListCurPos + __payCallbackListCustLen);

		payCallbackList.add(_payCallbackList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)payCallbackList.size());
	for(int _i = 0; _i < payCallbackList.size(); _i++) { 
		_buf.putInt(payCallbackList.get(_i).GetBufSize());
	payCallbackList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

