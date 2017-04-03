﻿
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
                    Case "/?" Or "-?" Or "-h"
                        ShowHelp()
                    Case "/s" Or "-s" Or "--start"
                        StartHost()
                    Case "/l" Or "-l" Or "--log"
                        SetLog()
                    Case "/t" Or "-t" Or "--test"
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
        ServerDownHost.ServerDownHostControl.GetInstance().SetLogState()
    End Sub

    Private Sub StartHost()
        ServerDownHost.ServerDownHostControl.GetInstance().InitializeHost()
    End Sub

    Private Sub ShowHelp()
        Throw New NotImplementedException()
        'Explanation of Parameters, etc.
    End Sub

    Private Sub TestLog()
        ServerDownHost.ServerDownHostControl.GetInstance.SetLogState(runOnce:=True)
    End Sub
#End Region

End Module
