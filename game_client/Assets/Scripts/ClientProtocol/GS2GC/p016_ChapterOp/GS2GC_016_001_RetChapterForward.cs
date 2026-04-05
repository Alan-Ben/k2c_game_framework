using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p016_ChapterOp
{

public class GS2GC_016_001_RetChapterForward : ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
private int point;
private int coefficient;
/// <summary>
/// 奖励大臣经验
/// </summary>
private long rewardExp;
/// <summary>
/// 奖励玩家经验
/// </summary>
private long rewardPlayerExp;
/// <summary>
/// 触发事件id
/// </summary>
private long eventId;
/// <summary>
/// 物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;
/// <summary>
/// 花费金币数量
/// </summary>
private long costGoldNum;


public GS2GC_016_001_RetChapterForward() {
	chapterId = (long)0;
	point = 0;
	coefficient = 0;
	rewardExp = (long)0;
	rewardPlayerExp = (long)0;
	eventId = (long)0;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
	costGoldNum = (long)0;
}

public GS2GC_016_001_RetChapterForward(
	long _chapterId
	, int _point
	, int _coefficient
	, long _rewardExp
	, long _rewardPlayerExp
	, long _eventId
	, List<NPCommon.NPCommon_ItemInfo> _itemList
	, long _costGoldNum
) {	chapterId = _chapterId;
	point = _point;
	coefficient = _coefficient;
	rewardExp = _rewardExp;
	rewardPlayerExp = _rewardPlayerExp;
	eventId = _eventId;
	itemList = _itemList;
	costGoldNum = _costGoldNum;
}

public byte getMainOrder() { return (byte)16; }

public byte getSubOrder() { return (byte)1; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
public int getPoint() { return point; }
public void setPoint(int _point) { point = _point; }
public int getCoefficient() { return coefficient; }
public void setCoefficient(int _coefficient) { coefficient = _coefficient; }
/// <summary>
/// 奖励大臣经验
/// </summary>
public long getRewardExp() { return rewardExp; }
/// <summary>
/// 奖励大臣经验
/// </summary>
public void setRewardExp(long _rewardExp) { rewardExp = _rewardExp; }
/// <summary>
/// 奖励玩家经验
/// </summary>
public long getRewardPlayerExp() { return rewardPlayerExp; }
/// <summary>
/// 奖励玩家经验
/// </summary>
public void setRewardPlayerExp(long _rewardPlayerExp) { rewardPlayerExp = _rewardPlayerExp; }
/// <summary>
/// 触发事件id
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 触发事件id
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 物品列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }
/// <summary>
/// 花费金币数量
/// </summary>
public long getCostGoldNum() { return costGoldNum; }
/// <summary>
/// 花费金币数量
/// </summary>
public void setCostGoldNum(long _costGoldNum) { costGoldNum = _costGoldNum; }


public int GetBufSize() {
	int _size = 48;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	point = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	coefficient = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardPlayerExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costGoldNum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(point);
	_buf.putInt(coefficient);
	_buf.putLong(rewardExp);
	_buf.putLong(rewardPlayerExp);
	_buf.putLong(eventId);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(costGoldNum);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)1);
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
	builder.Append("chapterId").Append(":").Append(chapterId.ToString()).Append(", ");
	builder.Append("point").Append(":").Append(point.ToString()).Append(", ");
	builder.Append("coefficient").Append(":").Append(coefficient.ToString()).Append(", ");
	builder.Append("rewardExp").Append(":").Append(rewardExp.ToString()).Append(", ");
	builder.Append("rewardPlayerExp").Append(":").Append(rewardPlayerExp.ToString()).Append(", ");
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("costGoldNum").Append(":").Append(costGoldNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

