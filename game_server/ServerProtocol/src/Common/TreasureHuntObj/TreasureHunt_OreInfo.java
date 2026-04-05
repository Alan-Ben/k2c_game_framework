package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石信息
 **/
public class TreasureHunt_OreInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石ID */
private long oreId;
/** 首次获得时间 ms */
private long firstGainTimeMs;
/** 最大记录 */
private int maxRecord;
/** 已领取奖励档位列表 */
private java.util.ArrayList<Integer> hadDrawRecordRewardList;
/** 数量信息 */
private Common.TreasureHuntObj.TreasureHunt_OreNumInfo numInfo;
/** 普通技能信息 */
private Common.TreasureHuntObj.TreasureHunt_OreSkillInfo normalSkillInfo;
/** 高级技能信息 */
private Common.TreasureHuntObj.TreasureHunt_OreSkillInfo advancedSkillInfo;


public TreasureHunt_OreInfo() {
	oreId = (long)0;
	firstGainTimeMs = (long)0;
	maxRecord = 0;
	hadDrawRecordRewardList = new java.util.ArrayList<Integer>();
	numInfo = new Common.TreasureHuntObj.TreasureHunt_OreNumInfo();
	normalSkillInfo = new Common.TreasureHuntObj.TreasureHunt_OreSkillInfo();
	advancedSkillInfo = new Common.TreasureHuntObj.TreasureHunt_OreSkillInfo();
}

public TreasureHunt_OreInfo(
	 long _oreId
	, long _firstGainTimeMs
	, int _maxRecord
	, java.util.ArrayList<Integer> _hadDrawRecordRewardList
	, Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo
	, Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _normalSkillInfo
	, Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _advancedSkillInfo
) {	oreId = _oreId;
	firstGainTimeMs = _firstGainTimeMs;
	maxRecord = _maxRecord;
	hadDrawRecordRewardList = _hadDrawRecordRewardList;
	numInfo = _numInfo;
	normalSkillInfo = _normalSkillInfo;
	advancedSkillInfo = _advancedSkillInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 矿石ID */
public long getOreId() { return oreId; }
/** 矿石ID */
public void setOreId(long _oreId) { oreId = _oreId; }
/** 首次获得时间 ms */
public long getFirstGainTimeMs() { return firstGainTimeMs; }
/** 首次获得时间 ms */
public void setFirstGainTimeMs(long _firstGainTimeMs) { firstGainTimeMs = _firstGainTimeMs; }
/** 最大记录 */
public int getMaxRecord() { return maxRecord; }
/** 最大记录 */
public void setMaxRecord(int _maxRecord) { maxRecord = _maxRecord; }
/** 已领取奖励档位列表 */
public java.util.ArrayList<Integer> getHadDrawRecordRewardList() { return hadDrawRecordRewardList; }
/** 已领取奖励档位列表 */
public void addHadDrawRecordRewardList(int _hadDrawRecordRewardList) { hadDrawRecordRewardList.add(_hadDrawRecordRewardList); }
/** 数量信息 */
public Common.TreasureHuntObj.TreasureHunt_OreNumInfo getNumInfo() { return numInfo; }
/** 数量信息 */
public void setNumInfo(Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo) { numInfo = _numInfo; }
/** 普通技能信息 */
public Common.TreasureHuntObj.TreasureHunt_OreSkillInfo getNormalSkillInfo() { return normalSkillInfo; }
/** 普通技能信息 */
public void setNormalSkillInfo(Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _normalSkillInfo) { normalSkillInfo = _normalSkillInfo; }
/** 高级技能信息 */
public Common.TreasureHuntObj.TreasureHunt_OreSkillInfo getAdvancedSkillInfo() { return advancedSkillInfo; }
/** 高级技能信息 */
public void setAdvancedSkillInfo(Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _advancedSkillInfo) { advancedSkillInfo = _advancedSkillInfo; }


public final int GetBufSize() {
	int _size = 60;
	_size += 2 + (hadDrawRecordRewardList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 62;
	_size += 2 + (hadDrawRecordRewardList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) firstGainTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxRecord = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawRecordRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRecordRewardListCount; _i++) { 
		int _hadDrawRecordRewardList = 0;
		if(_buf.remaining() > 0) _hadDrawRecordRewardList = _buf.getInt();
		hadDrawRecordRewardList.add(_hadDrawRecordRewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _numInfoCustLen = _buf.getInt();
	int _numInfoCurPos = _buf.position();
	numInfo.ReadUnzipBuf(_buf, _numInfoCurPos + _numInfoCustLen);
	_buf.position(_numInfoCurPos + _numInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _normalSkillInfoCustLen = _buf.getInt();
	int _normalSkillInfoCurPos = _buf.position();
	normalSkillInfo.ReadUnzipBuf(_buf, _normalSkillInfoCurPos + _normalSkillInfoCustLen);
	_buf.position(_normalSkillInfoCurPos + _normalSkillInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _advancedSkillInfoCustLen = _buf.getInt();
	int _advancedSkillInfoCurPos = _buf.position();
	advancedSkillInfo.ReadUnzipBuf(_buf, _advancedSkillInfoCurPos + _advancedSkillInfoCustLen);
	_buf.position(_advancedSkillInfoCurPos + _advancedSkillInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oreId);
	_buf.putLong(firstGainTimeMs);
	_buf.putInt(maxRecord);
	_buf.putShort((short)hadDrawRecordRewardList.size());
	for(int _i = 0; _i < hadDrawRecordRewardList.size(); _i++) { 
		_buf.putInt(hadDrawRecordRewardList.get(_i));
	}
	_buf.putInt(numInfo.GetBufSize());
	numInfo.PutUnzipBuf(_buf);
	_buf.putInt(normalSkillInfo.GetBufSize());
	normalSkillInfo.PutUnzipBuf(_buf);
	_buf.putInt(advancedSkillInfo.GetBufSize());
	advancedSkillInfo.PutUnzipBuf(_buf);
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

