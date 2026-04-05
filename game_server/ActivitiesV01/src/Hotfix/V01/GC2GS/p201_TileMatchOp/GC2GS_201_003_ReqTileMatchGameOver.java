package Hotfix.V01.GC2GS.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消死局重开
 **/
public class GC2GS_201_003_ReqTileMatchGameOver implements ALBasicProtocolPack._IALProtocolStructure {
/** 模式类型 */
private Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType modeType;


public GC2GS_201_003_ReqTileMatchGameOver() {
	modeType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType.values()[0];
}

public GC2GS_201_003_ReqTileMatchGameOver(
	 Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType _modeType
) {	modeType = _modeType;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)3; }

/** 模式类型 */
public Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType getModeType() { return modeType; }
/** 模式类型 */
public void setModeType(Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType _modeType) { modeType = _modeType; }


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
	if(_buf.remaining() > 0) modeType = Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType.ETileMatch_ModeType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(modeType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
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

