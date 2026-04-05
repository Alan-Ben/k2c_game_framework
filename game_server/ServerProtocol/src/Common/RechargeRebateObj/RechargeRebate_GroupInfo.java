package Common.RechargeRebateObj;

import java.nio.ByteBuffer;
/*********
 * 充值返利组信息
 **/
public class RechargeRebate_GroupInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 组ID */
private long groupId;
/** 计数 VIP经验或充值天数 */
private long count;
/** 已领取档位ID列表 */
private java.util.ArrayList<Long> hadDrawStepList;


public RechargeRebate_GroupInfo() {
	groupId = (long)0;
	count = (long)0;
	hadDrawStepList = new java.util.ArrayList<Long>();
}

public RechargeRebate_GroupInfo(
	 long _groupId
	, long _count
	, java.util.ArrayList<Long> _hadDrawStepList
) {	groupId = _groupId;
	count = _count;
	hadDrawStepList = _hadDrawStepList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 组ID */
public long getGroupId() { return groupId; }
/** 组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 计数 VIP经验或充值天数 */
public long getCount() { return count; }
/** 计数 VIP经验或充值天数 */
public void setCount(long _count) { count = _count; }
/** 已领取档位ID列表 */
public java.util.ArrayList<Long> getHadDrawStepList() { return hadDrawStepList; }
/** 已领取档位ID列表 */
public void addHadDrawStepList(long _hadDrawStepList) { hadDrawStepList.add(_hadDrawStepList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (hadDrawStepList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (hadDrawStepList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepListCount; _i++) { 
		long _hadDrawStepList = (long)0;
		if(_buf.remaining() > 0) _hadDrawStepList = _buf.getLong();
		hadDrawStepList.add(_hadDrawStepList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(count);
	_buf.putShort((short)hadDrawStepList.size());
	for(int _i = 0; _i < hadDrawStepList.size(); _i++) { 
		_buf.putLong(hadDrawStepList.get(_i));
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

