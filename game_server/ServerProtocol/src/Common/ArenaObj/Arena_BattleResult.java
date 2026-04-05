package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场战斗结果
 **/
public class Arena_BattleResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 已击败对手数量 */
private int hadDefeatNum;
/** 对手大臣数量 */
private int opponentHeroNum;
/** 对手扣除影响力 */
private int opponentDeductinfluence;
/** 获得影响力 */
private int gainInfluence;
/** 获得道具列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> gainItem;
/** 实力提升 */
private long addPower;
/** 指定攻击道具id */
private long selectAttackItemId;
/** 是否结算 */
private boolean isSettle;


public Arena_BattleResult() {
	hadDefeatNum = 0;
	opponentHeroNum = 0;
	opponentDeductinfluence = 0;
	gainInfluence = 0;
	gainItem = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	addPower = (long)0;
	selectAttackItemId = (long)0;
	isSettle = false;
}

public Arena_BattleResult(
	 int _hadDefeatNum
	, int _opponentHeroNum
	, int _opponentDeductinfluence
	, int _gainInfluence
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _gainItem
	, long _addPower
	, long _selectAttackItemId
	, boolean _isSettle
) {	hadDefeatNum = _hadDefeatNum;
	opponentHeroNum = _opponentHeroNum;
	opponentDeductinfluence = _opponentDeductinfluence;
	gainInfluence = _gainInfluence;
	gainItem = _gainItem;
	addPower = _addPower;
	selectAttackItemId = _selectAttackItemId;
	isSettle = _isSettle;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已击败对手数量 */
public int getHadDefeatNum() { return hadDefeatNum; }
/** 已击败对手数量 */
public void setHadDefeatNum(int _hadDefeatNum) { hadDefeatNum = _hadDefeatNum; }
/** 对手大臣数量 */
public int getOpponentHeroNum() { return opponentHeroNum; }
/** 对手大臣数量 */
public void setOpponentHeroNum(int _opponentHeroNum) { opponentHeroNum = _opponentHeroNum; }
/** 对手扣除影响力 */
public int getOpponentDeductinfluence() { return opponentDeductinfluence; }
/** 对手扣除影响力 */
public void setOpponentDeductinfluence(int _opponentDeductinfluence) { opponentDeductinfluence = _opponentDeductinfluence; }
/** 获得影响力 */
public int getGainInfluence() { return gainInfluence; }
/** 获得影响力 */
public void setGainInfluence(int _gainInfluence) { gainInfluence = _gainInfluence; }
/** 获得道具列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getGainItem() { return gainItem; }
/** 获得道具列表 */
public void addGainItem(NPCommon.NPCommon_ItemInfo _gainItem) { gainItem.add(_gainItem); }
/** 实力提升 */
public long getAddPower() { return addPower; }
/** 实力提升 */
public void setAddPower(long _addPower) { addPower = _addPower; }
/** 指定攻击道具id */
public long getSelectAttackItemId() { return selectAttackItemId; }
/** 指定攻击道具id */
public void setSelectAttackItemId(long _selectAttackItemId) { selectAttackItemId = _selectAttackItemId; }
/** 是否结算 */
public boolean getIsSettle() { return isSettle; }
/** 是否结算 */
public void setIsSettle(boolean _isSettle) { isSettle = _isSettle; }


public final int GetBufSize() {
	int _size = 33;
	_size += 2;
	for(int _i = 0; _i < gainItem.size(); _i++) {
	_size += 4 + gainItem.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;
	_size += 2;
	for(int _i = 0; _i < gainItem.size(); _i++) {
	_size += 4 + gainItem.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDefeatNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentDeductinfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainInfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainItemCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItem = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __gainItemCustLen = _buf.getInt();
	int __gainItemCurPos = _buf.position();
	_gainItem.ReadUnzipBuf(_buf, __gainItemCurPos + __gainItemCustLen);
	_buf.position(__gainItemCurPos + __gainItemCustLen);

		gainItem.add(_gainItem);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) selectAttackItemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSettle = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(hadDefeatNum);
	_buf.putInt(opponentHeroNum);
	_buf.putInt(opponentDeductinfluence);
	_buf.putInt(gainInfluence);
	_buf.putShort((short)gainItem.size());
	for(int _i = 0; _i < gainItem.size(); _i++) { 
		_buf.putInt(gainItem.get(_i).GetBufSize());
	gainItem.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(addPower);
	_buf.putLong(selectAttackItemId);
	_buf.put(isSettle?(byte)1:(byte)0);
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

