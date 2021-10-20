using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace JourneysTest;

public static class TestFileSystem
{
    public static readonly IFileSystem TestFiles = new MockFileSystem(
        new Dictionary<string, MockFileData> {
            {
                @"/testfile1", new MockFileData(
                    "1 1 E\n" +
                    "RFRFRFRF\n" +
                    "1 1 E")
            }, 
            {
                @"/testfile2", new MockFileData(
                    "\n" +
                    "3 2 N\n" +
                    "FRRFLLFFRRFLL\n" +
                    "3 3 N")
            }, 
            {
                @"/testfile3", new MockFileData(
                    "1 1 E\n" +
                    "RFRFRFRF\n" +
                    "1 1 E\n" +
                    "\n" +
                    "3 2 N\n" +
                    "FRRFLLFFRRFLL\n" +
                    "3 3 N\n" +
                    "\n" +
                    "0 3 W\n" +
                    "LLFFFLFLFL\n" +
                    "2 4 S\n")
            }, 
            {
                @"/testfile4", new MockFileData(
                    "1 1\n" +
                    "RFRFRFRF\n" +
                    "\n" +
                    "1 2 N\n" +
                    "F\n" +
                    "2 2 N\n" +
                    "2 2 N\n")
            }, 
            {
                @"/testfile5", new MockFileData("")
            }
        });
}

