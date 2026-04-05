package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 联盟宝箱奖励物品列表
 **/
public class GS2GC_042_056_OnGuildBoxRewardShow implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱类型 */
private Common.GuildEnum.EGuildBoxType boxType;
/** 宝箱奖励列表数据 */
private java.util.ArrayList<Common.GuildObj.Guild_BoxReward> rewardList;


public GS2GC_042_056_OnGuildBoxRewardShow() {
	boxType = Common.GuildEnum.EGuildBoxType.values()[0];
	rewardList = new java.util.ArrayList<Common.GuildObj.Guild_BoxReward>();
}

public GS2GC_042_056_OnGuildBoxRewardShow(
	 Common.GuildEnum.EGuildBoxType _boxType
	, java.util.ArrayList<Common.GuildObj.Guild_BoxReward> _rewardList
) {	boxType = _boxType;
	rewardList = _rewardList;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)56; }

/** 宝箱类型 */
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/** 宝箱类型 */
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
/** 宝箱奖励列表数据 */
public java.util.ArrayList<Common.GuildObj.Guild_BoxReward> getRewardList() { return rewardList; }
/** 宝箱奖励列表数据 */
public void addRewardList(Common.GuildObj.Guild_BoxReward _rewardList) { rewardList.add(_rewardList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxType = Common.GuildEnum.EGuildBoxType.EGuildBoxType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		Common.GuildObj.Guild_BoxReward _rewardList = new Common.GuildObj.Guild_BoxReward();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxType.ordinal());

	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)56);
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

