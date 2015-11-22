// A bitmap file
bitmap :
		/** The bitmap header */
		header

		/** The Device Independent Bitmap header */
		dibHeader

        /** The color table */
        colorTable

        /** The pixel data */
        pixelData
	;

header:
		/** The bitmap type */
		headerField (2 bytes as ASCII described by headerFieldType)

		/** The size of the entire file */
		fileSize (4 bytes as UINT32)

		/** Application specific */
		reserved1 (2 bytes)

		/** Application specific */
		reserved2 (2 bytes)

		/** Offset to the start of the image bytes */
		imageDataOffset (4 bytes as UINT32)
	;

headerFieldType :
		BM : "Windows Bitmap"
		BA : "OS/2 struct bitmap array"
		CI : "OS/2 struct color icon"
		CP : "OS/2 const color pointer"
		IC : "OS/2 struct icon"
		PT : "OS/2 pointer"
	;
	
dibHeader:
		/** The size of the DIB header */
		headerSize (4 bytes as UINT32)

		(additional properties identified by headerSize from bitmapType)
	;

bitmapType :
		/** Windows 2.0 or later / OS/2 1.x */
		12 : bitmapCoreHeader

		/** OS/2 BITMAPCOREHEADER2 - Adds halftoning. Adds RLE and Huffman 1D compression. */
		64 : os22xBitmapHeader

		/** Windows NT, 3.1x or later - Adds 16 bpp and 32 bpp formats. Adds RLE compression. */
		40 : bitmapInfoHeader

		/** Undocumented - adds RGB bit masks */
		52 : bitmapV2Header

		/** Bitmap with alpha mask */
		56 : bitmapV3Header

		/** Windows NT 4.0, 95 or later - Adds color space type and gamma correction */
		108 : bitmapV4Header

		/** Windows NT 5.0, 98 or later - Adds ICC color profiles */
		124 : bitmapV5Header
	;

bitmapInfoHeader :
		/** bitmap width in pixels */
		bitmapWidth (4 bytes as INT32)

		/** bitmap height in pixels */
		bitmapHeight (4 bytes as INT32)

		/** number of color planes. Must be 1. */
		colorPlanes (2 bytes as UINT16)

		/** number of bits per pixel, which is the color depth of the image. Typical values are 1, 4, 8, 16, 24 and 32. */
		bitsPerPixel (2 bytes as UINT16)

		/** compression method */
		compressionMethod (4 bytes as UINT32 described by compressionMethod)

		/** image size. This is the size of the raw bitmap data; a dummy 0 can be given for BI_RGB bitmaps. */
		imageSize (4 bytes as UINT32)

		/** horizontal resolution of the image (pixels per meter) */
		horizontalResolution (4 bytes as INT32)

		/** vertical resolution of the image (pixels per meter) */
		verticalResolution (4 bytes as INT32)

		/** number of colors in the color palette - or 0 to default to 2^n */
		colorsInColorPalette (4 bytes as UINT32)

		/** number of important colors used, or 0 when every color is important; generally ignored */
		importantColors (4 bytes as UINT32)
	;

compressionMethod :
		0 : "BI_RGB - none"
		1 : "BI_RLE8 - RLE 8-bit/pixel"
		2 : "BI_RLE4 - RLE 4-bit/pixel"
		3 : "BI_BITFIELDS - OS22XBITMAPHEADER: Huffman 1D, BITMAPV2INFOHEADER: RGB bit field masks, BITMAPV3INFOHEADER+: RGBA"
		4 : "BI_JPEG - OS22XBITMAPHEADER: RLE-24, BITMAPV4INFOHEADER+: JPEG image for printing"
		5 : "BI_PNG - BITMAPV4INFOHEADER+: PNG image for printing"
		6 : "BI_ALPHABITFIELDS - RGBA bit field masks (only Windows CE 5.0 with .NET 4.0 or later)"
		11 : "BI_CMYK - none (only Windows Metafile CMYK)"
		12 : "BI_CMYKRLE8 - RLE-8 (only Windows Metafile CMYK)"
		13 : "BI_CMYKRLE4 - RLE-4 (only Windows Metafile CMYK)"
	;

colorTable :
    /** Stored in RGBA32 format */
    colorTableEntries (bitmap.dibHeader.colorsInColorPalette items);

colorTableEntries :
    colorDefinition (4 bytes);

pixelData :
    rows (bitmap.dibHeader.bitmapHeight items);

rows :
    pixels (bitmap.dibHeader.bitmapWidth items);

pixels :
    pixel (3 bytes);