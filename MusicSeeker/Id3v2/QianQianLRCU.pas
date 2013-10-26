unit QianQianLRCU;
interface

uses
  SysUtils,Classes;

function LrcListLink(Atrist,Title:string;Server:Integer):string;
function LrcDownLoadLink(LrcId:Integer;Atrist,Title:string;Server:Integer):string;

implementation
function StrToHex(str: string; AEncoding: TEncoding): string;
var
  ss: TStringStream;
  i: Integer;
begin
  Result := '';
  ss := TStringStream.Create(str, AEncoding);
  for i := 0 to ss.Size - 1 do
    Result := Result + Format('%.2x', [ss.Bytes[i]]);
  ss.Free;
end;

function Clear(s:string):string;
var
  i:Integer;
  temp,BStr,QStr:string;
begin
  temp:=StringReplace(s,'''','',[rfReplaceAll,rfIgnoreCase]);
  BStr:=' `~!@#$%^&*()-_=+,<.>/?;:"[{]}\|€';
  QStr:='¡¡¡££¬¡¢£»£º£¿£¡¡­¡ª¡¤¡¥¡§¡®¡¯¡°¡±¡©¡«¡¬¡Ã£¢£§£à£ü¡¨¡²¡³¡´¡µ¡¶¡·¡¸¡¹¡º¡»£®¡¼¡½¡¾¡¿£¨£©£Û£Ý£û£ý';
  QStr:=QStr+'¡Ö¡Ô¡Ù£½¡Ü¡Ý£¼£¾¡Ú¡Û¡Ë¡À£«£­¡Á¡Â£¯¡Ò¡Ó¡Ø¡Þ¡Ä¡Å¡Æ¡Ç¡È¡É¡Ê¡ß¡à¡Í¡Î¡Ï¡Ð¡Ñ¡Õ¡×¡Ì';
  QStr:=QStr+'¡ì¡í¡î¡ï¡ð¡ñ¡ò¡ó¡ô¡õ¡æ¡ë¡ö¡÷¡ø¡ù¡ú¡û¡ü¡ý¡þ¡è¡ã£££¦£À£Ü¦ä£ß£þ¨D¡á¡â';
  QStr:=QStr+'¢ñ¢ò¢ó¢ô¢õ¢ö¢÷¢ø¢ù¢ú¢û¢ü¢±¢²¢³¢´¢µ¢¶¢·¢¸¢¹¢º¢»¢¼¢½¢¾¢¿¢À¢Á¢Â¢Ã¢Ä¢å¢æ¢ç¢è¢é¢ê¢ë¢ì¢í¢î';
  QStr:=QStr+'¢Ù¢Ú¢Û¢Ü¢Ý¢Þ¢ß¢à¢á¢â¢Å¢Æ¢Ç¢È¢É¢Ê¢Ë¢Ì¢Í¢Î¢Ï¢Ð¢Ñ¢Ò¢Ó¢Ô¢Õ¢Ö¢×¢Ø';
  QStr:=QStr+'©°©±©²©³©´©µ©¶©·©¤©¨©¬©¸©¹©º©»©¼©½©¾©¿©¥©©©­©À©Á©Â©Ã©Ä©Å©Æ©Ç©¦©ª©®©È©É©Ê©Ë©Ì©Í©Î©Ï©§©«©¯';
  QStr:=QStr+'©Ð©Ñ©Ò©Ó©Ô©Õ©Ö©×©Ø©Ù©Ú©Û©Ü©Ý©Þ©ß©à©á©â©ã©ä©å©æ©ç©è©é©ê©ë©ì©í©î©ï';
  for i:=1 to Length(BStr) do
   begin
     temp:=StringReplace(temp,BStr[i],'',[rfReplaceAll,rfIgnoreCase]);
   end;
  for i:=1 to (Length(QStr) div 2) do
   begin
     temp:=StringReplace(temp,QStr[(i-1)*2+1]+QStr[(i-1)*2+2],'',[rfReplaceAll,rfIgnoreCase]);
   end;
  Result:=temp;
end;

function Conv(i:Integer):Integer;
var
  t:Int64;
begin
    t:= i mod $100000000;
    if (i >= 0) and (t > $80000000) then
        Dec(t, $100000000);
    if (i < 0) and (t < $80000000) then
        Inc(t, $100000000);
    Result := t;
end;

function LrcListLink(Atrist,Title:string;Server:Integer):string;
var
  Url,A,T:string;
begin
  A:=LowerCase(Clear(Atrist));
  T:=LowerCase(Clear(Title));
if Server=0 then
    begin
      Url:='http://ttlrcct.qianqian.com/dll/lyricsvr.dll';
    end
else  {if Server=1 then }
    begin
      Url:='http://ttlrccnc.qianqian.com/dll/lyricsvr.dll';
    end;
Result:=Url+'?sh?Artist='+StrToHex(A,TEncoding.Unicode)+'&Title='+StrToHex(T,TEncoding.Unicode)+'&Flags=0';
end;

function LrcDownLoadLink(LrcId:Integer;Atrist,Title:string;Server:Integer):string;  //¼ÓÃÜËã·¨ ·­Òë×ÔC#
var
  UTF8Str,URL:string;
  len,i,j:Integer;
  t1,t2,t3,t4,c:Integer;
  t6,t5:Int64;
  song:array [0..1000] of Byte;
begin
  UTF8Str:=StrToHex(Atrist+Title,TEncoding.UTF8);
  if (length(UTF8Str) mod 2)=1 then
    begin
     UTF8Str:=UTF8Str+'0';
    end;
  len:=length(UTF8Str) div 2;
  for i:=1 to len do
    begin
        song[i]:=strtoint('$'+copy(UTF8Str,i*2-1,2));
    end;
    t2 := 0;
    t1 := ((lrcId and $0000FF00) shr 8);
    if ((lrcId and $00FF0000) = 0) then
      begin
       t3 := ($000000FF and (not t1));
      end
    else
      begin
       t3 := $000000FF and ((lrcId and $00FF0000) shr 16);
      end;
    t3 := t3 or (($000000FF and lrcId) shl 8);
    t3 := t3 shl 8;
    t3 := t3 or ($000000FF and t1);
    t3 := t3 shl 8;
    if (lrcId and $FF000000) = 0 then
     begin
       t3 := t3 or ($000000FF and (not lrcId));
     end
    else
     begin
       t3 := t3 or ($000000FF and (lrcId shr 24));
     end;
    j := len;
    while j >= 1 do
      begin
        c := song[j];
        if c >= $80 then
          begin
            c:=c-$100;
          end;
        t1 := (c+t2) and $00000000FFFFFFFF;
        t2 := (t2 shl (((j-1) mod 2) + 4)) and $00000000FFFFFFFF;
        t2 := (t1 + t2) and $00000000FFFFFFFF ;
        dec(j);
      end;
    j  := 1;
    t1 := 0;
    while j <= len do
      begin
        c := song[j];
        if c >= $80 then
          begin
            c:=c-$100;
          end;
        t4 := (c + t1) and $00000000FFFFFFFF;
        t1 := (t1 shl (((j-1) mod 2) + 3)) and $00000000FFFFFFFF;
        t1 := (t1 + t4) and $00000000FFFFFFFF;
        inc(j);
      end;
    t5 := Conv(t2 xor t3);
    t5 := Conv(t5 + (t1 or lrcId));
    t5 := Conv((t5 * (t1 or t3)));
    t5 := Conv(t5 * (t2 xor lrcId));
    t6 := t5;
    if t6 > $80000000 then
      begin
        t5 := t6 - $100000000;
      end;
  if Server=0 then
      begin
        Url:='http://ttlrcct.qianqian.com/dll/lyricsvr.dll';
      end
  else if Server=1 then
      begin
        Url:='http://ttlrccnc.qianqian.com/dll/lyricsvr.dll';
      end;
  Result:=Url+'?dl?Id='+inttostr(lrcId)+'&Code='+inttostr(t5);
end;

end.
