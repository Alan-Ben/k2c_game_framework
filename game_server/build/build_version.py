#!/usr/bin/env python

import os
import subprocess
import datetime
import sys


def my_popen(cmd):
    with os.popen(cmd) as fp:
        bf = fp._stream.buffer.read()
    try:
        return bf.decode('utf-8').strip()
    except UnicodeDecodeError:
        return bf.decode('gbk').strip()

def get_tag():
    commit = my_popen("git log -n1 --shortstat").split("\n")[0]
    commit = commit.split(" ")[1];
    tag = my_popen("git tag -l --contains " + commit)
    return tag, commit


def getTag():
    commit = os.popen("git log -n1 --shortstat").read().strip().split("\n")[0]
    commit = commit.split(" ")[1];
    tag = os.popen("git tag -l --contains " + commit).read().strip()
    return tag, commit


author = os.popen("git config user.name").read()
email = os.popen("git config user.email").read()
# log = os.popen("git log --graph -10").read()
now = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
if sys.version > '3':
    tag, commit = get_tag()
else:
    tag, commit = getTag()

info = "module.exports={\n\"author\":\"%s\",\n\"email\":\"%s\",\n\"time\":\"%s\",\n\"tag\":\"%s\",\n\"commit\":\"%s\"\n};" % (
    author.strip(), email.strip(), now.strip(), tag.strip(), commit.strip())
ver = open("./version.txt", "wt")
ver.write(info)
ver.write("\n/*\n")
# ver.write(log)
ver.write("\n*/")
ver.close()
print(info)
