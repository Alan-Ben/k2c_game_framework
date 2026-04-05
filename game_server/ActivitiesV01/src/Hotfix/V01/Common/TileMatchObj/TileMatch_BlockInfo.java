package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-方块信息
 **/
public class TileMatch_BlockInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 方块索引 */
private int index;
/** 方块类型 */
private Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo baseInfo;


public TileMatch_BlockInfo() {
	index = 0;
	baseInfo = new Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo();
}

public TileMatch_BlockInfo(
	 int _index
	, Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo _baseInfo
) {	index = _index;
	baseInfo = _baseInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 方块索引 */
public int getIndex() { return index; }
/** 方块索引 */
public void setIndex(int _index) { index = _index; }
/** 方块类型 */
public Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo getBaseInfo() { return baseInfo; }
/** 方块类型 */
public void setBaseInfo(Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo _baseInfo) { baseInfo = _baseInfo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.position();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.position(_baseInfoCurPos + _baseInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
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

