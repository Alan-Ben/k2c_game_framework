package GS2GC.p031_RankOp;

import java.nio.ByteBuffer;
public class GS2GC_031_006_RetRankFixedInfoByKey implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础数据 */
private Common.RankObj.Rank_BaseItem info;


public GS2GC_031_006_RetRankFixedInfoByKey() {
	info = new Common.RankObj.Rank_BaseItem();
}

public GS2GC_031_006_RetRankFixedInfoByKey(
	 Common.RankObj.Rank_BaseItem _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)31; }

public final byte getSubOrder() { return (byte)6; }

/** 基础数据 */
public Common.RankObj.Rank_BaseItem getInfo() { return info; }
/** 基础数据 */
public void setInfo(Common.RankObj.Rank_BaseItem _info) { info = _info; }


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
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)6);
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

