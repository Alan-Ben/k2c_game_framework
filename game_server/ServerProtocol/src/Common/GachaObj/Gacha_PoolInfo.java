package Common.GachaObj;

import java.nio.ByteBuffer;
/*********
 * 抽卡卡池信息
 **/
public class Gacha_PoolInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long poolId;
/** 保底信息列表 */
private java.util.ArrayList<Common.GachaObj.Gacha_GuaranteeInfo> guaranteeList;
/** 抽卡累计奖励次数 */
private int cumulativeRewardTimes;


public Gacha_PoolInfo() {
	poolId = (long)0;
	guaranteeList = new java.util.ArrayList<Common.GachaObj.Gacha_GuaranteeInfo>();
	cumulativeRewardTimes = 0;
}

public Gacha_PoolInfo(
	 long _poolId
	, java.util.ArrayList<Common.GachaObj.Gacha_GuaranteeInfo> _guaranteeList
	, int _cumulativeRewardTimes
) {	poolId = _poolId;
	guaranteeList = _guaranteeList;
	cumulativeRewardTimes = _cumulativeRewardTimes;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getPoolId() { return poolId; }
public void setPoolId(long _poolId) { poolId = _poolId; }
/** 保底信息列表 */
public java.util.ArrayList<Common.GachaObj.Gacha_GuaranteeInfo> getGuaranteeList() { return guaranteeList; }
/** 保底信息列表 */
public void addGuaranteeList(Common.GachaObj.Gacha_GuaranteeInfo _guaranteeList) { guaranteeList.add(_guaranteeList); }
/** 抽卡累计奖励次数 */
public int getCumulativeRewardTimes() { return cumulativeRewardTimes; }
/** 抽卡累计奖励次数 */
public void setCumulativeRewardTimes(int _cumulativeRewardTimes) { cumulativeRewardTimes = _cumulativeRewardTimes; }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (guaranteeList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (guaranteeList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) poolId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _guaranteeListCount = _buf.getShort();
	for(int _i = 0; _i < _guaranteeListCount; _i++) { 
		Common.GachaObj.Gacha_GuaranteeInfo _guaranteeList = new Common.GachaObj.Gacha_GuaranteeInfo();
		if(_buf.remaining() <= 0) return;
	int __guaranteeListCustLen = _buf.getInt();
	int __guaranteeListCurPos = _buf.position();
	_guaranteeList.ReadUnzipBuf(_buf, __guaranteeListCurPos + __guaranteeListCustLen);
	_buf.position(__guaranteeListCurPos + __guaranteeListCustLen);

		guaranteeList.add(_guaranteeList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cumulativeRewardTimes = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(poolId);
	_buf.putShort((short)guaranteeList.size());
	for(int _i = 0; _i < guaranteeList.size(); _i++) { 
		_buf.putInt(guaranteeList.get(_i).GetBufSize());
	guaranteeList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(cumulativeRewardTimes);
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

