package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 宴会索引列表
 **/
public class GS2GC_019_006_RetGetDinnerIdxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会索引数据列表 */
private java.util.ArrayList<Common.DinnerObj.Dinner_Idx> idxLIst;
/** 是否有后续宴会数据 */
private boolean hasNext;


public GS2GC_019_006_RetGetDinnerIdxList() {
	idxLIst = new java.util.ArrayList<Common.DinnerObj.Dinner_Idx>();
	hasNext = false;
}

public GS2GC_019_006_RetGetDinnerIdxList(
	 java.util.ArrayList<Common.DinnerObj.Dinner_Idx> _idxLIst
	, boolean _hasNext
) {	idxLIst = _idxLIst;
	hasNext = _hasNext;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)6; }

/** 宴会索引数据列表 */
public java.util.ArrayList<Common.DinnerObj.Dinner_Idx> getIdxLIst() { return idxLIst; }
/** 宴会索引数据列表 */
public void addIdxLIst(Common.DinnerObj.Dinner_Idx _idxLIst) { idxLIst.add(_idxLIst); }
/** 是否有后续宴会数据 */
public boolean getHasNext() { return hasNext; }
/** 是否有后续宴会数据 */
public void setHasNext(boolean _hasNext) { hasNext = _hasNext; }


public final int GetBufSize() {
	int _size = 1;
	_size += 2 + (idxLIst.size() * 57);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (idxLIst.size() * 57);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _idxLIstCount = _buf.getShort();
	for(int _i = 0; _i < _idxLIstCount; _i++) { 
		Common.DinnerObj.Dinner_Idx _idxLIst = new Common.DinnerObj.Dinner_Idx();
		if(_buf.remaining() <= 0) return;
	int __idxLIstCustLen = _buf.getInt();
	int __idxLIstCurPos = _buf.position();
	_idxLIst.ReadUnzipBuf(_buf, __idxLIstCurPos + __idxLIstCustLen);
	_buf.position(__idxLIstCurPos + __idxLIstCustLen);

		idxLIst.add(_idxLIst);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasNext = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)idxLIst.size());
	for(int _i = 0; _i < idxLIst.size(); _i++) { 
		_buf.putInt(idxLIst.get(_i).GetBufSize());
	idxLIst.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(hasNext?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)6);
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

