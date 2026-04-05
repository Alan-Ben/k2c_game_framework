package GS2GC.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 一键游历
 **/
public class GS2GC_008_002_RetAkeyTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件结果列表 */
private java.util.ArrayList<Common.TravelObj.Travel_EventResult> resultList;


public GS2GC_008_002_RetAkeyTravel() {
	resultList = new java.util.ArrayList<Common.TravelObj.Travel_EventResult>();
}

public GS2GC_008_002_RetAkeyTravel(
	 java.util.ArrayList<Common.TravelObj.Travel_EventResult> _resultList
) {	resultList = _resultList;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)2; }

/** 游历事件结果列表 */
public java.util.ArrayList<Common.TravelObj.Travel_EventResult> getResultList() { return resultList; }
/** 游历事件结果列表 */
public void addResultList(Common.TravelObj.Travel_EventResult _resultList) { resultList.add(_resultList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += 4 + resultList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += 4 + resultList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _resultListCount = _buf.getShort();
	for(int _i = 0; _i < _resultListCount; _i++) { 
		Common.TravelObj.Travel_EventResult _resultList = new Common.TravelObj.Travel_EventResult();
		if(_buf.remaining() <= 0) return;
	int __resultListCustLen = _buf.getInt();
	int __resultListCurPos = _buf.position();
	_resultList.ReadUnzipBuf(_buf, __resultListCurPos + __resultListCustLen);
	_buf.position(__resultListCurPos + __resultListCustLen);

		resultList.add(_resultList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)resultList.size());
	for(int _i = 0; _i < resultList.size(); _i++) { 
		_buf.putInt(resultList.get(_i).GetBufSize());
	resultList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
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

