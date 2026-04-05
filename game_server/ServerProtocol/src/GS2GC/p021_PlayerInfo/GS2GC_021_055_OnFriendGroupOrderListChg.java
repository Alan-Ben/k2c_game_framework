package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 好友分组顺序变更
 **/
public class GS2GC_021_055_OnFriendGroupOrderListChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 好友分组顺序 */
private java.util.ArrayList<Long> friendGroupOrderList;


public GS2GC_021_055_OnFriendGroupOrderListChg() {
	friendGroupOrderList = new java.util.ArrayList<Long>();
}

public GS2GC_021_055_OnFriendGroupOrderListChg(
	 java.util.ArrayList<Long> _friendGroupOrderList
) {	friendGroupOrderList = _friendGroupOrderList;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)55; }

/** 好友分组顺序 */
public java.util.ArrayList<Long> getFriendGroupOrderList() { return friendGroupOrderList; }
/** 好友分组顺序 */
public void addFriendGroupOrderList(long _friendGroupOrderList) { friendGroupOrderList.add(_friendGroupOrderList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (friendGroupOrderList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (friendGroupOrderList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _friendGroupOrderListCount = _buf.getShort();
	for(int _i = 0; _i < _friendGroupOrderListCount; _i++) { 
		long _friendGroupOrderList = (long)0;
		if(_buf.remaining() > 0) _friendGroupOrderList = _buf.getLong();
		friendGroupOrderList.add(_friendGroupOrderList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)friendGroupOrderList.size());
	for(int _i = 0; _i < friendGroupOrderList.size(); _i++) { 
		_buf.putLong(friendGroupOrderList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)55);
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

