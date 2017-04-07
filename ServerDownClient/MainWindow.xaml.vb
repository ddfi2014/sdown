Class MainWindow

#Region "Declarations"
    Dim control As ServerDownClientControl
#End Region

#Region "Events"
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        control = New ServerDownClientControl(Me)
    End Sub

    Private Sub SaveLog_Click(sender As Object, e As RoutedEventArgs) Handles buttonSaveLog.Click, menuItemOptionsSave.Click
        control.Save()
    End Sub

    Private Sub SaveLogAs_Click(sender As Object, e As RoutedEventArgs) Handles menuItemOptionsSaveAs.Click
        control.SaveAs()
    End Sub

    Private Sub buttonTest_Click(sender As Object, e As RoutedEventArgs) Handles buttonTest.Click
        control.GetTextLog()
    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        control.StopTimer()
    End Sub
#End Region

End Class
