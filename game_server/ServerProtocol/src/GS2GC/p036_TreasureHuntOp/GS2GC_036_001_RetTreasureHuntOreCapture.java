package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
public class GS2GC_036_001_RetTreasureHuntOreCapture implements ALBasicProtocolPack._IALProtocolStructure {
/** 获得经验值 */
private long addExp;
/** 捕捉结果列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureResult> resultList;


public GS2GC_036_001_RetTreasureHuntOreCapture() {
	addExp = (long)0;
	resultList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureResult>();
}

public GS2GC_036_001_RetTreasureHuntOreCapture(
	 long _addExp
	, java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureResult> _resultList
) {	addExp = _addExp;
	resultList = _resultList;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)1; }

/** 获得经验值 */
public long getAddExp() { return addExp; }
/** 获得经验值 */
public void setAddExp(long _addExp) { addExp = _addExp; }
/** 捕捉结果列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureResult> getResultList() { return resultList; }
/** 捕捉结果列表 */
public void addResultList(Common.TreasureHuntObj.TreasureHunt_CaptureResult _resultList) { resultList.add(_resultList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += 4 + resultList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < resultList.size(); _i++) {
	_size += 4 + resultList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _resultListCount = _buf.getShort();
	for(int _i = 0; _i < _resultListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_CaptureResult _resultList = new Common.TreasureHuntObj.TreasureHunt_CaptureResult();
		if(_buf.remaining() <= 0) return;
	int __resultListCustLen = _buf.getInt();
	int __resultListCurPos = _buf.position();
	_resultList.ReadUnzipBuf(_buf, __resultListCurPos + __resultListCustLen);
	_buf.position(__resultListCurPos + __resultListCustLen);

		resultList.add(_resultList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(addExp);
	_buf.putShort((short)resultList.size());
	for(int _i = 0; _i < resultList.size(); _i++) { 
		_buf.putInt(resultList.get(_i).GetBufSize());
	resultList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
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

