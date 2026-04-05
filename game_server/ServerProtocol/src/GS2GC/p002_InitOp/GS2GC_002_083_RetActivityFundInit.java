package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_083_RetActivityFundInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 基金列表 */
private java.util.ArrayList<Common.ActivityFundObj.ActivityFund_Info> fundList;


public GS2GC_002_083_RetActivityFundInit() {
	fundList = new java.util.ArrayList<Common.ActivityFundObj.ActivityFund_Info>();
}

public GS2GC_002_083_RetActivityFundInit(
	 java.util.ArrayList<Common.ActivityFundObj.ActivityFund_Info> _fundList
) {	fundList = _fundList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)83; }

/** 基金列表 */
public java.util.ArrayList<Common.ActivityFundObj.ActivityFund_Info> getFundList() { return fundList; }
/** 基金列表 */
public void addFundList(Common.ActivityFundObj.ActivityFund_Info _fundList) { fundList.add(_fundList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (fundList.size() * 44);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (fundList.size() * 44);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _fundListCount = _buf.getShort();
	for(int _i = 0; _i < _fundListCount; _i++) { 
		Common.ActivityFundObj.ActivityFund_Info _fundList = new Common.ActivityFundObj.ActivityFund_Info();
		if(_buf.remaining() <= 0) return;
	int __fundListCustLen = _buf.getInt();
	int __fundListCurPos = _buf.position();
	_fundList.ReadUnzipBuf(_buf, __fundListCurPos + __fundListCustLen);
	_buf.position(__fundListCurPos + __fundListCustLen);

		fundList.add(_fundList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)fundList.size());
	for(int _i = 0; _i < fundList.size(); _i++) { 
		_buf.putInt(fundList.get(_i).GetBufSize());
	fundList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)83);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)83);
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

