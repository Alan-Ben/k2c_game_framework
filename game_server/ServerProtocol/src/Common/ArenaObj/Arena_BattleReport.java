package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场战报
 **/
public class Arena_BattleReport implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 展示信息 */
private Common.ArenaObj.Arena_BattleReportShow showInfo;


public Arena_BattleReport() {
	dbId = (long)0;
	showInfo = new Common.ArenaObj.Arena_BattleReportShow();
}

public Arena_BattleReport(
	 long _dbId
	, Common.ArenaObj.Arena_BattleReportShow _showInfo
) {	dbId = _dbId;
	showInfo = _showInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 展示信息 */
public Common.ArenaObj.Arena_BattleReportShow getShowInfo() { return showInfo; }
/** 展示信息 */
public void setShowInfo(Common.ArenaObj.Arena_BattleReportShow _showInfo) { showInfo = _showInfo; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.position();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.position(_showInfoCurPos + _showInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
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

