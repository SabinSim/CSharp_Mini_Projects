using System;
using System.Collections.Generic;
using System.Linq;
using MusicPlaylistApp.Infrastructure;
using MusicPlaylistApp.Tracks;
using MusicPlaylistApp.Playlists;

namespace MusicPlaylistApp;

class Program

{
    private const FileFormat CurrentFileFormat = FileFormat.Json;

    static void Main()
    {
        try
        {   // FileMetadata fileMetadata = new FileMetadata("my_playlists", CurrentFileFormat)는 파일 메타데이터를 생성하는 코드입니다.
            // "my_playlists"는 파일의 이름을 나타내며, CurrentFileFormat은 파일 형식을 지정합니다.
            // 이 메타데이터는 나중에 파일을 저장하거나 불러올 때 사용됩니다.
            FileMetadata fileMetadata = new FileMetadata("my_playlists", CurrentFileFormat);
            
            // CurrentFileFormat == FileFormat.Json는 현재 파일 형식이 JSON인지 확인하는 조건문입니다.
            // 이 조건이 참이면, JSON 형식으로 데이터를 처리하기 위한 문자열 리포지토리를 생성할 수 있습니다.
            IStringsRepository stringsRepository = CurrentFileFormat == FileFormat.Json
                // ? new StringsJsonRepository()는 조건이 참일 때 실행되는 코드입니다.
                // 이 경우, JSON 형식으로 데이터를 처리하기 위한 문자열 리포지토리를 생성합니다.
                ? new StringsJsonRepository()
                // : new StringsTextualRepository();는 조건이 거짓일 때 실행되는 코드입니다.
                : new StringsTextualRepository();

        }
    }
}
