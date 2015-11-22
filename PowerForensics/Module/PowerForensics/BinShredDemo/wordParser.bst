header :
    magic (2 bytes as ASCII)
    wordCount (4 bytes as UINT32)
    words (wordCount items);
words :
    wordLength (4 bytes as UINT32)
    word (wordLength bytes as UTF8);