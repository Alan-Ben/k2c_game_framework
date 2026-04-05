package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场数据
 **/
public class Arena_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础数据 */
private Common.ArenaObj.Arena_BaseInfo baseInfo;
/** 战斗数据 */
private Common.ArenaObj.Arena_BattleInfo battleInfo;


public Arena_Info() {
	baseInfo = new Common.ArenaObj.Arena_BaseInfo();
	battleInfo = new Common.ArenaObj.Arena_BattleInfo();
}

public Arena_Info(
	 Common.ArenaObj.Arena_BaseInfo _baseInfo
	, Common.ArenaObj.Arena_BattleInfo _battleInfo
) {	baseInfo = _baseInfo;
	battleInfo = _battleInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 基础数据 */
public Common.ArenaObj.Arena_BaseInfo getBaseInfo() { return baseInfo; }
/** 基础数据 */
public void setBaseInfo(Common.ArenaObj.Arena_BaseInfo _baseInfo) { baseInfo = _baseInfo; }
/** 战斗数据 */
public Common.ArenaObj.Arena_BattleInfo getBattleInfo() { return battleInfo; }
/** 战斗数据 */
public void setBattleInfo(Common.ArenaObj.Arena_BattleInfo _battleInfo) { battleInfo = _battleInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + baseInfo.GetBufSize();
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + baseInfo.GetBufSize();
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.position();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.position(_baseInfoCurPos + _baseInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _battleInfoCustLen = _buf.getInt();
	int _battleInfoCurPos = _buf.position();
	battleInfo.ReadUnzipBuf(_buf, _battleInfoCurPos + _battleInfoCustLen);
	_buf.position(_battleInfoCurPos + _battleInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
	_buf.putInt(battleInfo.GetBufSize());
	battleInfo.PutUnzipBuf(_buf);
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

