package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-捕捉结果
 **/
public class TreasureHunt_CaptureResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 捕捉奖励列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureReward> captureRewardList;


public TreasureHunt_CaptureResult() {
	captureRewardList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureReward>();
}

public TreasureHunt_CaptureResult(
	 java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureReward> _captureRewardList
) {	captureRewardList = _captureRewardList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 捕捉奖励列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CaptureReward> getCaptureRewardList() { return captureRewardList; }
/** 捕捉奖励列表 */
public void addCaptureRewardList(Common.TreasureHuntObj.TreasureHunt_CaptureReward _captureRewardList) { captureRewardList.add(_captureRewardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < captureRewardList.size(); _i++) {
	_size += 4 + captureRewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < captureRewardList.size(); _i++) {
	_size += 4 + captureRewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _captureRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _captureRewardListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_CaptureReward _captureRewardList = new Common.TreasureHuntObj.TreasureHunt_CaptureReward();
		if(_buf.remaining() <= 0) return;
	int __captureRewardListCustLen = _buf.getInt();
	int __captureRewardListCurPos = _buf.position();
	_captureRewardList.ReadUnzipBuf(_buf, __captureRewardListCurPos + __captureRewardListCustLen);
	_buf.position(__captureRewardListCurPos + __captureRewardListCustLen);

		captureRewardList.add(_captureRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)captureRewardList.size());
	for(int _i = 0; _i < captureRewardList.size(); _i++) { 
		_buf.putInt(captureRewardList.get(_i).GetBufSize());
	captureRewardList.get(_i).PutUnzipBuf(_buf);
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

