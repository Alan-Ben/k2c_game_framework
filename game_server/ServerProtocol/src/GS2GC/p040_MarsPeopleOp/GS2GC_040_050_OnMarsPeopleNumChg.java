package GS2GC.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 人口数量变化
 **/
public class GS2GC_040_050_OnMarsPeopleNumChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 居民数量 */
private Common.MarsObj.Mars_PeopleNum peopleNum;


public GS2GC_040_050_OnMarsPeopleNumChg() {
	peopleNum = new Common.MarsObj.Mars_PeopleNum();
}

public GS2GC_040_050_OnMarsPeopleNumChg(
	 Common.MarsObj.Mars_PeopleNum _peopleNum
) {	peopleNum = _peopleNum;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)50; }

/** 居民数量 */
public Common.MarsObj.Mars_PeopleNum getPeopleNum() { return peopleNum; }
/** 居民数量 */
public void setPeopleNum(Common.MarsObj.Mars_PeopleNum _peopleNum) { peopleNum = _peopleNum; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _peopleNumCustLen = _buf.getInt();
	int _peopleNumCurPos = _buf.position();
	peopleNum.ReadUnzipBuf(_buf, _peopleNumCurPos + _peopleNumCustLen);
	_buf.position(_peopleNumCurPos + _peopleNumCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(peopleNum.GetBufSize());
	peopleNum.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)50);
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

