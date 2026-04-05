package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 变更好友分组的顺序
 **/
public class GC2GS_021_038_ReqChgFriendGroupOrderList implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组id的顺序列表 */
private java.util.ArrayList<Long> groupIdList;


public GC2GS_021_038_ReqChgFriendGroupOrderList() {
	groupIdList = new java.util.ArrayList<Long>();
}

public GC2GS_021_038_ReqChgFriendGroupOrderList(
	 java.util.ArrayList<Long> _groupIdList
) {	groupIdList = _groupIdList;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)38; }

/** 分组id的顺序列表 */
public java.util.ArrayList<Long> getGroupIdList() { return groupIdList; }
/** 分组id的顺序列表 */
public void addGroupIdList(long _groupIdList) { groupIdList.add(_groupIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (groupIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (groupIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupIdListCount = _buf.getShort();
	for(int _i = 0; _i < _groupIdListCount; _i++) { 
		long _groupIdList = (long)0;
		if(_buf.remaining() > 0) _groupIdList = _buf.getLong();
		groupIdList.add(_groupIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)groupIdList.size());
	for(int _i = 0; _i < groupIdList.size(); _i++) { 
		_buf.putLong(groupIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)38);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)38);
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

