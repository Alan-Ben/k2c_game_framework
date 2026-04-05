package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 杰出者数据记录
 **/
public class GC2GS_004_042_ReqGraveRecordList implements ALBasicProtocolPack._IALProtocolStructure {
private int typeId;
/** 当前页数 */
private int curPage;
/** 每页数量，不超过50条 */
private int pageCount;


public GC2GS_004_042_ReqGraveRecordList() {
	typeId = 0;
	curPage = 0;
	pageCount = 0;
}

public GC2GS_004_042_ReqGraveRecordList(
	 int _typeId
	, int _curPage
	, int _pageCount
) {	typeId = _typeId;
	curPage = _curPage;
	pageCount = _pageCount;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)42; }

public int getTypeId() { return typeId; }
public void setTypeId(int _typeId) { typeId = _typeId; }
/** 当前页数 */
public int getCurPage() { return curPage; }
/** 当前页数 */
public void setCurPage(int _curPage) { curPage = _curPage; }
/** 每页数量，不超过50条 */
public int getPageCount() { return pageCount; }
/** 每页数量，不超过50条 */
public void setPageCount(int _pageCount) { pageCount = _pageCount; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) typeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curPage = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pageCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(typeId);
	_buf.putInt(curPage);
	_buf.putInt(pageCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)42);
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

