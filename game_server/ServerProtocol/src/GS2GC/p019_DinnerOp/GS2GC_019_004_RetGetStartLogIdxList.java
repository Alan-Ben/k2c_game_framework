package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 开宴索引记录列表
 **/
public class GS2GC_019_004_RetGetStartLogIdxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 开宴索引记录列表 */
private java.util.ArrayList<Common.DinnerObj.Dinner_StartLogIdx> idxList;


public GS2GC_019_004_RetGetStartLogIdxList() {
	idxList = new java.util.ArrayList<Common.DinnerObj.Dinner_StartLogIdx>();
}

public GS2GC_019_004_RetGetStartLogIdxList(
	 java.util.ArrayList<Common.DinnerObj.Dinner_StartLogIdx> _idxList
) {	idxList = _idxList;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)4; }

/** 开宴索引记录列表 */
public java.util.ArrayList<Common.DinnerObj.Dinner_StartLogIdx> getIdxList() { return idxList; }
/** 开宴索引记录列表 */
public void addIdxList(Common.DinnerObj.Dinner_StartLogIdx _idxList) { idxList.add(_idxList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (idxList.size() * 48);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (idxList.size() * 48);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _idxListCount = _buf.getShort();
	for(int _i = 0; _i < _idxListCount; _i++) { 
		Common.DinnerObj.Dinner_StartLogIdx _idxList = new Common.DinnerObj.Dinner_StartLogIdx();
		if(_buf.remaining() <= 0) return;
	int __idxListCustLen = _buf.getInt();
	int __idxListCurPos = _buf.position();
	_idxList.ReadUnzipBuf(_buf, __idxListCurPos + __idxListCustLen);
	_buf.position(__idxListCurPos + __idxListCustLen);

		idxList.add(_idxList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)idxList.size());
	for(int _i = 0; _i < idxList.size(); _i++) { 
		_buf.putInt(idxList.get(_i).GetBufSize());
	idxList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)4);
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

