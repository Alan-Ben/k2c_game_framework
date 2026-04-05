using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.RankObj
{

/// <summary>
/// 常驻排行榜点赞结果
/// </summary>
public class RankFixed_LikeResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 常驻排行榜id
/// </summary>
private long rankFixedId;
/// <summary>
/// 基础数据
/// </summary>
private Common.RankObj.Rank_BaseItem info;
/// <summary>
/// 点赞积分
/// </summary>
private long likeScore;
/// <summary>
/// 奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> rewardItemList;


public RankFixed_LikeResult() {
	rankFixedId = (long)0;
	info = new Common.RankObj.Rank_BaseItem();
	likeScore = (long)0;
	rewardItemList = new List<NPCommon.NPCommon_ItemInfo>();
}

public RankFixed_LikeResult(
	long _rankFixedId
	, Common.RankObj.Rank_BaseItem _info
	, long _likeScore
	, List<NPCommon.NPCommon_ItemInfo> _rewardItemList
) {	rankFixedId = _rankFixedId;
	info = _info;
	likeScore = _likeScore;
	rewardItemList = _rewardItemList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 常驻排行榜id
/// </summary>
public long getRankFixedId() { return rankFixedId; }
/// <summary>
/// 常驻排行榜id
/// </summary>
public void setRankFixedId(long _rankFixedId) { rankFixedId = _rankFixedId; }
/// <summary>
/// 基础数据
/// </summary>
public Common.RankObj.Rank_BaseItem getInfo() { return info; }
/// <summary>
/// 基础数据
/// </summary>
public void setInfo(Common.RankObj.Rank_BaseItem _info) { info = _info; }
/// <summary>
/// 点赞积分
/// </summary>
public long getLikeScore() { return likeScore; }
/// <summary>
/// 点赞积分
/// </summary>
public void setLikeScore(long _likeScore) { likeScore = _likeScore; }
/// <summary>
/// 奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getRewardItemList() { return rewardItemList; }
/// <summary>
/// 奖励列表
/// </summary>
public void addRewardItemList(NPCommon.NPCommon_ItemInfo _rewardItemList) { rewardItemList.Add(_rewardItemList); }


public int GetBufSize() {
	int _size = 48;
	_size += 2;
for(int _i = 0; _i < rewardItemList.Count; _i++) {
	_size += 4 + rewardItemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;
	_size += 2;
for(int _i = 0; _i < rewardItemList.Count; _i++) {
	_size += 4 + rewardItemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rankFixedId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	likeScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardItemListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardItemList = new NPCommon.NPCommon_ItemInfo();
		int __rewardItemListCustLen = _buf.getInt();
	int __rewardItemListCurPos = _buf.getCurPos();
	_rewardItemList.ReadUnzipBuf(_buf, __rewardItemListCurPos + __rewardItemListCustLen);
	_buf.setPosition(__rewardItemListCurPos + __rewardItemListCustLen);

		rewardItemList.Add(_rewardItemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rankFixedId);
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putLong(likeScore);
	_buf.putShort((short)rewardItemList.Count);
	for(int _i = 0; _i < rewardItemList.Count; _i++) { 
		_buf.putInt(rewardItemList[_i].GetBufSize());
	rewardItemList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("rankFixedId").Append(":").Append(rankFixedId.ToString()).Append(", ");
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("likeScore").Append(":").Append(likeScore.ToString()).Append(", ");
	builder.Append("rewardItemList").Append(":").Append(rewardItemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

