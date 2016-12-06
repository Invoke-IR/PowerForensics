using System;

namespace PowerForensics.Utilities.Compression
{
    /// <summary>
    /// 
    /// </summary>
    public class Xpress
    {
        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputBuffer"></param>
        /// <param name="outputSize"></param>
        /// <param name="inputConsumed"></param>
        /// <returns></returns>
        public static byte[] DecompressBuffer(byte[] inputBuffer, uint outputSize, uint inputConsumed)
        {
            return DecompressBufferLZ77(inputBuffer, outputSize, inputConsumed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputBuffer"></param>
        /// <param name="outputSize"></param>
        /// <param name="inputConsumed"></param>
        /// <returns></returns>
        private static byte[] DecompressBufferLZ77(byte[] inputBuffer, uint outputSize, uint inputConsumed)
        {
            byte[] OutputBuffer = new byte[outputSize];
            int BufferedFlags = 0;
            int BufferedFlagCount = 0;
            int InputPosition = 0;
            int OutputPosition = 0;
            int LastLengthHalfByte = 0;
            int MatchBytes = 0;
            int MatchLength = 0;
            int MatchOffset = 0;

            while (OutputPosition < outputSize)
            {
                if (BufferedFlagCount == 0)
                {
                    BufferedFlags = BitConverter.ToInt32(inputBuffer, InputPosition);
                    InputPosition += 4;
                    BufferedFlagCount = 32;
                }
                BufferedFlagCount--;

                if ((BufferedFlags & (1 << BufferedFlagCount)) == 0)
                {
                    OutputBuffer[OutputPosition] = inputBuffer[InputPosition];
                    InputPosition += 1;
                    OutputPosition += 1;
                }
                else
                {
                    MatchBytes = BitConverter.ToInt16(inputBuffer, InputPosition);
                    InputPosition += 2;
                    MatchLength = MatchBytes % 8;
                    MatchOffset = (MatchBytes / 8) + 1;

                    if (MatchLength == 7)
                    {
                        if (LastLengthHalfByte == 0)
                        {
                            MatchLength = inputBuffer[InputPosition] % 16;
                            LastLengthHalfByte = InputPosition;
                            InputPosition += 1;
                        }
                        else
                        {
                            MatchLength = inputBuffer[LastLengthHalfByte] / 16;
                            LastLengthHalfByte = 0;
                        }

                        if (MatchLength == 15)
                        {
                            MatchLength = inputBuffer[InputPosition];
                            InputPosition += 1;

                            if (MatchLength == 255)
                            {
                                MatchLength = BitConverter.ToInt16(inputBuffer, InputPosition);
                                InputPosition += 2;

                                if (MatchLength < 15 + 7)
                                {
                                    // Return error.
                                    throw new Exception("Invalid Compressed Data.");
                                }

                                MatchLength -= (15 + 7);
                            }
                            MatchLength += 15;
                        }
                        MatchLength += 7;
                    }
                    MatchLength += 3;

                    while (MatchLength != 0)
                    {
                        OutputBuffer[OutputPosition] = OutputBuffer[OutputPosition - MatchOffset];
                        OutputPosition += 1;
                        MatchLength -= 1;
                    }
                }
            }
            return OutputBuffer;
        }

        #endregion Static Methods
    }
}