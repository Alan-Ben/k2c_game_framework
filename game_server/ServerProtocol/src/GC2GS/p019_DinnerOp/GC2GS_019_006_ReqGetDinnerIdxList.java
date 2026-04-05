package GC2GS.p019_DinnerOp;

import java.nio.ByteBuffer;
/*********
 * 宴会索引列表
 **/
public class GC2GS_019_006_ReqGetDinnerIdxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前页数 */
private int page;
/** 展示数量，上限不超过100 */
private int num;


public GC2GS_019_006_ReqGetDinnerIdxList() {
	page = 0;
	num = 0;
}

public GC2GS_019_006_ReqGetDinnerIdxList(
	 int _page
	, int _num
) {	page = _page;
	num = _num;
}

public final byte getMainOrder() { return (byte)19; }

public final byte getSubOrder() { return (byte)6; }

/** 当前页数 */
public int getPage() { return page; }
/** 当前页数 */
public void setPage(int _page) { page = _page; }
/** 展示数量，上限不超过100 */
public int getNum() { return num; }
/** 展示数量，上限不超过100 */
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) page = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(page);
	_buf.putInt(num);
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

