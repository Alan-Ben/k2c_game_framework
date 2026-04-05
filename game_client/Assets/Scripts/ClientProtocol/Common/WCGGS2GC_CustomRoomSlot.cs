using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_CustomRoomSlot : ALBasicProtocolPack._IALProtocolStructure {
private int slotId;
private bool isOB;
private int campId;
private int groupId;
private bool hasPlayer;
private Common.WCGGS2GC_CustomRoomPlayer Player;


public WCGGS2GC_CustomRoomSlot() {
	slotId = 0;
	isOB = false;
	campId = 0;
	groupId = 0;
	hasPlayer = false;
	Player = new Common.WCGGS2GC_CustomRoomPlayer();
}

public WCGGS2GC_CustomRoomSlot(
	int _slotId
	, bool _isOB
	, int _campId
	, int _groupId
	, bool _hasPlayer
	, Common.WCGGS2GC_CustomRoomPlayer _Player
) {	slotId = _slotId;
	isOB = _isOB;
	campId = _campId;
	groupId = _groupId;
	hasPlayer = _hasPlayer;
	Player = _Player;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getSlotId() { return slotId; }
public void setSlotId(int _slotId) { slotId = _slotId; }
public bool getIsOB() { return isOB; }
public void setIsOB(bool _isOB) { isOB = _isOB; }
public int getCampId() { return campId; }
public void setCampId(int _campId) { campId = _campId; }
public int getGroupId() { return groupId; }
public void setGroupId(int _groupId) { groupId = _groupId; }
public bool getHasPlayer() { return hasPlayer; }
public void setHasPlayer(bool _hasPlayer) { hasPlayer = _hasPlayer; }
public Common.WCGGS2GC_CustomRoomPlayer getPlayer() { return Player; }
public void setPlayer(Common.WCGGS2GC_CustomRoomPlayer _Player) { Player = _Player; }


public int GetBufSize() {
	int _size = 14;
	_size += 4 + Player.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 16;
	_size += 4 + Player.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	slotId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOB = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	campId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasPlayer = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _PlayerCustLen = _buf.getInt();
	int _PlayerCurPos = _buf.getCurPos();
	Player.ReadUnzipBuf(_buf, _PlayerCurPos + _PlayerCustLen);
	_buf.setPosition(_PlayerCurPos + _PlayerCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(slotId);
	_buf.put(isOB?(byte)1:(byte)0);
	_buf.putInt(campId);
	_buf.putInt(groupId);
	_buf.put(hasPlayer?(byte)1:(byte)0);
	_buf.putInt(Player.GetBufSize());
	Player.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("slotId").Append(":").Append(slotId.ToString()).Append(", ");
	builder.Append("isOB").Append(":").Append(isOB.ToString()).Append(", ");
	builder.Append("campId").Append(":").Append(campId.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("hasPlayer").Append(":").Append(hasPlayer.ToString()).Append(", ");
	builder.Append("Player").Append(":").Append(Player == null ? "null" : Player.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

