package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_004_RetEquipInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 藏品列表 */
private java.util.ArrayList<Common.HeroObj.Equip_BaseInfo> equipList;


public GS2GC_002_004_RetEquipInit() {
	equipList = new java.util.ArrayList<Common.HeroObj.Equip_BaseInfo>();
}

public GS2GC_002_004_RetEquipInit(
	 java.util.ArrayList<Common.HeroObj.Equip_BaseInfo> _equipList
) {	equipList = _equipList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)4; }

/** 藏品列表 */
public java.util.ArrayList<Common.HeroObj.Equip_BaseInfo> getEquipList() { return equipList; }
/** 藏品列表 */
public void addEquipList(Common.HeroObj.Equip_BaseInfo _equipList) { equipList.add(_equipList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (equipList.size() * 41);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (equipList.size() * 41);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _equipListCount = _buf.getShort();
	for(int _i = 0; _i < _equipListCount; _i++) { 
		Common.HeroObj.Equip_BaseInfo _equipList = new Common.HeroObj.Equip_BaseInfo();
		if(_buf.remaining() <= 0) return;
	int __equipListCustLen = _buf.getInt();
	int __equipListCurPos = _buf.position();
	_equipList.ReadUnzipBuf(_buf, __equipListCurPos + __equipListCustLen);
	_buf.position(__equipListCurPos + __equipListCustLen);

		equipList.add(_equipList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)equipList.size());
	for(int _i = 0; _i < equipList.size(); _i++) { 
		_buf.putInt(equipList.get(_i).GetBufSize());
	equipList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)4);
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

