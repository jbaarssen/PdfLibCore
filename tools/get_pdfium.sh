#!/bin/bash
# source of mac universal
# https://github.com/bblanchon/pdfium-binaries/blob/master/mac_create_universal.sh

set -e

SRC_DIR="$PWD"
STAGE_DIR="$PWD/mac"
DEST_DIR="../src/PdfLibCore/runtimes"

create_univ () {
    if [ -e "pdfium-mac-x64$1.tgz" ] && [ -e "pdfium-mac-arm64$1.tgz" ]; then
        echo "Extracting x64..."
        mkdir -p $STAGE_DIR/x64
        cd $STAGE_DIR/x64
        tar xf $SRC_DIR/pdfium-mac-x64$1.tgz

        echo "Extracting arm64..."
        mkdir -p $STAGE_DIR/arm64
        cd $STAGE_DIR/arm64
        tar xf $SRC_DIR/pdfium-mac-arm64$1.tgz

        echo "Creating universal..."
        mkdir -p $STAGE_DIR/univ
        cp -r $STAGE_DIR/x64/* $STAGE_DIR/univ
        lipo -create \
            $STAGE_DIR/x64/lib/libpdfium.dylib \
            $STAGE_DIR/arm64/lib/libpdfium.dylib \
            -output $STAGE_DIR/univ/lib/libpdfium.dylib

        # echo "Creating target..."
        # cd $STAGE_DIR/univ
        # tar cf "$SRC_DIR/pdfium-mac-universal$1.tgz" -- *

        cd $SRC_DIR
    fi
}

wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F5268/pdfium-linux-x64.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F5268/pdfium-win-x64.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F5268/pdfium-mac-x64.tgz
wget https://github.com/bblanchon/pdfium-binaries/releases/download/chromium%2F5268/pdfium-mac-arm64.tgz

mkdir linux
mkdir windows
mkdir mac

tar -xvf pdfium-linux-x64.tgz -C linux
tar -xvf pdfium-win-x64.tgz -C windows

create_univ ""

mkdir -p $DEST_DIR/linux-x64/native/
mkdir -p $DEST_DIR/mac-univ/native/
mkdir -p $DEST_DIR/win-x64/native/

cp linux/lib/libpdfium.so $DEST_DIR/linux-x64/native/pdfium.so
cp linux/LICENSE $DEST_DIR/linux-x64/native/LICENSE

cp $STAGE_DIR/univ/lib/libpdfium.dylib $DEST_DIR/mac-univ/native/pdfium.dylib
cp $STAGE_DIR/univ/LICENSE $DEST_DIR/mac-univ/native/LICENSE

cp windows/bin/pdfium.dll $DEST_DIR/win-x64/native/pdfium.dll
cp windows/LICENSE $DEST_DIR/win-x64/native/LICENSE

rm pdfium-linux-x64.tgz pdfium-win-x64.tgz pdfium-mac-x64.tgz pdfium-mac-arm64.tgz 
rm -rf linux windows mac