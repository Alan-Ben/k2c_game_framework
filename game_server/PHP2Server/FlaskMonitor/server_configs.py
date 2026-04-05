"""服务器配置独立文件

后续如果需要新增/修改服务器，只需在这里维护。
可以根据需要改成从环境变量或外部 JSON / YAML 加载。
"""

SERVER_CONFIGS = {
    'server1': {
        'name': 'master',
        'host': '192.168.10.252',
        'port': 37202,
        'client': 1,
        'user': 'MG-PHP',
        'password': 'MG-20190708',
        'key': 'MG-OL7NV8H1BS'
    },
    'server2': {
        'name': 'cooper',
        'host': '192.168.10.147',
        'port': 17202,
        'client': 1,
        'user': 'MG-PHP',
        'password': 'MG-20190708',
        'key': 'MG-OL7NV8H1BS'
    }
}
