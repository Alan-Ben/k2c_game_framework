package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 玩家禁言
 **/
public class NP2US_R_003_002_ReqForbidPlayerChat implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> cidList;
/** 禁言时长 0-永久 */
private int hours;
/** 禁言频道id */
private NPEnum.ENPChatRoomType roomType;


public NP2US_R_003_002_ReqForbidPlayerChat() {
	cidList = new java.util.ArrayList<Long>();
	hours = 0;
	roomType = NPEnum.ENPChatRoomType.values()[0];
}

public NP2US_R_003_002_ReqForbidPlayerChat(
	 java.util.ArrayList<Long> _cidList
	, int _hours
	, NPEnum.ENPChatRoomType _roomType
) {	cidList = _cidList;
	hours = _hours;
	roomType = _roomType;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)2; }

public java.util.ArrayList<Long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.add(_cidList); }
/** 禁言时长 0-永久 */
public int getHours() { return hours; }
/** 禁言时长 0-永久 */
public void setHours(int _hours) { hours = _hours; }
/** 禁言频道id */
public NPEnum.ENPChatRoomType getRoomType() { return roomType; }
/** 禁言频道id */
public void setRoomType(NPEnum.ENPChatRoomType _roomType) { roomType = _roomType; }


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
	if(_buf.remaining() > 0) hours = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomType = NPEnum.ENPChatRoomType.ENPChatRoomType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
	_buf.putInt(hours);
	_buf.putInt(roomType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)2);
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

