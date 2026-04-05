package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_052_OnMiddayDungeonTimeInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 副本时间信息 */
private Common.DungeonObj.MiddayDungeon_TimeInfo info;


public GS2GC_024_052_OnMiddayDungeonTimeInfoChg() {
	info = new Common.DungeonObj.MiddayDungeon_TimeInfo();
}

public GS2GC_024_052_OnMiddayDungeonTimeInfoChg(
	 Common.DungeonObj.MiddayDungeon_TimeInfo _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)52; }

/** 副本时间信息 */
public Common.DungeonObj.MiddayDungeon_TimeInfo getInfo() { return info; }
/** 副本时间信息 */
public void setInfo(Common.DungeonObj.MiddayDungeon_TimeInfo _info) { info = _info; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

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
	_buf.put((byte)24);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)52);
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

