package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
public class GS2GC_036_009_RetTreasureHuntTransOre implements ALBasicProtocolPack._IALProtocolStructure {
/** 转换结果列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TransOreResult> transResultList;


public GS2GC_036_009_RetTreasureHuntTransOre() {
	transResultList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TransOreResult>();
}

public GS2GC_036_009_RetTreasureHuntTransOre(
	 java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TransOreResult> _transResultList
) {	transResultList = _transResultList;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)9; }

/** 转换结果列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TransOreResult> getTransResultList() { return transResultList; }
/** 转换结果列表 */
public void addTransResultList(Common.TreasureHuntObj.TreasureHunt_TransOreResult _transResultList) { transResultList.add(_transResultList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (transResultList.size() * 17);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (transResultList.size() * 17);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _transResultListCount = _buf.getShort();
	for(int _i = 0; _i < _transResultListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_TransOreResult _transResultList = new Common.TreasureHuntObj.TreasureHunt_TransOreResult();
		if(_buf.remaining() <= 0) return;
	int __transResultListCustLen = _buf.getInt();
	int __transResultListCurPos = _buf.position();
	_transResultList.ReadUnzipBuf(_buf, __transResultListCurPos + __transResultListCustLen);
	_buf.position(__transResultListCurPos + __transResultListCustLen);

		transResultList.add(_transResultList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)transResultList.size());
	for(int _i = 0; _i < transResultList.size(); _i++) { 
		_buf.putInt(transResultList.get(_i).GetBufSize());
	transResultList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)9);
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

