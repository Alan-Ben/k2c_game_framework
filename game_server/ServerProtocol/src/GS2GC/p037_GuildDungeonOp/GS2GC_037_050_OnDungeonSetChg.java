package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_037_050_OnDungeonSetChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildDungeonObj.GuildDungeon_SetInfo setInfo;


public GS2GC_037_050_OnDungeonSetChg() {
	setInfo = new Common.GuildDungeonObj.GuildDungeon_SetInfo();
}

public GS2GC_037_050_OnDungeonSetChg(
	 Common.GuildDungeonObj.GuildDungeon_SetInfo _setInfo
) {	setInfo = _setInfo;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)50; }

public Common.GuildDungeonObj.GuildDungeon_SetInfo getSetInfo() { return setInfo; }
public void setSetInfo(Common.GuildDungeonObj.GuildDungeon_SetInfo _setInfo) { setInfo = _setInfo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _setInfoCustLen = _buf.getInt();
	int _setInfoCurPos = _buf.position();
	setInfo.ReadUnzipBuf(_buf, _setInfoCurPos + _setInfoCustLen);
	_buf.position(_setInfoCurPos + _setInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(setInfo.GetBufSize());
	setInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)50);
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

