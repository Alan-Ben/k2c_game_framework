using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-矿石信息
/// </summary>
public class TreasureHunt_OreInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 首次获得时间 ms
/// </summary>
private long firstGainTimeMs;
/// <summary>
/// 最大记录
/// </summary>
private int maxRecord;
/// <summary>
/// 已领取奖励档位列表
/// </summary>
private List<int> hadDrawRecordRewardList;
/// <summary>
/// 数量信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_OreNumInfo numInfo;
/// <summary>
/// 普通技能信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_OreSkillInfo normalSkillInfo;
/// <summary>
/// 高级技能信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_OreSkillInfo advancedSkillInfo;


public TreasureHunt_OreInfo() {
	oreId = (long)0;
	firstGainTimeMs = (long)0;
	maxRecord = 0;
	hadDrawRecordRewardList = new List<int>();
	numInfo = new Common.TreasureHuntObj.TreasureHunt_OreNumInfo();
	normalSkillInfo = new Common.TreasureHuntObj.TreasureHunt_OreSkillInfo();
	advancedSkillInfo = new Common.TreasureHuntObj.TreasureHunt_OreSkillInfo();
}

public TreasureHunt_OreInfo(
	long _oreId
	, long _firstGainTimeMs
	, int _maxRecord
	, List<int> _hadDrawRecordRewardList
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// 首次获得时间 ms
/// </summary>
public long getFirstGainTimeMs() { return firstGainTimeMs; }
/// <summary>
/// 首次获得时间 ms
/// </summary>
public void setFirstGainTimeMs(long _firstGainTimeMs) { firstGainTimeMs = _firstGainTimeMs; }
/// <summary>
/// 最大记录
/// </summary>
public int getMaxRecord() { return maxRecord; }
/// <summary>
/// 最大记录
/// </summary>
public void setMaxRecord(int _maxRecord) { maxRecord = _maxRecord; }
/// <summary>
/// 已领取奖励档位列表
/// </summary>
public List<int> getHadDrawRecordRewardList() { return hadDrawRecordRewardList; }
/// <summary>
/// 已领取奖励档位列表
/// </summary>
public void addHadDrawRecordRewardList(int _hadDrawRecordRewardList) { hadDrawRecordRewardList.Add(_hadDrawRecordRewardList); }
/// <summary>
/// 数量信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_OreNumInfo getNumInfo() { return numInfo; }
/// <summary>
/// 数量信息
/// </summary>
public void setNumInfo(Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo) { numInfo = _numInfo; }
/// <summary>
/// 普通技能信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_OreSkillInfo getNormalSkillInfo() { return normalSkillInfo; }
/// <summary>
/// 普通技能信息
/// </summary>
public void setNormalSkillInfo(Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _normalSkillInfo) { normalSkillInfo = _normalSkillInfo; }
/// <summary>
/// 高级技能信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_OreSkillInfo getAdvancedSkillInfo() { return advancedSkillInfo; }
/// <summary>
/// 高级技能信息
/// </summary>
public void setAdvancedSkillInfo(Common.TreasureHuntObj.TreasureHunt_OreSkillInfo _advancedSkillInfo) { advancedSkillInfo = _advancedSkillInfo; }


public int GetBufSize() {
	int _size = 60;
	_size += 2 + (hadDrawRecordRewardList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 62;
	_size += 2 + (hadDrawRecordRewardList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	firstGainTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxRecord = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawRecordRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRecordRewardListCount; _i++) { 
		int _hadDrawRecordRewardList = 0;
		_hadDrawRecordRewardList = _buf.getInt();
		hadDrawRecordRewardList.Add(_hadDrawRecordRewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _numInfoCustLen = _buf.getInt();
	int _numInfoCurPos = _buf.getCurPos();
	numInfo.ReadUnzipBuf(_buf, _numInfoCurPos + _numInfoCustLen);
	_buf.setPosition(_numInfoCurPos + _numInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _normalSkillInfoCustLen = _buf.getInt();
	int _normalSkillInfoCurPos = _buf.getCurPos();
	normalSkillInfo.ReadUnzipBuf(_buf, _normalSkillInfoCurPos + _normalSkillInfoCustLen);
	_buf.setPosition(_normalSkillInfoCurPos + _normalSkillInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _advancedSkillInfoCustLen = _buf.getInt();
	int _advancedSkillInfoCurPos = _buf.getCurPos();
	advancedSkillInfo.ReadUnzipBuf(_buf, _advancedSkillInfoCurPos + _advancedSkillInfoCustLen);
	_buf.setPosition(_advancedSkillInfoCurPos + _advancedSkillInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.putLong(firstGainTimeMs);
	_buf.putInt(maxRecord);
	_buf.putShort((short)hadDrawRecordRewardList.Count);
	for(int _i = 0; _i < hadDrawRecordRewardList.Count; _i++) { 
		_buf.putInt(hadDrawRecordRewardList[_i]);
	}
	_buf.putInt(numInfo.GetBufSize());
	numInfo.PutUnzipBuf(_buf);
	_buf.putInt(normalSkillInfo.GetBufSize());
	normalSkillInfo.PutUnzipBuf(_buf);
	_buf.putInt(advancedSkillInfo.GetBufSize());
	advancedSkillInfo.PutUnzipBuf(_buf);
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
	builder.Append("oreId").Append(":").Append(oreId.ToString()).Append(", ");
	builder.Append("firstGainTimeMs").Append(":").Append(firstGainTimeMs.ToString()).Append(", ");
	builder.Append("maxRecord").Append(":").Append(maxRecord.ToString()).Append(", ");
	builder.Append("hadDrawRecordRewardList").Append(":").Append(hadDrawRecordRewardList.ToString()).Append(", ");
	builder.Append("numInfo").Append(":").Append(numInfo == null ? "null" : numInfo.ToString()).Append(", ");
	builder.Append("normalSkillInfo").Append(":").Append(normalSkillInfo == null ? "null" : normalSkillInfo.ToString()).Append(", ");
	builder.Append("advancedSkillInfo").Append(":").Append(advancedSkillInfo == null ? "null" : advancedSkillInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

