library ProjectID3;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters. }

uses
  ID3v2_Lib in 'ID3v2_Lib.pas',
  ShlObj,
  Windows,
  SysUtils,
  Classes,
  WinInet,
  QianQianLRCU in 'QianQianLRCU.pas',
  ID3v1Library in 'ID3v1Library.pas';

{$R *.res}

procedure WriteITunesLyrics(FileName,Content:PAnsiChar); stdcall;
var
 ID3v2Tag: TID3v2Tag;
 Error: Integer;
 LanguageID: TLanguageID;
begin
  ID3v2Tag := TID3v2Tag.Create;
  Error := ID3v2Tag.LoadFromFile(FileName);

  if Error <> ID3V2LIBRARY_SUCCESS then
  begin
    ID3v2Tag.Free;
    Exit;
  end;
  LanguageID := 'eng';
  ID3v2Tag.SetUnicodeLyrics('USLT', string(Content), LanguageID, '');
  ID3v2Tag.SaveToFile(FileName);
  ID3v2Tag.Free;
end;

procedure SetCover(FileName,Path:PAnsiChar); stdcall;
var
  ID3v2Tag: TID3v2Tag;
  Error: Integer;
  Fext: String;
  MIMEType: AnsiString;
  FrameIndex: Integer;
  Description: String;
  PictureType: Integer;
begin
  ID3v2Tag := TID3v2Tag.Create;
  Error := ID3v2Tag.LoadFromFile(FileName);

  if Error <> ID3V2LIBRARY_SUCCESS then
  begin
    ID3v2Tag.Free;
    Exit;
  end;
  Fext := UpperCase(ExtractFileExt(Path));
  if (Fext = '.JPG')
  OR (Fext = '.JPEG')
  then begin
      MIMEType := 'image/jpeg';
  end;
  if (Fext = '.PNG')
  then begin
      MIMEType := 'image/png';
  end;
  if (Fext = '.BMP')
  then begin
      MIMEType := 'image/bmp';
  end;
  if (Fext = '.GIF')
  then begin
      MIMEType := 'image/gif';
  end;
  Description := '';
  PictureType := $03;
  FrameIndex := ID3v2Tag.AddFrame('APIC');
  ID3v2Tag.SetUnicodeCoverPictureFromFile(FrameIndex, Description, Path, MIMEType, PictureType);
  ID3v2Tag.SaveToFile(FileName);
  ID3v2Tag.Free;


end;

function GetWebPage(const Url: string):AnsiString;
var
  Session,
  HttpFile:HINTERNET;
  szSizeBuffer:Pointer;
  dwLengthSizeBuffer:DWord;
  dwReserved:DWord;
  dwFileSize:DWord;
  dwBytesRead:DWord;
  Contents:PAnsiChar;
begin
  Session:=InternetOpen('',0,nil,nil,0);
  HttpFile:=InternetOpenUrl(Session,PChar(Url),nil,0,0,0);
  dwLengthSizeBuffer:=1024;
  HttpQueryInfo(HttpFile,5,szSizeBuffer,dwLengthSizeBuffer,dwReserved);
  GetMem(Contents,dwFileSize);
  InternetReadFile(HttpFile,Contents,dwFileSize,dwBytesRead);
  InternetCloseHandle(HttpFile);
  InternetCloseHandle(Session);
  Result:=Contents;
  FreeMem(Contents);
end;

function ClearXMLZY(s:string):string; //清除XML中的转义字符
var
  temp:string;
begin
  temp:=StringReplace(s,'&amp;','&',[rfReplaceAll,rfIgnoreCase]);
  temp:=StringReplace(temp,'&apos;','''',[rfReplaceAll,rfIgnoreCase]);
  temp:=StringReplace(temp,'&quot;','"',[rfReplaceAll,rfIgnoreCase]);
  temp:=StringReplace(temp,'&lt;','<',[rfReplaceAll,rfIgnoreCase]);
  Result:=StringReplace(temp,'&gt;','>',[rfReplaceAll,rfIgnoreCase]);
end;

procedure GetLyrics(Title,Artist:PAnsiChar;Server:Integer;resulta:PAnsiChar); stdcall;
var
 URL:string;
 WXml:TStringList;
 LrcS,ID,Art,Tit,Titemp,Artemp:String;
 LrcContent:AnsiString;
 I:Integer;
begin
 //MyIni:=MyIni.Create(GetCurrentDir+'\Config.ini');
 URL:=QianQianLrcU.LrcListLink(Artist,Title,Server);

 WXml:=TStringList.Create;

 WXml.Text:=UTF8Decode(GetWebPage(Url));
 WXml.Delete(0); WXml.Delete(0);
 WXml.Delete(WXml.Count-1);
 for I:=0 to WXml.Count-1 do
 begin
  LrcS:=Trim(WXml.Strings[i]);
  Titemp:=Copy(LrcS,Pos('" title="',LrcS)+Length('" title="'),Pos('"></lrc>',LrcS)-Pos('" title="',LrcS)-Length('" title="'));
  Artemp:=Copy(LrcS,Pos('" artist="',LrcS)+Length('" artist="'),Pos('" title="',LrcS)-Pos('" artist="',LrcS)-Length('" artist="'));
  if (ID='')or(Pos('中',Titemp)>0)or(Pos('对照',Artemp)>0)or(Pos('双语',Artemp)>0)or(Pos('中',Artemp)>0) then
  begin
    ID:=Copy(LrcS,Length('<lrc id="')+1,Pos('" artist="',LrcS)-Length('<lrc id="')-1);
    Art:=ClearXMLZY(Artemp); //清除XML转义字符
    Tit:=ClearXMLZY(Titemp); //清除XML转义字符
  end;
 end;
 WXml.Free;
 if ID<>'' then
 begin
  Url:=QianQianLrcU.LrcDownLoadLink(StrToInt(ID),Art,Tit,1);
  LrcContent:=UTF8Decode(GetWebPage(Url));
  //Result:=LrcContent;

  CopyMemory(resulta,PAnsiChar(LrcContent),SizeOf(AnsiChar)*Length(LrcContent));
 end else
 begin
  //Result:='';
 end;
end;


procedure FixID3Tags(FileName,Title,Artist,Album:PAnsiChar); stdcall;
var
  ID3v2Tag: TID3v2Tag;
  ID3v1Tag: TID3v1Tag;
  Error: Integer;
begin
  ID3v2Tag := TID3v2Tag.Create;
  Error := ID3v2Tag.LoadFromFile(FileName);
  if Error <> ID3V2LIBRARY_SUCCESS then
  begin
    ID3v2Tag.Free;
    Exit;
  end;

  //* Set Title
  ID3v2Tag.SetUnicodeText('TIT2', Title);

  //* Set Artist
  ID3v2Tag.SetUnicodeText('TPE1', Artist);

  //* Set Album
  ID3v2Tag.SetUnicodeText('TALB', Album);

  //清理QQ音乐的痕迹
  ID3v2Tag.SetUnicodeText('TCOP','4321.La');
  ID3v2Tag.SaveToFile(FileName);
  ID3v2Tag.Free;

  //ID1标签
  ID3v1Tag := TID3v1Tag.Create;
  Error := ID3v1Tag.LoadFromFile(FileName);
  if Error<>ID3V1LIBRARY_SUCCESS then
  begin
    ID3v1Tag.Free;
    Exit;
  end;
  ID3v1Tag.Title := Title;
  ID3v1Tag.Artist := Artist;
  ID3v1Tag.Album := Album;
  ID3v1Tag.SaveToFile(FileName);
  ID3v1Tag.Free;
end;

exports
  WriteITunesLyrics,
  SetCover,
  GetLyrics,
  FixID3Tags;
begin

end.
