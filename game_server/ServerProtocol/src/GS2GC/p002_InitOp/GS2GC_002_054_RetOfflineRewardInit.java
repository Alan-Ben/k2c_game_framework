package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 离线玩家奖励初始化数据
 **/
public class GS2GC_002_054_RetOfflineRewardInit implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.OfflineRewardObj.OfflineReward_Info> rewardList;


public GS2GC_002_054_RetOfflineRewardInit() {
	rewardList = new java.util.ArrayList<Common.OfflineRewardObj.OfflineReward_Info>();
}

public GS2GC_002_054_RetOfflineRewardInit(
	 java.util.ArrayList<Common.OfflineRewardObj.OfflineReward_Info> _rewardList
) {	rewardList = _rewardList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)54; }

public java.util.ArrayList<Common.OfflineRewardObj.OfflineReward_Info> getRewardList() { return rewardList; }
public void addRewardList(Common.OfflineRewardObj.OfflineReward_Info _rewardList) { rewardList.add(_rewardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		Common.OfflineRewardObj.OfflineReward_Info _rewardList = new Common.OfflineRewardObj.OfflineReward_Info();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)54);
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

