#!/bin/bash
set -e

DEPS_DIR="/Users/Hassan/.gemini/antigravity/scratch/JitsiMeetMAUI/NativeBinaries/android/deps"
mkdir -p "$DEPS_DIR"
cd "$DEPS_DIR"

JITSI_REPO="https://github.com/jitsi/jitsi-maven-repository/raw/master/releases"
MAVEN_CENTRAL="https://repo1.maven.org/maven2"

download_aar() {
    local repo_url=$1
    local group_id=$2
    local artifact_id=$3
    local version=$4
    local classifier=$5

    local group_path=$(echo "$group_id" | tr '.' '/')
    local filename="${artifact_id}-${version}${classifier}.aar"
    local url="${repo_url}/${group_path}/${artifact_id}/${version}/${filename}"

    echo "Downloading $filename..."
    curl -L -s -O "$url"
}

# React Native components with Jitsi specific versioning
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-webrtc" "124.0.7-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-amplitude" "1.4.13-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-async-storage" "1.23.1-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-background-timer" "2.4.1-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-calendar-events" "2.2.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-community_clipboard" "1.14.3-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-community_netinfo" "11.1.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-default-preference" "1.4.4-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-device-info" "12.1.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-gesture-handler" "2.24.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-get-random-values" "1.11.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-giphy" "4.1.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-google-signin" "10.1.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-keep-awake" "1.3.1-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-orientation-locker" "1.5.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-pager-view" "6.8.1-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-performance" "5.1.2-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-safe-area-context" "5.6.1-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-screens" "4.11.1-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-slider" "4.5.6-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-sound" "0.11.2-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-splash-view" "0.0.18-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-svg" "15.11.2-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-video" "6.13.0-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-webview" "13.13.5-jitsi-22286284" ""
download_aar "$JITSI_REPO" "com.facebook.react" "react-native-worklets-core" "1.6.2-jitsi-22286284" ""

# Core React Native & Hermes
download_aar "$MAVEN_CENTRAL" "com.facebook.react" "hermes-android" "0.77.2" "-release"
download_aar "$MAVEN_CENTRAL" "com.facebook.react" "react-android" "0.77.2" "-release"

echo "Downloading Timber & Dropbox..."
# Missing Timber
curl -L -s -O "https://repo1.maven.org/maven2/com/jakewharton/timber/timber/5.0.1/timber-5.0.1.aar"
# Dropbox
curl -L -s -O "https://repo1.maven.org/maven2/com/dropbox/core/dropbox-core-sdk/4.0.1/dropbox-core-sdk-4.0.1.jar"

echo "Done!"
