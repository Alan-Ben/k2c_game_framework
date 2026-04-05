package Common.RankGiftPackObj;

import java.nio.ByteBuffer;
/*********
 * 冲榜礼包信息
 **/
public class RankGiftPack_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 已购买次数 */
private int hadBuyTimes;
/** 购买限制次数 */
private int buyLimitTimes;
/** 结束时间毫秒 */
private long endTimeMs;
/** 礼包名称 */
private String name;
/** 消耗道具 */
private NPCommon.NPCommon_ItemInfo consume;
/** 原价消耗道具 */
private NPCommon.NPCommon_ItemInfo oriConsume;
/** 奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> rewardList;
/** UI资源路径ID */
private long uiResPathId;
/** 折扣 */
private int sale;
/** 数据ID */
private long dbId;


public RankGiftPack_Info() {
	hadBuyTimes = 0;
	buyLimitTimes = 0;
	endTimeMs = (long)0;
	name = "";
	consume = new NPCommon.NPCommon_ItemInfo();
	oriConsume = new NPCommon.NPCommon_ItemInfo();
	rewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	uiResPathId = (long)0;
	sale = 0;
	dbId = (long)0;
}

public RankGiftPack_Info(
	 int _hadBuyTimes
	, int _buyLimitTimes
	, long _endTimeMs
	, String _name
	, NPCommon.NPCommon_ItemInfo _consume
	, NPCommon.NPCommon_ItemInfo _oriConsume
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _rewardList
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已购买次数 */
public int getHadBuyTimes() { return hadBuyTimes; }
/** 已购买次数 */
public void setHadBuyTimes(int _hadBuyTimes) { hadBuyTimes = _hadBuyTimes; }
/** 购买限制次数 */
public int getBuyLimitTimes() { return buyLimitTimes; }
/** 购买限制次数 */
public void setBuyLimitTimes(int _buyLimitTimes) { buyLimitTimes = _buyLimitTimes; }
/** 结束时间毫秒 */
public long getEndTimeMs() { return endTimeMs; }
/** 结束时间毫秒 */
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/** 礼包名称 */
public String getName() { return name; }
/** 礼包名称 */
public void setName(String _name) { name = _name; }
/** 消耗道具 */
public NPCommon.NPCommon_ItemInfo getConsume() { return consume; }
/** 消耗道具 */
public void setConsume(NPCommon.NPCommon_ItemInfo _consume) { consume = _consume; }
/** 原价消耗道具 */
public NPCommon.NPCommon_ItemInfo getOriConsume() { return oriConsume; }
/** 原价消耗道具 */
public void setOriConsume(NPCommon.NPCommon_ItemInfo _oriConsume) { oriConsume = _oriConsume; }
/** 奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/** 奖励列表 */
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.add(_rewardList); }
/** UI资源路径ID */
public long getUiResPathId() { return uiResPathId; }
/** UI资源路径ID */
public void setUiResPathId(long _uiResPathId) { uiResPathId = _uiResPathId; }
/** 折扣 */
public int getSale() { return sale; }
/** 折扣 */
public void setSale(int _sale) { sale = _sale; }
/** 数据ID */
public long getDbId() { return dbId; }
/** 数据ID */
public void setDbId(long _dbId) { dbId = _dbId; }


public final int GetBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + consume.GetBufSize();
	_size += 4 + oriConsume.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 4 + consume.GetBufSize();
	_size += 4 + oriConsume.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBuyTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buyLimitTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _consumeCustLen = _buf.getInt();
	int _consumeCurPos = _buf.position();
	consume.ReadUnzipBuf(_buf, _consumeCurPos + _consumeCustLen);
	_buf.position(_consumeCurPos + _consumeCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _oriConsumeCustLen = _buf.getInt();
	int _oriConsumeCurPos = _buf.position();
	oriConsume.ReadUnzipBuf(_buf, _oriConsumeCurPos + _oriConsumeCustLen);
	_buf.position(_oriConsumeCurPos + _oriConsumeCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uiResPathId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sale = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(hadBuyTimes);
	_buf.putInt(buyLimitTimes);
	_buf.putLong(endTimeMs);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putInt(consume.GetBufSize());
	consume.PutUnzipBuf(_buf);
	_buf.putInt(oriConsume.GetBufSize());
	oriConsume.PutUnzipBuf(_buf);
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(uiResPathId);
	_buf.putInt(sale);
	_buf.putLong(dbId);
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

