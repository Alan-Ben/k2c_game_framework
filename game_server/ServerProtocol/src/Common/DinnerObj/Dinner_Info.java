package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 宴会详情
 **/
public class Dinner_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 简要信息 */
private Common.DinnerObj.Dinner_Idx idx;
/** 开始时间戳（秒） */
private int startTs;
/** 凭证类型 */
private Common.DinnerEnum.EDinnerPermitType permitType;
/** 凭证类型ID */
private long permitTypeId;
/** 赴宴玩家列表 */
private java.util.ArrayList<Common.DinnerObj.Dinner_Joiner> joinerList;


public Dinner_Info() {
	idx = new Common.DinnerObj.Dinner_Idx();
	startTs = 0;
	permitType = Common.DinnerEnum.EDinnerPermitType.values()[0];
	permitTypeId = (long)0;
	joinerList = new java.util.ArrayList<Common.DinnerObj.Dinner_Joiner>();
}

public Dinner_Info(
	 Common.DinnerObj.Dinner_Idx _idx
	, int _startTs
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
	, java.util.ArrayList<Common.DinnerObj.Dinner_Joiner> _joinerList
) {	idx = _idx;
	startTs = _startTs;
	permitType = _permitType;
	permitTypeId = _permitTypeId;
	joinerList = _joinerList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 简要信息 */
public Common.DinnerObj.Dinner_Idx getIdx() { return idx; }
/** 简要信息 */
public void setIdx(Common.DinnerObj.Dinner_Idx _idx) { idx = _idx; }
/** 开始时间戳（秒） */
public int getStartTs() { return startTs; }
/** 开始时间戳（秒） */
public void setStartTs(int _startTs) { startTs = _startTs; }
/** 凭证类型 */
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/** 凭证类型 */
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/** 凭证类型ID */
public long getPermitTypeId() { return permitTypeId; }
/** 凭证类型ID */
public void setPermitTypeId(long _permitTypeId) { permitTypeId = _permitTypeId; }
/** 赴宴玩家列表 */
public java.util.ArrayList<Common.DinnerObj.Dinner_Joiner> getJoinerList() { return joinerList; }
/** 赴宴玩家列表 */
public void addJoinerList(Common.DinnerObj.Dinner_Joiner _joinerList) { joinerList.add(_joinerList); }


public final int GetBufSize() {
	int _size = 73;
	_size += 2 + (joinerList.size() * 40);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 75;
	_size += 2 + (joinerList.size() * 40);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _idxCustLen = _buf.getInt();
	int _idxCurPos = _buf.position();
	idx.ReadUnzipBuf(_buf, _idxCurPos + _idxCustLen);
	_buf.position(_idxCurPos + _idxCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitType = Common.DinnerEnum.EDinnerPermitType.EDinnerPermitType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitTypeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinerListCount = _buf.getShort();
	for(int _i = 0; _i < _joinerListCount; _i++) { 
		Common.DinnerObj.Dinner_Joiner _joinerList = new Common.DinnerObj.Dinner_Joiner();
		if(_buf.remaining() <= 0) return;
	int __joinerListCustLen = _buf.getInt();
	int __joinerListCurPos = _buf.position();
	_joinerList.ReadUnzipBuf(_buf, __joinerListCurPos + __joinerListCustLen);
	_buf.position(__joinerListCurPos + __joinerListCustLen);

		joinerList.add(_joinerList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(idx.GetBufSize());
	idx.PutUnzipBuf(_buf);
	_buf.putInt(startTs);
	_buf.putInt(permitType.ordinal());

	_buf.putLong(permitTypeId);
	_buf.putShort((short)joinerList.size());
	for(int _i = 0; _i < joinerList.size(); _i++) { 
		_buf.putInt(joinerList.get(_i).GetBufSize());
	joinerList.get(_i).PutUnzipBuf(_buf);
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

