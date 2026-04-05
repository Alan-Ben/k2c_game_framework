package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_CustomRoomSlot implements ALBasicProtocolPack._IALProtocolStructure {
private int slotId;
private boolean isOB;
private int campId;
private int groupId;
private boolean hasPlayer;
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
	, boolean _isOB
	, int _campId
	, int _groupId
	, boolean _hasPlayer
	, Common.WCGGS2GC_CustomRoomPlayer _Player
) {	slotId = _slotId;
	isOB = _isOB;
	campId = _campId;
	groupId = _groupId;
	hasPlayer = _hasPlayer;
	Player = _Player;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getSlotId() { return slotId; }
public void setSlotId(int _slotId) { slotId = _slotId; }
public boolean getIsOB() { return isOB; }
public void setIsOB(boolean _isOB) { isOB = _isOB; }
public int getCampId() { return campId; }
public void setCampId(int _campId) { campId = _campId; }
public int getGroupId() { return groupId; }
public void setGroupId(int _groupId) { groupId = _groupId; }
public boolean getHasPlayer() { return hasPlayer; }
public void setHasPlayer(boolean _hasPlayer) { hasPlayer = _hasPlayer; }
public Common.WCGGS2GC_CustomRoomPlayer getPlayer() { return Player; }
public void setPlayer(Common.WCGGS2GC_CustomRoomPlayer _Player) { Player = _Player; }


public final int GetBufSize() {
	int _size = 14;
	_size += 4 + Player.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;
	_size += 4 + Player.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) slotId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOB = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) campId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasPlayer = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _PlayerCustLen = _buf.getInt();
	int _PlayerCurPos = _buf.position();
	Player.ReadUnzipBuf(_buf, _PlayerCurPos + _PlayerCustLen);
	_buf.position(_PlayerCurPos + _PlayerCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(slotId);
	_buf.put(isOB?(byte)1:(byte)0);
	_buf.putInt(campId);
	_buf.putInt(groupId);
	_buf.put(hasPlayer?(byte)1:(byte)0);
	_buf.putInt(Player.GetBufSize());
	Player.PutUnzipBuf(_buf);
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

