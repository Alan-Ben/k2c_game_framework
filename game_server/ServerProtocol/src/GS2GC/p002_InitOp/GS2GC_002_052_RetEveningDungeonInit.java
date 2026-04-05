package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_052_RetEveningDungeonInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 副本信息 */
private Common.DungeonObj.EveningDungeon_Info info;
/** 副本时间信息 */
private Common.DungeonObj.EveningDungeon_TimeInfo timeInfo;


public GS2GC_002_052_RetEveningDungeonInit() {
	info = new Common.DungeonObj.EveningDungeon_Info();
	timeInfo = new Common.DungeonObj.EveningDungeon_TimeInfo();
}

public GS2GC_002_052_RetEveningDungeonInit(
	 Common.DungeonObj.EveningDungeon_Info _info
	, Common.DungeonObj.EveningDungeon_TimeInfo _timeInfo
) {	info = _info;
	timeInfo = _timeInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)52; }

/** 副本信息 */
public Common.DungeonObj.EveningDungeon_Info getInfo() { return info; }
/** 副本信息 */
public void setInfo(Common.DungeonObj.EveningDungeon_Info _info) { info = _info; }
/** 副本时间信息 */
public Common.DungeonObj.EveningDungeon_TimeInfo getTimeInfo() { return timeInfo; }
/** 副本时间信息 */
public void setTimeInfo(Common.DungeonObj.EveningDungeon_TimeInfo _timeInfo) { timeInfo = _timeInfo; }


public final int GetBufSize() {
	int _size = 36;
	_size += 4 + info.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + info.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _timeInfoCustLen = _buf.getInt();
	int _timeInfoCurPos = _buf.position();
	timeInfo.ReadUnzipBuf(_buf, _timeInfoCurPos + _timeInfoCustLen);
	_buf.position(_timeInfoCurPos + _timeInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putInt(timeInfo.GetBufSize());
	timeInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

