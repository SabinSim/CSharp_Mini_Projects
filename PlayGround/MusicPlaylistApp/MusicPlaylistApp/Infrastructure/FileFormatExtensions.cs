using System;

namespace MusicPlaylistApp.Infrastructure;

public static class FileFormatExtensions
{
    public static string ToExtension(this FileFormat format)
    {
        return format switch
        {
            
            // FileFormat.Txt => "txt",
            // FileFormat.Json => "json",
            // _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            // 는 switch 표현식을 사용하여 FileFormat 열거형의 각 값에 대해 해당 파일 확장자를 반환하는 코드입니다.
            // 쉽게 말하면, FileFormat이 Txt인 경우 "txt"를 반환하고, Json인 경우 "json"을 반환합니다.
            // 만약 FileFormat이 정의되지 않은 값이라면, ArgumentOutOfRangeException 예외를 발생시킵니다.
            FileFormat.Txt => "txt",
            FileFormat.Json => "json",
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}