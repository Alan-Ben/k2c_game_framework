package GS2GC.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_033_007_RetRechargeRebateInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 返利组信息列表 */
private java.util.ArrayList<Common.RechargeRebateObj.RechargeRebate_GroupInfo> groupInfoList;


public GS2GC_033_007_RetRechargeRebateInfo() {
	groupInfoList = new java.util.ArrayList<Common.RechargeRebateObj.RechargeRebate_GroupInfo>();
}

public GS2GC_033_007_RetRechargeRebateInfo(
	 java.util.ArrayList<Common.RechargeRebateObj.RechargeRebate_GroupInfo> _groupInfoList
) {	groupInfoList = _groupInfoList;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)7; }

/** 返利组信息列表 */
public java.util.ArrayList<Common.RechargeRebateObj.RechargeRebate_GroupInfo> getGroupInfoList() { return groupInfoList; }
/** 返利组信息列表 */
public void addGroupInfoList(Common.RechargeRebateObj.RechargeRebate_GroupInfo _groupInfoList) { groupInfoList.add(_groupInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < groupInfoList.size(); _i++) {
	_size += 4 + groupInfoList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < groupInfoList.size(); _i++) {
	_size += 4 + groupInfoList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _groupInfoListCount; _i++) { 
		Common.RechargeRebateObj.RechargeRebate_GroupInfo _groupInfoList = new Common.RechargeRebateObj.RechargeRebate_GroupInfo();
		if(_buf.remaining() <= 0) return;
	int __groupInfoListCustLen = _buf.getInt();
	int __groupInfoListCurPos = _buf.position();
	_groupInfoList.ReadUnzipBuf(_buf, __groupInfoListCurPos + __groupInfoListCustLen);
	_buf.position(__groupInfoListCurPos + __groupInfoListCustLen);

		groupInfoList.add(_groupInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)groupInfoList.size());
	for(int _i = 0; _i < groupInfoList.size(); _i++) { 
		_buf.putInt(groupInfoList.get(_i).GetBufSize());
	groupInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)7);
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

