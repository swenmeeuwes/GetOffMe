#! /bin/sh

BASE_URL=http://netstorage.unity3d.com/unity
HASH=88d00a7498cd
VERSION=5.5.1f1 #2017.1.0f3 # Should use newer version... but returns error (file not found)

download() {
  file=$1
  url="$BASE_URL/$HASH/$package"

  echo "Downloading from $url: "
  curl -o `basename "$package"` "$url"
}

install() {
  package=$1
  download "$package"

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

# See $BASE_URL/$HASH/unity-$VERSION-$PLATFORM.ini for complete list
# of available packages, where PLATFORM is `osx` or `win`

install "MacEditorInstaller/Unity-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-$VERSION.pkg"
install "MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-$VERSION.pkg"