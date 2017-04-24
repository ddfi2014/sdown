Public Class MainWindow

#Region "Declarations"
    Private control As ServerDownHostControl = Nothing
#End Region

#Region "Initialization"
    ''' <summary>
    ''' Registers the view at a control object.
    ''' </summary>
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

    <Obsolete()>
    Private Sub buttonLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonLog.Click
        'control.ToggleLogState()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        SetControl()
    End Sub

    Private Sub buttonDummyLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonDummyLog.Click
        control.CreateDummyLog()
    End Sub

    Private Sub checkBoxLog_Checked(sender As Object, e As RoutedEventArgs) Handles checkBoxLog.Checked
        control.SetLogOn()
    End Sub

    Private Sub checkBoxLog_Unchecked(sender As Object, e As RoutedEventArgs) Handles checkBoxLog.Unchecked
        control.SetLogOff()
    End Sub
#End Region

End Class