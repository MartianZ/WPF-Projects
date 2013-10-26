//********************************************************************************************************************************
//*                                                                                                                              *
//*     ID3v2 Library 2.0.4.7 ?3delite 2010-2011                                                                                *
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

unit ID3v2_Lib;

{$Optimization Off}

interface

Uses
    Windows,
    Classes;

Const
    ID3V2LIBRARY_VERSION = $0204;

type
    TID3v2ID = Array [0..2] of AnsiChar;
    TFrameID = Array [0..3] of AnsiChar;
    TLanguageID = Array [0..2] of AnsiChar;

const
    ID3V2LIBRARY_SUCCESS                    = 0;
    ID3V2LIBRARY_ERROR                      = $FFFF;
    ID3V2LIBRARY_ERROR_EMPTY_TAG            = 1;
    ID3V2LIBRARY_ERROR_EMPTY_FRAMES         = 2;
    ID3V2LIBRARY_ERROR_OPENING_FILE         = 3;
    ID3V2LIBRARY_ERROR_READING_FILE         = 4;
    ID3V2LIBRARY_ERROR_WRITING_FILE         = 5;
    ID3V2LIBRARY_ERROR_DOESNT_FIT           = 6;
    ID3V2LIBRARY_ERROR_NOT_SUPPORTED_VERSION= 7;

const
    ID3V2LIBRARY_DEFAULT_PADDING_SIZE    = 4096;

const
    ID3V2LIBRARY_SESC_ID    = $55555555;
    ID3V2LIBRARY_SESC_VERSION2: Byte = $02;

type
    TID3v2ExtendedHeader3 = class
        Size: Cardinal;
        CodedSize: Cardinal;
        Data: TMemoryStream;
        Flags: Word;
        CRCPresent: Boolean;
        Constructor Create;
        Destructor Destroy; override;
        procedure DecodeExtendedHeaderSize;
        procedure DecodeExtendedHeaderFlags;
    end;

type
    TID3v2ExtendedHeader4TagSizeRestriction = (
        NoMoreThan128FramesAnd1MBTotalTagSize,
        NoMoreThan64FramesAnd128KBTotalTagSize,
        NoMoreThan32FramesAnd40KBTotalTagSize,
        NoMoreThan32FramesAnd4KBTotalTagSize
    );

type
    TID3v2ExtendedHeader4TextEncodingRestriction = (
        NoTextEncodingRestrictions,
        OnlyEncodedWithISO88591OrUTF8
    );

type
    TID3v2ExtendedHeader4TextFieldsSizeRestriction = (
        NoTextFieldsSizeRestrictions,
        NoStringIsLongerThan1024Characters,
        NoStringIsLongerThan128Characters,
        NoStringIsLongerThan30Characters
    );

type
    TID3v2ExtendedHeader4ImageEncodingRestriction = (
        NoImageEncodingRestrictions,
        ImagesAreEncodedOnlyWithPNGOrJPEG
    );

type
    TID3v2ExtendedHeader4ImageSizeRestriction = (
        NoImageSizeRestrictions,
        AllImagesAre256x256PixelsOrSmaller,
        AllImagesAre64x64PixelsOrSmaller,
        AllImagesAreExactly64x64PixelsUnlessRequiredOtherwise
    );

type
    TID3v2ExtendedHeader4 = class
        Size: Cardinal;
        CodedSize: Cardinal;
        FlagBytes: Byte;
        Flags: Byte;
        ExtendedFlagsDataSize: Cardinal;
        ExtendedFlagsData: Array of Byte;
        TagIsAnUpdate: Boolean;
        CRCPresent: Boolean;
        TagRestrictions: Boolean;
        TagRestrictionsData: TID3v2ExtendedHeader4TagSizeRestriction;
        TextEncodingRestrictions: TID3v2ExtendedHeader4TextEncodingRestriction;
        TextFieldsSizeRestriction: TID3v2ExtendedHeader4TextFieldsSizeRestriction;
        ImageEncodingRestriction: TID3v2ExtendedHeader4ImageEncodingRestriction;
        ImageSizeRestriction: TID3v2ExtendedHeader4ImageSizeRestriction;
        Constructor Create;
        Destructor Destroy; override;
        procedure DecodeExtendedHeaderSize;
        procedure DecodeExtendedHeaderFlags;
        procedure CalculateExtendedFlagsDataSize;
        procedure DecodeExtendedHeaderFlagData;
    end;

type
    TID3v2SampleCache = Array of Byte;

type
    TID3v2Frame = class
    private
    public
        ID: TFrameID;
        Size: DWord;
        CodedSize: Cardinal;
        Stream: TMemoryStream;
        Flags: Word;
        TagAlterPreservation: Boolean;
        FileAlterPreservation: Boolean;
        ReadOnly: Boolean;
        Compressed: Boolean;
        Encrypted: Boolean;
        GroupingIdentity: Boolean;
        Unsynchronised: Boolean;
        DataLengthIndicator: Boolean;
        GroupIdentifier: Byte;
        EncryptionMethod: Byte;
        DataLengthIndicatorValue: Cardinal;
        Constructor Create;
        Destructor Destroy; override;
        procedure DecodeFlags3;
        procedure EncodeFlags3;
        procedure DecodeFlags4;
        procedure EncodeFlags4;
        function Compress: Boolean;
        function DeCompress: Boolean;
        function RemoveUnsynchronisation: Boolean;
        function ApplyUnsynchronisation: Boolean;
    end;

type
    TID3v2Tag = class
    private
        CodedSize: DWord;
        procedure DecodeFlags;
        procedure EncodeFlags;
        procedure DecodeSize;
        procedure EncodeSize;
        function ReadExtendedHeader(TagStream: TStream): Boolean;
        //function WriteExtendedHeader(TagStream: TStream): Boolean;
        function RemoveUnsynchronisationOnExtendedHeaderSize: Boolean;
        function ApplyUnsynchronisationOnExtendedHeaderSize: Boolean;
        function RemoveUnsynchronisationOnExtendedHeaderData: Boolean;
        function ApplyUnsynchronisationOnExtendedHeaderData: Boolean;
        function LoadFrame(TagStream: TStream): Boolean;
        function ValidFrameID(FrameID: TFrameID): Boolean;
        procedure LoadFrameData(TagStream: TStream; FrameIndex: Integer);
        procedure CompactFrameList;
        function WriteAllFrames(var TagStream: TStream): Integer;
        function WriteAllHeaders(var TagStream: TStream): Integer;
        function WritePadding(var TagStream: TStream; PaddingSize: Integer): Integer;
    public
        FileName: String;
        Loaded: Boolean;
        MajorVersion: Byte;
        MinorVersion: Byte;
        Flags: Byte;
        Unsynchronised: Boolean;
        ExtendedHeader: Boolean;
        Experimental: Boolean;
        FooterPresent: Boolean;
        Size: Cardinal;
        Frames: Array of TID3v2Frame;
        FrameCount: Integer;
        ExtendedHeader3: TID3v2ExtendedHeader3;
        ExtendedHeader4: TID3v2ExtendedHeader4;
        PaddingSize: Cardinal;
        PaddingToWrite: Cardinal;
        Constructor Create;
        Destructor Destroy; override;
        function LoadFromFile(FileName: String): Integer;
        function LoadFromStream(TagStream: TStream): Integer;
        function SaveToFile(FileName: String): Integer;
        function SaveToStream(var TagStream: TStream; PaddingSizeToWrite: Integer = 0): Integer;
        function AddFrame(FrameID: TFrameID): Integer; overload;
        function AddFrame(FrameID: AnsiString): Integer; overload;
        function DeleteFrame(FrameIndex: Integer): Boolean;
        procedure DeleteAllFrames;
        procedure Clear;
        function RemoveUnsynchronisationOnAllFrames: Boolean;
        function ApplyUnsynchronisationOnAllFrames: Boolean;
        function FrameExists(FrameID: TFrameID): Integer; overload;
        function FrameExists(FrameID: AnsiString): Integer; overload;
        function FrameTypeCount(FrameID: TFrameID): Integer;
        function CalculateTotalFramesSize: Integer;
        function CalculateTagSize(PaddingSize: Integer): Integer;
        function FullFrameSize(FrameIndex: Cardinal): Cardinal;
        function CalculateTagCRC32: Cardinal;
        function GetUnicodeText(FrameIndex: Integer): String; overload;
        function GetUnicodeText(FrameID: AnsiString): String; overload;
        function SetUnicodeText(FrameIndex: Integer; Text: String): Boolean; overload;
        function SetUnicodeText(FrameID: AnsiString; Text: String): Boolean; overload;
        function GetUnicodeContent(FrameIndex: Integer; var LanguageID: TLanguageID; var Description: String): String; overload;
        function GetUnicodeContent(FrameID: AnsiString; var LanguageID: TLanguageID; var Description: String): String; overload;
        function SetUnicodeContent(FrameIndex: Integer; Content: String; LanguageID: TLanguageID; Description: String): Boolean; overload;
        function SetUnicodeContent(FrameID: AnsiString; Content: String; LanguageID: TLanguageID; Description: String): Boolean; overload;
        function GetUnicodeComment(FrameIndex: Integer; var LanguageID: TLanguageID; var Description: String): String; overload;
        function GetUnicodeComment(FrameID: AnsiString; var LanguageID: TLanguageID; var Description: String): String; overload;
        function SetUnicodeComment(FrameIndex: Integer; Comment: String; LanguageID: TLanguageID; Description: String): Boolean; overload;
        function SetUnicodeComment(FrameID: AnsiString; Comment: String; LanguageID: TLanguageID; Description: String): Boolean; overload;
        function GetUnicodeLyrics(FrameIndex: Integer; var LanguageID: TLanguageID; var Description: String): String; overload;
        function GetUnicodeLyrics(FrameID: AnsiString; var LanguageID: TLanguageID; var Description: String): String; overload;
        function SetUnicodeLyrics(FrameIndex: Integer; Lyrics: String; LanguageID: TLanguageID; Description: String): Boolean; overload;
        function SetUnicodeLyrics(FrameID: AnsiString; Lyrics: String; LanguageID: TLanguageID; Description: String): Boolean; overload;
        function GetUnicodeCoverPictureStream(FrameIndex: Integer; var PictureStream: TStream; var MIMEType: AnsiString; var Description: String; var CoverType: Integer): Boolean; overload;
        function GetUnicodeCoverPictureStream(FrameID: AnsiString; var PictureStream: TStream; var MIMEType: AnsiString; var Description: String; var CoverType: Integer): Boolean; overload;
        function SetUnicodeCoverPictureFromStream(FrameIndex: Integer; Description: String; PictureStream: TStream; MIMEType: AnsiString; CoverType: Integer): Boolean; overload;
        function SetUnicodeCoverPictureFromStream(FrameID: AnsiString; Description: String; PictureStream: TStream; MIMEType: AnsiString; CoverType: Integer): Boolean; overload;
        function SetUnicodeCoverPictureFromFile(FrameIndex: Integer; Description: String; PictureFileName: String; MIMEType: AnsiString; CoverType: Integer): Boolean; overload;
        function SetUnicodeCoverPictureFromFile(FrameID: AnsiString; Description: String; PictureFileName: String; MIMEType: AnsiString; CoverType: Integer): Boolean; overload;
        function GetUnicodeURL(FrameIndex: Integer; var Description: String): AnsiString; overload;
        function GetUnicodeURL(FrameID: AnsiString; var Description: String): AnsiString; overload;
        function SetUnicodeURL(FrameIndex: Integer; URL: AnsiString; Description: String): Boolean; overload;
        function SetUnicodeURL(FrameID: AnsiString; URL: AnsiString; Description: String): Boolean; overload;
        function GetRecordingTime(FrameIndex: Integer): TDateTime; overload;
        function GetRecordingTime(FrameID: AnsiString): TDateTime; overload;
        function SetRecordingTime(FrameIndex: Integer; DateTime: TDateTime): Boolean; overload;
        function SetRecordingTime(FrameID: AnsiString; DateTime: TDateTime): Boolean; overload;
        function GetSEBR(FrameIndex: Integer): Extended; overload;
        function GetSEBR(FrameID: AnsiString): Extended; overload;
        function SetSEBR(FrameIndex: Integer; BitRate: Extended): Boolean; overload;
        function SetSEBR(FrameID: AnsiString; BitRate: Extended): Boolean; overload;
        function GetSampleCache(FrameIndex: Integer; ForceDecompression: Boolean; var Version: Byte; var Channels: Integer): TID3v2SampleCache;
        function SetSampleCache(FrameIndex: Integer; SESC: TID3v2SampleCache; Channels: Integer): Boolean;
        function GetSEFC(FrameIndex: Integer): Int64;
        function SetSEFC(FrameIndex: Integer; SEFC: Int64): Boolean;
        function SetAlbumColors(FrameIndex: Integer; TitleColor, TextColor: Cardinal): Boolean; overload;
        function SetAlbumColors(FrameID: AnsiString; TitleColor, TextColor: Cardinal): Boolean; overload;
        function GetAlbumColors(FrameIndex: Integer; var TitleColor, TextColor: Cardinal): Boolean; overload;
        function GetAlbumColors(FrameID: AnsiString; var TitleColor, TextColor: Cardinal): Boolean; overload;
        function SetTLEN(FrameIndex: Integer; TLEN: Integer): Boolean; overload;
        function SetTLEN(FrameID: AnsiString; TLEN: Integer): Boolean; overload;
        function GetPlayCount(FrameIndex: Integer): Cardinal; overload;
        function GetPlayCount(FrameID: AnsiString): Cardinal; overload;
        function SetPlayCount(FrameIndex: Integer; PlayCount: Cardinal): Boolean; overload;
        function SetPlayCount(FrameID: AnsiString; PlayCount: Cardinal): Boolean; overload;
    end;

// The constants here are for the CRC-32 generator
// polynomial, as defined in the Microsoft
// Systems Journal, March 1995, pp. 107-108
Const
    CRC32Table: array[0..255] of DWORD =
    ($00000000, $77073096, $EE0E612C, $990951BA,
    $076DC419, $706AF48F, $E963A535, $9E6495A3,
    $0EDB8832, $79DCB8A4, $E0D5E91E, $97D2D988,
    $09B64C2B, $7EB17CBD, $E7B82D07, $90BF1D91,
    $1DB71064, $6AB020F2, $F3B97148, $84BE41DE,
    $1ADAD47D, $6DDDE4EB, $F4D4B551, $83D385C7,
    $136C9856, $646BA8C0, $FD62F97A, $8A65C9EC,
    $14015C4F, $63066CD9, $FA0F3D63, $8D080DF5,
    $3B6E20C8, $4C69105E, $D56041E4, $A2677172,
    $3C03E4D1, $4B04D447, $D20D85FD, $A50AB56B,
    $35B5A8FA, $42B2986C, $DBBBC9D6, $ACBCF940,
    $32D86CE3, $45DF5C75, $DCD60DCF, $ABD13D59,
    $26D930AC, $51DE003A, $C8D75180, $BFD06116,
    $21B4F4B5, $56B3C423, $CFBA9599, $B8BDA50F,
    $2802B89E, $5F058808, $C60CD9B2, $B10BE924,
    $2F6F7C87, $58684C11, $C1611DAB, $B6662D3D,

    $76DC4190, $01DB7106, $98D220BC, $EFD5102A,
    $71B18589, $06B6B51F, $9FBFE4A5, $E8B8D433,
    $7807C9A2, $0F00F934, $9609A88E, $E10E9818,
    $7F6A0DBB, $086D3D2D, $91646C97, $E6635C01,
    $6B6B51F4, $1C6C6162, $856530D8, $F262004E,
    $6C0695ED, $1B01A57B, $8208F4C1, $F50FC457,
    $65B0D9C6, $12B7E950, $8BBEB8EA, $FCB9887C,
    $62DD1DDF, $15DA2D49, $8CD37CF3, $FBD44C65,
    $4DB26158, $3AB551CE, $A3BC0074, $D4BB30E2,
    $4ADFA541, $3DD895D7, $A4D1C46D, $D3D6F4FB,
    $4369E96A, $346ED9FC, $AD678846, $DA60B8D0,
    $44042D73, $33031DE5, $AA0A4C5F, $DD0D7CC9,
    $5005713C, $270241AA, $BE0B1010, $C90C2086,
    $5768B525, $206F85B3, $B966D409, $CE61E49F,
    $5EDEF90E, $29D9C998, $B0D09822, $C7D7A8B4,
    $59B33D17, $2EB40D81, $B7BD5C3B, $C0BA6CAD,

    $EDB88320, $9ABFB3B6, $03B6E20C, $74B1D29A,
    $EAD54739, $9DD277AF, $04DB2615, $73DC1683,
    $E3630B12, $94643B84, $0D6D6A3E, $7A6A5AA8,
    $E40ECF0B, $9309FF9D, $0A00AE27, $7D079EB1,
    $F00F9344, $8708A3D2, $1E01F268, $6906C2FE,
    $F762575D, $806567CB, $196C3671, $6E6B06E7,
    $FED41B76, $89D32BE0, $10DA7A5A, $67DD4ACC,
    $F9B9DF6F, $8EBEEFF9, $17B7BE43, $60B08ED5,
    $D6D6A3E8, $A1D1937E, $38D8C2C4, $4FDFF252,
    $D1BB67F1, $A6BC5767, $3FB506DD, $48B2364B,
    $D80D2BDA, $AF0A1B4C, $36034AF6, $41047A60,
    $DF60EFC3, $A867DF55, $316E8EEF, $4669BE79,
    $CB61B38C, $BC66831A, $256FD2A0, $5268E236,
    $CC0C7795, $BB0B4703, $220216B9, $5505262F,
    $C5BA3BBE, $B2BD0B28, $2BB45A92, $5CB36A04,
    $C2D7FFA7, $B5D0CF31, $2CD99E8B, $5BDEAE1D,

    $9B64C2B0, $EC63F226, $756AA39C, $026D930A,
    $9C0906A9, $EB0E363F, $72076785, $05005713,
    $95BF4A82, $E2B87A14, $7BB12BAE, $0CB61B38,
    $92D28E9B, $E5D5BE0D, $7CDCEFB7, $0BDBDF21,
    $86D3D2D4, $F1D4E242, $68DDB3F8, $1FDA836E,
    $81BE16CD, $F6B9265B, $6FB077E1, $18B74777,
    $88085AE6, $FF0F6A70, $66063BCA, $11010B5C,
    $8F659EFF, $F862AE69, $616BFFD3, $166CCF45,
    $A00AE278, $D70DD2EE, $4E048354, $3903B3C2,
    $A7672661, $D06016F7, $4969474D, $3E6E77DB,
    $AED16A4A, $D9D65ADC, $40DF0B66, $37D83BF0,
    $A9BCAE53, $DEBB9EC5, $47B2CF7F, $30B5FFE9,
    $BDBDF21C, $CABAC28A, $53B39330, $24B4A3A6,
    $BAD03605, $CDD70693, $54DE5729, $23D967BF,
    $B3667A2E, $C4614AB8, $5D681B02, $2A6F2B94,
    $B40BBE37, $C30C8EA1, $5A05DF1B, $2D02EF8D);

    procedure UnSyncSafe(var Source; const SourceSize: Integer; var Dest: Cardinal);
    procedure SyncSafe(Source: Cardinal; var Dest; const DestSize: Integer);

    function Min(const B1, B2: Integer): Integer; overload;

    function ReverseBytes(Value: Cardinal): Cardinal; overload;
    function Swap16(ASmallInt: SmallInt): SmallInt; register;

    function RemoveUnsynchronisationScheme(Source, Dest: TStream; BytesToRead: Integer): Boolean;
    function ApplyUnsynchronisationScheme(Source, Dest: TStream; BytesToRead: Integer): Boolean;

    function RemoveUnsynchronisationOnStream(Stream: TMemoryStream): Boolean;
    function ApplyUnsynchronisationOnStream(Stream: TMemoryStream): Boolean;

    function ID3v2TDRCEncodeTime(DateTime: TDateTime): String;
    function ID3v2TDRCDecodeTime(DateTime: String): TDateTime;

    function LanguageIDtoString(LangageId : TLanguageID): String;
    procedure AnsiStringToPAnsiChar(const Source: AnsiString; Dest: PAnsiChar; const MaxLength: Integer);

    function APICType2Str(PictureType: Integer): String;
    function APICTypeStr2No(PictureType: String): Integer;

    function ID3v2ValidTag(TagStream: TStream): Boolean;
    function ID3v2RemoveTag(FileName: String): Integer;

    procedure CalcCRC32(P: Pointer; ByteCount: DWORD; var CRCValue: DWORD);
    function CalculateStreamCRC32(Stream: TStream; var CRCvalue: DWORD): Boolean;

var
    ID3v2ID: TID3v2ID;

implementation

Uses
    SysUtils,
    //Dialogs,
    ZLib;

Constructor TID3v2ExtendedHeader3.Create;
begin
    inherited;
    Flags := 0;
    Size := 0;
    //SizeData := TMemoryStream.Create;
    Data := TMemoryStream.Create;
end;

Destructor TID3v2ExtendedHeader3.Destroy;
begin
    //FreeAndNil(SizeData);
    FreeAndNil(Data);
    inherited;
end;

procedure TID3v2ExtendedHeader3.DecodeExtendedHeaderSize;
begin
    UnSyncSafe(CodedSize, 4, Size);
end;

procedure TID3v2ExtendedHeader3.DecodeExtendedHeaderFlags;
var
    Bit: Byte;
begin
    Flags := Swap16(Flags);
    Bit := Flags SHR 15;
    CRCPresent := Boolean(Bit);
end;

Constructor TID3v2ExtendedHeader4.Create;
begin
    inherited;
    TagIsAnUpdate := False;
    CRCPresent := False;
    TagRestrictions := False;
    TagRestrictionsData := NoMoreThan128FramesAnd1MBTotalTagSize;
    TextEncodingRestrictions := NoTextEncodingRestrictions;
    TextFieldsSizeRestriction := NoTextFieldsSizeRestrictions;
    ImageEncodingRestriction := NoImageEncodingRestrictions;
    ImageSizeRestriction := NoImageSizeRestrictions;
end;

Destructor TID3v2ExtendedHeader4.Destroy;
begin
    inherited;
end;

procedure TID3v2ExtendedHeader4.DecodeExtendedHeaderSize;
begin
    UnSyncSafe(CodedSize, 4, Size);
end;

procedure TID3v2ExtendedHeader4.DecodeExtendedHeaderFlags;
var
    Bit: Byte;
begin
    Bit := Flags SHL 1;
    Bit := Bit SHR 7;
    TagIsAnUpdate := Boolean(Bit);
    Bit := Flags SHL 2;
    Bit := Bit SHR 7;
    CRCPresent := Boolean(Bit);
    Bit := Flags SHL 3;
    Bit := Bit SHR 7;
    TagRestrictions := Boolean(Bit);
end;

procedure TID3v2ExtendedHeader4.CalculateExtendedFlagsDataSize;
begin
    ExtendedFlagsDataSize := 0;
    if TagIsAnUpdate then begin
        //* No flag data
    end;
    if CRCPresent then begin
        ExtendedFlagsDataSize := ExtendedFlagsDataSize + 5;
    end;
    if TagRestrictions then begin
        ExtendedFlagsDataSize := ExtendedFlagsDataSize + 1;
    end;
end;

procedure TID3v2ExtendedHeader4.DecodeExtendedHeaderFlagData;
begin
    //* Not yet implemented
end;

Constructor TID3v2Frame.Create;
begin
    Inherited;
    ID := '';
    Flags := 0;
    TagAlterPreservation := False;
    FileAlterPreservation := False;
    ReadOnly := False;
    Compressed := False;
    Encrypted := False;
    GroupingIdentity := False;
    Unsynchronised := False;
    DataLengthIndicator := False;
    Stream := TMemoryStream.Create;
    Unsynchronised := False;
    DataLengthIndicatorValue := 0;
    GroupIdentifier := 0;
    EncryptionMethod := 0;
end;

Destructor TID3v2Frame.Destroy;
begin
    ID := #0#0#0#0;
    FreeAndNil(Stream);
    Inherited;
end;

procedure TID3v2Frame.DecodeFlags3;
var
    Bit: Word;
begin
    Bit := Flags SHR 15;
    TagAlterPreservation := Boolean(Bit);
    Bit := Flags SHL 1;
    Bit := Bit SHR 15;
    FileAlterPreservation := Boolean(Bit);
    Bit := Flags SHL 2;
    Bit := Bit SHR 15;
    ReadOnly := Boolean(Bit);
    Bit := Flags SHL 8;
    Bit := Bit SHR 15;
    Compressed := Boolean(Bit);
    Bit := Flags SHL 9;
    Bit := Bit SHR 15;
    Encrypted := Boolean(Bit);
    Bit := Flags SHL 10;
    Bit := Bit SHR 15;
    GroupingIdentity := Boolean(Bit);
end;

procedure TID3v2Frame.EncodeFlags3;
var
    EncodedFlags: Word;
    Bit: Word;
begin
    EncodedFlags := 0;
    if TagAlterPreservation then begin
        Bit := 1 SHL 7;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if FileAlterPreservation then begin
        Bit := 1 SHL 6;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if ReadOnly then begin
        Bit := 1 SHL 5;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if Compressed then begin
        Bit := 1 SHL 15;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if Encrypted then begin
        Bit := 1 SHL 14;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if GroupingIdentity then begin
        Bit := 1 SHL 13;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    Flags := EncodedFlags;
end;

procedure TID3v2Frame.DecodeFlags4;
var
    Bit: Word;
begin
    Bit := Flags SHR 14;
    TagAlterPreservation := Boolean(Bit);
    Bit := Flags SHL 1;
    Bit := Bit SHR 14;
    FileAlterPreservation := Boolean(Bit);
    Bit := Flags SHL 2;
    Bit := Bit SHR 14;
    ReadOnly := Boolean(Bit);
    Bit := Flags SHL 9;
    Bit := Bit SHR 15;
    GroupingIdentity := Boolean(Bit);
    Bit := Flags SHL 12;
    Bit := Bit SHR 15;
    Compressed := Boolean(Bit);
    Bit := Flags SHL 13;
    Bit := Bit SHR 15;
    Encrypted := Boolean(Bit);
    Bit := Flags SHL 14;
    Bit := Bit SHR 15;
    Unsynchronised := Unsynchronised OR Boolean(Bit);
    Bit := Flags SHL 15;
    Bit := Bit SHR 15;
    DataLengthIndicator := Boolean(Bit);
end;

procedure TID3v2Frame.EncodeFlags4;
var
    EncodedFlags: Word;
    Bit: Word;
begin
    EncodedFlags := 0;
    if TagAlterPreservation then begin
        Bit := 1 SHL 14;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if FileAlterPreservation then begin
        Bit := 1 SHL 13;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if ReadOnly then begin
        Bit := 1 SHL 12;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if GroupingIdentity then begin
        Bit := 1 SHL 6;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if Compressed then begin
        Bit := 1 SHL 3;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if Encrypted then begin
        Bit := 1 SHL 2;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if Unsynchronised then begin
        Bit := 1 SHL 1;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if DataLengthIndicator then begin
        Bit := 1;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    Flags := EncodedFlags;
end;

function TID3v2Frame.Compress: Boolean;
var
    CompressionStream: TZCompressionStream;
    CompressedStream: TStream;
    UnCompressedSize: DWord;
    SyncSafeSize: DWord;
begin
    Result := False;
    if Stream.Size = 0 then begin
        Exit;
    end;
    CompressionStream := nil;
    CompressedStream := nil;
    try
        try
            CompressedStream := TMemoryStream.Create;
            CompressionStream := TZCompressionStream.Create(CompressedStream, zcMax);
            Stream.Seek(0, soBeginning);
            CompressionStream.CopyFrom(Stream, Stream.Size);
            //* Needed to flush the buffer
            FreeAndNil(CompressionStream);
            if CompressedStream.Size > 0 then begin
                UnCompressedSize := Stream.Size;
                SyncSafe(UnCompressedSize, SyncSafeSize, 4);
                Stream.Clear;
                //Stream.Write(SyncSafeSize, 4);
                DataLengthIndicatorValue := SyncSafeSize;
                CompressedStream.Seek(0, soBeginning);
                Stream.CopyFrom(CompressedStream, CompressedStream.Size);
                Compressed := True;
                DataLengthIndicator := True;
                Result := True;
            end;
        except
            //*
        end;
    finally
        if Assigned(CompressionStream) then begin
            FreeAndNil(CompressionStream);
        end;
        if Assigned(CompressedStream) then begin
            FreeAndNil(CompressedStream);
        end;
    end;
end;

function TID3v2Frame.DeCompress: Boolean;
var
    DeCompressionStream: TZDeCompressionStream;
    UnCompressedStream: TMemoryStream;
begin
    Result := False;
    if Stream.Size <= 4 then begin
        Exit;
    end;
    DeCompressionStream := nil;
    UnCompressedStream := nil;
    try
        try
            UnCompressedStream := TMemoryStream.Create;
            Stream.Seek(0, soBeginning);
            DeCompressionStream := TZDeCompressionStream.Create(Stream);
            UnCompressedStream.CopyFrom(DeCompressionStream, 0);
            Stream.Clear;
            Stream.CopyFrom(UnCompressedStream, 0);
            Stream.Seek(0, soBeginning);
            Compressed := False;
            DataLengthIndicator := False;
            Result := True;
        except
            //*
        end;
    finally
        if Assigned(DeCompressionStream) then begin
            FreeAndNil(DeCompressionStream);
        end;
        if Assigned(UnCompressedStream) then begin
            FreeAndNil(UnCompressedStream);
        end;
    end;
end;

function TID3v2Frame.RemoveUnsynchronisation: Boolean;
begin
    Result := RemoveUnsynchronisationOnStream(Stream);
    if Result then begin
        Unsynchronised := False;
    end;
end;

function TID3v2Frame.ApplyUnsynchronisation: Boolean;
begin
    Result := ApplyUnsynchronisationOnStream(Stream);
    if Result then begin
        Unsynchronised := True;
    end;
end;

Constructor TID3v2Tag.Create;
begin
    Inherited;
    ExtendedHeader3 := TID3v2ExtendedHeader3.Create;
    ExtendedHeader4 := TID3v2ExtendedHeader4.Create;
    Clear;
end;

Destructor TID3v2Tag.Destroy;
begin
    Clear;
    FreeAndNil(ExtendedHeader3);
    FreeAndNil(ExtendedHeader4);
    Inherited;
end;

procedure TID3v2Tag.DeleteAllFrames;
var
    i: Integer;
begin
    for i := 0 to Length(Frames) - 1 do begin
        FreeAndNil(Frames[i]);
    end;
    SetLength(Frames, 0);
    FrameCount := 0;
end;

function TID3v2Tag.LoadFromStream(TagStream: TStream): Integer;
var
    ValidFrameLoaded: Boolean;
begin
    Result := ID3V2LIBRARY_ERROR;
    Loaded := False;
    Clear;
    if ID3v2ValidTag(TagStream) then begin
        try
            TagStream.Read(MajorVersion, 1);
            TagStream.Read(MinorVersion, 1);
        except
            Exit;
        end;
        if (MajorVersion > 4)
        OR (MajorVersion < 3)
        then begin
            Result := ID3V2LIBRARY_ERROR_NOT_SUPPORTED_VERSION;
            Exit;
        end;
        try
            TagStream.Read(Flags, 1);
            DecodeFlags;
        except
            Exit;
        end;
        try
            TagStream.Read(CodedSize, 4);
            DecodeSize;
        except
            Exit;
        end;
        if ExtendedHeader then begin
            //Showmessage('Extended header found!');
            ReadExtendedHeader(TagStream);
        end;
        repeat
            ValidFrameLoaded := LoadFrame(TagStream);
            //* TODO seek back 3 bytes for compatibility for corrupt tags and try again
        until NOT ValidFrameLoaded;
        Result := ID3V2LIBRARY_SUCCESS;
        Loaded := True;
    end;
end;

function TID3v2Tag.LoadFromFile(FileName: String): Integer;
var
    FileStream: TFileStream;
begin
    Result := ID3V2LIBRARY_ERROR;
    Clear;
    Loaded := False;
    if NOT FileExists(FileName) then begin
        Result := ID3V2LIBRARY_ERROR_OPENING_FILE;
        Exit;
    end;
    try
        FileStream := TFileStream.Create(FileName, fmOpenRead OR fmShareDenyWrite);
    except
        Result := ID3V2LIBRARY_ERROR_OPENING_FILE;
        Exit;
    end;
    try
        Result := LoadFromStream(FileStream);
        if (Result = ID3V2LIBRARY_SUCCESS)
        OR (Result = ID3V2LIBRARY_ERROR_NOT_SUPPORTED_VERSION)
        then begin
            Self.FileName := FileName;
        end;
    finally
        FreeAndNil(FileStream);
    end;
end;

procedure TID3v2Tag.DecodeFlags;
var
    Bit: Byte;
begin
    Bit := Flags SHR 7;
    Unsynchronised := Boolean(Bit);
    Bit := Flags SHL 1;
    Bit := Bit SHR 7;
    ExtendedHeader := Boolean(Bit);
    Bit := Flags SHL 2;
    Bit := Bit SHR 7;
    Experimental := Boolean(Bit);
    Bit := Flags SHL 3;
    Bit := Bit SHR 7;
    FooterPresent := Boolean(Bit);
end;

procedure TID3v2Tag.EncodeFlags;
var
    EncodedFlags: Byte;
    Bit: Byte;
begin
    EncodedFlags := 0;
    if Unsynchronised then begin
        Bit := 1 SHL 7;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if ExtendedHeader then begin
        //* Extended header writing is not supported
        //Bit := 1 SHL 6;
        //EncodedFlags := EncodedFlags OR Bit;
    end;
    if Experimental then begin
        Bit := 1 SHL 5;
        EncodedFlags := EncodedFlags OR Bit;
    end;
    if FooterPresent then begin
        //* Footer writing is not supported
        //Bit := 1 SHL 6;
        //EncodedFlags := EncodedFlags OR Bit;
    end;
    Flags := EncodedFlags;
end;

procedure TID3v2Tag.DecodeSize;
begin
    UnSyncSafe(CodedSize, 4, Size);
end;

function TID3v2Tag.ReadExtendedHeader(TagStream: TStream): Boolean;
var
    ExtendedFrameID: TFrameID;
begin
    Result := False;
    try
        TagStream.Read(ExtendedFrameID[0], 4);
        //* Support for bad Tags that report an extended header but don't have one
        if NOT ValidFrameID(ExtendedFrameID) then begin
            TagStream.Seek(-4, soCurrent);
            //* v3
            if MajorVersion = 3 then begin
                with ExtendedHeader3 do begin
                    //* If extended header is unsynchronised needed to remove it
                    //SizeData.CopyFrom(TagStream, 4);
                    //if Unsynchronised then begin
                    //    RemoveUnsynchronisationOnExtendedHeaderSize;
                    //end;
                    //SizeData.Seek(0, soBeginning);
                    //SizeData.Read(CodedExtendedHeaderSize3, 4);
                    //SizeData.Seek(0, soBeginning);
                    TagStream.Read(CodedSize, 4);
                    DecodeExtendedHeaderSize;

                    //* Read extended header flags
                    TagStream.Read(ExtendedHeader3.Flags, 2);
                    DecodeExtendedHeaderFlags;

                    Data.CopyFrom(TagStream, Size - 2);
                    if Unsynchronised then begin
                        RemoveUnsynchronisationOnExtendedHeaderData;
                    end;
                end;
            end;
            //* v4
            if MajorVersion = 4 then begin
                with ExtendedHeader4 do begin
                    TagStream.Read(CodedSize, 4);
                    DecodeExtendedHeaderSize;
                    TagStream.Read(FlagBytes, 1);
                    TagStream.Read(Flags, 1);
                    DecodeExtendedHeaderFlags;
                    CalculateExtendedFlagsDataSize;
                    SetLength(ExtendedFlagsData, ExtendedFlagsDataSize);
                    TagStream.Read(ExtendedFlagsData[0], ExtendedFlagsDataSize);
                    DecodeExtendedHeaderFlagData;
                end;
            end;
        end else begin
            ExtendedHeader := False;
            TagStream.Seek(-4, soCurrent);
        end;
        Result := True;
    except
        Result := False;
    end;
end;

procedure UnSyncSafe(var Source; const SourceSize: Integer; var Dest: Cardinal);
type
    TBytes = array [0..MaxInt - 1] of Byte;
var
    I: Byte;
begin
    { Test : Source = $01 $80 -> Dest = 255
             Source = $02 $00 -> Dest = 256
             Source = $02 $01 -> Dest = 257 etc.
    }
    Dest := 0;
    for I := 0 to SourceSize - 1 do begin
        Dest := Dest shl 7;
        Dest := Dest or (TBytes(Source)[I] and $7F); // $7F = %01111111
    end;
end;

procedure SyncSafe(Source: Cardinal; var Dest; const DestSize: Integer);
type
    TBytes = array [0..MaxInt - 1] of Byte;
var
    I: Byte;
begin
    { Test : Source = 255 -> Dest = $01 $80
             Source = 256 -> Dest = $02 $00
             Source = 257 -> Dest = $02 $01 etc.
    }
    for I := DestSize - 1 downto 0 do begin
        TBytes(Dest)[I] := Source and $7F; // $7F = %01111111
        Source := Source shr 7;
    end;
end;

function TID3v2Tag.LoadFrame(TagStream: TStream): Boolean;
var
    FrameID: TFrameID;
    FrameIndex: Integer;
    ValidFrame: Boolean;
begin
    Result := False;
    FrameID := #0#0#0#0;
    try
        TagStream.Read(FrameID[0], 4);
        ValidFrame := ValidFrameID(FrameID);
        //* Workaround for buggy DataLengthIndicator
        if NOT ValidFrame then begin
            TagStream.Read(FrameID[0], 4);
            ValidFrame := ValidFrameID(FrameID);
        end;
        if ValidFrame then begin
            FrameIndex := AddFrame(FrameID);
            if FrameIndex > -1 then begin
                LoadFrameData(TagStream, FrameIndex);
                Result := True;
            end;
        end;
    except

    end;
end;

function TID3v2Tag.ValidFrameID(FrameID: TFrameID): Boolean;
var
    FrameIDChar: AnsiChar;
    i: Integer;
begin
    Result := True;
    for i := 0 to 3 do begin
        FrameIDChar := FrameID[i];
        if NOT (FrameIDChar in ['A'..'Z'] + ['0'..'9']) then begin
            Result := False;
            Break;
        end;
    end;
end;

procedure TID3v2Tag.LoadFrameData(TagStream: TStream; FrameIndex: Integer);
var
    Size: DWord;
    Flags: Word;
    ReversedFlags: Cardinal;
    DecodedSize: Cardinal;
    CopySize: Cardinal;
    DataLengthIndicatorValueCoded: Cardinal;
    ActualFrameSize: Cardinal;
begin
    try
        TagStream.Read(Size, 4);
        TagStream.Read(Flags, 2);
        if MajorVersion = 3 then begin
            Frames[FrameIndex].Size := ReverseBytes(Size);
            Frames[FrameIndex].Flags := Swap16(Flags);
            Frames[FrameIndex].DecodeFlags3;
            Frames[FrameIndex].Unsynchronised := Unsynchronised;
            if Frames[FrameIndex].Compressed then begin
                TagStream.Read(DataLengthIndicatorValueCoded, 4);
                UnSyncSafe(DataLengthIndicatorValueCoded, 4, Frames[FrameIndex].DataLengthIndicatorValue);
                Frames[FrameIndex].DataLengthIndicator := True;
                Frames[FrameIndex].Stream.CopyFrom(TagStream, Frames[FrameIndex].Size - 4);
            end else begin
                Frames[FrameIndex].Stream.CopyFrom(TagStream, Frames[FrameIndex].Size);
            end;
        end;
        if MajorVersion > 3 then begin
            UnSyncSafe(Size, 4, Frames[FrameIndex].Size);
            ActualFrameSize := Frames[FrameIndex].Size;
            Frames[FrameIndex].Flags := Swap16(Flags);
            Frames[FrameIndex].DecodeFlags4;
            if Frames[FrameIndex].GroupingIdentity then begin
                TagStream.Read(Frames[FrameIndex].GroupIdentifier, 1);
                ActualFrameSize := ActualFrameSize - 1;
            end;
            if Frames[FrameIndex].Encrypted then begin
                TagStream.Read(Frames[FrameIndex].EncryptionMethod, 1);
                ActualFrameSize := ActualFrameSize - 1;
            end;
            if Frames[FrameIndex].DataLengthIndicator then begin
                TagStream.Read(DataLengthIndicatorValueCoded, 4);
                UnSyncSafe(DataLengthIndicatorValueCoded, 4, Frames[FrameIndex].DataLengthIndicatorValue);
                ActualFrameSize := ActualFrameSize - 4;
            end;
            Frames[FrameIndex].Stream.CopyFrom(TagStream, ActualFrameSize);
        end;
    except
        //*
    end;
end;

function TID3v2Tag.AddFrame(FrameID: TFrameID): Integer;
begin
    Result := -1;
    try
        SetLength(Frames, Length(Frames) + 1);
        Frames[Length(Frames) - 1] := TID3v2Frame.Create;
        Frames[Length(Frames) - 1].ID := FrameID;
        Result := Length(Frames) - 1;
        Inc(FrameCount);
    except
        //*
    end;
end;

function TID3v2Tag.AddFrame(FrameID: AnsiString): Integer;
var
    ID: TFrameID;
begin
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Result := AddFrame(ID);
end;

function TID3v2Tag.DeleteFrame(FrameIndex: Integer): Boolean;
begin
    Result := False;
    if (FrameIndex >= Length(Frames))
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    FreeAndNil(Frames[FrameIndex]);
    CompactFrameList;
    Dec(FrameCount);
    Result := True;
end;

procedure TID3v2Tag.CompactFrameList;
var
    i: Integer;
    Compacted: Boolean;
begin
    Compacted := False;
    if Frames[FrameCount - 1]  = nil then begin
        Compacted := True;
    end else begin
        for i := 0 to FrameCount - 2 do begin
            if Frames[i] = nil then begin
                Frames[i] := Frames[i + 1];
                Frames[i + 1] := nil;
                Compacted := True;
            end;
        end;
    end;
    if Compacted then begin
        SetLength(Frames, Length(Frames) - 1);
    end;
end;

function TID3v2Tag.FrameExists(FrameID: TFrameID): Integer;
var
    i: Integer;
begin
    Result := -1;
    for i := 0 to FrameCount - 1 do begin
        if FrameID = Frames[i].ID then begin
            Result := i;
            Break;
        end;
    end;
end;

function TID3v2Tag.FrameExists(FrameID: AnsiString): Integer;
var
    i: Integer;
begin
    Result := -1;
    for i := 0 to FrameCount - 1 do begin
        if FrameID = Frames[i].ID then begin
            Result := i;
            Break;
        end;
    end;
end;

function TID3v2Tag.FrameTypeCount(FrameID: TFrameID): Integer;
var
    i: Integer;
begin
    Result := 0;
    for i := 0 to FrameCount - 1 do begin
        if FrameID = Frames[i].ID then begin
            Inc(Result);
        end;
    end;
end;

function TID3v2Tag.SaveToStream(var TagStream: TStream; PaddingSizeToWrite: Integer = 0): Integer;
var
    UnCodedSize: Cardinal;
begin
    Result := ID3V2LIBRARY_ERROR;
    try
        if (MajorVersion < 3)
        OR (MajorVersion > 4)
        then begin
            Result := ID3V2LIBRARY_ERROR_NOT_SUPPORTED_VERSION;
            Exit;
        end;
        PaddingSize := PaddingSizeToWrite;
        EncodeSize;
        EncodeFlags;
        //* EncodeExtendedHeader;
        Result := WriteAllHeaders(TagStream);
        if Result <> ID3V2LIBRARY_SUCCESS then begin
            Exit;
        end;
        Result := WriteAllFrames(TagStream);
        if Result <> ID3V2LIBRARY_SUCCESS then begin
            Exit;
        end;
        Result := WritePadding(TagStream, PaddingSize);
        if Result <> ID3V2LIBRARY_SUCCESS then begin
            Exit;
        end;
        Result := ID3V2LIBRARY_SUCCESS;
    except
        Result := ID3V2LIBRARY_ERROR;
    end;
end;

function TID3v2Tag.SaveToFile(FileName: String): Integer;
var
    TagStream: TStream;
    NewTagStream: TStream;
    TagSizeInExistingStream: Cardinal;
    TagCodedSizeInExistingStream: Cardinal;
    WriteTagTotalSize: Integer;
    NeedToCopyExistingStream: Boolean;
    PaddingNeededToWrite: Integer;
    NewFile: Boolean;
begin
    Result := ID3V2LIBRARY_ERROR;
    TagStream := nil;
    NewTagStream := nil;
    NewFile := False;
    try
        try
            if FrameCount = 0 then begin
                Result := ID3V2LIBRARY_ERROR_EMPTY_TAG;
                Exit;
            end;
            if CalculateTotalFramesSize = 0 then begin
                Result := ID3V2LIBRARY_ERROR_EMPTY_FRAMES;
                Exit;
            end;
            if NOT FileExists(FileName) then begin
                TagStream := TFileStream.Create(FileName, fmCreate OR fmShareDenyWrite);
            end else begin
                TagStream := TFileStream.Create(FileName, fmOpenReadWrite OR fmShareDenyWrite);
            end;
            NeedToCopyExistingStream := False;
            WriteTagTotalSize := CalculateTagSize(0);
            try
                if ID3v2ValidTag(TagStream) then begin
                    //* Skip version data and flags
                    TagStream.Seek(3, soCurrent);
                    TagStream.Read(TagCodedSizeInExistingStream, 4);
                    UnSyncSafe(TagCodedSizeInExistingStream, 4, TagSizeInExistingStream);
                    //* Add header size to size
                    TagSizeInExistingStream := TagSizeInExistingStream + 10;
                    if WriteTagTotalSize > TagSizeInExistingStream then begin
                        NeedToCopyExistingStream := True;
                        NewFile := True;
                    end;
                    TagStream.Seek(0, soBeginning);
                end else begin
                    TagSizeInExistingStream := 0;
                    NeedToCopyExistingStream := True;
                    NewFile := True;
                end;
            except
                Result := ID3V2LIBRARY_ERROR_READING_FILE;
                Exit;
            end;

            if TagSizeInExistingStream = 0 then begin
                PaddingNeededToWrite := PaddingToWrite;
            end else begin
                //* Calculate padding here
                PaddingNeededToWrite := TagSizeInExistingStream - WriteTagTotalSize;
                if PaddingNeededToWrite < 0 then begin
                    PaddingNeededToWrite := PaddingToWrite;
                end;
            end;

            if NewFile then begin
                NewTagStream := TFileStream.Create(FileName + '.tmp', fmCreate OR fmShareExclusive);
                try
                    Result := SaveToStream(NewTagStream, PaddingNeededToWrite);
                    TagStream.Seek(TagSizeInExistingStream, soBeginning);
                    NewTagStream.CopyFrom(TagStream, TagStream.Size - TagSizeInExistingStream);
                    if Assigned(TagStream) then begin
                        FreeAndNil(TagStream);
                    end;
                    if Assigned(NewTagStream) then begin
                        FreeAndNil(NewTagStream);
                    end;
                    if DeleteFile(FileName) then begin
                        if RenameFile(FileName + '.tmp', FileName) then begin
                            Result := ID3V2LIBRARY_SUCCESS;
                            Exit;
                        end;
                    end;
                except
                    Result := ID3V2LIBRARY_ERROR_WRITING_FILE;
                    Exit;
                end;
            end else begin
                if NeedToCopyExistingStream then begin
                    try
                        Result := SaveToStream(TagStream, PaddingNeededToWrite);
                        if NeedToCopyExistingStream then begin
                            TagStream.Seek(TagSizeInExistingStream, soBeginning);
                            NewTagStream.CopyFrom(TagStream, TagStream.Size - TagSizeInExistingStream);
                        end;
                    except
                        Result := ID3V2LIBRARY_ERROR_WRITING_FILE;
                        Exit;
                    end;
                end else begin
                    try
                        Result := SaveToStream(TagStream, PaddingNeededToWrite);
                        if NeedToCopyExistingStream then begin
                            TagStream.Seek(TagSizeInExistingStream, soBeginning);
                            NewTagStream.CopyFrom(TagStream, TagStream.Size - TagSizeInExistingStream);
                        end;
                    except
                        Result := ID3V2LIBRARY_ERROR_WRITING_FILE;
                        Exit;
                    end;
                end;
            end;

        finally
            if Assigned(TagStream) then begin
                FreeAndNil(TagStream);
            end;
            if Assigned(NewTagStream) then begin
                FreeAndNil(NewTagStream);
            end;
        end;
    except
        Result := ID3V2LIBRARY_ERROR;
    end;
end;

function TID3v2Tag.GetUnicodeText(FrameID: AnsiString): String;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := '';
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetUnicodeText(Index);
end;

function TID3v2Tag.GetUnicodeText(FrameIndex: Integer): String;
var
    AnsiText: AnsiString;
    DataByte: Byte;
    PASCIIText: PANSIChar;
    PUText: PWideChar;
begin
    Result := '';
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        if Frames[FrameIndex].Stream.Size = 0 then begin
            Exit;
        end;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        Frames[FrameIndex].Stream.Read(DataByte, 1);
        case DataByte of
            //* ISO-8859-1
            0: begin
                PASCIIText := AllocMem(Frames[FrameIndex].Stream.Size - 1 + 1);
                Frames[FrameIndex].Stream.Read(PASCIIText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                AnsiText := PASCIIText;
                FreeMem(PASCIIText);
                Result := AnsiText;
            end;
            //* UTF-16
            1: begin
                Frames[FrameIndex].Stream.Read(DataByte, 1);
                if DataByte = $FF then begin
                    Frames[FrameIndex].Stream.Read(DataByte, 1);
                    if DataByte = $FE then begin
                        PUText := AllocMem(Frames[FrameIndex].Stream.Size - 2 + 2);
                        Frames[FrameIndex].Stream.Read(PUText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                        Result := PUText;
                        FreeMem(PUText);
                    end;
                end;
            end;
            //* UTF-16BE
            2: begin
                PUText := AllocMem(Frames[FrameIndex].Stream.Size - 1 + 2);
                Frames[FrameIndex].Stream.Read(PUText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                Result := PUText;
                FreeMem(PUText);
            end;
            //* UTF-8
            3: begin
                PASCIIText := AllocMem(Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position + 1);
                Frames[FrameIndex].Stream.Read(PASCIIText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                PUText := AllocMem((Length(PASCIIText) + 1) * 2);
                Utf8ToUnicode(PUText, Length(PASCIIText) * 2, PANSIChar(PASCIIText), Length(PASCIIText));
                Result := PUText;
                FreeMem(PASCIIText);
                FreeMem(PUText);
            end;
        end;
    except
        //*
    end;
end;

function TID3v2Tag.SetUnicodeText(FrameID: AnsiString; Text: String): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeText(Index, Text);
end;

function TID3v2Tag.SetUnicodeText(FrameIndex: Integer; Text: String): Boolean;
var
    DataByte: Byte;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        DataByte := $01;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        DataByte := $FF;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        DataByte := $FE;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        Frames[FrameIndex].Stream.Write(PWideChar(Text)^, (Length(Text) + 1) * 2);
        Frames[FrameIndex].Stream.Seek(0, soFromBeginning);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.GetUnicodeComment(FrameID: AnsiString; var LanguageID: TLanguageID; var Description: String): String;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := '';
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    LanguageID := '';
    Description := '';
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetUnicodeComment(Index, LanguageID, Description);
end;

function TID3v2Tag.GetUnicodeComment(FrameIndex: Integer; var LanguageID: TLanguageID; var Description: String): String;
begin
    Result := GetUnicodeContent(FrameIndex, LanguageID, Description);
end;

function TID3v2Tag.GetUnicodeContent(FrameID: AnsiString; var LanguageID: TLanguageID; var Description: String): String;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := '';
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    LanguageID := '';
    Description := '';
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetUnicodeComment(Index, LanguageID, Description);
end;

function TID3v2Tag.GetUnicodeContent(FrameIndex: Integer; var LanguageID: TLanguageID; var Description: String): String;
var
    DataByte: Byte;
    UData: Word;
    ASCIIText: PANSIChar;
    StrASCIIDescription: AnsiString;
    StrUDescription: String;
    PUDescription: PWideChar;
    EncodingFormat: Byte;
    UContent: PWideChar;
    StrAnsi: AnsiString;
begin
    Result := '';
    LanguageID := '';
    Description := '';
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        if Frames[FrameIndex].Stream.Size = 0 then begin
            Exit;
        end;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        //* Get encoding format
        Frames[FrameIndex].Stream.Read(EncodingFormat, 1);
        //* Get language ID
        Frames[FrameIndex].Stream.Read(LanguageID[0], 3);
        //* Get decription and content
        case EncodingFormat of
            0: begin
                //* Get description
                StrASCIIDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(DataByte, 1);
                    if DataByte <> $0 then begin
                        StrASCIIDescription := StrASCIIDescription + ANSIChar(DataByte);
                    end;
                until DataByte = 0;
                Description := StrASCIIDescription;
                //* Get the content
                ASCIIText := AllocMem(Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position + 1);
                Frames[FrameIndex].Stream.Read(ASCIIText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                Result := ASCIIText;
                FreeMem(ASCIIText);
            end;
            1: begin
                //* Get description
                StrUDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(UData, 2);
                    if UData <> $0 then begin
                        StrUDescription := StrUDescription + Char(UData);
                    end;
                until UData = 0;
                Description := StrUDescription;
                //* Get the content
                repeat
                    Frames[FrameIndex].Stream.Read(DataByte, 1);
                    if DataByte = $FF then begin
                        Frames[FrameIndex].Stream.Read(DataByte, 1);
                        if DataByte = $FE then begin
                            Break;
                        end;
                    end;
                until (Frames[FrameIndex].Stream.Position >= Frames[FrameIndex].Stream.Size);
                UContent := AllocMem(Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position + 1);
                Frames[FrameIndex].Stream.Read(UContent^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                Result := UContent;
                FreeMem(UContent);
            end;
            2: begin
                //* Get description
                StrUDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(UData, 2);
                    if UData <> $0 then begin
                        StrUDescription := StrUDescription + Char(UData);
                    end;
                until UData = 0;
                //* Get the content
                UContent := AllocMem(Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position + 1);
                Frames[FrameIndex].Stream.Read(UContent^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                Result := UContent;
                FreeMem(UContent);
            end;
            3: begin
                //* Get description
                StrASCIIDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(DataByte, 1);
                    if DataByte <> $0 then begin
                        StrASCIIDescription := StrASCIIDescription + ANSIChar(DataByte);
                    end;
                until DataByte = 0;
                PUDescription := AllocMem((Length(StrASCIIDescription) + 1) * 2);
                Utf8ToUnicode(PUDescription, Length(StrASCIIDescription) * 2, PANSIChar(StrASCIIDescription), Length(StrASCIIDescription));
                Description := PUDescription;
                FreeMem(PUDescription);
                //* Get the content
                ASCIIText := AllocMem(Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position + 2);
                Frames[FrameIndex].Stream.Read(ASCIIText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
                StrAnsi := ASCIIText;
                UContent := AllocMem((Length(StrAnsi) + 1) * 2);
                Utf8ToUnicode(UContent, Length(StrAnsi) * 2, PANSIChar(StrAnsi), Length(StrAnsi));
                Result := UContent;
                FreeMem(ASCIIText);
            end;
        end;
    except
        //*
    end;
end;

function TID3v2Tag.SetUnicodeComment(FrameID: AnsiString; Comment: String; LanguageID: TLanguageID; Description: String): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeComment(Index, Comment, LanguageID, Description);
end;

function TID3v2Tag.SetUnicodeComment(FrameIndex: Integer; Comment: String; LanguageID: TLanguageID; Description: String): Boolean;
begin
    Result := SetUnicodeContent(FrameIndex, Comment, LanguageID, Description);
end;

function TID3v2Tag.SetUnicodeContent(FrameID: AnsiString; Content: String; LanguageID: TLanguageID; Description: String): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeContent(Index, Content, LanguageID, Description);
end;

function TID3v2Tag.SetUnicodeContent(FrameIndex: Integer; Content: String; LanguageID: TLanguageID; Description: String): Boolean;
var
    DataByte: Byte;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        //* Set unicode flag
        DataByte := $01;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        //* Set the language
        Frames[FrameIndex].Stream.Write(LanguageID[0], 3);
        //* Set the description
        Frames[FrameIndex].Stream.Write(PWideChar(Description)^, (Length(Description) + 1) * 2);
        //* Write the content with BOM
        DataByte := $FF;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        DataByte := $FE;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        Frames[FrameIndex].Stream.Write(PWideChar(Content)^, (Length(Content) + 1) * 2);
        Frames[FrameIndex].Stream.Seek(0, soFromBeginning);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.GetUnicodeLyrics(FrameID: AnsiString; var LanguageID: TLanguageID; var Description: String): String;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := '';
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    LanguageID := '';
    Description := '';
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetUnicodeLyrics(Index, LanguageID, Description);
end;

function TID3v2Tag.GetUnicodeLyrics(FrameIndex: Integer; var LanguageID: TLanguageID; var Description: String): String;
begin
    Result := GetUnicodeContent(FrameIndex, LanguageID, Description);
end;

function TID3v2Tag.SetUnicodeLyrics(FrameID: AnsiString; Lyrics: String; LanguageID: TLanguageID; Description: String): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeContent(Index, Lyrics, LanguageID, Description);
end;

function TID3v2Tag.SetUnicodeLyrics(FrameIndex: Integer; Lyrics: String; LanguageID: TLanguageID; Description: String): Boolean;
begin
    Result := SetUnicodeContent(FrameIndex, Lyrics, LanguageID, Description);
end;

function TID3v2Tag.GetUnicodeCoverPictureStream(FrameID: AnsiString; var PictureStream: TStream; var MIMEType: AnsiString; var Description: String; var CoverType: Integer): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    MIMEType := '';
    Description := '';
    CoverType := 0;
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetUnicodeCoverPictureStream(Index, PictureStream, MIMEType, Description, CoverType);
end;

function TID3v2Tag.GetUnicodeCoverPictureStream(FrameIndex: Integer; var PictureStream: TStream; var MIMEType: AnsiString; var Description: String; var CoverType: Integer): Boolean;
var
    StrMimeType: AnsiString;
    Data: Byte;
    TextEncoding: Integer;
    StrASCIIDescription: AnsiString;
    StrUDescription: String;
    UData: Word;
    PUDescription: PWideChar;
begin
    Result := False;
    MIMEType := '';
    Description := '';
    CoverType := 0;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        if Frames[FrameIndex].Stream.Size = 0 then begin
            Exit;
        end;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);

        //* Get text encoding
        Frames[FrameIndex].Stream.Read(Data, 1);
        TextEncoding := Data;

        //* Get MIME type
        StrMimeType := '';
        repeat
            Frames[FrameIndex].Stream.Read(Data, 1);
            if Data <> 0 then begin
                StrMimeType := StrMimeType + ANSIChar(Data);
            end;
        until Data = 0;

        //* Get picture type
        Frames[FrameIndex].Stream.Read(Data, 1);
        CoverType := Data;

        //* Get description
        //* ASCII format ISO-8859-1
        case TextEncoding of
            0: begin
                StrASCIIDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(Data, 1);
                    if Data <> $0 then begin
                        StrASCIIDescription := StrASCIIDescription + ANSIChar(Data);
                    end;
                until Data = 0;
                StrUDescription := StrASCIIDescription;
            end;
            //* Unicode format UTF-16 with BOM
            1: begin
                StrUDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(UData, 2);
                    if UData <> $0 then begin
                        StrUDescription := StrUDescription + Char(UData);
                    end;
                until UData = 0;
            end;
            //* Unicode format UTF-16BE without BOM
            2: begin
                StrUDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(UData, 2);
                    if UData <> $0 then begin
                        StrUDescription := StrUDescription + Char(UData);
                    end;
                until UData = 0;
            end;
            //* Unicode format UTF-8
            3: begin
                StrASCIIDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(Data, 1);
                    if Data <> $0 then begin
                        StrASCIIDescription := StrASCIIDescription + ANSIChar(Data);
                    end;
                until Data = 0;
                PUDescription := AllocMem((Length(StrASCIIDescription) + 1) * 2);
                Utf8ToUnicode(PUDescription, Length(StrASCIIDescription) * 2, PANSIChar(StrASCIIDescription), Length(StrASCIIDescription));
                StrUDescription := PUDescription;
                FreeMem(PUDescription);
            end;
        end;

        //* Get binary picture data
        PictureStream.Seek(0, soBeginning);
        try
            PictureStream.CopyFrom(Frames[FrameIndex].Stream, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
            PictureStream.Seek(0, soFromBeginning);

            //TMemoryStream(PictureStream).SaveToFile('C:\APIC.jpg');

        except

        end;

        //* Set results
        MIMEType := StrMimeType;
        Description := StrUDescription;
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.SetUnicodeCoverPictureFromStream(FrameID: AnsiString; Description: String; PictureStream: TStream; MIMEType: AnsiString; CoverType: Integer): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeCoverPictureFromStream(Index, Description, PictureStream, MIMEType, CoverType);
end;


function TID3v2Tag.SetUnicodeCoverPictureFromStream(FrameIndex: Integer; Description: String; PictureStream: TStream; MIMEType: AnsiString; CoverType: Integer): Boolean;
var
    DataByte: Byte;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        ///* Set data is unicode
        DataByte := $00;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        //* Set the MIME type
        //iTunes¼æÈÝBUGÐÞ¸Ä
        Frames[FrameIndex].Stream.Write(PANSIChar(MIMEType)^, Length(MIMEType) + 1);
        ///* Set pitcure type
        DataByte := CoverType;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        //* Set the description
        //Frames[FrameIndex].Stream.Write(PWideChar(Description)^, (Length(Description) + 1) * 2);
        DataByte := $00;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        //* Set picture data
        PictureStream.Seek(0, soBeginning);
        Frames[FrameIndex].Stream.CopyFrom(PictureStream, PictureStream.Size);
        Frames[FrameIndex].Stream.Seek(0, soFromBeginning);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.SetUnicodeCoverPictureFromFile(FrameID: AnsiString; Description: String; PictureFileName: String; MIMEType: AnsiString; CoverType: Integer): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeCoverPictureFromFile(Index, Description, PictureFileName, MIMEType, CoverType);
end;


function TID3v2Tag.SetUnicodeCoverPictureFromFile(FrameIndex: Integer; Description: String; PictureFileName: String; MIMEType: AnsiString; CoverType: Integer): Boolean;
var
    PictureStream: TFileStream;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        PictureStream := nil;
        try
            PictureStream := TFileStream.Create(PictureFileName, fmOpenRead);
            Result := SetUnicodeCoverPictureFromStream(FrameIndex, Description, PictureStream, MIMEType, CoverType);
        finally
            if Assigned(PictureStream) then begin
                FreeAndNil(PictureStream);
            end;
        end;
    except
        //*
    end;
end;

function Min(const B1, B2: Integer): Integer;
begin
    if B1 < B2 then begin
        Result := B1
    end else begin
        Result := B2;
    end;
end;

function ReverseBytes(Value: Cardinal): Cardinal;
begin
    Result := (Value SHR 24) OR (Value SHL 24) OR ((Value AND $00FF0000) SHR 8) OR ((Value AND $0000FF00) SHL 8);
end;
(*
asm
  {$IFDEF CPU32}
  // --> EAX Value
  // <-- EAX Value
  BSWAP  EAX
  {$ENDIF CPU32}
  {$IFDEF CPU64}
  // --> ECX Value
  // <-- EAX Value
  MOV    EAX, ECX
  BSWAP  EAX
  {$ENDIF CPU64}
end;
*)

function RemoveUnsynchronisationScheme(Source, Dest: TStream; BytesToRead: Integer): Boolean;
const
    MaxBufSize = $F000;
var
    LastWasFF: Boolean;
    BytesRead: Integer;
    SourcePtr, DestPtr: Integer;
    SourceBuf, DestBuf: PANSIChar;
begin
    Result := False;
    { Replace $FF 00 with $FF }
    GetMem(SourceBuf, Min(MaxBufSize, BytesToRead));
    GetMem(DestBuf, Min(MaxBufSize, BytesToRead));
    try
        LastWasFF := False;
        while BytesToRead > 0 do begin
            { Read at max CBufferSize bytes from the stream }
            BytesRead := Source.Read(SourceBuf^, Min(MaxBufSize, BytesToRead));
            //if BytesRead = 0 then
                //ID3Error(RsECouldNotReadData);
            Dec(BytesToRead, BytesRead);
            DestPtr := 0;
            SourcePtr := 0;
            while SourcePtr < BytesRead do begin
                { If previous was $FF and current is $00 then skip.. }
                if not LastWasFF or (SourceBuf[SourcePtr] <> #$00) then begin
                    { ..otherwise copy }
                    DestBuf[DestPtr] := SourceBuf[SourcePtr];
                    Inc(DestPtr);
                end;
                LastWasFF := SourceBuf[SourcePtr] = #$FF;
                Inc(SourcePtr);
            end;
            Dest.Write(DestBuf^, DestPtr);
        end;
        Result := True;
    finally
        FreeMem(DestBuf);
        FreeMem(SourceBuf);
    end;
end;

function ApplyUnsynchronisationScheme(Source, Dest: TStream; BytesToRead: Integer): Boolean;
const
    MaxBufSize = $F000;
var
    LastWasFF: Boolean;
    BytesRead: Integer;
    SourcePtr, DestPtr: Integer;
    SourceBuf, DestBuf: PANSIChar;
begin
    Result := False;
    { Replace $FF 00         with  $FF 00 00
        Replace $FF %111xxxxx  with  $FF 00 %111xxxxx (%11100000 = $E0 = 224 }
    GetMem(SourceBuf, Min(MaxBufSize div 2, BytesToRead));
    GetMem(DestBuf, 2 * Min(MaxBufSize div 2, BytesToRead));
    try
        LastWasFF := False;
        while BytesToRead > 0 do begin
            { Read at max CBufferSize div 2 bytes from the stream }
            BytesRead := Source.Read(SourceBuf^, Min(MaxBufSize div 2, BytesToRead));
            //if BytesRead = 0 then
            //  ID3Error(RsECouldNotReadData);
            Dec(BytesToRead, BytesRead);
            DestPtr := 0;
            SourcePtr := 0;
            while SourcePtr < BytesRead do begin
                { If previous was $FF and current is $00 or >=$E0 then add space.. }
                if LastWasFF and ((SourceBuf[SourcePtr] = #$00) or (Byte(SourceBuf[SourcePtr]) and $E0 > 0)) then begin
                    DestBuf[DestPtr] := #$00;
                    Inc(DestPtr);
                end;
                { Copy }
                DestBuf[DestPtr] := SourceBuf[SourcePtr];
                Inc(DestPtr);
                LastWasFF := SourceBuf[SourcePtr] = #$FF;
                Inc(SourcePtr);
            end;
            Dest.Write(DestBuf^, DestPtr);
        end;
        Result := True;
    finally
        FreeMem(SourceBuf);
        FreeMem(DestBuf);
    end;
end;

function TID3v2Tag.GetUnicodeURL(FrameID: AnsiString; var Description: String): AnsiString;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := '';
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Description := '';
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetUnicodeURL(Index, Description);
end;

function TID3v2Tag.GetUnicodeURL(FrameIndex: Integer; var Description: String): AnsiString;
var
    DataByte: Byte;
    UData: Word;
    ASCIIText: PANSIChar;
    StrASCIIDescription: AnsiString;
    StrUDescription: String;
    PUDescription: PWideChar;
    EncodingFormat: Byte;
begin
    Result := '';
    Description := '';
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        if Frames[FrameIndex].Stream.Size = 0 then begin
            Exit;
        end;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        //* Get encoding format
        Frames[FrameIndex].Stream.Read(EncodingFormat, 1);
        //* Get decription and content
        case EncodingFormat of
            0: begin
                //* Get description
                StrASCIIDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(DataByte, 1);
                    if DataByte <> $0 then begin
                        StrASCIIDescription := StrASCIIDescription + ANSIChar(DataByte);
                    end;
                until DataByte = 0;
                Description := StrASCIIDescription;
            end;
            1: begin
                //* Get description
                StrUDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(UData, 2);
                    if UData <> $0 then begin
                        StrUDescription := StrUDescription + Char(UData);
                    end;
                until UData = 0;
                Description := StrUDescription;
            end;
            2: begin
                //* Get description
                StrUDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(UData, 2);
                    if UData <> $0 then begin
                        StrUDescription := StrUDescription + Char(UData);
                    end;
                until UData = 0;
            end;
            3: begin
                //* Get description
                StrASCIIDescription := '';
                repeat
                    Frames[FrameIndex].Stream.Read(DataByte, 1);
                    if DataByte <> $0 then begin
                        StrASCIIDescription := StrASCIIDescription + ANSIChar(DataByte);
                    end;
                until DataByte = 0;
                PUDescription := AllocMem((Length(StrASCIIDescription) + 1) * 2);
                Utf8ToUnicode(PUDescription, Length(StrASCIIDescription) * 2, PANSIChar(StrASCIIDescription), Length(StrASCIIDescription));
                Description := PUDescription;
                FreeMem(PUDescription);
            end;
        end;
        //* Get the URL
        ASCIIText := AllocMem(Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position + 1);
        Frames[FrameIndex].Stream.Read(ASCIIText^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
        Result := ASCIIText;
        FreeMem(ASCIIText);
    except
        //*
    end;
end;

function TID3v2Tag.SetUnicodeURL(FrameID: AnsiString; URL: AnsiString; Description: String): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetUnicodeURL(Index, URL, Description);
end;

function TID3v2Tag.SetUnicodeURL(FrameIndex: Integer; URL: AnsiString; Description: String): Boolean;
var
    DataByte: Byte;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        //* Set unicode flag
        DataByte := $01;
        Frames[FrameIndex].Stream.Write(DataByte, 1);
        //* Set the description
        Frames[FrameIndex].Stream.Write(PWideChar(Description)^, (Length(Description) + 1) * 2);
        //* Write the URL
        Frames[FrameIndex].Stream.Write(PANSIChar(URL)^, (Length(URL)));
        Frames[FrameIndex].Stream.Seek(0, soFromBeginning);
        Result := True;
    except
        //*
    end;
end;

function ID3v2TDRCEncodeTime(DateTime: TDateTime): String;
var
    Year: Word;
    Month: Word;
    Day: Word;
    Hour: Word;
    Minute: Word;
    Second: Word;
    MSec: Word;
    StrYear: String;
    StrMonth: String;
    StrDay: String;
    StrHour: String;
    StrMinute: String;
    StrSecond: String;
begin
    DecodeTime(DateTime, Hour, Minute, Second, MSec);
    DecodeDate(DateTime, Year, Month, Day);
    StrYear := IntToStr(Year);
    if Length(StrYear) = 1 then begin
        StrYear := '0' + StrYear;
    end;
    StrMonth := IntToStr(Month);
    if Length(StrMonth) = 1 then begin
        StrMonth := '0' + StrMonth;
    end;
    StrDay := IntToStr(Day);
    if Length(StrDay) = 1 then begin
        StrDay := '0' + StrDay;
    end;
    StrHour := IntToStr(Hour);
    if Length(StrHour) = 1 then begin
        StrHour := '0' + StrHour;
    end;
    StrMinute := IntToStr(Minute);
    if Length(StrMinute) = 1 then begin
        StrMinute := '0' + StrMinute;
    end;
    StrSecond := IntToStr(Second);
    if Length(StrSecond) = 1 then begin
        StrSecond := '0' + StrSecond;
    end;
    //* yyyy-MM-ddTHH:mm:ss
    Result := StrYear + '-' + StrMonth + '-' + StrDay + 'T' + StrHour + ':' + StrMinute + ':' + StrSecond;
end;

function ID3v2TDRCDecodeTime(DateTime: String): TDateTime;
var
    Year: Word;
    Month: Word;
    Day: Word;
    Hour: Word;
    Minute: Word;
    Second: Word;
    MSec: Word;
    StrYear: String;
    StrMonth: String;
    StrDay: String;
    StrHour: String;
    StrMinute: String;
    StrSecond: String;
    Date: TDateTime;
    Time: TDateTime;
begin
    //* yyyy-MM-ddTHH:mm:ss
    StrYear := Copy(DateTime, 1, 4);
    StrMonth := Copy(DateTime, 6, 2);
    StrDay := Copy(DateTime, 9, 2);
    StrHour := Copy(DateTime, 12, 2);
    StrMinute := Copy(DateTime, 15, 2);
    StrSecond := Copy(DateTime, 18, 2);
    Year := StrToIntDef(StrYear, 0);
    Month := StrToIntDef(StrMonth, 0);
    Day := StrToIntDef(StrDay, 0);
    Hour := StrToIntDef(StrHour, 0);
    Minute := StrToIntDef(StrMinute, 0);
    Second := StrToIntDef(StrSecond, 0);
    MSec := 0;
    if Year = 0 then begin
        Year := 2000;
    end;
    if Month = 0 then begin
        Month := 1;
    end;
    if Day = 0 then begin
        Day := 1;
    end;
    Time := EncodeTime(Hour, Minute, Second, MSec);
    Date := EncodeDate(Year, Month, Day);
    Result := Date + Time;
end;

function TID3v2Tag.GetRecordingTime(FrameID: AnsiString): TDateTime;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := 0;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetRecordingTime(Index);
end;

function TID3v2Tag.GetRecordingTime(FrameIndex: Integer): TDateTime;
var
    TDRCValue: PANSIChar;
    TDRCDateTime: String;
    Data: Byte;
    ReadAmount: Integer;
begin
    Result := 0;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        if Frames[FrameIndex].Stream.Size = 0 then begin
            Exit;
        end;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        ReadAmount := Frames[FrameIndex].Stream.Size;

        Frames[FrameIndex].Stream.Read(Data, 1);
        if Data <> 0 then begin
            Frames[FrameIndex].Stream.Seek(0, soBeginning);
        end else begin
            ReadAmount := Frames[FrameIndex].Stream.Size - 1;
        end;

        TDRCValue := AllocMem(Frames[FrameIndex].Stream.Size);
        Frames[FrameIndex].Stream.Read(TDRCValue^, ReadAmount);
        TDRCDateTime := TDRCValue;
        FreeMem(TDRCValue);
        Result := ID3v2TDRCDecodeTime(TDRCDateTime);
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
    except
        //*
    end;
end;

function TID3v2Tag.SetRecordingTime(FrameID: AnsiString; DateTime: TDateTime): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetRecordingTime(Index, DateTime);
end;

function TID3v2Tag.SetRecordingTime(FrameIndex: Integer; DateTime: TDateTime): Boolean;
var
    TDRCDateTime: AnsiString;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        TDRCDateTime := ID3v2TDRCEncodeTime(DateTime);
        //* Set the date time
        Frames[FrameIndex].Stream.Write(PANSIChar(TDRCDateTime)^, (Length(TDRCDateTime)));
        Frames[FrameIndex].Stream.Seek(0, soFromBeginning);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.CalculateTagSize(PaddingSize: Integer): Integer;
var
    TotalTagSize: Integer;
    i: Integer;
begin
    //* TODO: Ext header size
    TotalTagSize := 10{ + ExtendedHeaderSize3};
    if MajorVersion = 3 then begin
        for i := 0 to FrameCount - 1 do begin
            TotalTagSize := TotalTagSize + Frames[i].Stream.Size + 10;
            if Frames[i].DataLengthIndicator
            OR Frames[i].Compressed
            then begin
                TotalTagSize := TotalTagSize + 4;
            end;
        end;
    end;
    if MajorVersion > 3 then begin
        for i := 0 to FrameCount - 1 do begin
            TotalTagSize := TotalTagSize + 10;
            TotalTagSize := TotalTagSize + Frames[i].Stream.Size;
            if Frames[i].GroupingIdentity then begin
                TotalTagSize := TotalTagSize + 1;
            end;
            if Frames[i].Encrypted then begin
                TotalTagSize := TotalTagSize + 1;
            end;
            if Frames[i].DataLengthIndicator then begin
                TotalTagSize := TotalTagSize + 4;
            end;
        end;
    end;
    TotalTagSize := TotalTagSize + PaddingSize;
    Result := TotalTagSize;
end;

function TID3v2Tag.CalculateTotalFramesSize: Integer;
var
    TotalFramesSize: Integer;
    i: Integer;
begin
    TotalFramesSize := 0;
    for i := 0 to FrameCount - 1 do begin
        TotalFramesSize := TotalFramesSize + Frames[i].Stream.Size;
    end;
    Result := TotalFramesSize;
end;

function TID3v2Tag.FullFrameSize(FrameIndex: Cardinal): Cardinal;
begin
    if MajorVersion = 3 then begin
        Result := Frames[FrameIndex].Stream.Size;
        if Frames[FrameIndex].Compressed
        OR Frames[FrameIndex].DataLengthIndicator
        then begin
            Result := Result + 4;
        end;
    end;
    if MajorVersion > 3 then begin
        Result := Frames[FrameIndex].Stream.Size;
        if Frames[FrameIndex].GroupingIdentity then begin
            Result := Result + 1;
        end;
        if Frames[FrameIndex].Encrypted then begin
            Result := Result + 1;
        end;
        if Frames[FrameIndex].DataLengthIndicator then begin
            Result := Result + 4;
        end;
    end;
end;

procedure TID3v2Tag.Clear;
begin
    Self.DeleteAllFrames;
    FileName := '';
    Loaded := False;
    MajorVersion := 3;
    MinorVersion := 0;
    Flags := 0;
    Unsynchronised := False;
    ExtendedHeader := False;
    Experimental := False;
    Size := 0;
    CodedSize := 0;
    PaddingSize := 0;
    PaddingToWrite := ID3V2LIBRARY_DEFAULT_PADDING_SIZE;
    if Assigned(ExtendedHeader3) then begin
        FreeAndNil(ExtendedHeader3);
    end;
    ExtendedHeader3 := TID3v2ExtendedHeader3.Create;
    if Assigned(ExtendedHeader4) then begin
        FreeAndNil(ExtendedHeader4);
    end;
    ExtendedHeader4 := TID3v2ExtendedHeader4.Create;
end;

function TID3v2Tag.WriteAllFrames(var TagStream: TStream): Integer;
var
    i: Integer;
    UnCodedSize: Cardinal;
    ReversedFlags: Word;
    CodedUncompressedSize: Cardinal;
begin
    Result := ID3V2LIBRARY_ERROR;
    try
        for i := 0 to FrameCount - 1 do begin
            if NOT ValidFrameID(Frames[i].ID) then begin
                Continue;
            end;
            TagStream.Write(Frames[i].ID, 4);
            UnCodedSize := FullFrameSize(i);
            if MajorVersion = 3 then begin
                CodedSize := ReverseBytes(UnCodedSize);
                TagStream.Write(CodedSize, 4);
                Frames[i].EncodeFlags3;
                TagStream.Write(Frames[i].Flags, 2);
                if Frames[i].Compressed
                OR Frames[i].DataLengthIndicator
                then begin
                    TagStream.Write(Frames[i].DataLengthIndicatorValue, 4);
                end;
            end;
            if MajorVersion = 4 then begin
                UnCodedSize := FullFrameSize(i);
                SyncSafe(UnCodedSize, CodedSize, 4);
                TagStream.Write(CodedSize, 4);
                Frames[i].EncodeFlags4;
                ReversedFlags := Swap16(Frames[i].Flags);
                TagStream.Write(ReversedFlags, 2);
                if Frames[i].GroupingIdentity then begin
                    TagStream.Write(Frames[i].GroupIdentifier, 1);
                end;
                if Frames[i].Encrypted then begin
                    TagStream.Write(Frames[i].EncryptionMethod, 1);
                end;
                if Frames[i].DataLengthIndicator then begin
                    TagStream.Write(Frames[i].DataLengthIndicatorValue, 4);
                end;
            end;
            TagStream.CopyFrom(Frames[i].Stream, 0);
        end;
        Result := ID3V2LIBRARY_SUCCESS;
    except
        Result := ID3V2LIBRARY_ERROR_WRITING_FILE;
    end;
end;

function TID3v2Tag.WriteAllHeaders(var TagStream: TStream): Integer;
begin
    Result := ID3V2LIBRARY_ERROR;
    try
        TagStream.Write(ID3v2ID, 3);
        TagStream.Write(MajorVersion, 1);
        TagStream.Write(MinorVersion, 1);
        if MajorVersion = 3 then begin
            TagStream.Write(Flags, 1);
            TagStream.Write(CodedSize, 4);
        end;
        if MajorVersion = 4 then begin
            TagStream.Write(Flags, 1);
            TagStream.Write(CodedSize, 4);
        end;
        if ExtendedHeader then begin
            //* TODO
            if MajorVersion = 3 then begin

            end;
            if MajorVersion >= 4 then begin

            end;
        end;
        Result := ID3V2LIBRARY_SUCCESS;
    except
        Result := ID3V2LIBRARY_ERROR_WRITING_FILE;
    end;
end;

function TID3v2Tag.WritePadding(var TagStream: TStream; PaddingSize: Integer): Integer;
var
    i: Integer;
    Data: Byte;
begin
    Result := ID3V2LIBRARY_ERROR;
    try
        Data := $00;
        for i := 0 to PaddingSize - 1 do begin
            TagStream.Write(Data, 1);
        end;
        Result := ID3V2LIBRARY_SUCCESS;
    except
        Result := ID3V2LIBRARY_ERROR_WRITING_FILE;
    end;
end;

function LanguageIDtoString(LangageId : TLanguageID): String;
var
    i: integer;
begin
    Result := '';
    for i := low(TLanguageID) to high(TLanguageID) do begin
        if LangageId[i] <> #0 then begin
            Result := Result + LangageId[i];
        end;
    end;
end;

procedure TID3v2Tag.EncodeSize;
var
    UnCodedSize: Cardinal;
begin
    UnCodedSize := CalculateTagSize(PaddingSize) - 10;
    SyncSafe(UnCodedSize, CodedSize, 4);
end;

function TID3v2Tag.RemoveUnsynchronisationOnExtendedHeaderSize: Boolean;
begin
    //Result := RemoveUnsynchronisationOnStream(ExtendedHeader3.SizeData);
end;

function TID3v2Tag.ApplyUnsynchronisationOnExtendedHeaderSize: Boolean;
begin
    //Result := ApplyUnsynchronisationOnStream(ExtendedHeader3.SizeData);
end;

function TID3v2Tag.RemoveUnsynchronisationOnExtendedHeaderData: Boolean;
begin
    Result := RemoveUnsynchronisationOnStream(ExtendedHeader3.Data);
end;

function TID3v2Tag.ApplyUnsynchronisationOnExtendedHeaderData: Boolean;
begin
    Result := ApplyUnsynchronisationOnStream(ExtendedHeader3.Data);
end;

function RemoveUnsynchronisationOnStream(Stream: TMemoryStream): Boolean;
var
    UnUnsyncronisedStream: TMemoryStream;
    Success: Boolean;
begin
    Result := False;
    UnUnsyncronisedStream := nil;
    try
        UnUnsyncronisedStream := TMemoryStream.Create;
        Stream.Seek(0, soBeginning);
        Success := RemoveUnsynchronisationScheme(Stream, UnUnsyncronisedStream, Stream.Size);
        if Success then begin
            Stream.Clear;
            UnUnsyncronisedStream.Seek(0, soBeginning);
            Stream.CopyFrom(UnUnsyncronisedStream, 0);
            Result := True;
        end;
    finally
        if Assigned(UnUnsyncronisedStream) then begin
            FreeAndNil(UnUnsyncronisedStream);
        end;
    end;
end;

function ApplyUnsynchronisationOnStream(Stream: TMemoryStream): Boolean;
var
    UnsyncronisedStream: TMemoryStream;
    Success: Boolean;
begin
    Result := False;
    UnsyncronisedStream := nil;
    try
        UnsyncronisedStream := TMemoryStream.Create;
        Stream.Seek(0, soBeginning);
        Success := ApplyUnsynchronisationScheme(Stream, UnsyncronisedStream, Stream.Size);
        if Success then begin
            Stream.Clear;
            UnsyncronisedStream.Seek(0, soBeginning);
            Stream.CopyFrom(UnsyncronisedStream, 0);
            Result := True;
        end;
    finally
        if Assigned(UnsyncronisedStream) then begin
            FreeAndNil(UnsyncronisedStream);
        end;
    end;
end;

function TID3v2Tag.GetSEBR(FrameID: AnsiString): Extended;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := 0;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Exit;
    end;
    Result := GetSEBR(Index);
end;

function TID3v2Tag.GetSEBR(FrameIndex: Integer): Extended;
var
    SEBR: Extended;
begin
    Result := 0;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    if Frames[FrameIndex].Stream.Size = 0 then begin
        Exit;
    end;
    Frames[FrameIndex].Stream.Seek(0, soBeginning);
    try
        SEBR := 0;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        Frames[FrameIndex].Stream.Read(SEBR, 10);
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        Result := SEBR;
    except
        //*
    end;
end;

function TID3v2Tag.SetSEBR(FrameID: AnsiString; BitRate: Extended): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetSEBR(Index, BitRate);
end;

function TID3v2Tag.SetSEBR(FrameIndex: Integer; BitRate: Extended): Boolean;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    Frames[FrameIndex].Stream.Clear;
    try
        Frames[FrameIndex].Stream.Write(BitRate, 10);
        Result := True;
    except
        //*
    end;
end;


function TID3v2Tag.GetSampleCache(FrameIndex: Integer; ForceDecompression: Boolean; var Version: Byte; var Channels: Integer): TID3v2SampleCache;
var
    ID: Integer;
    SESCHeaderSize: Cardinal;
    ReportedChannels: Integer;
    DataVersion: Byte;
    SeekPosition: Integer;
begin
    SetLength(Result, 0);
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    if Frames[FrameIndex].Stream.Size = 0 then begin
        Exit;
    end;
    Version := 1;
    Channels := 2;
    if Frames[FrameIndex].Unsynchronised then begin
        Frames[FrameIndex].RemoveUnsynchronisation;
    end;
    if Frames[FrameIndex].Compressed
    OR ForceDecompression
    then begin
        Frames[FrameIndex].DeCompress;
    end;
    Frames[FrameIndex].Stream.Seek(0, soBeginning);
    try
        Frames[FrameIndex].Stream.Read(ID, 4);
        if ID = ID3V2LIBRARY_SESC_ID then begin
            Frames[FrameIndex].Stream.Read(DataVersion, 1);
            Frames[FrameIndex].Stream.Read(SESCHeaderSize, 4);
            Version := DataVersion;
            if DataVersion = ID3V2LIBRARY_SESC_VERSION2 then begin
                if SESCHeaderSize >= 4 then begin
                    Frames[FrameIndex].Stream.Read(ReportedChannels, 4);
                    SeekPosition := SESCHeaderSize - 4;
                    Frames[FrameIndex].Stream.Seek(SeekPosition, soCurrent);
                end;
            end;
        end else begin
            Frames[FrameIndex].Stream.Seek(-4, soCurrent);
        end;
        SetLength(Result, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
        Frames[FrameIndex].Stream.Read(Pointer(Result)^, Frames[FrameIndex].Stream.Size - Frames[FrameIndex].Stream.Position);
    except
        //*
    end;
    Frames[FrameIndex].Stream.Seek(0, soBeginning);
end;

function TID3v2Tag.SetSampleCache(FrameIndex: Integer; SESC: TID3v2SampleCache; Channels: Integer): Boolean;
var
    SESCHeaderSize: Cardinal;
    SESCID: Integer;
    DataVersion: Byte;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        SESCID := ID3V2LIBRARY_SESC_ID;
        Frames[FrameIndex].Stream.Write(SESCID, 4);
        DataVersion := ID3V2LIBRARY_SESC_VERSION2;
        Frames[FrameIndex].Stream.Write(DataVersion, 1);
        SESCHeaderSize := 4;
        Frames[FrameIndex].Stream.Write(SESCHeaderSize, 4);
        Frames[FrameIndex].Stream.Write(Channels, 4);
        Frames[FrameIndex].Stream.Write(Pointer(SESC)^, Length(SESC));
        Frames[FrameIndex].Compress;
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.GetSEFC(FrameIndex: Integer): Int64;
var
    PSEFC: PANSIChar;
    SEFC: AnsiString;
    Data: Byte;
begin
    Result := -1;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    if Frames[FrameIndex].Stream.Size = 0 then begin
        Exit;
    end;
    Frames[FrameIndex].Stream.Seek(0, soBeginning);
    PSEFC := AllocMem(Frames[FrameIndex].Stream.Size);
    try
        try
            Frames[FrameIndex].Stream.Read(Data, 1);
            if Data = $01 then begin
                Frames[FrameIndex].Stream.Read(PSEFC, Frames[FrameIndex].Stream.Size);
                SEFC := PSEFC;
                Result := StrToIntDef(SEFC, 0);
            end;
        except
            //*
        end;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
    finally
        FreeMem(PSEFC);
    end;
end;

function TID3v2Tag.SetSEFC(FrameIndex: Integer; SEFC: Int64): Boolean;
var
    StrSEFC: AnsiString;
    Data: Byte;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        StrSEFC := IntToStr(SEFC);
        Data := $00;
        Frames[FrameIndex].Stream.Write(Data, 1);
        Frames[FrameIndex].Stream.Write(PANSIChar(StrSEFC)^, Length(StrSEFC));
        Result := True;
    except
        //*
    end;
end;

procedure AnsiStringToPAnsiChar(const Source: AnsiString; Dest: PAnsiChar; const MaxLength: Integer);
begin
    Move(PAnsiChar(Source)^, Dest^, Min(MaxLength, Length(Source)));
end;

function TID3v2Tag.SetAlbumColors(FrameIndex: Integer; TitleColor, TextColor: Cardinal): Boolean;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        Frames[FrameIndex].Stream.Write(TitleColor, 4);
        Frames[FrameIndex].Stream.Write(TextColor, 4);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.SetAlbumColors(FrameID: AnsiString; TitleColor, TextColor: Cardinal): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetAlbumColors(Index, TitleColor, TextColor);
end;

function TID3v2Tag.GetAlbumColors(FrameID: AnsiString; var TitleColor, TextColor: Cardinal): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := GetAlbumColors(Index, TitleColor, TextColor);
end;

function TID3v2Tag.GetAlbumColors(FrameIndex: Integer; var TitleColor, TextColor: Cardinal): Boolean;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    if Frames[FrameIndex].Stream.Size = 0 then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        Frames[FrameIndex].Stream.Read(TitleColor, 4);
        Frames[FrameIndex].Stream.Read(TextColor, 4);
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.SetTLEN(FrameID: AnsiString; TLEN: Integer): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetTLEN(Index, TLEN);
end;

function TID3v2Tag.SetTLEN(FrameIndex: Integer; TLEN: Integer): Boolean;
var
    TLENString: AnsiString;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        TLENString := #0 + IntToStr(TLEN);
        Frames[FrameIndex].Stream.Write(TLENString[1], System.Length(TLENString));
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.GetPlayCount(FrameID: AnsiString): Cardinal;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := 0;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := GetPlayCount(Index);
end;

function TID3v2Tag.GetPlayCount(FrameIndex: Integer): Cardinal;
var
    Data: Byte;
    i: Integer;
    Value: Cardinal;
begin
    Result := 0;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    if Frames[FrameIndex].Stream.Size = 0 then begin
        Exit;
    end;
    try
        Value := 0;
        Frames[FrameIndex].Stream.Seek(0, soBeginning);
        for i := 0 to Frames[FrameIndex].Stream.Size - 1 do begin
            Value := Value SHL 8;
            Frames[FrameIndex].Stream.Read(Data, 1);
            Value := Value + Data;
        end;
        Result := Value;
    except
        //*
    end;
end;

function TID3v2Tag.SetPlayCount(FrameID: AnsiString; PlayCount: Cardinal): Boolean;
var
    Index: Integer;
    ID: TFrameID;
begin
    Result := False;
    AnsiStringToPAnsiChar(FrameID, @ID, 4);
    Index := FrameExists(ID);
    if Index < 0 then begin
        Index := AddFrame(ID);
        if Index < 0 then begin
            Exit;
        end;
    end;
    Result := SetPlayCount(Index, PlayCount);
end;

function TID3v2Tag.SetPlayCount(FrameIndex: Integer; PlayCount: Cardinal): Boolean;
var
    Data: Byte;
    Value: Cardinal;
begin
    Result := False;
    if (FrameIndex >= FrameCount)
    OR (FrameIndex < 0)
    then begin
        Exit;
    end;
    try
        Frames[FrameIndex].Stream.Clear;
        Value := PlayCount SHR 24;
        Data := Value;
        Frames[FrameIndex].Stream.Write(Data, 1);
        Value := PlayCount SHL 8;
        Value := Value SHR 24;
        Data := Value;
        Frames[FrameIndex].Stream.Write(Data, 1);
        Value := PlayCount SHL 16;
        Value := Value SHR 24;
        Data := Value;
        Frames[FrameIndex].Stream.Write(Data, 1);
        Value := PlayCount SHL 24;
        Value := Value SHR 24;
        Data := Value;
        Frames[FrameIndex].Stream.Write(Data, 1);
        Result := True;
    except
        //*
    end;
end;

function Swap16(ASmallInt: SmallInt): SmallInt; register;
asm
    xchg al,ah
end;

function TID3v2Tag.RemoveUnsynchronisationOnAllFrames: Boolean;
var
    i: Integer;
begin
    Result := False;
    try
        if MajorVersion = 3 then begin
            if Unsynchronised then begin
                for i := 0 to FrameCount - 1 do begin
                    Frames[i].RemoveUnsynchronisation;
                end;
                Unsynchronised := False;
            end;
        end;
        if MajorVersion = 4 then begin
            for i := 0 to FrameCount - 1 do begin
                if Frames[i].Unsynchronised then begin
                    Frames[i].RemoveUnsynchronisation;
                end;
            end;
            Unsynchronised := False;
        end;
        Result := True;
    except
        //*
    end;
end;

function TID3v2Tag.ApplyUnsynchronisationOnAllFrames: Boolean;
var
    i: Integer;
begin
    Result := False;
    try
        if MajorVersion = 3 then begin
            for i := 0 to FrameCount - 1 do begin
                Frames[i].ApplyUnsynchronisation;
            end;
            Unsynchronised := True;
        end;
        if MajorVersion = 4 then begin
            for i := 0 to FrameCount - 1 do begin
                if NOT Frames[i].Unsynchronised then begin
                    Frames[i].ApplyUnsynchronisation;
                end;
            end;
            Unsynchronised := True;
        end;
        Result := True;
    except
        //*
    end;
end;

function APICType2Str(PictureType: Integer): String;
begin
    Result := 'Other';
    if PictureType = $00 then begin
        Result := 'Other';
        Exit;
    end;
    if PictureType = $01 then begin
        Result := '32x32 pixels ''file icon'' (PNG only)';
        Exit;
    end;
    if PictureType = $02 then begin
        Result := 'Other file icon';
        Exit;
    end;
    if PictureType = $03 then begin
        Result := 'Cover (front)';
        Exit;
    end;
    if PictureType = $04 then begin
        Result := 'Cover (back)';
        Exit;
    end;
    if PictureType = $05 then begin
        Result := 'Leaflet page';
        Exit;
    end;
    if PictureType = $06 then begin
        Result := 'Media (e.g. label side of CD)';
        Exit;
    end;
    if PictureType = $07 then begin
        Result := 'Lead artist/lead performer/soloist';
        Exit;
    end;
    if PictureType = $08 then begin
        Result := 'Artist/performer';
        Exit;
    end;
    if PictureType = $09 then begin
        Result := 'Conductor';
        Exit;
    end;
    if PictureType = $0A then begin
        Result := 'Band/Orchestra';
        Exit;
    end;
    if PictureType = $0B then begin
        Result := 'Composer';
    end;
    if PictureType = $0C then begin
        Result := 'Lyricist/text writer';
        Exit;
    end;
    if PictureType = $0D then begin
        Result := 'Recording Location';
        Exit;
    end;
    if PictureType = $0E then begin
        Result := 'During recording';
        Exit;
    end;
    if PictureType = $0F then begin
        Result := 'During performance';
        Exit;
    end;
    if PictureType = $10 then begin
        Result := 'Movie/video screen capture';
        Exit;
    end;
    if PictureType = $11 then begin
        Result := 'A bright coloured fish';
        Exit;
    end;
    if PictureType = $12 then begin
        Result := 'Illustration';
        Exit;
    end;
    if PictureType = $13 then begin
        Result := 'Band/artist logotype';
        Exit;
    end;
    if PictureType = $14 then begin
        Result := 'Publisher/Studio logotype';
        Exit;
    end;
end;

function APICTypeStr2No(PictureType: String): Integer;
begin
    Result := $00;
    if PictureType = 'Other' then begin
        Result := $00;
        Exit;
    end;
    if PictureType = '32x32 pixels ''file icon'' (PNG only)' then begin
        Result := $01;
        Exit;
    end;
    if PictureType = 'Other file icon' then begin
        Result := $02;
        Exit;
    end;
    if PictureType = 'Cover (front)' then begin
        Result := $03;
        Exit;
    end;
    if PictureType = 'Cover (back)' then begin
        Result := $04;
        Exit;
    end;
    if PictureType = 'Leaflet page' then begin
        Result := $05;
        Exit;
    end;
    if PictureType = 'Media (e.g. label side of CD)' then begin
        Result := $06;
        Exit;
    end;
    if PictureType = 'Lead artist/lead performer/soloist' then begin
        Result := $07;
        Exit;
    end;
    if PictureType = 'Artist/performer' then begin
        Result := $08;
        Exit;
    end;
    if PictureType = 'Conductor' then begin
        Result := $09;
        Exit;
    end;
    if PictureType = 'Band/Orchestra' then begin
        Result := $0A;
        Exit;
    end;
    if PictureType = 'Composer' then begin
        Result := $0B;
    end;
    if PictureType = 'Lyricist/text writer' then begin
        Result := $0C;
        Exit;
    end;
    if PictureType = 'Recording Location' then begin
        Result := $0D;
        Exit;
    end;
    if PictureType = 'During recording' then begin
        Result := $0E;
        Exit;
    end;
    if PictureType = 'During performance' then begin
        Result := $0F;
        Exit;
    end;
    if PictureType = 'Movie/video screen capture' then begin
        Result := $10;
        Exit;
    end;
    if PictureType = 'A bright coloured fish' then begin
        Result := $11;
        Exit;
    end;
    if PictureType = 'Illustration' then begin
        Result := $12;
        Exit;
    end;
    if PictureType = 'Band/artist logotype' then begin
        Result := $13;
        Exit;
    end;
    if PictureType = 'Publisher/Studio logotype' then begin
        Result := $14;
        Exit;
    end;
end;

function ID3v2RemoveTag(FileName: String): Integer;
var
    AudioFileName: String;
    AudioFile: TFileStream;
    OutputFileName: String;
    OutputFile: TFileStream;
    ID3v2Size: Integer;
    TagCodedSizeInExistingStream: Cardinal;
    TagSizeInExistingStream: Cardinal;

begin
    Result := ID3V2LIBRARY_ERROR;
    if NOT FileExists(FileName) then begin
        Exit;
    end;
    try
        Result := ID3V2LIBRARY_ERROR_EMPTY_TAG;
        AudioFileName := FileName;
        try
            try
                AudioFile := TFileStream.Create(AudioFileName, fmOpenRead);
            except
                Result := ID3V2LIBRARY_ERROR_OPENING_FILE;
                Exit;
            end;
            if ID3v2ValidTag(AudioFile) then begin
                //* Skip version data and flags
                AudioFile.Seek(3, soCurrent);
                AudioFile.Read(TagCodedSizeInExistingStream, 4);
                UnSyncSafe(TagCodedSizeInExistingStream, 4, TagSizeInExistingStream);
                //* Add header size to size
                ID3v2Size := TagSizeInExistingStream + 10;
            end;
        finally
            FreeAndNil(AudioFile);
        end;
        //ID3v2Size := Size + 10;
        if ID3v2Size > 0 then begin
            try
                AudioFile := TFileStream.Create(AudioFileName, fmOpenRead);
            except
                Result := ID3V2LIBRARY_ERROR_OPENING_FILE;
                Exit;
            end;
            OutputFileName := ChangeFileExt(AudioFileName, '.tmp');
            try
                OutputFile := TFileStream.Create(OutputFileName, fmCreate OR fmOpenReadWrite);
            except
                Result := ID3V2LIBRARY_ERROR_OPENING_FILE;
                Exit;
            end;
            AudioFile.Seek(ID3v2Size, soBeginning);
            OutputFile.CopyFrom(AudioFile, AudioFile.Size - ID3v2Size);
            FreeAndNil(AudioFile);
            FreeAndNil(OutputFile);
            if NOT DeleteFile(PWideChar(AudioFileName)) then begin
                Result := GetLastError;
                DeleteFile(PWideChar(OutputFileName));
            end else begin
                RenameFile(OutputFileName, AudioFileName);
                Result := ID3V2LIBRARY_SUCCESS;
            end;
        end;
    except
        Result := ID3V2LIBRARY_ERROR;
    end;
end;

function ID3v2ValidTag(TagStream: TStream): Boolean;
var
    Identification: TID3v2ID;
begin
    Result := False;
    try
        Identification := #0#0#0;
        TagStream.Read(Identification[0], 3);
        if Identification = ID3v2ID then begin
            Result := True;
        end;
    except
        //*
    end;
end;

  // Use CalcCRC32 as a procedure so CRCValue can be passed in but
  // also returned. This allows multiple calls to CalcCRC32 for
  // the "same" CRC-32 calculation.
procedure CalcCRC32(P: Pointer; ByteCount: DWORD; var CRCValue: DWORD);
  // The following is a little cryptic (but executes very quickly).
  // The algorithm is as follows:
  // 1. exclusive-or the input byte with the low-order byte of
  // the CRC register to get an INDEX
  // 2. shift the CRC register eight bits to the right
  // 3. exclusive-or the CRC register with the contents of Table[INDEX]
  // 4. repeat steps 1 through 3 for all bytes
var
    i: DWORD;
    q: ^BYTE;
begin
    q := p;
    for i := 0 to ByteCount - 1 do begin
        CRCvalue := (CRCvalue SHR 8) XOR CRC32Table[q^ XOR (CRCvalue AND $000000FF)];
        Inc(q)
    end;
end;

function CalculateStreamCRC32(Stream: TStream; var CRCvalue: DWORD): Boolean;
var
    MemoryStream: TMemoryStream;
begin
    Result := False;
    CRCValue := $FFFFFFFF;
    MemoryStream := TMemoryStream(Stream);
    try
        MemoryStream.Seek(0, soBeginning);
        if MemoryStream.Size > 0 then begin
            CalcCRC32(MemoryStream.Memory, MemoryStream.Size, CRCvalue);
            Result := True;
        end;
    except
        Result := False;
    end;
    CRCvalue := NOT CRCvalue;
end;

function TID3v2Tag.CalculateTagCRC32: Cardinal;
var
    CRC32: Cardinal;
    TagsStream: TStream;
    Error: Integer;
    ReUnsynchronise: Boolean;
begin
    Result := 0;
    TagsStream := TMemoryStream.Create;
    try
        ReUnsynchronise := Unsynchronised;
        if ReUnsynchronise then begin
            RemoveUnsynchronisationOnAllFrames;
        end;
        Error := WriteAllFrames(TagsStream);
        if Error <> ID3V2LIBRARY_SUCCESS then begin
            Exit;
        end;
        CalculateStreamCRC32(TagsStream, CRC32);
        Result := CRC32;
    finally
        FreeAndNil(TagsStream);
        if ReUnsynchronise then begin
            ApplyUnsynchronisationOnAllFrames;
        end;
    end;
end;

Initialization
    ID3v2ID := 'ID3';

end.
