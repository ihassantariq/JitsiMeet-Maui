#!/bin/bash
rm -rf /tmp/baksmali_test
mkdir -p /tmp/baksmali_test
java -jar /Users/Hassan/Library/Developer/Xamarin/android-sdk-macosx/cmdline-tools/latest/lib/d8.jar --help > /dev/null 2>&1
# use baksmali
wget -q https://bitbucket.org/JesusFreke/smali/downloads/baksmali-2.5.2.jar -O /tmp/baksmali_test/baksmali.jar
java -jar /tmp/baksmali_test/baksmali.jar d /tmp/dex_test/classes3.dex -c com.facebook.soloader.ExternalSoMapping -o /tmp/baksmali_test/out
cat /tmp/baksmali_test/out/com/facebook/soloader/ExternalSoMapping.smali
