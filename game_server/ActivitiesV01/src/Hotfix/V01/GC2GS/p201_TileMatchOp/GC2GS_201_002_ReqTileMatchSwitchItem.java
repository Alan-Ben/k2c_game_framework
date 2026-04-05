package Hotfix.V01.GC2GS.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消交换格子
 **/
public class GC2GS_201_002_ReqTileMatchSwitchItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 模式类型 */
private Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType modeType;
/** 交换起点 */
private int startIndex;
/** 交换终点 */
private int endIndex;


public GC2GS_201_002_ReqTileMatchSwitchItem() {
	modeType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType.values()[0];
	startIndex = 0;
	endIndex = 0;
}

public GC2GS_201_002_ReqTileMatchSwitchItem(
	 Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType _modeType
	, int _startIndex
	, int _endIndex
) {	modeType = _modeType;
	startIndex = _startIndex;
	endIndex = _endIndex;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)2; }

/** 模式类型 */
public Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType getModeType() { return modeType; }
/** 模式类型 */
public void setModeType(Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType _modeType) { modeType = _modeType; }
/** 交换起点 */
public int getStartIndex() { return startIndex; }
/** 交换起点 */
public void setStartIndex(int _startIndex) { startIndex = _startIndex; }
/** 交换终点 */
public int getEndIndex() { return endIndex; }
/** 交换终点 */
public void setEndIndex(int _endIndex) { endIndex = _endIndex; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) modeType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType.ETileMatch_ModeType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endIndex = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(modeType.ordinal());

	_buf.putInt(startIndex);
	_buf.putInt(endIndex);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
	_recBuf.put((byte)2);
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

