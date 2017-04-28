
Imports System.ServiceModel

<Obsolete()>
Module Module1

#Region "Main"
    <STAThread>
    Sub Main(args As String())
        Dim lArgs As New List(Of String)
        lArgs.AddRange(args)
        If Not lArgs.Any() Then
            Dim application As New Windows.Application()
            application.Run(New ServerDownHost.MainWindow())
        Else
            For Each item As String In lArgs
                Select Case item.ToLower()
                    Case "/?", "-?", "-h"
                        ShowHelp()
                    Case "/s", "-s", "--start"
                        StartHost()
                    Case "/l", "-l", "--log"
                        StartLog()
                    Case "/nl", "-nl", "--nolog"
                        EndLog()
                    Case Else
                        Console.WriteLine("Invalid Argument. List of possible arguments:")
                        Console.ReadKey()
                        ShowHelp()
                End Select
            Next
        End If
    End Sub
#End Region

#Region "Functionality"
    Private Sub StartLog()
        ServerDownHost.ServerDownHostControl.GetInstance().SetLogOn()
    End Sub

    Private Sub EndLog()
        ServerDownHost.ServerDownHostControl.GetInstance().SetLogOff()
    End Sub

    Private Sub StartHost()
        ServerDownHost.ServerDownHostControl.GetInstance().InitializeHost()
    End Sub

    Private Sub ShowHelp()
        Throw New NotImplementedException("ShowHelp()")
        'Explanation of Parameters, etc.
    End Sub
#End Region

End Module
