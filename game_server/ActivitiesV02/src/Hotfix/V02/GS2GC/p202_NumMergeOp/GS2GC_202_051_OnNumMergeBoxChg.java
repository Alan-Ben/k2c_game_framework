package Hotfix.V02.GS2GC.p202_NumMergeOp;

import java.nio.ByteBuffer;
/*********
 * 箱子信息变更
 **/
public class GS2GC_202_051_OnNumMergeBoxChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 箱子信息 */
private Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo boxInfo;


public GS2GC_202_051_OnNumMergeBoxChg() {
	boxInfo = new Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo();
}

public GS2GC_202_051_OnNumMergeBoxChg(
	 Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo
) {	boxInfo = _boxInfo;
}

public final byte getMainOrder() { return (byte)202; }

public final byte getSubOrder() { return (byte)51; }

/** 箱子信息 */
public Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo getBoxInfo() { return boxInfo; }
/** 箱子信息 */
public void setBoxInfo(Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo _boxInfo) { boxInfo = _boxInfo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _boxInfoCustLen = _buf.getInt();
	int _boxInfoCurPos = _buf.position();
	boxInfo.ReadUnzipBuf(_buf, _boxInfoCurPos + _boxInfoCustLen);
	_buf.position(_boxInfoCurPos + _boxInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(boxInfo.GetBufSize());
	boxInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)202);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)202);
	_recBuf.put((byte)51);
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

