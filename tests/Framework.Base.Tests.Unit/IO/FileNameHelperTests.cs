// Copyright (c) Mahmoud Shaheen. All rights reserved.

using Framework.IO;

namespace Tests.IO;

public sealed class FileNameHelperTests
{
    public static readonly TheoryData<string, string> SanitizeFileNameData = new()
    {
        { "_", "_" },
        { "hello-word", "hello-word" },
        // Remove invalid file name chars
        { "/hello/-/word/", "hello-word" },
        { "%hello\nword___)..", "%helloword___).." },
        { "شاهين", "شاهين" },
        // Remove duplicated spaces
        { "hello   \n\t word", "hello word" },
        // HTML encode
        { "<>@hello \n   word&", "@hello word&amp;" },
    };

    [Theory]
    [MemberData(nameof(SanitizeFileNameData))]
    public void should_sanitize_as_expected_when_use_sanitize_file_name_method(string name, string expected)
    {
        // when
        var result = FileNameHelper.SanitizeFileName(name);

        // then
        result.Should().Be(expected);
    }
}
