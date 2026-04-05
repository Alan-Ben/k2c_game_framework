package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-伤害排行榜
 **/
public class GC2GS_037_007_ReqDamageRank implements ALBasicProtocolPack._IALProtocolStructure {
private int page;
/** 每页数量，不超过100 */
private int pageNum;


public GC2GS_037_007_ReqDamageRank() {
	page = 0;
	pageNum = 0;
}

public GC2GS_037_007_ReqDamageRank(
	 int _page
	, int _pageNum
) {	page = _page;
	pageNum = _pageNum;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)7; }

public int getPage() { return page; }
public void setPage(int _page) { page = _page; }
/** 每页数量，不超过100 */
public int getPageNum() { return pageNum; }
/** 每页数量，不超过100 */
public void setPageNum(int _pageNum) { pageNum = _pageNum; }


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
	if(_buf.remaining() > 0) pageNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(page);
	_buf.putInt(pageNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)7);
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

