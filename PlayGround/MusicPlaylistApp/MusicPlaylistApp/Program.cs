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

            ITrackRepository trackRepository = new TrackRepository();
            // new PlaylistRepository(stringsRepository, trackRepository, fileMetadata);는 플레이리스트 리포지토리를 생성하는 코드입니다.
            PlaylistRepository playlistRepository = new PlaylistRepository(stringsRepository, trackRepository, fileMetadata);
            
            // 1. create a new playlist
            Playlist workoutMix = new Playlist(new List<int> { 3, 5, 1 });
            Playlist chillVibes = new Playlist(new List<int> { 2, 6, 4 });
            
            // 2. save playlists
            // playlistRepository.Write(workoutMix)는 workoutMix 플레이리스트를 저장하는 코드입니다.
            playlistRepository.Write(workoutMix);
            playlistRepository.Write(chillVibes);
            
            // 3. read playlists
            List<Playlist> savedPlaylists = playlistsRepository.Read();
            
            Console.WriteLine("saved playlists loaded");
            
            // int mixNumber = 1;는 mixNumber라는 정수 변수를 선언하고 초기값으로 1을 할당하는 코드입니다.
            // 이유는 mixNumber 변수를 사용하여 플레이리스트의 번호를 추적하거나 식별하는 데 사용할 수 있기 때문입니다.
            // 예를 들어, 플레이리스트를 저장하거나 불러올 때 mixNumber를 사용하여 각 플레이리스트에 고유한 번호를 할당할 수 있습니다.
            // 이렇게 하면 나중에 플레이리스트를 관리하거나 구분하는 데 도움이 됩니다.
            int mixNumber = 1;
            
            foreach (Playlist playlist in savedPlaylists)
            {
                var trackDetails = playlist.TrackIds.Select(id =>
                {
                    // GetById는 trackRepository에서 특정 ID에 해당하는 트랙을 가져오는 메서드입니다.
                    Track t = trackRepository.GetById(id);
                    return $"'{t.Title}' by {t.Artist}";
                });
             
                // Console.WriteLine($"Playlist #{mixNumber++}: {string.Join(" | ", trackDetails)}");는 플레이리스트의 번호와 트랙 세부 정보를 출력하는 코드입니다.
                // mixNumber++는 현재 mixNumber 값을 출력한 후에 mixNumber를 1씩 증가시키는 역할을 합니다.
                // 이렇게 하면 각 플레이리스트에 고유한 번호가 할당되어 출력됩니다. 이유는 플레이리스트를 구분하고 식별하는 데 도움이 되기 때문입니다.
                // 예를 들어, "Playlist #1: 'Track Title' by Artist | 'Another Track' by Another Artist"와 같은 형식으로 출력될 수 있습니다.
                //string.Join은 trackDetails 컬렉션의 요소들을 " | " 구분자로 연결하여 하나의 문자열로 만드는 메서드입니다.
                //이렇게 하면 각 트랙의 세부 정보가 구분되어 출력됩니다. 이유는 플레이리스트에 포함된 트랙들을 명확하게 구분하여 보여주기 위해서입니다.
                Console.WriteLine($"Playlist #{mixNumber++}: {string.Join(" | ", trackDetails)}");
            }
            // Console.Out.Flush는   콘솔 출력 버퍼에 있는 모든 데이터를 즉시 출력하는 메서드입니다.
            // 이유는 프로그램이 종료되기 전에 모든 출력이 콘솔에 표시되도록 보장하기 위해서입니다. 
            // 버퍼에 있는 모든 데이터를 즉시 출력한다는 의미는 프로그램이 종료되기 전에 모든 출력이 콘솔에 표시되도록 보장하기 위해서입니다.
            // 콘솔의 의미는 쉽게 말해 컴퓨터 화면에 텍스트를 출력하는 영역입니다.
            // 프로그램이 실행되면서 콘솔에 출력되는 텍스트는 일시적으로 버퍼라는 임시 저장 공간에 저장됩니다.
            Console.Out.Flush();
            Console.WriteLine("\n Press Enter to exit...");
            Console.ReadLine();
        }
        // catch는 try 블록에서 발생할 수 있는 예외를 처리하는 코드입니다.
        // 쉽게말해, 프로그램이 실행되는 동안 발생할 수 있는 오류나 예외 상황을 처리하기 위해 사용됩니다.
        // 이유는 프로그램이 예외 상황에 직면했을 때 적절한 조치를 취하거나 사용자에게 오류 메시지를 제공하기 위해서입니다.
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[에러 발생] {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
        }
    }
}
