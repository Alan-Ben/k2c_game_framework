package Common.EventObj;

import java.nio.ByteBuffer;
/*********
 * 事件完成信息
 **/
public class CommonEvent_DoneInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> rewardList;
/** 额外信息 */
private byte[] extraInfo;


public CommonEvent_DoneInfo() {
	rewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	extraInfo = null;
}

public CommonEvent_DoneInfo(
	 java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _rewardList
	, byte[] _extraInfo
) {	rewardList = _rewardList;
	extraInfo = _extraInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/** 奖励列表 */
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.add(_rewardList); }
/** 额外信息 */
public byte[] getExtraInfo() { return extraInfo; }
public java.nio.ByteBuffer get_buffer_ExtraInfo() { if(null == extraInfo)return null; else return ByteBuffer.wrap(extraInfo); }

/** 额外信息 */
public void setExtraInfo(byte[] _extraInfo) { extraInfo = _extraInfo; }
public void setExtraInfo(java.nio.ByteBuffer _extraInfo) 
{
	if(null == _extraInfo){return;}
	int _oldPos = _extraInfo.position();
	int _bufLength = _extraInfo.remaining();
	extraInfo = new byte[_bufLength];
	_extraInfo.get(extraInfo);
	_extraInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}

	_size += 4 + (extraInfo == null ? 0 : extraInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}

	_size += 4 + (extraInfo == null ? 0 : extraInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
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
	if(_buf.remaining() <= 0) return;
	int _extraInfoCount = _buf.getInt();
	if(0 < _extraInfoCount){
		extraInfo = new byte[_extraInfoCount];
		_buf.get(extraInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt((extraInfo == null ? 0 : extraInfo.length));
	if(null != extraInfo){_buf.put(extraInfo);}

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

