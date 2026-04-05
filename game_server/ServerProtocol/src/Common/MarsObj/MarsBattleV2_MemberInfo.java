package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-队伍战斗单个成员队伍信息
 **/
public class MarsBattleV2_MemberInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 成员标记Id，用于区分战损 */
private long memberId;
/** 兵种等级 */
private int teamSoldierLvl;
/** 兵种数量 */
private long soldierNum;
/** 兵种原损耗量 */
private long soldierLossValue;


public MarsBattleV2_MemberInfo() {
	memberId = (long)0;
	teamSoldierLvl = 0;
	soldierNum = (long)0;
	soldierLossValue = (long)0;
}

public MarsBattleV2_MemberInfo(
	 long _memberId
	, int _teamSoldierLvl
	, long _soldierNum
	, long _soldierLossValue
) {	memberId = _memberId;
	teamSoldierLvl = _teamSoldierLvl;
	soldierNum = _soldierNum;
	soldierLossValue = _soldierLossValue;
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
/** 兵种原损耗量 */
public long getSoldierLossValue() { return soldierLossValue; }
/** 兵种原损耗量 */
public void setSoldierLossValue(long _soldierLossValue) { soldierLossValue = _soldierLossValue; }


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
	if(_buf.remaining() > 0) memberId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamSoldierLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) soldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) soldierLossValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(memberId);
	_buf.putInt(teamSoldierLvl);
	_buf.putLong(soldierNum);
	_buf.putLong(soldierLossValue);
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

