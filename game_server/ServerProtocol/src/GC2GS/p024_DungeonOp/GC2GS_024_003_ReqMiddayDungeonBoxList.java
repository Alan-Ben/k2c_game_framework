package GC2GS.p024_DungeonOp;

import java.nio.ByteBuffer;
/*********
 * 查询午间副本宝箱列表
 **/
public class GC2GS_024_003_ReqMiddayDungeonBoxList implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱类型 */
private Common.DungeonEnum.EDungeonBoxType boxType;


public GC2GS_024_003_ReqMiddayDungeonBoxList() {
	boxType = Common.DungeonEnum.EDungeonBoxType.values()[0];
}

public GC2GS_024_003_ReqMiddayDungeonBoxList(
	 Common.DungeonEnum.EDungeonBoxType _boxType
) {	boxType = _boxType;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)3; }

/** 宝箱类型 */
public Common.DungeonEnum.EDungeonBoxType getBoxType() { return boxType; }
/** 宝箱类型 */
public void setBoxType(Common.DungeonEnum.EDungeonBoxType _boxType) { boxType = _boxType; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxType = Common.DungeonEnum.EDungeonBoxType.EDungeonBoxType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxType.ordinal());

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

