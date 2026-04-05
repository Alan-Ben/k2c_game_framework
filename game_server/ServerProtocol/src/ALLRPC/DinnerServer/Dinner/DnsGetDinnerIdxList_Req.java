package ALLRPC.DinnerServer.Dinner;

import java.nio.ByteBuffer;
public class DnsGetDinnerIdxList_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long groupId;
private long cid;
private int page;
private int num;


public DnsGetDinnerIdxList_Req() {
	groupId = (long)0;
	cid = (long)0;
	page = 0;
	num = 0;
}

public DnsGetDinnerIdxList_Req(
	 long _groupId
	, long _cid
	, int _page
	, int _num
) {	groupId = _groupId;
	cid = _cid;
	page = _page;
	num = _num;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGroupId() { return groupId; }
public void setGroupId(long _groupId) { groupId = _groupId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public int getPage() { return page; }
public void setPage(int _page) { page = _page; }
public int getNum() { return num; }
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) page = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(cid);
	_buf.putInt(page);
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

