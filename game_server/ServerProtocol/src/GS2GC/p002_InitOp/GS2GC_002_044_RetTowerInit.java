package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_044_RetTowerInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 位置信息 */
private Common.TowerObj.Tower_PosInfo posInfo;
/** 已领取研究奖励列表 */
private java.util.ArrayList<Long> hadDrawResearchRewardList;
/** 已到达最高位置信息 */
private Common.TowerObj.Tower_PosInfo highestPosHadReach;
/** 已激活研究位置信息 */
private Common.TowerObj.Tower_PosInfo hadActiveResearchPos;
/** 建筑收益提升万分比 */
private int buildingProfitAddPer;


public GS2GC_002_044_RetTowerInit() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
	hadDrawResearchRewardList = new java.util.ArrayList<Long>();
	highestPosHadReach = new Common.TowerObj.Tower_PosInfo();
	hadActiveResearchPos = new Common.TowerObj.Tower_PosInfo();
	buildingProfitAddPer = 0;
}

public GS2GC_002_044_RetTowerInit(
	 Common.TowerObj.Tower_PosInfo _posInfo
	, java.util.ArrayList<Long> _hadDrawResearchRewardList
	, Common.TowerObj.Tower_PosInfo _highestPosHadReach
	, Common.TowerObj.Tower_PosInfo _hadActiveResearchPos
	, int _buildingProfitAddPer
) {	posInfo = _posInfo;
	hadDrawResearchRewardList = _hadDrawResearchRewardList;
	highestPosHadReach = _highestPosHadReach;
	hadActiveResearchPos = _hadActiveResearchPos;
	buildingProfitAddPer = _buildingProfitAddPer;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)44; }

/** 位置信息 */
public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
/** 位置信息 */
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }
/** 已领取研究奖励列表 */
public java.util.ArrayList<Long> getHadDrawResearchRewardList() { return hadDrawResearchRewardList; }
/** 已领取研究奖励列表 */
public void addHadDrawResearchRewardList(long _hadDrawResearchRewardList) { hadDrawResearchRewardList.add(_hadDrawResearchRewardList); }
/** 已到达最高位置信息 */
public Common.TowerObj.Tower_PosInfo getHighestPosHadReach() { return highestPosHadReach; }
/** 已到达最高位置信息 */
public void setHighestPosHadReach(Common.TowerObj.Tower_PosInfo _highestPosHadReach) { highestPosHadReach = _highestPosHadReach; }
/** 已激活研究位置信息 */
public Common.TowerObj.Tower_PosInfo getHadActiveResearchPos() { return hadActiveResearchPos; }
/** 已激活研究位置信息 */
public void setHadActiveResearchPos(Common.TowerObj.Tower_PosInfo _hadActiveResearchPos) { hadActiveResearchPos = _hadActiveResearchPos; }
/** 建筑收益提升万分比 */
public int getBuildingProfitAddPer() { return buildingProfitAddPer; }
/** 建筑收益提升万分比 */
public void setBuildingProfitAddPer(int _buildingProfitAddPer) { buildingProfitAddPer = _buildingProfitAddPer; }


public final int GetBufSize() {
	int _size = 52;
	_size += 2 + (hadDrawResearchRewardList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += 2 + (hadDrawResearchRewardList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.position();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.position(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawResearchRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawResearchRewardListCount; _i++) { 
		long _hadDrawResearchRewardList = (long)0;
		if(_buf.remaining() > 0) _hadDrawResearchRewardList = _buf.getLong();
		hadDrawResearchRewardList.add(_hadDrawResearchRewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _highestPosHadReachCustLen = _buf.getInt();
	int _highestPosHadReachCurPos = _buf.position();
	highestPosHadReach.ReadUnzipBuf(_buf, _highestPosHadReachCurPos + _highestPosHadReachCustLen);
	_buf.position(_highestPosHadReachCurPos + _highestPosHadReachCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _hadActiveResearchPosCustLen = _buf.getInt();
	int _hadActiveResearchPosCurPos = _buf.position();
	hadActiveResearchPos.ReadUnzipBuf(_buf, _hadActiveResearchPosCurPos + _hadActiveResearchPosCustLen);
	_buf.position(_hadActiveResearchPosCurPos + _hadActiveResearchPosCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingProfitAddPer = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)hadDrawResearchRewardList.size());
	for(int _i = 0; _i < hadDrawResearchRewardList.size(); _i++) { 
		_buf.putLong(hadDrawResearchRewardList.get(_i));
	}
	_buf.putInt(highestPosHadReach.GetBufSize());
	highestPosHadReach.PutUnzipBuf(_buf);
	_buf.putInt(hadActiveResearchPos.GetBufSize());
	hadActiveResearchPos.PutUnzipBuf(_buf);
	_buf.putInt(buildingProfitAddPer);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)44);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)44);
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

