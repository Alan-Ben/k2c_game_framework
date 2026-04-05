using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.RankGiftPackObj
{

/// <summary>
/// 冲榜礼包信息
/// </summary>
public class RankGiftPack_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已购买次数
/// </summary>
private int hadBuyTimes;
/// <summary>
/// 购买限制次数
/// </summary>
private int buyLimitTimes;
/// <summary>
/// 结束时间毫秒
/// </summary>
private long endTimeMs;
/// <summary>
/// 礼包名称
/// </summary>
private string name;
/// <summary>
/// 消耗道具
/// </summary>
private NPCommon.NPCommon_ItemInfo consume;
/// <summary>
/// 原价消耗道具
/// </summary>
private NPCommon.NPCommon_ItemInfo oriConsume;
/// <summary>
/// 奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> rewardList;
/// <summary>
/// UI资源路径ID
/// </summary>
private long uiResPathId;
/// <summary>
/// 折扣
/// </summary>
private int sale;
/// <summary>
/// 数据ID
/// </summary>
private long dbId;


public RankGiftPack_Info() {
	hadBuyTimes = 0;
	buyLimitTimes = 0;
	endTimeMs = (long)0;
	name = "";
	consume = new NPCommon.NPCommon_ItemInfo();
	oriConsume = new NPCommon.NPCommon_ItemInfo();
	rewardList = new List<NPCommon.NPCommon_ItemInfo>();
	uiResPathId = (long)0;
	sale = 0;
	dbId = (long)0;
}

public RankGiftPack_Info(
	int _hadBuyTimes
	, int _buyLimitTimes
	, long _endTimeMs
	, string _name
	, NPCommon.NPCommon_ItemInfo _consume
	, NPCommon.NPCommon_ItemInfo _oriConsume
	, List<NPCommon.NPCommon_ItemInfo> _rewardList
	, long _uiResPathId
	, int _sale
	, long _dbId
) {	hadBuyTimes = _hadBuyTimes;
	buyLimitTimes = _buyLimitTimes;
	endTimeMs = _endTimeMs;
	name = _name;
	consume = _consume;
	oriConsume = _oriConsume;
	rewardList = _rewardList;
	uiResPathId = _uiResPathId;
	sale = _sale;
	dbId = _dbId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已购买次数
/// </summary>
public int getHadBuyTimes() { return hadBuyTimes; }
/// <summary>
/// 已购买次数
/// </summary>
public void setHadBuyTimes(int _hadBuyTimes) { hadBuyTimes = _hadBuyTimes; }
/// <summary>
/// 购买限制次数
/// </summary>
public int getBuyLimitTimes() { return buyLimitTimes; }
/// <summary>
/// 购买限制次数
/// </summary>
public void setBuyLimitTimes(int _buyLimitTimes) { buyLimitTimes = _buyLimitTimes; }
/// <summary>
/// 结束时间毫秒
/// </summary>
public long getEndTimeMs() { return endTimeMs; }
/// <summary>
/// 结束时间毫秒
/// </summary>
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/// <summary>
/// 礼包名称
/// </summary>
public string getName() { return name; }
/// <summary>
/// 礼包名称
/// </summary>
public void setName(string _name) { name = _name; }
/// <summary>
/// 消耗道具
/// </summary>
public NPCommon.NPCommon_ItemInfo getConsume() { return consume; }
/// <summary>
/// 消耗道具
/// </summary>
public void setConsume(NPCommon.NPCommon_ItemInfo _consume) { consume = _consume; }
/// <summary>
/// 原价消耗道具
/// </summary>
public NPCommon.NPCommon_ItemInfo getOriConsume() { return oriConsume; }
/// <summary>
/// 原价消耗道具
/// </summary>
public void setOriConsume(NPCommon.NPCommon_ItemInfo _oriConsume) { oriConsume = _oriConsume; }
/// <summary>
/// 奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/// <summary>
/// 奖励列表
/// </summary>
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.Add(_rewardList); }
/// <summary>
/// UI资源路径ID
/// </summary>
public long getUiResPathId() { return uiResPathId; }
/// <summary>
/// UI资源路径ID
/// </summary>
public void setUiResPathId(long _uiResPathId) { uiResPathId = _uiResPathId; }
/// <summary>
/// 折扣
/// </summary>
public int getSale() { return sale; }
/// <summary>
/// 折扣
/// </summary>
public void setSale(int _sale) { sale = _sale; }
/// <summary>
/// 数据ID
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据ID
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }


public int GetBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + consume.GetBufSize();
	_size += 4 + oriConsume.GetBufSize();
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + consume.GetBufSize();
	_size += 4 + oriConsume.GetBufSize();
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadBuyTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buyLimitTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _consumeCustLen = _buf.getInt();
	int _consumeCurPos = _buf.getCurPos();
	consume.ReadUnzipBuf(_buf, _consumeCurPos + _consumeCustLen);
	_buf.setPosition(_consumeCurPos + _consumeCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _oriConsumeCustLen = _buf.getInt();
	int _oriConsumeCurPos = _buf.getCurPos();
	oriConsume.ReadUnzipBuf(_buf, _oriConsumeCurPos + _oriConsumeCustLen);
	_buf.setPosition(_oriConsumeCurPos + _oriConsumeCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.getCurPos();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.setPosition(__rewardListCurPos + __rewardListCustLen);

		rewardList.Add(_rewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uiResPathId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sale = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(hadBuyTimes);
	_buf.putInt(buyLimitTimes);
	_buf.putLong(endTimeMs);
	_buf.putString(name);
	_buf.putInt(consume.GetBufSize());
	consume.PutUnzipBuf(_buf);
	_buf.putInt(oriConsume.GetBufSize());
	oriConsume.PutUnzipBuf(_buf);
	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(uiResPathId);
	_buf.putInt(sale);
	_buf.putLong(dbId);
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
	builder.Append("hadBuyTimes").Append(":").Append(hadBuyTimes.ToString()).Append(", ");
	builder.Append("buyLimitTimes").Append(":").Append(buyLimitTimes.ToString()).Append(", ");
	builder.Append("endTimeMs").Append(":").Append(endTimeMs.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("consume").Append(":").Append(consume == null ? "null" : consume.ToString()).Append(", ");
	builder.Append("oriConsume").Append(":").Append(oriConsume == null ? "null" : oriConsume.ToString()).Append(", ");
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("uiResPathId").Append(":").Append(uiResPathId.ToString()).Append(", ");
	builder.Append("sale").Append(":").Append(sale.ToString()).Append(", ");
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

