// http://people.freebsd.org/~kientzle/libarchive/tar.5.txt

tarFile : fileRecord (unlimited items);

fileRecord :
    fileName (100 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    fileMode (8 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    ownerId (8 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    groupId (8 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    fileSize (12 bytes as {
            string fileString = Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0');
            if(! String.IsNullOrEmpty(fileString))
            {
                return Convert.ToInt32(fileString, 8);
            }
            else
            {
                return 0;
            }
        })
    lastModificationTime (12 bytes as {
            string lastModificationTimeString = Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0');
            
            if(! String.IsNullOrEmpty(lastModificationTimeString))
            {
                int secondsSinceEpoch = Convert.ToInt32(lastModificationTimeString, 8);
                return DateTime.Parse("January 1, 1970 00:00:00 GMT").AddSeconds(secondsSinceEpoch);
            }
            else
            {
                return DateTime.MinValue;
            }
        })
    checksum (8 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    linkIndicator (1 byte as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    linkedFileName (100 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    ustarIndicator (6 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    ustarVersion (2 bytes as {
            string fileString = Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0');
            if(! String.IsNullOrEmpty(fileString))
            {
                return Convert.ToInt32(fileString, 8);
            }
            else
            {
                return 0;
            }
        })
    ownerUserName (32 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    ownerGroupName (32 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    deviceMajorNumber (8 bytes as {
            string fileString = Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0');
            if(! String.IsNullOrEmpty(fileString))
            {
                return Convert.ToInt32(fileString, 8);
            }
            else
            {
                return 0;
            }
        })
    deviceMinorNumber (8 bytes as {
            string fileString = Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0');
            if(! String.IsNullOrEmpty(fileString))
            {
                return Convert.ToInt32(fileString, 8);
            }
            else
            {
                return 0;
            }
        })
    filenamePrefix (155 bytes as { return Encoding.ASCII.GetString(_content, _contentPosition, _byteCount).Trim().Trim('\0'); })
    fileContentGap (12 bytes)
    fileContent ( fileSize bytes)
    
    (padding to multiple of 512 bytes);