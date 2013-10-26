VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Begin VB.Form MainForm 
   Caption         =   "IdSharp Test Harness"
   ClientHeight    =   5355
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   10305
   LinkTopic       =   "Form1"
   ScaleHeight     =   5355
   ScaleWidth      =   10305
   StartUpPosition =   2  'CenterScreen
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   2880
      Top             =   4800
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "&Close"
      Height          =   375
      Left            =   8880
      TabIndex        =   18
      Top             =   4800
      Width           =   1215
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save"
      Enabled         =   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   17
      Top             =   4800
      Width           =   1215
   End
   Begin VB.ComboBox cmbID3v2 
      Height          =   315
      Left            =   1080
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   600
      Width           =   2055
   End
   Begin VB.ComboBox cmbGenre 
      Height          =   315
      Left            =   1080
      Sorted          =   -1  'True
      TabIndex        =   14
      Top             =   2040
      Width           =   2055
   End
   Begin VB.TextBox txtTrack 
      Height          =   315
      Left            =   1080
      TabIndex        =   16
      Top             =   2760
      Width           =   3735
   End
   Begin VB.TextBox txtYear 
      Height          =   315
      Left            =   1080
      TabIndex        =   15
      Top             =   2400
      Width           =   3735
   End
   Begin VB.TextBox txtAlbum 
      Height          =   315
      Left            =   1080
      TabIndex        =   13
      Top             =   1680
      Width           =   3735
   End
   Begin VB.TextBox txtTitle 
      Height          =   315
      Left            =   1080
      TabIndex        =   12
      Top             =   1320
      Width           =   3735
   End
   Begin VB.TextBox txtArtist 
      Height          =   315
      Left            =   1080
      TabIndex        =   10
      Top             =   960
      Width           =   3735
   End
   Begin VB.TextBox txtFileName 
      Enabled         =   0   'False
      Height          =   315
      Left            =   1080
      TabIndex        =   0
      TabStop         =   0   'False
      Top             =   240
      Width           =   3735
   End
   Begin VB.CommandButton cmdLoad 
      Caption         =   "&Load"
      Height          =   375
      Left            =   240
      TabIndex        =   1
      Top             =   4800
      Width           =   1215
   End
   Begin VB.Label Label8 
      AutoSize        =   -1  'True
      Caption         =   "Track"
      Height          =   195
      Left            =   240
      TabIndex        =   11
      Top             =   2760
      Width           =   420
   End
   Begin VB.Label Label7 
      AutoSize        =   -1  'True
      Caption         =   "Year"
      Height          =   195
      Left            =   240
      TabIndex        =   9
      Top             =   2400
      Width           =   330
   End
   Begin VB.Label Label6 
      AutoSize        =   -1  'True
      Caption         =   "Genre"
      Height          =   195
      Left            =   240
      TabIndex        =   8
      Top             =   2040
      Width           =   435
   End
   Begin VB.Label Label5 
      AutoSize        =   -1  'True
      Caption         =   "Album"
      Height          =   195
      Left            =   240
      TabIndex        =   7
      Top             =   1680
      Width           =   435
   End
   Begin VB.Label Label4 
      AutoSize        =   -1  'True
      Caption         =   "Title"
      Height          =   195
      Left            =   240
      TabIndex        =   6
      Top             =   1320
      Width           =   300
   End
   Begin VB.Label Label3 
      AutoSize        =   -1  'True
      Caption         =   "Artist"
      Height          =   195
      Left            =   240
      TabIndex        =   4
      Top             =   960
      Width           =   345
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      Caption         =   "ID3v2"
      Height          =   195
      Left            =   240
      TabIndex        =   3
      Top             =   600
      Width           =   435
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      Caption         =   "File name"
      Height          =   195
      Left            =   240
      TabIndex        =   2
      Top             =   240
      Width           =   675
   End
   Begin VB.Image Image1 
      Height          =   4215
      Left            =   5400
      Stretch         =   -1  'True
      Top             =   240
      Width           =   4695
   End
End
Attribute VB_Name = "MainForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'   IdSharp - A tagging library for .NET
'   Copyright (C) 2007  Jud White
'
'   This program is free software; you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation; either version 2 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License along
'   with this program; if not, write to the Free Software Foundation, Inc.,
'   51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
Option Explicit

Private m_PicHelper As New IdSharp.PictureUtils
Private m_GenreHelper As New IdSharp.GenreHelper
Private m_ID3v2Helper As New IdSharp.ID3v2Helper

Private m_ID3v2 As IdSharp.IID3v2
Private m_Frames As IdSharp.IFrameContainer
Private m_FileName As String

Private Sub Form_Load()

    ' Genres
    Dim genreList() As String
    genreList = m_GenreHelper.GenreByIndex
    
    Dim i As Integer
    For i = 0 To 147
        cmbGenre.AddItem genreList(i)
    Next
    
    ' ID3v2 versions
    cmbID3v2.AddItem "ID3v2.2"
    cmbID3v2.AddItem "ID3v2.3"
    cmbID3v2.AddItem "ID3v2.4"
    
    ' Open dialog filter
    CommonDialog1.Filter = "*.mp3|*.mp3"
    
End Sub

Private Sub cmdClose_Click()

    ' Clean up
    Set m_PicHelper = Nothing
    Set m_GenreHelper = Nothing
    Set m_ID3v2Helper = Nothing
    
    Set m_ID3v2 = Nothing
    Set m_Frames = Nothing
    
    ' Close
    Unload Me
    
End Sub

Private Sub cmdLoad_Click()
    
    CommonDialog1.ShowOpen
    
    If (CommonDialog1.fileName <> "") Then
    
        Load CommonDialog1.fileName
    
    End If
    
End Sub

Private Sub Load(fileName As String)

    m_FileName = fileName
    
    Set m_ID3v2 = Nothing
    Set m_Frames = Nothing
    
    Set m_ID3v2 = m_ID3v2Helper.CreateID3v2FromFile(m_FileName)
    Set m_Frames = m_ID3v2
    
    ' Fields
    txtFileName.Text = m_FileName
    txtArtist.Text = m_Frames.Artist
    txtTitle.Text = m_Frames.Title
    txtAlbum.Text = m_Frames.Album
    cmbGenre.Text = m_Frames.genre
    txtTrack.Text = m_Frames.TrackNumber
    txtYear.Text = m_Frames.Year
    
    ' ID3v2 version
    Dim tagVersion As IdSharp.ID3v2TagVersion
    tagVersion = m_ID3v2.Header.tagVersion
    If tagVersion = ID3v2TagVersion_ID3v22 Then
        cmbID3v2.ListIndex = 0
    ElseIf tagVersion = ID3v2TagVersion_ID3v23 Then
        cmbID3v2.ListIndex = 1
    Else
        cmbID3v2.ListIndex = 2
    End If
    
    ' Pictures
    Dim pics As IdSharp.IFrameList
    Set pics = m_Frames.GetFrameList("APIC")
    
    If pics.Count > 0 Then
        Dim pic As IAttachedPicture
        Set pic = pics.Item(0)
        Set Image1.Picture = m_PicHelper.GetIPictureDispFromImage(pic.Picture)
    Else
        Set Image1.Picture = Nothing
    End If
    
    ' Cleanup
    Set pic = Nothing
    Set pics = Nothing
    
    ' Enable save
    cmdSave.Enabled = True
    
End Sub

Private Sub cmdSave_Click()

    If Not m_ID3v2 Is Nothing And Not m_Frames Is Nothing Then
    
        ' ID3v2 version
        If cmbID3v2.ListIndex = 0 Then
            m_ID3v2.Header.tagVersion = ID3v2TagVersion_ID3v22
        ElseIf cmbID3v2.ListIndex = 1 Then
            m_ID3v2.Header.tagVersion = ID3v2TagVersion_ID3v23
        Else
            m_ID3v2.Header.tagVersion = ID3v2TagVersion_ID3v24
        End If
        
        ' Fields
        m_Frames.Artist = txtArtist.Text
        m_Frames.Title = txtTitle.Text
        m_Frames.Album = txtAlbum.Text
        m_Frames.genre = cmbGenre.Text
        m_Frames.TrackNumber = txtTrack.Text
        m_Frames.Year = txtYear.Text
        
        ' Save
        m_ID3v2.Save m_FileName
        
        ' Reload
        Load m_FileName
        
    End If
    
End Sub


