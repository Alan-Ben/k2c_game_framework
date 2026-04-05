using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

public class GS2GC_021_034_RetFriendRecommend : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家信息列表
/// </summary>
private List<NPCommon.PlayerInfo_IconShow> playerList;


public GS2GC_021_034_RetFriendRecommend() {
	playerList = new List<NPCommon.PlayerInfo_IconShow>();
}

public GS2GC_021_034_RetFriendRecommend(
	List<NPCommon.PlayerInfo_IconShow> _playerList
) {	playerList = _playerList;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)34; }

/// <summary>
/// 玩家信息列表
/// </summary>
public List<NPCommon.PlayerInfo_IconShow> getPlayerList() { return playerList; }
/// <summary>
/// 玩家信息列表
/// </summary>
public void addPlayerList(NPCommon.PlayerInfo_IconShow _playerList) { playerList.Add(_playerList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < playerList.Count; _i++) {
	_size += 4 + playerList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < playerList.Count; _i++) {
	_size += 4 + playerList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _playerListCount = _buf.getShort();
	for(int _i = 0; _i < _playerListCount; _i++) { 
		NPCommon.PlayerInfo_IconShow _playerList = new NPCommon.PlayerInfo_IconShow();
		int __playerListCustLen = _buf.getInt();
	int __playerListCurPos = _buf.getCurPos();
	_playerList.ReadUnzipBuf(_buf, __playerListCurPos + __playerListCustLen);
	_buf.setPosition(__playerListCurPos + __playerListCustLen);

		playerList.Add(_playerList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)playerList.Count);
	for(int _i = 0; _i < playerList.Count; _i++) { 
		_buf.putInt(playerList[_i].GetBufSize());
	playerList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)34);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)34);
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
	builder.Append("playerList").Append(":").Append(playerList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

