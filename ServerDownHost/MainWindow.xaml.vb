Public Class MainWindow

#Region "Declarations"
    Private control As ServerDownHostControl = Nothing
#End Region

#Region "Initialization"
    Private Sub SetControl()
        Try
            control = ServerDownHostControl.GetInstance()
            ServerDownHostControl.AddView(Me)
            'control.InitializeWindow()
        Catch ex As Exception
            Console.WriteLine("ERROR: Couldn't set control to ServerDownHostControl.")
            Close()
        End Try
    End Sub
#End Region

#Region "Events"
    Private Sub buttonStart_Click(sender As Object, e As RoutedEventArgs) Handles buttonStart.Click
        control.InitializeHost()
    End Sub

    Private Sub buttonStop_Click(sender As Object, e As RoutedEventArgs) Handles buttonStop.Click
        control.CloseHost()
    End Sub

    Private Sub buttonLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonLog.Click
        control.SetLogState()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        SetControl()
    End Sub

    Private Sub buttonDummyLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonDummyLog.Click
        control.CreateDummyLog()
    End Sub
#End Region

End Class