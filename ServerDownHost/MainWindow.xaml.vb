Public Class MainWindow

#Region "Declarations"
    Private control As ServerDownHostControl
#End Region

#Region "Initialization"
    Private Sub SetControl()
        Try
            control = ServerDownHostControl.GetInstance()
        Catch ex As Exception
            Console.WriteLine("ERROR: Couldn't set control to ServerDownHostControl.")
            Close()
        End Try
    End Sub
#End Region

#Region "Functionality"
    Private Sub StartHost()
        control.InitializeHost()
    End Sub

    Private Sub StopHost()
        control.CloseHost()
    End Sub

    Private Sub TestHost()
        control.InitializeHost()
        control.SetLogState(runOnce:=True)
        control.CloseHost()
    End Sub
#End Region

#Region "Events"
    Private Sub buttonStart_Click(sender As Object, e As RoutedEventArgs) Handles buttonStart.Click
        StartHost()
    End Sub

    Private Sub buttonStop_Click(sender As Object, e As RoutedEventArgs) Handles buttonStop.Click
        StopHost()
    End Sub

    Private Sub buttonTest_Click(sender As Object, e As RoutedEventArgs) Handles buttonTest.Click
        TestHost()
    End Sub

    Private Sub buttonLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonLog.Click
        control.SetLogState()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        SetControl()
    End Sub
#End Region

End Class