package GC2GS.p011_ClientDataOp;

import java.nio.ByteBuffer;
public class GC2GS_011_003_ReqWebEncryptedData implements ALBasicProtocolPack._IALProtocolStructure {
/** 账号充值美金价值 */
private float userPay;
/** 用户语言 */
private String langCode;
/** 项目id */
private String project;
/** 项目名称 */
private String packageName;
/** 商品列表 */
private String products;


public GC2GS_011_003_ReqWebEncryptedData() {
	userPay = 0f;
	langCode = "";
	project = "";
	packageName = "";
	products = "";
}

public GC2GS_011_003_ReqWebEncryptedData(
	 float _userPay
	, String _langCode
	, String _project
	, String _packageName
	, String _products
) {	userPay = _userPay;
	langCode = _langCode;
	project = _project;
	packageName = _packageName;
	products = _products;
}

public final byte getMainOrder() { return (byte)11; }

public final byte getSubOrder() { return (byte)3; }

/** 账号充值美金价值 */
public float getUserPay() { return userPay; }
/** 账号充值美金价值 */
public void setUserPay(float _userPay) { userPay = _userPay; }
/** 用户语言 */
public String getLangCode() { return langCode; }
/** 用户语言 */
public void setLangCode(String _langCode) { langCode = _langCode; }
/** 项目id */
public String getProject() { return project; }
/** 项目id */
public void setProject(String _project) { project = _project; }
/** 项目名称 */
public String getPackageName() { return packageName; }
/** 项目名称 */
public void setPackageName(String _packageName) { packageName = _packageName; }
/** 商品列表 */
public String getProducts() { return products; }
/** 商品列表 */
public void setProducts(String _products) { products = _products; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(langCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(project);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(packageName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(products);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(langCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(project);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(packageName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(products);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) userPay = _buf.getFloat();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) langCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) project = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) packageName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) products = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putFloat(userPay);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, langCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, project);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, packageName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, products);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)11);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)11);
	_recBuf.put((byte)3);
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

