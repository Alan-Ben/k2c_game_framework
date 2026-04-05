package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSGetGroupTeamList_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组实例ID */
private long groupId;
/** 请求页码，从1开始 */
private int page;


public CTSGetGroupTeamList_Req() {
	groupId = (long)0;
	page = 0;
}

public CTSGetGroupTeamList_Req(
	 long _groupId
	, int _page
) {	groupId = _groupId;
	page = _page;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组实例ID */
public long getGroupId() { return groupId; }
/** 分组实例ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 请求页码，从1开始 */
public int getPage() { return page; }
/** 请求页码，从1开始 */
public void setPage(int _page) { page = _page; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) page = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putInt(page);
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

