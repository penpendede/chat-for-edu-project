#!/usr/bin/ksh
BASEDIR="/cygdrive/d/Development/Visual Studio 2013/Projects/chat-for-edu-project"
TEMPDIR="$BASEDIR/___temporaryFiles___"
TARGET="Schugt_Seyock_Dokumentation_Chatprogramm"
DOCDIR="$TEMPDIR/$TARGET"
mkdir -p "$DOCDIR"

set -A SOURCES	"Documentation/Dokumentation_chatprogramm.pdf" \
		"Documentation/ClassDiagram.png" \
		"Documentation/Datenbankschema.png" \
		"Documentation/Screenshot_chatprogramm.png"

for file in "${SOURCES[@]}"; do
	cp "$BASEDIR/$file" "$DOCDIR"
done

(cd "$TEMPDIR" && zip -9r "$BASEDIR/$TARGET.zip" "$TARGET")

rm -rf "$TEMPDIR"
