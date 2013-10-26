//********************************************************************************************************************************
//*                                                                                                                              *
//*     ID3v1 Library 2.0.3.6 ?3delite 2010-2011                                                                                *
//*     See ID3v2 Library 2.0 Readme.txt for details                                                                             *
//*                                                                                                                              *
//* Two licenses are available for commercial usage of this component:                                                           *
//* Shareware License: 50 Euros                                                                                                  *
//* Commercial License: 250 Euros                                                                                                *
//*                                                                                                                              *
//*     http://www.shareit.com/product.html?productid=300294127                                                                  *
//*                                                                                                                              *
//* Using the component in free programs is free.                                                                                *
//*                                                                                                                              *
//*     http://www.3delite.hu/Object%20Pascal%20Developer%20Resources/id3v2library.html                                          *
//*                                                                                                                              *
//* For other Delphi components see the home page:                                                                               *
//*                                                                                                                              *
//*     http://www.3delite.hu/                                                                                                   *
//*                                                                                                                              *
//* If you have any questions or enquiries please mail: 3delite@3delite.hu                                                       *
//*                                                                                                                              *
//* Good coding! :)                                                                                                              *
//* 3delite                                                                                                                      *
//********************************************************************************************************************************

unit ID3v1Library;

{$Optimization Off}

interface

Uses
    Windows,
    Classes;

const
    ID3V1TAGSIZE = 128;

const
    ID3V1TAGID: AnsiString = 'TAG';

const
    ID3V1LIBRARY_SUCCESS            = 0;
    ID3V1LIBRARY_ERROR              = $FFFF;
    ID3V1LIBRARY_ERROR_OPENING_FILE = 3;
    ID3V1LIBRARY_ERROR_READING_FILE = 4;
    ID3V1LIBRARY_ERROR_WRITING_FILE = 5;

type
    TID3v1TagData = packed record
        Identifier: Array [0..2] of AnsiChar;
        Title: Array [0..29] of AnsiChar;
        Artist: Array [0..29] of AnsiChar;
        Album: Array [0..29] of AnsiChar;
        Year: Array [0..3] of AnsiChar;
        Comment: Array [0..29] of AnsiChar;
        Genre: Byte;
    end;

const
    ID3Genres: Array[0..147] of PAnsiChar = (
        { The following genres are defined in ID3v1 }
        'Blues',
        'Classic Rock',
        'Country',
        'Dance',
        'Disco',
        'Funk',
        'Grunge',
        'Hip-Hop',
        'Jazz',
        'Metal',
        'New Age',
        'Oldies',
        'Other',     { <= 12 Default }
        'Pop',
        'R&B',
        'Rap',
        'Reggae',
        'Rock',
        'Techno',
        'Industrial',
        'Alternative',
        'Ska',
        'Death Metal',
        'Pranks',
        'Soundtrack',
        'Euro-Techno',
        'Ambient',
        'Trip-Hop',
        'Vocal',
        'Jazz+Funk',
        'Fusion',
        'Trance',
        'Classical',
        'Instrumental',
        'Acid',
        'House',
        'Game',
        'Sound Clip',
        'Gospel',
        'Noise',
        'AlternRock',
        'Bass',
        'Soul',
        'Punk',
        'Space',
        'Meditative',
        'Instrumental Pop',
        'Instrumental Rock',
        'Ethnic',
        'Gothic',
        'Darkwave',
        'Techno-Industrial',
        'Electronic',
        'Pop-Folk',
        'Eurodance',
        'Dream',
        'Southern Rock',
        'Comedy',
        'Cult',
        'Gangsta',
        'Top 40',
        'Christian Rap',
        'Pop/Funk',
        'Jungle',
        'Native American',
        'Cabaret',
        'New Wave',
        'Psychedelic', // = 'Psychadelic' in ID3 docs, 'Psychedelic' in winamp.
        'Rave',
        'Showtunes',
        'Trailer',
        'Lo-Fi',
        'Tribal',
        'Acid Punk',
        'Acid Jazz',
        'Polka',
        'Retro',
        'Musical',
        'Rock & Roll',
        'Hard Rock',
        { The following genres are Winamp extensions }
        'Folk',
        'Folk-Rock',
        'National Folk',
        'Swing',
        'Fast Fusion',
        'Bebob',
        'Latin',
        'Revival',
        'Celtic',
        'Bluegrass',
        'Avantgarde',
        'Gothic Rock',
        'Progressive Rock',
        'Psychedelic Rock',
        'Symphonic Rock',
        'Slow Rock',
        'Big Band',
        'Chorus',
        'Easy Listening',
        'Acoustic',
        'Humour',
        'Speech',
        'Chanson',
        'Opera',
        'Chamber Music',
        'Sonata',
        'Symphony',
        'Booty Bass',
        'Primus',
        'Porn Groove',
        'Satire',
        'Slow Jam',
        'Club',
        'Tango',
        'Samba',
        'Folklore',
        'Ballad',
        'Power Ballad',
        'Rhythmic Soul',
        'Freestyle',
        'Duet',
        'Punk Rock',
        'Drum Solo',
        'A capella', // A Capella
        'Euro-House',
        'Dance Hall',
        { winamp ?? genres }
        'Goa',
        'Drum & Bass',
        'Club-House',
        'Hardcore',
        'Terror',
        'Indie',
        'BritPop',
        'Negerpunk',
        'Polsk Punk',
        'Beat',
        'Christian Gangsta Rap',
        'Heavy Metal',
        'Black Metal',
        'Crossover',
        'Contemporary Christian',
        'Christian Rock',
        { winamp 1.91 genres }
        'Merengue',
        'Salsa',
        'Trash Metal',
        { winamp 1.92 genres }
        'Anime',
        'JPop',
        'SynthPop'
    );

type
    TID3v1Tag = class
    private
    public
        FileName: String;
        Loaded: Boolean;
        Revision1: Boolean;
        Title: AnsiString;
        Artist: AnsiString;
        Album: AnsiString;
        Year: AnsiString;
        Comment: AnsiString;
        Track: Byte;
        Genre: AnsiString;
        Constructor Create;
        Destructor Destroy; override;
        function LoadFromFile(FileName: String): Integer;
        function LoadFromStream(TagStream: TStream): Integer;
        function SaveToFile(FileName: String): Integer;
        function SaveToStream(var TagStream: TStream): Integer;
        procedure Clear;
    end;

    function Min(const B1, B2: Integer): Integer;

    procedure AnsiStringToPAnsiChar(const Source: AnsiString; Dest: PAnsiChar; const MaxLength: Integer);
    function PAnsiCharToAnsiString(P: PAnsiChar; MaxLength: Integer): AnsiString;

    function ID3GenreDataToString(GenreIndex: Byte): AnsiString;
    function ID3GenreStringToData(Genre: AnsiString): Byte;

    function ID3v1RemoveTag(FileName: String): Integer;

implementation

Uses
    SysUtils;

function Min(const B1, B2: Integer): Integer;
begin
    if B1 < B2 then begin
        Result := B1
    end else begin
        Result := B2;
    end;
end;

procedure AnsiStringToPAnsiChar(const Source: AnsiString; Dest: PAnsiChar; const MaxLength: Integer);
begin
    Move(PAnsiChar(Source)^, Dest^, Min(MaxLength, Length(Source)));
end;

function PAnsiCharToAnsiString(P: PAnsiChar; MaxLength: Integer): AnsiString;
var
    Q: PAnsiChar;
begin
    Q := P;
    while (P - Q < MaxLength) AND (P^ <> #0) do begin
        Inc(P);
    end;
    { [Q..P) is valid }
    SetString(Result, Q, P - Q);
end;

Constructor TID3v1Tag.Create;
begin
    Inherited;
    Clear;
end;

Destructor TID3v1Tag.Destroy;
begin
    Inherited;
end;

procedure TID3v1Tag.Clear;
begin
    FileName := '';
    Loaded := False;
    Revision1 := False;
    Title := '';
    Artist := '';
    Album := '';
    Year := '';
    Comment := '';
    Genre := '';
end;

function TID3v1Tag.LoadFromFile(FileName: String): Integer;
var
    FileStream: TFileStream;
begin
    Result := ID3V1LIBRARY_ERROR;
    Clear;
    Loaded := False;
    if NOT FileExists(FileName) then begin
        Result := ID3V1LIBRARY_ERROR_OPENING_FILE;
        Exit;
    end;
    FileStream := TFileStream.Create(FileName, fmOpenRead OR fmShareDenyWrite);
    try
        FileStream.Seek(-ID3V1TAGSIZE, soEnd);
        Result := LoadFromStream(FileStream);
        if Result = ID3V1LIBRARY_SUCCESS then begin
            Self.FileName := FileName;
        end;
    finally
        FreeAndNil(FileStream);
    end;
end;

function TID3v1Tag.LoadFromStream(TagStream: TStream): Integer;
var
    TagData: TID3v1TagData;
begin
    Result := ID3V1LIBRARY_ERROR;
    Loaded := False;
    ZeroMemory(@TagData, SizeOf(TID3v1TagData));
    try
        TagStream.Read(TagData, ID3V1TAGSIZE);
        if TagData.Identifier <> ID3V1TAGID then begin
            Exit;
        end;
        Title := PAnsiCharToAnsiString(@TagData.Title, 30);
        Artist := PAnsiCharToAnsiString(@TagData.Artist, 30);
        Album := PAnsiCharToAnsiString(@TagData.Album, 30);
        Year := PAnsiCharToAnsiString(@TagData.Year, 4);
        Comment := PAnsiCharToAnsiString(@TagData.Comment, 30);
        Genre := ID3GenreDataToString(TagData.Genre);
        if TagData.Comment[28] = #0 then begin
            Track := Byte(TagData.Comment[29]);
            Revision1 := True;
        end else begin
            Track := 0;
            Revision1 := False;
        end;
        Loaded := True;
        Result := ID3V1LIBRARY_SUCCESS;
    except
        Clear;
        Result := ID3V1LIBRARY_ERROR_READING_FILE;
    end;
end;

function TID3v1Tag.SaveToFile(FileName: String): Integer;
var
    FileStream: TStream;
begin
    Result := ID3V1LIBRARY_ERROR;
    try
        if FileExists(FileName) then begin
            FileStream := TFileStream.Create(FileName, fmOpenReadWrite OR fmShareDenyWrite);
        end else begin
            FileStream := TFileStream.Create(FileName, fmCreate OR fmShareDenyWrite);
        end;
    except
        Result := ID3V1LIBRARY_ERROR_OPENING_FILE;
        Exit;
    end;
    try
        FileStream.Seek(-ID3V1TAGSIZE, soEnd);
        Result := SaveToStream(FileStream);
        if Result = ID3V1LIBRARY_SUCCESS then begin
            Self.FileName := FileName;
        end;
    finally
        FreeAndNil(FileStream);
    end;
end;

function TID3v1Tag.SaveToStream(var TagStream: TStream): Integer;
var
    TagData: TID3v1TagData;
begin
    Result := ID3V1LIBRARY_ERROR;
    ZeroMemory(@TagData, SizeOf(TID3v1TagData));
    try
        TagStream.Read(TagData, ID3V1TAGSIZE);
    except
        Result := ID3V1LIBRARY_ERROR_READING_FILE;
        Exit;
    end;
    try
        if TagData.Identifier = ID3V1TAGID then begin
            TagStream.Seek(-ID3V1TAGSIZE, soCurrent);
        end else begin
            TagStream.Seek(0, soEnd);
        end;
        ZeroMemory(@TagData, SizeOf(TID3v1TagData));
        Move(Pointer(ID3V1TAGID)^, TagData.Identifier[0], 3);
        AnsiStringToPAnsiChar(Title, @TagData.Title, 30);
        AnsiStringToPAnsiChar(Artist, @TagData.Artist, 30);
        AnsiStringToPAnsiChar(Album, @TagData.Album, 30);
        AnsiStringToPAnsiChar(Year, @TagData.Year, 4);
        AnsiStringToPAnsiChar(Comment, @TagData.Comment, 30);
        TagData.Genre := ID3GenreStringToData(Genre);
        if TagData.Comment[28] = #0 then begin
            TagData.Comment[29] := AnsiChar(Track);
        end;
        TagStream.Write(TagData, ID3V1TAGSIZE);
        Result := ID3V1LIBRARY_SUCCESS;
    except
        Result := ID3V1LIBRARY_ERROR_WRITING_FILE;
    end;
end;

function ID3GenreDataToString(GenreIndex: Byte): AnsiString;
begin
    Result := ID3Genres[GenreIndex];
end;

function ID3GenreStringToData(Genre: AnsiString): Byte;
var
    i: Integer;
    GenreString: AnsiString;
begin
    Result := 12;
    GenreString := Genre;
    if GenreString = 'Psychadelic' then begin
        GenreString := 'Psychedelic';
    end;
    for i := 0 to Length(ID3Genres) - 1 do begin
        if GenreString = ID3Genres[i] then begin
            Result := i;
            Break;
        end;
    end;
end;

function ID3v1RemoveTag(FileName: String): Integer;
var
    AudioFile: TFileStream;
    OutputFileName: String;
    OutputFile: TFileStream;
    DataByte: Byte;
begin
    Result := ID3V1LIBRARY_ERROR;
    if NOT FileExists(FileName) then begin
        Exit;
    end;
    try
        try
            AudioFile := TFileStream.Create(FileName, fmOpenRead);
        except
            Result := ID3V1LIBRARY_ERROR_OPENING_FILE;
            Exit;
        end;
        AudioFile.Seek(-ID3V1TAGSIZE, soEnd);
        AudioFile.Read(DataByte, 1);
        if DataByte = Ord('T') then begin
            AudioFile.Read(DataByte, 1);
            if DataByte = Ord('A') then begin
                AudioFile.Read(DataByte, 1);
                if DataByte = Ord('G') then begin
                    OutputFileName := ChangeFileExt(FileName, '.tmp');
                    try
                        try
                            OutputFile := TFileStream.Create(OutputFileName, fmCreate OR fmOpenReadWrite);
                        except
                            Result := ID3V1LIBRARY_ERROR_OPENING_FILE;
                            Exit;
                        end;
                        AudioFile.Seek(0, soBeginning);
                        OutputFile.CopyFrom(AudioFile, AudioFile.Size - ID3V1TAGSIZE);
                        FreeAndNil(AudioFile);
                        FreeAndNil(OutputFile);
                        if NOT DeleteFile(PWideChar(FileName)) then begin
                            Result := GetLastError;
                        end else begin
                            RenameFile(OutputFileName, FileName);
                            Result := ID3V1LIBRARY_SUCCESS;
                        end;
                    except
                        Result := ID3V1LIBRARY_ERROR_WRITING_FILE;
                        FreeAndNil(OutputFile);
                        DeleteFile(PWideChar(OutputFileName));
                        Exit;
                    end;
                    if OutputFile <> nil then begin
                        FreeAndNil(OutputFile);
                    end;
                end;
            end;
        end;
        if AudioFile <> nil then begin
            FreeAndNil(AudioFile);
        end;
    except
        Result := ID3V1LIBRARY_ERROR;
    end;
end;

end.

