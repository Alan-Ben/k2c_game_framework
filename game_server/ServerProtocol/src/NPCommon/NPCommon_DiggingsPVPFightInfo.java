package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 矿区PVP战斗初始化数据
 **/
public class NPCommon_DiggingsPVPFightInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 攻击玩家阵容信息 */
private NPCommon.NPCommon_PlayerFightInfo attackerFightInfo;
/** 防守玩家阵容信息 */
private NPCommon.NPCommon_PlayerFightInfo defencerFightInfo;


public NPCommon_DiggingsPVPFightInfo() {
	attackerFightInfo = new NPCommon.NPCommon_PlayerFightInfo();
	defencerFightInfo = new NPCommon.NPCommon_PlayerFightInfo();
}

public NPCommon_DiggingsPVPFightInfo(
	 NPCommon.NPCommon_PlayerFightInfo _attackerFightInfo
	, NPCommon.NPCommon_PlayerFightInfo _defencerFightInfo
) {	attackerFightInfo = _attackerFightInfo;
	defencerFightInfo = _defencerFightInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 攻击玩家阵容信息 */
public NPCommon.NPCommon_PlayerFightInfo getAttackerFightInfo() { return attackerFightInfo; }
/** 攻击玩家阵容信息 */
public void setAttackerFightInfo(NPCommon.NPCommon_PlayerFightInfo _attackerFightInfo) { attackerFightInfo = _attackerFightInfo; }
/** 防守玩家阵容信息 */
public NPCommon.NPCommon_PlayerFightInfo getDefencerFightInfo() { return defencerFightInfo; }
/** 防守玩家阵容信息 */
public void setDefencerFightInfo(NPCommon.NPCommon_PlayerFightInfo _defencerFightInfo) { defencerFightInfo = _defencerFightInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + attackerFightInfo.GetBufSize();
	_size += 4 + defencerFightInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + attackerFightInfo.GetBufSize();
	_size += 4 + defencerFightInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _attackerFightInfoCustLen = _buf.getInt();
	int _attackerFightInfoCurPos = _buf.position();
	attackerFightInfo.ReadUnzipBuf(_buf, _attackerFightInfoCurPos + _attackerFightInfoCustLen);
	_buf.position(_attackerFightInfoCurPos + _attackerFightInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _defencerFightInfoCustLen = _buf.getInt();
	int _defencerFightInfoCurPos = _buf.position();
	defencerFightInfo.ReadUnzipBuf(_buf, _defencerFightInfoCurPos + _defencerFightInfoCustLen);
	_buf.position(_defencerFightInfoCurPos + _defencerFightInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(attackerFightInfo.GetBufSize());
	attackerFightInfo.PutUnzipBuf(_buf);
	_buf.putInt(defencerFightInfo.GetBufSize());
	defencerFightInfo.PutUnzipBuf(_buf);
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

