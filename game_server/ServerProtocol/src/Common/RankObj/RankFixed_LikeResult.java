package Common.RankObj;

import java.nio.ByteBuffer;
/*********
 * 常驻排行榜点赞结果
 **/
public class RankFixed_LikeResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 常驻排行榜id */
private long rankFixedId;
/** 基础数据 */
private Common.RankObj.Rank_BaseItem info;
/** 点赞积分 */
private long likeScore;
/** 奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> rewardItemList;


public RankFixed_LikeResult() {
	rankFixedId = (long)0;
	info = new Common.RankObj.Rank_BaseItem();
	likeScore = (long)0;
	rewardItemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public RankFixed_LikeResult(
	 long _rankFixedId
	, Common.RankObj.Rank_BaseItem _info
	, long _likeScore
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _rewardItemList
) {	rankFixedId = _rankFixedId;
	info = _info;
	likeScore = _likeScore;
	rewardItemList = _rewardItemList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 常驻排行榜id */
public long getRankFixedId() { return rankFixedId; }
/** 常驻排行榜id */
public void setRankFixedId(long _rankFixedId) { rankFixedId = _rankFixedId; }
/** 基础数据 */
public Common.RankObj.Rank_BaseItem getInfo() { return info; }
/** 基础数据 */
public void setInfo(Common.RankObj.Rank_BaseItem _info) { info = _info; }
/** 点赞积分 */
public long getLikeScore() { return likeScore; }
/** 点赞积分 */
public void setLikeScore(long _likeScore) { likeScore = _likeScore; }
/** 奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getRewardItemList() { return rewardItemList; }
/** 奖励列表 */
public void addRewardItemList(NPCommon.NPCommon_ItemInfo _rewardItemList) { rewardItemList.add(_rewardItemList); }


public final int GetBufSize() {
	int _size = 48;
	_size += 2;
	for(int _i = 0; _i < rewardItemList.size(); _i++) {
	_size += 4 + rewardItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;
	_size += 2;
	for(int _i = 0; _i < rewardItemList.size(); _i++) {
	_size += 4 + rewardItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankFixedId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) likeScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardItemListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardItemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardItemListCustLen = _buf.getInt();
	int __rewardItemListCurPos = _buf.position();
	_rewardItemList.ReadUnzipBuf(_buf, __rewardItemListCurPos + __rewardItemListCustLen);
	_buf.position(__rewardItemListCurPos + __rewardItemListCustLen);

		rewardItemList.add(_rewardItemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rankFixedId);
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putLong(likeScore);
	_buf.putShort((short)rewardItemList.size());
	for(int _i = 0; _i < rewardItemList.size(); _i++) { 
		_buf.putInt(rewardItemList.get(_i).GetBufSize());
	rewardItemList.get(_i).PutUnzipBuf(_buf);
	}
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

