package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_011_RetArenaFightBackData implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ArenaObj.Arena_FightBackInfo> fightBackList;


public GS2GC_023_011_RetArenaFightBackData() {
	fightBackList = new java.util.ArrayList<Common.ArenaObj.Arena_FightBackInfo>();
}

public GS2GC_023_011_RetArenaFightBackData(
	 java.util.ArrayList<Common.ArenaObj.Arena_FightBackInfo> _fightBackList
) {	fightBackList = _fightBackList;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)11; }

public java.util.ArrayList<Common.ArenaObj.Arena_FightBackInfo> getFightBackList() { return fightBackList; }
public void addFightBackList(Common.ArenaObj.Arena_FightBackInfo _fightBackList) { fightBackList.add(_fightBackList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (fightBackList.size() * 41);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (fightBackList.size() * 41);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _fightBackListCount = _buf.getShort();
	for(int _i = 0; _i < _fightBackListCount; _i++) { 
		Common.ArenaObj.Arena_FightBackInfo _fightBackList = new Common.ArenaObj.Arena_FightBackInfo();
		if(_buf.remaining() <= 0) return;
	int __fightBackListCustLen = _buf.getInt();
	int __fightBackListCurPos = _buf.position();
	_fightBackList.ReadUnzipBuf(_buf, __fightBackListCurPos + __fightBackListCustLen);
	_buf.position(__fightBackListCurPos + __fightBackListCustLen);

		fightBackList.add(_fightBackList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)fightBackList.size());
	for(int _i = 0; _i < fightBackList.size(); _i++) { 
		_buf.putInt(fightBackList.get(_i).GetBufSize());
	fightBackList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)11);
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

