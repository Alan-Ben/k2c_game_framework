package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_064_RetActivityCurrencyList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.Common_ActivityCurrencyInfo> currencyList;


public GS2GC_002_064_RetActivityCurrencyList() {
	currencyList = new java.util.ArrayList<Common.Common_ActivityCurrencyInfo>();
}

public GS2GC_002_064_RetActivityCurrencyList(
	 java.util.ArrayList<Common.Common_ActivityCurrencyInfo> _currencyList
) {	currencyList = _currencyList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)64; }

public java.util.ArrayList<Common.Common_ActivityCurrencyInfo> getCurrencyList() { return currencyList; }
public void addCurrencyList(Common.Common_ActivityCurrencyInfo _currencyList) { currencyList.add(_currencyList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (currencyList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (currencyList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _currencyListCount = _buf.getShort();
	for(int _i = 0; _i < _currencyListCount; _i++) { 
		Common.Common_ActivityCurrencyInfo _currencyList = new Common.Common_ActivityCurrencyInfo();
		if(_buf.remaining() <= 0) return;
	int __currencyListCustLen = _buf.getInt();
	int __currencyListCurPos = _buf.position();
	_currencyList.ReadUnzipBuf(_buf, __currencyListCurPos + __currencyListCustLen);
	_buf.position(__currencyListCurPos + __currencyListCustLen);

		currencyList.add(_currencyList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)currencyList.size());
	for(int _i = 0; _i < currencyList.size(); _i++) { 
		_buf.putInt(currencyList.get(_i).GetBufSize());
	currencyList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)64);
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

