package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 变更归属的好友分组
 **/
public class GC2GS_021_036_ReqChgBelongFriendGroup implements ALBasicProtocolPack._IALProtocolStructure {
/** 好友列表 */
private java.util.ArrayList<Long> cidList;
/** 目标分组数据id */
private long groupDbId;


public GC2GS_021_036_ReqChgBelongFriendGroup() {
	cidList = new java.util.ArrayList<Long>();
	groupDbId = (long)0;
}

public GC2GS_021_036_ReqChgBelongFriendGroup(
	 java.util.ArrayList<Long> _cidList
	, long _groupDbId
) {	cidList = _cidList;
	groupDbId = _groupDbId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)36; }

/** 好友列表 */
public java.util.ArrayList<Long> getCidList() { return cidList; }
/** 好友列表 */
public void addCidList(long _cidList) { cidList.add(_cidList); }
/** 目标分组数据id */
public long getGroupDbId() { return groupDbId; }
/** 目标分组数据id */
public void setGroupDbId(long _groupDbId) { groupDbId = _groupDbId; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (cidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (cidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		if(_buf.remaining() > 0) _cidList = _buf.getLong();
		cidList.add(_cidList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupDbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
	_buf.putLong(groupDbId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)36);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)36);
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

