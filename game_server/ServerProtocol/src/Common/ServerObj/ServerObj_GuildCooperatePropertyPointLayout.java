package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作-属性据点布局数据
 **/
public class ServerObj_GuildCooperatePropertyPointLayout implements ALBasicProtocolPack._IALProtocolStructure {
/** 区域ID */
private long areaId;
/** 奖励据点索引 */
private int rewardPointIndex;
/** 奖励据点展示id */
private long posId;
/** 属性据点属性类型列表 */
private java.util.ArrayList<CommonEnum.ESpecAttrType> attrType;


public ServerObj_GuildCooperatePropertyPointLayout() {
	areaId = (long)0;
	rewardPointIndex = 0;
	posId = (long)0;
	attrType = new java.util.ArrayList<CommonEnum.ESpecAttrType>();
}

public ServerObj_GuildCooperatePropertyPointLayout(
	 long _areaId
	, int _rewardPointIndex
	, long _posId
	, java.util.ArrayList<CommonEnum.ESpecAttrType> _attrType
) {	areaId = _areaId;
	rewardPointIndex = _rewardPointIndex;
	posId = _posId;
	attrType = _attrType;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 区域ID */
public long getAreaId() { return areaId; }
/** 区域ID */
public void setAreaId(long _areaId) { areaId = _areaId; }
/** 奖励据点索引 */
public int getRewardPointIndex() { return rewardPointIndex; }
/** 奖励据点索引 */
public void setRewardPointIndex(int _rewardPointIndex) { rewardPointIndex = _rewardPointIndex; }
/** 奖励据点展示id */
public long getPosId() { return posId; }
/** 奖励据点展示id */
public void setPosId(long _posId) { posId = _posId; }
/** 属性据点属性类型列表 */
public java.util.ArrayList<CommonEnum.ESpecAttrType> getAttrType() { return attrType; }
/** 属性据点属性类型列表 */
public void addAttrType(CommonEnum.ESpecAttrType _attrType) { attrType.add(_attrType); }


public final int GetBufSize() {
	int _size = 20;
	_size += 2 + (attrType.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (attrType.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) areaId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardPointIndex = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _attrTypeCount = _buf.getShort();
	for(int _i = 0; _i < _attrTypeCount; _i++) { 
		CommonEnum.ESpecAttrType _attrType = CommonEnum.ESpecAttrType.values()[0];
		if(_buf.remaining() > 0) _attrType = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
		attrType.add(_attrType);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(areaId);
	_buf.putInt(rewardPointIndex);
	_buf.putLong(posId);
	_buf.putShort((short)attrType.size());
	for(int _i = 0; _i < attrType.size(); _i++) { 
		_buf.putInt(attrType.get(_i).ordinal());

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

