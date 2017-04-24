
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
                        SetLog()
                    Case "/t", "-t", "--test"
                        TestLog()
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
    Private Sub SetLog()
        ServerDownHost.ServerDownHostControl.GetInstance().ToggleLogState()
    End Sub

    Private Sub StartHost()
        ServerDownHost.ServerDownHostControl.GetInstance().InitializeHost()
    End Sub

    Private Sub ShowHelp()
        Throw New NotImplementedException("ShowHelp()")
        'Explanation of Parameters, etc.
    End Sub

    Private Sub TestLog()
        ServerDownHost.ServerDownHostControl.GetInstance().ToggleLogState(runOnce:=True, isConsole:=True)
    End Sub
#End Region

End Module
