using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_087_RetPlayerRoomSkin : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家拥有的房间皮肤信息队列
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin> roomSkinInfoList;


public GS2GC_002_087_RetPlayerRoomSkin() {
	roomSkinInfoList = new List<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin>();
}

public GS2GC_002_087_RetPlayerRoomSkin(
	List<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin> _roomSkinInfoList
) {	roomSkinInfoList = _roomSkinInfoList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)87; }

/// <summary>
/// 玩家拥有的房间皮肤信息队列
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_RoomSkin> getRoomSkinInfoList() { return roomSkinInfoList; }
/// <summary>
/// 玩家拥有的房间皮肤信息队列
/// </summary>
public void addRoomSkinInfoList(Common.NpPlayerInfoObj.PlayerInfo_RoomSkin _roomSkinInfoList) { roomSkinInfoList.Add(_roomSkinInfoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (roomSkinInfoList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (roomSkinInfoList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _roomSkinInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _roomSkinInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_RoomSkin _roomSkinInfoList = new Common.NpPlayerInfoObj.PlayerInfo_RoomSkin();
		int __roomSkinInfoListCustLen = _buf.getInt();
	int __roomSkinInfoListCurPos = _buf.getCurPos();
	_roomSkinInfoList.ReadUnzipBuf(_buf, __roomSkinInfoListCurPos + __roomSkinInfoListCustLen);
	_buf.setPosition(__roomSkinInfoListCurPos + __roomSkinInfoListCustLen);

		roomSkinInfoList.Add(_roomSkinInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)roomSkinInfoList.Count);
	for(int _i = 0; _i < roomSkinInfoList.Count; _i++) { 
		_buf.putInt(roomSkinInfoList[_i].GetBufSize());
	roomSkinInfoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)87);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)87);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("roomSkinInfoList").Append(":").Append(roomSkinInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

