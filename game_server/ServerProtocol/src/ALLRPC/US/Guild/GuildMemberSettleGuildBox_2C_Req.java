package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildMemberSettleGuildBox_2C_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private Common.ServerObj.ServerObj_GuildBoxSettleList boxList;


public GuildMemberSettleGuildBox_2C_Req() {
	cid = (long)0;
	boxList = new Common.ServerObj.ServerObj_GuildBoxSettleList();
}

public GuildMemberSettleGuildBox_2C_Req(
	 long _cid
	, Common.ServerObj.ServerObj_GuildBoxSettleList _boxList
) {	cid = _cid;
	boxList = _boxList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public Common.ServerObj.ServerObj_GuildBoxSettleList getBoxList() { return boxList; }
public void setBoxList(Common.ServerObj.ServerObj_GuildBoxSettleList _boxList) { boxList = _boxList; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + boxList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + boxList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _boxListCustLen = _buf.getInt();
	int _boxListCurPos = _buf.position();
	boxList.ReadUnzipBuf(_buf, _boxListCurPos + _boxListCustLen);
	_buf.position(_boxListCurPos + _boxListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(boxList.GetBufSize());
	boxList.PutUnzipBuf(_buf);
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

