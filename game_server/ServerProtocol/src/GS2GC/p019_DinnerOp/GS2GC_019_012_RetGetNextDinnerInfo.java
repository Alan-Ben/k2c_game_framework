package GS2GC.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 取指定宴会的下一条宴会数据
 **/
public class GS2GC_019_012_RetGetNextDinnerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会详情 */
private Common.DinnerObj.Dinner_Info info;
/** 当前排序 */
private int idx;
/** 是否有上一条宴会数据 true-有 */
private boolean hasPre;
/** 是否有下一条宴会数据 true-有 */
private boolean hasNext;


public GS2GC_019_012_RetGetNextDinnerInfo() {
	info = new Common.DinnerObj.Dinner_Info();
	idx = 0;
	hasPre = false;
	hasNext = false;
}

public GS2GC_019_012_RetGetNextDinnerInfo(
	 Common.DinnerObj.Dinner_Info _info
	, int _idx
	, boolean _hasPre
	, boolean _hasNext
) {	info = _info;
	idx = _idx;
	hasPre = _hasPre;
	hasNext = _hasNext;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)12; }

/** 宴会详情 */
public Common.DinnerObj.Dinner_Info getInfo() { return info; }
/** 宴会详情 */
public void setInfo(Common.DinnerObj.Dinner_Info _info) { info = _info; }
/** 当前排序 */
public int getIdx() { return idx; }
/** 当前排序 */
public void setIdx(int _idx) { idx = _idx; }
/** 是否有上一条宴会数据 true-有 */
public boolean getHasPre() { return hasPre; }
/** 是否有上一条宴会数据 true-有 */
public void setHasPre(boolean _hasPre) { hasPre = _hasPre; }
/** 是否有下一条宴会数据 true-有 */
public boolean getHasNext() { return hasNext; }
/** 是否有下一条宴会数据 true-有 */
public void setHasNext(boolean _hasNext) { hasNext = _hasNext; }


public final int GetBufSize() {
	int _size = 6;
	_size += 4 + info.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 8;
	_size += 4 + info.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) idx = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasPre = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasNext = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putInt(idx);
	_buf.put(hasPre?(byte)1:(byte)0);
	_buf.put(hasNext?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)12);
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

