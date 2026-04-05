using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场战斗结果
/// </summary>
public class Arena_BattleResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已击败对手数量
/// </summary>
private int hadDefeatNum;
/// <summary>
/// 对手大臣数量
/// </summary>
private int opponentHeroNum;
/// <summary>
/// 对手扣除影响力
/// </summary>
private int opponentDeductinfluence;
/// <summary>
/// 获得影响力
/// </summary>
private int gainInfluence;
/// <summary>
/// 获得道具列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> gainItem;
/// <summary>
/// 实力提升
/// </summary>
private long addPower;
/// <summary>
/// 指定攻击道具id
/// </summary>
private long selectAttackItemId;
/// <summary>
/// 是否结算
/// </summary>
private bool isSettle;


public Arena_BattleResult() {
	hadDefeatNum = 0;
	opponentHeroNum = 0;
	opponentDeductinfluence = 0;
	gainInfluence = 0;
	gainItem = new List<NPCommon.NPCommon_ItemInfo>();
	addPower = (long)0;
	selectAttackItemId = (long)0;
	isSettle = false;
}

public Arena_BattleResult(
	int _hadDefeatNum
	, int _opponentHeroNum
	, int _opponentDeductinfluence
	, int _gainInfluence
	, List<NPCommon.NPCommon_ItemInfo> _gainItem
	, long _addPower
	, long _selectAttackItemId
	, bool _isSettle
) {	hadDefeatNum = _hadDefeatNum;
	opponentHeroNum = _opponentHeroNum;
	opponentDeductinfluence = _opponentDeductinfluence;
	gainInfluence = _gainInfluence;
	gainItem = _gainItem;
	addPower = _addPower;
	selectAttackItemId = _selectAttackItemId;
	isSettle = _isSettle;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已击败对手数量
/// </summary>
public int getHadDefeatNum() { return hadDefeatNum; }
/// <summary>
/// 已击败对手数量
/// </summary>
public void setHadDefeatNum(int _hadDefeatNum) { hadDefeatNum = _hadDefeatNum; }
/// <summary>
/// 对手大臣数量
/// </summary>
public int getOpponentHeroNum() { return opponentHeroNum; }
/// <summary>
/// 对手大臣数量
/// </summary>
public void setOpponentHeroNum(int _opponentHeroNum) { opponentHeroNum = _opponentHeroNum; }
/// <summary>
/// 对手扣除影响力
/// </summary>
public int getOpponentDeductinfluence() { return opponentDeductinfluence; }
/// <summary>
/// 对手扣除影响力
/// </summary>
public void setOpponentDeductinfluence(int _opponentDeductinfluence) { opponentDeductinfluence = _opponentDeductinfluence; }
/// <summary>
/// 获得影响力
/// </summary>
public int getGainInfluence() { return gainInfluence; }
/// <summary>
/// 获得影响力
/// </summary>
public void setGainInfluence(int _gainInfluence) { gainInfluence = _gainInfluence; }
/// <summary>
/// 获得道具列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getGainItem() { return gainItem; }
/// <summary>
/// 获得道具列表
/// </summary>
public void addGainItem(NPCommon.NPCommon_ItemInfo _gainItem) { gainItem.Add(_gainItem); }
/// <summary>
/// 实力提升
/// </summary>
public long getAddPower() { return addPower; }
/// <summary>
/// 实力提升
/// </summary>
public void setAddPower(long _addPower) { addPower = _addPower; }
/// <summary>
/// 指定攻击道具id
/// </summary>
public long getSelectAttackItemId() { return selectAttackItemId; }
/// <summary>
/// 指定攻击道具id
/// </summary>
public void setSelectAttackItemId(long _selectAttackItemId) { selectAttackItemId = _selectAttackItemId; }
/// <summary>
/// 是否结算
/// </summary>
public bool getIsSettle() { return isSettle; }
/// <summary>
/// 是否结算
/// </summary>
public void setIsSettle(bool _isSettle) { isSettle = _isSettle; }


public int GetBufSize() {
	int _size = 33;
	_size += 2;
for(int _i = 0; _i < gainItem.Count; _i++) {
	_size += 4 + gainItem[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 35;
	_size += 2;
for(int _i = 0; _i < gainItem.Count; _i++) {
	_size += 4 + gainItem[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDefeatNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentDeductinfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainInfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gainItemCount = _buf.getShort();
	for(int _i = 0; _i < _gainItemCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _gainItem = new NPCommon.NPCommon_ItemInfo();
		int __gainItemCustLen = _buf.getInt();
	int __gainItemCurPos = _buf.getCurPos();
	_gainItem.ReadUnzipBuf(_buf, __gainItemCurPos + __gainItemCustLen);
	_buf.setPosition(__gainItemCurPos + __gainItemCustLen);

		gainItem.Add(_gainItem);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	selectAttackItemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSettle = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(hadDefeatNum);
	_buf.putInt(opponentHeroNum);
	_buf.putInt(opponentDeductinfluence);
	_buf.putInt(gainInfluence);
	_buf.putShort((short)gainItem.Count);
	for(int _i = 0; _i < gainItem.Count; _i++) { 
		_buf.putInt(gainItem[_i].GetBufSize());
	gainItem[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(addPower);
	_buf.putLong(selectAttackItemId);
	_buf.put(isSettle?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("hadDefeatNum").Append(":").Append(hadDefeatNum.ToString()).Append(", ");
	builder.Append("opponentHeroNum").Append(":").Append(opponentHeroNum.ToString()).Append(", ");
	builder.Append("opponentDeductinfluence").Append(":").Append(opponentDeductinfluence.ToString()).Append(", ");
	builder.Append("gainInfluence").Append(":").Append(gainInfluence.ToString()).Append(", ");
	builder.Append("gainItem").Append(":").Append(gainItem.ToString()).Append(", ");
	builder.Append("addPower").Append(":").Append(addPower.ToString()).Append(", ");
	builder.Append("selectAttackItemId").Append(":").Append(selectAttackItemId.ToString()).Append(", ");
	builder.Append("isSettle").Append(":").Append(isSettle.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

