/*

Properties in the TAR file format are all expressed as ASCII values.
When a property represents a number, they are the ASCII representation
of those numbers as expressed in Octal.

http://people.freebsd.org/~kientzle/libarchive/tar.5.txt

A complete TAR file format parser would contain runtime processing
logic to convert these "Octal in ASCII numbers" to their integer
equivalents.

This example uses a native byte expression (written in C#) for
the fileContent 'bytes' property to determine the file size so
that it can be read from the archive.

For a more complete (but slower) example, see tar_full.bst.

*/

tarFile : fileRecord (unlimited items);

fileRecord :
    fileName (100 bytes as ASCII)
    fileMode (8 bytes as ASCII)
    ownerId (8 bytes as ASCII)
    groupId (8 bytes as ASCII)
    fileSize (12 bytes as ASCII)
    lastModificationTime (12 bytes as ASCII)
    checksum (8 bytes as ASCII)
    linkIndicator (1 byte as ASCII)
    linkedFileName (100 bytes as ASCII)
    ustarIndicator (6 bytes as ASCII)
    ustarVersion (2 bytes as ASCII)
    ownerUserName (32 bytes as ASCII)
    ownerGroupName (32 bytes as ASCII)
    deviceMajorNumber (8 bytes as ASCII)
    deviceMinorNumber (8 bytes as ASCII)
    filenamePrefix (155 bytes as ASCII)
    fileContentGap (12 bytes as ASCII)
    fileContent ( {
        return (fileSize[0] == '\0') ? 0 :
            Convert.ToInt32(fileSize.Trim(), 8); } bytes)
    
    (padding to multiple of 512 bytes);