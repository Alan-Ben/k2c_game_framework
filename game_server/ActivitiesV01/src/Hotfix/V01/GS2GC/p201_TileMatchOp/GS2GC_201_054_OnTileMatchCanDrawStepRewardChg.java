package Hotfix.V01.GS2GC.p201_TileMatchOp;

import java.nio.ByteBuffer;
/*********
 * 三消阶段可领取奖励数据变更
 **/
public class GS2GC_201_054_OnTileMatchCanDrawStepRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward> canDrawStepRewardList;


public GS2GC_201_054_OnTileMatchCanDrawStepRewardChg() {
	canDrawStepRewardList = new java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward>();
}

public GS2GC_201_054_OnTileMatchCanDrawStepRewardChg(
	 java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward> _canDrawStepRewardList
) {	canDrawStepRewardList = _canDrawStepRewardList;
}

public final byte getMainOrder() { return (byte)201; }

public final byte getSubOrder() { return (byte)54; }

public java.util.ArrayList<Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward> getCanDrawStepRewardList() { return canDrawStepRewardList; }
public void addCanDrawStepRewardList(Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList) { canDrawStepRewardList.add(_canDrawStepRewardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (canDrawStepRewardList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (canDrawStepRewardList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _canDrawStepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _canDrawStepRewardListCount; _i++) { 
		Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList = new Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward();
		if(_buf.remaining() <= 0) return;
	int __canDrawStepRewardListCustLen = _buf.getInt();
	int __canDrawStepRewardListCurPos = _buf.position();
	_canDrawStepRewardList.ReadUnzipBuf(_buf, __canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);
	_buf.position(__canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);

		canDrawStepRewardList.add(_canDrawStepRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)canDrawStepRewardList.size());
	for(int _i = 0; _i < canDrawStepRewardList.size(); _i++) { 
		_buf.putInt(canDrawStepRewardList.get(_i).GetBufSize());
	canDrawStepRewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)201);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)201);
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

