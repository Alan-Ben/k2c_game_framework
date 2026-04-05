package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟宝箱数据
 **/
public class Guild_BoxInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 宝箱ID */
private long boxId;
/** 分享玩家CID */
private long shareCid;
/** 截至时间（毫秒） */
private long endMs;


public Guild_BoxInfo() {
	id = (long)0;
	boxId = (long)0;
	shareCid = (long)0;
	endMs = (long)0;
}

public Guild_BoxInfo(
	 long _id
	, long _boxId
	, long _shareCid
	, long _endMs
) {	id = _id;
	boxId = _boxId;
	shareCid = _shareCid;
	endMs = _endMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 宝箱ID */
public long getBoxId() { return boxId; }
/** 宝箱ID */
public void setBoxId(long _boxId) { boxId = _boxId; }
/** 分享玩家CID */
public long getShareCid() { return shareCid; }
/** 分享玩家CID */
public void setShareCid(long _shareCid) { shareCid = _shareCid; }
/** 截至时间（毫秒） */
public long getEndMs() { return endMs; }
/** 截至时间（毫秒） */
public void setEndMs(long _endMs) { endMs = _endMs; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shareCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(boxId);
	_buf.putLong(shareCid);
	_buf.putLong(endMs);
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

