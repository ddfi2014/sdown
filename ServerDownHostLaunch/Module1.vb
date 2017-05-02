
Imports System.ServiceModel
Module Module1

    Private result As String

    Public Property StatusMessage As String

    Public Sub Main(args As String())
        Dim deleg = If(args.Any, Sub() WithArgs(), Sub() NoArgs())
        deleg()
    End Sub

    Private Sub NoArgs()
        Console.WriteLine("What do you want to do?")
        Console.WriteLine("* Log")
        Console.WriteLine("* Host")
        Console.WriteLine("* StopLog")
        Console.WriteLine("* StopHost" & vbNewLine)
        Reply()
    End Sub

    Private Sub Reply()
        Console.Write("ServerDown>")
        result = Console.ReadLine()
        Select Case result.ToLower()
            Case "Log".ToLower()
                StartLog()
            Case "Host".ToLower()
                StartHost()
            Case "StopLog".ToLower()
                StopLog()
            Case "StopHost".ToLower()
                StopHost()
            Case ""
                Reply()
            Case Else
                Console.WriteLine("Invalid Option.")
        End Select

    End Sub

    Private Sub WithArgs()
        Console.WriteLine("With Args.")
        Console.ReadKey()
    End Sub

    Private Sub StartLog()
        Console.WriteLine("Logging start.")
    End Sub

    Private Sub StartHost()
        Console.WriteLine("Hosting start.")
    End Sub

    Private Sub StopLog()
        Console.WriteLine("Logging stop.")
    End Sub

    Private Sub StopHost()
        Console.WriteLine("Hosting stop.")
    End Sub
    '<Obsolete()>
    'Module Module1

    '#Region "Main"
    '    <STAThread>
    '    Sub Main(args As String())
    '        Dim lArgs As New List(Of String)
    '        lArgs.AddRange(args)
    '        If Not lArgs.Any() Then
    '            Dim application As New Windows.Application()
    '            application.Run(New ServerDownHost.MainWindow())
    '        Else
    '            For Each item As String In lArgs
    '                Select Case item.ToLower()
    '                    Case "/?", "-?", "-h"
    '                        ShowHelp()
    '                    Case "/s", "-s", "--start"
    '                        StartHost()
    '                    Case "/l", "-l", "--log"
    '                        StartLog()
    '                    Case "/nl", "-nl", "--nolog"
    '                        EndLog()
    '                    Case Else
    '                        Console.WriteLine("Invalid Argument. List of possible arguments:")
    '                        Console.ReadKey()
    '                        ShowHelp()
    '                End Select
    '            Next
    '        End If
    '    End Sub
    '#End Region

    '#Region "Functionality"
    '    Private Sub StartLog()
    '        ServerDownHost.ServerDownHostControl.GetInstance().SetLogOn()
    '    End Sub

    '    Private Sub EndLog()
    '        ServerDownHost.ServerDownHostControl.GetInstance().SetLogOff()
    '    End Sub

    '    Private Sub StartHost()
    '        ServerDownHost.ServerDownHostControl.GetInstance().InitializeHost()
    '    End Sub

    '    Private Sub ShowHelp()
    '        Throw New NotImplementedException("ShowHelp()")
    '        'Explanation of Parameters, etc.
    '    End Sub
    '#End Region

End Module
