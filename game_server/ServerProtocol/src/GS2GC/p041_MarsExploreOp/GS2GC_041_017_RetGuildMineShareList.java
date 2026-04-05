package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
public class GS2GC_041_017_RetGuildMineShareList implements ALBasicProtocolPack._IALProtocolStructure {
/** 分享矿列表 */
private java.util.ArrayList<Common.GuildObj.Guild_MineShareInfo> mineShareList;


public GS2GC_041_017_RetGuildMineShareList() {
	mineShareList = new java.util.ArrayList<Common.GuildObj.Guild_MineShareInfo>();
}

public GS2GC_041_017_RetGuildMineShareList(
	 java.util.ArrayList<Common.GuildObj.Guild_MineShareInfo> _mineShareList
) {	mineShareList = _mineShareList;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)17; }

/** 分享矿列表 */
public java.util.ArrayList<Common.GuildObj.Guild_MineShareInfo> getMineShareList() { return mineShareList; }
/** 分享矿列表 */
public void addMineShareList(Common.GuildObj.Guild_MineShareInfo _mineShareList) { mineShareList.add(_mineShareList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (mineShareList.size() * 36);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (mineShareList.size() * 36);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _mineShareListCount = _buf.getShort();
	for(int _i = 0; _i < _mineShareListCount; _i++) { 
		Common.GuildObj.Guild_MineShareInfo _mineShareList = new Common.GuildObj.Guild_MineShareInfo();
		if(_buf.remaining() <= 0) return;
	int __mineShareListCustLen = _buf.getInt();
	int __mineShareListCurPos = _buf.position();
	_mineShareList.ReadUnzipBuf(_buf, __mineShareListCurPos + __mineShareListCustLen);
	_buf.position(__mineShareListCurPos + __mineShareListCustLen);

		mineShareList.add(_mineShareList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)mineShareList.size());
	for(int _i = 0; _i < mineShareList.size(); _i++) { 
		_buf.putInt(mineShareList.get(_i).GetBufSize());
	mineShareList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)17);
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

