package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_022_RetTowerChallengeList implements ALBasicProtocolPack._IALProtocolStructure {
/** 对手列表 */
private java.util.ArrayList<Common.TowerObj.Tower_OpponentInfo> opponentList;


public GS2GC_023_022_RetTowerChallengeList() {
	opponentList = new java.util.ArrayList<Common.TowerObj.Tower_OpponentInfo>();
}

public GS2GC_023_022_RetTowerChallengeList(
	 java.util.ArrayList<Common.TowerObj.Tower_OpponentInfo> _opponentList
) {	opponentList = _opponentList;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)22; }

/** 对手列表 */
public java.util.ArrayList<Common.TowerObj.Tower_OpponentInfo> getOpponentList() { return opponentList; }
/** 对手列表 */
public void addOpponentList(Common.TowerObj.Tower_OpponentInfo _opponentList) { opponentList.add(_opponentList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (opponentList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (opponentList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _opponentListCount = _buf.getShort();
	for(int _i = 0; _i < _opponentListCount; _i++) { 
		Common.TowerObj.Tower_OpponentInfo _opponentList = new Common.TowerObj.Tower_OpponentInfo();
		if(_buf.remaining() <= 0) return;
	int __opponentListCustLen = _buf.getInt();
	int __opponentListCurPos = _buf.position();
	_opponentList.ReadUnzipBuf(_buf, __opponentListCurPos + __opponentListCustLen);
	_buf.position(__opponentListCurPos + __opponentListCustLen);

		opponentList.add(_opponentList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)opponentList.size());
	for(int _i = 0; _i < opponentList.size(); _i++) { 
		_buf.putInt(opponentList.get(_i).GetBufSize());
	opponentList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)22);
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

