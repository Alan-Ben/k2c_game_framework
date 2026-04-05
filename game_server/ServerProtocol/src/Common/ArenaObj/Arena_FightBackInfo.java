package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场反击数据
 **/
public class Arena_FightBackInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 展示信息 */
private Common.ArenaObj.Arena_BattleReportShow showInfo;
/** 是否反击 */
private boolean hadFightBack;


public Arena_FightBackInfo() {
	dbId = (long)0;
	showInfo = new Common.ArenaObj.Arena_BattleReportShow();
	hadFightBack = false;
}

public Arena_FightBackInfo(
	 long _dbId
	, Common.ArenaObj.Arena_BattleReportShow _showInfo
	, boolean _hadFightBack
) {	dbId = _dbId;
	showInfo = _showInfo;
	hadFightBack = _hadFightBack;
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
/** 是否反击 */
public boolean getHadFightBack() { return hadFightBack; }
/** 是否反击 */
public void setHadFightBack(boolean _hadFightBack) { hadFightBack = _hadFightBack; }


public final int GetBufSize() {
	int _size = 37;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 39;

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

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadFightBack = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
	_buf.put(hadFightBack?(byte)1:(byte)0);
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

