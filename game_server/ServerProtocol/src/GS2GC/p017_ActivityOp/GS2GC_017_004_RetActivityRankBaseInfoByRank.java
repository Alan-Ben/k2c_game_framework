package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_017_004_RetActivityRankBaseInfoByRank implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础数据 */
private Common.RankObj.Rank_BaseItem baseItem;


public GS2GC_017_004_RetActivityRankBaseInfoByRank() {
	baseItem = new Common.RankObj.Rank_BaseItem();
}

public GS2GC_017_004_RetActivityRankBaseInfoByRank(
	 Common.RankObj.Rank_BaseItem _baseItem
) {	baseItem = _baseItem;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)4; }

/** 基础数据 */
public Common.RankObj.Rank_BaseItem getBaseItem() { return baseItem; }
/** 基础数据 */
public void setBaseItem(Common.RankObj.Rank_BaseItem _baseItem) { baseItem = _baseItem; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _baseItemCustLen = _buf.getInt();
	int _baseItemCurPos = _buf.position();
	baseItem.ReadUnzipBuf(_buf, _baseItemCurPos + _baseItemCustLen);
	_buf.position(_baseItemCurPos + _baseItemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(baseItem.GetBufSize());
	baseItem.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
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

