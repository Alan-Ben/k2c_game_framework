package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_087_RetPlayerRoomSkin implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家拥有的房间皮肤信息队列 */
private java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin> roomSkinInfoList;


public GS2GC_002_087_RetPlayerRoomSkin() {
	roomSkinInfoList = new java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin>();
}

public GS2GC_002_087_RetPlayerRoomSkin(
	 java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin> _roomSkinInfoList
) {	roomSkinInfoList = _roomSkinInfoList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)87; }

/** 玩家拥有的房间皮肤信息队列 */
public java.util.ArrayList<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin> getRoomSkinInfoList() { return roomSkinInfoList; }
/** 玩家拥有的房间皮肤信息队列 */
public void addRoomSkinInfoList(Common.NpPlayerInfoObj.PlayerInfo_RoomSkin _roomSkinInfoList) { roomSkinInfoList.add(_roomSkinInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (roomSkinInfoList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (roomSkinInfoList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _roomSkinInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _roomSkinInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_RoomSkin _roomSkinInfoList = new Common.NpPlayerInfoObj.PlayerInfo_RoomSkin();
		if(_buf.remaining() <= 0) return;
	int __roomSkinInfoListCustLen = _buf.getInt();
	int __roomSkinInfoListCurPos = _buf.position();
	_roomSkinInfoList.ReadUnzipBuf(_buf, __roomSkinInfoListCurPos + __roomSkinInfoListCustLen);
	_buf.position(__roomSkinInfoListCurPos + __roomSkinInfoListCustLen);

		roomSkinInfoList.add(_roomSkinInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)roomSkinInfoList.size());
	for(int _i = 0; _i < roomSkinInfoList.size(); _i++) { 
		_buf.putInt(roomSkinInfoList.get(_i).GetBufSize());
	roomSkinInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)87);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)87);
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

