package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-队伍战斗单个成员战斗结果信息
 **/
public class ServerObj_MarsBattleV2_MemberFightResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 成员标记Id，用于区分战损 */
private long memberId;
/** 兵种等级 */
private int teamSoldierLvl;
/** 兵种数量 */
private long soldierNum;
/** 本次战斗兵种损耗数量 */
private long soldierLossValue;
/** 兵种最终损耗量 */
private long soldierFinalLossValue;


public ServerObj_MarsBattleV2_MemberFightResult() {
	memberId = (long)0;
	teamSoldierLvl = 0;
	soldierNum = (long)0;
	soldierLossValue = (long)0;
	soldierFinalLossValue = (long)0;
}

public ServerObj_MarsBattleV2_MemberFightResult(
	 long _memberId
	, int _teamSoldierLvl
	, long _soldierNum
	, long _soldierLossValue
	, long _soldierFinalLossValue
) {	memberId = _memberId;
	teamSoldierLvl = _teamSoldierLvl;
	soldierNum = _soldierNum;
	soldierLossValue = _soldierLossValue;
	soldierFinalLossValue = _soldierFinalLossValue;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 成员标记Id，用于区分战损 */
public long getMemberId() { return memberId; }
/** 成员标记Id，用于区分战损 */
public void setMemberId(long _memberId) { memberId = _memberId; }
/** 兵种等级 */
public int getTeamSoldierLvl() { return teamSoldierLvl; }
/** 兵种等级 */
public void setTeamSoldierLvl(int _teamSoldierLvl) { teamSoldierLvl = _teamSoldierLvl; }
/** 兵种数量 */
public long getSoldierNum() { return soldierNum; }
/** 兵种数量 */
public void setSoldierNum(long _soldierNum) { soldierNum = _soldierNum; }
/** 本次战斗兵种损耗数量 */
public long getSoldierLossValue() { return soldierLossValue; }
/** 本次战斗兵种损耗数量 */
public void setSoldierLossValue(long _soldierLossValue) { soldierLossValue = _soldierLossValue; }
/** 兵种最终损耗量 */
public long getSoldierFinalLossValue() { return soldierFinalLossValue; }
/** 兵种最终损耗量 */
public void setSoldierFinalLossValue(long _soldierFinalLossValue) { soldierFinalLossValue = _soldierFinalLossValue; }


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
	if(_buf.remaining() > 0) memberId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamSoldierLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) soldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) soldierLossValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) soldierFinalLossValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(memberId);
	_buf.putInt(teamSoldierLvl);
	_buf.putLong(soldierNum);
	_buf.putLong(soldierLossValue);
	_buf.putLong(soldierFinalLossValue);
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

