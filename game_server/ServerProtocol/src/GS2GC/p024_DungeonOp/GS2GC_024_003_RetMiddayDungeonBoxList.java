package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_003_RetMiddayDungeonBoxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱列表 */
private java.util.ArrayList<Common.DungeonObj.MiddayDungeon_BoxInfo> boxList;


public GS2GC_024_003_RetMiddayDungeonBoxList() {
	boxList = new java.util.ArrayList<Common.DungeonObj.MiddayDungeon_BoxInfo>();
}

public GS2GC_024_003_RetMiddayDungeonBoxList(
	 java.util.ArrayList<Common.DungeonObj.MiddayDungeon_BoxInfo> _boxList
) {	boxList = _boxList;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)3; }

/** 宝箱列表 */
public java.util.ArrayList<Common.DungeonObj.MiddayDungeon_BoxInfo> getBoxList() { return boxList; }
/** 宝箱列表 */
public void addBoxList(Common.DungeonObj.MiddayDungeon_BoxInfo _boxList) { boxList.add(_boxList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < boxList.size(); _i++) {
	_size += 4 + boxList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < boxList.size(); _i++) {
	_size += 4 + boxList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _boxListCount = _buf.getShort();
	for(int _i = 0; _i < _boxListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_BoxInfo _boxList = new Common.DungeonObj.MiddayDungeon_BoxInfo();
		if(_buf.remaining() <= 0) return;
	int __boxListCustLen = _buf.getInt();
	int __boxListCurPos = _buf.position();
	_boxList.ReadUnzipBuf(_buf, __boxListCurPos + __boxListCustLen);
	_buf.position(__boxListCurPos + __boxListCustLen);

		boxList.add(_boxList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)boxList.size());
	for(int _i = 0; _i < boxList.size(); _i++) { 
		_buf.putInt(boxList.get(_i).GetBufSize());
	boxList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)3);
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

